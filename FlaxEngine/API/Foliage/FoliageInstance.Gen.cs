// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Foliage instanced mesh instance. Packed data with very little of logic. Managed by the foliage chunks and foliage actor itself.
    /// </summary>
    [Tooltip("Foliage instanced mesh instance. Packed data with very little of logic. Managed by the foliage chunks and foliage actor itself.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct FoliageInstance
    {
        /// <summary>
        /// The local-space transformation of the mesh relative to the foliage actor.
        /// </summary>
        [Tooltip("The local-space transformation of the mesh relative to the foliage actor.")]
        public Transform Transform;

        /// <summary>
        /// The cached world transformation matrix of this instance.
        /// </summary>
        [Tooltip("The cached world transformation matrix of this instance.")]
        public Matrix World;

        /// <summary>
        /// The foliage type index. Foliage types are hold in foliage actor and shared by instances using the same model.
        /// </summary>
        [Tooltip("The foliage type index. Foliage types are hold in foliage actor and shared by instances using the same model.")]
        public int Type;

        /// <summary>
        /// The per-instance random value from range [0;1].
        /// </summary>
        [Tooltip("The per-instance random value from range [0;1].")]
        public float Random;

        /// <summary>
        /// The cached instance bounds (in world space).
        /// </summary>
        [Tooltip("The cached instance bounds (in world space).")]
        public BoundingSphere Bounds;
    }
}
