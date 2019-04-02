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

class xpcf_corePINVOKE {

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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionCallbacks_xpcf_core")]
    public static extern void SWIGRegisterExceptionCallbacks_xpcf_core(
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_xpcf_core")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_xpcf_core(
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
      SWIGRegisterExceptionCallbacks_xpcf_core(
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

      SWIGRegisterExceptionCallbacksArgument_xpcf_core(
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
      lock(typeof(xpcf_corePINVOKE)) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(typeof(xpcf_corePINVOKE)) {
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterStringCallback_xpcf_core")]
    public static extern void SWIGRegisterStringCallback_xpcf_core(SWIGStringDelegate stringDelegate);

    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_xpcf_core(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static xpcf_corePINVOKE() {
  }


  protected class SWIGWStringHelper {

    public delegate string SWIGWStringDelegate(global::System.IntPtr message);
    static SWIGWStringDelegate wstringDelegate = new SWIGWStringDelegate(CreateWString);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterWStringCallback_xpcf_core")]
    public static extern void SWIGRegisterWStringCallback_xpcf_core(SWIGWStringDelegate wstringDelegate);

    static string CreateWString([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)]global::System.IntPtr cString) {
      return global::System.Runtime.InteropServices.Marshal.PtrToStringUni(cString);
    }

    static SWIGWStringHelper() {
      SWIGRegisterWStringCallback_xpcf_core(wstringDelegate);
    }
  }

  static protected SWIGWStringHelper swigWStringHelper = new SWIGWStringHelper();


  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_UUID___")]
  public static extern global::System.IntPtr new_UUID(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_UUID_ToString___")]
  public static extern string UUID_ToString(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_UUID___")]
  public static extern void delete_UUID(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_Exception__SWIG_0___")]
  public static extern global::System.IntPtr new_Exception__SWIG_0(int jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_Exception__SWIG_1___")]
  public static extern global::System.IntPtr new_Exception__SWIG_1(string jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_Exception__SWIG_2___")]
  public static extern global::System.IntPtr new_Exception__SWIG_2(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_Exception___")]
  public static extern void delete_Exception(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_Exception_getErrorCode___")]
  public static extern int Exception_getErrorCode(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_AccessDeniedException__SWIG_0___")]
  public static extern global::System.IntPtr new_AccessDeniedException__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_AccessDeniedException__SWIG_1___")]
  public static extern global::System.IntPtr new_AccessDeniedException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_AccessDeniedException___")]
  public static extern void delete_AccessDeniedException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_IllegalStateException__SWIG_0___")]
  public static extern global::System.IntPtr new_IllegalStateException__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_IllegalStateException__SWIG_1___")]
  public static extern global::System.IntPtr new_IllegalStateException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_IllegalStateException___")]
  public static extern void delete_IllegalStateException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_ModuleException__SWIG_0___")]
  public static extern global::System.IntPtr new_ModuleException__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_ModuleException__SWIG_1___")]
  public static extern global::System.IntPtr new_ModuleException__SWIG_1(string jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_ModuleException__SWIG_2___")]
  public static extern global::System.IntPtr new_ModuleException__SWIG_2(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_ModuleException___")]
  public static extern void delete_ModuleException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_NotImplementedException__SWIG_0___")]
  public static extern global::System.IntPtr new_NotImplementedException__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_NotImplementedException__SWIG_1___")]
  public static extern global::System.IntPtr new_NotImplementedException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_NotImplementedException___")]
  public static extern void delete_NotImplementedException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_NullPointerException__SWIG_0___")]
  public static extern global::System.IntPtr new_NullPointerException__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_NullPointerException__SWIG_1___")]
  public static extern global::System.IntPtr new_NullPointerException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_NullPointerException___")]
  public static extern void delete_NullPointerException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_TimeoutException__SWIG_0___")]
  public static extern global::System.IntPtr new_TimeoutException__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_TimeoutException__SWIG_1___")]
  public static extern global::System.IntPtr new_TimeoutException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_TimeoutException___")]
  public static extern void delete_TimeoutException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_ComponentNotFoundException__SWIG_0___")]
  public static extern global::System.IntPtr new_ComponentNotFoundException__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_ComponentNotFoundException__SWIG_1___")]
  public static extern global::System.IntPtr new_ComponentNotFoundException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_ComponentNotFoundException___")]
  public static extern void delete_ComponentNotFoundException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_InterfaceNotImplementedException__SWIG_0___")]
  public static extern global::System.IntPtr new_InterfaceNotImplementedException__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_InterfaceNotImplementedException__SWIG_1___")]
  public static extern global::System.IntPtr new_InterfaceNotImplementedException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_InterfaceNotImplementedException___")]
  public static extern void delete_InterfaceNotImplementedException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_ModuleNotFoundException__SWIG_0___")]
  public static extern global::System.IntPtr new_ModuleNotFoundException__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_new_ModuleNotFoundException__SWIG_1___")]
  public static extern global::System.IntPtr new_ModuleNotFoundException__SWIG_1(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_delete_ModuleNotFoundException___")]
  public static extern void delete_ModuleNotFoundException(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_AccessDeniedException_SWIGUpcast___")]
  public static extern global::System.IntPtr AccessDeniedException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_IllegalStateException_SWIGUpcast___")]
  public static extern global::System.IntPtr IllegalStateException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_ModuleException_SWIGUpcast___")]
  public static extern global::System.IntPtr ModuleException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_NotImplementedException_SWIGUpcast___")]
  public static extern global::System.IntPtr NotImplementedException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_NullPointerException_SWIGUpcast___")]
  public static extern global::System.IntPtr NullPointerException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_TimeoutException_SWIGUpcast___")]
  public static extern global::System.IntPtr TimeoutException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_UUIDNotFoundException_SWIGUpcast___")]
  public static extern global::System.IntPtr UUIDNotFoundException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_ComponentNotFoundException_SWIGUpcast___")]
  public static extern global::System.IntPtr ComponentNotFoundException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_InterfaceNotImplementedException_SWIGUpcast___")]
  public static extern global::System.IntPtr InterfaceNotImplementedException_SWIGUpcast(global::System.IntPtr jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCore_ModuleNotFoundException_SWIGUpcast___")]
  public static extern global::System.IntPtr ModuleNotFoundException_SWIGUpcast(global::System.IntPtr jarg1);
}

}
