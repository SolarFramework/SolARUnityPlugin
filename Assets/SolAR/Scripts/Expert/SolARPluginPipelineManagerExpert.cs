using System;
using SolAR.Api.Pipeline;
using SolAR.Api.Sink;
using SolAR.Api.Source;
using SolAR.Core;
using SolAR.Datastructure;
using SolARPipelineManager;
using UnityEngine;
using XPCF.Api;

namespace SolAR.Expert
{
    public class SolARPluginPipelineManagerExpert : ISolARPluginPipelineManager
    {
        IPoseEstimationPipeline m_pipeline;

        public SolARPluginPipelineManagerExpert()
        {
            //LOG_INFO("Pipeline Manager Constructor");
        }

        public void Dispose()
        {
            xpcf_api.getComponentManagerInstance().clear();
        }

        public bool init(string conf_path, string pipelineUUID)
        {
            //LOG_INFO("Start PipelineManager.init")
            //LOG_FLUSH
            var xpcfComponentManager = xpcf_api.getComponentManagerInstance();
            bool load_ok = false;
            //LOG_INFO("conf_path : {}", conf_path.c_str())
            try
            {
                if (xpcfComponentManager.load(conf_path) == XPCF.Core.XPCFErrorCode._SUCCESS)
                    load_ok = true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
                return false;
            }

            if (!load_ok)
                return false;

            m_pipeline = xpcfComponentManager.createComponent(new XPCF.Core.UUID(pipelineUUID)).BindTo<IPoseEstimationPipeline>();
            //LOG_INFO("Pipeline Component has been created")


            if (m_pipeline == null)
                return false;

            return m_pipeline.init(xpcfComponentManager) == FrameworkReturnCode._SUCCESS;
        }

        public CameraParameters getCameraParameters()
        {
            return m_pipeline.getCameraParameters();
        }

        public PIPELINEMANAGER_RETURNCODE loadSourceImage(IntPtr sourceTextureHandle, int width, int height)
        {
            if (m_pipeline == null)
                return PIPELINEMANAGER_RETURNCODE._ERROR;

            SourceReturnCode returnCode = m_pipeline.loadSourceImage(sourceTextureHandle, width, height);
            if (returnCode == SourceReturnCode._ERROR)
                return PIPELINEMANAGER_RETURNCODE._ERROR;

            return PIPELINEMANAGER_RETURNCODE._NEW_IMAGE;
        }

        public bool start(IntPtr textureHandle)
        {
            if (m_pipeline == null)
                return false;

            return (m_pipeline.start(textureHandle) == FrameworkReturnCode._SUCCESS);
        }

        public PIPELINEMANAGER_RETURNCODE udpate(Transform3Df pose)
        {
            if (m_pipeline == null)
                return PIPELINEMANAGER_RETURNCODE._ERROR;

            SinkReturnCode returnCode = m_pipeline.update(pose);
            if (returnCode == SinkReturnCode._ERROR)
                return PIPELINEMANAGER_RETURNCODE._ERROR;
            if (returnCode == SinkReturnCode._NEW_POSE)
                return PIPELINEMANAGER_RETURNCODE._NEW_POSE;
            if (returnCode == SinkReturnCode._NEW_POSE_AND_IMAGE)
                return PIPELINEMANAGER_RETURNCODE._NEW_POSE_AND_IMAGE;
            if (returnCode == SinkReturnCode._NEW_IMAGE)
                return PIPELINEMANAGER_RETURNCODE._NEW_IMAGE;

            return PIPELINEMANAGER_RETURNCODE._NOTHING;
        }

        public void udpatePose(IntPtr pose)
        {
            if (m_pipeline == null)
                return;

            Transform3Df solarPose = new Transform3Df();
            SinkReturnCode returnCode = m_pipeline.update(solarPose);
            if (returnCode == SinkReturnCode._ERROR)
                return;

            if ((returnCode & SinkReturnCode._NEW_POSE) != SinkReturnCode._NOTHING)
            {
                //std.cout << "  new pose \n";
                //float* tmp2 = solarPose.matrix().data();
                //float* tmp1 = (float*)pose;
                //for (int i = 0; i < 16; i++)
                //    tmp1[i] = tmp2[i];
                //TODO: Copy solarPose dans pose

                return;
            }

            // std.cout <<" no new pose \n";
            // return false if the pose has not been updated
            // TODO : return a more explicit returnCode to make the difference beteen "Error" and "Pose not updated"
            return;
        }

        public bool stop()
        {
            if (m_pipeline != null)
            {
                return (m_pipeline.stop() == FrameworkReturnCode._SUCCESS);
            }
            return true;
        }
    }
}
