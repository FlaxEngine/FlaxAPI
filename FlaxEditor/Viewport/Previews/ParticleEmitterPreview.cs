// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Particle Emitter asset preview editor viewport.
    /// </summary>
    /// <seealso cref="AssetPreview" />
    public class ParticleEmitterPreview : ParticleSystemPreview
    {
        private ParticleEmitter _emitter;
        private ParticleSystem _system;

        /// <summary>
        /// Gets or sets the particle emitter asset to preview.
        /// </summary>
        public ParticleEmitter Emitter
        {
            get => _emitter;
            set
            {
                if (_emitter != value)
                {
                    _emitter = value;
                    _system.Init(value, 5.0f);
                    PreviewActor.ResetSimulation();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public ParticleEmitterPreview(bool useWidgets)
        : base(useWidgets)
        {
            _system = FlaxEngine.Content.CreateVirtualAsset<ParticleSystem>();
            System = _system;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Cleanup objects
            _emitter = null;
            Object.Destroy(ref _system);

            base.OnDestroy();
        }
    }
}
