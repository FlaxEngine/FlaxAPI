// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A base class for all Joint types. Joints constrain how two rigidbodies move relative to one another (for example a door hinge).
    /// One of the bodies in the joint must always be movable (non-kinematic).
    /// </summary>
    /// <remarks>
    /// Joint constraint is created between the parent physic actor (rigidbody, character controller, etc.) and the specified target actor.
    /// </remarks>
    /// <seealso cref="Actor" />
    public abstract unsafe partial class Joint : Actor
    {
        /// <inheritdoc />
        protected Joint() : base()
        {
        }

        /// <summary>
        /// The target actor for the joint. It has to be IPhysicsActor type (eg. RigidBody or CharacterController).
        /// </summary>
        [EditorOrder(0), DefaultValue(null), EditorDisplay("Joint")]
        [Tooltip("The target actor for the joint. It has to be IPhysicsActor type (eg. RigidBody or CharacterController).")]
        public Actor Target
        {
            get { return Internal_GetTarget(unmanagedPtr); }
            set { Internal_SetTarget(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetTarget(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTarget(IntPtr obj, IntPtr value);

        /// <summary>
        /// Gets or sets the break force. Determines the maximum force the joint can apply before breaking. Broken joints no longer participate in physics simulation.
        /// </summary>
        [EditorOrder(10), DefaultValue(float.MaxValue), EditorDisplay("Joint")]
        [Tooltip("The break force. Determines the maximum force the joint can apply before breaking. Broken joints no longer participate in physics simulation.")]
        public float BreakForce
        {
            get { return Internal_GetBreakForce(unmanagedPtr); }
            set { Internal_SetBreakForce(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBreakForce(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBreakForce(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the break torque. Determines the maximum torque the joint can apply before breaking. Broken joints no longer participate in physics simulation.
        /// </summary>
        [EditorOrder(20), DefaultValue(float.MaxValue), EditorDisplay("Joint")]
        [Tooltip("The break torque. Determines the maximum torque the joint can apply before breaking. Broken joints no longer participate in physics simulation.")]
        public float BreakTorque
        {
            get { return Internal_GetBreakTorque(unmanagedPtr); }
            set { Internal_SetBreakTorque(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBreakTorque(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBreakTorque(IntPtr obj, float value);

        /// <summary>
        /// Determines whether collision between the two bodies managed by the joint are enabled.
        /// </summary>
        [EditorOrder(30), DefaultValue(true), EditorDisplay("Joint")]
        [Tooltip("Determines whether collision between the two bodies managed by the joint are enabled.")]
        public bool EnableCollision
        {
            get { return Internal_GetEnableCollision(unmanagedPtr); }
            set { Internal_SetEnableCollision(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetEnableCollision(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEnableCollision(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the target anchor.
        /// </summary>
        /// <remarks>
        /// This is the relative pose which locates the joint frame relative to the target actor.
        /// </remarks>
        [EditorOrder(40), DefaultValue(typeof(Vector3), "0,0,0"), EditorDisplay("Joint")]
        [Tooltip("The target anchor.")]
        public Vector3 TargetAnchor
        {
            get { Internal_GetTargetAnchor(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetTargetAnchor(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetTargetAnchor(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTargetAnchor(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the target anchor rotation.
        /// </summary>
        /// <remarks>
        /// This is the relative pose rotation which locates the joint frame relative to the target actor.
        /// </remarks>
        [EditorOrder(50), DefaultValue(typeof(Quaternion), "0,0,0,1"), EditorDisplay("Joint")]
        [Tooltip("The target anchor rotation.")]
        public Quaternion TargetAnchorRotation
        {
            get { Internal_GetTargetAnchorRotation(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetTargetAnchorRotation(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetTargetAnchorRotation(IntPtr obj, out Quaternion resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTargetAnchorRotation(IntPtr obj, ref Quaternion value);

        /// <summary>
        /// Gets the current force applied by the solver to maintain all constraints.
        /// </summary>
        /// <param name="linear">The result linear force.</param>
        /// <param name="angular">The result angular force.</param>
        public void GetCurrentForce(out Vector3 linear, out Vector3 angular)
        {
            Internal_GetCurrentForce(unmanagedPtr, out linear, out angular);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCurrentForce(IntPtr obj, out Vector3 linear, out Vector3 angular);
    }
}
