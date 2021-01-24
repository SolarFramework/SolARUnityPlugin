using System;
using System.Runtime.InteropServices;
using SolAR.Datastructure;
using SolAR.Utilities;
using SolARPipelineManager;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif

namespace SolAR.Expert
{
    public class SolARPipeline : MonoBehaviour
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

        new public Camera camera;

        [HideInInspector] public bool hasCustomCanvas;

        [HideInInspector] public Canvas canvas;
        [HideInInspector] public Material material;

        Texture2D texture;
        byte[] array_imageData;

        [HideInInspector] public string m_pipelineFolder;
        [HideInInspector] public string[] m_pipelinesName;
        [HideInInspector] public string[] m_pipelinesUUID;
        [HideInInspector] public string[] m_pipelinesPath;
        [HideInInspector] public int m_selectedPipeline;
        [HideInInspector] public string m_configurationPath;
        [HideInInspector] public string m_uuid;
        [HideInInspector] public int webcamIndex;
        [HideInInspector] public XpcfRegistry conf;
        [HideInInspector] public SolARPluginPipelineManager pipelineManager;
        [HideInInspector] public WebCamTexture webcamTexture;
        [HideInInspector] public bool isUnityWebcam = false;

        IntPtr sourceTexture;

        byte[] frameBuffer;
        Color32[] colorsBuffer;
        bool isUpdateReady = false;

        #endregion

        protected void OnDestroy()
        {
            pipelineManager.stop();
            pipelineManager.Dispose();
            pipelineManager = null;
        }

        protected void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }

            Android.AndroidCloneResources(Application.streamingAssetsPath + "/SolAR/Android/android.xml");
            Android.LoadConfiguration(this);
