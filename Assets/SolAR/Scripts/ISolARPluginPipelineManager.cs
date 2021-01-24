using System;
using SolAR.Datastructure;

namespace SolARPipelineManager
{
#pragma warning disable IDE1006 // Styles d'affectation de noms
    public interface ISolARPluginPipelineManager : IDisposable
    {
        bool init(string conf_path, string pipelineUUID);
        CameraParameters getCameraParameters();
        bool start(IntPtr textureHandle);
        bool stop();
        PIPELINEMANAGER_RETURNCODE udpate(Transform3Df pose);
        PIPELINEMANAGER_RETURNCODE loadSourceImage(IntPtr sourceTextureHandle, int width, int height);
        void udpatePose(IntPtr pose);
    }
#pragma warning restore IDE1006 // Styles d'affectation de noms

    public partial class SolARPluginPipelineManager : ISolARPluginPipelineManager { }
}
