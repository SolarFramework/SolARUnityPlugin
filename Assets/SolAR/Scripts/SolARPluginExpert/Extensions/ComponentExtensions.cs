using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;
using XPCF.Api;
using XPCF.Core;

namespace SolAR
{
    public static class ComponentExtensions
    {
        public static readonly Dictionary<string, string> modulesDict = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> interfacesDict = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> componentsDict = new Dictionary<string, string>();

        public static string GetUUID(string name)
        {
            string res = null;
            bool ok = false;
            ok = ok || modulesDict.TryGetValue(name, out res);
            ok = ok || interfacesDict.TryGetValue(name, out res);
            ok = ok || componentsDict.TryGetValue(name, out res);
            if (!ok) throw new System.Exception("Unknown UUID for: " + name);
            return res;
        }

        public static UUID GetUUID<T>()
        {
            var name = typeof(T).Name;
            return new UUID(GetUUID(name));
        }

        [Obsolete("Use type parameter")]
        public static IComponentIntrospect Create<T>(this IComponentManager xpcfComponentManager)
        {
            return xpcfComponentManager.createComponent(GetUUID<T>());
        }

        public static IComponentIntrospect Create(this IComponentManager xpcfComponentManager, string type)
        {
            var uuid = new UUID(GetUUID(type));
            return xpcfComponentManager.createComponent(uuid);
        }

        public static IComponentIntrospect Create(this IComponentManager xpcfComponentManager, string type, string name)
        {
            var uuid = new UUID(GetUUID(type));
            return xpcfComponentManager.createComponent(name, uuid);
        }

        public static T BindTo<T>(this IComponentIntrospect component) where T : class
        {
            return (T)component.BindTo(typeof(T).Name);
        }

        public static object BindTo(this IComponentIntrospect component, string name)
        {
            var type = typeof(solar);
            var method = type.GetMethod("bindTo_" + name, BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(method);
            return method.Invoke(null, new[] { component });
        }

        [Obsolete]
        public static T CastTo<T>(this IComponentIntrospect from, bool cMemoryOwn = false)
        {
            var type = typeof(IComponentIntrospect);
            MethodInfo getCPtr = type.GetMethod("getCPtr", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(getCPtr);
            FieldInfo swigCMemOwnBase = type.GetField("swigCMemOwnBase", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(swigCMemOwnBase);
            //cMemoryOwn = (bool) swigCMemOwnBase.GetValue(from);
            swigCMemOwnBase.SetValue(from, false);
            var cptr = (HandleRef)getCPtr.Invoke(null, new object[] { from });
            return (T)Activator.CreateInstance
            (
                typeof(T),
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { cptr.Handle, cMemoryOwn },
                null
            );
        }
    }
}
