// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Foliage tools for editor. Allows to spawn and modify foliage instances.
    /// </summary>
    [Tooltip("Foliage tools for editor. Allows to spawn and modify foliage instances.")]
    public static unsafe partial class FoliageTools
    {
        /// <summary>
        /// Paints the foliage instances using the given foliage types selection and the brush location.
        /// </summary>
        /// <param name="foliage">The foliage actor.</param>
        /// <param name="foliageTypesIndices">The foliage types indices to use for painting.</param>
        /// <param name="brushPosition">The brush position.</param>
        /// <param name="brushRadius">The brush radius.</param>
        /// <param name="additive">True if paint using additive mode, false if remove foliage instances.</param>
        public static void Paint(Foliage foliage, int[] foliageTypesIndices, Vector3 brushPosition, float brushRadius, bool additive)
        {
            Internal_Paint(FlaxEngine.Object.GetUnmanagedPtr(foliage), foliageTypesIndices, ref brushPosition, brushRadius, additive);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Paint(IntPtr foliage, int[] foliageTypesIndices, ref Vector3 brushPosition, float brushRadius, bool additive);

        /// <summary>
        /// Paints the foliage instances using the given foliage types selection and the brush location.
        /// </summary>
        /// <param name="foliage">The foliage actor.</param>
        /// <param name="foliageTypesIndices">The foliage types indices to use for painting.</param>
        /// <param name="brushPosition">The brush position.</param>
        /// <param name="brushRadius">The brush radius.</param>
        public static void Paint(Foliage foliage, int[] foliageTypesIndices, Vector3 brushPosition, float brushRadius)
        {
            Internal_Paint1(FlaxEngine.Object.GetUnmanagedPtr(foliage), foliageTypesIndices, ref brushPosition, brushRadius);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Paint1(IntPtr foliage, int[] foliageTypesIndices, ref Vector3 brushPosition, float brushRadius);

        /// <summary>
        /// Removes the foliage instances using the given foliage types selection and the brush location.
        /// </summary>
        /// <param name="foliage">The foliage actor.</param>
        /// <param name="foliageTypesIndices">The foliage types indices to use for painting.</param>
        /// <param name="brushPosition">The brush position.</param>
        /// <param name="brushRadius">The brush radius.</param>
        public static void Remove(Foliage foliage, int[] foliageTypesIndices, Vector3 brushPosition, float brushRadius)
        {
            Internal_Remove(FlaxEngine.Object.GetUnmanagedPtr(foliage), foliageTypesIndices, ref brushPosition, brushRadius);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Remove(IntPtr foliage, int[] foliageTypesIndices, ref Vector3 brushPosition, float brushRadius);
    }
}
