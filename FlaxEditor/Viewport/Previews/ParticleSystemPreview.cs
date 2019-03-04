// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.Rendering;
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
        /// Initializes a new instance of the <see cref="ParticleSystemPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public ParticleSystemPreview(bool useWidgets)
        : base(useWidgets)
        {
            Task.Begin += OnBegin;

            // Setup preview scene
            _previewEffect = ParticleEffect.New();

            // Link actors for rendering
            Task.CustomActors.Add(_previewEffect);
        }

        private void OnBegin(SceneRenderTask task, GPUContext context)
        {
            // Update preview actor scale to fit the preview
            var particleSystem = System;
            if (particleSystem && particleSystem.IsLoaded)
            {
                float targetSize = 30.0f;
                float maxSize = Mathf.Max(0.001f, _previewEffect.Box.Size.MaxValue);
                _previewEffect.Scale = new Vector3(targetSize / maxSize);
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
