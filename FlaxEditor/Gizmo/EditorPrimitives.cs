// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// Interface for editor viewports that can contain and use <see cref="EditorPrimitives"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.IGizmoOwner" />
    public interface IEditorPrimitivesOwner : IGizmoOwner
    {
        /// <summary>
        /// Draws the custom editor primitives.
        /// </summary>
        /// <param name="context">The GPU commands context.</param>
        /// <param name="task">The current scene rendering task.</param>
        /// <param name="target">The output texture to render to.</param>
        /// <param name="targetDepth">The scene depth buffer that can be used to z-buffering.</param>
        /// <param name="collector">The draw calls collector.</param>
        void DrawEditorPrimitives(GPUContext context, SceneRenderTask task, GPUTexture target, GPUTexture targetDepth, DrawCallsCollector collector);
    }

    /// <summary>
    /// In-build postFx used to render debug shapes, gizmo tools and other editor primitives to MSAA render target and composite it with the editor preview window.
    /// </summary>
    public sealed class EditorPrimitives : PostProcessEffect
    {
        private readonly DrawCallsCollector _drawCallsCollector = new DrawCallsCollector();

        /// <summary>
        /// The target viewport.
        /// </summary>
        public IEditorPrimitivesOwner Viewport;

        /// <inheritdoc />
        public override int Order => -100;

        /// <inheritdoc />
        public override void Render(GPUContext context, SceneRenderTask task, GPUTexture input, GPUTexture output)
        {
            if (Viewport == null)
                throw new NullReferenceException();

            Profiler.BeginEventGPU("Editor Primitives");

            // Check if use MSAA
            var format = output.Format;
            GPUDevice.GetFeatures(format, out var formatSupport);
            bool enableMsaa = formatSupport.MSAALevelMax >= MSAALevel.X4 && Editor.Instance.Options.Options.Visual.EnableMSAAForDebugDraw;

            // Prepare
            var msaaLevel = enableMsaa ? MSAALevel.X4 : MSAALevel.None;
            var width = output.Width;
            var height = output.Height;
            var desc = GPUTextureDescription.New2D(width, height, format, GPUTextureFlags.RenderTarget | GPUTextureFlags.ShaderResource, 1, 1, msaaLevel);
            var target = RenderTargetPool.Get(ref desc);
            desc = GPUTextureDescription.New2D(width, height, PixelFormat.D24_UNorm_S8_UInt, GPUTextureFlags.DepthStencil, 1, 1, msaaLevel);
            var targetDepth = RenderTargetPool.Get(ref desc);

            // Copy frame and clear depth
            context.Draw(target, input);
            context.ClearDepth(targetDepth);

            // Draw gizmos and other editor primitives (collect draw calls only)
            _drawCallsCollector.Clear();
            for (int i = 0; i < Viewport.Gizmos.Count; i++)
            {
                Viewport.Gizmos[i].Draw(_drawCallsCollector);
            }
            Viewport.DrawEditorPrimitives(context, task, target, targetDepth, _drawCallsCollector);

            // Draw gizmos (actual drawing)
            _drawCallsCollector.ExecuteDrawCalls(context, task, target, DrawPass.GBuffer);
            _drawCallsCollector.ExecuteDrawCalls(context, task, target, DrawPass.Forward);

            // Resolve MSAA texture
            if (enableMsaa)
                context.ResolveMultisample(target, output);
            else
                context.Draw(output, target);

            // Cleanup
            RenderTargetPool.Release(targetDepth);
            RenderTargetPool.Release(target);

            Profiler.EndEventGPU();
        }
    }
}
