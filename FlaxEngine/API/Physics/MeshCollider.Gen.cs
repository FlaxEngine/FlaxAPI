// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A collider represented by an arbitrary mesh.
    /// </summary>
    /// <seealso cref="Collider" />
    [Tooltip("A collider represented by an arbitrary mesh.")]
    public unsafe partial class MeshCollider : Collider
    {
        /// <inheritdoc />
        protected MeshCollider() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="MeshCollider"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static MeshCollider New()
        {
            return Internal_Create(typeof(MeshCollider)) as MeshCollider;
        }

        /// <summary>
        /// Linked collision data asset that contains convex mesh or triangle mesh used to represent a mesh collider shape.
        /// </summary>
        [EditorOrder(100), DefaultValue(null), EditorDisplay("Collider")]
        [Tooltip("Linked collision data asset that contains convex mesh or triangle mesh used to represent a mesh collider shape.")]
        public CollisionData CollisionData
        {
            get { return Internal_GetCollisionData(unmanagedPtr); }
            set { Internal_SetCollisionData(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CollisionData Internal_GetCollisionData(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCollisionData(IntPtr obj, IntPtr value);
    }
}
