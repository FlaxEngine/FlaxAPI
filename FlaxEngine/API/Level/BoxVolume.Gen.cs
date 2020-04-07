// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A base class for actors that define 3D bounding box volume.
    /// </summary>
    [Tooltip("A base class for actors that define 3D bounding box volume.")]
    public abstract unsafe partial class BoxVolume : Actor
    {
        /// <inheritdoc />
        protected BoxVolume() : base()
        {
        }

        /// <summary>
        /// Gets or sets the size of the volume (in local space).
        /// </summary>
        [EditorDisplay("Box Volume"), DefaultValue(typeof(Vector3), "1000,1000,1000"), EditorOrder(0)]
        [Tooltip("The size of the volume (in local space).")]
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
        /// Gets the volume bounding box (oriented in world space).
        /// </summary>
        [Tooltip("The volume bounding box (oriented in world space).")]
        public OrientedBoundingBox OrientedBox
        {
            get { Internal_GetOrientedBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOrientedBox(IntPtr obj, out OrientedBoundingBox resultAsRef);
    }
}
