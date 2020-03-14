// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline media that represents a nested scene animation media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class NestedSceneAnimationMedia : SingleMediaAssetMedia
    {
        private sealed class Proxy : ProxyBase<NestedSceneAnimationTrack, NestedSceneAnimationMedia>
        {
            /// <summary>
            /// Gets or sets the nested scene animation to play.
            /// </summary>
            [EditorDisplay("General"), EditorOrder(10), Tooltip("The nested scene animation to play.")]
            public SceneAnimation NestedSceneAnimation
            {
                get => Track.Asset;
                set => Track.Asset = value;
            }

            /// <summary>
            /// Gets or sets the nested animation looping mode.
            /// </summary>
            [EditorDisplay("General"), EditorOrder(20), Tooltip("If checked, the nested animation will loop when playback exceeds its duration. Otherwise it will stop play.")]
            public bool Loop
            {
                get => Track.Loop;
                set => Track.Loop = value;
            }

            /// <inheritdoc />
            public Proxy(NestedSceneAnimationTrack track, NestedSceneAnimationMedia media)
            : base(track, media)
            {
            }
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Track track)
        {
            base.OnTimelineChanged(track);

            PropertiesEditObject = new Proxy(Track as NestedSceneAnimationTrack, this);
        }
    }

    /// <summary>
    /// The timeline track that represents a nested scene animation playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class NestedSceneAnimationTrack : SingleMediaAssetTrack<SceneAnimation, NestedSceneAnimationMedia>
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
        /// Gets or sets the nested animation looping mode.
        /// </summary>
        public bool TrackLoop
        {
            get => Loop;
            set
            {
                NestedSceneAnimationMedia media = TrackMedia;
                if (Loop == value)
                    return;

                Loop = value;
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
