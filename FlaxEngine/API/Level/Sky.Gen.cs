// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Sky actor renders atmosphere around the scene with fog and sky.
    /// </summary>
    [Tooltip("Sky actor renders atmosphere around the scene with fog and sky.")]
    public unsafe partial class Sky : Actor
    {
        /// <inheritdoc />
        protected Sky() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="Sky"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static Sky New()
        {
            return Internal_Create(typeof(Sky)) as Sky;
        }

        /// <summary>
        /// Directional light that is used to simulate the sun.
        /// </summary>
        [EditorOrder(10), DefaultValue(null), EditorDisplay("Sun")]
        [Tooltip("Directional light that is used to simulate the sun.")]
        public DirectionalLight SunLight
        {
            get { return Internal_GetSunLight(unmanagedPtr); }
            set { Internal_SetSunLight(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DirectionalLight Internal_GetSunLight(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSunLight(IntPtr obj, IntPtr value);

        /// <summary>
        /// The sun disc scale.
        /// </summary>
        [EditorOrder(20), DefaultValue(2.0f), EditorDisplay("Sun"), Limit(0, 100, 0.01f)]
        [Tooltip("The sun disc scale.")]
        public float SunDiscScale
        {
            get { return Internal_GetSunDiscScale(unmanagedPtr); }
            set { Internal_SetSunDiscScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSunDiscScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSunDiscScale(IntPtr obj, float value);

        /// <summary>
        /// The sun power.
        /// </summary>
        [EditorOrder(30), DefaultValue(8.0f), EditorDisplay("Sun"), Limit(0, 1000, 0.01f)]
        [Tooltip("The sun power.")]
        public float SunPower
        {
            get { return Internal_GetSunPower(unmanagedPtr); }
            set { Internal_SetSunPower(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSunPower(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSunPower(IntPtr obj, float value);
    }
}
