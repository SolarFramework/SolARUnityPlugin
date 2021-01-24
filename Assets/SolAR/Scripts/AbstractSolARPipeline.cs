using System;
using SolAR.Datastructure;
using UnityEngine;

namespace SolAR
{
#pragma warning disable IDE1006 // Styles d'affectation de noms
    public abstract class AbstractSolARPipeline : MonoBehaviour, ISolARPipeline
    {
        public event Action<Sizei, Matrix3x3f, Vector5f> OnCalibrate;
        public event Action<bool> OnStatus;
        public event Action<Texture, Image.ImageLayout> OnFrame;

        protected void onCalibrate(Sizei size, Matrix3x3f intrinsic, Vector5f distortion)
            => OnCalibrate?.Invoke(size, intrinsic, distortion);

        protected void onStatus(bool isTracking)
            => OnStatus?.Invoke(isTracking);

        protected void onFrame(Texture texture, Image.ImageLayout layout)
            => OnFrame?.Invoke(texture, layout);

        public Pose Pose { get => throw new NotImplementedException(); }
    }
#pragma warning restore IDE1006 // Styles d'affectation de noms
}
