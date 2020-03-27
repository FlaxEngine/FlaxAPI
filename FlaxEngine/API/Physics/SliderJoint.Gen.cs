// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Flags that control slider joint options.
    /// </summary>
    [Flags]
    [Tooltip("Flags that control slider joint options.")]
    public enum SliderJointFlag
    {
        /// <summary>
        /// The none.
        /// </summary>
        [Tooltip("The none.")]
        None = 0,

        /// <summary>
        /// The joint linear range limit is enabled.
        /// </summary>
        [Tooltip("The joint linear range limit is enabled.")]
        Limit = 0x1,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Physics joint that removes all but a single translational degree of freedom. Bodies are allowed to move along a single axis.
    /// </summary>
    /// <seealso cref="Joint" />
    [Tooltip("Physics joint that removes all but a single translational degree of freedom. Bodies are allowed to move along a single axis.")]
    public unsafe partial class SliderJoint : Joint
    {
        /// <inheritdoc />
        protected SliderJoint() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SliderJoint"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static SliderJoint New()
        {
            return Internal_Create(typeof(SliderJoint)) as SliderJoint;
        }

        /// <summary>
        /// Gets or sets the joint mode flags. Controls joint behaviour.
        /// </summary>
        [EditorOrder(100), DefaultValue(SliderJointFlag.Limit), EditorDisplay("Joint")]
        [Tooltip("The joint mode flags. Controls joint behaviour.")]
        public SliderJointFlag Flags
        {
            get { return Internal_GetFlags(unmanagedPtr); }
            set { Internal_SetFlags(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SliderJointFlag Internal_GetFlags(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFlags(IntPtr obj, SliderJointFlag value);

        /// <summary>
        /// Gets or sets the joint limit properties.
        /// </summary>
        /// <remarks>
        /// Determines the limit of the joint. Limit constrains the motion to the specified angle range. You must enable the limit flag on the joint in order for this to be recognized.
        /// </remarks>
        [EditorOrder(110), EditorDisplay("Joint")]
        [Tooltip("The joint limit properties.")]
        public LimitLinearRange Limit
        {
            get { Internal_GetLimit(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLimit(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimit(IntPtr obj, out LimitLinearRange resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimit(IntPtr obj, ref LimitLinearRange value);

        /// <summary>
        /// Gets the current displacement of the joint along its axis.
        /// </summary>
        [Tooltip("The current displacement of the joint along its axis.")]
        public float CurrentPosition
        {
            get { return Internal_GetCurrentPosition(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentPosition(IntPtr obj);

        /// <summary>
        /// Gets the current velocity of the joint along its axis.
        /// </summary>
        [Tooltip("The current velocity of the joint along its axis.")]
        public float CurrentVelocity
        {
            get { return Internal_GetCurrentVelocity(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentVelocity(IntPtr obj);
    }
}
