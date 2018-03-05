////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// In-build postFx used to render debug shapes, gizmo tools and other editor primities to MSAA render target and composite it with the editor preview window.
    /// </summary>
    public sealed class EditorPrimitives : PostProcessEffect
    {
        private readonly DrawCallsCollector _drawCallsCollector = new DrawCallsCollector();

        /// <inheritdoc />
        public override void Render(GPUContext context, SceneRenderTask task, RenderTarget input, RenderTarget output)
        {
			Profiler.BeginEventGPU("Editor Primitives");

            // Check if use MSAA
            // TODO: add edito option to switch between msaa and non-msaa
            bool enableMsaa = true;

            // Prepare
            var editor = Editor.Instance;
            var viewport = editor.Windows.EditWin.Viewport;
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
            for (int i = 0; i < viewport.Gizmos.Count; i++)
            {
                viewport.Gizmos[i].Draw(_drawCallsCollector);
            }

            // Draw selected objects debug shapes and visuals
            var debugDrawData = viewport.DebugDrawData;
            DebugDraw.Draw(task, debugDrawData.ActorsPtrs, target, context, targetDepth, true);

            // Draw gizmos (actual drawing)
            _drawCallsCollector.ExecuteDrawCalls(context, task, target, RenderPass.ForwardPass);

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
