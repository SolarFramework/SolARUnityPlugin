using System;
using System.Collections.Generic;
using SolAR.Core;
using SolAR.Datastructure;
using XPCF.Api;

public interface IPipeline : IDisposable
{
    Sizef GetMarkerSize();
    void SetCameraParameters(Matrix3x3f intrinsic, Vector5f distorsion);
    FrameworkReturnCode Proceed(Image inputImage, Transform3Df pose);
    IEnumerable<IComponentIntrospect> xpcfComponents { get; }
}
