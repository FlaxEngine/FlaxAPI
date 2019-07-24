// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEditor.Content.Thumbnails;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline media that represents a nested scene animation media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class NestedSceneAnimationTrackMedia : SingleMediaAssetTrackMedia
    {
        /// <summary>
        /// True if loop track, otherwise animation will stop on the end.
        /// </summary>
        public bool Loop;
    }

    /// <summary>
    /// The timeline track that represents a nested scene animation playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class NestedSceneAnimationTrack : SingleMediaAssetTrack<SceneAnimation, NestedSceneAnimationTrackMedia>
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 3,
                Name = "Nested Timeline",
                Create = options => new NestedSceneAnimationTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (NestedSceneAnimationTrack)track;
            Guid id = new Guid(stream.ReadBytes(16));
            e.Asset = FlaxEngine.Content.LoadAsync<SceneAnimation>(ref id);
            var m = e.TrackMedia;
            var tmp = stream.ReadInt32();
            m.Loop = (tmp & 1) == 1;
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (NestedSceneAnimationTrack)track;
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
        /// Gets or sets the nested animation looping mode.
        /// </summary>
        public bool Loop
        {
            get => TrackMedia.Loop;
            set
            {
                NestedSceneAnimationTrackMedia media = TrackMedia;
                if (media.Loop == value)
                    return;

                media.Loop = value;
                Timeline?.MarkAsEdited();
            }
        }

        /// <inheritdoc />
        public NestedSceneAnimationTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }
    }
}
