// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Particle System asset preview editor viewport.
    /// </summary>
    /// <seealso cref="AssetPreview" />
    public class ParticleSystemPreview : AssetPreview
    {
        private ParticleEffect _previewEffect;
        private ContextMenuButton _showBoundsButton;
        private StaticModel _boundsModel;

        /// <summary>
        /// Gets or sets the particle system asset to preview.
        /// </summary>
        public ParticleSystem System
        {
            get => _previewEffect.ParticleSystem;
            set => _previewEffect.ParticleSystem = value;
        }

        /// <summary>
        /// Gets the particle effect actor used to preview selected asset.
        /// </summary>
        public ParticleEffect PreviewActor => _previewEffect;

        /// <summary>
        /// Gets or sets a value indicating whether to play the particles simulation in editor.
        /// </summary>
        public bool PlaySimulation { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to show particle effect bounding box.
        /// </summary>
        public bool ShowBounds
        {
            get => _boundsModel?.IsActive ?? false;
            set
            {
                if (value == ShowBounds)
                    return;

                if (value)
                {
                    if (!_boundsModel)
                    {
                        _boundsModel = StaticModel.New();
                        _boundsModel.Model = FlaxEngine.Content.LoadAsyncInternal<Model>("Editor/Gizmo/WireBox");
                        _boundsModel.Model.WaitForLoaded();
                        _boundsModel.Entries[0].Material = FlaxEngine.Content.LoadAsyncInternal<MaterialBase>("Editor/Gizmo/MaterialWireFocus");
                        Task.CustomActors.Add(_boundsModel);
                    }
                    else if (!_boundsModel.IsActive)
                    {
                        _boundsModel.IsActive = true;
                        Task.CustomActors.Add(_boundsModel);
                    }

                    UpdateBounds();
                }
                else
                {
                    _boundsModel.IsActive = false;
                    Task.CustomActors.Remove(_boundsModel);
                }

                _showBoundsButton.Checked = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystemPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public ParticleSystemPreview(bool useWidgets)
        : base(useWidgets)
        {
            // Setup preview scene
            _previewEffect = ParticleEffect.New();
            _previewEffect.IsLooping = true;

            // Link actors for rendering
            Task.CustomActors.Add(_previewEffect);

            if (useWidgets)
            {
                _showBoundsButton = ViewWidgetShowMenu.AddButton("Show Bounds", () => ShowBounds = !ShowBounds);
            }
        }

        private void UpdateBounds()
        {
            var bounds = _previewEffect.Box;
            Transform t = Transform.Identity;
            t.Translation = bounds.Center;
            t.Scale = bounds.Size;
            _boundsModel.Transform = t;
        }

        /// <summary>
        /// Fits the particle system into view (scales the emitter based on the current bounds of the system).
        /// </summary>
        /// <param name="targetSize">The target size of the effect.</param>
        public void FitIntoView(float targetSize = 30.0f)
        {
            float maxSize = Mathf.Max(0.001f, _previewEffect.Box.Size.MaxValue);
            _previewEffect.Scale = new Vector3(targetSize / maxSize);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Manually update simulation
            if (PlaySimulation)
            {
                _previewEffect.UpdateSimulation();
            }

            // Keep bounds matching the model
            if (_boundsModel && _boundsModel.IsActive)
            {
                UpdateBounds();
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Cleanup objects
            _previewEffect.ParticleSystem = null;
            Object.Destroy(ref _previewEffect);

            base.OnDestroy();
        }
    }
}
