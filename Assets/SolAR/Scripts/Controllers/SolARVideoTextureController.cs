using System.Linq;
using SolAR.Datastructure;
using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR.Controllers
{
    /// Captures camera input and sets it as the main texture on a game object.
    public class SolARVideoTextureController : MonoBehaviour
    {
        [SerializeField] protected AbstractSolARPipeline solARManager;

        public Material[] materials;
        public string property = "_MainTex";

        Material material;
        int layoutId;
        int propertyId;
        public Shader shader;

        RenderTexture rTex;

        protected void Reset()
        {
            if (solARManager == null)
                solARManager = FindObjectsOfType<AbstractSolARPipeline>().Single();
        }

        protected void OnEnable()
        {
            Assert.IsNotNull(solARManager);

            Assert.IsNotNull(shader);
            material = new Material(shader)
            {
                mainTextureOffset = new Vector2(0, 1),
                mainTextureScale = new Vector2(1, -1),
            };

            layoutId = Shader.PropertyToID("_Layout");
            propertyId = Shader.PropertyToID(property);

            solARManager.OnFrame += OnFrame;
        }

        protected void OnDisable()
        {
            solARManager.OnFrame -= OnFrame;
        }

        void OnFrame(Texture texture, Image.ImageLayout layout)
        {
            var w = texture.width;
            var h = texture.height;
            if (rTex != null && (rTex.width != w || rTex.height != h))
            {
                Destroy(rTex);
                rTex = null;
            }
            if (rTex == null)
            {
                rTex = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
                foreach (var m in materials)
                {
                    m.SetTexture(propertyId, rTex);
                }
            }
            //material.mainTexture = texture;
            material.SetInt(layoutId, 2);
            Graphics.Blit(texture, rTex, material);
        }
    }
}
