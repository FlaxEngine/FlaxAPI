// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using System.IO;
using FlaxEditor.GUI.ContextMenu;
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
        private sealed class Proxy : ProxyBase<ScreenFadeTrack, ScreenFadeMedia>
        {
            /// <summary>
            /// Gets or sets the color gradient stops collection.
            /// </summary>
            [EditorDisplay("Gradient", EditorDisplayAttribute.InlineStyle), EditorOrder(10), Tooltip("The color gradient stops list.")]
            public List<GradientEditor.Stop> Gradient
            {
                get => Media.Gradient.Stops;
                set => Media.Gradient.Stops = value;
            }

            /// <inheritdoc />
            public Proxy(ScreenFadeTrack track, ScreenFadeMedia media)
            : base(track, media)
            {
            }
        }

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
                DockStyle = DockStyle.Fill,
                Parent = this,
            };
            Gradient.Edited += OnGradientEdited;
        }

        private void OnGradientEdited()
        {
            Timeline?.MarkAsEdited();
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Track track)
        {
            base.OnTimelineChanged(track);

            PropertiesEditObject = new Proxy(Track as ScreenFadeTrack, this);
        }

        /// <inheritdoc />
        public override void OnTimelineZoomChanged()
        {
            base.OnTimelineZoomChanged();

            Gradient.SetScale(Timeline.Zoom * Timeline.UnitsPerSecond / Timeline.FramesPerSecond);
        }

        /// <inheritdoc />
        public override void OnTimelineFpsChanged(float before, float after)
        {
            base.OnTimelineFpsChanged(before, after);

            Gradient.OnTimelineFpsChanged(before, after);
        }

        /// <inheritdoc />
        public override void OnTimelineShowContextMenu(ContextMenu.ContextMenu menu, Control controlUnderMouse)
        {
            base.OnTimelineShowContextMenu(menu, controlUnderMouse);

            if (controlUnderMouse is GradientEditor.StopControl stop)
            {
                menu.AddButton("Remove gradient stop", OnRemoveGradientStop).Tag = stop.Index;
                menu.AddSeparator();
            }
        }

        private void OnRemoveGradientStop(ContextMenuButton button)
        {
            var stops = new List<GradientEditor.Stop>(Gradient.Stops);
            stops.RemoveAt((int)button.Tag);
            Gradient.Stops = stops;
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
            var stops = new List<GradientEditor.Stop>(stopsCount);
            for (int i = 0; i < stopsCount; i++)
            {
                GradientEditor.Stop stop;
                stop.Frame = stream.ReadInt32();
                stop.Value = stream.ReadColor();
                stops.Add(stop);
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
                    stream.Write(stop.Frame);
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
