using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolAR
{
    public class CameraProjectionMatrix : MonoBehaviour
    {
        [HideInInspector]
        public float focalX;

        [HideInInspector]
        public float focalY;

        [HideInInspector]
        public int width;

        [HideInInspector]
        public int height;

        [HideInInspector]
        public float centerX;

        [HideInInspector]
        public float centerY;

        void Update()
        {
            Debug.Log(focalX + "   " + focalY + "   " + centerX + "    "  + centerY + "   " +width + "   " +height);
            Matrix4x4 projectionMatrix = new Matrix4x4();
            float near = Camera.main.nearClipPlane;
            float far = Camera.main.farClipPlane;

            Vector4 row0 = new Vector4(2.0f * focalX / width, 0, 1.0f - 2.0f * centerX / width, 0);
            Vector4 row1 = new Vector4(0, 2.0f * focalY / height, 2.0f * centerY / height - 1.0f, 0);
            Vector4 row2 = new Vector4(0, 0, (far + near) / (near - far), 2.0f * far * near / (near - far));
            Vector4 row3 = new Vector4(0, 0, -1, 0);

            projectionMatrix.SetRow(0, row0);
            projectionMatrix.SetRow(1, row1);
            projectionMatrix.SetRow(2, row2);
            projectionMatrix.SetRow(3, row3);

            Camera.main.fieldOfView = (Mathf.Rad2Deg * 2 * Mathf.Atan(width / (2 * focalX))) - 10;
            Camera.main.projectionMatrix = projectionMatrix;
        }
    }
}
