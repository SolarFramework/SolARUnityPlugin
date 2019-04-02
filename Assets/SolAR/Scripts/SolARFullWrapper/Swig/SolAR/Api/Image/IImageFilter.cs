//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Image {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class IImageFilter : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal IImageFilter(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_imagePINVOKE.IImageFilter_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(IImageFilter obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~IImageFilter() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_imagePINVOKE.delete_IImageFilter(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual FrameworkReturnCode filter(Image input, Image output) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_imagePINVOKE.IImageFilter_filter(swigCPtr, Image.getCPtr(input), Image.getCPtr(output));
    if (solar_api_imagePINVOKE.SWIGPendingException.Pending) throw solar_api_imagePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
