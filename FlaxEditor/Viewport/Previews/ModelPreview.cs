// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Viewport.Cameras;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Model asset preview editor viewport.
    /// </summary>
    /// <seealso cref="AssetPreview" />
    public class ModelPreview : AssetPreview
    {
        private StaticModel _previewModel;

        /// <summary>
        /// Gets or sets the model asset to preview.
        /// </summary>
        public Model Model
        {
            get => _previewModel.Model;
            set => _previewModel.Model = value;
        }

        /// <summary>
        /// Gets the model actor used to preview selected asset.
        /// </summary>
        public StaticModel PreviewStaticModel => _previewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public ModelPreview(bool useWidgets)
        : base(useWidgets)
        {
            Task.Begin += OnBegin;

            // Setup preview scene
            _previewModel = StaticModel.New();

            // Link actors for rendering
            Task.CustomActors.Add(_previewModel);

            if (useWidgets)
            {
                // Preview LOD
                {
                    var previewLOD = ViewWidgetButtonMenu.AddButton("Preview LOD");
                    var previewLODValue = new IntValueBox(-1, 75, 2, 50.0f, -1, 10, 0.02f);
                    previewLODValue.Parent = previewLOD;
                    previewLODValue.ValueChanged += () => _previewModel.ForcedLOD = previewLODValue.Value;
                    ViewWidgetButtonMenu.VisibleChanged += control => previewLODValue.Value = _previewModel.ForcedLOD;
                }
            }
        }

        private void OnBegin(SceneRenderTask task, GPUContext context)
        {
            // Update preview model scale to fit the preview
            var model = Model;
            if (model && model.IsLoaded)
            {
                float targetSize = 30.0f;
                float maxSize = Mathf.Max(0.001f, model.Box.Size.MaxValue);
                _previewModel.Scale = new Vector3(targetSize / maxSize);
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to cleanup created actor objects
            Object.Destroy(ref _previewModel);

            base.OnDestroy();
        }
    }
}
