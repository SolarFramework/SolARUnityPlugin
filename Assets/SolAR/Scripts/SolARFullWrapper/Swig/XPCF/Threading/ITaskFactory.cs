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

public class ITaskFactory : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal ITaskFactory(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(ITaskFactory obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~ITaskFactory() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          xpcf_threadingPINVOKE.delete_ITaskFactory(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public virtual ITask newTask(SWIGTYPE_p_std__functionT_void_fvoidF_t command) {
    global::System.IntPtr cPtr = xpcf_threadingPINVOKE.ITaskFactory_newTask(swigCPtr, SWIGTYPE_p_std__functionT_void_fvoidF_t.getCPtr(command));
    ITask ret = (cPtr == global::System.IntPtr.Zero) ? null : new ITask(cPtr, true);
    if (xpcf_threadingPINVOKE.SWIGPendingException.Pending) throw xpcf_threadingPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}