// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Object that stores various render statistics.
    /// </summary>
    [Tooltip("Object that stores various render statistics.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct RenderStatsData
    {
        /// <summary>
        /// The draw calls count.
        /// </summary>
        [Tooltip("The draw calls count.")]
        public long DrawCalls;

        /// <summary>
        /// The compute shader dispatch calls count.
        /// </summary>
        [Tooltip("The compute shader dispatch calls count.")]
        public long DispatchCalls;

        /// <summary>
        /// The vertices drawn count.
        /// </summary>
        [Tooltip("The vertices drawn count.")]
        public long Vertices;

        /// <summary>
        /// The triangles drawn count.
        /// </summary>
        [Tooltip("The triangles drawn count.")]
        public long Triangles;

        /// <summary>
        /// The pipeline state changes count.
        /// </summary>
        [Tooltip("The pipeline state changes count.")]
        public long PipelineStateChanges;
    }
}
