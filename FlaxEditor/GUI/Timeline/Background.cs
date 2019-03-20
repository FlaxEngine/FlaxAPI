// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline background control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Background : ContainerControl
    {
        private readonly Timeline _timeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="Background"/> class.
        /// </summary>
        /// <param name="timeline">The timeline.</param>
        public Background(Timeline timeline)
        {
            _timeline = timeline;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var tracks = _timeline.Tracks;
            var linesColor = Style.Current.BackgroundNormal;
            var areaLeft = -X;
            var areaRight = Parent.Width;

            // Draw lines between tracks
            Render2D.DrawLine(new Vector2(areaLeft, 0.5f), new Vector2(areaRight, 0.5f), linesColor);
            for (int i = 0; i < tracks.Count; i++)
            {
                var track = tracks[i];

                if (track.Visible)
                {
                    var top = track.Bottom + 0.5f;
                    Render2D.DrawLine(new Vector2(areaLeft, top), new Vector2(areaRight, top), linesColor);
                }
            }

            // Highlight selected tracks
            for (int i = 0; i < tracks.Count; i++)
            {
                var track = tracks[i];

                if (track.Visible && _timeline.SelectedTracks.Contains(track) && _timeline.ContainsFocus)
                {
                    Render2D.FillRectangle(new Rectangle(areaLeft, track.Top, areaRight, track.Height), style.BackgroundSelected);
                }
            }

            // TODO: vertical lines

            // TODO: time codes

            DrawChildren();

            // TODO: darken area outside the duration

            // 
        }
    }
}
