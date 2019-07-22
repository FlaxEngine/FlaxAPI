// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline editor for scene animation asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Timeline" />
    public sealed class SceneAnimationTimeline : Timeline
    {
        private SceneAnimationPlayer _player;

        /// <summary>
        /// Gets or sets the animation player actor used for the timeline preview.
        /// </summary>
        public SceneAnimationPlayer Player
        {
            get => _player;
            set
            {
                if (_player == value)
                    return;

                _player = value;

                UpdatePlaybackState();
                PlayerChanged?.Invoke();
            }
        }

        /// <summary>
        /// Occurs when the selected player gets changed.
        /// </summary>
        public event Action PlayerChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneAnimationTimeline"/> class.
        /// </summary>
        public SceneAnimationTimeline()
        : base(PlaybackButtons.Play | PlaybackButtons.Stop)
        {
            PlaybackState = PlaybackStates.Disabled;

            // Setup track types
            var icons = Editor.Instance.Icons;
            TrackArchetypes.Add(new TrackArchetype
            {
                Name = "Folder",
                Icon = icons.Folder64,
                Create = archetype => new FolderTrack(archetype, false),
            });
        }

        private void UpdatePlaybackState()
        {
            PlaybackStates state;
            if (!_player)
                state = PlaybackStates.Disabled;
            else if (_player.IsPlaying)
                state = PlaybackStates.Playing;
            else if (_player.IsPaused)
                state = PlaybackStates.Paused;
            else if (_player.IsStopped)
                state = PlaybackStates.Stopped;
            else
                state = PlaybackStates.Disabled;
            PlaybackState = state;
            CurrentFrame = _player ? (int)(_player.Time * _player.Animation.DurationFrames) : 0;
        }

        /// <inheritdoc />
        public override void OnPlay()
        {
            _player.Play();

            base.OnPlay();
        }

        /// <inheritdoc />
        public override void OnPause()
        {
            _player.Pause();

            base.OnPause();
        }

        /// <inheritdoc />
        public override void OnStop()
        {
            _player.Stop();

            base.OnStop();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdatePlaybackState();
        }

        /// <summary>
        /// Loads the timeline from the specified <see cref="FlaxEngine.SceneAnimation"/> asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Load(SceneAnimation asset)
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
                    throw new Exception("Unknown scene animation timeline version " + version);
                FramesPerSecond = stream.ReadSingle();
                DurationFrames = stream.ReadInt32();

                // Load tracks
                int tracksCount = stream.ReadInt32();
                _tracks.Capacity = Math.Max(_tracks.Capacity, tracksCount);
                for (int i = 0; i < tracksCount; i++)
                {
                    var type = stream.ReadByte();
                    var flag = stream.ReadByte();
                    Track track;
                    var mute = (flag & 1) == 1;
                    switch (type)
                    {
                    // Folder
                    case 0:
                    {
                        track = new FolderTrack(TrackArchetypes[0], mute);
                        break;
                    }
                    default: throw new Exception("Unknown Scene Animation track type " + type);
                    }
                    int parentIndex = stream.ReadInt32();
                    int childrenCount = stream.ReadInt32();
                    track.Name = Utilities.Utils.ReadStr(stream, -13);
                    track.Tag = parentIndex;
                    track.Color = new Color(stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());

                    switch (type)
                    {
                    // Folder
                    case 0:
                    {
                        var e = (FolderTrack)track;
                        track.IconColor = e.Color;
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
                stream.Write(1);
                stream.Write(FramesPerSecond);
                stream.Write(DurationFrames);

                // Save tracks
                int tracksCount = Tracks.Count;
                stream.Write(tracksCount);
                for (int i = 0; i < tracksCount; i++)
                {
                    var track = Tracks[i];

                    byte type;
                    if (track is FolderTrack)
                        type = 0;
                    else
                        throw new NotSupportedException("Unknown Scene Animation track type.");
                    stream.Write(type);
                    byte flag = 0;
                    if (track.Mute)
                        flag |= 1;
                    stream.Write(flag);
                    stream.Write(_tracks.IndexOf(track.ParentTrack));
                    stream.Write(track.SubTracks.Count);
                    Utilities.Utils.WriteStr(stream, track.Name, -13);
                    {
                        var color = (Color32)track.Color;
                        stream.Write(color.R);
                        stream.Write(color.G);
                        stream.Write(color.B);
                        stream.Write(color.A);
                    }

                    switch (type)
                    {
                    // Folder
                    case 0:
                    {
                        var e = (FolderTrack)track;
                        break;
                    }
                    }
                }

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Saves the timeline data to the <see cref="FlaxEngine.SceneAnimation"/> asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Save(SceneAnimation asset)
        {
            var data = Save();
            asset.SaveTimeline(data);
        }
    }
}
