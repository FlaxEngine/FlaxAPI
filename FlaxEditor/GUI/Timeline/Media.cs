// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
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
        private Vector2 _mouseLocation = Vector2.Minimum;
        private bool _isMoving;
        private Vector2 _startMoveLocation;
        private int _startMoveStartFrame;
        private int _startMoveDuration;
        private bool _startMoveLeftEdge;
        private bool _startMoveRightEdge;

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
                value = Math.Max(value, 1);
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

        private Rectangle MoveLeftEdgeRect => new Rectangle(-5, -5, 10, Height + 10);

        private Rectangle MoveRightEdgeRect => new Rectangle(Width - 5, -5, 10, Height + 10);

        /// <summary>
        /// Initializes a new instance of the <see cref="Media"/> class.
        /// </summary>
        public Media()
        {
            CanFocus = false;
        }

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

        /// <summary>
        /// Called when timeline FPS gets changed.
        /// </summary>
        /// <param name="before">The before value.</param>
        /// <param name="after">The after value.</param>
        public virtual void OnTimelineFpsChanged(float before, float after)
        {
            StartFrame = (int)((_startFrame / before) * after);
            DurationFrames = (int)((_durationFrames / before) * after);
        }

        /// <summary>
        /// Called when media gets removed by the user.
        /// </summary>
        public virtual void OnDeleted()
        {
            Dispose();
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var bounds = new Rectangle(Vector2.Zero, Size);

            var fillColor = style.Background * 1.5f;
            Render2D.FillRectangle(bounds, fillColor);

            var isMovingWholeMedia = _isMoving && !_startMoveRightEdge && !_startMoveLeftEdge;
            var borderHighlightColor = style.BorderHighlighted;
            var moveColor = style.ProgressNormal;
            var moveThickness = 2.0f;
            var borderColor = isMovingWholeMedia ? moveColor : (IsMouseOver ? borderHighlightColor : style.BorderNormal);
            Render2D.DrawRectangle(bounds, borderColor, isMovingWholeMedia ? moveThickness : 1.0f);
            if (_startMoveLeftEdge)
            {
                Render2D.DrawLine(bounds.UpperLeft, bounds.BottomLeft, moveColor, moveThickness);
            }
            else if (IsMouseOver && MoveLeftEdgeRect.Contains(ref _mouseLocation))
            {
                Render2D.DrawLine(bounds.UpperLeft, bounds.BottomLeft, Color.Yellow);
            }
            if (_startMoveRightEdge)
            {
                Render2D.DrawLine(bounds.UpperRight, bounds.BottomRight, moveColor, moveThickness);
            }
            else if (IsMouseOver && MoveRightEdgeRect.Contains(ref _mouseLocation))
            {
                Render2D.DrawLine(bounds.UpperRight, bounds.BottomRight, Color.Yellow);
            }

            DrawChildren();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            if (buttons == MouseButton.Left)
            {
                _isMoving = true;
                _startMoveLocation = Root.MousePosition;
                _startMoveStartFrame = StartFrame;
                _startMoveDuration = DurationFrames;
                _startMoveLeftEdge = MoveLeftEdgeRect.Contains(ref location);
                _startMoveRightEdge = MoveRightEdgeRect.Contains(ref location);

                StartMouseCapture(true);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _mouseLocation = location;

            if (_isMoving)
            {
                var moveLocation = Root.MousePosition;
                var moveLocationDelta = moveLocation - _startMoveLocation;
                var moveDelta = (int)(moveLocationDelta.X / Timeline.UnitsPerSecond * _timeline.FramesPerSecond);
                var startFrame = StartFrame;
                var durationFrames = DurationFrames;

                if (_startMoveLeftEdge)
                {
                    StartFrame = _startMoveStartFrame + moveDelta;
                    DurationFrames = _startMoveDuration - moveDelta;
                }
                else if (_startMoveRightEdge)
                {
                    DurationFrames = _startMoveDuration + moveDelta;
                }
                else
                {
                    StartFrame = _startMoveStartFrame + moveDelta;
                }

                if (StartFrame != startFrame || DurationFrames != durationFrames)
                {
                    _timeline.MarkAsEdited();
                }
            }
            else
            {
                base.OnMouseMove(location);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left && _isMoving)
            {
                EndMoving();
                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnEndMouseCapture()
        {
            if (_isMoving)
            {
                EndMoving();
            }

            base.OnEndMouseCapture();
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            if (_isMoving)
            {
                EndMoving();
            }

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            base.OnMouseEnter(location);

            _mouseLocation = location;
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            _mouseLocation = Vector2.Minimum;
        }

        private void EndMoving()
        {
            _isMoving = false;
            _startMoveLeftEdge = false;
            _startMoveRightEdge = false;

            EndMouseCapture();
        }
    }
}
