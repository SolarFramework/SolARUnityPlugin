using System.Linq;
using SolAR.Datastructure;
using SolAR.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR.Controllers
{
    /// Captures camera input and sets it as the background of your scene.
    [RequireComponent(typeof(Camera))]
    public class SolARCalibrateController : MonoBehaviour
    {
        [SerializeField] protected AbstractSolARPipeline solARManager = null;
        [SerializeField] protected ScaleMode scaleMode = ScaleMode.ScaleAndCrop;
        protected new Camera camera;

        protected void Reset()
        {
            if (solARManager == null)
                solARManager = FindObjectsOfType<AbstractSolARPipeline>().Single();
        }

        protected void OnEnable()
        {
            Assert.IsNotNull(solARManager);
            camera = GetComponent<Camera>();
            Assert.IsNotNull(camera);
            Assert.IsFalse(camera.orthographic);

            solARManager.OnCalibrate += OnCalibrate;
        }

        protected void OnDisable()
        {
            solARManager.OnCalibrate -= OnCalibrate;
        }

        private void OnCalibrate(Sizei resolution, Matrix3x3f intrinsic, Vector5f distortion)
        {
            var fY = intrinsic.coeff(1, 1);
            var fovY = CameraUtility.Focal2Fov(fY, resolution.height);
            var fX = intrinsic.coeff(0, 0);
            var aspect = (fY / resolution.height) / (fX / resolution.width);
            var projectionMatrix = Matrix4x4.Perspective(fovY, aspect, camera.nearClipPlane, camera.farClipPlane);
            CameraUtility.ApplyProjectionMatrix(camera, projectionMatrix);
        }
    }
}
