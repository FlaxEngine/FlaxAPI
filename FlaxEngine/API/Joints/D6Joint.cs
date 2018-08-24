// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies axes that the D6 joint can constrain motion on.
    /// </summary>
    public enum D6JointAxis
    {
        /// <summary>
        /// Movement on the X axis.
        /// </summary>
        X = 0,

        /// <summary>
        /// Movement on the Y axis.
        /// </summary>
        Y = 1,

        /// <summary>
        /// Movement on the Z axis.
        /// </summary>
        Z = 2,

        /// <summary>
        /// Rotation around the X axis.
        /// </summary>
        Twist = 3,

        /// <summary>
        /// Rotation around the Y axis.
        /// </summary>
        SwingY = 4,

        /// <summary>
        /// Rotation around the Z axis.
        /// </summary>
        SwingZ = 5
    }

    /// <summary>
    /// Specifies type of constraint placed on a specific axis.
    /// </summary>
    public enum D6JointMotion
    {
        /// <summary>
        /// Axis is immovable.
        /// </summary>
        Locked,

        /// <summary>
        /// Axis will be constrained by the specified limits.
        /// </summary>
        Limited,

        /// <summary>
        /// Axis will not be constrained.
        /// </summary>
        Free
    }

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
    public enum D6JointDriveType
    {
        /// <summary>
        /// Linear movement on the X axis using the linear drive model.
        /// </summary>
        X = 0,

        /// <summary>
        /// Linear movement on the Y axis using the linear drive model.
        /// </summary>
        Y = 1,

        /// <summary>
        /// Linear movement on the Z axis using the linear drive model.
        /// </summary>
        Z = 2,

        /// <summary>
        /// Rotation around the Y axis using the twist/swing angular drive model. Should not be used together with Slerp mode.
        /// </summary>
        Swing = 3,

        /// <summary>
        /// Rotation around the Z axis using the twist/swing angular drive model. Should not be used together with Slerp mode.
        /// </summary>
        Twist = 4,

        /// <summary>
        /// Rotation using spherical linear interpolation. Uses the SLERP angular drive mode which performs rotation
        /// by interpolating the quaternion values directly over the shortest path (applies to all three axes, which
        /// they all must be unlocked).
        /// </summary>
        Slerp = 5
    }

    /// <summary>
    /// Specifies parameters for a drive that will attempt to move the joint bodies to the specified drive position and velocity.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct D6JointDrive
    {
        /// <summary>
        /// The spring strength. Force proportional to the position error.
        /// </summary>
        [EditorOrder(0), Limit(0.0f), Tooltip("The spring strength. Force proportional to the position error.")]
        public float Stiffness;

        /// <summary>
        /// Damping strength. Force proportional to the velocity error.
        /// </summary>
        [EditorOrder(10), Limit(0.0f), Tooltip("Damping strength. Force proportional to the velocity error.")]
        public float Damping;

        /// <summary>
        /// The maximum force the drive can apply.
        /// </summary>
        [EditorOrder(20), Limit(0.0f), Tooltip("The maximum force the drive can apply.")]
        public float ForceLimit;

        /// <summary>
        /// If true the drive will generate acceleration instead of forces. Acceleration drives are easier to tune as
        /// they account for the masses of the actors to which the joint is attached.
        /// </summary>
        [EditorOrder(30), Tooltip("If true the drive will generate acceleration instead of forces. Acceleration drives are easier to tune as they account for the masses of the actors to which the joint is attached.")]
        public bool Acceleration;

        /// <summary>
        /// The default <see cref="D6JointDrive"/> structure.
        /// </summary>
        public static readonly D6JointDrive Default = new D6JointDrive(0.0f, 0.0f, float.MaxValue, false);

        /// <summary>
        /// Initializes a new instance of the <see cref="D6JointDrive"/> struct.
        /// </summary>
        /// <param name="stiffness">The stiffness.</param>
        /// <param name="damping">The damping.</param>
        /// <param name="forceLimit">The force limit.</param>
        /// <param name="acceleration">if set to <c>true</c> the drive will generate acceleration instead of forces.</param>
        public D6JointDrive(float stiffness, float damping, float forceLimit, bool acceleration)
        {
            Stiffness = stiffness;
            Damping = damping;
            ForceLimit = forceLimit;
            Acceleration = acceleration;
        }
    }

    public sealed partial class D6Joint
    {
    }
}
