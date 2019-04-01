// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
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
        /// Gets or sets a value indicating whether play the particles simulation in editor.
        /// </summary>
        public bool PlaySimulation { get; set; } = false;

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
