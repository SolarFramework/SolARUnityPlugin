//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace XPCF.Core {


public class UUID : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal UUID(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(UUID obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~UUID() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          xpcf_corePINVOKE.delete_UUID(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

		public static implicit operator UUID(string uuidString) {return new UUID(uuidString);}
		public static implicit operator string(UUID uuid) {return uuid.ToString();}
	
  public UUID(string uuidString) : this(xpcf_corePINVOKE.new_UUID(uuidString), true) {
    if (xpcf_corePINVOKE.SWIGPendingException.Pending) throw xpcf_corePINVOKE.SWIGPendingException.Retrieve();
  }

  public override string ToString() {
    string ret = xpcf_corePINVOKE.UUID_ToString(swigCPtr);
    if (xpcf_corePINVOKE.SWIGPendingException.Pending) throw xpcf_corePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
