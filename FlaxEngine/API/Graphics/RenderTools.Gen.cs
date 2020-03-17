// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Set of utilities for rendering.
    /// </summary>
    [Tooltip("Set of utilities for rendering.")]
    public static unsafe partial class RenderTools
    {
        /// <summary>
        /// Computes the model LOD index to use during rendering.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="origin">The bounds origin.</param>
        /// <param name="radius">The bounds radius.</param>
        /// <param name="renderContext">The rendering context.</param>
        /// <returns>The zero-based LOD index. Returns -1 if model should not be rendered.</returns>
        public static int ComputeModelLOD(Model model, ref Vector3 origin, float radius, ref RenderContext renderContext)
        {
            return Internal_ComputeModelLOD(FlaxEngine.Object.GetUnmanagedPtr(model), ref origin, radius, ref renderContext);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_ComputeModelLOD(IntPtr model, ref Vector3 origin, float radius, ref RenderContext renderContext);

        /// <summary>
        /// Computes the skinned model LOD index to use during rendering.
        /// </summary>
        /// <param name="model">The skinned model.</param>
        /// <param name="origin">The bounds origin.</param>
        /// <param name="radius">The bounds radius.</param>
        /// <param name="renderContext">The rendering context.</param>
        /// <returns>The zero-based LOD index. Returns -1 if model should not be rendered.</returns>
        public static int ComputeSkinnedModelLOD(SkinnedModel model, ref Vector3 origin, float radius, ref RenderContext renderContext)
        {
            return Internal_ComputeSkinnedModelLOD(FlaxEngine.Object.GetUnmanagedPtr(model), ref origin, radius, ref renderContext);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_ComputeSkinnedModelLOD(IntPtr model, ref Vector3 origin, float radius, ref RenderContext renderContext);
    }
}
