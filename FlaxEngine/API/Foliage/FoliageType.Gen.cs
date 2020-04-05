// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The foliage instances scaling modes.
    /// </summary>
    [Tooltip("The foliage instances scaling modes.")]
    public enum FoliageScalingModes
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
}

namespace FlaxEngine
{
    /// <summary>
    /// Foliage mesh instances type descriptor. Defines the shared properties of the spawned mesh instances.
    /// </summary>
    [Tooltip("Foliage mesh instances type descriptor. Defines the shared properties of the spawned mesh instances.")]
    public sealed unsafe partial class FoliageType : FlaxEngine.Object
    {
        private FoliageType() : base()
        {
        }

        /// <summary>
        /// The parent foliage actor.
        /// </summary>
        [Tooltip("The parent foliage actor.")]
        public Foliage Foliage
        {
            get { return Internal_GetFoliage(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Foliage Internal_GetFoliage(IntPtr obj);

        /// <summary>
        /// The foliage type index.
        /// </summary>
        [Tooltip("The foliage type index.")]
        public int Index
        {
            get { return Internal_GetIndex(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetIndex(IntPtr obj);

        /// <summary>
        /// The model to draw by the instances.
        /// </summary>
        [Tooltip("The model to draw by the instances.")]
        public Model Model
        {
            get { return Internal_GetModel(unmanagedPtr); }
            set { Internal_SetModel(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Model Internal_GetModel(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetModel(IntPtr obj, IntPtr value);

        /// <summary>
        /// The per-instance cull distance.
        /// </summary>
        [Tooltip("The per-instance cull distance.")]
        public float CullDistance
        {
            get { return Internal_GetCullDistance(unmanagedPtr); }
            set { Internal_SetCullDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCullDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCullDistance(IntPtr obj, float value);

        /// <summary>
        /// The per-instance cull distance randomization range (randomized per instance and added to master CullDistance value).
        /// </summary>
        [Tooltip("The per-instance cull distance randomization range (randomized per instance and added to master CullDistance value).")]
        public float CullDistanceRandomRange
        {
            get { return Internal_GetCullDistanceRandomRange(unmanagedPtr); }
            set { Internal_SetCullDistanceRandomRange(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCullDistanceRandomRange(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCullDistanceRandomRange(IntPtr obj, float value);

        /// <summary>
        /// The draw passes to use for rendering this foliage type.
        /// </summary>
        [Tooltip("The draw passes to use for rendering this foliage type.")]
        public DrawPass DrawModes
        {
            get { return Internal_GetDrawModes(unmanagedPtr); }
            set { Internal_SetDrawModes(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DrawPass Internal_GetDrawModes(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrawModes(IntPtr obj, DrawPass value);

        /// <summary>
        /// The shadows casting mode.
        /// </summary>
        [Tooltip("The shadows casting mode.")]
        public ShadowsCastingMode ShadowsMode
        {
            get { return Internal_GetShadowsMode(unmanagedPtr); }
            set { Internal_SetShadowsMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsMode(IntPtr obj, ShadowsCastingMode value);

        /// <summary>
        /// The foliage instances density defined in instances count per 1000x1000 units area.
        /// </summary>
        [Tooltip("The foliage instances density defined in instances count per 1000x1000 units area.")]
        public float PaintDensity
        {
            get { return Internal_GetPaintDensity(unmanagedPtr); }
            set { Internal_SetPaintDensity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPaintDensity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPaintDensity(IntPtr obj, float value);

        /// <summary>
        /// The minimum radius between foliage instances.
        /// </summary>
        [Tooltip("The minimum radius between foliage instances.")]
        public float PaintRadius
        {
            get { return Internal_GetPaintRadius(unmanagedPtr); }
            set { Internal_SetPaintRadius(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPaintRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPaintRadius(IntPtr obj, float value);

        /// <summary>
        /// The minimum ground slope angle to paint foliage on it (in degrees).
        /// </summary>
        [Tooltip("The minimum ground slope angle to paint foliage on it (in degrees).")]
        public float PaintGroundSlopeAngleMin
        {
            get { return Internal_GetPaintGroundSlopeAngleMin(unmanagedPtr); }
            set { Internal_SetPaintGroundSlopeAngleMin(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPaintGroundSlopeAngleMin(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPaintGroundSlopeAngleMin(IntPtr obj, float value);

        /// <summary>
        /// The maximum ground slope angle to paint foliage on it (in degrees).
        /// </summary>
        [Tooltip("The maximum ground slope angle to paint foliage on it (in degrees).")]
        public float PaintGroundSlopeAngleMax
        {
            get { return Internal_GetPaintGroundSlopeAngleMax(unmanagedPtr); }
            set { Internal_SetPaintGroundSlopeAngleMax(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPaintGroundSlopeAngleMax(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPaintGroundSlopeAngleMax(IntPtr obj, float value);

        /// <summary>
        /// The scaling mode.
        /// </summary>
        [Tooltip("The scaling mode.")]
        public FoliageScalingModes PaintScaling
        {
            get { return Internal_GetPaintScaling(unmanagedPtr); }
            set { Internal_SetPaintScaling(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FoliageScalingModes Internal_GetPaintScaling(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPaintScaling(IntPtr obj, FoliageScalingModes value);

        /// <summary>
        /// The scale minimum values per axis.
        /// </summary>
        [Tooltip("The scale minimum values per axis.")]
        public Vector3 PaintScaleMin
        {
            get { Internal_GetPaintScaleMin(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetPaintScaleMin(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPaintScaleMin(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPaintScaleMin(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// The scale maximum values per axis.
        /// </summary>
        [Tooltip("The scale maximum values per axis.")]
        public Vector3 PaintScaleMax
        {
            get { Internal_GetPaintScaleMax(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetPaintScaleMax(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPaintScaleMax(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPaintScaleMax(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// The per-instance random offset range on axis Y.
        /// </summary>
        [Tooltip("The per-instance random offset range on axis Y.")]
        public Vector2 PlacementOffsetY
        {
            get { Internal_GetPlacementOffsetY(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetPlacementOffsetY(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPlacementOffsetY(IntPtr obj, out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPlacementOffsetY(IntPtr obj, ref Vector2 value);

        /// <summary>
        /// The random pitch angle range (uniform in both ways around normal vector).
        /// </summary>
        [Tooltip("The random pitch angle range (uniform in both ways around normal vector).")]
        public float PlacementRandomPitchAngle
        {
            get { return Internal_GetPlacementRandomPitchAngle(unmanagedPtr); }
            set { Internal_SetPlacementRandomPitchAngle(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPlacementRandomPitchAngle(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPlacementRandomPitchAngle(IntPtr obj, float value);

        /// <summary>
        /// The random roll angle range (uniform in both ways around normal vector).
        /// </summary>
        [Tooltip("The random roll angle range (uniform in both ways around normal vector).")]
        public float PlacementRandomRollAngle
        {
            get { return Internal_GetPlacementRandomRollAngle(unmanagedPtr); }
            set { Internal_SetPlacementRandomRollAngle(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPlacementRandomRollAngle(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPlacementRandomRollAngle(IntPtr obj, float value);

        /// <summary>
        /// The density scaling scale applied to the global scale for the foliage instances of this type. Can be used to boost or reduce density scaling effect on this foliage type. Default is 1.
        /// </summary>
        [Tooltip("The density scaling scale applied to the global scale for the foliage instances of this type. Can be used to boost or reduce density scaling effect on this foliage type. Default is 1.")]
        public float DensityScalingScale
        {
            get { return Internal_GetDensityScalingScale(unmanagedPtr); }
            set { Internal_SetDensityScalingScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDensityScalingScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDensityScalingScale(IntPtr obj, float value);

        /// <summary>
        /// Determines whenever this meshes can receive decals.
        /// </summary>
        [Tooltip("Determines whenever this meshes can receive decals.")]
        public bool ReceiveDecals
        {
            get { return Internal_GetReceiveDecals(unmanagedPtr); }
            set { Internal_SetReceiveDecals(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetReceiveDecals(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetReceiveDecals(IntPtr obj, bool value);

        /// <summary>
        /// Flag used to determinate whenever use global foliage density scaling for instances of this foliage type.
        /// </summary>
        [Tooltip("Flag used to determinate whenever use global foliage density scaling for instances of this foliage type.")]
        public bool UseDensityScaling
        {
            get { return Internal_GetUseDensityScaling(unmanagedPtr); }
            set { Internal_SetUseDensityScaling(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseDensityScaling(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseDensityScaling(IntPtr obj, bool value);

        /// <summary>
        /// If checked, instances will be aligned to normal of the placed surface.
        /// </summary>
        [Tooltip("If checked, instances will be aligned to normal of the placed surface.")]
        public bool PlacementAlignToNormal
        {
            get { return Internal_GetPlacementAlignToNormal(unmanagedPtr); }
            set { Internal_SetPlacementAlignToNormal(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetPlacementAlignToNormal(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPlacementAlignToNormal(IntPtr obj, bool value);

        /// <summary>
        /// If checked, instances will use randomized yaw when placed. Random yaw uses will rotation range over the Y axis.
        /// </summary>
        [Tooltip("If checked, instances will use randomized yaw when placed. Random yaw uses will rotation range over the Y axis.")]
        public bool PlacementRandomYaw
        {
            get { return Internal_GetPlacementRandomYaw(unmanagedPtr); }
            set { Internal_SetPlacementRandomYaw(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetPlacementRandomYaw(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPlacementRandomYaw(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the foliage instance type materials buffer (overrides).
        /// </summary>
        [Tooltip("The foliage instance type materials buffer (overrides).")]
        public MaterialBase[] Materials
        {
            get { return Internal_GetMaterials(unmanagedPtr, typeof(MaterialBase)); }
            set { Internal_SetMaterials(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase[] Internal_GetMaterials(IntPtr obj, System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterials(IntPtr obj, MaterialBase[] value);
    }
}
