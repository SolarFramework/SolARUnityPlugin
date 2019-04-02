//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Datastructure {

    using XPCF.Core;
    using SolAR.Core;

public class Keypoint : Point2Df {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal Keypoint(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_datastructurePINVOKE.Keypoint_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Keypoint obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~Keypoint() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_datastructurePINVOKE.delete_Keypoint(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public Keypoint() : this(solar_datastructurePINVOKE.new_Keypoint__SWIG_0(), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Keypoint(float x, float y, float size, float angle, float response, int octave, int class_id) : this(solar_datastructurePINVOKE.new_Keypoint__SWIG_1(x, y, size, angle, response, octave, class_id), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void init(float x, float y, float size, float angle, float response, int octave, int class_id) {
    solar_datastructurePINVOKE.Keypoint_init(swigCPtr, x, y, size, angle, response, octave, class_id);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public float getAngle() {
    float ret = solar_datastructurePINVOKE.Keypoint_getAngle(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public float getSize() {
    float ret = solar_datastructurePINVOKE.Keypoint_getSize(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public float getResponse() {
    float ret = solar_datastructurePINVOKE.Keypoint_getResponse(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public int getOctave() {
    int ret = solar_datastructurePINVOKE.Keypoint_getOctave(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public int getClassId() {
    int ret = solar_datastructurePINVOKE.Keypoint_getClassId(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
