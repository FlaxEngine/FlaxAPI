// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public static partial class Render2D
    {
        /// <summary>
        /// Draws a text.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="layoutRect">The size and position of the area in which the text is drawn.</param>
        /// <param name="color">The text color.</param>
        /// <param name="horizontalAlignment">The horizontal alignment of the text in a layout rectangle.</param>
        /// <param name="verticalAlignment">The vertical alignment of the text in a layout rectangle.</param>
        /// <param name="textWrapping">Describes how wrap text inside a layout rectangle.</param>
        /// <param name="baseLinesGapScale">The scale for distance one baseline from another. Default is 1.</param>
        /// <param name="scale">The text drawing scale. Default is 1.</param>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void DrawText(Font font, string text, Rectangle layoutRect, Color color, TextAlignment horizontalAlignment = TextAlignment.Near, TextAlignment verticalAlignment = TextAlignment.Near, TextWrapping textWrapping = TextWrapping.NoWrap, float baseLinesGapScale = 1.0f, float scale = 1.0f)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            TextLayoutOptions layout;
            layout.Bounds = layoutRect;
            layout.HorizontalAlignment = horizontalAlignment;
            layout.VerticalAlignment = verticalAlignment;
            layout.TextWrapping = textWrapping;
            layout.Scale = scale;
            layout.BaseLinesGapScale = baseLinesGapScale;
            Internal_DrawText1(Object.GetUnmanagedPtr(font), text, ref color, ref layout);
#endif
        }

        /// <summary>
        /// Draws a text using a custom material shader. Given material must have GUI domain and a public parameter named Font (texture parameter used for a font atlas sampling).
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="customMaterial">Custom material for font characters rendering. It must contain texture parameter named Font used to sample font texture.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="layoutRect">The size and position of the area in which the text is drawn.</param>
        /// <param name="color">The text color.</param>
        /// <param name="horizontalAlignment">The horizontal alignment of the text in a layout rectangle.</param>
        /// <param name="verticalAlignment">The vertical alignment of the text in a layout rectangle.</param>
        /// <param name="textWrapping">Describes how wrap text inside a layout rectangle.</param>
        /// <param name="baseLinesGapScale">The scale for distance one baseline from another. Default is 1.</param>
        /// <param name="scale">The text drawing scale. Default is 1.</param>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void DrawText(Font font, MaterialBase customMaterial, string text, Rectangle layoutRect, Color color, TextAlignment horizontalAlignment = TextAlignment.Near, TextAlignment verticalAlignment = TextAlignment.Near, TextWrapping textWrapping = TextWrapping.NoWrap, float baseLinesGapScale = 1.0f, float scale = 1.0f)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            TextLayoutOptions layout;
            layout.Bounds = layoutRect;
            layout.HorizontalAlignment = horizontalAlignment;
            layout.VerticalAlignment = verticalAlignment;
            layout.TextWrapping = textWrapping;
            layout.Scale = scale;
            layout.BaseLinesGapScale = baseLinesGapScale;
            Internal_DrawText2(Object.GetUnmanagedPtr(font), Object.GetUnmanagedPtr(customMaterial), text, ref color, ref layout);
#endif
        }

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
        /// <remarks>This method should be called only during <see cref="PostProcessEffect.Render"/></remarks>
        /// <param name="drawableElement">The root container for Draw methods.</param>
        /// <param name="context">The GPU context to handle graphics commands.</param>
        /// <param name="output">The output render target.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void CallDrawing(IDrawable drawableElement, GPUContext context, RenderTarget output)
        {
            if (context == null || output == null || drawableElement == null)
                throw new ArgumentNullException();

#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            if (Internal_DrawBegin1(context.unmanagedPtr, output.unmanagedPtr))
                throw new InvalidOperationException("Cannot perform GUI rendering.");
            try
            {
                drawableElement.Draw();
            }
            finally
            {
                Internal_DrawEnd();
            }
#endif
        }

        /// <summary>
        /// Calls drawing GUI to the texture using custom View*Projection matrix.
        /// If depth buffer texture is provided there will be depth test performed during rendering.
        /// </summary>
        /// <remarks>This method should be called only during <see cref="PostProcessEffect.Render"/></remarks>
        /// <param name="drawableElement">The root container for Draw methods.</param>
        /// <param name="context">The GPU context to handle graphics commands.</param>
        /// <param name="output">The output render target.</param>
        /// <param name="depthBuffer">The depth buffer render target. It's optional parameter but if provided must match output texture.</param>
        /// <param name="viewProjection">The View*Projection matrix used to transform all rendered vertices.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void CallDrawing(IDrawable drawableElement, GPUContext context, RenderTarget output, RenderTarget depthBuffer, ref Matrix viewProjection)
        {
            if (context == null || output == null || drawableElement == null)
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
            try
            {
                drawableElement.Draw();
            }
            finally
            {
                Internal_DrawEnd();
            }
#endif
        }

        /// <summary>
        /// Draws a sprite.
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
        /// Draws a sprite.
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
        /// Draws a sprite (uses point sampler).
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
        /// Draws a sprite (uses point sampler).
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
