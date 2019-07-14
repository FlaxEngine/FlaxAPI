// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline media that represents a particle miter playback media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class ParticleEmitterMedia : Media
    {
        /// <summary>
        /// The emitter asset id.
        /// </summary>
        public Guid Emitter;
    }

    /// <summary>
    /// The timeline track that represents a particle emitter playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class ParticleEmitterTrack : Track
    {
        private readonly AssetPicker _picker;

        /// <summary>
        /// Gets or sets the emitter asset.
        /// </summary>
        public ParticleEmitter Emitter
        {
            get => Media.Count > 0 ? FlaxEngine.Content.LoadAsync<ParticleEmitter>(((ParticleEmitterMedia)Media[0]).Emitter) : null;
            set
            {
                ParticleEmitterMedia media;
                if (Media.Count == 0)
                {
                    media = new ParticleEmitterMedia
                    {
                        StartFrame = 0,
                        DurationFrames = Timeline?.DurationFrames ?? 60,
                    };
                    AddMedia(media);
                }
                else
                {
                    media = (ParticleEmitterMedia)Media[0];
                }
                var prev = media.Emitter;
                media.Emitter = value?.ID ?? Guid.Empty;
                if (prev != media.Emitter)
                {
                    _picker.SelectedAsset = value;
                    Timeline?.MarkAsEdited();
                }
            }
        }

        /// <summary>
        /// Gets the emitter media object (or null if not created.
        /// </summary>
        public ParticleEmitterMedia EmitterMedia => Media.Count > 0 ? (ParticleEmitterMedia)Media[0] : null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterTrack"/> class.
        /// </summary>
        /// <param name="archetype">The archetype.</param>
        /// <param name="mute">The mute flag.</param>
        public ParticleEmitterTrack(Timeline.TrackArchetype archetype, bool mute)
        {
            Name = archetype.Name;
            Icon = archetype.Icon;
            Mute = mute;

            _picker = new AssetPicker(typeof(ParticleEmitter), Vector2.Zero)
            {
                Size = new Vector2(50.0f, 36.0f),
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            _picker.Location = new Vector2(Width - _picker.Width - 2, 2);
            _picker.SelectedItemChanged += OnPickerSelectedItemChanged;
            Height = 4 + _picker.Height;

            const float buttonSize = 14;
            var muteButton = new CheckBox(_picker.Left - buttonSize - 2.0f, 0, !Mute, buttonSize)
            {
                TooltipText = "Mute track",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Parent = this
            };
            muteButton.StateChanged += OnMuteButtonStateChanged;
        }

        private void OnMuteButtonStateChanged(CheckBox checkBox)
        {
            Mute = !checkBox.Checked;
            Timeline.MarkAsEdited();
        }

        private void OnPickerSelectedItemChanged()
        {
            Emitter = (ParticleEmitter)_picker.SelectedAsset;
        }

        /// <inheritdoc />
        public override void OnSpawned()
        {
            base.OnSpawned();

            // Ask user to specify the particle emitter asset to playback
            AssetSearchPopup.Show(this, Size * 0.5f, IsValid, (assetItem) => Emitter = FlaxEngine.Content.LoadAsync<ParticleEmitter>(assetItem.ID));
        }

        private bool IsValid(AssetItem item)
        {
            return item is BinaryAssetItem binaryItem && typeof(ParticleEmitter).IsAssignableFrom(binaryItem.Type);
        }
    }
}
