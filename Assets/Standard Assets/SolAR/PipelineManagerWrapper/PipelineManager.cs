//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR {

public class PipelineManager : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal PipelineManager(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(PipelineManager obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~PipelineManager() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          SolARPipelineManagerPINVOKE.delete_PipelineManager(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public class Pose : global::System.IDisposable {
    private global::System.Runtime.InteropServices.HandleRef swigCPtr;
    protected bool swigCMemOwn;
  
    internal Pose(global::System.IntPtr cPtr, bool cMemoryOwn) {
      swigCMemOwn = cMemoryOwn;
      swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
    }
  
    internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Pose obj) {
      return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
    }
  
    ~Pose() {
      Dispose();
    }
  
    public virtual void Dispose() {
      lock(this) {
        if (swigCPtr.Handle != global::System.IntPtr.Zero) {
          if (swigCMemOwn) {
            swigCMemOwn = false;
            SolARPipelineManagerPINVOKE.delete_PipelineManager_Pose(swigCPtr);
          }
          swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
        }
        global::System.GC.SuppressFinalize(this);
      }
    }
  
    public SWIGTYPE_p_float T {
      set {
        SolARPipelineManagerPINVOKE.PipelineManager_Pose_T_set(swigCPtr, SWIGTYPE_p_float.getCPtr(value));
      } 
      get {
        global::System.IntPtr cPtr = SolARPipelineManagerPINVOKE.PipelineManager_Pose_T_get(swigCPtr);
        SWIGTYPE_p_float ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_float(cPtr, false);
        return ret;
      } 
    }
  
    public SWIGTYPE_p_a_3__float R {
      set {
        SolARPipelineManagerPINVOKE.PipelineManager_Pose_R_set(swigCPtr, SWIGTYPE_p_a_3__float.getCPtr(value));
      } 
      get {
        global::System.IntPtr cPtr = SolARPipelineManagerPINVOKE.PipelineManager_Pose_R_get(swigCPtr);
        SWIGTYPE_p_a_3__float ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_a_3__float(cPtr, false);
        return ret;
      } 
    }
  
    public void reset() {
      SolARPipelineManagerPINVOKE.PipelineManager_Pose_reset(swigCPtr);
    }
  
    public float translation(int i) {
      float ret = SolARPipelineManagerPINVOKE.PipelineManager_Pose_translation(swigCPtr, i);
      return ret;
    }
  
    public float rotation(int i, int j) {
      float ret = SolARPipelineManagerPINVOKE.PipelineManager_Pose_rotation(swigCPtr, i, j);
      return ret;
    }
  
    public Pose() : this(SolARPipelineManagerPINVOKE.new_PipelineManager_Pose(), true) {
    }
  
  }

  public class CamParams : global::System.IDisposable {
    private global::System.Runtime.InteropServices.HandleRef swigCPtr;
    protected bool swigCMemOwn;
  
    internal CamParams(global::System.IntPtr cPtr, bool cMemoryOwn) {
      swigCMemOwn = cMemoryOwn;
      swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
    }
  
    internal static global::System.Runtime.InteropServices.HandleRef getCPtr(CamParams obj) {
      return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
    }
  
    ~CamParams() {
      Dispose();
    }
  
    public virtual void Dispose() {
      lock(this) {
        if (swigCPtr.Handle != global::System.IntPtr.Zero) {
          if (swigCMemOwn) {
            swigCMemOwn = false;
            SolARPipelineManagerPINVOKE.delete_PipelineManager_CamParams(swigCPtr);
          }
          swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
        }
        global::System.GC.SuppressFinalize(this);
      }
    }
  
    public int width {
      set {
        SolARPipelineManagerPINVOKE.PipelineManager_CamParams_width_set(swigCPtr, value);
      } 
      get {
        int ret = SolARPipelineManagerPINVOKE.PipelineManager_CamParams_width_get(swigCPtr);
        return ret;
      } 
    }
  
    public int height {
      set {
        SolARPipelineManagerPINVOKE.PipelineManager_CamParams_height_set(swigCPtr, value);
      } 
      get {
        int ret = SolARPipelineManagerPINVOKE.PipelineManager_CamParams_height_get(swigCPtr);
        return ret;
      } 
    }
  
    public float focalX {
      set {
        SolARPipelineManagerPINVOKE.PipelineManager_CamParams_focalX_set(swigCPtr, value);
      } 
      get {
        float ret = SolARPipelineManagerPINVOKE.PipelineManager_CamParams_focalX_get(swigCPtr);
        return ret;
      } 
    }
  
    public float focalY {
      set {
        SolARPipelineManagerPINVOKE.PipelineManager_CamParams_focalY_set(swigCPtr, value);
      } 
      get {
        float ret = SolARPipelineManagerPINVOKE.PipelineManager_CamParams_focalY_get(swigCPtr);
        return ret;
      } 
    }
  
    public CamParams() : this(SolARPipelineManagerPINVOKE.new_PipelineManager_CamParams(), true) {
    }
  
  }

  public PipelineManager() : this(SolARPipelineManagerPINVOKE.new_PipelineManager(), true) {
  }

  public bool init(string conf_path, string pipelineUUID) {
    bool ret = SolARPipelineManagerPINVOKE.PipelineManager_init(swigCPtr, conf_path, pipelineUUID);
    if (SolARPipelineManagerPINVOKE.SWIGPendingException.Pending) throw SolARPipelineManagerPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public PipelineManager.CamParams getCameraParameters() {
    PipelineManager.CamParams ret = new PipelineManager.CamParams(SolARPipelineManagerPINVOKE.PipelineManager_getCameraParameters(swigCPtr), true);
    return ret;
  }

  public bool start(System.IntPtr textureHandle) {
    bool ret = SolARPipelineManagerPINVOKE.PipelineManager_start(swigCPtr,  textureHandle );
    return ret;
  }

  public bool udpate(PipelineManager.Pose pose) {
    bool ret = SolARPipelineManagerPINVOKE.PipelineManager_udpate(swigCPtr, PipelineManager.Pose.getCPtr(pose));
    if (SolARPipelineManagerPINVOKE.SWIGPendingException.Pending) throw SolARPipelineManagerPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool stop() {
    bool ret = SolARPipelineManagerPINVOKE.PipelineManager_stop(swigCPtr);
    return ret;
  }

}

}