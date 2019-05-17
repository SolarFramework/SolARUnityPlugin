using UnityEngine;

namespace SolAR.Utilities
{
    public static class TransformUtility
    {
        public static void SetLocalToWorldMatrix(Transform transform, Matrix4x4 localToWorldMatrix)
        {
            transform.position = Matrix4x4Utility.GetPosition(localToWorldMatrix);
            transform.rotation = Matrix4x4Utility.GetRotation(localToWorldMatrix);
            transform.localScale = Vector3.one;
        }

        public static void SetLocalToParentMatrix(Transform transform, Matrix4x4 localToParentMatrix)
        {
            var pos = Matrix4x4Utility.GetPosition(localToParentMatrix);
            var rot = Matrix4x4Utility.GetRotation(localToParentMatrix);
            var scale = new Vector3();

            var right = Matrix4x4Utility.GetRight(localToParentMatrix);
            scale.x = Vector3.Dot(rot * Vector3.right, right);

            var up = Matrix4x4Utility.GetUp(localToParentMatrix);
            scale.y = Vector3.Dot(rot * Vector3.up, up);

            var forward = Matrix4x4Utility.GetForward(localToParentMatrix);
            scale.z = Vector3.Dot(rot * Vector3.forward, forward);

            transform.localPosition = pos;
            transform.localRotation = rot;
            transform.localScale = scale;
        }

        public static void Move(Transform transform, Matrix4x4 motion, Space relativeTo = Space.World)
        {
            var localToWorldMatrix = transform.localToWorldMatrix;
            switch (relativeTo)
            {
                case Space.World:
                    localToWorldMatrix = motion * localToWorldMatrix;
                    break;
                case Space.Self:
                    localToWorldMatrix = localToWorldMatrix * motion;
                    break;
            }
            SetLocalToWorldMatrix(transform, localToWorldMatrix);
        }
    }
}
