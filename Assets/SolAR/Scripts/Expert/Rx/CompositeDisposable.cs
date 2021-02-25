
#if !UniRx

using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
    public class CompositeDisposable : ICollection<IDisposable>, IDisposable
    {
        readonly List<IDisposable> list = new List<IDisposable>();

        public CompositeDisposable() { }

        public void Dispose() => Clear();

        public void Clear()
        {
            list.ForEach(d => d.Dispose());
            list.Clear();
        }

        bool ICollection<IDisposable>.Remove(IDisposable item)
        {
            item.Dispose();
            return list.Remove(item);
        }

        int ICollection<IDisposable>.Count => list.Count;
        void ICollection<IDisposable>.Add(IDisposable item) => list.Add(item);
        bool ICollection<IDisposable>.Contains(IDisposable item) => list.Contains(item);
        bool ICollection<IDisposable>.IsReadOnly => false;
        void ICollection<IDisposable>.CopyTo(IDisposable[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        IEnumerator<IDisposable> IEnumerable<IDisposable>.GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
    }
}
#endif
