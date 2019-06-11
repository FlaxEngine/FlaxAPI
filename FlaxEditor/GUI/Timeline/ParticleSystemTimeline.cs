// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
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
            _preview = preview;

            // Setup track types
            var icons = Editor.Instance.Icons;
            TrackArchetypes.Add(new TrackArchetype
            {
                Name = "Folder",
                Icon = icons.Folder64,
                Create = (archetype) => new FolderTrack(archetype),
            });
            TrackArchetypes.Add(new TrackArchetype
            {
                Name = "Emitter",
                Create = (archetype) => new ParticleEmitterTrack(archetype),
            });
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
                int engineBuild = stream.ReadInt32();
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
                    Track track;
                    switch (type)
                    {
                    // Emitter
                    case 0:
                    {
                        track = new ParticleEmitterTrack(TrackArchetypes[1]);
                        break;
                    }
                    // Folder
                    case 1:
                    {
                        track = new FolderTrack(TrackArchetypes[0]);
                        break;
                    }
                    default: throw new Exception("Unknown Particle System track type " + type);
                    }
                    int parentIndex = stream.ReadInt32();
                    int childrenCount = stream.ReadInt32();
                    track.Name = Utilities.Utils.ReadStr(stream, -13);
                    track.Tag = parentIndex;

                    switch (type)
                    {
                    // Emitter
                    case 0:
                    {
                        var e = (ParticleEmitterTrack)track;
                        Guid id = new Guid(stream.ReadBytes(16));
                        e.Emitter = FlaxEngine.Content.LoadAsync<ParticleEmitter>(ref id);
                        var emitterIndex = stream.ReadInt32();
                        var m = e.Media[0];
                        m.StartFrame = stream.ReadInt32();
                        m.DurationFrames = stream.ReadInt32();
                        break;
                    }
                    // Folder
                    case 1:
                    {
                        var e = (FolderTrack)track;
                        e.IconColor = new Color(stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
                        break;
                    }
                    }

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
                stream.Write(Globals.BuildNumber);
                stream.Write(FramesPerSecond);
                stream.Write(DurationFrames);

                // Save emitters
                var emitters = Tracks.Where(track => track is ParticleEmitterTrack).Cast<ParticleEmitterTrack>().ToList();
                int emittersCount = emitters.Count;
                stream.Write(emittersCount);

                // Save tracks
                int tracksCount = Tracks.Count;
                stream.Write(tracksCount);
                for (int i = 0; i < tracksCount; i++)
                {
                    var track = Tracks[i];

                    byte type;
                    if (track is ParticleEmitterTrack)
                        type = 0;
                    else if (track is FolderTrack)
                        type = 1;
                    else
                        throw new NotSupportedException("Unknown Particle System track type.");
                    stream.Write(type);
                    stream.Write(_tracks.IndexOf(track.ParentTrack));
                    stream.Write(track.SubTracks.Count);
                    Utilities.Utils.WriteStr(stream, track.Name, -13);

                    switch (type)
                    {
                    // Emitter
                    case 0:
                    {
                        var e = (ParticleEmitterTrack)track;
                        var emitter = e.Emitter;
                        var emitterId = emitter?.ID ?? Guid.Empty;

                        stream.Write(emitterId.ToByteArray());
                        stream.Write(emitters.IndexOf(e));

                        if (e.Media.Count != 0)
                        {
                            var m = e.Media[0];
                            stream.Write(m.StartFrame);
                            stream.Write(m.DurationFrames);
                        }
                        else
                        {
                            stream.Write(0);
                            stream.Write(DurationFrames);
                        }

                        break;
                    }
                    // Folder
                    case 1:
                    {
                        var e = (FolderTrack)track;
                        var color = (Color32)e.IconColor;
                        stream.Write(color.R);
                        stream.Write(color.G);
                        stream.Write(color.B);
                        stream.Write(color.A);
                        break;
                    }
                    }
                }

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
    }
}
