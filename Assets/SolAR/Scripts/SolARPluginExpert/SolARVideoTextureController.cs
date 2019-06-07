using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR
{
    /// Captures camera input and sets it as the main texture on a game object.
    public class SolARVideoTextureController : MonoBehaviour
    {
        [SerializeField] protected PipelineManager solARManager;

        public Material[] materials;
        public string property = "_MainTex";

        Material material;
        int layoutId;
        int propertyId;
        public Shader m_shader;

        RenderTexture rTex;

        protected void Awake()
        {
            Assert.IsNotNull(solARManager);
            //GetComponent<Renderer>().material;

            Assert.IsNotNull(m_shader);
            material = new Material(m_shader)
            {
                mainTextureOffset = new Vector2(0, 1),
                mainTextureScale = new Vector2(1, -1),
            };

            layoutId = Shader.PropertyToID("_Layout");
            propertyId = Shader.PropertyToID(property);
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
