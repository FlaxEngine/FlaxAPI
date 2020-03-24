// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The material slot descriptor that specifies how to render geometry using it.
    /// </summary>
    [Tooltip("The material slot descriptor that specifies how to render geometry using it.")]
    public unsafe partial class MaterialSlot : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected MaterialSlot() : base()
        {
        }

        /// <summary>
        /// The material to use for rendering.
        /// </summary>
        [Tooltip("The material to use for rendering.")]
        public MaterialBase Material
        {
            get { return Internal_GetMaterial(unmanagedPtr); }
            set { Internal_SetMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMaterial(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterial(IntPtr obj, IntPtr value);

        /// <summary>
        /// The shadows casting mode by this visual element.
        /// </summary>
        [Tooltip("The shadows casting mode by this visual element.")]
        public ShadowsCastingMode ShadowsMode
        {
            get { return Internal_GetShadowsMode(unmanagedPtr); }
            set { Internal_SetShadowsMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsMode(IntPtr obj, ShadowsCastingMode value);

        /// <summary>
        /// The slot name.
        /// </summary>
        [Tooltip("The slot name.")]
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
            set { Internal_SetName(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetName(IntPtr obj, string value);
    }
}
