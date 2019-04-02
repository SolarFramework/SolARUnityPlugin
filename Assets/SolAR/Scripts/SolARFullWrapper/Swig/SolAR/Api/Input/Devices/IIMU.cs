//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Input.Devices {

    using XPCF.Api;
    using SolAR.Core;
    using SolAR.Datastructure;

public class IIMU : IComponentIntrospect {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnDerived;

  internal IIMU(global::System.IntPtr cPtr, bool cMemoryOwn) : base(solar_api_input_devicesPINVOKE.IIMU_SWIGSmartPtrUpcast(cPtr), true) {
    swigCMemOwnDerived = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(IIMU obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~IIMU() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnDerived) {
          swigCMemOwnDerived = false;
          solar_api_input_devicesPINVOKE.delete_IIMU(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public virtual FrameworkReturnCode start() {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_input_devicesPINVOKE.IIMU_start(swigCPtr);
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual FrameworkReturnCode getGyroscopeData(Vector3f gyroData) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_input_devicesPINVOKE.IIMU_getGyroscopeData(swigCPtr, Vector3f.getCPtr(gyroData));
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual FrameworkReturnCode getAccelerometerData(Vector3f accelData) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_input_devicesPINVOKE.IIMU_getAccelerometerData(swigCPtr, Vector3f.getCPtr(accelData));
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual FrameworkReturnCode getMagnetometerData(Vector3f magData) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_input_devicesPINVOKE.IIMU_getMagnetometerData(swigCPtr, Vector3f.getCPtr(magData));
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual FrameworkReturnCode getAllSensorsData(Vector3f gyroData, Vector3f accelData, Vector3f magData) {
    FrameworkReturnCode ret = (FrameworkReturnCode)solar_api_input_devicesPINVOKE.IIMU_getAllSensorsData(swigCPtr, Vector3f.getCPtr(gyroData), Vector3f.getCPtr(accelData), Vector3f.getCPtr(magData));
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual bool isGyroscopeAvailable() {
    bool ret = solar_api_input_devicesPINVOKE.IIMU_isGyroscopeAvailable(swigCPtr);
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual bool isAccelerometerAvailable() {
    bool ret = solar_api_input_devicesPINVOKE.IIMU_isAccelerometerAvailable(swigCPtr);
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual bool isMagnetometerAvailable() {
    bool ret = solar_api_input_devicesPINVOKE.IIMU_isMagnetometerAvailable(swigCPtr);
    if (solar_api_input_devicesPINVOKE.SWIGPendingException.Pending) throw solar_api_input_devicesPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
