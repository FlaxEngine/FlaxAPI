// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    /// <summary>
    /// Base class every script derives from
    /// </summary>
    public abstract class Script : Object
    {
        /// <summary>
        /// Enable/disable script updates
        /// </summary>
        public bool Enabled
        {
            get { return Internal_GetEnabled(unmanagedPtr); }
            set { Internal_SetEnabled(unmanagedPtr, value); }
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool Internal_GetEnabled(IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetEnabled(IntPtr ptr, bool value);

        #endregion
    }
}
