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

class xpcf_collectionPINVOKE {

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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionCallbacks_xpcf_collection")]
    public static extern void SWIGRegisterExceptionCallbacks_xpcf_collection(
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_xpcf_collection")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_xpcf_collection(
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
      SWIGRegisterExceptionCallbacks_xpcf_collection(
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

      SWIGRegisterExceptionCallbacksArgument_xpcf_collection(
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
      lock(typeof(xpcf_collectionPINVOKE)) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(typeof(xpcf_collectionPINVOKE)) {
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterStringCallback_xpcf_collection")]
    public static extern void SWIGRegisterStringCallback_xpcf_collection(SWIGStringDelegate stringDelegate);

    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_xpcf_collection(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static xpcf_collectionPINVOKE() {
  }


  protected class SWIGWStringHelper {

    public delegate string SWIGWStringDelegate(global::System.IntPtr message);
    static SWIGWStringDelegate wstringDelegate = new SWIGWStringDelegate(CreateWString);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterWStringCallback_xpcf_collection")]
    public static extern void SWIGRegisterWStringCallback_xpcf_collection(SWIGWStringDelegate wstringDelegate);

    static string CreateWString([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)]global::System.IntPtr cString) {
      return global::System.Runtime.InteropServices.Marshal.PtrToStringUni(cString);
    }

    static SWIGWStringHelper() {
      SWIGRegisterWStringCallback_xpcf_collection(wstringDelegate);
    }
  }

  static protected SWIGWStringHelper swigWStringHelper = new SWIGWStringHelper();


  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_UUIDEnumerator___")]
  public static extern void delete_UUIDEnumerator(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_UUIDEnumerator_MoveNext___")]
  public static extern bool UUIDEnumerator_MoveNext(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_UUIDEnumerator_Reset___")]
  public static extern void UUIDEnumerator_Reset(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_UUIDEnumerator_current___")]
  public static extern global::System.IntPtr UUIDEnumerator_current(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_UUIDEnumerator_endReached___")]
  public static extern bool UUIDEnumerator_endReached(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_UUIDEnumerable___")]
  public static extern void delete_UUIDEnumerable(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_UUIDEnumerable_getEnumerator__SWIG_0___")]
  public static extern global::System.IntPtr UUIDEnumerable_getEnumerator__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_UUIDEnumerable_getEnumerator__SWIG_1___")]
  public static extern global::System.IntPtr UUIDEnumerable_getEnumerator__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_UUIDEnumerable_size___")]
  public static extern uint UUIDEnumerable_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_ComponentMetadataEnumerator___")]
  public static extern void delete_ComponentMetadataEnumerator(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ComponentMetadataEnumerator_MoveNext___")]
  public static extern bool ComponentMetadataEnumerator_MoveNext(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ComponentMetadataEnumerator_Reset___")]
  public static extern void ComponentMetadataEnumerator_Reset(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ComponentMetadataEnumerator_current___")]
  public static extern global::System.IntPtr ComponentMetadataEnumerator_current(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ComponentMetadataEnumerator_endReached___")]
  public static extern bool ComponentMetadataEnumerator_endReached(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_ComponentMetadataEnumerable___")]
  public static extern void delete_ComponentMetadataEnumerable(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ComponentMetadataEnumerable_getEnumerator__SWIG_0___")]
  public static extern global::System.IntPtr ComponentMetadataEnumerable_getEnumerator__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ComponentMetadataEnumerable_getEnumerator__SWIG_1___")]
  public static extern global::System.IntPtr ComponentMetadataEnumerable_getEnumerator__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ComponentMetadataEnumerable_size___")]
  public static extern uint ComponentMetadataEnumerable_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_InterfaceMetadataEnumerator___")]
  public static extern void delete_InterfaceMetadataEnumerator(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_InterfaceMetadataEnumerator_MoveNext___")]
  public static extern bool InterfaceMetadataEnumerator_MoveNext(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_InterfaceMetadataEnumerator_Reset___")]
  public static extern void InterfaceMetadataEnumerator_Reset(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_InterfaceMetadataEnumerator_current___")]
  public static extern global::System.IntPtr InterfaceMetadataEnumerator_current(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_InterfaceMetadataEnumerator_endReached___")]
  public static extern bool InterfaceMetadataEnumerator_endReached(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_InterfaceMetadataEnumerable___")]
  public static extern void delete_InterfaceMetadataEnumerable(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_InterfaceMetadataEnumerable_getEnumerator__SWIG_0___")]
  public static extern global::System.IntPtr InterfaceMetadataEnumerable_getEnumerator__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_InterfaceMetadataEnumerable_getEnumerator__SWIG_1___")]
  public static extern global::System.IntPtr InterfaceMetadataEnumerable_getEnumerator__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_InterfaceMetadataEnumerable_size___")]
  public static extern uint InterfaceMetadataEnumerable_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_PropertyEnumerator___")]
  public static extern void delete_PropertyEnumerator(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_PropertyEnumerator_MoveNext___")]
  public static extern bool PropertyEnumerator_MoveNext(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_PropertyEnumerator_Reset___")]
  public static extern void PropertyEnumerator_Reset(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_PropertyEnumerator_current___")]
  public static extern global::System.IntPtr PropertyEnumerator_current(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_PropertyEnumerator_endReached___")]
  public static extern bool PropertyEnumerator_endReached(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_PropertyEnumerable___")]
  public static extern void delete_PropertyEnumerable(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_PropertyEnumerable_getEnumerator__SWIG_0___")]
  public static extern global::System.IntPtr PropertyEnumerable_getEnumerator__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_PropertyEnumerable_getEnumerator__SWIG_1___")]
  public static extern global::System.IntPtr PropertyEnumerable_getEnumerator__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_PropertyEnumerable_size___")]
  public static extern uint PropertyEnumerable_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_ModuleMetadataEnumerator___")]
  public static extern void delete_ModuleMetadataEnumerator(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ModuleMetadataEnumerator_MoveNext___")]
  public static extern bool ModuleMetadataEnumerator_MoveNext(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ModuleMetadataEnumerator_Reset___")]
  public static extern void ModuleMetadataEnumerator_Reset(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ModuleMetadataEnumerator_current___")]
  public static extern global::System.IntPtr ModuleMetadataEnumerator_current(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ModuleMetadataEnumerator_endReached___")]
  public static extern bool ModuleMetadataEnumerator_endReached(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_delete_ModuleMetadataEnumerable___")]
  public static extern void delete_ModuleMetadataEnumerable(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ModuleMetadataEnumerable_getEnumerator__SWIG_0___")]
  public static extern global::System.IntPtr ModuleMetadataEnumerable_getEnumerator__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ModuleMetadataEnumerable_getEnumerator__SWIG_1___")]
  public static extern global::System.IntPtr ModuleMetadataEnumerable_getEnumerator__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfCollection_ModuleMetadataEnumerable_size___")]
  public static extern uint ModuleMetadataEnumerable_size(global::System.Runtime.InteropServices.HandleRef jarg1);
}

}