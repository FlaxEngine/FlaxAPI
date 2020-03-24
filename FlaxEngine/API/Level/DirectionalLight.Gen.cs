// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Directional light emits light from direction in space.
    /// </summary>
    [Tooltip("Directional light emits light from direction in space.")]
    public unsafe partial class DirectionalLight : LightWithShadow
    {
        /// <inheritdoc />
        protected DirectionalLight() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="DirectionalLight"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static DirectionalLight New()
        {
            return Internal_Create(typeof(DirectionalLight)) as DirectionalLight;
        }

        /// <summary>
        /// The number of cascades used for slicing the range of depth covered by the light. Values are 1, 2 or 4 cascades; a typical scene uses 4 cascades.
        /// </summary>
        [EditorOrder(65), DefaultValue(4), Limit(1, 4), EditorDisplay("Shadow")]
        [Tooltip("The number of cascades used for slicing the range of depth covered by the light. Values are 1, 2 or 4 cascades; a typical scene uses 4 cascades.")]
        public int CascadeCount
        {
            get { return Internal_GetCascadeCount(unmanagedPtr); }
            set { Internal_SetCascadeCount(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetCascadeCount(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCascadeCount(IntPtr obj, int value);
    }
}
