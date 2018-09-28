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
        private ModelActor _previewModel;

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
        public ModelActor PreviewModelActor => _previewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public ModelPreview(bool useWidgets)
        : base(useWidgets)
        {
            Task.Begin += OnBegin;

            // Setup preview scene
            _previewModel = ModelActor.New();

            // Link actors for rendering
            Task.CustomActors.Add(_previewModel);

            if (useWidgets)
            {
                // Forced LOD
                {
                    var forcedLOD = ViewWidgetButtonMenu.AddButton("Preview LOD");
                    var forcedLODValue = new IntValueBox(-1, 75, 2, 50.0f, -1, 10, 0.02f);
                    forcedLODValue.Parent = forcedLOD;
                    forcedLODValue.ValueChanged += () => _previewModel.ForcedLOD = forcedLODValue.Value;
                    ViewWidgetButtonMenu.VisibleChanged += control => forcedLODValue.Value = _previewModel.ForcedLOD;
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
