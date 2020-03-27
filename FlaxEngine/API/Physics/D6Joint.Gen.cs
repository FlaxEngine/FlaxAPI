// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies axes that the D6 joint can constrain motion on.
    /// </summary>
    [Tooltip("Specifies axes that the D6 joint can constrain motion on.")]
    public enum D6JointAxis
    {
        /// <summary>
        /// Movement on the X axis.
        /// </summary>
        [Tooltip("Movement on the X axis.")]
        X = 0,

        /// <summary>
        /// Movement on the Y axis.
        /// </summary>
        [Tooltip("Movement on the Y axis.")]
        Y = 1,

        /// <summary>
        /// Movement on the Z axis.
        /// </summary>
        [Tooltip("Movement on the Z axis.")]
        Z = 2,

        /// <summary>
        /// Rotation around the X axis.
        /// </summary>
        [Tooltip("Rotation around the X axis.")]
        Twist = 3,

        /// <summary>
        /// Rotation around the Y axis.
        /// </summary>
        [Tooltip("Rotation around the Y axis.")]
        SwingY = 4,

        /// <summary>
        /// Rotation around the Z axis.
        /// </summary>
        [Tooltip("Rotation around the Z axis.")]
        SwingZ = 5,

        /// <summary>
        /// The count of items in the D6JointAxis enum.
        /// </summary>
        [Tooltip("The count of items in the D6JointAxis enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Specifies type of constraint placed on a specific axis.
    /// </summary>
    [Tooltip("Specifies type of constraint placed on a specific axis.")]
    public enum D6JointMotion
    {
        /// <summary>
        /// Axis is immovable.
        /// </summary>
        [Tooltip("Axis is immovable.")]
        Locked,

        /// <summary>
        /// Axis will be constrained by the specified limits.
        /// </summary>
        [Tooltip("Axis will be constrained by the specified limits.")]
        Limited,

        /// <summary>
        /// Axis will not be constrained.
        /// </summary>
        [Tooltip("Axis will not be constrained.")]
        Free,

        /// <summary>
        /// The count of items in the D6JointMotion enum.
        /// </summary>
        [Tooltip("The count of items in the D6JointMotion enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Type of drives that can be used for moving or rotating bodies attached to the joint.
    /// </summary>
    /// <remarks>
    /// Each drive is an implicit force-limited damped spring:
    /// force = spring * (target position - position) + damping * (targetVelocity - velocity)
    ///
    /// Alternatively, the spring may be configured to generate a specified acceleration instead of a force.
    ///
    /// A linear axis is affected by drive only if the corresponding drive flag is set.There are two possible models
    /// for angular drive : swing / twist, which may be used to drive one or more angular degrees of freedom, or slerp,
    /// which may only be used to drive all three angular degrees simultaneously.
    /// </remarks>
    [Tooltip("Type of drives that can be used for moving or rotating bodies attached to the joint.")]
    public enum D6JointDriveType
    {
        /// <summary>
        /// Linear movement on the X axis using the linear drive model.
        /// </summary>
        [Tooltip("Linear movement on the X axis using the linear drive model.")]
        X = 0,

        /// <summary>
        /// Linear movement on the Y axis using the linear drive model.
        /// </summary>
        [Tooltip("Linear movement on the Y axis using the linear drive model.")]
        Y = 1,

        /// <summary>
        /// Linear movement on the Z axis using the linear drive model.
        /// </summary>
        [Tooltip("Linear movement on the Z axis using the linear drive model.")]
        Z = 2,

        /// <summary>
        /// Rotation around the Y axis using the twist/swing angular drive model. Should not be used together with Slerp mode.
        /// </summary>
        [Tooltip("Rotation around the Y axis using the twist/swing angular drive model. Should not be used together with Slerp mode.")]
        Swing = 3,

        /// <summary>
        /// Rotation around the Z axis using the twist/swing angular drive model. Should not be used together with Slerp mode.
        /// </summary>
        [Tooltip("Rotation around the Z axis using the twist/swing angular drive model. Should not be used together with Slerp mode.")]
        Twist = 4,

        /// <summary>
        /// Rotation using spherical linear interpolation. Uses the SLERP angular drive mode which performs rotation
        /// by interpolating the quaternion values directly over the shortest path (applies to all three axes, which
        /// they all must be unlocked).
        /// </summary>
        Slerp = 5,

        /// <summary>
        /// The count of items in the D6JointDriveType enum.
        /// </summary>
        [Tooltip("The count of items in the D6JointDriveType enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Specifies parameters for a drive that will attempt to move the joint bodies to the specified drive position and velocity.
    /// </summary>
    [Tooltip("Specifies parameters for a drive that will attempt to move the joint bodies to the specified drive position and velocity.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct D6JointDrive
    {
        /// <summary>
        /// The spring strength. Force proportional to the position error.
        /// </summary>
        [Tooltip("The spring strength. Force proportional to the position error.")]
        public float Stiffness;

        /// <summary>
        /// Damping strength. Force proportional to the velocity error.
        /// </summary>
        [Tooltip("Damping strength. Force proportional to the velocity error.")]
        public float Damping;

        /// <summary>
        /// The maximum force the drive can apply.
        /// </summary>
        [Tooltip("The maximum force the drive can apply.")]
        public float ForceLimit;

        /// <summary>
        /// If true the drive will generate acceleration instead of forces. Acceleration drives are easier to tune as they account for the masses of the actors to which the joint is attached.
        /// </summary>
        [Tooltip("If true the drive will generate acceleration instead of forces. Acceleration drives are easier to tune as they account for the masses of the actors to which the joint is attached.")]
        public bool Acceleration;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Physics joint that is the most customizable type of joint. This joint type can be used to create all other built-in joint
    /// types, and to design your own custom ones, but is less intuitive to use.Allows a specification of a linear
    /// constraint (for example for a slider), twist constraint (rotating around X) and swing constraint (rotating around Y and Z).
    /// It also allows you to constrain limits to only specific axes or completely lock specific axes.
    /// </summary>
    /// <seealso cref="Joint" />
    public unsafe partial class D6Joint : Joint
    {
        /// <inheritdoc />
        protected D6Joint() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="D6Joint"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static D6Joint New()
        {
            return Internal_Create(typeof(D6Joint)) as D6Joint;
        }

        /// <summary>
        /// Determines the linear limit used for constraining translation degrees of freedom.
        /// </summary>
        [EditorOrder(200), EditorDisplay("Joint")]
        [Tooltip("Determines the linear limit used for constraining translation degrees of freedom.")]
        public LimitLinear LimitLinear
        {
            get { Internal_GetLimitLinear(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLimitLinear(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimitLinear(IntPtr obj, out LimitLinear resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimitLinear(IntPtr obj, ref LimitLinear value);

        /// <summary>
        /// Determines the angular limit used for constraining the twist (rotation around X) degree of freedom.
        /// </summary>
        [EditorOrder(210), EditorDisplay("Joint")]
        [Tooltip("Determines the angular limit used for constraining the twist (rotation around X) degree of freedom.")]
        public LimitAngularRange LimitTwist
        {
            get { Internal_GetLimitTwist(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLimitTwist(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimitTwist(IntPtr obj, out LimitAngularRange resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimitTwist(IntPtr obj, ref LimitAngularRange value);

        /// <summary>
        /// Determines the cone limit used for constraining the swing (rotation around Y and Z) degree of freedom.
        /// </summary>
        [EditorOrder(220), EditorDisplay("Joint")]
        [Tooltip("Determines the cone limit used for constraining the swing (rotation around Y and Z) degree of freedom.")]
        public LimitConeRange LimitSwing
        {
            get { Internal_GetLimitSwing(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLimitSwing(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimitSwing(IntPtr obj, out LimitConeRange resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimitSwing(IntPtr obj, ref LimitConeRange value);

        /// <summary>
        /// Gets or sets the drive's target position relative to the joint's first body.
        /// </summary>
        [HideInEditor]
        [Tooltip("The drive's target position relative to the joint's first body.")]
        public Vector3 DrivePosition
        {
            get { Internal_GetDrivePosition(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDrivePosition(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDrivePosition(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrivePosition(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the drive's target rotation relative to the joint's first body.
        /// </summary>
        [HideInEditor]
        [Tooltip("The drive's target rotation relative to the joint's first body.")]
        public Quaternion DriveRotation
        {
            get { Internal_GetDriveRotation(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDriveRotation(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDriveRotation(IntPtr obj, out Quaternion resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDriveRotation(IntPtr obj, ref Quaternion value);

        /// <summary>
        /// Gets or sets the drive's target linear velocity.
        /// </summary>
        [HideInEditor]
        [Tooltip("The drive's target linear velocity.")]
        public Vector3 DriveLinearVelocity
        {
            get { Internal_GetDriveLinearVelocity(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDriveLinearVelocity(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDriveLinearVelocity(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDriveLinearVelocity(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the drive's target angular velocity.
        /// </summary>
        [HideInEditor]
        [Tooltip("The drive's target angular velocity.")]
        public Vector3 DriveAngularVelocity
        {
            get { Internal_GetDriveAngularVelocity(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDriveAngularVelocity(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDriveAngularVelocity(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDriveAngularVelocity(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets the twist angle of the joint.
        /// </summary>
        [Tooltip("The twist angle of the joint.")]
        public float CurrentTwist
        {
            get { return Internal_GetCurrentTwist(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentTwist(IntPtr obj);

        /// <summary>
        /// Gets the current swing angle of the joint from the Y axis.
        /// </summary>
        [Tooltip("The current swing angle of the joint from the Y axis.")]
        public float CurrentSwingYAngle
        {
            get { return Internal_GetCurrentSwingYAngle(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentSwingYAngle(IntPtr obj);

        /// <summary>
        /// Gets the current swing angle of the joint from the Z axis.
        /// </summary>
        [Tooltip("The current swing angle of the joint from the Z axis.")]
        public float CurrentSwingZAngle
        {
            get { return Internal_GetCurrentSwingZAngle(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentSwingZAngle(IntPtr obj);

        /// <summary>
        /// Gets the motion type around the specified axis.
        /// </summary>
        /// <remarks>
        /// Each axis may independently specify that the degree of freedom is locked (blocking relative movement along or around this axis), limited by the corresponding limit, or free.
        /// </remarks>
        /// <param name="axis">The axis the degree of freedom around which the motion type is specified.</param>
        /// <returns>The value.</returns>
        public D6JointMotion GetMotion(D6JointAxis axis)
        {
            return Internal_GetMotion(unmanagedPtr, axis);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern D6JointMotion Internal_GetMotion(IntPtr obj, D6JointAxis axis);

        /// <summary>
        /// Sets the motion type around the specified axis.
        /// </summary>
        /// <remarks>
        /// Each axis may independently specify that the degree of freedom is locked (blocking relative movement along or around this axis), limited by the corresponding limit, or free.
        /// </remarks>
        /// <param name="axis">The axis the degree of freedom around which the motion type is specified.</param>
        /// <param name="value">The value.</param>
        public void SetMotion(D6JointAxis axis, D6JointMotion value)
        {
            Internal_SetMotion(unmanagedPtr, axis, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMotion(IntPtr obj, D6JointAxis axis, D6JointMotion value);

        /// <summary>
        /// Gets the drive parameters for the specified drive type.
        /// </summary>
        /// <param name="index">The type of drive being specified.</param>
        /// <returns>The value.</returns>
        public D6JointDrive GetDrive(D6JointDriveType index)
        {
            Internal_GetDrive(unmanagedPtr, index, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDrive(IntPtr obj, D6JointDriveType index, out D6JointDrive resultAsRef);

        /// <summary>
        /// Sets the drive parameters for the specified drive type.
        /// </summary>
        /// <param name="index">The type of drive being specified.</param>
        /// <param name="value">The value.</param>
        public void SetDrive(D6JointDriveType index, D6JointDrive value)
        {
            Internal_SetDrive(unmanagedPtr, index, ref value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrive(IntPtr obj, D6JointDriveType index, ref D6JointDrive value);
    }
}
