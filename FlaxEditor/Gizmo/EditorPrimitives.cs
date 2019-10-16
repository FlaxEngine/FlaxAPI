// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// Interface for editor viewports that can contain and use <see cref="EditorPrimitives"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.IGizmoOwner" />
    public interface IEditorPrimitivesOwner : IGizmoOwner
    {
        /// <summary>
        /// Gets the debug draw data container.
        /// </summary>
        ViewportDebugDrawData DebugDrawData { get; }
    }

    /// <summary>
    /// In-build postFx used to render debug shapes, gizmo tools and other editor primitives to MSAA render target and composite it with the editor preview window.
    /// </summary>
    public sealed class EditorPrimitives : PostProcessEffect
    {
        private readonly DrawCallsCollector _drawCallsCollector = new DrawCallsCollector();

        /// <summary>
        /// Gets or sets a value indicating whether draw <see cref="DebugDraw"/> shapes.
        /// </summary>
        public bool DrawDebugDraw = false;

        /// <summary>
        /// The target viewport.
        /// </summary>
        public IEditorPrimitivesOwner Viewport;

        /// <inheritdoc />
        public override int Order => -100;

        /// <inheritdoc />
        public override void Render(GPUContext context, SceneRenderTask task, RenderTarget input, RenderTarget output)
        {
            if (Viewport == null)
                throw new NullReferenceException();

            Profiler.BeginEventGPU("Editor Primitives");

            // Check if use MSAA
            var format = output.Format;
            GraphicsDevice.GetFeatures(format, out var formatSupport);
            bool enableMsaa = formatSupport.MSAALevelMax >= MSAALevel.X4 && Editor.Instance.Options.Options.Visual.EnableMSAAForDebugDraw;

            // Prepare
            var msaaLevel = enableMsaa ? MSAALevel.X4 : MSAALevel.None;
            var width = output.Width;
            var height = output.Height;
            var target = RenderTarget.GetTemporary(format, width, height, TextureFlags.RenderTarget | TextureFlags.ShaderResource, msaaLevel);
            var targetDepth = RenderTarget.GetTemporary(PixelFormat.D24_UNorm_S8_UInt, width, height, TextureFlags.DepthStencil, msaaLevel);

            // Copy frame and clear depth
            context.Draw(target, input);
            context.ClearDepth(targetDepth);

            // Draw gizmos (collect draw calls only)
            _drawCallsCollector.Clear();
            for (int i = 0; i < Viewport.Gizmos.Count; i++)
            {
                Viewport.Gizmos[i].Draw(_drawCallsCollector);
            }

            // Draw selected objects debug shapes and visuals
            if (DrawDebugDraw && (task.View.Flags & ViewFlags.DebugDraw) == ViewFlags.DebugDraw)
            {
                var debugDrawData = Viewport.DebugDrawData;
                DebugDraw.Draw(task, debugDrawData.ActorsPtrs, target, context, targetDepth, true);
            }

            // Draw gizmos (actual drawing)
            _drawCallsCollector.ExecuteDrawCalls(context, task, target, DrawPass.Forward);
            _drawCallsCollector.ExecuteDrawCalls(context, task, target, DrawPass.GBuffer);

            // Resolve MSAA texture
            if (enableMsaa)
                context.ResolveMultisample(target, output);
            else
                context.Draw(output, target);

            // Cleanup
            RenderTarget.ReleaseTemporary(targetDepth);
            RenderTarget.ReleaseTemporary(target);

            Profiler.EndEventGPU();
        }
    }
}
