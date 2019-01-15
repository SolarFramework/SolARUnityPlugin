using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Linq;
using System.IO;

namespace SolAR
{

    public class SolARPipeline : MonoBehaviour
    {
        public Camera m_camera;
        private Canvas m_canvas;
        public Texture2D m_texture;
        private Material m_material;

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

        public bool m_showDebugConsole = true;

        [HideInInspector]
        public ConfXml conf;

        [HideInInspector]
        public PipelineManager m_pipelineManager = new PipelineManager();

        [DllImport("SolARPipelineManager")]
        private static extern System.IntPtr RedirectIOToConsole(bool activate);
 /*      
        [DllImport("SolARPipelineManager")]
        private static extern System.IntPtr LogInFile([MarshalAs(UnmanagedType.LPStr)]string logFilePath, bool rewind);
   */       


        void OnEnable()
        {
            if (m_showDebugConsole)
                RedirectIOToConsole(true);
 //           else
 //               LogInFile("F:\\Dev\\SolAR\\sources\\Plugins\\Unity\\SolARPipelineManager.log", true);

            if (m_camera)
            {
                m_pipelineManager = new PipelineManager();
                m_pipelineManager.init(m_configurationPath, m_uuid);
            
                PipelineManager.CamParams camParams = m_pipelineManager.getCameraParameters();

                GameObject goCanvas = new GameObject("VideoSeeThroughCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(RawImage));
                goCanvas.transform.SetParent(m_camera.transform);

                //m_texture = new Texture2D(camParams.width, camParams.height, TextureFormat.RGB24, false);
                //m_texture.filterMode = FilterMode.Point;
                //m_texture.Apply();

                m_canvas = goCanvas.GetComponent<Canvas>();
                m_canvas.worldCamera = m_camera;

                CanvasScaler scaler = goCanvas.GetComponent<CanvasScaler>();
                scaler.referenceResolution = new Vector2(camParams.width, camParams.height);
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                scaler.referencePixelsPerUnit = 1;

                m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
                m_canvas.pixelPerfect = true;
                m_canvas.worldCamera = Camera.main;
                m_canvas.planeDistance = Camera.main.farClipPlane - 100;

                RawImage image = goCanvas.GetComponent<RawImage>();
                image.texture = m_texture;
                m_material = new Material(Shader.Find("Unlit/Texture"));
                image.material = m_material;
                image.rectTransform.sizeDelta = new Vector2(camParams.width, camParams.height);

                m_camera.fieldOfView = Mathf.Rad2Deg * 2 * Mathf.Atan(camParams.width / (2 * camParams.focalX)); 
            }
            else
                Debug.Log("A canvas must be specified for the SolAR Pipeline component");
        }

        void OnDisable()
        {
            m_pipelineManager.stop();
            m_pipelineManager.Dispose();
            if (m_showDebugConsole)
                RedirectIOToConsole(false);
        }

        // Use this for initialization
        void Start()
        {
            if (m_texture != null)
                m_pipelineManager.start(m_texture.GetNativeTexturePtr());
        }

        // Update is called once per frame
        void Update()
        {
            PipelineManager.Pose pose = new PipelineManager.Pose();
            if (m_pipelineManager.udpate(pose))
                Debug.Log("Translation = (" + pose.translation(0) + ", " + pose.translation(1) + ", " + pose.translation(2) + ")");
            m_texture.Apply();
        }
    }

}
