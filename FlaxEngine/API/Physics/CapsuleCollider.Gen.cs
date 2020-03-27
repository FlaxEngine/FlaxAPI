// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A capsule-shaped primitive collider.
    /// </summary>
    /// <remarks>
    /// Capsules are cylinders with a half-sphere at each end.
    /// </remarks>
    /// <seealso cref="Collider" />
    [Tooltip("A capsule-shaped primitive collider.")]
    public unsafe partial class CapsuleCollider : Collider
    {
        /// <inheritdoc />
        protected CapsuleCollider() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="CapsuleCollider"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static CapsuleCollider New()
        {
            return Internal_Create(typeof(CapsuleCollider)) as CapsuleCollider;
        }

        /// <summary>
        /// Gets or sets the radius of the sphere, measured in the object's local space.
        /// </summary>
        /// <remarks>
        /// The sphere radius will be scaled by the actor's world scale.
        /// </remarks>
        [EditorOrder(100), DefaultValue(20.0f), EditorDisplay("Collider")]
        [Tooltip("The radius of the sphere, measured in the object's local space.")]
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
            set { Internal_SetRadius(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRadius(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the height of the capsule, measured in the object's local space.
        /// </summary>
        /// <remarks>
        /// The capsule height will be scaled by the actor's world scale.
        /// </remarks>
        [EditorOrder(110), DefaultValue(100.0f), EditorDisplay("Collider")]
        [Tooltip("The height of the capsule, measured in the object's local space.")]
        public float Height
        {
            get { return Internal_GetHeight(unmanagedPtr); }
            set { Internal_SetHeight(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetHeight(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetHeight(IntPtr obj, float value);
    }
}
