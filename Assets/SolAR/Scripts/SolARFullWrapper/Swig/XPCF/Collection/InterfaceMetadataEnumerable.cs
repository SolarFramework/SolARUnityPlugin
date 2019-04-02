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

public class InterfaceMetadataEnumerable : global::System.IDisposable, IEnumerable<InterfaceMetadata> {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal InterfaceMetadataEnumerable(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(InterfaceMetadataEnumerable obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~InterfaceMetadataEnumerable() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          xpcf_collectionPINVOKE.delete_InterfaceMetadataEnumerable(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public virtual InterfaceMetadataEnumerator getEnumerator() {
    global::System.IntPtr cPtr = xpcf_collectionPINVOKE.InterfaceMetadataEnumerable_getEnumerator__SWIG_0(swigCPtr);
    InterfaceMetadataEnumerator ret = (cPtr == global::System.IntPtr.Zero) ? null : new InterfaceMetadataEnumerator(cPtr, true);
    if (xpcf_collectionPINVOKE.SWIGPendingException.Pending) throw xpcf_collectionPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual InterfaceMetadataEnumerator getEnumerator(uint offset, uint chunkSize) {
    global::System.IntPtr cPtr = xpcf_collectionPINVOKE.InterfaceMetadataEnumerable_getEnumerator__SWIG_1(swigCPtr, offset, chunkSize);
    InterfaceMetadataEnumerator ret = (cPtr == global::System.IntPtr.Zero) ? null : new InterfaceMetadataEnumerator(cPtr, true);
    if (xpcf_collectionPINVOKE.SWIGPendingException.Pending) throw xpcf_collectionPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual uint size() {
    uint ret = xpcf_collectionPINVOKE.InterfaceMetadataEnumerable_size(swigCPtr);
    if (xpcf_collectionPINVOKE.SWIGPendingException.Pending) throw xpcf_collectionPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

		public IEnumerator<InterfaceMetadata> GetEnumerator() { return getEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return getEnumerator(); }
	
}

}