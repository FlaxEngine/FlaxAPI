// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A base class for all physical collider actors.
    /// </summary>
    /// <seealso cref="Actor" />
    [Tooltip("A base class for all physical collider actors.")]
    public abstract unsafe partial class PhysicsColliderActor : Actor
    {
        /// <inheritdoc />
        protected PhysicsColliderActor() : base()
        {
        }

        /// <summary>
        /// Gets the attached rigid body.
        /// </summary>
        [Tooltip("The attached rigid body.")]
        public RigidBody AttachedRigidBody
        {
            get { return Internal_GetAttachedRigidBody(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RigidBody Internal_GetAttachedRigidBody(IntPtr obj);
    }
}
