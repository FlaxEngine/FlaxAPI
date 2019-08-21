// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlaxEditor.GUI.Timeline.Tracks;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline editor for particle system asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Timeline" />
    public sealed class ParticleSystemTimeline : Timeline
    {
        private sealed class Proxy : ProxyBase<ParticleSystemTimeline>
        {
            /// <inheritdoc />
            public Proxy(ParticleSystemTimeline timeline)
            : base(timeline)
            {
            }
        }

        private ParticleSystemPreview _preview;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystemTimeline"/> class.
        /// </summary>
        /// <param name="preview">The particle system preview.</param>
        public ParticleSystemTimeline(ParticleSystemPreview preview)
        : base(PlaybackButtons.Play | PlaybackButtons.Stop)
        {
            PlaybackState = PlaybackStates.Playing;
            PropertiesEditObject = new Proxy(this);

            _preview = preview;

            // Setup track types
            TrackArchetypes.Add(ParticleEmitterTrack.GetArchetype());
            TrackArchetypes.Add(FolderTrack.GetArchetype());
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            CurrentFrame = (int)(_preview.PreviewActor.Time * _preview.System.FramesPerSecond);

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void OnPlay()
        {
            _preview.PlaySimulation = true;

            base.OnPlay();
        }

        /// <inheritdoc />
        public override void OnPause()
        {
            _preview.PlaySimulation = false;
            _preview.PreviewActor.LastUpdateTime = -1.0f;

            base.OnPause();
        }

        /// <inheritdoc />
        public override void OnStop()
        {
            _preview.PlaySimulation = false;
            _preview.PreviewActor.ResetSimulation();

            base.OnStop();
        }

        /// <summary>
        /// Loads the timeline from the specified <see cref="FlaxEngine.ParticleSystem"/> asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Load(ParticleSystem asset)
        {
            var data = asset.LoadTimeline();
            Load(data);
        }

        /// <summary>
        /// Saves the timeline data to the <see cref="FlaxEngine.ParticleSystem"/> asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Save(ParticleSystem asset)
        {
            var data = Save();
            asset.SaveTimeline(data);
        }

        /// <inheritdoc />
        protected override void LoadTimelineData(int version, BinaryReader stream)
        {
            base.LoadTimelineData(version, stream);

            // Load emitters
            int emittersCount = stream.ReadInt32();
        }

        internal List<ParticleEmitterTrack> Emitters;

        /// <inheritdoc />
        protected override void SaveTimelineData(BinaryWriter stream)
        {
            base.SaveTimelineData(stream);

            // Save emitters
            Emitters = Tracks.Where(track => track is ParticleEmitterTrack).Cast<ParticleEmitterTrack>().ToList();
            int emittersCount = Emitters.Count;
            stream.Write(emittersCount);
        }

        /// <inheritdoc />
        public override void Load(byte[] data)
        {
            base.Load(data);

            OnPlay();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            _preview = null;

            base.Dispose();
        }
    }
}
