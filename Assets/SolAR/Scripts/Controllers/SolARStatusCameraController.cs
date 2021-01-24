using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR.Controllers
{
    /// Updates the cullingMask of the camera as the target is tracked or not.
    [RequireComponent(typeof(Camera))]
    public class SolARStatusCameraController : MonoBehaviour
    {
        [SerializeField] protected AbstractSolARPipeline solARManager;
        Camera arCamera;

        //public int layer;

        protected void Reset()
        {
            if (solARManager == null)
                solARManager = FindObjectsOfType<AbstractSolARPipeline>().Single();
        }

        protected void OnEnable()
        {
            Assert.IsNotNull(solARManager);
            arCamera = GetComponent<Camera>();
            Assert.IsNotNull(arCamera);

            solARManager.OnStatus += OnStatus;
        }

        protected void OnDisable()
        {
            solARManager.OnStatus -= OnStatus;
        }

        void OnStatus(bool isTracking)
        {
            arCamera.cullingMask = isTracking ? -1 : 0;
        }
    }
}
