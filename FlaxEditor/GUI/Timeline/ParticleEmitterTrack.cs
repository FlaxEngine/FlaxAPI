// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEditor.Content;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline media that represents a particle miter playback media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class ParticleEmitterMedia : SingleMediaAssetTrackMedia
    {
    }

    /// <summary>
    /// The timeline track that represents a particle emitter playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class ParticleEmitterTrack : SingleMediaAssetTrack<ParticleEmitter, ParticleEmitterMedia>
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 0,
                Name = "Particle Emitter",
                Create = options => new ParticleEmitterTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (ParticleEmitterTrack)track;
            Guid id = new Guid(stream.ReadBytes(16));
            e.Asset = FlaxEngine.Content.LoadAsync<ParticleEmitter>(ref id);
            var emitterIndex = stream.ReadInt32();
            var m = e.TrackMedia;
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (ParticleEmitterTrack)track;
            var emitterId = e.Asset?.ID ?? Guid.Empty;

            stream.Write(emitterId.ToByteArray());
            stream.Write(((ParticleSystemTimeline)track.Timeline).Emitters.IndexOf(e));

            if (e.Media.Count != 0)
            {
                var m = e.TrackMedia;
                stream.Write(m.StartFrame);
                stream.Write(m.DurationFrames);
            }
            else
            {
                stream.Write(0);
                stream.Write(track.Timeline.DurationFrames);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterTrack"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ParticleEmitterTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }

        /// <inheritdoc />
        public override void OnSpawned()
        {
            base.OnSpawned();

            // Ask user to specify the particle emitter asset to playback
            AssetSearchPopup.Show(this, Size * 0.5f, IsValid, (assetItem) => Asset = FlaxEngine.Content.LoadAsync<ParticleEmitter>(assetItem.ID));
        }

        private bool IsValid(AssetItem item)
        {
            return item is BinaryAssetItem binaryItem && typeof(ParticleEmitter).IsAssignableFrom(binaryItem.Type);
        }
    }
}
