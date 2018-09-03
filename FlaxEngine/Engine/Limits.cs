// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a joint limit between two distance values. Lower value must be less than the upper value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LimitLinearRange
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [EditorOrder(0), Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [EditorOrder(10), Limit(0.0f, 1.0f), Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [EditorOrder(20), Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// The lower distance of the limit. Must be less than upper.
        /// </summary>
        [EditorOrder(50), Tooltip("The lower distance of the limit. Must be less than upper.")]
        public float Lower;

        /// <summary>
        /// The upper distance of the limit. Must be more than lower.
        /// </summary>
        [EditorOrder(60), Tooltip("The upper distance of the limit. Must be more than lower.")]
        public float Upper;

        /// <summary>
        /// The default <see cref="LimitLinearRange"/> structure with empty limit.
        /// </summary>
        public static readonly LimitLinearRange Default = new LimitLinearRange(0.0f, 0.0f);

        /// <summary>
        /// Constructs a hard limit. Once the limit is reached the movement of the attached bodies will come to a stop.
        /// </summary>
        /// <param name="lower">The lower distance of the limit. Must be less than upper.</param>
        /// <param name="upper">The upper distance of the limit. Must be more than lower.</param>
        /// <param name="contactDist">Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit. Specify -1 for the default.</param>
        public LimitLinearRange(float lower, float upper, float contactDist = -1.0f)
        {
            ContactDist = contactDist;
            Restitution = 0.0f;
            Spring = new SpringParameters();
            Lower = lower;
            Upper = upper;
        }

        /// <summary>
        /// Constructs a soft limit. Once the limit is reached the bodies will bounce back according to the restitution parameter and will be pulled back towards the limit by the provided spring.
        /// </summary>
        /// <param name="lower">The lower distance of the limit. Must be less than upper.</param>
        /// <param name="upper">The upper distance of the limit. Must be more than lower.</param>
        /// <param name="spring">The spring that controls how are the bodies pulled back towards the limit when they breach it.</param>
        /// <param name="restitution">Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.</param>
        public LimitLinearRange(float lower, float upper, SpringParameters spring, float restitution = 0.0f)
        {
            ContactDist = -1.0f;
            Restitution = restitution;
            Spring = spring;
            Lower = lower;
            Upper = upper;
        }
    }

    /// <summary>
    /// Represents a joint limit between zero a single distance value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LimitLinear
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [EditorOrder(0), Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [EditorOrder(10), Limit(0.0f, 1.0f), Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [EditorOrder(20), Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// The distance at which the limit becomes active.
        /// </summary>
        [EditorOrder(50), Limit(0.0f), Tooltip("The distance at which the limit becomes active.")]
        public float Extent;

        /// <summary>
        /// The default <see cref="LimitLinear"/> structure with empty limit.
        /// </summary>
        public static readonly LimitLinear Default = new LimitLinear(0.0f);

        /// <summary>
        /// Constructs a hard limit. Once the limit is reached the movement of the attached bodies will come to a stop.
        /// </summary>
        /// <param name="extent">The distance at which the limit becomes active.</param>
        /// <param name="contactDist">The distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit. Specify -1 for the default.</param>
        public LimitLinear(float extent, float contactDist = -1.0f)
        {
            ContactDist = contactDist;
            Restitution = 0.0f;
            Spring = new SpringParameters();
            Extent = extent;
        }

        /// <summary>
        /// Constructs a soft limit. Once the limit is reached the bodies will bounce back according to the restitution parameter and will be pulled back towards the limit by the provided spring.
        /// </summary>
        /// <param name="extent">The distance at which the limit becomes active.</param>
        /// <param name="spring">The spring that controls how are the bodies pulled back towards the limit when they breach it.</param>
        /// <param name="restitution">Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.</param>
        public LimitLinear(float extent, SpringParameters spring, float restitution = 0.0f)
        {
            ContactDist = -1.0f;
            Restitution = restitution;
            Spring = spring;
            Extent = extent;
        }
    }

    /// <summary>
    /// Represents a joint limit between two angles.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LimitAngularRange
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [EditorOrder(0), Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [EditorOrder(10), Limit(0.0f, 1.0f), Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [EditorOrder(20), Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// Lower angle of the limit (in degrees). Must be less than upper.
        /// </summary>
        [EditorOrder(50), Tooltip("Lower angle of the limit (in degrees). Must be less than upper.")]
        public float Lower;

        /// <summary>
        /// Upper angle of the limit (in degrees). Must be less than lower.
        /// </summary>
        [EditorOrder(60), Tooltip("Upper angle of the limit (in degrees). Must be less than lower.")]
        public float Upper;

        /// <summary>
        /// The default <see cref="LimitAngularRange"/> structure with empty limit.
        /// </summary>
        public static readonly LimitAngularRange Default = new LimitAngularRange(0.0f, 0.0f);

        /// <summary>
        /// Constructs a hard limit. Once the limit is reached the movement of the attached bodies will come to a stop.
        /// </summary>
        /// <param name="lower">The lower angle of the limit (in degrees). Must be less than upper.</param>
        /// <param name="upper">The upper angle of the limit (in degrees). Must be more than lower.</param>
        /// <param name="contactDist">Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit. Specify -1 for the default.</param>
        public LimitAngularRange(float lower, float upper, float contactDist = -1.0f)
        {
            ContactDist = contactDist;
            Restitution = 0.0f;
            Spring = new SpringParameters();
            Lower = lower;
            Upper = upper;
        }

        /// <summary>
        /// Constructs a soft limit. Once the limit is reached the bodies will bounce back according to the restitution parameter and will be pulled back towards the limit by the provided spring.
        /// </summary>
        /// <param name="lower">The lower angle of the limit. Must be less than upper.</param>
        /// <param name="upper">The upper angle of the limit. Must be more than lower.</param>
        /// <param name="spring">The spring that controls how are the bodies pulled back towards the limit when they breach it.</param>
        /// <param name="restitution">Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.</param>
        public LimitAngularRange(float lower, float upper, SpringParameters spring, float restitution = 0.0f)
        {
            ContactDist = -1.0f;
            Restitution = restitution;
            Spring = spring;
            Lower = lower;
            Upper = upper;
        }
    }

    /// <summary>
    /// Represents a joint limit that constraints movement to within an elliptical cone.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LimitConeRange
    {
        /// <summary>
        /// Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.
        /// </summary>
        [EditorOrder(0), Tooltip("Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit.")]
        public float ContactDist;

        /// <summary>
        /// Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.
        /// </summary>
        [EditorOrder(10), Limit(0.0f, 1.0f), Tooltip("Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.")]
        public float Restitution;

        /// <summary>
        /// The spring that controls how are the bodies pulled back towards the limit when they breach it.
        /// </summary>
        [EditorOrder(20), Tooltip("The spring that controls how are the bodies pulled back towards the limit when they breach it.")]
        public SpringParameters Spring;

        /// <summary>
        /// The Y angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Y axis.
        /// </summary>
        [EditorOrder(50), Limit(0.1f, 179.9f), Tooltip("The Y angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Y axis.")]
        public float YLimitAngle;

        /// <summary>
        /// The Z angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Z axis.
        /// </summary>
        [EditorOrder(60), Limit(0.1f, 179.9f), Tooltip("The Z angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Z axis.")]
        public float ZLimitAngle;

        /// <summary>
        /// The default <see cref="LimitConeRange"/> structure with a 45 degree cone limit.
        /// </summary>
        public static readonly LimitConeRange Default = new LimitConeRange(90.0f, 90.0f);

        /// <summary>
        /// Constructs a hard limit. Once the limit is reached the movement of the attached bodies will come to a stop.
        /// </summary>
        /// <param name="yLimitAngle">The Y angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Y axis.</param>
        /// <param name="zLimitAngle">The Z angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Z axis.</param>
        /// <param name="contactDist">Distance from the limit at which it becomes active. Allows the solver to activate earlier than the limit is reached to avoid breaking the limit. Specify -1 for the default.</param>
        public LimitConeRange(float yLimitAngle, float zLimitAngle, float contactDist = -1.0f)
        {
            ContactDist = contactDist;
            Restitution = 0.0f;
            Spring = new SpringParameters();
            YLimitAngle = yLimitAngle;
            ZLimitAngle = zLimitAngle;
        }

        /// <summary>
        /// Constructs a soft limit. Once the limit is reached the bodies will bounce back according to the restitution parameter and will be pulled back towards the limit by the provided spring.
        /// </summary>
        /// <param name="yLimitAngle">The Y angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Y axis.</param>
        /// <param name="zLimitAngle">The Z angle of the cone (in degrees). Movement is constrained between 0 and this angle on the Z axis.</param>
        /// <param name="spring">The spring that controls how are the bodies pulled back towards the limit when they breach it.</param>
        /// <param name="restitution">Controls how do objects react when the limit is reached, values closer to zero specify non-elastic collision, while those closer to one specify more elastic (i.e bouncy) collision. Must be in [0, 1] range.</param>
        public LimitConeRange(float yLimitAngle, float zLimitAngle, SpringParameters spring, float restitution = 0.0f)
        {
            ContactDist = -1.0f;
            Restitution = restitution;
            Spring = spring;
            YLimitAngle = yLimitAngle;
            ZLimitAngle = zLimitAngle;
        }
    }
}
