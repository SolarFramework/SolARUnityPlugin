using System.Linq;
using SolAR.Datastructure;
using SolAR.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR.Controllers
{
    /// Captures camera input and sets it as the background of your scene.
    [RequireComponent(typeof(Camera))]
    public class SolARVideoController : MonoBehaviour
    {
        [SerializeField] protected AbstractSolARPipeline solARManager;
        new Camera camera;
        GameObject quad;
        Material material;
        int layoutId;
        public Shader shader;

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

            layoutId = Shader.PropertyToID("_Layout");

            solARManager.OnCalibrate += OnCalibrate;
            solARManager.OnFrame += OnFrame;

            quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.SetParent(transform);
            MoveVideoPlane();

            var renderer = quad.GetComponent<Renderer>();
            Assert.IsNotNull(renderer);

            if (shader == null) shader = Shader.Find("SolAR/UnlitShader");
            material = new Material(shader)
            {
                mainTextureOffset = new Vector2(0, 1),
                mainTextureScale = new Vector2(1, -1),
            };
            material.SetInt(layoutId, 2);
            renderer.sharedMaterial = material;
        }

        protected void OnDisable()
        {
            solARManager.OnCalibrate -= OnCalibrate;
            solARManager.OnFrame -= OnFrame;
            Destroy(quad);
            Destroy(material);
        }

        private void OnCalibrate(Sizei resolution, Matrix3x3f intrinsic, Vector5f distortion)
        {
            /*
            var m = new Matrix4x4();
            for (int r = 0; r < 3; ++r)
            {
                for (int c = 0; c < 3; ++c)
                {
                    m[r, c] = intrinsic.coeff(r, c);
                }
            }
            Debug.Log(m, this);
            Debug.Log(resolution.width, this);
            Debug.Log(resolution.height, this);
            */
            var fX = intrinsic.coeff(0, 0) * 2 / resolution.width;
            var fY = intrinsic.coeff(1, 1) * 2 / resolution.height;
            //var fovX = CameraUtility.Focal2Fov(fX, resolution.width);
            //var fovY = CameraUtility.Focal2Fov(fY, resolution.height);
            //camera.fieldOfView = fovY;
            //CameraUtility.ApplyProjectionMatrix()

            var projectionMatrix = Perspective(fX, fY, camera.nearClipPlane, camera.farClipPlane);
            CameraUtility.ApplyProjectionMatrix(camera, projectionMatrix);

            MoveVideoPlane();
        }

        public static Matrix4x4 Perspective(float fX, float fY, float zNear, float zFar)
        {
            var projectionMatrix = new Matrix4x4();
            projectionMatrix[3, 2] = -1;
            projectionMatrix[0, 0] = fX;
            projectionMatrix[1, 1] = fY;
            Matrix4x4Utility.SetClipping(ref projectionMatrix, zNear, zFar);
            return projectionMatrix;
        }

        void MoveVideoPlane()
        {
            var z = (camera.nearClipPlane + camera.farClipPlane) / 2;
            quad.transform.SetPositionAndRotation(new Vector3(0, 0, z), Quaternion.identity);
            float aspect = (float)camera.pixelWidth / camera.pixelHeight;
            quad.transform.localScale = z * Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * 2 * new Vector3(aspect, 1, 1);
        }

        void OnFrame(Texture texture, Image.ImageLayout layout)
        {
            material.mainTexture = texture;
            material.SetInt(layoutId, (int)layout);
        }
    }
}
