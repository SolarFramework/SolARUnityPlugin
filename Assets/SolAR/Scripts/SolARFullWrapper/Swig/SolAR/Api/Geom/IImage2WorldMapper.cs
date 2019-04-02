//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Geom {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class IImage2WorldMapper : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal IImage2WorldMapper(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_geomPINVOKE.IImage2WorldMapper_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(IImage2WorldMapper obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~IImage2WorldMapper() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_geomPINVOKE.delete_IImage2WorldMapper(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual FrameworkReturnCode map(Point2DfList digitalPoints, Point3DfList worldPoints) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_geomPINVOKE.IImage2WorldMapper_map(swigCPtr, Point2DfList.getCPtr(digitalPoints), Point3DfList.getCPtr(worldPoints));
    if (solar_api_geomPINVOKE.SWIGPendingException.Pending) throw solar_api_geomPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
