// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// High-level rendering service.
    /// </summary>
    [Tooltip("High-level rendering service.")]
    public static unsafe partial class Renderer
    {
        /// <summary>
        /// Draws scene objects depth (to the output Z buffer). The output must be depth texture to write hardware depth to it.
        /// </summary>
        /// <param name="context">The GPU commands context to use.</param>
        /// <param name="task">Render task to use it's view description and the render buffers.</param>
        /// <param name="output">The output texture. Must be valid and created.</param>
        /// <param name="customActors">The custom set of actors to render. If empty, the loaded scenes will be rendered.</param>
        public static void DrawSceneDepth(GPUContext context, SceneRenderTask task, GPUTexture output, Actor[] customActors)
        {
            Internal_DrawSceneDepth(FlaxEngine.Object.GetUnmanagedPtr(context), FlaxEngine.Object.GetUnmanagedPtr(task), FlaxEngine.Object.GetUnmanagedPtr(output), customActors);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawSceneDepth(IntPtr context, IntPtr task, IntPtr output, Actor[] customActors);

        /// <summary>
        /// Draws postFx material to the render target.
        /// </summary>
        /// <param name="context">The GPU commands context to use.</param>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="material">The material to render. It must be a post fx material.</param>
        /// <param name="output">The output texture. Must be valid and created.</param>
        /// <param name="input">The input texture. It's optional.</param>
        public static void DrawPostFxMaterial(GPUContext context, ref RenderContext renderContext, MaterialBase material, GPUTexture output, GPUTextureView input)
        {
            Internal_DrawPostFxMaterial(FlaxEngine.Object.GetUnmanagedPtr(context), ref renderContext, FlaxEngine.Object.GetUnmanagedPtr(material), FlaxEngine.Object.GetUnmanagedPtr(output), FlaxEngine.Object.GetUnmanagedPtr(input));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawPostFxMaterial(IntPtr context, ref RenderContext renderContext, IntPtr material, IntPtr output, IntPtr input);
    }
}
