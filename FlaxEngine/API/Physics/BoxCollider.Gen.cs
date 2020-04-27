// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A box-shaped primitive collider.
    /// </summary>
    /// <seealso cref="Collider" />
    [Tooltip("A box-shaped primitive collider.")]
    public unsafe partial class BoxCollider : Collider
    {
        /// <inheritdoc />
        protected BoxCollider() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="BoxCollider"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static BoxCollider New()
        {
            return Internal_Create(typeof(BoxCollider)) as BoxCollider;
        }

        /// <summary>
        /// Gets or sets the size of the box, measured in the object's local space.
        /// </summary>
        /// <remarks>
        /// The box size will be scaled by the actor's world scale.
        /// </remarks>
        [EditorOrder(100), DefaultValue(typeof(Vector3), "100,100,100"), EditorDisplay("Collider")]
        [Tooltip("The size of the box, measured in the object's local space.")]
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
        /// Gets the volume bounding box (oriented).
        /// </summary>
        [Tooltip("The volume bounding box (oriented).")]
        public OrientedBoundingBox OrientedBox
        {
            get { Internal_GetOrientedBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOrientedBox(IntPtr obj, out OrientedBoundingBox resultAsRef);
    }
}
