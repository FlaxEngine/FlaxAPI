// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Flags that control hinge joint options.
    /// </summary>
    [Flags]
    public enum HingeJointFlag
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The joint limit is enabled.
        /// </summary>
        Limit = 0x1,

        /// <summary>
        /// The joint drive is enabled.
        /// </summary>
        Drive = 0x2
    }

    /// <summary>
    /// Properties of a drive that drives the joint's angular velocity towards a particular value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HingeJointDrive
    {
        /// <summary>
        /// Target velocity of the joint.
        /// </summary>
        [Limit(0)]
        public float Velocity;

        /// <summary>
        /// Maximum torque the drive is allowed to apply.
        /// </summary>
        [Limit(0)]
        public float ForceLimit;

        /// <summary>
        /// Scales the velocity of the first body, and its response to drive torque is scaled down.
        /// </summary>
        [Limit(0)]
        public float GearRatio;

        /// <summary>
        /// If the joint is moving faster than the drive's target speed, the drive will try to break.
        /// If you don't want the breaking to happen set this to true.
        /// </summary>
        public bool FreeSpin;

        /// <summary>
        /// The default <see cref="HingeJointDrive"/> structure.
        /// </summary>
        public static readonly HingeJointDrive Default = new HingeJointDrive(0.0f, float.MaxValue, 1.0f, false);

        /// <summary>
        /// Initializes a new instance of the <see cref="HingeJointDrive"/> struct.
        /// </summary>
        /// <param name="velocity">The velocity.</param>
        /// <param name="forceLimit">The force limit.</param>
        /// <param name="gearRatio">The gear ratio.</param>
        /// <param name="freeSpin">if set to <c>true</c> [free spin].</param>
        public HingeJointDrive(float velocity, float forceLimit, float gearRatio, bool freeSpin)
        {
            Velocity = velocity;
            ForceLimit = forceLimit;
            GearRatio = gearRatio;
            FreeSpin = freeSpin;
        }
    }

    public sealed partial class HingeJoint
    {
    }
}
