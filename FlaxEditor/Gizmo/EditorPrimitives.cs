// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
            bool enableMsaa = Editor.Instance.Options.Options.Visual.EnableMSAAForDebugDraw;

            // Prepare
            var msaaLevel = enableMsaa ? MSAALevel.X4 : MSAALevel.None;
            var width = output.Width;
            var height = output.Height;
            var target = RenderTarget.GetTemporary(output.Format, width, height, TextureFlags.RenderTarget | TextureFlags.ShaderResource, msaaLevel);
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
            if (DrawDebugDraw)
            {
                var debugDrawData = Viewport.DebugDrawData;
                DebugDraw.Draw(task, debugDrawData.ActorsPtrs, target, context, targetDepth, true);
            }

            // Draw gizmos (actual drawing)
            _drawCallsCollector.ExecuteDrawCalls(context, task, target, RenderPass.ForwardPass);
            _drawCallsCollector.ExecuteDrawCalls(context, task, target, RenderPass.GBufferFill);

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
