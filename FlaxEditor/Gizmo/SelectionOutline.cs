////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// In-build postFx used to render outline for selected objects in editor.
    /// </summary>
    public sealed class SelectionOutline : PostProcessEffect
    {
        private Material _outlineMaterial;
        private MaterialInstance _material;

        /// <summary>
        /// Gets or sets the first outline color.
        /// </summary>
        public Color OutlineColor0 { get; set; } = new Color(0.039f, 0.827f, 0.156f);

        /// <summary>
        /// Gets or sets the second outline color.
        /// </summary>
        public Color OutlineColor1 { get; set; } = new Color(0.019f, 0.615f, 0.101f);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionOutline"/> class.
        /// </summary>
        public SelectionOutline()
        {
            _outlineMaterial = FlaxEngine.Content.LoadAsyncInternal<Material>("Editor/Gizmo/SelectionOutlineMaterial");
            if (_outlineMaterial)
            {
                _material = _outlineMaterial.CreateVirtualInstance();
            }
            else
            {
                Editor.LogWarning("Failed to load gizmo selection outline material");
            }
        }

        /// <inheritdoc />
        public override bool CanRender => _outlineMaterial && _outlineMaterial.IsLoaded && Editor.Instance.SceneEditing.HasSthSelected;

        /// <inheritdoc />
        public override void Render(GPUContext context, SceneRenderTask task, RenderTarget input, RenderTarget output)
        {
			Profiler.BeginEventGPU("Selection Outline");

            // Pick a temporary depth buffer
            var customDepth = RenderTarget.GetTemporary(PixelFormat.R32_Typeless, input.Width, input.Height, TextureFlags.DepthStencil | TextureFlags.ShaderResource);
            context.ClearDepth(customDepth);

            // Render selected objects depth
            var actors = Editor.Instance.SceneEditing.Selection.ConvertAll(x => (x as ActorNode)?.Actor).ToArray();
            context.DrawSceneDepth(task, customDepth, true, actors, ActorsSources.CustomActors);

            var near = task.View.Near;
            var far = task.View.Far;
            var projection = task.View.Projection;

            // Render outline
            _material.GetParam("OutlineColor0").Value = OutlineColor0;
            _material.GetParam("OutlineColor1").Value = OutlineColor1;
            _material.GetParam("CustomDepth").Value = customDepth;
            _material.GetParam("ViewInfo").Value = new Vector4(1.0f / projection.M11, 1.0f / projection.M22, far / (far - near), (-far * near) / (far - near) / far);
            context.DrawPostFxMaterial(_material, output, input, task);

            // Cleanup
            RenderTarget.ReleaseTemporary(customDepth);

			Profiler.EndEventGPU();
        }
    }
}
