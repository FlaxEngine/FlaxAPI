// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Controls spring parameters for a physics joint limits. If a limit is soft (body bounces back due to restitution when
    /// the limit is reached) the spring will pull the body back towards the limit using the specified parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct SpringParameters
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
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents a joint limit between two distance values. Lower value must be less than the upper value.
    /// </summary>
    [Tooltip("Represents a joint limit between two distance values. Lower value must be less than the upper value.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct LimitLinearRange
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [Limit(0.0f, 1.0f)]
        [Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// The lower distance of the limit. Must be less than upper.
        /// </summary>
        [Tooltip("The lower distance of the limit. Must be less than upper.")]
        public float Lower;

        /// <summary>
        /// The upper distance of the limit. Must be more than lower.
        /// </summary>
        [Tooltip("The upper distance of the limit. Must be more than lower.")]
        public float Upper;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents a joint limit between zero a single distance value.
    /// </summary>
    [Tooltip("Represents a joint limit between zero a single distance value.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct LimitLinear
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [Limit(0.0f, 1.0f)]
        [Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// The distance at which the limit becomes active.
        /// </summary>
        [Tooltip("The distance at which the limit becomes active.")]
        public float Extent;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents a joint limit between two angles.
    /// </summary>
    [Tooltip("Represents a joint limit between two angles.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct LimitAngularRange
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [Limit(0.0f, 1.0f)]
        [Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// Lower angle of the limit (in degrees). Must be less than upper.
        /// </summary>
        [Tooltip("Lower angle of the limit (in degrees). Must be less than upper.")]
        public float Lower;

        /// <summary>
        /// Upper angle of the limit (in degrees). Must be less than lower.
        /// </summary>
        [Tooltip("Upper angle of the limit (in degrees). Must be less than lower.")]
        public float Upper;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents a joint limit that constraints movement to within an elliptical cone.
    /// </summary>
    [Tooltip("Represents a joint limit that constraints movement to within an elliptical cone.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct LimitConeRange
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [Limit(0.0f, 1.0f)]
        [Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// The Y angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Y axis.
        /// </summary>
        [Tooltip("The Y angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Y axis.")]
        public float YLimitAngle;

        /// <summary>
        /// The Z angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Z axis.
        /// </summary>
        [Tooltip("The Z angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Z axis.")]
        public float ZLimitAngle;
    }
}
