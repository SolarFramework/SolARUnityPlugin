using System;
using UnityEngine;

namespace SolAR.Utilities
{
    public struct Pose3d
    {
        public static implicit operator Pose(Pose3d pose3d) { return pose3d.pose; }

        public static implicit operator Pose3d(Pose pose) { return new Pose3d(pose); }
        public static implicit operator Pose3d(GameObject gameObject) { return gameObject.transform; }
        public static implicit operator Pose3d(Component component) { return component.transform; }
        public static implicit operator Pose3d(Transform transform) { return new Pose3d(transform.position, transform.rotation); }
        public static implicit operator Pose3d(Matrix4x4 matrix) { return PoseUtility.From(matrix); }

        Pose pose;

        Pose3d(Vector3 position, Quaternion rotation) : this(new Pose(position, rotation)) { }
        Pose3d(Pose pose) { this.pose = pose; }

        public Vector3 position
        {
            get { return pose.position; }
            set { pose.position = value; }
        }

        public Quaternion rotation
        {
            get { return pose.rotation; }
            set { pose.rotation = value; }
        }
    }

    public static class PoseUtility
    {
        public static Pose From(Vector3 position, Quaternion rotation) { return new Pose(position, rotation); }
        public static Pose From(Vector3 position) { return From(position, Quaternion.identity); }
        public static Pose From(Quaternion rotation) { return From(Vector3.zero, rotation); }

        public static Pose From(Pose3d pose) { return From(pose.position, pose.rotation); }

        public static Pose From(Matrix4x4 matrix) { return From(matrix.GetPosition(), matrix.GetRotation()); }

        public static Pose WithPosition(this Pose pose, Vector3 position) { return From(position, pose.rotation); }
        public static Pose WithRotation(this Pose pose, Quaternion rotation) { return From(pose.position, rotation); }

        public static void ApplyTo(this Pose pose, GameObject gameObject) { pose.ApplyTo(gameObject.transform); }
        public static void ApplyTo(this Pose pose, Component component) { pose.ApplyTo(component.transform); }
        public static void ApplyTo(this Pose pose, Transform transform) { transform.SetPositionAndRotation(pose.position, pose.rotation); }

        public static void ApplyToLocal(this Pose pose, GameObject gameObject) { pose.ApplyToLocal(gameObject.transform); }
        public static void ApplyToLocal(this Pose pose, Component component) { pose.ApplyToLocal(component.transform); }
        public static void ApplyToLocal(this Pose pose, Transform transform)
        {
            transform.localPosition = pose.position;
            transform.localRotation = pose.rotation;
        }

        public static Pose Inverse(this Pose pose)
        {
            var rot = Quaternion.Inverse(pose.rotation);
            var pos = rot * -pose.position;
            return From(pos, rot);
        }

        public static Matrix4x4 GetLocalToWorldMatrix(this Pose pose) { return Matrix4x4.TRS(pose.position, pose.rotation, Vector3.one); }
        public static Matrix4x4 GetWorldToLocalMatrix(this Pose pose) { return pose.Inverse().GetLocalToWorldMatrix(); }
        [Obsolete("Use GetLocalToWorldMatrix")]
        public static Matrix4x4 GetMatrix(this Pose pose) { return pose.GetLocalToWorldMatrix(); }

        //public static Pose Append(this Pose pose, Pose3d append) { return From(pose).Append(append); }
        public static Pose Append(this Pose pose, Pose3d append) { return From(pose.position + pose.rotation * append.position, pose.rotation * append.rotation); }
        public static Pose Prepend(this Pose pose, Pose3d prepend) { return Append(prepend, pose); }

        public static Pose Move(this Pose pose, Matrix4x4 motion) { return From(motion * pose.GetLocalToWorldMatrix()); }

        public static Pose LocalToWorld(this Pose pose, Pose3d relativeTo) { return pose.Prepend(relativeTo); }
        public static Pose WorldToLocal(this Pose pose, Pose3d relativeTo) { return pose.Prepend(Inverse(relativeTo)); }

        public static Pose Rotate(this Pose pose, Vector3 eulerAngle, Space relativeTo = Space.Self) { return pose.Rotate(Quaternion.Euler(eulerAngle), relativeTo); }
        public static Pose Rotate(this Pose pose, Vector3 axis, float angle, Space relativeTo = Space.Self) { return pose.Rotate(Quaternion.AngleAxis(angle, axis), relativeTo); }
        public static Pose Rotate(this Pose pose, Quaternion q, Space relativeTo = Space.Self)
        {
            q = relativeTo == Space.World ? q * pose.rotation : pose.rotation * q;
            return pose.WithRotation(q);
        }

        public static Pose RotateAround(this Pose pose, Vector3 point, Vector3 axis, float angle) { return pose.RotateAround(point, Quaternion.AngleAxis(angle, axis)); }
        public static Pose RotateAround(this Pose pose, Vector3 point, Quaternion q)
        {
            //return pose.Prepend(From(point - q * point, q));
            return From(point + q * (pose.position - point), q * pose.rotation);
        }

        public static Pose Translate(this Pose pose, Vector3 translation, Space relativeTo = Space.Self)
        {
            if (relativeTo != Space.World)
            {
                translation = pose.rotation * translation;
            }
            return pose.WithPosition(translation + pose.position);
        }
        public static Pose Translate(this Pose pose, Vector3 translation, Pose relativeTo)
        {
            return pose.Translate(relativeTo.rotation * translation, Space.World);
        }

        public static Pose Lerp(Pose a, Pose b, float t)
        {
            var pos = Vector3.Lerp(a.position, b.position, t);
            var rot = Quaternion.Slerp(a.rotation, b.rotation, t);
            return From(pos, rot);
        }

        public static Vector3 InverseTransformDirection(this Pose pose, Vector3 direction) { return Quaternion.Inverse(pose.rotation) * direction; }
        public static Vector3 InverseTransformPoint(this Pose pose, Vector3 position) { return pose.Inverse().TransformPoint(position); }
        [Obsolete("No scale to apply")]
        public static Vector3 InverseTransformVector(this Pose pose, Vector3 vector) { return pose.InverseTransformDirection(vector); }

        public static Vector3 TransformDirection(this Pose pose, Vector3 direction) { return pose.rotation * direction; }
        public static Vector3 TransformPoint(this Pose pose, Vector3 position) { return pose.position + pose.rotation * position; }
        [Obsolete("No scale to apply")]
        public static Vector3 TransformVector(this Pose pose, Vector3 vector) { return pose.TransformDirection(vector); }

        public static Pose LookAt(this Pose pose, Vector3 position) { return pose.LookAt(position, Vector3.up); }
        public static Pose LookAt(this Pose pose, Vector3 position, Vector3 upwards) { return pose.WithForward(position - pose.position); }

        public static Pose WithForward(this Pose pose, Vector3 forward) { return pose.WithRotation(Quaternion.LookRotation(forward)); }
        public static Pose WithForward(this Pose pose, Vector3 forward, Vector3 upwards) { return pose.WithRotation(Quaternion.LookRotation(forward, upwards)); }
        public static Pose WithUpwards(this Pose pose, Vector3 upwards) { return pose.WithRotation(Quaternion.LookRotation(pose.forward, upwards)); }

        public static Pose Debug(this Pose pose, object label = null)
        {
            UnityEngine.Debug.LogFormat("{0}: {1}", label, pose);
            return pose;
        }
    }
}
