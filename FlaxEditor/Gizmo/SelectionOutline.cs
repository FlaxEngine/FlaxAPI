// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

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
    public class SelectionOutline : PostProcessEffect
    {
        private Material _outlineMaterial;
        private MaterialInstance _material;
        private Color _color0, _color1;
        private bool _enabled;
        private bool _useEditorOptions;

        /// <summary>
        /// The cached actors list used for drawing (reusable to reduce memory allocations). Always cleared before and after objects rendering.
        /// </summary>
        protected List<Actor> _actors;

        /// <summary>
        /// The selection getter.
        /// </summary>
        public Func<List<SceneGraphNode>> SelectionGetter;

        /// <summary>
        /// Gets or sets a value indicating whether show selection outline effect.
        /// </summary>
        public bool ShowSelectionOutline
        {
            get => _enabled;
            set => _enabled = value;
        }

        /// <summary>
        /// Gets or sets the selection outline first color (top of the screen-space gradient).
        /// </summary>
        public Color SelectionOutlineColor0
        {
            get => _color0;
            set => _color0 = value;
        }

        /// <summary>
        /// Gets or sets the selection outline second color (bottom of the screen-space gradient).
        /// </summary>
        public Color SelectionOutlineColor1
        {
            get => _color1;
            set => _color1 = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether use editor options for selection outline color and visibility. Otherwise, if disabled it can be controlled from code.
        /// </summary>
        public bool UseEditorOptions
        {
            get => _useEditorOptions;
            set
            {
                if (_useEditorOptions != value)
                {
                    var options = Editor.Instance.Options;

                    if (_useEditorOptions)
                        options.OptionsChanged -= OnOptionsChanged;

                    _useEditorOptions = value;

                    if (_useEditorOptions)
                        options.OptionsChanged += OnOptionsChanged;
                }
            }
        }

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

            _useEditorOptions = true;
            var options = Editor.Instance.Options;
            options.OptionsChanged += OnOptionsChanged;
            OnOptionsChanged(options.Options);
        }

        private void OnOptionsChanged(EditorOptions options)
        {
            _enabled = options.Visual.ShowSelectionOutline;
            _color0 = options.Visual.SelectionOutlineColor0;
            _color1 = options.Visual.SelectionOutlineColor1;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has data ready.
        /// </summary>
        protected bool HasDataReady => _enabled && _material && _outlineMaterial.IsLoaded;

        /// <inheritdoc />
        public override bool CanRender => _enabled && _material && _outlineMaterial.IsLoaded && SelectionGetter().Count > 0;

        /// <inheritdoc />
        public override void Render(GPUContext context, SceneRenderTask task, RenderTarget input, RenderTarget output)
        {
            Profiler.BeginEventGPU("Selection Outline");

            // Pick a temporary depth buffer
            var customDepth = RenderTarget.GetTemporary(PixelFormat.R32_Typeless, input.Width, input.Height, TextureFlags.DepthStencil | TextureFlags.ShaderResource);
            context.ClearDepth(customDepth);

            // Draw objects to depth buffer
            if (_actors == null)
                _actors = new List<Actor>();
            else
                _actors.Clear();
            DrawSelectionDepth(context, task, customDepth);
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

        /// <summary>
        /// Draws the selected object to depth buffer.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="task">The task.</param>
        /// <param name="customDepth">The custom depth (output).</param>
        protected virtual void DrawSelectionDepth(GPUContext context, SceneRenderTask task, RenderTarget customDepth)
        {
            // Get selected actors
            var selection = SelectionGetter();
            _actors.Capacity = Mathf.NextPowerOfTwo(Mathf.Max(_actors.Capacity, selection.Count));
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i] is ActorNode actorNode)
                    _actors.Add(actorNode.Actor);
            }

            // Render selected objects depth
            context.DrawSceneDepth(task, customDepth, _actors, ActorsSources.CustomActors);
        }
    }
}
