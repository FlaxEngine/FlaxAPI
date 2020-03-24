// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Actor that draws the can be used to draw a custom decals on top of the other objects.
    /// </summary>
    [Tooltip("Actor that draws the can be used to draw a custom decals on top of the other objects.")]
    public unsafe partial class Decal : Actor
    {
        /// <inheritdoc />
        protected Decal() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="Decal"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static Decal New()
        {
            return Internal_Create(typeof(Decal)) as Decal;
        }

        /// <summary>
        /// The decal material. Must have domain mode to Decal type.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Decal")]
        [Tooltip("The decal material. Must have domain mode to Decal type.")]
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
        /// The decal rendering order. The higher values are render later (on top).
        /// </summary>
        [EditorOrder(20), DefaultValue(0), EditorDisplay("Decal")]
        [Tooltip("The decal rendering order. The higher values are render later (on top).")]
        public int SortOrder
        {
            get { return Internal_GetSortOrder(unmanagedPtr); }
            set { Internal_SetSortOrder(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetSortOrder(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSortOrder(IntPtr obj, int value);

        /// <summary>
        /// Gets or sets the decal bounds size (in local space).
        /// </summary>
        [EditorOrder(30), DefaultValue(typeof(Vector3), "100,100,100"), Limit(0), EditorDisplay("Decal")]
        [Tooltip("The decal bounds size (in local space).")]
        public Vector3 Size
        {
            get { Internal_GetSize(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetSize(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSize(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSize(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Utility to crate a new virtual Material Instance asset, set its parent to the currently applied material, and assign it to the decal. Can be used to modify the decal material parameters from code.
        /// </summary>
        /// <returns>The created virtual material instance.</returns>
        public MaterialInstance CreateAndSetVirtualMaterialInstance()
        {
            return Internal_CreateAndSetVirtualMaterialInstance(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialInstance Internal_CreateAndSetVirtualMaterialInstance(IntPtr obj);
    }
}
