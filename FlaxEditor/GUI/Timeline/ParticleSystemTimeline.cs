// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private ParticleSystemPreview _preview;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystemTimeline"/> class.
        /// </summary>
        /// <param name="preview">The particle system preview.</param>
        public ParticleSystemTimeline(ParticleSystemPreview preview)
        : base(PlaybackButtons.Play | PlaybackButtons.Stop)
        {
            PlaybackState = PlaybackStates.Playing;

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
        /// Loads the timeline from the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Load(byte[] data)
        {
            Clear();

            using (var memory = new MemoryStream(data))
            using (var stream = new BinaryReader(memory))
            {
                // Load properties
                int version = stream.ReadInt32();
                if (version != 1)
                    throw new Exception("Unknown timeline version " + version);
                FramesPerSecond = stream.ReadSingle();
                DurationFrames = stream.ReadInt32();

                // Load emitters
                int emittersCount = stream.ReadInt32();

                // Load tracks
                int tracksCount = stream.ReadInt32();
                _tracks.Capacity = Math.Max(_tracks.Capacity, tracksCount);
                for (int i = 0; i < tracksCount; i++)
                {
                    var type = stream.ReadByte();
                    var flag = stream.ReadByte();
                    Track track = null;
                    var mute = (flag & 1) == 1;
                    for (int j = 0; j < TrackArchetypes.Count; j++)
                    {
                        if (TrackArchetypes[j].TypeId == type)
                        {
                            var options = new TrackCreateOptions
                            {
                                Archetype = TrackArchetypes[j],
                                Mute = mute,
                            };
                            track = TrackArchetypes[j].Create(options);
                            break;
                        }
                    }
                    if (track == null)
                        throw new Exception("Unknown timeline track type " + type);
                    int parentIndex = stream.ReadInt32();
                    int childrenCount = stream.ReadInt32();
                    track.Name = Utilities.Utils.ReadStr(stream, -13);
                    track.Tag = parentIndex;
                    track.Archetype.Load(version, track, stream);

                    AddLoadedTrack(track);
                }
                for (int i = 0; i < tracksCount; i++)
                {
                    var parentIndex = (int)_tracks[i].Tag;
                    _tracks[i].Tag = null;
                    if (parentIndex != -1)
                        _tracks[i].ParentTrack = _tracks[parentIndex];
                }
                for (int i = 0; i < tracksCount; i++)
                {
                    _tracks[i].OnLoaded();
                }
            }

            ArrangeTracks();
            ClearEditedFlag();
            OnPlay();
        }

        internal List<ParticleEmitterTrack> Emitters;

        /// <summary>
        /// Saves the timeline data.
        /// </summary>
        /// <returns>The saved timeline data.</returns>
        public byte[] Save()
        {
            // Serialize timeline to stream
            using (var memory = new MemoryStream(512))
            using (var stream = new BinaryWriter(memory))
            {
                // Save properties
                stream.Write(1);
                stream.Write(FramesPerSecond);
                stream.Write(DurationFrames);

                // Save emitters
                Emitters = Tracks.Where(track => track is ParticleEmitterTrack).Cast<ParticleEmitterTrack>().ToList();
                int emittersCount = Emitters.Count;
                stream.Write(emittersCount);

                // Save tracks
                int tracksCount = Tracks.Count;
                stream.Write(tracksCount);
                for (int i = 0; i < tracksCount; i++)
                {
                    var track = Tracks[i];

                    stream.Write((byte)track.Archetype.TypeId);
                    byte flag = 0;
                    if (track.Mute)
                        flag |= 1;
                    stream.Write(flag);
                    stream.Write(_tracks.IndexOf(track.ParentTrack));
                    stream.Write(track.SubTracks.Count);
                    Utilities.Utils.WriteStr(stream, track.Name, -13);
                    track.Archetype.Save(track, stream);
                }
                Emitters = null;

                return memory.ToArray();
            }
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
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            _preview = null;

            base.Dispose();
        }
    }
}
