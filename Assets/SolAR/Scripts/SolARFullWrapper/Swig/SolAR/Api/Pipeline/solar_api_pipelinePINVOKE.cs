//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace SolAR.Api.Pipeline {

class solar_api_pipelinePINVOKE {

  protected class SWIGExceptionHelper {

    public delegate void ExceptionDelegate(string message);
    public delegate void ExceptionArgumentDelegate(string message, string paramName);

    static ExceptionDelegate applicationDelegate = new ExceptionDelegate(SetPendingApplicationException);
    static ExceptionDelegate arithmeticDelegate = new ExceptionDelegate(SetPendingArithmeticException);
    static ExceptionDelegate divideByZeroDelegate = new ExceptionDelegate(SetPendingDivideByZeroException);
    static ExceptionDelegate indexOutOfRangeDelegate = new ExceptionDelegate(SetPendingIndexOutOfRangeException);
    static ExceptionDelegate invalidCastDelegate = new ExceptionDelegate(SetPendingInvalidCastException);
    static ExceptionDelegate invalidOperationDelegate = new ExceptionDelegate(SetPendingInvalidOperationException);
    static ExceptionDelegate ioDelegate = new ExceptionDelegate(SetPendingIOException);
    static ExceptionDelegate nullReferenceDelegate = new ExceptionDelegate(SetPendingNullReferenceException);
    static ExceptionDelegate outOfMemoryDelegate = new ExceptionDelegate(SetPendingOutOfMemoryException);
    static ExceptionDelegate overflowDelegate = new ExceptionDelegate(SetPendingOverflowException);
    static ExceptionDelegate systemDelegate = new ExceptionDelegate(SetPendingSystemException);

    static ExceptionArgumentDelegate argumentDelegate = new ExceptionArgumentDelegate(SetPendingArgumentException);
    static ExceptionArgumentDelegate argumentNullDelegate = new ExceptionArgumentDelegate(SetPendingArgumentNullException);
    static ExceptionArgumentDelegate argumentOutOfRangeDelegate = new ExceptionArgumentDelegate(SetPendingArgumentOutOfRangeException);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionCallbacks_solar_api_pipeline")]
    public static extern void SWIGRegisterExceptionCallbacks_solar_api_pipeline(
                                ExceptionDelegate applicationDelegate,
                                ExceptionDelegate arithmeticDelegate,
                                ExceptionDelegate divideByZeroDelegate, 
                                ExceptionDelegate indexOutOfRangeDelegate, 
                                ExceptionDelegate invalidCastDelegate,
                                ExceptionDelegate invalidOperationDelegate,
                                ExceptionDelegate ioDelegate,
                                ExceptionDelegate nullReferenceDelegate,
                                ExceptionDelegate outOfMemoryDelegate, 
                                ExceptionDelegate overflowDelegate, 
                                ExceptionDelegate systemExceptionDelegate);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_solar_api_pipeline")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_solar_api_pipeline(
                                ExceptionArgumentDelegate argumentDelegate,
                                ExceptionArgumentDelegate argumentNullDelegate,
                                ExceptionArgumentDelegate argumentOutOfRangeDelegate);

