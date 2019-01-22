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

        public Canvas m_canvas;
        private Texture2D m_texture;
        public Material m_material;

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

        public bool m_showDebugConsole = true;

        [HideInInspector]
        public ConfXml conf;

        [HideInInspector]
        public PipelineManager m_pipelineManager;

        private delegate void eventCallbackDelegate(int eventID);
        eventCallbackDelegate m_eventCallback = null;

        [DllImport("SolARPipelineManager")]
        private static extern System.IntPtr RedirectIOToConsole(bool activate);
        /*      
               [DllImport("SolARPipelineManager")]
               private static extern System.IntPtr LogInFile([MarshalAs(UnmanagedType.LPStr)]string logFilePath, bool rewind);
          */

        void OnDisable()
        {
            StopCoroutine("CallPluginAtEndOfFrames");
            m_pipelineManager.stop();
            m_pipelineManager.Dispose();
            m_pipelineManager = null;
            if (m_showDebugConsole)
                RedirectIOToConsole(false);
        }

        // Use this for initialization
        IEnumerator Start()
        {
            if (m_showDebugConsole)
                RedirectIOToConsole(true);

            if (m_camera)
            {
                m_pipelineManager = new PipelineManager();
                m_pipelineManager.init(Application.dataPath + m_configurationPath, m_uuid);

                PipelineManager.CamParams camParams = m_pipelineManager.getCameraParameters();

                m_texture = new Texture2D(camParams.width, camParams.height, TextureFormat.RGB24, false);
                m_texture.filterMode = FilterMode.Point;
                m_texture.Apply();

                m_material.mainTexture = m_texture;
                m_canvas.transform.GetChild(0).GetComponent<RawImage>().material = m_material;
                m_canvas.transform.GetChild(0).GetComponent<RawImage>().texture = m_texture;

                // Set Camera projection matrix according to calibration parameters provided by SolAR Pipeline
                Matrix4x4 projectionMatrix = new Matrix4x4();
                float near = m_camera.nearClipPlane;
                float far = m_camera.farClipPlane;

                Vector4 row0 = new Vector4(2.0f * camParams.focalX / camParams.width, 0, 1.0f - 2.0f * camParams.centerX / camParams.width, 0);
                Vector4 row1 = new Vector4(0, 2.0f * camParams.focalY / camParams.height, 2.0f * camParams.centerY / camParams.height - 1.0f, 0);
                Vector4 row2 = new Vector4(0, 0, (far + near) / (near - far), 2.0f * far * near / (near - far));
                Vector4 row3 = new Vector4(0, 0, -1, 0);

                projectionMatrix.SetRow(0, row0);
                projectionMatrix.SetRow(1, row1);
                projectionMatrix.SetRow(2, row2);
                projectionMatrix.SetRow(3, row3);

                m_camera.fieldOfView = (Mathf.Rad2Deg * 2 * Mathf.Atan(camParams.width / (2 * camParams.focalX))) - 10;
                m_camera.projectionMatrix = projectionMatrix;

                m_eventCallback = new eventCallbackDelegate(m_pipelineManager.updateFrameDataOGL);

                m_pipelineManager.start(m_texture.GetNativeTexturePtr());
                yield return StartCoroutine("CallPluginAtEndOfFrames");
            }
            else
                Debug.Log("A camera must be specified for the SolAR Pipeline component");
        }

        private IEnumerator CallPluginAtEndOfFrames()
        {
            if (m_eventCallback != null)
            {
                while (m_pipelineManager != null)
                {
                    yield return new WaitForEndOfFrame();

                    GL.IssuePluginEvent(Marshal.GetFunctionPointerForDelegate(m_eventCallback), 1);

                    PipelineManager.Pose pose = new PipelineManager.Pose();
                    if ((m_pipelineManager.udpate(pose) & PIPELINEMANAGER_RETURNCODE._NEW_POSE) != PIPELINEMANAGER_RETURNCODE._NOTHING)
                    {
                        m_camera.cullingMask = -1;

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
                }

            }
        }


        // Update is called once per frame
        void Update()
        {

            // m_texture.Apply();
        }
    }

}
