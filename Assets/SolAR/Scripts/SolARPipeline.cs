using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SolAR
{
    public class SolARPipeline : MonoBehaviour
    {

        #region Variables
//#####################################################
        [HideInInspector]
        public float focalX;

        [HideInInspector]
        public float focalY;

        [HideInInspector]
        public int width;

        [HideInInspector]
        public int height;

        [HideInInspector]
        public float centerX;

        [HideInInspector]
        public float centerY;
//#####################################################
        public Camera m_camera;
        public Button m_EventButton;
        [HideInInspector]
        public bool m_CustomCanvas = false;

        [HideInInspector]
        public Canvas m_canvas;
        [HideInInspector]
        public Material m_material;

        private Texture2D m_texture;
        private byte[] array_imageData;

        [HideInInspector]
        public string m_pipelineFolder;

        [HideInInspector]
        public string[] m_pipelinesName;

        [HideInInspector]
        public string[] m_pipelinesUUID;

        [HideInInspector]
        public string[] m_pipelinesPath;

        [HideInInspector]
        public int m_selectedPipeline;

        [HideInInspector]
        public string m_configurationPath;

        [HideInInspector]
        public string m_uuid;

        [HideInInspector]
        public int m_webCamNum;

        [HideInInspector]
        public ConfXml conf;

        [HideInInspector]
        public PipelineManager m_pipelineManager;

        [DllImport("SolARPipelineManager")]
        private static extern System.IntPtr RedirectIOToConsole(bool activate);

        [HideInInspector]
        public WebCamTexture m_webCamTexture;

        [HideInInspector]
        public bool m_Unity_Webcam = false;

        private IntPtr sourceTexture;
        private UnityAction m_myAction;

        #endregion

        void OnDestroy()
        {
            m_pipelineManager.stop();
            m_pipelineManager.Dispose();
            m_pipelineManager = null;
        }

        void Start()
        {
            if (m_camera)
            {
                m_pipelineManager = new PipelineManager();
                m_pipelineManager.init(Application.dataPath + m_configurationPath, m_uuid);

                if (m_Unity_Webcam)
                {
                    m_webCamTexture = new WebCamTexture(WebCamTexture.devices[m_webCamNum].name, width, height);
                    m_webCamTexture.Play();

                    Color32[] data = new Color32[width * height];
                    byte[] m_vidframe_byte = new byte[width * height * 3];

                    m_webCamTexture.GetPixels32(data);

                    for (int i = 0; i < data.Length; i++)
                    {
                        m_vidframe_byte[3 * i] = data[i].b;
                        m_vidframe_byte[3 * i + 1] = data[i].g;
                        m_vidframe_byte[3 * i + 2] = data[i].r;
                    }
                    m_texture = new Texture2D(width, height, TextureFormat.RGB24, false);
                    
                    sourceTexture = Marshal.UnsafeAddrOfPinnedArrayElement(m_vidframe_byte, 0);
                    m_pipelineManager.loadSourceImage(sourceTexture, width, height);
                }
                else
                {
                    PipelineManager.CamParams camParams = m_pipelineManager.getCameraParameters();
                    m_texture = new Texture2D(camParams.width, camParams.height, TextureFormat.RGB24, false);
                    width = camParams.width;
                    height = camParams.height;
                    focalX = camParams.focalX;
                    focalY = camParams.focalY;
                    centerX = camParams.centerX;
                    centerY = camParams.centerY;
                }

                SendParamtersToCameraProjectionMatrix();
                array_imageData = new byte[width * height * 3];
                m_texture.filterMode = FilterMode.Point;
                m_texture.Apply();

                if (!m_CustomCanvas)
                {
                    GameObject goCanvas = new GameObject("VideoSeeThroughCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(RawImage));

                    m_canvas = goCanvas.GetComponent<Canvas>();
                    m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    m_canvas.pixelPerfect = true;
                    m_canvas.worldCamera = m_camera;
                    m_canvas.planeDistance = m_camera.farClipPlane * 0.95f;

                    CanvasScaler scaler = goCanvas.GetComponent<CanvasScaler>();
                    scaler.referenceResolution = new Vector2(width, height);
                    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                    scaler.referencePixelsPerUnit = 1;

                    RawImage image = goCanvas.GetComponent<RawImage>();
                    m_material = new Material(Shader.Find("Custom/SolARImageShader"));
                    m_material.mainTexture = m_texture;
                    image.material = m_material;
                    image.uvRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                    image.rectTransform.sizeDelta = new Vector2(width, height);
                }
                else
                {
                    RawImage img = m_canvas.transform.GetChild(0).GetComponent<RawImage>();
                    img.texture = m_texture;
                    img.material = m_material;
                }               

                IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(array_imageData, 0);
                m_pipelineManager.start(ptr);  //IntPtr
            }
            else
                Debug.Log("A camera must be specified for the SolAR Pipeline component");

            m_myAction += MyEvent;
            m_EventButton.onClick.AddListener(m_myAction);
        }

        void Update()
        {
            if (m_pipelineManager != null)
            {
                if (m_Unity_Webcam)
                {
                    Color32[] data = new Color32[width * height];
                    byte[] m_vidframe_byte = new byte[width * height * 3];

                    m_webCamTexture.GetPixels32(data);

                    for (int i = 0; i < data.Length; i++)
                    {
                        m_vidframe_byte[3 * i] = data[i].b;
                        m_vidframe_byte[3 * i + 1] = data[i].g;
                        m_vidframe_byte[3 * i + 2] = data[i].r;
                    }

                    sourceTexture = Marshal.UnsafeAddrOfPinnedArrayElement(m_vidframe_byte, 0);
                    m_pipelineManager.loadSourceImage(sourceTexture, width, height);
                }
                PipelineManager.Pose pose = new PipelineManager.Pose();
                if ((m_pipelineManager.udpate(pose) & PIPELINEMANAGER_RETURNCODE._NEW_POSE) != PIPELINEMANAGER_RETURNCODE._NOTHING)
                {
                    GameObject.Find("AR_Cube").GetComponent<Renderer>().enabled = true;
                    Matrix4x4 cameraPoseFromSolAR = new Matrix4x4();
                    cameraPoseFromSolAR.SetRow(0, new Vector4(pose.rotation(0, 0), pose.rotation(0, 1), pose.rotation(0, 2), pose.translation(0)));
                    cameraPoseFromSolAR.SetRow(1, new Vector4(pose.rotation(1, 0), pose.rotation(1, 1), pose.rotation(1, 2), pose.translation(1)));
                    cameraPoseFromSolAR.SetRow(2, new Vector4(pose.rotation(2, 0), pose.rotation(2, 1), pose.rotation(2, 2), pose.translation(2)));
                    cameraPoseFromSolAR.SetRow(3, new Vector4(0, 0, 0, 1));

                    Matrix4x4 invertMatrix = new Matrix4x4();
                    invertMatrix.SetRow(0, new Vector4(1, 0, 0, 0));
                    invertMatrix.SetRow(1, new Vector4(0, -1, 0, 0));
                    invertMatrix.SetRow(2, new Vector4(0, 0, 1, 0));
                    invertMatrix.SetRow(3, new Vector4(0, 0, 0, 1));
                    Matrix4x4 unityCameraPose = invertMatrix * cameraPoseFromSolAR;

                    Vector3 forward = new Vector3(unityCameraPose.m02, unityCameraPose.m12, unityCameraPose.m22);
                    Vector3 up = new Vector3(unityCameraPose.m01, unityCameraPose.m11, unityCameraPose.m21);

                    m_camera.transform.rotation = Quaternion.LookRotation(forward, -up);
                    m_camera.transform.position = new Vector3(unityCameraPose.m03, unityCameraPose.m13, unityCameraPose.m23);
                }
                else GameObject.Find("AR_Cube").GetComponent<Renderer>().enabled = false;

            }
            m_texture.LoadRawTextureData(array_imageData);
            m_texture.Apply();
            m_material.SetTexture("_MainTex", m_texture);
        }

        void MyEvent()
        {
            Debug.Log(Input.mousePosition.x + "  " + Input.mousePosition.y);
        }

        void SendParamtersToCameraProjectionMatrix()
        {
            m_camera.GetComponent<CameraProjectionMatrix>().focalX = focalX;
            m_camera.GetComponent<CameraProjectionMatrix>().focalY = focalY;
            m_camera.GetComponent<CameraProjectionMatrix>().centerX = centerX;
            m_camera.GetComponent<CameraProjectionMatrix>().centerY = centerY;
            m_camera.GetComponent<CameraProjectionMatrix>().width = width;
            m_camera.GetComponent<CameraProjectionMatrix>().height = height;

        }
    }

}