#endif
            Init();
        }

        public void Init()
        {
            if (!camera)
            {
                Debug.Log("A camera must be specified for the SolAR Pipeline component");
                enabled = false;
                return;
            }

            pipelineManager = new SolARPluginPipelineManager();
#if UNITY_EDITOR
            // If in editor mode, the pipeline configuration file are stored in the Unity Assets folder but not in the StreaminAssets folder
            if (!pipelineManager.init(Application.dataPath + m_configurationPath, m_uuid))
            {
                Debug.LogError("Cannot init pipeline manager " + Application.dataPath + m_configurationPath + " with uuid " + m_uuid);
                enabled = false;
                return;
            }

#elif UNITY_ANDROID
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Android.ReplacePathToApp(Application.persistentDataPath + "/StreamingAssets" + m_configurationPath);
            // When the application is built, only the pipeline configuration files used by the application are moved to the an external folder on terminal
            Debug.Log("[ANDROID] Load pipeline : " + Application.persistentDataPath + "/StreamingAssets" + m_configurationPath);

            if (!pipelineManager.init(Application.persistentDataPath + "/StreamingAssets" + m_configurationPath, m_uuid))
            {
                Debug.Log("Cannot init pipeline manager " + Application.persistentDataPath + "/StreamingAssets" + m_configurationPath + " with uuid " + m_uuid);
                enabled = false;
                return;
            }
            Debug.Log("[ANDROID] Pipeline initialization successful");
            //m_Unity_Webcam = true;

#else
            // When the application is built, only the pipeline configuration files used by the application are moved to the streamingAssets folder
            if (!pipelineManager.init(Application.streamingAssetsPath + m_configurationPath, m_uuid))
            {
                Debug.Log("Cannot init pipeline manager " + Application.streamingAssetsPath + m_configurationPath + " with uuid " + m_uuid);
                enabled = false;
                return;
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
                texture.UpdateExternalTexture(nativePtr);

                //webcamTexture.GetPixels32(colorsBuffer);

                //for (int i = 0; i < colorsBuffer.Length; i++)
                //{
                //    frameBuffer[3 * i + 0] = colorsBuffer[i].b;
                //    frameBuffer[3 * i + 1] = colorsBuffer[i].g;
                //    frameBuffer[3 * i + 2] = colorsBuffer[i].r;
                //}

                //sourceTexture = Marshal.UnsafeAddrOfPinnedArrayElement(frameBuffer, 0);
                //pipelineManager.loadSourceImage(sourceTexture, width, height);
            }
            else
            {
                var intrinsic = pipelineManager.getCameraParameters().intrinsic;
                width = 640; // Screen.width; //TODO
                height = 480; // Screen.height;
                focalX = intrinsic.coeff(0, 0);
                focalY = intrinsic.coeff(1, 1);
                centerX = intrinsic.coeff(0, 2);
                centerY = intrinsic.coeff(1, 2);
            }

            SendParametersToCameraProjectionMatrix();
            array_imageData = new byte[width * height * 3];
            texture = new Texture2D(width, height, TextureFormat.RGB24, false) { filterMode = FilterMode.Trilinear };
            texture.Apply();

            if (!hasCustomCanvas)
            {
                if (canvas != null) { Destroy(canvas.gameObject); }
                var goCanvas = new GameObject("VideoSeeThroughCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(RawImage));

                canvas = goCanvas.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.pixelPerfect = true;
                canvas.worldCamera = camera;
                canvas.planeDistance = camera.farClipPlane * 0.95f;

                var scaler = goCanvas.GetComponent<CanvasScaler>();
                scaler.referenceResolution = new Vector2(width, height);
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                scaler.referencePixelsPerUnit = 1;

                var rawImage = goCanvas.GetComponent<RawImage>();
                material = new Material(Shader.Find("Custom/SolARImageShader")) { mainTexture = texture };
                rawImage.material = material;
                rawImage.uvRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                rawImage.rectTransform.sizeDelta = new Vector2(width, height);
            }
            else
            {
                var rawImage = canvas.transform.GetChild(0).GetComponent<RawImage>();
                rawImage.texture = texture;
                rawImage.material = material;
            }

            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(array_imageData, 0);
            pipelineManager.start(ptr);

            isUpdateReady = true;
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
                Debug.Log(returnCode);

                if (returnCode != PIPELINEMANAGER_RETURNCODE._NOTHING)
                {
                    texture.LoadRawTextureData(array_imageData);
                    texture.Apply();
                    material.SetTexture("_MainTex", texture);
                }

                if (returnCode == PIPELINEMANAGER_RETURNCODE._NEW_POSE || returnCode == PIPELINEMANAGER_RETURNCODE._NEW_POSE_AND_IMAGE)
                {
                    foreach (var solARObj in GameObject.FindGameObjectsWithTag("SolARObject"))
                    {
                        var renderers = solARObj.GetComponentsInChildren<Renderer>(true);
                        foreach (var r in renderers)
                        {
                            r.enabled = true;
                        }
                    }

                    var pose = pose3Df.ToUnity();
                    pose.ApplyTo(camera.transform);
                }
                else if (returnCode == PIPELINEMANAGER_RETURNCODE._NEW_IMAGE)
                {
                    foreach (var solARObj in GameObject.FindGameObjectsWithTag("SolARObject"))
                    {
                        var renderers = solARObj.GetComponentsInChildren<Renderer>(true);
                        foreach (var r in renderers)
                        {
                            r.enabled = false;
                        }
                    }
                }
            }
        }

        void SendParametersToCameraProjectionMatrix()
        {
            var cam = Camera.main;
            var zNear = cam.nearClipPlane;
            var zFar = cam.farClipPlane;
            var fovY = CameraUtility.Focal2Fov(focalY, height);

            var projectionMatrix = new Matrix4x4();
            projectionMatrix.SetRow(0, new Vector4(2 * focalX / width, 0, 1 - 2 * centerX / width, 0));
            projectionMatrix.SetRow(1, new Vector4(0, 2 * focalY / height, 2 * centerY / height - 1, 0));
            projectionMatrix.SetRow(2, new Vector4(0, 0, (zNear + zFar) / (zNear - zFar), 2 * zNear * zFar / (zNear - zFar)));
            projectionMatrix.SetRow(3, new Vector4(0, 0, -1, 0));

            cam.fieldOfView = fovY;
            cam.projectionMatrix = projectionMatrix;
        }
    }
}
