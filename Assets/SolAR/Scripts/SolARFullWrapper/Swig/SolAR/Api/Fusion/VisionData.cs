//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Fusion {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class VisionData : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal VisionData(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(VisionData obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~VisionData() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          solar_api_fusionPINVOKE.delete_VisionData(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public Transform3Df pose {
    set {
      solar_api_fusionPINVOKE.VisionData_pose_set(swigCPtr, Transform3Df.getCPtr(value));
      if (solar_api_fusionPINVOKE.SWIGPendingException.Pending) throw solar_api_fusionPINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      Transform3Df ret = new Transform3Df(solar_api_fusionPINVOKE.VisionData_pose_get(swigCPtr), true);
      if (solar_api_fusionPINVOKE.SWIGPendingException.Pending) throw solar_api_fusionPINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public bool isPoseValid {
    set {
      solar_api_fusionPINVOKE.VisionData_isPoseValid_set(swigCPtr, value);
      if (solar_api_fusionPINVOKE.SWIGPendingException.Pending) throw solar_api_fusionPINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      bool ret = solar_api_fusionPINVOKE.VisionData_isPoseValid_get(swigCPtr);
      if (solar_api_fusionPINVOKE.SWIGPendingException.Pending) throw solar_api_fusionPINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public VisionData() : this(solar_api_fusionPINVOKE.new_VisionData(), true) {
    if (solar_api_fusionPINVOKE.SWIGPendingException.Pending) throw solar_api_fusionPINVOKE.SWIGPendingException.Retrieve();
  }

}

}
