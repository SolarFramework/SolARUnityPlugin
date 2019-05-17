using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR
{
    /// Modifies the position and rotation of the camera in your scene as you move your device in the real world.
    [RequireComponent(typeof(Camera))]
    public class SolARCameraController : MonoBehaviour
    {
        [SerializeField] protected PipelineManager solARManager;
        //new Camera camera;

        protected void Awake()
        {
            Assert.IsNotNull(solARManager);
            //camera = GetComponent<Camera>();
        }

        protected void OnEnable()
        {
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
