// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline media that represents a post-process material media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class PostProcessMaterialTrackMedia : SingleMediaAssetTrackMedia
    {
    }

    /// <summary>
    /// The timeline track that represents a post-process material playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class PostProcessMaterialTrack : SingleMediaAssetTrack<MaterialBase, PostProcessMaterialTrackMedia>
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 2,
                Name = "Post Process Material",
                Create = options => new PostProcessMaterialTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (PostProcessMaterialTrack)track;
            Guid id = new Guid(stream.ReadBytes(16));
            e.Asset = FlaxEngine.Content.LoadAsync<MaterialBase>(ref id);
            var m = e.TrackMedia;
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (PostProcessMaterialTrack)track;
            var materialId = e.Asset?.ID ?? Guid.Empty;

            stream.Write(materialId.ToByteArray());

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
        /// Initializes a new instance of the <see cref="PostProcessMaterialTrack"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public PostProcessMaterialTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }
    }
}
