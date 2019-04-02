//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace XPCF.Properties {

class xpcf_propertiesPINVOKE {

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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionCallbacks_xpcf_properties")]
    public static extern void SWIGRegisterExceptionCallbacks_xpcf_properties(
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_xpcf_properties")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_xpcf_properties(
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
      SWIGRegisterExceptionCallbacks_xpcf_properties(
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

      SWIGRegisterExceptionCallbacksArgument_xpcf_properties(
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
      lock(typeof(xpcf_propertiesPINVOKE)) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(typeof(xpcf_propertiesPINVOKE)) {
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterStringCallback_xpcf_properties")]
    public static extern void SWIGRegisterStringCallback_xpcf_properties(SWIGStringDelegate stringDelegate);

    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_xpcf_properties(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static xpcf_propertiesPINVOKE() {
  }


  protected class SWIGWStringHelper {

    public delegate string SWIGWStringDelegate(global::System.IntPtr message);
    static SWIGWStringDelegate wstringDelegate = new SWIGWStringDelegate(CreateWString);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterWStringCallback_xpcf_properties")]
    public static extern void SWIGRegisterWStringCallback_xpcf_properties(SWIGWStringDelegate wstringDelegate);

    static string CreateWString([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)]global::System.IntPtr cString) {
      return global::System.Runtime.InteropServices.Marshal.PtrToStringUni(cString);
    }

    static SWIGWStringHelper() {
      SWIGRegisterWStringCallback_xpcf_properties(wstringDelegate);
    }
  }

  static protected SWIGWStringHelper swigWStringHelper = new SWIGWStringHelper();


  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_delete_IProperty___")]
  public static extern void delete_IProperty(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getName___")]
  public static extern string IProperty_getName(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getType___")]
  public static extern int IProperty_getType(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_copyTo___")]
  public static extern int IProperty_copyTo(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setName___")]
  public static extern void IProperty_setName(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_size___")]
  public static extern uint IProperty_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setIntegerValue__SWIG_0___")]
  public static extern void IProperty_setIntegerValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setIntegerValue__SWIG_1___")]
  public static extern void IProperty_setIntegerValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setUnsignedIntegerValue__SWIG_0___")]
  public static extern void IProperty_setUnsignedIntegerValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setUnsignedIntegerValue__SWIG_1___")]
  public static extern void IProperty_setUnsignedIntegerValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setLongValue__SWIG_0___")]
  public static extern void IProperty_setLongValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, long jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setLongValue__SWIG_1___")]
  public static extern void IProperty_setLongValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, long jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setUnsignedLongValue__SWIG_0___")]
  public static extern void IProperty_setUnsignedLongValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, ulong jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setUnsignedLongValue__SWIG_1___")]
  public static extern void IProperty_setUnsignedLongValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, ulong jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setFloatingValue__SWIG_0___")]
  public static extern void IProperty_setFloatingValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, float jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setFloatingValue__SWIG_1___")]
  public static extern void IProperty_setFloatingValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, float jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setDoubleValue__SWIG_0___")]
  public static extern void IProperty_setDoubleValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, double jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setDoubleValue__SWIG_1___")]
  public static extern void IProperty_setDoubleValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, double jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setStringValue__SWIG_0___")]
  public static extern void IProperty_setStringValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setStringValue__SWIG_1___")]
  public static extern void IProperty_setStringValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setUnicodeStringValue__SWIG_0___")]
  public static extern void IProperty_setUnicodeStringValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)]string jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setUnicodeStringValue__SWIG_1___")]
  public static extern void IProperty_setUnicodeStringValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)]string jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setStructureValue__SWIG_0___")]
  public static extern void IProperty_setStructureValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setStructureValue__SWIG_1___")]
  public static extern void IProperty_setStructureValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getStructureValue__SWIG_0___")]
  public static extern global::System.IntPtr IProperty_getStructureValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getStructureValue__SWIG_1___")]
  public static extern global::System.IntPtr IProperty_getStructureValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setProperties__SWIG_0___")]
  public static extern int IProperty_setProperties__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2, uint jarg3, int jarg4);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setProperties__SWIG_1___")]
  public static extern int IProperty_setProperties__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setProperties__SWIG_2___")]
  public static extern int IProperty_setProperties__SWIG_2(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getIntegerValue__SWIG_0___")]
  public static extern int IProperty_getIntegerValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getIntegerValue__SWIG_1___")]
  public static extern int IProperty_getIntegerValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getUnsignedIntegerValue__SWIG_0___")]
  public static extern uint IProperty_getUnsignedIntegerValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getUnsignedIntegerValue__SWIG_1___")]
  public static extern uint IProperty_getUnsignedIntegerValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getLongValue__SWIG_0___")]
  public static extern long IProperty_getLongValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getLongValue__SWIG_1___")]
  public static extern long IProperty_getLongValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getUnsignedLongValue___")]
  public static extern ulong IProperty_getUnsignedLongValue(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getStringValue__SWIG_0___")]
  public static extern string IProperty_getStringValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getStringValue__SWIG_1___")]
  public static extern string IProperty_getStringValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getUnicodeStringValue__SWIG_0___")]
  public static extern global::System.IntPtr IProperty_getUnicodeStringValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getUnicodeStringValue__SWIG_1___")]
  public static extern global::System.IntPtr IProperty_getUnicodeStringValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getFloatingValue__SWIG_0___")]
  public static extern float IProperty_getFloatingValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getFloatingValue__SWIG_1___")]
  public static extern float IProperty_getFloatingValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getDoubleValue__SWIG_0___")]
  public static extern double IProperty_getDoubleValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getDoubleValue__SWIG_1___")]
  public static extern double IProperty_getDoubleValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_setAccessSpecifier___")]
  public static extern void IProperty_setAccessSpecifier(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IProperty_getAccessSpecifier___")]
  public static extern int IProperty_getAccessSpecifier(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_delete_IPropertyMap___")]
  public static extern void delete_IPropertyMap(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IPropertyMap_addProperty___")]
  public static extern int IPropertyMap_addProperty(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IPropertyMap_setProperty___")]
  public static extern int IPropertyMap_setProperty(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IPropertyMap_getProperties___")]
  public static extern global::System.IntPtr IPropertyMap_getProperties(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_IPropertyMap_at___")]
  public static extern global::System.IntPtr IPropertyMap_at(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_XPCFfProperties_getPropertyMapInstance___")]
  public static extern global::System.IntPtr getPropertyMapInstance();
}

}
