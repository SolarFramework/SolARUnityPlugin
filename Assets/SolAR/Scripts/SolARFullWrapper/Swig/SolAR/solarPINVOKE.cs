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

class solarPINVOKE {

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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionCallbacks_solar")]
    public static extern void SWIGRegisterExceptionCallbacks_solar(
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_solar")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_solar(
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
      SWIGRegisterExceptionCallbacks_solar(
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

      SWIGRegisterExceptionCallbacksArgument_solar(
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
      lock(typeof(solarPINVOKE)) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(typeof(solarPINVOKE)) {
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

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterStringCallback_solar")]
    public static extern void SWIGRegisterStringCallback_solar(SWIGStringDelegate stringDelegate);

    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_solar(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static solarPINVOKE() {
  }


  protected class SWIGWStringHelper {

    public delegate string SWIGWStringDelegate(global::System.IntPtr message);
    static SWIGWStringDelegate wstringDelegate = new SWIGWStringDelegate(CreateWString);

    [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="SWIGRegisterWStringCallback_solar")]
    public static extern void SWIGRegisterWStringCallback_solar(SWIGWStringDelegate wstringDelegate);

    static string CreateWString([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPWStr)]global::System.IntPtr cString) {
      return global::System.Runtime.InteropServices.Marshal.PtrToStringUni(cString);
    }

    static SWIGWStringHelper() {
      SWIGRegisterWStringCallback_solar(wstringDelegate);
    }
  }

  static protected SWIGWStringHelper swigWStringHelper = new SWIGWStringHelper();


  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IConfigurable")]
  public static extern global::System.IntPtr bindTo_IConfigurable(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I2DOverlay")]
  public static extern global::System.IntPtr bindTo_I2DOverlay(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I3DOverlay")]
  public static extern global::System.IntPtr bindTo_I3DOverlay(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I3DPointsViewer")]
  public static extern global::System.IntPtr bindTo_I3DPointsViewer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IImageViewer")]
  public static extern global::System.IntPtr bindTo_IImageViewer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMatchesOverlay")]
  public static extern global::System.IntPtr bindTo_IMatchesOverlay(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IInterface1")]
  public static extern global::System.IntPtr bindTo_IInterface1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IInterface2")]
  public static extern global::System.IntPtr bindTo_IInterface2(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IContoursExtractor")]
  public static extern global::System.IntPtr bindTo_IContoursExtractor(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IContoursFilter")]
  public static extern global::System.IntPtr bindTo_IContoursFilter(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IDescriptorMatcher")]
  public static extern global::System.IntPtr bindTo_IDescriptorMatcher(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IDescriptorsExtractor")]
  public static extern global::System.IntPtr bindTo_IDescriptorsExtractor(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IDescriptorsExtractorSBPattern")]
  public static extern global::System.IntPtr bindTo_IDescriptorsExtractorSBPattern(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IKeypointDetector")]
  public static extern global::System.IntPtr bindTo_IKeypointDetector(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IKeypointDetectorRegion")]
  public static extern global::System.IntPtr bindTo_IKeypointDetectorRegion(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IKeypointsReIndexer")]
  public static extern global::System.IntPtr bindTo_IKeypointsReIndexer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMatchesFilter")]
  public static extern global::System.IntPtr bindTo_IMatchesFilter(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_ISBPatternReIndexer")]
  public static extern global::System.IntPtr bindTo_ISBPatternReIndexer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IVisualInertialFusion")]
  public static extern global::System.IntPtr bindTo_IVisualInertialFusion(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I2DTransform")]
  public static extern global::System.IntPtr bindTo_I2DTransform(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IProject")]
  public static extern global::System.IntPtr bindTo_IProject(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IUnproject")]
  public static extern global::System.IntPtr bindTo_IUnproject(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I3DTransform")]
  public static extern global::System.IntPtr bindTo_I3DTransform(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IImage2WorldMapper")]
  public static extern global::System.IntPtr bindTo_IImage2WorldMapper(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IUndistortPoints")]
  public static extern global::System.IntPtr bindTo_IUndistortPoints(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IImageConvertor")]
  public static extern global::System.IntPtr bindTo_IImageConvertor(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IImageFilter")]
  public static extern global::System.IntPtr bindTo_IImageFilter(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IImageLoader")]
  public static extern global::System.IntPtr bindTo_IImageLoader(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IPerspectiveController")]
  public static extern global::System.IntPtr bindTo_IPerspectiveController(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_ICamera")]
  public static extern global::System.IntPtr bindTo_ICamera(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_ICameraCalibration")]
  public static extern global::System.IntPtr bindTo_ICameraCalibration(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IIMU")]
  public static extern global::System.IntPtr bindTo_IIMU(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMarker")]
  public static extern global::System.IntPtr bindTo_IMarker(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMarker2DNaturalImage")]
  public static extern global::System.IntPtr bindTo_IMarker2DNaturalImage(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMarker2DSquared")]
  public static extern global::System.IntPtr bindTo_IMarker2DSquared(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMarker2DSquaredBinary")]
  public static extern global::System.IntPtr bindTo_IMarker2DSquaredBinary(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IPipeline")]
  public static extern global::System.IntPtr bindTo_IPipeline(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IKeyframeRetriever")]
  public static extern global::System.IntPtr bindTo_IKeyframeRetriever(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IRelocalizer")]
  public static extern global::System.IntPtr bindTo_IRelocalizer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_ISinkPoseImage")]
  public static extern global::System.IntPtr bindTo_ISinkPoseImage(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_ISinkPoseTextureBuffer")]
  public static extern global::System.IntPtr bindTo_ISinkPoseTextureBuffer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IBundler")]
  public static extern global::System.IntPtr bindTo_IBundler(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IKeyframeSelector")]
  public static extern global::System.IntPtr bindTo_IKeyframeSelector(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMapFilter")]
  public static extern global::System.IntPtr bindTo_IMapFilter(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IMapper")]
  public static extern global::System.IntPtr bindTo_IMapper(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_ITriangulator")]
  public static extern global::System.IntPtr bindTo_ITriangulator(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I2D3DCorrespondencesFinder")]
  public static extern global::System.IntPtr bindTo_I2D3DCorrespondencesFinder(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I2Dto3DTransformDecomposer")]
  public static extern global::System.IntPtr bindTo_I2Dto3DTransformDecomposer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I2DTransformFinder")]
  public static extern global::System.IntPtr bindTo_I2DTransformFinder(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I3DTransformFinderFrom2D2D")]
  public static extern global::System.IntPtr bindTo_I3DTransformFinderFrom2D2D(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I3DTransformFinderFrom2D3D")]
  public static extern global::System.IntPtr bindTo_I3DTransformFinderFrom2D3D(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_I3DTransformSACFinderFrom2D3D")]
  public static extern global::System.IntPtr bindTo_I3DTransformSACFinderFrom2D3D(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IHomographyValidation")]
  public static extern global::System.IntPtr bindTo_IHomographyValidation(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_ISourceImage")]
  public static extern global::System.IntPtr bindTo_ISourceImage(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("SolARWrapper", EntryPoint="CSharp_SolAR_bindTo_IOpticalFlowEstimator")]
  public static extern global::System.IntPtr bindTo_IOpticalFlowEstimator(global::System.Runtime.InteropServices.HandleRef jarg1);
}

}
