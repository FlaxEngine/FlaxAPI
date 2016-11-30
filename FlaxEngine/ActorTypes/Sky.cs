// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Sky actor renders atmosphere around the scene with fog and sky
    /// </summary>
    public sealed class Sky : Actor
    {
        // TODO: Fog properties

        /// <summary>
        /// Gets or sets value indicating if visual element affects the world
        /// </summary>
        public bool AffectsWorld
        {
            get { return Internal_GetAffectsWorld(unmanagedPtr); }
            set { Internal_SetAffectsWorld(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets linked directional light actor that is used to simulate sun
        /// </summary>
        public DirectionalLight SunLight
        {
            get { return Internal_GetSunLight(unmanagedPtr); }
            set { Internal_SetSunLight(unmanagedPtr, GetUnmanagedPtr(value)); }
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool Internal_GetAffectsWorld(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetAffectsWorld(IntPtr obj, bool value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern DirectionalLight Internal_GetSunLight(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetSunLight(IntPtr obj, IntPtr value);

        #endregion
    }
}
