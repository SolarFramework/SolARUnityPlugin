using System;
using UniRx;
using UnityEngine;

namespace SolAR
{
    public static class GUIScope
    {
        static int CHANGE_COUNT;
        static int CHANGE_DEPTH = int.MinValue;

        /// Scope class that can be used to change the current changed state
        public static IDisposable ChangeCheck
        {
            get
            {
                if (GUI.changed) { CHANGE_DEPTH = CHANGE_COUNT; }
                ++CHANGE_COUNT;
                GUI.changed = false;
                return _ChangeCheck;
            }
        }

        static readonly IDisposable _ChangeCheck = Disposable.Create(() =>
        {
            bool changed = GUI.changed;
            --CHANGE_COUNT;
            if (changed) { CHANGE_DEPTH = int.MinValue; }
            else { GUI.changed = CHANGE_DEPTH == CHANGE_COUNT; }
        });

        static int GUI_DISABLE_COUNT;

        public static IDisposable Disable
        {
            get
            {
                // Because we shouldn't use "enbaled=true" inside "enabled=false" area,
                // we could juste count the number of call to "GUI.enabled=false"
                ++GUI_DISABLE_COUNT;
                GUI.enabled = false;
                return _Disable;
            }
        }

        static readonly IDisposable _Disable = Disposable.Create(() =>
        {
            --GUI_DISABLE_COUNT;
            if (GUI_DISABLE_COUNT <= 0) { GUI.enabled = true; }
        });

        /// Scope class that can be used to change the current enabled state
        public static IDisposable Enable(bool enabled)
        {
            return (!enabled || GUI_DISABLE_COUNT > 0) ? Disable : Disposable.Empty;
        }
    }
}
