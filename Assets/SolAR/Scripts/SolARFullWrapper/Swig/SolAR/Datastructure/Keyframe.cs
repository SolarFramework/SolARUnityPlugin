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

public class Keyframe : Frame {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal Keyframe(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_datastructurePINVOKE.Keyframe_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Keyframe obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~Keyframe() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_datastructurePINVOKE.delete_Keyframe(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public Keyframe(Frame frame) : this(solar_datastructurePINVOKE.new_Keyframe__SWIG_0(Frame.getCPtr(frame)), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Keyframe(KeypointList keypoints, DescriptorBuffer descriptors, Image view, Keyframe refKeyframe, Transform3Df pose) : this(solar_datastructurePINVOKE.new_Keyframe__SWIG_1(KeypointList.getCPtr(keypoints), DescriptorBuffer.getCPtr(descriptors), Image.getCPtr(view), Keyframe.getCPtr(refKeyframe), Transform3Df.getCPtr(pose)), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Keyframe(KeypointList keypoints, DescriptorBuffer descriptors, Image view, Keyframe refKeyframe) : this(solar_datastructurePINVOKE.new_Keyframe__SWIG_2(KeypointList.getCPtr(keypoints), DescriptorBuffer.getCPtr(descriptors), Image.getCPtr(view), Keyframe.getCPtr(refKeyframe)), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Keyframe(KeypointList keypoints, DescriptorBuffer descriptors, Image view, Transform3Df pose) : this(solar_datastructurePINVOKE.new_Keyframe__SWIG_3(KeypointList.getCPtr(keypoints), DescriptorBuffer.getCPtr(descriptors), Image.getCPtr(view), Transform3Df.getCPtr(pose)), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Keyframe(KeypointList keypoints, DescriptorBuffer descriptors, Image view) : this(solar_datastructurePINVOKE.new_Keyframe__SWIG_4(KeypointList.getCPtr(keypoints), DescriptorBuffer.getCPtr(descriptors), Image.getCPtr(view)), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void addVisibleMapPoints(MapIntCloudPoint mapPoints) {
    solar_datastructurePINVOKE.Keyframe_addVisibleMapPoints(swigCPtr, MapIntCloudPoint.getCPtr(mapPoints));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public MapIntCloudPoint getVisibleMapPoints() {
    MapIntCloudPoint ret = new MapIntCloudPoint(solar_datastructurePINVOKE.Keyframe_getVisibleMapPoints(swigCPtr), false);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public int m_idx {
    set {
      solar_datastructurePINVOKE.Keyframe_m_idx_set(swigCPtr, value);
      if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      int ret = solar_datastructurePINVOKE.Keyframe_m_idx_get(swigCPtr);
      if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

}

}