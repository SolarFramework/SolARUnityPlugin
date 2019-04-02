//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Solver.Pose {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class I2Dto3DTransformDecomposer : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal I2Dto3DTransformDecomposer(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_solver_posePINVOKE.I2Dto3DTransformDecomposer_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(I2Dto3DTransformDecomposer obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~I2Dto3DTransformDecomposer() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_solver_posePINVOKE.delete_I2Dto3DTransformDecomposer(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual void setCameraParameters(Matrix3x3f intrinsicParams, Vector5f distorsionParams) {
    solar_api_solver_posePINVOKE.I2Dto3DTransformDecomposer_setCameraParameters(swigCPtr, Matrix3x3f.getCPtr(intrinsicParams), Vector5f.getCPtr(distorsionParams));
    if (solar_api_solver_posePINVOKE.SWIGPendingException.Pending) throw solar_api_solver_posePINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual bool decompose(Transform2Df F, Transform3DfList decomposedPoses) {
    bool ret = solar_api_solver_posePINVOKE.I2Dto3DTransformDecomposer_decompose(swigCPtr, Transform2Df.getCPtr(F), Transform3DfList.getCPtr(decomposedPoses));
    if (solar_api_solver_posePINVOKE.SWIGPendingException.Pending) throw solar_api_solver_posePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
