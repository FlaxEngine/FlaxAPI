// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
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

        /// <summary>
        /// Gets actor owning that script
        /// </summary>
        public Actor Actor
        {
            get { return Internal_GetActor(unmanagedPtr); }
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool Internal_GetEnabled(IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetEnabled(IntPtr ptr, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern Actor Internal_GetActor(IntPtr ptr);

        #endregion
    }
}
