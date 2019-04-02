using System;
using System.Reflection;
using SolAR.Datastructure;
using UnityEngine.Assertions;

namespace SolAR
{
    public static class SharedPtr
    {
        [Obsolete]
        public static T Alloc<T>(int cb)
        {
            var ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(cb);
            return (T)Activator.CreateInstance
                (
                    typeof(T),
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new object[] { ptr, false },
                    null
                );
        }

        public static T Alloc<T>() where T : class
        {
            var type = typeof(T);
            var ptr = Alloc(type.Name);
            return (T)Activator.CreateInstance
                (
                    type,
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new object[] { ptr, true },
                    null
                );
        }

        public static IntPtr Alloc(string name)
        {
            var type = typeof(solar_datastructure);
            var method = type.GetMethod("newPointer_" + name, BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(method);
            return (IntPtr)method.Invoke(null, new object[] { });
        }
    }
}
