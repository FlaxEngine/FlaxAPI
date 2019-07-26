// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.IO;
using FlaxEditor.GUI.Timeline.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline media that represents a screen fade animation event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class ScreenFadeMedia : Media
    {
        /// <summary>
        /// The gradient.
        /// </summary>
        public GradientEditor Gradient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenFadeMedia"/> class.
        /// </summary>
        public ScreenFadeMedia()
        {
            Gradient = new GradientEditor
            {
                BackgroundColor = Color.Red,
                DockStyle = DockStyle.Fill,
                Parent = this,
            };
        }
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
            var stopsCount = stream.ReadInt32();
            var stops = new GradientEditor.Stop[stopsCount];
            for (int i = 0; i < stopsCount; i++)
            {
                var stop = stops[i];
                stop.Time = stream.ReadSingle();
                stop.Value = stream.ReadColor();
                stops[i] = stop;
            }
            m.Gradient.Stops = stops;
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (ScreenFadeTrack)track;

            if (e.Media.Count != 0)
            {
                var m = e.TrackMedia;
                stream.Write(m.StartFrame);
                stream.Write(m.DurationFrames);
                var stops = m.Gradient.Stops;
                stream.Write(stops.Count);
                for (int i = 0; i < stops.Count; i++)
                {
                    var stop = stops[i];
                    stream.Write(stop.Time);
                    stream.Write(stop.Value);
                }
            }
            else
            {
                stream.Write(0);
                stream.Write(track.Timeline.DurationFrames);
                stream.Write(0);
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
