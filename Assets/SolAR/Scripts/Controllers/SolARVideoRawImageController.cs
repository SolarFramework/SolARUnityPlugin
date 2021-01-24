using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace SolAR.Controllers
{
    /// Captures camera input and sets it as the Material on a RawImage.
    [RequireComponent(typeof(RawImage))]
    public class SolARVideoRawImageController : MonoBehaviour
    {
        [SerializeField] protected AbstractSolARPipeline solARManager;

        int layoutId;
        AspectRatioFitter aspectRatioFitter;
        Material material;

        protected void Reset()
        {
            if (solARManager == null)
                solARManager = FindObjectsOfType<AbstractSolARPipeline>().Single();
        }

        protected void OnEnable()
        {
            Assert.IsNotNull(solARManager);

            layoutId = Shader.PropertyToID("_Layout");

            aspectRatioFitter = GetComponent<AspectRatioFitter>();// ?? gameObject.AddComponent<AspectRatioFitter>();

            var shader = Shader.Find("SolAR/ImageEffectShader");
            Assert.IsNotNull(shader);
            material = new Material(shader)
            {
                mainTextureOffset = new Vector2(0, 1),
                mainTextureScale = new Vector2(1, -1),
            };
            var rawImage = GetComponent<RawImage>();
            rawImage.material = material;
            rawImage.uvRect = new Rect(0, 1, 1, -1);

            solARManager.OnFrame += OnFrame;
        }

        protected void OnDisable()
        {
            solARManager.OnFrame -= OnFrame;
        }

        void OnFrame(Texture texture, Datastructure.Image.ImageLayout layout)
        {
            material.mainTexture = texture;
            material.SetInt(layoutId, 2);
            if (aspectRatioFitter != null) { aspectRatioFitter.aspectRatio = (float)texture.width / texture.height; }
        }
    }
}
