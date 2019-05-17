using System;
using SolAR.Datastructure;
using SolAR.Utilities;
using UnityEngine;

namespace SolAR
{
    public static class ObsoleteExtensions
    {
        // Set Camera projection matrix according to calibration parameters provided by SolAR Pipeline
        [Obsolete]
        public static void CalibCamera(Camera camera)
        {
            Matrix4x4 projectionMatrix = new Matrix4x4();
            float near = camera.nearClipPlane;
            float far = camera.farClipPlane;

            var width = 0f;
            var height = 0f;
            var focalX = 0f;
            var focalY = 0f;
            var centerX = 0f;
            var centerY = 0f;

            var row0 = new Vector4(2 * focalX / width, 0, 1 - 2 * centerX / width, 0);
            var row1 = new Vector4(0, 2 * focalY / height, 2 * centerY / height - 1, 0);
            var row2 = new Vector4(0, 0, (far + near) / (near - far), 2 * far * near / (near - far));
            var row3 = new Vector4(0, 0, -1, 0);

            projectionMatrix.SetRow(0, row0);
            projectionMatrix.SetRow(1, row1);
            projectionMatrix.SetRow(2, row2);
            projectionMatrix.SetRow(3, row3);

            camera.fieldOfView = CameraUtility.Focal2Fov(focalX, width);
            camera.projectionMatrix = projectionMatrix;
        }

        static readonly Matrix4x4 invertMatrix;
        static ObsoleteExtensions()
        {
            invertMatrix = new Matrix4x4();
            invertMatrix.SetRow(0, new Vector4(+1, +0, +0, +0));
            invertMatrix.SetRow(1, new Vector4(+0, -1, +0, +0));
            invertMatrix.SetRow(2, new Vector4(+0, +0, +1, +0));
            invertMatrix.SetRow(3, new Vector4(+0, +0, +0, +1));
        }

        static Pose CallPluginAtEndOfFrames(Transform3Df pose)
        {
            Matrix4x4 cameraPoseFromSolAR = Matrix4x4.identity;

            var pos = pose.translation();
            var rot = pose.rotation();
            //cameraPoseFromSolAR.SetRow(0, new Vector4(rot.coeff(0, 0), rot.coeff(0, 1), rot.coeff(0, 2), pos.coeff(0, 0)));
            //cameraPoseFromSolAR.SetRow(1, new Vector4(rot.coeff(1, 0), rot.coeff(1, 1), rot.coeff(1, 2), pos.coeff(0, 1)));
            //cameraPoseFromSolAR.SetRow(2, new Vector4(rot.coeff(2, 0), rot.coeff(2, 1), rot.coeff(2, 2), pos.coeff(0, 2)));
            //cameraPoseFromSolAR.SetRow(3, new Vector4(0, 0, 0, 1));
            for (int r = 0; r < 3; ++r)
            {
                for (int c = 0; c < 3; ++c)
                {
                    cameraPoseFromSolAR[r, c] = rot.coeff(r, c);
                }
                cameraPoseFromSolAR[r, 3] = pos.coeff(0, r);
            }

            Matrix4x4 unityCameraPose = invertMatrix * cameraPoseFromSolAR;

            Vector3 forward = new Vector3(unityCameraPose.m02, unityCameraPose.m12, unityCameraPose.m22);
            Vector3 up = new Vector3(unityCameraPose.m01, unityCameraPose.m11, unityCameraPose.m21);

            var q = Quaternion.LookRotation(forward, -up);
            var p = new Vector3(unityCameraPose.m03, unityCameraPose.m13, unityCameraPose.m23);
            return new Pose(p, q);
        }
    }
}
