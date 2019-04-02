//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace XPCF.Collection {

	using System.Collections;
	using System.Collections.Generic;
	using XPCF.Core;
	using XPCF.Api;
	using XPCF.Properties;

public class PropertyEnumerator : global::System.IDisposable, IEnumerator<IProperty> {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnBase;

  internal PropertyEnumerator(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwnBase = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(PropertyEnumerator obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~PropertyEnumerator() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnBase) {
          swigCMemOwnBase = false;
          xpcf_collectionPINVOKE.delete_PropertyEnumerator(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public virtual bool MoveNext() {
    bool ret = xpcf_collectionPINVOKE.PropertyEnumerator_MoveNext(swigCPtr);
    if (xpcf_collectionPINVOKE.SWIGPendingException.Pending) throw xpcf_collectionPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual void Reset() {
    xpcf_collectionPINVOKE.PropertyEnumerator_Reset(swigCPtr);
    if (xpcf_collectionPINVOKE.SWIGPendingException.Pending) throw xpcf_collectionPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual IProperty current() {
    global::System.IntPtr cPtr = xpcf_collectionPINVOKE.PropertyEnumerator_current(swigCPtr);
    IProperty ret = (cPtr == global::System.IntPtr.Zero) ? null : new IProperty(cPtr, true);
    if (xpcf_collectionPINVOKE.SWIGPendingException.Pending) throw xpcf_collectionPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual bool endReached() {
    bool ret = xpcf_collectionPINVOKE.PropertyEnumerator_endReached(swigCPtr);
    if (xpcf_collectionPINVOKE.SWIGPendingException.Pending) throw xpcf_collectionPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

		public IProperty Current { get { return current(); } }
		object IEnumerator.Current { get { return current(); } }
	
}

}
