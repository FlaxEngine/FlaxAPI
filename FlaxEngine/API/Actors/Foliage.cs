// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    public sealed partial class Foliage
    {
        /// <summary>
        /// The foliage instance data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Instance
        {
            /// <summary>
            /// The local-space transformation of the mesh relative to the foliage actor.
            /// </summary>
            public Transform Transform;

            /// <summary>
            /// The foliage type index. Foliage types are hold in foliage actor and shared by instances using the same model.
            /// </summary>
            public int Type;

            /// <summary>
            /// The cached instance bounds (in world space).
            /// </summary>
            public BoundingSphere Bounds;
        }

        /// <summary>
        /// The foliage instances scaling modes.
        /// </summary>
        public enum ScalingModes
        {
            /// <summary>
            /// The uniform scaling. All axes are scaled the same.
            /// </summary>
            [Tooltip("The uniform scaling. All axes are scaled the same.")]
            Uniform,

            /// <summary>
            /// The free scaling. Each axis can have custom scale.
            /// </summary>
            [Tooltip("The free scaling. Each axis can have custom scale.")]
            Free,

            /// <summary>
            /// The lock XZ plane axis. Axes X and Z are constrained to-gather and axis Y is free.
            /// </summary>
            [Tooltip("The lock XZ plane axis. Axes X and Z are constrained to-gather and axis Y is free.")]
            LockXZ,

            /// <summary>
            /// The lock XY plane axis. Axes X and Y are constrained to-gather and axis Z is free.
            /// </summary>
            [Tooltip("The lock XY plane axis. Axes X and Y are constrained to-gather and axis Z is free.")]
            LockXY,

            /// <summary>
            /// The lock YZ plane axis. Axes Y and Z are constrained to-gather and axis X is free.
            /// </summary>
            [Tooltip("The lock YZ plane axis. Axes Y and Z are constrained to-gather and axis X is free.")]
            LockYZ,
        }

        /// <summary>
        /// The foliage instance type options (packed into single structure - raw data).
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TypeOptions
        {
            /// <summary>
            /// The per-instance cull distance.
            /// </summary>
            public float CullDistance;

            /// <summary>
            /// The per-instance cull distance randomization range (randomized per instance and added to master CullDistance value).
            /// </summary>
            public float CullDistanceRandomRange;

            /// <summary>
            /// The draw passes to use for rendering this foliage type.
            /// </summary>
            public DrawPass DrawModes;

            /// <summary>
            /// The shadows casting mode.
            /// </summary>
            public ShadowsCastingMode ShadowsMode;

            /// <summary>
            /// The foliage instances density defined in instances count per 1000x1000 units area.
            /// </summary>
            public float PaintDensity;

            /// <summary>
            /// The minimum radius between foliage instances.
            /// </summary>
            public float PaintRadius;

            /// <summary>
            /// The minimum ground slope angle to paint foliage on it (in degrees).
            /// </summary>
            public float PaintGroundSlopeAngleMin;

            /// <summary>
            /// The maximum ground slope angle to paint foliage on it (in degrees).
            /// </summary>
            public float PaintGroundSlopeAngleMax;

            /// <summary>
            /// The scaling mode.
            /// </summary>
            public ScalingModes PaintScaling;

            /// <summary>
            /// The scale minimum values per axis.
            /// </summary>
            public Vector3 PaintScaleMin;

            /// <summary>
            /// The scale maximum values per axis.
            /// </summary>
            public Vector3 PaintScaleMax;

            /// <summary>
            /// The per-instance random offset range on axis Y.
            /// </summary>
            public Vector2 PlacementOffsetY;

            /// <summary>
            /// The random pitch angle range (uniform in both ways around normal vector).
            /// </summary>
            public float PlacementRandomPitchAngle;

            /// <summary>
            /// The random roll angle range (uniform in both ways around normal vector).
            /// </summary>
            public float PlacementRandomRollAngle;

            /// <summary>
            /// The density scaling scale applied to the global scale for the foliage instances of this type. Can be used to boost or reduce density scaling effect on this foliage type. Default is 1.
            /// </summary>
            public float DensityScalingScale;

            /// <summary>
            /// Determines whenever this meshes can receive decals.
            /// </summary>
            public byte ReceiveDecals;

            /// <summary>
            /// Flag used to determinate whenever use global foliage density scaling for instances of this foliage type.
            /// </summary>
            public byte UseDensityScaling;

            /// <summary>
            /// If checked, instances will be aligned to normal of the placed surface.
            /// </summary>
            public byte PlacementAlignToNormal;

            /// <summary>
            /// If checked, instances will use randomized yaw when placed. Random yaw uses will rotation range over the Y axis.
            /// </summary>
            public byte PlacementRandomYaw;
        }
    }
}
