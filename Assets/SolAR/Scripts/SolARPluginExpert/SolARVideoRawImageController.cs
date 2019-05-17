using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace SolAR
{
    /// Captures camera input and sets it as the Material on a RawImage.
    [RequireComponent(typeof(RawImage))]
    public class SolARVideoRawImageController : MonoBehaviour
    {
        [SerializeField] protected PipelineManager solARManager;

        int layoutId;
        AspectRatioFitter aspectRatioFitter;
        Material material;

        protected void Awake()
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
        }

        protected void OnEnable()
        {
            solARManager.OnFrame += OnFrame;
        }

        protected void OnDisable()
        {
            solARManager.OnFrame -= OnFrame;
        }

        void OnFrame(Texture texture)
        {
            material.mainTexture = texture;
            material.SetInt(layoutId, 2);
            if (aspectRatioFitter != null) { aspectRatioFitter.aspectRatio = (float)texture.width / texture.height; }
        }
    }
}
