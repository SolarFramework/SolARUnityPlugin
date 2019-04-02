
#if !UniRx

using System;
using System.Collections.Generic;

namespace UniRx
{
    public static class Disposable
    {
        public static readonly IDisposable Empty = Create(() => { });

        public static IDisposable Create(Action action) { return new DisposableAction(action); }
        class DisposableAction : IDisposable
        {
            readonly Action action;
            public DisposableAction(Action action) { this.action = action; }
            public void Dispose() { action(); }
        }

        public static T AddTo<T>(this T disposable, CompositeDisposable collection) where T : IDisposable
        {
            ((ICollection<IDisposable>)collection).Add(disposable);
            return disposable;
        }
    }
}

#endif
