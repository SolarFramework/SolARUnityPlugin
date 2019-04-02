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

public class DescriptorBufferList : global::System.IDisposable, global::System.Collections.IEnumerable
    , global::System.Collections.Generic.IEnumerable<DescriptorBuffer>
 {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal DescriptorBufferList(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(DescriptorBufferList obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~DescriptorBufferList() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          solar_datastructurePINVOKE.delete_DescriptorBufferList(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public DescriptorBufferList(global::System.Collections.ICollection c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (DescriptorBuffer element in c) {
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

  public DescriptorBuffer this[int index]  {
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

  public void CopyTo(DescriptorBuffer[] array)
  {
    CopyTo(0, array, 0, this.Count);
  }

  public void CopyTo(DescriptorBuffer[] array, int arrayIndex)
  {
    CopyTo(0, array, arrayIndex, this.Count);
  }

  public void CopyTo(int index, DescriptorBuffer[] array, int arrayIndex, int count)
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

  global::System.Collections.Generic.IEnumerator<DescriptorBuffer> global::System.Collections.Generic.IEnumerable<DescriptorBuffer>.GetEnumerator() {
    return new DescriptorBufferListEnumerator(this);
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
    return new DescriptorBufferListEnumerator(this);
  }

  public DescriptorBufferListEnumerator GetEnumerator() {
    return new DescriptorBufferListEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class DescriptorBufferListEnumerator : global::System.Collections.IEnumerator
    , global::System.Collections.Generic.IEnumerator<DescriptorBuffer>
  {
    private DescriptorBufferList collectionRef;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public DescriptorBufferListEnumerator(DescriptorBufferList collection) {
      collectionRef = collection;
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public DescriptorBuffer Current {
      get {
        if (currentIndex == -1)
          throw new global::System.InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new global::System.InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new global::System.InvalidOperationException("Collection modified.");
        return (DescriptorBuffer)currentObject;
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
    solar_datastructurePINVOKE.DescriptorBufferList_Clear(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void Add(DescriptorBuffer x) {
    solar_datastructurePINVOKE.DescriptorBufferList_Add(swigCPtr, DescriptorBuffer.getCPtr(x));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  private uint size() {
    uint ret = solar_datastructurePINVOKE.DescriptorBufferList_size(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private uint capacity() {
    uint ret = solar_datastructurePINVOKE.DescriptorBufferList_capacity(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void reserve(uint n) {
    solar_datastructurePINVOKE.DescriptorBufferList_reserve(swigCPtr, n);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public DescriptorBufferList() : this(solar_datastructurePINVOKE.new_DescriptorBufferList__SWIG_0(), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public DescriptorBufferList(DescriptorBufferList other) : this(solar_datastructurePINVOKE.new_DescriptorBufferList__SWIG_1(DescriptorBufferList.getCPtr(other)), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public DescriptorBufferList(int capacity) : this(solar_datastructurePINVOKE.new_DescriptorBufferList__SWIG_2(capacity), true) {
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  private DescriptorBuffer getitemcopy(int index) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.DescriptorBufferList_getitemcopy(swigCPtr, index);
    DescriptorBuffer ret = (cPtr == global::System.IntPtr.Zero) ? null : new DescriptorBuffer(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private DescriptorBuffer getitem(int index) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.DescriptorBufferList_getitem(swigCPtr, index);
    DescriptorBuffer ret = (cPtr == global::System.IntPtr.Zero) ? null : new DescriptorBuffer(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int index, DescriptorBuffer val) {
    solar_datastructurePINVOKE.DescriptorBufferList_setitem(swigCPtr, index, DescriptorBuffer.getCPtr(val));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void AddRange(DescriptorBufferList values) {
    solar_datastructurePINVOKE.DescriptorBufferList_AddRange(swigCPtr, DescriptorBufferList.getCPtr(values));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public DescriptorBufferList GetRange(int index, int count) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.DescriptorBufferList_GetRange(swigCPtr, index, count);
    DescriptorBufferList ret = (cPtr == global::System.IntPtr.Zero) ? null : new DescriptorBufferList(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Insert(int index, DescriptorBuffer x) {
    solar_datastructurePINVOKE.DescriptorBufferList_Insert(swigCPtr, index, DescriptorBuffer.getCPtr(x));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void InsertRange(int index, DescriptorBufferList values) {
    solar_datastructurePINVOKE.DescriptorBufferList_InsertRange(swigCPtr, index, DescriptorBufferList.getCPtr(values));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveAt(int index) {
    solar_datastructurePINVOKE.DescriptorBufferList_RemoveAt(swigCPtr, index);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveRange(int index, int count) {
    solar_datastructurePINVOKE.DescriptorBufferList_RemoveRange(swigCPtr, index, count);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public static DescriptorBufferList Repeat(DescriptorBuffer value, int count) {
    global::System.IntPtr cPtr = solar_datastructurePINVOKE.DescriptorBufferList_Repeat(DescriptorBuffer.getCPtr(value), count);
    DescriptorBufferList ret = (cPtr == global::System.IntPtr.Zero) ? null : new DescriptorBufferList(cPtr, true);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Reverse() {
    solar_datastructurePINVOKE.DescriptorBufferList_Reverse__SWIG_0(swigCPtr);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void Reverse(int index, int count) {
    solar_datastructurePINVOKE.DescriptorBufferList_Reverse__SWIG_1(swigCPtr, index, count);
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRange(int index, DescriptorBufferList values) {
    solar_datastructurePINVOKE.DescriptorBufferList_SetRange(swigCPtr, index, DescriptorBufferList.getCPtr(values));
    if (solar_datastructurePINVOKE.SWIGPendingException.Pending) throw solar_datastructurePINVOKE.SWIGPendingException.Retrieve();
  }

}

}