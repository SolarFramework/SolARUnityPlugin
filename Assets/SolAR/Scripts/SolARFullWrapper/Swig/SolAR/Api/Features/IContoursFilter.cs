//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Features {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class IContoursFilter : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal IContoursFilter(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_featuresPINVOKE.IContoursFilter_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(IContoursFilter obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~IContoursFilter() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_featuresPINVOKE.delete_IContoursFilter(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual FrameworkReturnCode filter(Contour2DfList inContours, Contour2DfList outContours) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_featuresPINVOKE.IContoursFilter_filter(swigCPtr, Contour2DfList.getCPtr(inContours), Contour2DfList.getCPtr(outContours));
    if (solar_api_featuresPINVOKE.SWIGPendingException.Pending) throw solar_api_featuresPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}