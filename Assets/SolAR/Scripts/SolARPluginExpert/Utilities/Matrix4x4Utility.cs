using UnityEngine;

namespace SolAR.Utilities
{
    public static class Matrix4x4Utility
    {
        public static Vector3 GetRight(this Matrix4x4 m) { return m.GetColumn(0); }
        public static Vector3 GetUp(this Matrix4x4 m) { return m.GetColumn(1); }
        public static Vector3 GetForward(this Matrix4x4 m) { return m.GetColumn(2); }

        public static Vector3 GetPosition(this Matrix4x4 m) { return m.GetColumn(3); }
        public static Quaternion GetRotation(this Matrix4x4 m) { return Quaternion.LookRotation(GetForward(m), GetUp(m)); }
        public static Vector3 GetScale(this Matrix4x4 m) { return new Vector3(GetRight(m).magnitude, GetUp(m).magnitude, GetForward(m).magnitude); }

        public static Matrix4x4 Perspective(float fov, float aspect, float zNear, float zFar)
        {
            //return Matrix4x4.Perspective(fov, aspect, zNear, zFar);
            var projectionMatrix = new Matrix4x4();
            projectionMatrix[3, 2] = -1;
            SetFieldOfView(ref projectionMatrix, fov, aspect);
            SetClipping(ref projectionMatrix, zNear, zFar);
            return projectionMatrix;
        }

        public static void SetFieldOfView(ref Matrix4x4 projectionMatrix, float fov, float aspect)
        {
            float focal = CameraUtility.Fov2Focal(fov);
            projectionMatrix[0, 0] = focal / aspect;
            projectionMatrix[1, 1] = focal;
        }

        public static void SetClipping(ref Matrix4x4 projectionMatrix, float zNear, float zFar)
        {
            projectionMatrix[2, 2] = -(zFar + zNear) / (zFar - zNear);
            projectionMatrix[2, 3] = -2 * zNear * zFar / (zFar - zNear);
        }

        public static void GetProjectionParameters(Matrix4x4 projectionMatrix, out float fov, out float aspect, out float zNear, out float zFar)
        {
            GetFieldOfView(projectionMatrix, out fov, out aspect);
            GetClipping(projectionMatrix, out zNear, out zFar);
        }

        public static void GetFieldOfView(Matrix4x4 projectionMatrix, out float fov, out float aspect)
        {
            float focal = projectionMatrix[1, 1];
            fov = CameraUtility.Focal2Fov(focal);
            aspect = focal / projectionMatrix[0, 0];
        }

        public static void GetClipping(Matrix4x4 projectionMatrix, out float zNear, out float zFar)
        {
            float x = projectionMatrix[2, 2];
            float y = projectionMatrix[2, 3];
            zNear = y / (x - 1);
            zFar = y / (x + 1);
        }
    }
}
