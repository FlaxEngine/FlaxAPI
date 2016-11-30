// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Environment Probe can capture space around the objects to provide reflections
    /// </summary>
    public sealed class EnvironmentProbe : Actor
    {
        // TODO: HasProbe, IsUsingCustomProbe, SetCustomprobe
        // TODO: Bake()

        /// <summary>
        /// Gets or sets value indicating if visual element affects the world
        /// </summary>
        public bool AffectsWorld
        {
            get { return Internal_GetAffectsWorld(unmanagedPtr); }
            set { Internal_SetAffectsWorld(unmanagedPtr, value); }
        }
        
        /// <summary>
        /// Gets or sets probe brightness parameter
        /// </summary>
        public float Brightness
        {
            get { return Internal_GetBrightness(unmanagedPtr); }
            set { Internal_SetBrightness(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets probe radius parameter
        /// </summary>
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
            set { Internal_SetRadius(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets probe scaled radius parameter
        /// </summary>
        public float ScaledRadius
        {
            get { return Scale.MaxValue * Radius; }
        }
        
        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool Internal_GetAffectsWorld(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetAffectsWorld(IntPtr obj, bool value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetBrightness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetBrightness(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetRadius(IntPtr obj, float value);
                
        #endregion
    }
}
