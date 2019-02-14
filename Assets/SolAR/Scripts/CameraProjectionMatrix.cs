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

        }
    }
}
