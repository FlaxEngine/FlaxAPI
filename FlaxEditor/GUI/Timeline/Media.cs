// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// Timeline track media event (range-based). Can be added to the timeline track.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public abstract class Media : ContainerControl
    {
        private Timeline _timeline;
        private Track _tack;
        private int _startFrame, _durationFrames;

        /// <summary>
        /// Gets or sets the start frame of the media event.
        /// </summary>
        public int StartFrame
        {
            get => _startFrame;
            set
            {
                if (_startFrame == value)
                    return;

                _startFrame = value;
                if (_timeline != null)
                {
                    X = Start * Timeline.UnitsPerSecond + Timeline.StartOffset;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total duration of the media event in the timeline sequence frames amount.
        /// </summary>
        public int DurationFrames
        {
            get => _durationFrames;
            set
            {
                if (_durationFrames == value)
                    return;

                _durationFrames = value;
                if (_timeline != null)
                {
                    Width = Duration * Timeline.UnitsPerSecond;
                }
            }
        }

        /// <summary>
        /// Gets the media start time in seconds.
        /// </summary>
        /// <seealso cref="StartFrame"/>
        public float Start => _startFrame / _timeline.FramesPerSecond;

        /// <summary>
        /// Get the media duration in seconds.
        /// </summary>
        /// <seealso cref="DurationFrames"/>
        public float Duration => _durationFrames / _timeline.FramesPerSecond;

        /// <summary>
        /// Gets the parent timeline.
        /// </summary>
        public Timeline Timeline => _timeline;

        /// <summary>
        /// Gets the track.
        /// </summary>
        public Track Track => _tack;

        /// <summary>
        /// Called when parent track gets changed.
        /// </summary>
        /// <param name="track">The track.</param>
        public virtual void OnTimelineChanged(Track track)
        {
            _timeline = track?.Timeline;
            _tack = track;
            Parent = _timeline?.MediaPanel;
            if (_timeline != null)
            {
                X = Start * Timeline.UnitsPerSecond + Timeline.StartOffset;
                Width = Duration * Timeline.UnitsPerSecond;
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), style.BorderNormal);

            DrawChildren();
        }
    }
}
