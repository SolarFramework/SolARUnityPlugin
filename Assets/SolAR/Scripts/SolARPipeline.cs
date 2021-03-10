using System;
using System.Runtime.InteropServices;
using SolAR.Datastructure;
using SolAR.Pipeline;
using SolAR.Utilities;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID && !UNITY_EDITOR
using System.Threading.Tasks;
using UnityEngine.Android;
#endif

using System.Threading;

namespace SolAR
{
    public class SolARPipeline : AbstractSolARPipeline
    {
#region Variables

        //#####################################################
        [HideInInspector] public float focalX;
        [HideInInspector] public float focalY;
        [HideInInspector] public int width;
        [HideInInspector] public int height;
        [HideInInspector] public float centerX;
        [HideInInspector] public float centerY;
        //#####################################################

        public Camera arCamera;

        [HideInInspector] public bool hasCustomCanvas;

        [HideInInspector] public Canvas canvas;
        [HideInInspector] public Material material;

        Texture2D texture;
        byte[] array_imageData;

        [HideInInspector] public string m_pipelineFolder;
        [HideInInspector] public string[] m_pipelinesName;
        [HideInInspector] public string[] m_pipelinesPath;
        [HideInInspector] public int m_selectedPipeline;
        [HideInInspector] public string m_configurationPath;
        [HideInInspector] public int webcamIndex;
        [HideInInspector] public XpcfRegistry conf;
        [HideInInspector] public ISolARPluginPipelineManager pipelineManager;
        [HideInInspector] public WebCamTexture webcamTexture;
        [HideInInspector] public bool isUnityWebcam = false;

        IntPtr sourceTexture;

        byte[] frameBuffer;
        Color32[] colorsBuffer;
        bool isUpdateReady = false;

#endregion

        protected void Reset()
        {
            if (arCamera == null)
                arCamera = Camera.main;
        }

        protected void OnDestroy()
        {
            pipelineManager.stop();
            pipelineManager.Dispose();
            pipelineManager = null;
        }


#if UNITY_ANDROID && !UNITY_EDITOR
        protected async void Start()
#else
        protected void Start()
#endif
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }

            // TODO(jmhenaff): investigate cleaner solution
            // 'Await' is used otherwise first run of app fails because resource are not yet deployed when pipeline starts.
            // But async/await are "contagious", Start() must be async (is this an issue?), plus async void is an anti-pattern.
            await Android.AndroidCloneResources(Application.streamingAssetsPath + "/SolAR/Android/android.xml");
            Android.LoadConfiguration(this);
#endif
            enabled = Init();

