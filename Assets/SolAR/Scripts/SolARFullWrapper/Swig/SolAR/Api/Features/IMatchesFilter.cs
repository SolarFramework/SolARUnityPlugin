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

public class IMatchesFilter : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal IMatchesFilter(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_featuresPINVOKE.IMatchesFilter_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(IMatchesFilter obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~IMatchesFilter() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_featuresPINVOKE.delete_IMatchesFilter(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual void filter(DescriptorMatchVector inputMatches, DescriptorMatchVector outputMatches, KeypointList keyPoints_1, KeypointList keyPoints_2) {
    solar_api_featuresPINVOKE.IMatchesFilter_filter(swigCPtr, DescriptorMatchVector.getCPtr(inputMatches), DescriptorMatchVector.getCPtr(outputMatches), KeypointList.getCPtr(keyPoints_1), KeypointList.getCPtr(keyPoints_2));
    if (solar_api_featuresPINVOKE.SWIGPendingException.Pending) throw solar_api_featuresPINVOKE.SWIGPendingException.Retrieve();
  }

}

}