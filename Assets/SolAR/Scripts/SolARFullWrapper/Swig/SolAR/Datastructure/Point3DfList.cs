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

public class Point3DfList : global::System.IDisposable, global::System.Collections.IEnumerable
    , global::System.Collections.Generic.IEnumerable<Point3Df>
 {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal Point3DfList(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Point3DfList obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~Point3DfList() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          solar_datastructurePINVOKE.delete_Point3DfList(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public Point3DfList(global::System.Collections.ICollection c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (Point3Df element in c) {
      this.Add(element);
    }
  }

  public bool IsFixedSize {
    get {
      return false;
    }
  }

  public bool IsReadOnly {
    get {
      return false;
    }
  }

  public Point3Df this[int index]  {
    get {
      return getitem(index);
    }
    set {
      setitem(index, value);
    }
  }

  public int Capacity {
    get {
      return (int)capacity();
    }
    set {
      if (value < size())
        throw new global::System.ArgumentOutOfRangeException("Capacity");
      reserve((uint)value);
    }
  }

  public int Count {
    get {
      return (int)size();
    }
  }

  public bool IsSynchronized {
    get {
      return false;
    }
  }

  public void CopyTo(Point3Df[] array)
  {
    CopyTo(0, array, 0, this.Count);
  }

  public void CopyTo(Point3Df[] array, int arrayIndex)
  {
    CopyTo(0, array, arrayIndex, this.Count);
  }

  public void CopyTo(int index, Point3Df[] array, int arrayIndex, int count)
  {
    if (array == null)
      throw new global::System.ArgumentNullException("array");
    if (index < 0)
      throw new global::System.ArgumentOutOfRangeException("index", "Value is less than zero");
    if (arrayIndex < 0)
      throw new global::System.ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
    if (count < 0)
      throw new global::System.ArgumentOutOfRangeException("count", "Value is less than zero");
    if (array.Rank > 1)
      throw new global::System.ArgumentException("Multi dimensional array.", "array");
    if (index+count > this.Count || arrayIndex+count > array.Length)
      throw new global::System.ArgumentException("Number of elements to copy is too large.");
    for (int i=0; i<count; i++)
      array.SetValue(getitemcopy(index+i), arrayIndex+i);
  }

  global::System.Collections.Generic.IEnumerator<Point3Df> global::System.Collections.Generic.IEnumerable<Point3Df>.GetEnumerator() {
    return new Point3DfListEnumerator(this);
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
    return new Point3DfListEnumerator(this);
  }

  public Point3DfListEnumerator GetEnumerator() {
    return new Point3DfListEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class Point3DfListEnumerator : global::System.Collections.IEnumerator
    , global::System.Collections.Generic.IEnumerator<Point3Df>
  {
    private Point3DfList collectionRef;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public Point3DfListEnumerator(Point3DfList collection) {
      collectionRef = collection;
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public Point3Df Current {
      get {
        if (currentIndex == -1)
          throw new global::System.InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new global::System.InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new global::System.InvalidOperationException("Collection modified.");
        return (Point3Df)currentObject;
      }
    }

    // Type-unsafe IEnumerator.Current
    object global::System.Collections.IEnumerator.Current {
      get {
        return Current;
      }
    }

    public bool MoveNext() {
      int size = collectionRef.Count;
      bool moveOkay = (currentIndex+1 < size) && (size == currentSize);
      if (moveOkay) {
        currentIndex++;
        currentObject = collectionRef[currentIndex];
      } else {
        currentObject = null;
      }
      return moveOkay;
    }

    public void Reset() {
      currentIndex = -1;
      currentObject = null;
      if (collectionRef.Count != currentSize) {
        throw new global::System.InvalidOperationException("Collection modified.");
      }
    }

    public void Dispose() {
        currentIndex = -1;
        currentObject = null;
    }
  }

  public void Clear() {
    solar_datastructurePINVOKE.Point3DfList_Clear(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void Add(Point3Df x) {
    solar_datastructurePINVOKE.Point3DfList_Add(swigCPtr, Point3Df.getCPtr(x));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  private uint size() {
    uint ret = solar_datastructurePINVOKE.Point3DfList_size(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private uint capacity() {
    uint ret = solar_datastructurePINVOKE.Point3DfList_capacity(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void reserve(uint n) {
    solar_datastructurePINVOKE.Point3DfList_reserve(swigCPtr, n);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Point3DfList() : this(solar_datastructurePINVOKE.new_Point3DfList__SWIG_0(), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Point3DfList(Point3DfList other) : this(solar_datastructurePINVOKE.new_Point3DfList__SWIG_1(Point3DfList.getCPtr(other)), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Point3DfList(int capacity) : this(solar_datastructurePINVOKE.new_Point3DfList__SWIG_2(capacity), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  private Point3Df getitemcopy(int index) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.Point3DfList_getitemcopy(swigCPtr, index);
    Point3Df ret = (cPtr == global::System.IntPtr.Zero) ? null : new Point3Df(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private Point3Df getitem(int index) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.Point3DfList_getitem(swigCPtr, index);
    Point3Df ret = (cPtr == global::System.IntPtr.Zero) ? null : new Point3Df(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int index, Point3Df val) {
    solar_datastructurePINVOKE.Point3DfList_setitem(swigCPtr, index, Point3Df.getCPtr(val));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void AddRange(Point3DfList values) {
    solar_datastructurePINVOKE.Point3DfList_AddRange(swigCPtr, Point3DfList.getCPtr(values));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public Point3DfList GetRange(int index, int count) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.Point3DfList_GetRange(swigCPtr, index, count);
    Point3DfList ret = (cPtr == global::System.IntPtr.Zero) ? null : new Point3DfList(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Insert(int index, Point3Df x) {
    solar_datastructurePINVOKE.Point3DfList_Insert(swigCPtr, index, Point3Df.getCPtr(x));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void InsertRange(int index, Point3DfList values) {
    solar_datastructurePINVOKE.Point3DfList_InsertRange(swigCPtr, index, Point3DfList.getCPtr(values));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveAt(int index) {
    solar_datastructurePINVOKE.Point3DfList_RemoveAt(swigCPtr, index);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveRange(int index, int count) {
    solar_datastructurePINVOKE.Point3DfList_RemoveRange(swigCPtr, index, count);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public static Point3DfList Repeat(Point3Df value, int count) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.Point3DfList_Repeat(Point3Df.getCPtr(value), count);
    Point3DfList ret = (cPtr == global::System.IntPtr.Zero) ? null : new Point3DfList(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Reverse() {
    solar_datastructurePINVOKE.Point3DfList_Reverse__SWIG_0(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void Reverse(int index, int count) {
    solar_datastructurePINVOKE.Point3DfList_Reverse__SWIG_1(swigCPtr, index, count);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRange(int index, Point3DfList values) {
    solar_datastructurePINVOKE.Point3DfList_SetRange(swigCPtr, index, Point3DfList.getCPtr(values));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

}

}
