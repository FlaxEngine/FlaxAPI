// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Renders model on the screen.
    /// </summary>
    [Tooltip("Renders model on the screen.")]
    public unsafe partial class StaticModel : ModelInstanceActor
    {
        /// <inheritdoc />
        protected StaticModel() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="StaticModel"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static StaticModel New()
        {
            return Internal_Create(typeof(StaticModel)) as StaticModel;
        }

        /// <summary>
        /// The model asset to draw.
        /// </summary>
        [EditorOrder(20), DefaultValue(null), EditorDisplay("Model")]
        [Tooltip("The model asset to draw.")]
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
        /// The draw passes to use for rendering this object.
        /// </summary>
        [EditorOrder(15), DefaultValue(DrawPass.Default), EditorDisplay("Model")]
        [Tooltip("The draw passes to use for rendering this object.")]
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
        /// Gets or sets the model scale in lightmap (applied to all the meshes).
        /// </summary>
        [EditorOrder(10), DefaultValue(1.0f), EditorDisplay("Model", "Scale In Lightmap"), Limit(0, 1000.0f, 0.1f)]
        [Tooltip("The model scale in lightmap (applied to all the meshes).")]
        public float ScaleInLightmap
        {
            get { return Internal_GetScaleInLightmap(unmanagedPtr); }
            set { Internal_SetScaleInLightmap(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScaleInLightmap(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScaleInLightmap(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the model bounds scale. It is useful when using Position Offset to animate the vertices of the object outside of its bounds. Increasing the bounds of an object will reduce performance.
        /// </summary>
        [EditorOrder(12), DefaultValue(1.0f), EditorDisplay("Model"), Limit(0, 10.0f, 0.1f)]
        [Tooltip("The model bounds scale. It is useful when using Position Offset to animate the vertices of the object outside of its bounds. Increasing the bounds of an object will reduce performance.")]
        public float BoundsScale
        {
            get { return Internal_GetBoundsScale(unmanagedPtr); }
            set { Internal_SetBoundsScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBoundsScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBoundsScale(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the model Level Of Detail bias value. Allows to increase or decrease rendered model quality.
        /// </summary>
        [EditorOrder(40), DefaultValue(0), Limit(-100, 100, 0.1f), EditorDisplay("Model", "LOD Bias")]
        [Tooltip("The model Level Of Detail bias value. Allows to increase or decrease rendered model quality.")]
        public int LODBias
        {
            get { return Internal_GetLODBias(unmanagedPtr); }
            set { Internal_SetLODBias(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetLODBias(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLODBias(IntPtr obj, int value);

        /// <summary>
        /// Gets or sets the model forced Level Of Detail index. Allows to bind the given model LOD to show. Value -1 disables this feature.
        /// </summary>
        [EditorOrder(50), DefaultValue(-1), Limit(-1, 100, 0.1f), EditorDisplay("Model", "Forced LOD")]
        [Tooltip("The model forced Level Of Detail index. Allows to bind the given model LOD to show. Value -1 disables this feature.")]
        public int ForcedLOD
        {
            get { return Internal_GetForcedLOD(unmanagedPtr); }
            set { Internal_SetForcedLOD(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetForcedLOD(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetForcedLOD(IntPtr obj, int value);

        /// <summary>
        /// Determines whether this model has valid lightmap data.
        /// </summary>
        [Tooltip("Determines whether this model has valid lightmap data.")]
        public bool HasLightmap
        {
            get { return Internal_HasLightmap(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasLightmap(IntPtr obj);

        /// <summary>
        /// Removes the lightmap data from the model.
        /// </summary>
        public void RemoveLightmap()
        {
            Internal_RemoveLightmap(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemoveLightmap(IntPtr obj);

        /// <summary>
        /// Gets the material used to render mesh at given index (overriden by model instance buffer or model default).
        /// </summary>
        /// <param name="meshIndex">The zero-based mesh index.</param>
        /// <param name="lodIndex">The LOD index.</param>
        /// <returns>Material or null if not assigned.</returns>
        public MaterialBase GetMaterial(int meshIndex, int lodIndex = 0)
        {
            return Internal_GetMaterial(unmanagedPtr, meshIndex, lodIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMaterial(IntPtr obj, int meshIndex, int lodIndex);
    }
}
