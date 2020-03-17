// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Contains IES profile texture used by the lights to simulate real life bulb light emission.
    /// </summary>
    [Tooltip("Contains IES profile texture used by the lights to simulate real life bulb light emission.")]
    public unsafe partial class IESProfile : TextureBase
    {
        /// <inheritdoc />
        protected IESProfile() : base()
        {
        }

        /// <summary>
        /// The light brightness in Lumens, imported from IES profile.
        /// </summary>
        [Tooltip("The light brightness in Lumens, imported from IES profile.")]
        public float Brightness
        {
            get { return Internal_GetBrightness(unmanagedPtr); }
            set { Internal_SetBrightness(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBrightness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBrightness(IntPtr obj, float value);

        /// <summary>
        /// The multiplier to map texture value to result to integrate over the sphere to 1.
        /// </summary>
        [Tooltip("The multiplier to map texture value to result to integrate over the sphere to 1.")]
        public float TextureMultiplier
        {
            get { return Internal_GetTextureMultiplier(unmanagedPtr); }
            set { Internal_SetTextureMultiplier(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTextureMultiplier(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTextureMultiplier(IntPtr obj, float value);
    }
}
