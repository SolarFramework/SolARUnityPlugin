//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Solver.Map {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class IMapFilter : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal IMapFilter(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_solver_mapPINVOKE.IMapFilter_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(IMapFilter obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~IMapFilter() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_solver_mapPINVOKE.delete_IMapFilter(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual void filter(Transform3Df pose1, Transform3Df pose2, CloudPointList input, CloudPointList output) {
    solar_api_solver_mapPINVOKE.IMapFilter_filter(swigCPtr, Transform3Df.getCPtr(pose1), Transform3Df.getCPtr(pose2), CloudPointList.getCPtr(input), CloudPointList.getCPtr(output));
    if (solar_api_solver_mapPINVOKE.SWIGPendingException.Pending) throw solar_api_solver_mapPINVOKE.SWIGPendingException.Retrieve();
  }

}

}