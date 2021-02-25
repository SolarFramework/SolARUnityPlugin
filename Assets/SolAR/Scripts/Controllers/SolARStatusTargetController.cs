using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR.Controllers
{
    /// Updates the layer of the target as it is tracked or not.
    public class SolARStatusTargetController : MonoBehaviour
    {
        [SerializeField] protected AbstractSolARPipeline solARManager;

        public int visibleLayer;
        public int hiddenLayer;

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
            gameObject.layer = isTracking ? visibleLayer : hiddenLayer;
        }
    }
}
