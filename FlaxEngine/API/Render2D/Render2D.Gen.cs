// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Rendering 2D shapes and text using Graphics Device.
    /// </summary>
    [Tooltip("Rendering 2D shapes and text using Graphics Device.")]
    public static unsafe partial class Render2D
    {
        /// <summary>
        /// The active rendering features flags.
        /// </summary>
        [Tooltip("The active rendering features flags.")]
        public static RenderingFeatures Features
        {
            get { return Internal_GetFeatures(); }
            set { Internal_SetFeatures(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RenderingFeatures Internal_GetFeatures();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFeatures(RenderingFeatures value);

        /// <summary>
        /// Begins the rendering phrase.
        /// </summary>
        /// <param name="context">The GPU commands context to use.</param>
        /// <param name="output">The output target.</param>
        /// <param name="depthBuffer">The depth buffer.</param>
        public static void Begin(GPUContext context, GPUTexture output, GPUTexture depthBuffer = null)
        {
            Internal_Begin(FlaxEngine.Object.GetUnmanagedPtr(context), FlaxEngine.Object.GetUnmanagedPtr(output), FlaxEngine.Object.GetUnmanagedPtr(depthBuffer));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Begin(IntPtr context, IntPtr output, IntPtr depthBuffer);

        /// <summary>
        /// Begins the rendering phrase.
        /// </summary>
        /// <param name="context">The GPU commands context to use.</param>
        /// <param name="output">The output target.</param>
        /// <param name="depthBuffer">The depth buffer.</param>
        /// <param name="viewProjection">The View*Projection matrix. Allows to render GUI in 3D or with custom transformations.</param>
        public static void Begin(GPUContext context, GPUTexture output, GPUTexture depthBuffer, ref Matrix viewProjection)
        {
            Internal_Begin1(FlaxEngine.Object.GetUnmanagedPtr(context), FlaxEngine.Object.GetUnmanagedPtr(output), FlaxEngine.Object.GetUnmanagedPtr(depthBuffer), ref viewProjection);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Begin1(IntPtr context, IntPtr output, IntPtr depthBuffer, ref Matrix viewProjection);

        /// <summary>
        /// Begins the rendering phrase.
        /// </summary>
        /// <param name="context">The GPU commands context to use.</param>
        /// <param name="output">The output target.</param>
        /// <param name="depthBuffer">The depth buffer.</param>
        /// <param name="viewport">The output viewport.</param>
        public static void Begin(GPUContext context, GPUTextureView output, GPUTextureView depthBuffer, ref Viewport viewport)
        {
            Internal_Begin2(FlaxEngine.Object.GetUnmanagedPtr(context), FlaxEngine.Object.GetUnmanagedPtr(output), FlaxEngine.Object.GetUnmanagedPtr(depthBuffer), ref viewport);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Begin2(IntPtr context, IntPtr output, IntPtr depthBuffer, ref Viewport viewport);

        /// <summary>
        /// Begins the rendering phrase.
        /// </summary>
        /// <param name="context">The GPU commands context to use.</param>
        /// <param name="output">The output target.</param>
        /// <param name="depthBuffer">The depth buffer.</param>
        /// <param name="viewport">The output viewport.</param>
        /// <param name="viewProjection">The View*Projection matrix. Allows to render GUI in 3D or with custom transformations.</param>
        public static void Begin(GPUContext context, GPUTextureView output, GPUTextureView depthBuffer, ref Viewport viewport, ref Matrix viewProjection)
        {
            Internal_Begin3(FlaxEngine.Object.GetUnmanagedPtr(context), FlaxEngine.Object.GetUnmanagedPtr(output), FlaxEngine.Object.GetUnmanagedPtr(depthBuffer), ref viewport, ref viewProjection);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Begin3(IntPtr context, IntPtr output, IntPtr depthBuffer, ref Viewport viewport, ref Matrix viewProjection);

        /// <summary>
        /// Ends the rendering phrase.
        /// </summary>
        public static void End()
        {
            Internal_End();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_End();

        /// <summary>
        /// Pushes transformation layer.
        /// </summary>
        /// <param name="transform">The transformation to apply.</param>
        public static void PushTransform(ref Matrix3x3 transform)
        {
            Internal_PushTransform(ref transform);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_PushTransform(ref Matrix3x3 transform);

        /// <summary>
        /// Pops transformation layer.
        /// </summary>
        public static void PopTransform()
        {
            Internal_PopTransform();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_PopTransform();

        /// <summary>
        /// Pushes clipping rectangle mask.
        /// </summary>
        /// <param name="clipRect">The axis aligned clipping mask rectangle.</param>
        public static void PushClip(ref Rectangle clipRect)
        {
            Internal_PushClip(ref clipRect);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_PushClip(ref Rectangle clipRect);

        /// <summary>
        /// Pops clipping rectangle mask.
        /// </summary>
        public static void PopClip()
        {
            Internal_PopClip();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_PopClip();

        /// <summary>
        /// Draws a text.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="color">The text color.</param>
        /// <param name="location">The text location.</param>
        /// <param name="customMaterial">The custom material for font characters rendering. It must contain texture parameter named Font used to sample font texture.</param>
        public static void DrawText(Font font, string text, Color color, Vector2 location, MaterialBase customMaterial = null)
        {
            Internal_DrawText(FlaxEngine.Object.GetUnmanagedPtr(font), text, ref color, ref location, FlaxEngine.Object.GetUnmanagedPtr(customMaterial));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawText(IntPtr font, string text, ref Color color, ref Vector2 location, IntPtr customMaterial);

        /// <summary>
        /// Draws a text.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="textRange">The input text range (substring range of the input text parameter).</param>
        /// <param name="color">The text color.</param>
        /// <param name="location">The text location.</param>
        /// <param name="customMaterial">The custom material for font characters rendering. It must contain texture parameter named Font used to sample font texture.</param>
        public static void DrawText(Font font, string text, ref TextRange textRange, Color color, Vector2 location, MaterialBase customMaterial = null)
        {
            Internal_DrawText1(FlaxEngine.Object.GetUnmanagedPtr(font), text, ref textRange, ref color, ref location, FlaxEngine.Object.GetUnmanagedPtr(customMaterial));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawText1(IntPtr font, string text, ref TextRange textRange, ref Color color, ref Vector2 location, IntPtr customMaterial);

        /// <summary>
        /// Draws a text with formatting.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="color">The text color.</param>
        /// <param name="layout">The text layout properties.</param>
        /// <param name="customMaterial">The custom material for font characters rendering. It must contain texture parameter named Font used to sample font texture.</param>
        public static void DrawText(Font font, string text, Color color, ref TextLayoutOptions layout, MaterialBase customMaterial = null)
        {
            Internal_DrawText2(FlaxEngine.Object.GetUnmanagedPtr(font), text, ref color, ref layout, FlaxEngine.Object.GetUnmanagedPtr(customMaterial));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawText2(IntPtr font, string text, ref Color color, ref TextLayoutOptions layout, IntPtr customMaterial);

        /// <summary>
        /// Draws a text with formatting.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="textRange">The input text range (substring range of the input text parameter).</param>
        /// <param name="color">The text color.</param>
        /// <param name="layout">The text layout properties.</param>
        /// <param name="customMaterial">The custom material for font characters rendering. It must contain texture parameter named Font used to sample font texture.</param>
        public static void DrawText(Font font, string text, ref TextRange textRange, Color color, ref TextLayoutOptions layout, MaterialBase customMaterial = null)
        {
            Internal_DrawText3(FlaxEngine.Object.GetUnmanagedPtr(font), text, ref textRange, ref color, ref layout, FlaxEngine.Object.GetUnmanagedPtr(customMaterial));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawText3(IntPtr font, string text, ref TextRange textRange, ref Color color, ref TextLayoutOptions layout, IntPtr customMaterial);

        /// <summary>
        /// Fills a rectangle area.
        /// </summary>
        /// <param name="rect">The rectangle to fill.</param>
        /// <param name="color">The color to use.</param>
        public static void FillRectangle(Rectangle rect, Color color)
        {
            Internal_FillRectangle(ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_FillRectangle(ref Rectangle rect, ref Color color);

        /// <summary>
        /// Fills a rectangle area.
        /// </summary>
        /// <param name="rect">The rectangle to fill.</param>
        /// <param name="color1">The color to use for upper left vertex.</param>
        /// <param name="color2">The color to use for upper right vertex.</param>
        /// <param name="color3">The color to use for bottom right vertex.</param>
        /// <param name="color4">The color to use for bottom left vertex.</param>
        public static void FillRectangle(Rectangle rect, Color color1, Color color2, Color color3, Color color4)
        {
            Internal_FillRectangle1(ref rect, ref color1, ref color2, ref color3, ref color4);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_FillRectangle1(ref Rectangle rect, ref Color color1, ref Color color2, ref Color color3, ref Color color4);

        /// <summary>
        /// Draws a rectangle borders.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color to use.</param>
        /// <param name="thickness">The line thickness.</param>
        public static void DrawRectangle(Rectangle rect, Color color, float thickness = 1.0f)
        {
            Internal_DrawRectangle(ref rect, ref color, thickness);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawRectangle(ref Rectangle rect, ref Color color, float thickness);

        /// <summary>
        /// Draws a rectangle borders.
        /// </summary>
        /// <param name="rect">The rectangle to fill.</param>
        /// <param name="color1">The color to use for upper left vertex.</param>
        /// <param name="color2">The color to use for upper right vertex.</param>
        /// <param name="color3">The color to use for bottom right vertex.</param>
        /// <param name="color4">The color to use for bottom left vertex.</param>
        /// <param name="thickness">The line thickness.</param>
        public static void DrawRectangle(Rectangle rect, Color color1, Color color2, Color color3, Color color4, float thickness = 1.0f)
        {
            Internal_DrawRectangle1(ref rect, ref color1, ref color2, ref color3, ref color4, thickness);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawRectangle1(ref Rectangle rect, ref Color color1, ref Color color2, ref Color color3, ref Color color4, float thickness);

        /// <summary>
        /// Draws the render target.
        /// </summary>
        /// <param name="rt">The render target handle to draw.</param>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color to multiply all texture pixels.</param>
        public static void DrawTexture(GPUTextureView rt, Rectangle rect, Color color)
        {
            Internal_DrawTexture(FlaxEngine.Object.GetUnmanagedPtr(rt), ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawTexture(IntPtr rt, ref Rectangle rect, ref Color color);

        /// <summary>
        /// Draws the texture.
        /// </summary>
        /// <param name="t">The texture to draw.</param>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color to multiply all texture pixels.</param>
        public static void DrawTexture(GPUTexture t, Rectangle rect, Color color)
        {
            Internal_DrawTexture1(FlaxEngine.Object.GetUnmanagedPtr(t), ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawTexture1(IntPtr t, ref Rectangle rect, ref Color color);

        /// <summary>
        /// Draws the texture.
        /// </summary>
        /// <param name="t">The texture to draw.</param>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color to multiply all texture pixels.</param>
        public static void DrawTexture(TextureBase t, Rectangle rect, Color color)
        {
            Internal_DrawTexture2(FlaxEngine.Object.GetUnmanagedPtr(t), ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawTexture2(IntPtr t, ref Rectangle rect, ref Color color);

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="spriteHandle">The sprite to draw.</param>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color to multiply all texture pixels.</param>
        public static void DrawSprite(SpriteHandle spriteHandle, Rectangle rect, Color color)
        {
            Internal_DrawSprite(ref spriteHandle, ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawSprite(ref SpriteHandle spriteHandle, ref Rectangle rect, ref Color color);

        /// <summary>
        /// Draws the texture (uses point sampler).
        /// </summary>
        /// <param name="t">The texture to draw.</param>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color to multiply all texture pixels.</param>
        public static void DrawTexturePoint(GPUTexture t, Rectangle rect, Color color)
        {
            Internal_DrawTexturePoint(FlaxEngine.Object.GetUnmanagedPtr(t), ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawTexturePoint(IntPtr t, ref Rectangle rect, ref Color color);

        /// <summary>
        /// Draws a sprite (uses point sampler).
        /// </summary>
        /// <param name="spriteHandle">The sprite to draw.</param>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color to multiply all texture pixels.</param>
        public static void DrawSpritePoint(SpriteHandle spriteHandle, Rectangle rect, Color color)
        {
            Internal_DrawSpritePoint(ref spriteHandle, ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawSpritePoint(ref SpriteHandle spriteHandle, ref Rectangle rect, ref Color color);

        /// <summary>
        /// Performs custom rendering.
        /// </summary>
        /// <param name="t">The texture to use.</param>
        /// <param name="rect">The rectangle area to draw.</param>
        /// <param name="ps">The custom pipeline state to use (input must match default Render2D vertex shader and can use single texture).</param>
        /// <param name="color">The color to multiply all texture pixels.</param>
        public static void DrawCustom(GPUTexture t, Rectangle rect, IntPtr ps, Color color)
        {
            Internal_DrawCustom(FlaxEngine.Object.GetUnmanagedPtr(t), ref rect, ps, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawCustom(IntPtr t, ref Rectangle rect, IntPtr ps, ref Color color);

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="p1">The start point.</param>
        /// <param name="p2">The end point.</param>
        /// <param name="color">The line color.</param>
        /// <param name="thickness">The line thickness.</param>
        public static void DrawLine(Vector2 p1, Vector2 p2, Color color, float thickness = 1.0f)
        {
            Internal_DrawLine(ref p1, ref p2, ref color, thickness);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawLine(ref Vector2 p1, ref Vector2 p2, ref Color color, float thickness);

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="p1">The start point.</param>
        /// <param name="p2">The end point.</param>
        /// <param name="color1">The line start color.</param>
        /// <param name="color2">The line end color.</param>
        /// <param name="thickness">The line thickness.</param>
        public static void DrawLine(Vector2 p1, Vector2 p2, Color color1, Color color2, float thickness = 1.0f)
        {
            Internal_DrawLine1(ref p1, ref p2, ref color1, ref color2, thickness);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawLine1(ref Vector2 p1, ref Vector2 p2, ref Color color1, ref Color color2, float thickness);

        /// <summary>
        /// Draws a Bezier curve.
        /// </summary>
        /// <param name="p1">The start point.</param>
        /// <param name="p2">The first control point.</param>
        /// <param name="p3">The second control point.</param>
        /// <param name="p4">The end point.</param>
        /// <param name="color">The line color</param>
        /// <param name="thickness">The line thickness.</param>
        public static void DrawBezier(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness = 1.0f)
        {
            Internal_DrawBezier(ref p1, ref p2, ref p3, ref p4, ref color, thickness);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawBezier(ref Vector2 p1, ref Vector2 p2, ref Vector2 p3, ref Vector2 p4, ref Color color, float thickness);

        /// <summary>
        /// Draws the GUI material.
        /// </summary>
        /// <param name="material">The material to render. Must be a GUI material type.</param>
        /// <param name="rect">The target rectangle to draw.</param>
        /// <param name="color">The color to use.</param>
        public static void DrawMaterial(MaterialBase material, Rectangle rect, Color color)
        {
            Internal_DrawMaterial(FlaxEngine.Object.GetUnmanagedPtr(material), ref rect, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawMaterial(IntPtr material, ref Rectangle rect, ref Color color);

        /// <summary>
        /// Draws the background blur.
        /// </summary>
        /// <param name="rect">The target rectangle to draw (blurs its background).</param>
        /// <param name="blurStrength">The blur strength defines how blurry the background is. Larger numbers increase blur, resulting in a larger runtime cost on the GPU.</param>
        public static void DrawBlur(Rectangle rect, float blurStrength)
        {
            Internal_DrawBlur(ref rect, blurStrength);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawBlur(ref Rectangle rect, float blurStrength);

        /// <summary>
        /// The rendering features and options flags.
        /// </summary>
        [Flags]
        [Tooltip("The rendering features and options flags.")]
        public enum RenderingFeatures
        {
            /// <summary>
            /// The none.
            /// </summary>
            [Tooltip("The none.")]
            None = 0,

            /// <summary>
            /// Enables automatic geometry vertices snapping to integer coordinates in screen space. Reduces aliasing and sampling artifacts. Might be disabled for 3D projection viewport or for complex UI transformations.
            /// </summary>
            [Tooltip("Enables automatic geometry vertices snapping to integer coordinates in screen space. Reduces aliasing and sampling artifacts. Might be disabled for 3D projection viewport or for complex UI transformations.")]
            VertexSnapping = 1,
        }
    }
}
