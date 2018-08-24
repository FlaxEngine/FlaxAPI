// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Options;
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
        private Color _color0, _color1;
        private List<Actor> _actors;

        /// <summary>
        /// The selection getter.
        /// </summary>
        public Func<List<SceneGraphNode>> SelectionGetter;

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

            var options = Editor.Instance.Options;
            options.OptionsChanged += OnOptionsChanged;
            OnOptionsChanged(options.Options);
        }

        private void OnOptionsChanged(EditorOptions options)
        {
            _color0 = options.Visual.SelectionOutlineColor0;
            _color1 = options.Visual.SelectionOutlineColor1;
        }

        /// <inheritdoc />
        public override bool CanRender => _material && _outlineMaterial.IsLoaded && SelectionGetter().Count > 0;

        /// <inheritdoc />
        public override void Render(GPUContext context, SceneRenderTask task, RenderTarget input, RenderTarget output)
        {
            Profiler.BeginEventGPU("Selection Outline");

            // Pick a temporary depth buffer
            var customDepth = RenderTarget.GetTemporary(PixelFormat.R32_Typeless, input.Width, input.Height, TextureFlags.DepthStencil | TextureFlags.ShaderResource);
            context.ClearDepth(customDepth);

            // Get selected actors
            var selection = SelectionGetter();
            if (_actors == null)
                _actors = new List<Actor>();
            else
                _actors.Clear();
            _actors.Capacity = Mathf.NextPowerOfTwo(Mathf.Max(_actors.Capacity, selection.Count));
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i] is ActorNode actorNode)
                    _actors.Add(actorNode.Actor);
            }

            // Render selected objects depth
            context.DrawSceneDepth(task, customDepth, true, _actors, ActorsSources.CustomActors);

            _actors.Clear();

            var near = task.View.Near;
            var far = task.View.Far;
            var projection = task.View.Projection;

            // Render outline
            _material.GetParam("OutlineColor0").Value = _color0;
            _material.GetParam("OutlineColor1").Value = _color1;
            _material.GetParam("CustomDepth").Value = customDepth;
            _material.GetParam("ViewInfo").Value = new Vector4(1.0f / projection.M11, 1.0f / projection.M22, far / (far - near), (-far * near) / (far - near) / far);
            context.DrawPostFxMaterial(_material, output, input, task);

            // Cleanup
            RenderTarget.ReleaseTemporary(customDepth);

            Profiler.EndEventGPU();
        }
    }
}
