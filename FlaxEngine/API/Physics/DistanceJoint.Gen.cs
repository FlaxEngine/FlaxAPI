// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Controls distance joint options.
    /// </summary>
    [Flags]
    [Tooltip("Controls distance joint options.")]
    public enum DistanceJointFlag
    {
        /// <summary>
        /// The none limits.
        /// </summary>
        [Tooltip("The none limits.")]
        None = 0,

        /// <summary>
        /// The minimum distance limit.
        /// </summary>
        [Tooltip("The minimum distance limit.")]
        MinDistance = 0x1,

        /// <summary>
        /// Uses the maximum distance limit.
        /// </summary>
        [Tooltip("Uses the maximum distance limit.")]
        MaxDistance = 0x2,

        /// <summary>
        /// Uses the spring when maintaining limits
        /// </summary>
        [Tooltip("Uses the spring when maintaining limits")]
        Spring = 0x4,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Physics joint that maintains an upper or lower (or both) bound on the distance between two bodies.
    /// </summary>
    /// <seealso cref="Joint" />
    [Tooltip("Physics joint that maintains an upper or lower (or both) bound on the distance between two bodies.")]
    public unsafe partial class DistanceJoint : Joint
    {
        /// <inheritdoc />
        protected DistanceJoint() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="DistanceJoint"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static DistanceJoint New()
        {
            return Internal_Create(typeof(DistanceJoint)) as DistanceJoint;
        }

        /// <summary>
        /// Gets or sets the joint mode flags. Controls joint behaviour.
        /// </summary>
        [EditorOrder(100), DefaultValue(DistanceJointFlag.MinDistance | DistanceJointFlag.MaxDistance)]
        [Tooltip("The joint mode flags. Controls joint behaviour.")]
        public DistanceJointFlag Flags
        {
            get { return Internal_GetFlags(unmanagedPtr); }
            set { Internal_SetFlags(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DistanceJointFlag Internal_GetFlags(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFlags(IntPtr obj, DistanceJointFlag value);

        /// <summary>
        /// Gets or sets the allowed minimum distance for the joint.
        /// </summary>
        /// <remarks>
        /// Used only when DistanceJointFlag.MinDistance flag is set. The minimum distance must be no more than the maximum distance. Default: 0, Range: [0, float.MaxValue].
        /// </remarks>
        [EditorOrder(110), DefaultValue(0.0f), Limit(0.0f), EditorDisplay("Joint")]
        [Tooltip("The allowed minimum distance for the joint.")]
        public float MinDistance
        {
            get { return Internal_GetMinDistance(unmanagedPtr); }
            set { Internal_SetMinDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMinDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMinDistance(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the allowed maximum distance for the joint.
        /// </summary>
        /// <remarks>
        /// Used only when DistanceJointFlag.MaxDistance flag is set. The maximum distance must be no less than the minimum distance. Default: 0, Range: [0, float.MaxValue].
        /// </remarks>
        [EditorOrder(120), DefaultValue(10.0f), Limit(0.0f), EditorDisplay("Joint")]
        [Tooltip("The allowed maximum distance for the joint.")]
        public float MaxDistance
        {
            get { return Internal_GetMaxDistance(unmanagedPtr); }
            set { Internal_SetMaxDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMaxDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaxDistance(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the error tolerance of the joint.
        /// </summary>
        /// <remarks>
        /// The distance beyond the joint's [min, max] range before the joint becomes active. Default: 25, Range: [0.1, float.MaxValue].
        /// </remarks>
        [EditorOrder(130), DefaultValue(25.0f), Limit(0.0f), EditorDisplay("Joint")]
        [Tooltip("The error tolerance of the joint.")]
        public float Tolerance
        {
            get { return Internal_GetTolerance(unmanagedPtr); }
            set { Internal_SetTolerance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTolerance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTolerance(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the spring parameters.
        /// </summary>
        [EditorOrder(140), EditorDisplay("Joint")]
        [Tooltip("The spring parameters.")]
        public SpringParameters SpringParameters
        {
            get { Internal_GetSpringParameters(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetSpringParameters(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSpringParameters(IntPtr obj, out SpringParameters resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSpringParameters(IntPtr obj, ref SpringParameters value);

        /// <summary>
        /// Gets the current distance of the joint.
        /// </summary>
        [Tooltip("The current distance of the joint.")]
        public float CurrentDistance
        {
            get { return Internal_GetCurrentDistance(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentDistance(IntPtr obj);
    }
}
