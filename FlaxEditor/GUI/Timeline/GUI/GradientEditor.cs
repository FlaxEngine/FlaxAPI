// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.GUI
{
    /// <summary>
    /// The color gradient editing control for a timeline media event. Allows to edit the gradients stops to create the linear color animation over time.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class GradientEditor : ContainerControl
    {
        /// <summary>
        /// The gradient stop.
        /// </summary>
        public struct Stop
        {
            /// <summary>
            /// The gradient stop frame position (on time axis, relative to the event start).
            /// </summary>
            [EditorOrder(0), Tooltip("The gradient stop frame position (on time axis, relative to the event start).")]
            public int Frame;

            /// <summary>
            /// The color gradient value.
            /// </summary>
            [CustomEditor(typeof(CustomEditors.Editors.GenericEditor))] // Don't use default editor with color picker (focus change issue)
            [EditorOrder(1), Tooltip("The color gradient value.")]
            public Color Value;
        }

        private class StopControl : Control
        {
            private bool _isMoving;
            private Vector2 _startMovePos;

            public GradientEditor Gradient;
            public int Index;

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var isSelected = Gradient._selected == this;
                var arrowRect = new Rectangle(0, 0, 16.0f, 16.0f);
                var arrowTransform = Matrix3x3.Translation2D(new Vector2(-16.0f, -8.0f)) * Matrix3x3.RotationZ(-Mathf.PiOverTwo) * Matrix3x3.Translation2D(new Vector2(8.0f, 0));
                var color = Gradient._data[Index].Value;
                if (IsMouseOver)
                    color *= 1.3f;
                color.A = 1.0f;
                var icons = Editor.Instance.Icons;
                var icon = isSelected ? icons.VisjectArrowClose : icons.VisjectArrowOpen;

                Render2D.PushTransform(ref arrowTransform);
                Render2D.DrawSprite(icon, arrowRect, color);
                Render2D.PopTransform();
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left)
                {
                    Gradient.Select(this);
                    _isMoving = true;
                    _startMovePos = location;
                    StartMouseCapture();
                    return true;
                }

                return base.OnMouseDown(location, buttons);
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left && _isMoving)
                {
                    _isMoving = false;
                    EndMouseCapture();
                }

                return base.OnMouseUp(location, buttons);
            }

            /// <inheritdoc />
            public override bool OnShowTooltip(out string text, out Vector2 location, out Rectangle area)
            {
                // Don't show tooltip is user is moving the stop
                return base.OnShowTooltip(out text, out location, out area) && !_isMoving;
            }

            /// <inheritdoc />
            public override bool OnTestTooltipOverControl(ref Vector2 location)
            {
                // Don't show tooltip is user is moving the stop
                return base.OnTestTooltipOverControl(ref location) && !_isMoving;
            }

            /// <inheritdoc />
            public override void OnMouseMove(Vector2 location)
            {
                if (_isMoving && Vector2.DistanceSquared(ref location, ref _startMovePos) > 25.0f)
                {
                    _startMovePos = Vector2.Minimum;
                    var index = Gradient._stops.IndexOf(this);
                    var time = (PointToParent(location).X - Gradient.BottomLeft.X) / Gradient.Width;
                    // TODO: finish moving stops
                    //Gradient.SetStopTime(index, time);
                }

                base.OnMouseMove(location);
            }

            /// <inheritdoc />
            public override void OnEndMouseCapture()
            {
                _isMoving = false;

                base.OnEndMouseCapture();
            }
        }

        private List<Stop> _data = new List<Stop>();
        private List<StopControl> _stops = new List<StopControl>();
        private StopControl _selected;

        /// <summary>
        /// Gets or sets the list of gradient stops.
        /// </summary>
        public List<Stop> Stops
        {
            get => _data;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.SequenceEqual(_data))
                    return;

                _data.Clear();
                _data.AddRange(value);
                _data.Sort((a, b) => a.Frame > b.Frame ? 1 : 0);
                UpdateControls();
            }
        }

        /// <summary>
        /// Occurs when stops collection gets changed (added/removed).
        /// </summary>
        public event Action StopsChanged;

        /// <summary>
        /// Occurs when stops collection gets modified (stop value or time modified).
        /// </summary>
        public event Action Edited;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientEditor"/> class.
        /// </summary>
        public GradientEditor()
        {
            AutoFocus = false;
        }

        /// <summary>
        /// Called when stops collection gets changed (added/removed).
        /// </summary>
        protected virtual void OnStopsChanged()
        {
            StopsChanged?.Invoke();
        }

        /// <summary>
        /// Called when stops collection gets modified (stop value or time modified).
        /// </summary>
        protected virtual void OnEdited()
        {
            Edited?.Invoke();
        }

        private void Select(StopControl stop)
        {
            _selected = stop;
            UpdateControls();
        }
        /*
        private void SetStopFrame(int index, int frame)
        {
            time = Mathf.Saturate(time);
            if (index != 0)
            {
                time = Mathf.Max(time, _data[index - 1].Time);
            }
            if (index != _stops.Count - 1)
            {
                time = Mathf.Min(time, _data[index + 1].Time);
            }

            var stop = _data[index];
            stop.Time = time;
            _data[index] = stop;
        }

        private void SetStopColor(int index, Color color)
        {
            var stop = _data[index];
            stop.Value = color;
            _data[index] = stop;
        }
        */
        private void UpdateControls()
        {
            var count = _data.Count;

            // Remove unused stops
            while (_stops.Count > count)
            {
                var last = _stops.Count - 1;
                if (_selected == _stops[last])
                    _selected = null;
                _stops[last].Dispose();
                _stops.RemoveAt(last);
            }

            // Add missing stops
            while (_stops.Count < count)
            {
                var stop = new StopControl
                {
                    AutoFocus = false,
                    Gradient = this,
                    Size = new Vector2(16.0f, 16.0f),
                    Parent = this,
                };
                _stops.Add(stop);
            }

            // Update stops
            for (var i = 0; i < count; i++)
            {
                var control = _stops[i];
                var stop = _data[i];
                control.Location = new Vector2(stop.Frame * Width - control.Width * 0.5f, 0.0f);
                control.Index = i;
                control.TooltipText = stop.Value + " at " + stop.Frame;
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;
            var bounds = new Rectangle(Vector2.Zero, Size);
            var count = _data.Count;
            if (count == 0)
            {
                //Render2D.FillRectangle(bounds, Color.Black);
            }
            else if (count == 1)
            {
                Render2D.FillRectangle(bounds, _data[0].Value);
            }
            else
            {
                var prevStop = _data[0];
                var width = Width;
                var height = Height;

                // TODO: finish drawing gradient stops
                /*if (prevStop.Time > 0.0f)
                {
                    Render2D.FillRectangle(new Rectangle(Vector2.Zero, prevStop.Time * width, height), prevStop.Value);
                }

                for (int i = 1; i < count; i++)
                {
                    var curStop = _data[i];

                    Render2D.FillRectangle(new Rectangle(prevStop.Time * width, 0, (curStop.Time - prevStop.Time) * width, height), prevStop.Value, curStop.Value, curStop.Value, prevStop.Value);

                    prevStop = curStop;
                }

                if (prevStop.Time < 1.0f)
                {
                    Render2D.FillRectangle(new Rectangle(prevStop.Time * width, 0, (1.0f - prevStop.Time) * width, height), prevStop.Value);
                }*/
            }
            Render2D.DrawRectangle(bounds, IsMouseOver ? style.BackgroundHighlighted : style.Background);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            _stops.Clear();
            _data.Clear();
            _stops = null;
            _data = null;

            base.Dispose();
        }
    }
}
