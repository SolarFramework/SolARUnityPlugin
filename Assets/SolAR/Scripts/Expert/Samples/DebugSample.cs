using SolAR.Api.Input.Devices;
using SolAR.Core;
using SolAR.Datastructure;
using XPCF.Api;

namespace SolAR.Expert.Samples
{
    public class DebugSample : AbstractSample
    {
        public DebugSample(IComponentManager xpcfComponentManager) : base(xpcfComponentManager)
        {
        }

        public override FrameworkReturnCode Proceed(Image inputImage, Transform3Df pose, ICamera camera)
        {
            return FrameworkReturnCode._ERROR_;
        }

        public override void SetCameraParameters(Matrix3x3f intrinsic, Vector5f distortion)
        {
        }
    }
}
