// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
    public static partial class Render2D
    {
        /// <summary>
        /// Pushes 2D transformation matrix on the stack.
        /// </summary>
        /// <param name="transform">The transformation.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void PushTransform(ref Matrix3x3 transform)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_PushTransform(ref transform);
#endif
        }

        /// <summary>
        /// Push clipping rectangle mask
        /// </summary>
        /// <param name="clipRect">Axis aligned clipping mask rectangle</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void PushClip(ref Rectangle clipRect)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_PushClip(ref clipRect);
#endif
        }

        /// <summary>
        /// Calls drawing GUI to the texture.
        /// </summary>
        /// <param name="guiRoot">The root control of the GUI to draw.</param>
        /// <param name="context">The GPU context to handle graphics commands.</param>
        /// <param name="output">The output render target.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void CallDrawing(Control guiRoot, GPUContext context, RenderTarget output)
        {
            if (context == null || output == null || guiRoot == null)
                throw new ArgumentNullException();

#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            if (Internal_DrawBegin1(context.unmanagedPtr, output.unmanagedPtr))
                throw new InvalidOperationException("Cannot perform GUI rendering.");
            guiRoot.Draw();
            Internal_DrawEnd();
#endif
        }

        /// <summary>
        /// Calls drawing GUI to the texture using custom View*Projection matrix.
        /// If depth buffer texture is provided there will be depth test performed during rendering.
        /// </summary>
        /// <param name="guiRoot">The root control of the GUI to draw.</param>
        /// <param name="context">The GPU context to handle graphics commands.</param>
        /// <param name="output">The output render target.</param>
        /// <param name="depthBuffer">The depth buffer render target. It's optional parameter but if provided must match output texture.</param>
        /// <param name="viewProjection">The View*Projection matrix used to transform all rendered vertices.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void CallDrawing(Control guiRoot, GPUContext context, RenderTarget output, RenderTarget depthBuffer, ref Matrix viewProjection)
        {
            if (context == null || output == null || guiRoot == null)
                throw new ArgumentNullException();
            if (depthBuffer != null)
            {
                if (!depthBuffer.IsAllocated)
                    throw new InvalidOperationException("Depth buffer is not allocated. Use RenderTarget.Init before rendering.");
                if (output.Size != depthBuffer.Size)
                    throw new InvalidOperationException("Output buffer and depth buffer dimensions must be equal.");
            }

#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            if (Internal_DrawBegin2(context.unmanagedPtr, output.unmanagedPtr, Object.GetUnmanagedPtr(depthBuffer), ref viewProjection))
                throw new InvalidOperationException("Cannot perform GUI rendering.");
            guiRoot.Draw();
            Internal_DrawEnd();
#endif
        }

        /// <summary>
        /// Draws sprite.
        /// </summary>
        /// <param name="sprite">Sprite to draw.</param>
        /// <param name="rect">Rectangle to draw.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void DrawSprite(Sprite sprite, Rectangle rect)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Color color = Color.White;
            Internal_DrawSprite(Object.GetUnmanagedPtr(sprite.Atlas), sprite.Index, ref rect, ref color);
#endif
        }

        /// <summary>
        /// Draws sprite.
        /// </summary>
        /// <param name="sprite">Sprite to draw.</param>
        /// <param name="rect">Rectangle to draw.</param>
        /// <param name="color">Color to multiply all texture pixels.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void DrawSprite(Sprite sprite, Rectangle rect, Color color)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_DrawSprite(Object.GetUnmanagedPtr(sprite.Atlas), sprite.Index, ref rect, ref color);
#endif
        }

        /// <summary>
        /// Draws sprite (uses point sampler).
        /// </summary>
        /// <param name="sprite">Sprite to draw.</param>
        /// <param name="rect">Rectangle to draw.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void DrawSpritePoint(Sprite sprite, Rectangle rect)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Color color = Color.White;
            Internal_DrawSpritePoint(Object.GetUnmanagedPtr(sprite.Atlas), sprite.Index, ref rect, ref color);
#endif
        }

        /// <summary>
        /// Draws sprite (uses point sampler).
        /// </summary>
        /// <param name="sprite">Sprite to draw.</param>
        /// <param name="rect">Rectangle to draw.</param>
        /// <param name="color">Color to multiply all texture pixels.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void DrawSpritePoint(Sprite sprite, Rectangle rect, Color color)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_DrawSpritePoint(Object.GetUnmanagedPtr(sprite.Atlas), sprite.Index, ref rect, ref color);
#endif
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_DrawBegin1(IntPtr context, IntPtr output);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_DrawBegin2(IntPtr context, IntPtr output, IntPtr depth, ref Matrix viewProjection);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawEnd();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawSprite(IntPtr atlas, int index, ref Rectangle rect, ref Color color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawSpritePoint(IntPtr atlas, int index, ref Rectangle rect, ref Color color);
#endif

        #endregion
    }
}
