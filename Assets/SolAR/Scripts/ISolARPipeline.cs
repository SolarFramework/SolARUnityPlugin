using System;
using SolAR.Datastructure;
using UnityEngine;

namespace SolAR
{
    public interface ISolARPipeline
    {
        event Action<Sizei, Matrix3x3f, Vector5f> OnCalibrate;
        event Action<bool> OnStatus;
        event Action<Texture, Image.ImageLayout> OnFrame;
        Pose Pose { get; }
    }
}
