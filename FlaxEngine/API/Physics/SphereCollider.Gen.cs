// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A sphere-shaped primitive collider.
    /// </summary>
    /// <seealso cref="Collider" />
    [Tooltip("A sphere-shaped primitive collider.")]
    public unsafe partial class SphereCollider : Collider
    {
        /// <inheritdoc />
        protected SphereCollider() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SphereCollider"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static SphereCollider New()
        {
            return Internal_Create(typeof(SphereCollider)) as SphereCollider;
        }

        /// <summary>
        /// Gets or sets the radius of the sphere, measured in the object's local space.
        /// </summary>
        /// <remarks>
        /// The sphere radius will be scaled by the actor's world scale.
        /// </remarks>
        [EditorOrder(100), DefaultValue(50.0f), EditorDisplay("Collider")]
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
    }
}
