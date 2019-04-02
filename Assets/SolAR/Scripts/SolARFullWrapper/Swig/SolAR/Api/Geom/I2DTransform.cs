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

public class I2DTransform : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal I2DTransform(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_geomPINVOKE.I2DTransform_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(I2DTransform obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~I2DTransform() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_geomPINVOKE.delete_I2DTransform(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual FrameworkReturnCode transform(Point2DfList inputPoints, Transform2Df transformation, Point2DfList outputPoints) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_geomPINVOKE.I2DTransform_transform(swigCPtr, Point2DfList.getCPtr(inputPoints), Transform2Df.getCPtr(transformation), Point2DfList.getCPtr(outputPoints));
    if (solar_api_geomPINVOKE.SWIGPendingException.Pending) throw solar_api_geomPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
