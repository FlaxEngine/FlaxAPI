// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.GUI
{
    /// <summary>
    /// Timeline ending edge control that can be used to modify timeline duration with a mouse.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class TimelineEdge : Control
    {
        private Timeline _timeline;
        private bool _isMoving;
        private Vector2 _startMoveLocation;
        private int _startMoveDuration;
        private bool _isStart;
        private bool _canEdit;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineEdge"/> class.
        /// </summary>
        /// <param name="timeline">The parent timeline.</param>
        /// <param name="isStart">True if edge edits the timeline start, otherwise it's for the ending cap.</param>
        /// <param name="canEdit">True if can edit the edge.</param>
        public TimelineEdge(Timeline timeline, bool isStart, bool canEdit)
        {
            AutoFocus = false;
            _timeline = timeline;
            _isStart = isStart;
            _canEdit = canEdit;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;

            var moveColor = style.ProgressNormal;
            var thickness = 2.0f;
            var borderColor = _isMoving ? moveColor : (IsMouseOver && _canEdit ? Color.Yellow : style.BorderNormal);
            Render2D.FillRectangle(new Rectangle((Width - thickness) * 0.5f, 0, thickness, Height), borderColor);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            if (buttons == MouseButton.Left && _canEdit)
            {
                _isMoving = true;
                _startMoveLocation = Root.MousePosition;
                _startMoveDuration = _timeline.DurationFrames;

                StartMouseCapture(true);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            if (_isMoving)
            {
                var moveLocation = Root.MousePosition;
                var moveLocationDelta = moveLocation - _startMoveLocation;
                var moveDelta = (int)(moveLocationDelta.X / (Timeline.UnitsPerSecond * _timeline.Zoom) * _timeline.FramesPerSecond);
                var durationFrames = _timeline.DurationFrames;

                if (_isStart)
                {
                    // TODO: editing timeline start frame?
                }
                else
                {
                    _timeline.DurationFrames = _startMoveDuration + moveDelta;
                }

                if (_timeline.DurationFrames != durationFrames)
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

        private void EndMoving()
        {
            _isMoving = false;

            EndMouseCapture();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _timeline = null;

            base.Dispose();
        }
    }
}
