using UnityEngine;

namespace SolAR.Utilities
{
    public static class CameraUtility
    {
        /// <summary>
        /// Gets the focal lenght corresponding to the given field of view.
        /// </summary>
        /// <returns>The focal length.</returns>
        /// <param name="fov">Field of view, in degree.</param>
        /// <param name="size">Image size.</param>
        public static float Fov2Focal(float fov, float size = 2f)
        {
            const float FOV_SCALE = Mathf.Deg2Rad / 2f;
            return size / 2f / Mathf.Tan(fov * FOV_SCALE);
        }

        /// <summary>
        /// Gets the fiels of view corresponding to the given focal length.
        /// </summary>
        /// <returns>The field of view, in degree.</returns>
        /// <param name="focal">Focal length.</param>
        /// <param name="size">Image size.</param>
        public static float Focal2Fov(float focal, float size = 2f)
        {
            const float FOV_UNSCALE = Mathf.Rad2Deg * 2f;
            return Mathf.Atan(size / 2f / focal) * FOV_UNSCALE;
        }

        public static void ApplyProjectionMatrix(Camera camera, Matrix4x4 projectionMatrix)
        {
            Matrix4x4Utility.GetProjectionParameters(projectionMatrix, out float fov, out float aspect, out float zNear, out float zFar);
            camera.fieldOfView = fov;
            //camera.aspect = aspect;
            camera.nearClipPlane = zNear;
            camera.farClipPlane = zFar;
            camera.ResetProjectionMatrix();
        }
    }
}
