// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The draw calls list types.
    /// </summary>
    [Tooltip("The draw calls list types.")]
    public enum DrawCallsListType
    {
        /// <summary>
        /// Hardware depth rendering.
        /// </summary>
        [Tooltip("Hardware depth rendering.")]
        Depth,

        /// <summary>
        /// GBuffer rendering.
        /// </summary>
        [Tooltip("GBuffer rendering.")]
        GBuffer,

        /// <summary>
        /// GBuffer rendering after decals.
        /// </summary>
        [Tooltip("GBuffer rendering after decals.")]
        GBufferNoDecals,

        /// <summary>
        /// Transparency rendering.
        /// </summary>
        [Tooltip("Transparency rendering.")]
        Forward,

        /// <summary>
        /// Distortion accumulation rendering.
        /// </summary>
        [Tooltip("Distortion accumulation rendering.")]
        Distortion,

        /// <summary>
        /// Motion vectors rendering.
        /// </summary>
        [Tooltip("Motion vectors rendering.")]
        MotionVectors,

        /// <summary>
        /// The count of items in the DrawCallsListType enum.
        /// </summary>
        [Tooltip("The count of items in the DrawCallsListType enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Rendering cache container object for the draw calls collecting, sorting and executing.
    /// </summary>
    [Tooltip("Rendering cache container object for the draw calls collecting, sorting and executing.")]
    public sealed unsafe partial class RenderList : FlaxEngine.Object
    {
        private RenderList() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RenderList"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public new static RenderList New()
        {
            return Internal_Create(typeof(RenderList)) as RenderList;
        }

        /// <summary>
        /// Allocates the new renderer list object or reuses already allocated one.
        /// </summary>
        /// <returns>The cache object.</returns>
        public static RenderList GetFromPool()
        {
            return Internal_GetFromPool();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RenderList Internal_GetFromPool();

        /// <summary>
        /// Frees the list back to the pool.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public static void ReturnToPool(RenderList cache)
        {
            Internal_ReturnToPool(FlaxEngine.Object.GetUnmanagedPtr(cache));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ReturnToPool(IntPtr cache);

        /// <summary>
        /// Sorts the collected draw calls list.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="reverseDistance">If set to <c>true</c> reverse draw call distance to the view. Results in back to front sorting.</param>
        /// <param name="listType">The collected draw calls list type.</param>
        public void SortDrawCalls(ref RenderContext renderContext, bool reverseDistance, DrawCallsListType listType)
        {
            Internal_SortDrawCalls(unmanagedPtr, ref renderContext, reverseDistance, listType);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SortDrawCalls(IntPtr obj, ref RenderContext renderContext, bool reverseDistance, DrawCallsListType listType);

        /// <summary>
        /// Executes the collected draw calls.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="listType">The collected draw calls list type.</param>
        public void ExecuteDrawCalls(ref RenderContext renderContext, DrawCallsListType listType)
        {
            Internal_ExecuteDrawCalls(unmanagedPtr, ref renderContext, listType);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ExecuteDrawCalls(IntPtr obj, ref RenderContext renderContext, DrawCallsListType listType);
    }
}
