//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace XPCF.Threading {

    using XPCF.Core;

public class FiberTask : AbstractDelegateTask {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal FiberTask(global::System.IntPtr cPtr, bool cMemoryOwn) : base(xpcf_threadingPINVOKE.FiberTask_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FiberTask obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~FiberTask() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          xpcf_threadingPINVOKE.delete_FiberTask(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public FiberTask(SWIGTYPE_p_std__functionT_void_fvoidF_t processingFunction) : this(xpcf_threadingPINVOKE.new_FiberTask(SWIGTYPE_p_std__functionT_void_fvoidF_t.getCPtr(processingFunction)), true) {
    if (xpcf_threadingPINVOKE.SWIGPendingException.Pending) throw xpcf_threadingPINVOKE.SWIGPendingException.Retrieve();
  }

  public static void yield() {
    xpcf_threadingPINVOKE.FiberTask_yield();
    if (xpcf_threadingPINVOKE.SWIGPendingException.Pending) throw xpcf_threadingPINVOKE.SWIGPendingException.Retrieve();
  }

}

}
