using System;
using System.Collections.Generic;

namespace SolAR
{
    public class KeyBasedEqualityComparer<T, TKey> : IEqualityComparer<T>
    {
        private readonly Func<T, TKey> keyGetter;

        public KeyBasedEqualityComparer(Func<T, TKey> keyGetter) { this.keyGetter = keyGetter; }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<TKey>.Default.Equals(keyGetter(x), keyGetter(y));
        }

        public int GetHashCode(T obj)
        {
            TKey key = keyGetter(obj);
            return key == null ? 0 : key.GetHashCode();
        }
    }
}