    static void SetPendingApplicationException(string message) {
      SWIGPendingException.Set(new global::System.ApplicationException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingArithmeticException(string message) {
      SWIGPendingException.Set(new global::System.ArithmeticException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingDivideByZeroException(string message) {
      SWIGPendingException.Set(new global::System.DivideByZeroException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingIndexOutOfRangeException(string message) {
      SWIGPendingException.Set(new global::System.IndexOutOfRangeException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingInvalidCastException(string message) {
      SWIGPendingException.Set(new global::System.InvalidCastException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingInvalidOperationException(string message) {
      SWIGPendingException.Set(new global::System.InvalidOperationException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingIOException(string message) {
      SWIGPendingException.Set(new global::System.IO.IOException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingNullReferenceException(string message) {
      SWIGPendingException.Set(new global::System.NullReferenceException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingOutOfMemoryException(string message) {
      SWIGPendingException.Set(new global::System.OutOfMemoryException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingOverflowException(string message) {
      SWIGPendingException.Set(new global::System.OverflowException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingSystemException(string message) {
      SWIGPendingException.Set(new global::System.SystemException(message, SWIGPendingException.Retrieve()));
    }

    static void SetPendingArgumentException(string message, string paramName) {
      SWIGPendingException.Set(new global::System.ArgumentException(message, paramName, SWIGPendingException.Retrieve()));
    }
    static void SetPendingArgumentNullException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentNullException(paramName, message));
    }
    static void SetPendingArgumentOutOfRangeException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentOutOfRangeException(paramName, message));
    }

    static SWIGExceptionHelper() {
      SWIGRegisterExceptionCallbacks_solar_api_pipeline(
                                applicationDelegate,
                                arithmeticDelegate,
                                divideByZeroDelegate,
                                indexOutOfRangeDelegate,
                                invalidCastDelegate,
                                invalidOperationDelegate,
                                ioDelegate,
                                nullReferenceDelegate,
                                outOfMemoryDelegate,
                                overflowDelegate,
                                systemDelegate);

      SWIGRegisterExceptionCallbacksArgument_solar_api_pipeline(
                                argumentDelegate,
                                argumentNullDelegate,
                                argumentOutOfRangeDelegate);
    }
  }

  protected static SWIGExceptionHelper swigExceptionHelper = new SWIGExceptionHelper();

  public class SWIGPendingException {
    [global::System.ThreadStatic]
    private static global::System.Exception pendingException = null;
    private static int numExceptionsPending = 0;

    public static bool Pending {
      get {
        bool pending = false;
        if (numExceptionsPending > 0)
          if (pendingException != null)
            pending = true;
        return pending;
      } 
    }

    public static void Set(global::System.Exception e) {
      if (pendingException != null)
        throw new global::System.ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + pendingException.ToString() + ")", e);
      pendingException = e;
      lock(typeof(solar_api_pipelinePINVOKE)) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(typeof(solar_api_pipelinePINVOKE)) {
            numExceptionsPending--;
          }
        }
      }
      return e;
    }
  }


  protected class SWIGStringHelper {

    public delegate string SWIGStringDelegate(string message);
    static SWIGStringDelegate stringDelegate = new SWIGStringDelegate(CreateString);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterStringCallback_solar_api_pipeline")]
    public static extern void SWIGRegisterStringCallback_solar_api_pipeline(SWIGStringDelegate stringDelegate);

    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_solar_api_pipeline(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static solar_api_pipelinePINVOKE() {
  }


  protected class SWIGWStringHelper {

    public delegate string SWIGWStringDelegate(global::System.IntPtr message);
    static SWIGWStringDelegate wstringDelegate = new SWIGWStringDelegate(CreateWString);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterWStringCallback_solar_api_pipeline")]
    public static extern void SWIGRegisterWStringCallback_solar_api_pipeline(SWIGWStringDelegate wstringDelegate);

    static string CreateWString([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)]global::System.IntPtr cString) {
      return global::System.Runtime.InteropServices.Marshal.PtrToStringUni(cString);
    }

    static SWIGWStringHelper() {
      SWIGRegisterWStringCallback_solar_api_pipeline(wstringDelegate);
    }
  }

  static protected SWIGWStringHelper swigWStringHelper = new SWIGWStringHelper();


  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_width_set___")]
  public static extern void CameraParameters_width_set(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_width_get___")]
  public static extern int CameraParameters_width_get(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_height_set___")]
  public static extern void CameraParameters_height_set(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_height_get___")]
  public static extern int CameraParameters_height_get(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_focalX_set___")]
  public static extern void CameraParameters_focalX_set(global::System.Runtime.InteropServices.HandleRef jarg1, float jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_focalX_get___")]
  public static extern float CameraParameters_focalX_get(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_focalY_set___")]
  public static extern void CameraParameters_focalY_set(global::System.Runtime.InteropServices.HandleRef jarg1, float jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_focalY_get___")]
  public static extern float CameraParameters_focalY_get(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_centerX_set___")]
  public static extern void CameraParameters_centerX_set(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_centerX_get___")]
  public static extern int CameraParameters_centerX_get(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_centerY_set___")]
  public static extern void CameraParameters_centerY_set(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_CameraParameters_centerY_get___")]
  public static extern int CameraParameters_centerY_get(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_new_CameraParameters___")]
  public static extern global::System.IntPtr new_CameraParameters();

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_delete_CameraParameters___")]
  public static extern void delete_CameraParameters(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_delete_IPipeline___")]
  public static extern void delete_IPipeline(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_IPipeline_init___")]
  public static extern int IPipeline_init(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_IPipeline_getCameraParameters___")]
  public static extern global::System.IntPtr IPipeline_getCameraParameters(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_IPipeline_start___")]
  public static extern int IPipeline_start(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.IntPtr jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_IPipeline_stop___")]
  public static extern int IPipeline_stop(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_IPipeline_update___")]
  public static extern int IPipeline_update(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_IPipeline_loadSourceImage___")]
  public static extern int IPipeline_loadSourceImage(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.IntPtr jarg2, int jarg3, int jarg4);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolARfApifPipeline_IPipeline_SWIGSmartPtrUpcast___")]
  public static extern global::System.IntPtr IPipeline_SWIGSmartPtrUpcast(global::System.IntPtr jarg1);
}

}
