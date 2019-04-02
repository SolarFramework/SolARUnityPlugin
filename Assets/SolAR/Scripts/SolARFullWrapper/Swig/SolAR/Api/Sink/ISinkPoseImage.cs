//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Sink {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class ISinkPoseImage : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal ISinkPoseImage(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_sinkPINVOKE.ISinkPoseImage_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(ISinkPoseImage obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~ISinkPoseImage() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_sinkPINVOKE.delete_ISinkPoseImage(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual void set(Transform3Df pose, Image image) {
    solar_api_sinkPINVOKE.ISinkPoseImage_set__SWIG_0(swigCPtr, Transform3Df.getCPtr(pose), Image.getCPtr(image));
    if (solar_api_sinkPINVOKE.SWIGPendingException.Pending) throw solar_api_sinkPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void set(Image image) {
    solar_api_sinkPINVOKE.ISinkPoseImage_set__SWIG_1(swigCPtr, Image.getCPtr(image));
    if (solar_api_sinkPINVOKE.SWIGPendingException.Pending) throw solar_api_sinkPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual FrameworkReturnCode setImageBuffer(global::System.IntPtr imageBufferPointer) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_sinkPINVOKE.ISinkPoseImage_setImageBuffer(swigCPtr, imageBufferPointer);
    if (solar_api_sinkPINVOKE.SWIGPendingException.Pending) throw solar_api_sinkPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual SinkReturnCode get(Transform3Df pose) {
    SinkReturnCode ret = (SinkReturnCode)solar_api_sinkPINVOKE.ISinkPoseImage_get(swigCPtr, Transform3Df.getCPtr(pose));
    if (solar_api_sinkPINVOKE.SWIGPendingException.Pending) throw solar_api_sinkPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual SinkReturnCode tryGet(Transform3Df pose) {
    SinkReturnCode ret = (SinkReturnCode)solar_api_sinkPINVOKE.ISinkPoseImage_tryGet(swigCPtr, Transform3Df.getCPtr(pose));
    if (solar_api_sinkPINVOKE.SWIGPendingException.Pending) throw solar_api_sinkPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}