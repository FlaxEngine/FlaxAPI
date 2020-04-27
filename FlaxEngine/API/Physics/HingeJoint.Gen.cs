// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Flags that control hinge joint options.
    /// </summary>
    [Flags]
    [Tooltip("Flags that control hinge joint options.")]
    public enum HingeJointFlag
    {
        /// <summary>
        /// The none.
        /// </summary>
        [Tooltip("The none.")]
        None = 0,

        /// <summary>
        /// The joint limit is enabled.
        /// </summary>
        [Tooltip("The joint limit is enabled.")]
        Limit = 0x1,

        /// <summary>
        /// The joint drive is enabled.
        /// </summary>
        [Tooltip("The joint drive is enabled.")]
        Drive = 0x2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Properties of a drive that drives the joint's angular velocity towards a paricular value.
    /// </summary>
    [Tooltip("Properties of a drive that drives the joint's angular velocity towards a paricular value.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct HingeJointDrive
    {
        /// <summary>
        /// Target velocity of the joint.
        /// </summary>
        [Limit(0)]
        [Tooltip("Target velocity of the joint.")]
        public float Velocity;

        /// <summary>
        /// Maximum torque the drive is allowed to apply.
        /// </summary>
        [Limit(0)]
        [Tooltip("Maximum torque the drive is allowed to apply.")]
        public float ForceLimit;

        /// <summary>
        /// Scales the velocity of the first body, and its response to drive torque is scaled down.
        /// </summary>
        [Limit(0)]
        [Tooltip("Scales the velocity of the first body, and its response to drive torque is scaled down.")]
        public float GearRatio;

        /// <summary>
        /// If the joint is moving faster than the drive's target speed, the drive will try to break.
        /// If you don't want the breaking to happen set this to true.
        /// </summary>
        public bool FreeSpin;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Physics joint that removes all but a single rotation degree of freedom from its two attached bodies (for example a door hinge).
    /// </summary>
    /// <seealso cref="Joint" />
    [Tooltip("Physics joint that removes all but a single rotation degree of freedom from its two attached bodies (for example a door hinge).")]
    public unsafe partial class HingeJoint : Joint
    {
        /// <inheritdoc />
        protected HingeJoint() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HingeJoint"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static HingeJoint New()
        {
            return Internal_Create(typeof(HingeJoint)) as HingeJoint;
        }

        /// <summary>
        /// Gets or sets the joint mode flags. Controls joint behaviour.
        /// </summary>
        [EditorOrder(100), DefaultValue(HingeJointFlag.Limit | HingeJointFlag.Drive)]
        [Tooltip("The joint mode flags. Controls joint behaviour.")]
        public HingeJointFlag Flags
        {
            get { return Internal_GetFlags(unmanagedPtr); }
            set { Internal_SetFlags(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern HingeJointFlag Internal_GetFlags(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFlags(IntPtr obj, HingeJointFlag value);

        /// <summary>
        /// Gets or sets the joint limit properties.
        /// </summary>
        /// <remarks>
        /// Determines the limit of the joint. Limit constrains the motion to the specified angle range. You must enable the limit flag on the joint in order for this to be recognized.
        /// </remarks>
        [EditorOrder(110), EditorDisplay("Joint")]
        [Tooltip("The joint limit properties.")]
        public LimitAngularRange Limit
        {
            get { Internal_GetLimit(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLimit(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimit(IntPtr obj, out LimitAngularRange resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimit(IntPtr obj, ref LimitAngularRange value);

        /// <summary>
        /// Gets or sets the joint drive properties.
        /// </summary>
        /// <remarks>
        /// Determines the drive properties of the joint. It drives the joint's angular velocity towards a particular value. You must enable the drive flag on the joint in order for the drive to be active.
        /// </remarks>
        [EditorOrder(120), EditorDisplay("Joint")]
        [Tooltip("The joint drive properties.")]
        public HingeJointDrive Drive
        {
            get { Internal_GetDrive(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDrive(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDrive(IntPtr obj, out HingeJointDrive resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrive(IntPtr obj, ref HingeJointDrive value);

        /// <summary>
        /// Gets the current angle of the joint (in radians, in the range (-Pi, Pi]).
        /// </summary>
        [Tooltip("The current angle of the joint (in radians, in the range (-Pi, Pi]).")]
        public float CurrentAngle
        {
            get { return Internal_GetCurrentAngle(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentAngle(IntPtr obj);

        /// <summary>
        /// Gets the current velocity of the joint.
        /// </summary>
        [Tooltip("The current velocity of the joint.")]
        public float CurrentVelocity
        {
            get { return Internal_GetCurrentVelocity(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentVelocity(IntPtr obj);
    }
}
