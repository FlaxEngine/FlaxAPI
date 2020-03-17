// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The scene rendering buffers container.
    /// </summary>
    [Tooltip("The scene rendering buffers container.")]
    public unsafe partial class RenderBuffers : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected RenderBuffers() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RenderBuffers"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public new static RenderBuffers New()
        {
            return Internal_Create(typeof(RenderBuffers)) as RenderBuffers;
        }

        /// <summary>
        /// Gets the depth buffer render target allocated within this render buffers collection (read only).
        /// </summary>
        [Tooltip("The depth buffer render target allocated within this render buffers collection (read only).")]
        public GPUTexture DepthBuffer
        {
            get { return Internal_GetDepthBuffer(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTexture Internal_GetDepthBuffer(IntPtr obj);

        /// <summary>
        /// Gets the motion vectors render target allocated within this render buffers collection (read only).
        /// </summary>
        /// <remarks>
        /// Texture ca be null or not initialized if motion blur is disabled or not yet rendered.
        /// </remarks>
        [Tooltip("The motion vectors render target allocated within this render buffers collection (read only).")]
        public GPUTexture MotionVectors
        {
            get { return Internal_GetMotionVectors(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTexture Internal_GetMotionVectors(IntPtr obj);

        /// <summary>
        /// Gets the buffers width (in pixels).
        /// </summary>
        [Tooltip("The buffers width (in pixels).")]
        public int Width
        {
            get { return Internal_GetWidth(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetWidth(IntPtr obj);

        /// <summary>
        /// Gets the buffers height (in pixels).
        /// </summary>
        [Tooltip("The buffers height (in pixels).")]
        public int Height
        {
            get { return Internal_GetHeight(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetHeight(IntPtr obj);

        /// <summary>
        /// Gets the buffers width and height (in pixels).
        /// </summary>
        [Tooltip("The buffers width and height (in pixels).")]
        public Vector2 Size
        {
            get { Internal_GetSize(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSize(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets the buffers aspect ratio.
        /// </summary>
        [Tooltip("The buffers aspect ratio.")]
        public float AspectRatio
        {
            get { return Internal_GetAspectRatio(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetAspectRatio(IntPtr obj);

        /// <summary>
        /// Gets the buffers rendering viewport.
        /// </summary>
        [Tooltip("The buffers rendering viewport.")]
        public Viewport Viewport
        {
            get { Internal_GetViewport(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetViewport(IntPtr obj, out Viewport resultAsRef);

        /// <summary>
        /// Allocates the buffers.
        /// </summary>
        /// <param name="width">The surface width (in pixels).</param>
        /// <param name="height">The surface height (in pixels).</param>
        /// <returns>True if cannot allocate buffers, otherwise false.</returns>
        public bool Init(int width, int height)
        {
            return Internal_Init(unmanagedPtr, width, height);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Init(IntPtr obj, int width, int height);

        /// <summary>
        /// Release the buffers data.
        /// </summary>
        public void Release()
        {
            Internal_Release(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Release(IntPtr obj);
    }
}
