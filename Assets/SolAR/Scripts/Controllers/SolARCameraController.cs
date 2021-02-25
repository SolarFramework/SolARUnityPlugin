using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR.Controllers
{
    /// Modifies the position and rotation of the camera in your scene as you move your device in the real world.
    [RequireComponent(typeof(Camera))]
    public class SolARCameraController : MonoBehaviour
    {
        [SerializeField] protected AbstractSolARPipeline solARManager;

        protected void Reset()
        {
            if (solARManager == null)
                solARManager = FindObjectsOfType<AbstractSolARPipeline>().Single();
        }

        protected void OnEnable()
        {
            Assert.IsNotNull(solARManager);

            solARManager.OnStatus += OnStatus;
        }

        protected void OnDisable()
        {
            solARManager.OnStatus -= OnStatus;
        }

        void OnStatus(bool isTracking)
        {
            if (!isTracking) return;
            //camera.cullingMask = isTracking ? -1 : 0;
            var pose = solARManager.Pose;
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
    }
}
