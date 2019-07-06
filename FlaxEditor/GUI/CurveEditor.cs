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
        /// The generic keyframe value accessor object for curve editor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
        public interface IKeyframeAccess<T> where T : struct
        {
            /// <summary>
            /// Gets the curve components count. Vector types should return amount of component to use for value editing.
            /// </summary>
            /// <returns>The components count.</returns>
            int GetCurveComponents();

            /// <summary>
            /// Gets the value of the component for the curve.
            /// </summary>
            /// <param name="value">The keyframe value.</param>
            /// <param name="component">The component index.</param>
            /// <returns>The curve value.</returns>
            float GetCurveValue(ref T value, int component);

            /// <summary>
            /// Sets the curve value of the component.
            /// </summary>
            /// <param name="curve">The curve value to assign.</param>
            /// <param name="value">The keyframe value.</param>
            /// <param name="component">The component index.</param>
            void SetCurveValue(float curve, ref T value, int component);
        }

        private class KeyframeAccess :
        IKeyframeAccess<int>,
        IKeyframeAccess<double>,
        IKeyframeAccess<float>,
        IKeyframeAccess<Vector2>,
        IKeyframeAccess<Vector3>,
        IKeyframeAccess<Vector4>,
        IKeyframeAccess<Quaternion>,
        IKeyframeAccess<Color>
        {
            int IKeyframeAccess<int>.GetCurveComponents()
            {
                return 1;
            }

            float IKeyframeAccess<int>.GetCurveValue(ref int value, int component)
            {
                return value;
            }

            void IKeyframeAccess<int>.SetCurveValue(float curve, ref int value, int component)
            {
                value = (int)curve;
            }

            int IKeyframeAccess<double>.GetCurveComponents()
            {
                return 1;
            }

            float IKeyframeAccess<double>.GetCurveValue(ref double value, int component)
            {
                return (float)value;
            }

            void IKeyframeAccess<double>.SetCurveValue(float curve, ref double value, int component)
            {
                value = curve;
            }

            int IKeyframeAccess<float>.GetCurveComponents()
            {
                return 1;
            }

            float IKeyframeAccess<float>.GetCurveValue(ref float value, int component)
            {
                return value;
            }

            void IKeyframeAccess<float>.SetCurveValue(float curve, ref float value, int component)
            {
                value = curve;
            }

            int IKeyframeAccess<Vector2>.GetCurveComponents()
            {
                return 2;
            }

            float IKeyframeAccess<Vector2>.GetCurveValue(ref Vector2 value, int component)
            {
                return value[component];
            }

            void IKeyframeAccess<Vector2>.SetCurveValue(float curve, ref Vector2 value, int component)
            {
                value[component] = curve;
            }

            int IKeyframeAccess<Vector3>.GetCurveComponents()
            {
                return 3;
            }

            float IKeyframeAccess<Vector3>.GetCurveValue(ref Vector3 value, int component)
            {
                return value[component];
            }

            void IKeyframeAccess<Vector3>.SetCurveValue(float curve, ref Vector3 value, int component)
            {
                value[component] = curve;
            }

            int IKeyframeAccess<Vector4>.GetCurveComponents()
            {
                return 4;
            }

            float IKeyframeAccess<Vector4>.GetCurveValue(ref Vector4 value, int component)
            {
                return value[component];
            }

            void IKeyframeAccess<Vector4>.SetCurveValue(float curve, ref Vector4 value, int component)
            {
                value[component] = curve;
            }

            int IKeyframeAccess<Quaternion>.GetCurveComponents()
            {
                return 3;
            }

            float IKeyframeAccess<Quaternion>.GetCurveValue(ref Quaternion value, int component)
            {
                return value.EulerAngles[component];
            }

            void IKeyframeAccess<Quaternion>.SetCurveValue(float curve, ref Quaternion value, int component)
            {
                var euler = value.EulerAngles;
                euler[component] = curve;
                Quaternion.Euler(euler.X, euler.Y, euler.Z, out value);
            }

            int IKeyframeAccess<Color>.GetCurveComponents()
            {
                return 4;
            }

            float IKeyframeAccess<Color>.GetCurveValue(ref Color value, int component)
            {
                return value[component];
            }

            void IKeyframeAccess<Color>.SetCurveValue(float curve, ref Color value, int component)
            {
                value[component] = curve;
            }
        }

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
            private bool _leftMouseDown;
            private bool _rightMouseDown;
            private Vector2 _leftMouseDownPos = Vector2.Minimum;
            private Vector2 _rightMouseDownPos = Vector2.Minimum;
            private Vector2 _mousePos = Vector2.Minimum;
            private float _mouseMoveAmount;
            private bool _isMovingSelection;
            private Vector2 _movingSelectionViewPos;

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
            public override void OnMouseEnter(Vector2 location)
            {
                _mousePos = location;

                base.OnMouseEnter(location);
            }

            /// <inheritdoc />
            public override void OnMouseMove(Vector2 location)
            {
                _mousePos = location;

                // Moving view
                if (_rightMouseDown)
                {
                    // Calculate delta
                    Vector2 delta = location - _rightMouseDownPos;
                    if (delta.LengthSquared > 0.01f)
                    {
                        // Move view
                        _mouseMoveAmount += delta.Length;
                        _curve.ViewOffset += delta * _curve.ViewScale;
                        _rightMouseDownPos = location;
                        Cursor = CursorType.SizeAll;
                    }

                    return;
                }
                // Moving selection
                else if (_isMovingSelection)
                {
                    // Calculate delta (apply view offset)
                    /*Vector2 viewDelta = _curve.ViewOffset - _movingSelectionViewPos;
                    _movingSelectionViewPos = _curve.ViewOffset;
                    Vector2 delta = location - _leftMouseDownPos - viewDelta;
                    if (delta.LengthSquared > 0.01f)
                    {
                        // Move selected keyframes
                        delta /= _targetScale;
                        for (auto keyframe : _curve._points)
                        {
                            control.Location += delta;
                        }
                        _leftMouseDownPos = location;
                        Cursor = CursorType.SizeAll;
                        MarkAsEdited(false);
                    }*/

                    return;
                }
                // Selecting
                else if (_rightMouseDown)
                {
                    //UpdateSelectionRectangle();

                    return;
                }

                base.OnMouseMove(location);
            }

            /// <inheritdoc />
            public override void OnLostFocus()
            {
                // Clear flags and state
                if (_leftMouseDown)
                {
                    _leftMouseDown = false;
                }
                if (_rightMouseDown)
                {
                    _rightMouseDown = false;
                    Cursor = CursorType.Default;
                }
                _isMovingSelection = false;

                base.OnLostFocus();
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (base.OnMouseDown(location, buttons))
                {
                    // Clear flags
                    _isMovingSelection = false;
                    _rightMouseDown = false;
                    _leftMouseDown = false;
                    return true;
                }

                // Cache data
                _isMovingSelection = false;
                _mousePos = location;
                if (buttons == MouseButton.Left)
                {
                    _leftMouseDown = true;
                    _leftMouseDownPos = location;
                }
                if (buttons == MouseButton.Right)
                {
                    _rightMouseDown = true;
                    _rightMouseDownPos = location;
                }
                /*
                // Check if any node is under the mouse
                var controlUnderMouse = GetControlUnderMouse();
                Vector2 cLocation = _rootControl.PointFromParent(ref location);
                if (controlUnderMouse != null)
                {
                    // Check if mouse is over header and user is pressing mouse left button
                    if (_leftMouseDown && controlUnderMouse.CanSelect(ref cLocation))
                    {
                        // Check if user is pressing control
                        if (Root.GetKey(Keys.Control))
                        {
                            // Add to selection
                            AddToSelection(controlUnderMouse);
                        }
                        // Check if node isn't selected
                        else if (!controlUnderMouse.IsSelected)
                        {
                            // Select node
                            Select(controlUnderMouse);
                        }

                        // Start moving selected nodes
                        StartMouseCapture();
                        _isMovingSelection = true;
                        _movingSelectionViewPos = _curve.ViewOffset;
                        Focus();
                        return true;
                    }
                }
                else*/
                {
                    if (_leftMouseDown)
                    {
                        // Start selecting
                        StartMouseCapture();
                        _curve.ClearSelection();
                        Focus();
                        return true;
                    }
                    if (_rightMouseDown)
                    {
                        // Start navigating
                        StartMouseCapture();
                        Focus();
                        return true;
                    }
                }

                Focus();
                return true;
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                _mousePos = location;

                // Check if any control is under the mouse
                //var controlUnderMouse = GetControlUnderMouse();

                // Cache flags and state
                if (_leftMouseDown && buttons == MouseButton.Left)
                {
                    _leftMouseDown = false;
                    EndMouseCapture();
                    Cursor = CursorType.Default;

                    // Selecting
                    if (!_isMovingSelection)
                    {
                        //UpdateSelectionRectangle();
                    }
                }
                if (_rightMouseDown && buttons == MouseButton.Right)
                {
                    _rightMouseDown = false;
                    EndMouseCapture();
                    Cursor = CursorType.Default;

                    // Check if no move has been made at all
                    if (_mouseMoveAmount < 3.0f)
                    {
                        // TODO: select keyframe under mouse if no selection
                        // TODO: show context menu for selected keyframes
                    }
                    _mouseMoveAmount = 0;
                }
                if (base.OnMouseUp(location, buttons))
                {
                    // Clear flags
                    _rightMouseDown = false;
                    _leftMouseDown = false;
                    return true;
                }

                return true;
            }

            /// <inheritdoc />
            public override bool OnMouseWheel(Vector2 location, float delta)
            {
                if (base.OnMouseWheel(location, delta))
                    return true;

                // Zoom in/out
                if (IsMouseOver && !_leftMouseDown)
                {
                    // TODO: preserve the view center point for easier zooming
                    _curve.ViewScale += delta * 0.1f;
                    return true;
                }

                return false;
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
        /// Gets or sets the view offset (via scroll bars).
        /// </summary>
        public Vector2 ViewOffset
        {
            get => _mainPanel.ViewOffset;
            set => _mainPanel.ViewOffset = value;
        }

        /// <summary>
        /// Gets or sets the view scale.
        /// </summary>
        public Vector2 ViewScale
        {
            get => _contents.Scale;
            set => _contents.Scale = Vector2.Clamp(value, new Vector2(0.05f), new Vector2(4.0f));
        }

        /// <summary>
        /// The keyframes data accessor.
        /// </summary>
        public readonly IKeyframeAccess<T> Accessor = new KeyframeAccess() as IKeyframeAccess<T>;

        public CurveEditor()
        {
            _mainPanel = new Panel(ScrollBars.Both)
            {
                AlwaysShowScrollbars = true,
                //ScrollMargin = new Margin(50.0f),
                BackgroundColor = Style.Current.Background.RGBMultiplied(0.7f),
                DockStyle = DockStyle.Fill,
                Parent = this
            };
            _contents = new Contents(this)
            {
                ClipChildren = false,
                AutoFocus = false,
                BackgroundColor = Color.Red.AlphaMultiplied(0.1f),
                Parent = _mainPanel
            };
            _background = new Background(this)
            {
                ClipChildren = false,
                AutoFocus = false,
                Size = Vector2.Zero,
                Parent = _contents
            };

            UpdateKeyframes();
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

            if (Accessor.GetCurveComponents() != 1)
                throw new NotImplementedException("TODO: add support for multi-component curves editing");

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

            UpdateKeyframes();

            KeyframesChanged?.Invoke();
        }

        private void UpdateKeyframes()
        {
            if (_points.Count == 0)
            {
                // No keyframes
                _contents.Bounds = Rectangle.Empty;
                return;
            }

            _mainPanel.IsLayoutLocked = true;

            // Place keyframes
            for (int i = 0; i < _keyframes.Count; i++)
            {
                var p = _points[i];
                var k = _keyframes[i];

                var x = k.Time;
                var y = Accessor.GetCurveValue(ref k.Value, 0);

                p.Location = new Vector2
                (
                    x * UnitsPerSecond - p.Width * 0.5f,
                    y * UnitsPerSecond - p.Height * 0.5f
                );
            }

            // Calculate bounds
            var bounds = _points[0].Bounds;
            for (var i = 1; i < _points.Count; i++)
            {
                bounds = Rectangle.Union(bounds, _points[i].Bounds);
            }

            // Adjust contents bounds to fill the curve area
            _contents.Bounds = bounds;

            // Offset the keyframes (parent container changed its location)
            for (var i = 0; i < _points.Count; i++)
            {
                _points[i].Location -= bounds.Location;
            }

            _mainPanel.IsLayoutLocked = false;
            _mainPanel.PerformLayout();
        }

        private void ClearSelection()
        {
            // TODO: impl this
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var rect = new Rectangle(Vector2.Zero, Size);

            base.Draw();

            // TODO: draw selection
            /*if (IsSelecting)
            {
                DrawSelection();
            }*/

            // Draw border
            if (ContainsFocus)
            {
                Render2D.DrawRectangle(rect, style.BackgroundSelected);
            }
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
