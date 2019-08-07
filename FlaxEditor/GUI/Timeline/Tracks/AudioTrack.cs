// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline media that represents an audio clip media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class AudioMedia : SingleMediaAssetMedia
    {
        private bool _loop;

        /// <summary>
        /// True if loop track, otherwise audio clip will stop on the end.
        /// </summary>
        public bool Loop
        {
            get => _loop;
            set
            {
                if (_loop != value)
                {
                    _loop = value;
                    Preview.DrawMode = value ? AudioClipPreview.DrawModes.Looped : AudioClipPreview.DrawModes.Single;
                }
            }
        }

        private sealed class Proxy : ProxyBase<AudioTrack, AudioMedia>
        {
            /// <summary>
            /// Gets or sets the audio clip to play.
            /// </summary>
            [EditorDisplay("General"), EditorOrder(10), Tooltip("The audio clip to play.")]
            public AudioClip Audio
            {
                get => Track.Asset;
                set => Track.Asset = value;
            }

            /// <summary>
            /// Gets or sets the audio clip looping mode.
            /// </summary>
            [EditorDisplay("General"), EditorOrder(20), Tooltip("If checked, the audio clip will loop when playback exceeds its duration. Otherwise it will stop play.")]
            public bool Loop
            {
                get => Track.Loop;
                set => Track.Loop = value;
            }

            /// <inheritdoc />
            public Proxy(AudioTrack track, AudioMedia media)
            : base(track, media)
            {
            }
        }

        /// <summary>
        /// The audio clip preview.
        /// </summary>
        public AudioClipPreview Preview;

        /// <inheritdoc />
        public AudioMedia()
        {
            Preview = new AudioClipPreview
            {
                DockStyle = FlaxEngine.GUI.DockStyle.Fill,
                DrawMode = AudioClipPreview.DrawModes.Single,
                Parent = this,
            };
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Track track)
        {
            base.OnTimelineChanged(track);

            PropertiesEditObject = new Proxy(Track as AudioTrack, this);
        }

        /// <inheritdoc />
        public override void OnTimelineZoomChanged()
        {
            base.OnTimelineZoomChanged();

            Preview.ViewScale = Timeline.UnitsPerSecond / AudioClipPreview.UnitsPerSecond * Timeline.Zoom;
        }
    }

    /// <summary>
    /// The timeline track that represents an audio clip playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class AudioTrack : SingleMediaAssetTrack<AudioClip, AudioMedia>
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 5,
                Name = "Audio",
                Create = options => new AudioTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (AudioTrack)track;
            Guid id = new Guid(stream.ReadBytes(16));
            e.Asset = FlaxEngine.Content.LoadAsync<AudioClip>(ref id);
            var m = e.TrackMedia;
            var tmp = stream.ReadInt32();
            m.Loop = (tmp & 1) == 1;
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (AudioTrack)track;
            var assetId = e.Asset?.ID ?? Guid.Empty;

            stream.Write(assetId.ToByteArray());

            if (e.Media.Count != 0)
            {
                var m = e.TrackMedia;
                var tmp = 0;
                if (e.Loop)
                    tmp |= 1;
                stream.Write(tmp);
                stream.Write(m.StartFrame);
                stream.Write(m.DurationFrames);
            }
            else
            {
                stream.Write(0);
                stream.Write(0);
                stream.Write(track.Timeline.DurationFrames);
            }
        }

        /// <summary>
        /// Gets or sets the audio clip looping mode.
        /// </summary>
        public bool Loop
        {
            get => TrackMedia.Loop;
            set
            {
                AudioMedia media = TrackMedia;
                if (media.Loop == value)
                    return;

                media.Loop = value;
                Timeline?.MarkAsEdited();
            }
        }

        /// <inheritdoc />
        public AudioTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }

        /// <inheritdoc />
        protected override void OnAssetChanged()
        {
            base.OnAssetChanged();

            TrackMedia.Preview.Asset = Asset;
        }
    }
}
