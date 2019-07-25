// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.IO;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline media that represents a screen fade animation event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class ScreenFadeMedia : Media
    {
    }

    /// <summary>
    /// The timeline track that represents a screen fade animation.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class ScreenFadeTrack : SingleMediaTrack<ScreenFadeMedia>
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 4,
                Name = "Screen Fade",
                Create = options => new ScreenFadeTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (ScreenFadeTrack)track;
            var m = e.TrackMedia;
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (ScreenFadeTrack)track;

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
        /// Initializes a new instance of the <see cref="ScreenFadeTrack"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ScreenFadeTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }
    }
}
