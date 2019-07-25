// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.GUI
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
            AutoFocus = false;

            _timeline = timeline;
        }

        /// <inheritdoc />
        public override bool IntersectsContent(ref Vector2 locationParent, out Vector2 location)
        {
            // Pass all events
            location = PointFromParent(ref locationParent);
            return true;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var mediaBackground = _timeline.MediaBackground;
            var tracks = _timeline.Tracks;
            var linesColor = Style.Current.BackgroundNormal;
            var areaLeft = -X;
            var areaRight = Parent.Width + mediaBackground.ControlsBounds.BottomRight.X;
            var height = Height;
            var leftSideMin = PointFromParent(Vector2.Zero);
            var leftSideMax = BottomLeft;
            var rightSideMin = UpperRight;
            var rightSideMax = PointFromParent(Parent.BottomRight) + mediaBackground.ControlsBounds.BottomRight;

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

            // Draw vertical lines for time axis
            var zoom = Timeline.UnitsPerSecond * _timeline.Zoom;
            var leftFrame = Mathf.Floor((leftSideMin.X - Timeline.StartOffset) / zoom) * _timeline.FramesPerSecond;
            var rightFrame = Mathf.Ceil((rightSideMax.X - Timeline.StartOffset) / zoom) * _timeline.FramesPerSecond;
            var verticalLinesHeaderExtend = Timeline.HeaderTopAreaHeight * 0.5f;
            var timeShowMode = _timeline.TimeShowMode;
            for (float frame = leftFrame; frame <= rightFrame; frame += _timeline.FramesPerSecond)
            {
                var time = frame / _timeline.FramesPerSecond;
                var x = time * zoom + Timeline.StartOffset;

                // Vertical line
                Render2D.FillRectangle(new Rectangle(x - 0.5f, 0, 1.0f, height), style.ForegroundDisabled.RGBMultiplied(0.7f));

                // Header line
                Render2D.FillRectangle(new Rectangle(x - 0.5f, -verticalLinesHeaderExtend, 1.0f, verticalLinesHeaderExtend), style.Foreground.RGBMultiplied(0.8f));

                // Time
                string label;
                switch (timeShowMode)
                {
                case Timeline.TimeShowModes.Frames:
                    label = frame.ToString("0000");
                    break;
                case Timeline.TimeShowModes.Seconds:
                    label = time.ToString() + 's';
                    break;
                case Timeline.TimeShowModes.Time:
                    label = TimeSpan.FromSeconds(time).ToString();
                    break;
                default: throw new ArgumentOutOfRangeException();
                }
                var labelRect = new Rectangle(x + 2, -verticalLinesHeaderExtend, 50, verticalLinesHeaderExtend);
                Render2D.DrawText(style.FontSmall, label, labelRect, style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, 0.8f);
            }

            DrawChildren();

            // Disabled overlay
            for (int i = 0; i < tracks.Count; i++)
            {
                var track = tracks[i];
                if (track.DrawDisabled)
                {
                    Render2D.FillRectangle(new Rectangle(areaLeft, track.Top, areaRight, track.Height), new Color(0, 0, 0, 100));
                }
            }

            // Darken area outside the duration
            var outsideDurationAreaColor = new Color(0, 0, 0, 100);
            Render2D.FillRectangle(new Rectangle(leftSideMin, leftSideMax.X - leftSideMin.X, height), outsideDurationAreaColor);
            Render2D.FillRectangle(new Rectangle(rightSideMin, rightSideMax.X - rightSideMin.X, height), outsideDurationAreaColor);
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            if (base.OnMouseWheel(location, delta))
                return true;

            // Zoom in/out
            if (IsMouseOver && Root.GetKey(Keys.Control))
            {
                // TODO: preserve the view center point for easier zooming
                _timeline.Zoom += delta * 0.1f;
                return true;
            }

            return false;
        }
    }
}
