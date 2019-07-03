// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// The Bezier curve editor control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class CurveEditor<T> : ContainerControl where T : struct
    {
        /// <summary>
        /// A single keyframe that can be injected into Bezier curve.
        /// </summary>
        public struct Keyframe
        {
            /// <summary>
            /// The time of the keyframe.
            /// </summary>
            public float Time;

            /// <summary>
            /// The value of the curve at keyframe.
            /// </summary>
            public T Value;

            /// <summary>
            /// The input tangent (going from the previous key to this one) of the key.
            /// </summary>
            public T TangentIn;

            /// <summary>
            /// The output tangent (going from this key to next one) of the key.
            /// </summary>
            public T TangentOut;
        }

        /// <summary>
        /// The curve background control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
        private class Background : ContainerControl
        {
            private readonly CurveEditor<T> _curve;

            /// <summary>
            /// Initializes a new instance of the <see cref="Background"/> class.
            /// </summary>
            /// <param name="curve">The curve.</param>
            public Background(CurveEditor<T> curve)
            {
                _curve = curve;
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
                var mainPanel = _curve._mainPanel;
                var linesColor = Style.Current.BackgroundNormal;
                var areaLeft = -X;
                var areaRight = Parent.Width + mainPanel.ControlsBounds.BottomRight.X;
                var height = Height;
                var leftSideMin = PointFromParent(Vector2.Zero);
                var leftSideMax = BottomLeft;
                var rightSideMin = UpperRight;
                var rightSideMax = PointFromParent(Parent.BottomRight) + mainPanel.ControlsBounds.BottomRight;
                /*
                // Draw vertical lines for time axis
                var framesPerSecond = 60.0f;
                var leftFrame = Mathf.Floor(leftSideMin.X / UnitsPerSecond) * framesPerSecond;
                var rightFrame = Mathf.Ceil(rightSideMax.X / UnitsPerSecond) * framesPerSecond;
                var verticalLinesHeaderExtend = IntervalsAreaHeight * 0.5f;
                for (float frame = leftFrame; frame <= rightFrame; frame += framesPerSecond)
                {
                    var time = frame / framesPerSecond;
                    var x = time * UnitsPerSecond;

                    // Vertical line
                    Render2D.FillRectangle(new Rectangle(x - 0.5f, 0, 1.0f, height), style.ForegroundDisabled.RGBMultiplied(0.7f));

                    // Header line
                    Render2D.FillRectangle(new Rectangle(x - 0.5f, -verticalLinesHeaderExtend, 1.0f, verticalLinesHeaderExtend), style.Foreground.RGBMultiplied(0.8f));

                    // Time
                    string label = time.ToString();
                    var labelRect = new Rectangle(x + 2, -verticalLinesHeaderExtend, 50, verticalLinesHeaderExtend);
                    Render2D.DrawText(style.FontSmall, label, labelRect, style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, 0.8f);
                }
                */
                DrawChildren();
                /*
                // Darken area outside the duration
                var outsideDurationAreaColor = new Color(0, 0, 0, 100);
                Render2D.FillRectangle(new Rectangle(leftSideMin, leftSideMax.X - leftSideMin.X, height), outsideDurationAreaColor);
                Render2D.FillRectangle(new Rectangle(rightSideMin, rightSideMax.X - rightSideMin.X, height), outsideDurationAreaColor);
                */
            }
        }

        /// <summary>
        /// The curve contents container control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
        private class Contents : ContainerControl
        {
            private readonly CurveEditor<T> _curve;

            /// <summary>
            /// Initializes a new instance of the <see cref="Contents"/> class.
            /// </summary>
            /// <param name="curve">The curve.</param>
            public Contents(CurveEditor<T> curve)
            {
                _curve = curve;
            }

            /// <inheritdoc />
            public override bool IntersectsContent(ref Vector2 locationParent, out Vector2 location)
            {
                // Pass all events
                location = PointFromParent(ref locationParent);
                return true;
            }

            /// <inheritdoc />
            public override bool OnMouseWheel(Vector2 location, float delta)
            {
                //if (Root.GetKey(Keys.Control))
                {
                    // Zoom in/out
                    _curve.ViewScale += delta * 0.1f;
                    return true;
                }

                return base.OnMouseWheel(location, delta);
            }
        }

        /// <summary>
        /// The single keyframe control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
        private class KeyframePoint : Control
        {
            /// <summary>
            /// The parent curve editor.
            /// </summary>
            public CurveEditor<T> Curve;

            /// <summary>
            /// The keyframe index.
            /// </summary>
            public int Index;

            /// <inheritdoc />
            public override void Draw()
            {
                // TODO: impl drawing
                BackgroundColor = Color.Green;

                base.Draw();
            }
        }

        /// <summary>
        /// The timeline intervals metric area size (in pixels).
        /// </summary>
        public static readonly float IntervalsAreaHeight = 20.0f;

        /// <summary>
        /// The timeline units per second (on time axis).
        /// </summary>
        public static readonly float UnitsPerSecond = 100.0f;

        private Background _background;
        private Contents _contents;
        private Panel _mainPanel;
        private readonly List<KeyframePoint> _points = new List<KeyframePoint>();

        /// <summary>
        /// The keyframes collection.
        /// </summary>
        protected readonly List<Keyframe> _keyframes = new List<Keyframe>();

        /// <summary>
        /// Occurs when keyframes collection gets changed (keyframe added or removed).
        /// </summary>
        public event Action KeyframesChanged;

        /// <summary>
        /// Gets the keyframes collection (read-only).
        /// </summary>
        public IReadOnlyCollection<Keyframe> Keyframes => _keyframes;

        /// <summary>
        /// Gets or sets the view scale.
        /// </summary>
        public Vector2 ViewScale
        {
            get => _contents.Scale;
            set => _contents.Scale = Vector2.Clamp(value, new Vector2(0.05f), new Vector2(4.0f));
        }

        public CurveEditor()
        {
            _mainPanel = new Panel(ScrollBars.Both)
            {
                AlwaysShowScrollbars = true,
                BackgroundColor = Style.Current.Background.RGBMultiplied(0.7f),
                DockStyle = DockStyle.Fill,
                Parent = this
            };
            _contents = new Contents(this)
            {
                ClipChildren = false,
                BackgroundColor = Color.Red,
                Parent = _mainPanel
            };
            _background = new Background(this)
            {
                ClipChildren = false,
                Size = Vector2.Zero,
                Parent = _contents
            };

            UpdateBackgroundBounds();
        }

        /// <summary>
        /// Sets the keyframes collection.
        /// </summary>
        /// <param name="keyframes">The keyframes.</param>
        public void SetKeyframes(IEnumerable<Keyframe> keyframes)
        {
            if (keyframes == null)
                throw new ArgumentNullException(nameof(keyframes));
            var keyframesArray = keyframes as Keyframe[] ?? keyframes.ToArray();
            if (_keyframes.SequenceEqual(keyframesArray))
                return;

            _keyframes.Clear();
            _keyframes.AddRange(keyframesArray);
            _keyframes.Sort((a, b) => a.Time < b.Time ? 1 : 0);

            OnKeyframesChanged();
        }

        /// <summary>
        /// Called when keyframes collection gets changed (keyframe added or removed).
        /// </summary>
        protected virtual void OnKeyframesChanged()
        {
            while (_points.Count > _keyframes.Count)
            {
                var last = _points.Count - 1;
                _points[last].Dispose();
                _points.RemoveAt(last);
            }

            while (_points.Count < _keyframes.Count)
            {
                _points.Add(new KeyframePoint
                {
                    Size = new Vector2(4.0f),
                    Curve = this,
                    Index = _keyframes.Count,
                    Parent = _contents,
                });
            }

            UpdateBackgroundBounds();

            KeyframesChanged?.Invoke();
        }

        private void UpdateBackgroundBounds()
        {
            // TODO: get proper values from keyframes
            float minTime = 0.0f;
            float maxTime = 1.0f;
            float minValue = 0.0f;
            float maxValue = 1.0f;

            _background.Bounds = new Rectangle(minTime * UnitsPerSecond, minValue * UnitsPerSecond, (maxTime - minTime) * UnitsPerSecond, (maxValue - minValue) * UnitsPerSecond);
            _contents.Bounds = _background.Bounds;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            // Clear references to the controls
            _background = null;
            _mainPanel = null;

            // Cleanup
            _points.Clear();
            _keyframes.Clear();

            base.Dispose();
        }
    }
}