            Thread.Sleep(2000);
        }

        public bool Init()
        {
            if (!arCamera)
            {
                Debug.Log("A camera must be specified for the SolAR Pipeline component");
                return false;
            }

            pipelineManager = new SolARPluginPipelineManager();
#if UNITY_EDITOR
            // If in editor mode, the pipeline configuration file are stored in the Unity Assets folder but not in the StreaminAssets folder
            if (!pipelineManager.init(Application.dataPath + m_configurationPath))
            {
                Debug.LogErrorFormat("Cannot init pipeline manager {0}{1}", Application.dataPath, m_configurationPath);
                return false;
            }

#elif UNITY_ANDROID
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Android.ReplacePathToApp(Application.persistentDataPath + "/StreamingAssets" + m_configurationPath);
            // When the application is built, only the pipeline configuration files used by the application are moved to the an external folder on terminal
            Debug.Log("[ANDROID] Load pipeline : " + Application.persistentDataPath + "/StreamingAssets" + m_configurationPath);

            if (!pipelineManager.init(Application.persistentDataPath + "/StreamingAssets" + m_configurationPath))
            {
                Debug.Log("Cannot init pipeline manager " + Application.persistentDataPath + "/StreamingAssets" + m_configurationPath);
                return false;
            }
            Debug.Log("[ANDROID] Pipeline initialization successful");
            //m_Unity_Webcam = true;
            //isUnityWebcam = true;

#else
            // When the application is built, only the pipeline configuration files used by the application are moved to the streamingAssets folder
            if (!pipelineManager.init(Application.streamingAssetsPath + m_configurationPath))
            {
                Debug.Log("Cannot init pipeline manager " + Application.streamingAssetsPath + m_configurationPath);
                return false;
            }
            //m_Unity_Webcam = true;
#endif

            if (isUnityWebcam)
            {
                webcamTexture = new WebCamTexture(WebCamTexture.devices[webcamIndex].name, width, height);
                webcamTexture.Play();
                width = webcamTexture.width;
                height = webcamTexture.height;

                colorsBuffer = new Color32[width * height];
                frameBuffer = new byte[width * height * 3];

                var nativePtr = webcamTexture.GetNativeTexturePtr();
                texture = new Texture2D(0, 0);
                texture.UpdateExternalTexture(nativePtr);

                webcamTexture.GetPixels32(colorsBuffer);

                for (int i = 0; i < colorsBuffer.Length; i++)
                {
                    frameBuffer[3 * i + 0] = colorsBuffer[i].b;
                    frameBuffer[3 * i + 1] = colorsBuffer[i].g;
                    frameBuffer[3 * i + 2] = colorsBuffer[i].r;
                }

                sourceTexture = Marshal.UnsafeAddrOfPinnedArrayElement(frameBuffer, 0);
                pipelineManager.loadSourceImage(sourceTexture, width, height);

                Debug.Log("[JMH] Unity camera texture size ( w: " + width + ", height: " + height + ")");
            }
            else
            {
                var calibration = pipelineManager.getCameraParameters();
                var resolution = calibration.resolution;
                width = (int)resolution.width;
                height = (int)resolution.height;
                var intrinsic = calibration.intrinsic;
                focalX = intrinsic.coeff(0, 0);
                focalY = intrinsic.coeff(1, 1);
                centerX = intrinsic.coeff(0, 2);
                centerY = intrinsic.coeff(1, 2);

                onCalibrate(resolution, intrinsic, calibration.distortion);
            }

            SendParametersToCameraProjectionMatrix();
            array_imageData = new byte[width * height * 3];
            texture = new Texture2D(width, height, TextureFormat.RGB24, false) { filterMode = FilterMode.Trilinear };
            texture.Apply();

            if (!hasCustomCanvas)
            {
                if (canvas != null) { Destroy(canvas.gameObject); }

                var canvasGO = new GameObject("SolARVideoCanvas", typeof(Canvas));

                canvas = canvasGO.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.pixelPerfect = true;
                canvas.worldCamera = arCamera;
                canvas.planeDistance = arCamera.farClipPlane * 0.95f;
                canvas.sortingOrder = -1;

                var imageGO = new GameObject("SolARVideoBackground", typeof(RawImage), typeof(AspectRatioFitter));
                imageGO.transform.SetParent(canvasGO.transform, false);

                var rawImage = imageGO.GetComponent<RawImage>();
                material = new Material(Shader.Find("Custom/SolARImageShader")) { mainTexture = texture };
                rawImage.material = material;
                //rawImage.uvRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                //rawImage.rectTransform.sizeDelta = new Vector2(width, height);

                var fitter = imageGO.GetComponent<AspectRatioFitter>();
                fitter.aspectRatio = (float) width / height;
                fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            }
            else
            {
                var rawImage = canvas.GetComponentInChildren<RawImage>();
                rawImage.texture = texture;
                rawImage.material = material;
            }

            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(array_imageData, 0);
            pipelineManager.start(ptr);

            isUpdateReady = true;
            return true;
        }

        protected void Update()
        {
            if (isUpdateReady && pipelineManager != null)
            {
                if (isUnityWebcam)
                {
                    //Graphics.ConvertTexture();
                    //Graphics.CopyTexture();
                    webcamTexture.GetPixels32(colorsBuffer);

                    for (int i = 0; i < colorsBuffer.Length; i++)
                    {
                        frameBuffer[3 * i + 0] = colorsBuffer[i].b;
                        frameBuffer[3 * i + 1] = colorsBuffer[i].g;
                        frameBuffer[3 * i + 2] = colorsBuffer[i].r;
                    }

                    sourceTexture = Marshal.UnsafeAddrOfPinnedArrayElement(frameBuffer, 0);
                    pipelineManager.loadSourceImage(sourceTexture, width, height);
                }
                var pose3Df = new Transform3Df();

                var returnCode = pipelineManager.udpate(pose3Df);

                if (returnCode != PIPELINEMANAGER_RETURNCODE._NOTHING)
                {
                    texture.LoadRawTextureData(array_imageData);
                    texture.Apply();
                    material.SetTexture("_MainTex", texture);
                }

                if (returnCode == PIPELINEMANAGER_RETURNCODE._NEW_POSE || returnCode == PIPELINEMANAGER_RETURNCODE._NEW_POSE_AND_IMAGE)
                {
                    //foreach (var solARObj in GameObject.FindGameObjectsWithTag("SolARObject"))
                    //{
                    //    var renderers = solARObj.GetComponentsInChildren<Renderer>(true);
                    //    foreach (var r in renderers)
                    //    {
                    //        r.enabled = true;
                    //    }
                    //}

                    Pose = pose3Df.ToUnity();
                    //pose.ApplyTo(arCamera.transform);
                    onStatus(true);
                    onPose(Pose);
                }
                else if (returnCode == PIPELINEMANAGER_RETURNCODE._NEW_IMAGE)
                {
                    //foreach (var solARObj in GameObject.FindGameObjectsWithTag("SolARObject"))
                    //{
                    //    var renderers = solARObj.GetComponentsInChildren<Renderer>(true);
                    //    foreach (var r in renderers)
                    //    {
                    //        r.enabled = false;
                    //    }
                    //}

                    Pose = Pose.identity;
                    onStatus(false);
                    onPose(null);
                }
            }
        }

        void SendParametersToCameraProjectionMatrix()
        {
            var cam = Camera.main;
            float near = cam.nearClipPlane;
            float far = cam.farClipPlane;

            var projectionMatrix = new Matrix4x4();
            projectionMatrix.SetRow(0, new Vector4(2 * focalX / width, 0, 1 - 2 * centerX / width, 0));
            projectionMatrix.SetRow(1, new Vector4(0, 2 * focalY / height, 2 * centerY / height - 1, 0));
            projectionMatrix.SetRow(2, new Vector4(0, 0, (near + far) / (near - far), 2 * near * far / (near - far)));
            projectionMatrix.SetRow(3, new Vector4(0, 0, -1, 0));

            cam.fieldOfView = CameraUtility.Focal2Fov(focalY, height);
            CameraUtility.ApplyProjectionMatrix(cam, projectionMatrix);
        }
    }
}
