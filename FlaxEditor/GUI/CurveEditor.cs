// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI.ContextMenu;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// The Bezier curve editor control.
    /// </summary>
    /// <typeparam name="T">The keyframe value type.</typeparam>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class CurveEditor<T> : ContainerControl where T : struct
    {
        /// <summary>
        /// The generic keyframe value accessor object for curve editor.
        /// </summary>
        /// <typeparam name="TT">The keyframe value type.</typeparam>
        public interface IKeyframeAccess<TT> where TT : struct
        {
            /// <summary>
            /// Gets the default value.
            /// </summary>
            /// <param name="value">The value.</param>
            void GetDefaultValue(out TT value);

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
            float GetCurveValue(ref TT value, int component);

            /// <summary>
            /// Sets the curve value of the component.
            /// </summary>
            /// <param name="curve">The curve value to assign.</param>
            /// <param name="value">The keyframe value.</param>
            /// <param name="component">The component index.</param>
            void SetCurveValue(float curve, ref TT value, int component);
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
            public void GetDefaultValue(out int value)
            {
                value = 0;
            }

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

            public void GetDefaultValue(out double value)
            {
                value = 0.0;
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

            public void GetDefaultValue(out float value)
            {
                value = 0.0f;
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

            public void GetDefaultValue(out Vector2 value)
            {
                value = Vector2.Zero;
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

            public void GetDefaultValue(out Vector3 value)
            {
                value = Vector3.Zero;
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

            public void GetDefaultValue(out Vector4 value)
            {
                value = Vector4.Zero;
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

            public void GetDefaultValue(out Quaternion value)
            {
                value = Quaternion.Identity;
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

            public void GetDefaultValue(out Color value)
            {
                value = Color.Black;
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
        /// The curve contents container control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
        private class Contents : ContainerControl
        {
            private readonly CurveEditor<T> _curve;
            internal bool _leftMouseDown;
            private bool _rightMouseDown;
            internal Vector2 _leftMouseDownPos = Vector2.Minimum;
            private Vector2 _rightMouseDownPos = Vector2.Minimum;
            internal Vector2 _mousePos = Vector2.Minimum;
            private float _mouseMoveAmount;
            internal bool _isMovingSelection;
            internal bool _isMovingTangent;
            private TangentPoint _movingTangent;
            private Vector2 _movingSelectionViewPos;
            private Vector2 _cmShowPos;

            /// <summary>
            /// Initializes a new instance of the <see cref="Contents"/> class.
            /// </summary>
            /// <param name="curve">The curve.</param>
            public Contents(CurveEditor<T> curve)
            {
                _curve = curve;
            }

            private void UpdateSelectionRectangle()
            {
                var selectionRect = Rectangle.FromPoints(_leftMouseDownPos, _mousePos);

                // Find controls to select
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i] is KeyframePoint p)
                    {
                        p.IsSelected = p.Bounds.Intersects(ref selectionRect);
                    }
                }

                _curve.UpdateTangents();
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
                    if (delta.LengthSquared > 0.01f && _curve.EnablePanning)
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
                    Vector2 viewDelta = _curve.ViewOffset - _movingSelectionViewPos;
                    _movingSelectionViewPos = _curve.ViewOffset;
                    var viewRect = _curve._mainPanel.GetClientArea();
                    var delta = location - _leftMouseDownPos - viewDelta;
                    _mouseMoveAmount += delta.Length;
                    if (delta.LengthSquared > 0.01f)
                    {
                        // Move selected keyframes
                        var keyframeDelta = PointToKeyframes(location, ref viewRect) - PointToKeyframes(_leftMouseDownPos - viewDelta, ref viewRect);
                        var accessor = _curve.Accessor;
                        var components = accessor.GetCurveComponents();
                        for (var i = 0; i < _curve._points.Count; i++)
                        {
                            var p = _curve._points[i];
                            if (p.IsSelected)
                            {
                                var k = _curve._keyframes[p.Index];
                                float value = accessor.GetCurveValue(ref k.Value, p.Component);

                                float minTime = p.Index != 0 ? _curve._keyframes[p.Index - 1].Time : float.MinValue;
                                float maxTime = p.Index != _curve._keyframes.Count - 1 ? _curve._keyframes[p.Index + 1].Time : float.MaxValue;

                                value += keyframeDelta.Y;

                                // Let the first selected point of this keyframe to edit time
                                bool isFirstSelected = false;
                                for (var j = 0; j < components; j++)
                                {
                                    var idx = p.Index * components + j;
                                    if (idx == i)
                                    {
                                        isFirstSelected = true;
                                        break;
                                    }
                                    if (_curve._points[idx].IsSelected)
                                        break;
                                }
                                if (isFirstSelected)
                                {
                                    k.Time = Mathf.Clamp(k.Time + keyframeDelta.X, minTime, maxTime);

                                    if (_curve.FPS.HasValue)
                                    {
                                        float fps = _curve.FPS.Value;
                                        k.Time = Mathf.Floor(k.Time * fps) / fps;
                                    }
                                }

                                // TODO: snapping keyframes to grid when moving

                                accessor.SetCurveValue(value, ref k.Value, p.Component);
                                _curve._keyframes[p.Index] = k;
                            }
                        }
                        _curve.UpdateKeyframes();
                        if (_curve.EnablePanning)
                            _curve._mainPanel.ScrollViewTo(PointToParent(location));
                        _leftMouseDownPos = location;
                        Cursor = CursorType.SizeAll;
                    }

                    return;
                }
                // Moving tangent
                else if (_isMovingTangent)
                {
                    // Calculate delta (apply view offset)
                    Vector2 viewDelta = _curve.ViewOffset - _movingSelectionViewPos;
                    _movingSelectionViewPos = _curve.ViewOffset;
                    var viewRect = _curve._mainPanel.GetClientArea();
                    var delta = location - _leftMouseDownPos - viewDelta;
                    _mouseMoveAmount += delta.Length;
                    if (delta.LengthSquared > 0.01f)
                    {
                        // Move selected tangent
                        var keyframeDelta = PointToKeyframes(location, ref viewRect) - PointToKeyframes(_leftMouseDownPos - viewDelta, ref viewRect);
                        var direction = _movingTangent.IsIn ? -1.0f : 1.0f;
                        _movingTangent.TangentValue += direction * keyframeDelta.Y;
                        _curve.UpdateTangents();
                        _leftMouseDownPos = location;
                        Cursor = CursorType.SizeNS;
                    }

                    return;
                }
                // Selecting
                else if (_leftMouseDown)
                {
                    UpdateSelectionRectangle();
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
                _isMovingTangent = false;

                base.OnLostFocus();
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (base.OnMouseDown(location, buttons))
                {
                    // Clear flags
                    _isMovingSelection = false;
                    _isMovingTangent = false;
                    _rightMouseDown = false;
                    _leftMouseDown = false;
                    return true;
                }

                // Cache data
                _isMovingSelection = false;
                _isMovingTangent = false;
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

                // Check if any node is under the mouse
                var underMouse = GetChildAt(location);
                if (underMouse is KeyframePoint keyframe)
                {
                    if (_leftMouseDown)
                    {
                        // Check if user is pressing control
                        if (Root.GetKey(Keys.Control))
                        {
                            // Add to selection
                            keyframe.Select();
                            _curve.UpdateTangents();
                        }
                        // Check if node isn't selected
                        else if (!keyframe.IsSelected)
                        {
                            // Select node
                            _curve.ClearSelection();
                            keyframe.Select();
                            _curve.UpdateTangents();
                        }

                        // Start moving selected nodes
                        StartMouseCapture();
                        _mouseMoveAmount = 0;
                        _isMovingSelection = true;
                        _movingSelectionViewPos = _curve.ViewOffset;
                        Focus();
                        return true;
                    }
                }
                else if (underMouse is TangentPoint tangent && tangent.Visible)
                {
                    if (_leftMouseDown)
                    {
                        // Start moving tangent
                        StartMouseCapture();
                        _mouseMoveAmount = 0;
                        _isMovingTangent = true;
                        _movingTangent = tangent;
                        _movingSelectionViewPos = _curve.ViewOffset;
                        Focus();
                        return true;
                    }
                }
                else
                {
                    if (_leftMouseDown)
                    {
                        // Start selecting
                        StartMouseCapture();
                        _curve.ClearSelection();
                        _curve.UpdateTangents();
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

                if (_leftMouseDown && buttons == MouseButton.Left)
                {
                    _leftMouseDown = false;
                    EndMouseCapture();
                    Cursor = CursorType.Default;

                    // Editing tangent
                    if (_isMovingTangent)
                    {
                        if (_mouseMoveAmount > 3.0f)
                            _curve.MarkAsEdited();
                    }
                    // Moving keyframes
                    else if (_isMovingSelection)
                    {
                        if (_mouseMoveAmount > 3.0f)
                            _curve.MarkAsEdited();
                    }
                    // Selecting
                    else
                    {
                        UpdateSelectionRectangle();
                    }

                    _isMovingSelection = false;
                    _isMovingTangent = false;
                }
                if (_rightMouseDown && buttons == MouseButton.Right)
                {
                    _rightMouseDown = false;
                    EndMouseCapture();
                    Cursor = CursorType.Default;

                    // Check if no move has been made at all
                    if (_mouseMoveAmount < 3.0f)
                    {
                        var selectionCount = _curve.SelectionCount;
                        var underMouse = GetChildAt(location);
                        if (selectionCount == 0 && underMouse is KeyframePoint point)
                        {
                            // Select node
                            selectionCount = 1;
                            point.Select();
                            _curve.UpdateTangents();
                        }

                        var viewRect = _curve._mainPanel.GetClientArea();
                        _cmShowPos = PointToKeyframes(location, ref viewRect);

                        var cm = new ContextMenu.ContextMenu();
                        cm.AddButton("Add keyframe", () => _curve.AddKeyframe(_cmShowPos)).Enabled = _curve.Keyframes.Count < _curve.MaxKeyframes;
                        if (selectionCount == 0)
                        {
                        }
                        else if (selectionCount == 1)
                        {
                            cm.AddButton("Edit keyframe", () => _curve.EditKeyframes(this, location));
                            cm.AddButton("Remove keyframe", _curve.RemoveKeyframes);
                        }
                        else
                        {
                            cm.AddButton("Edit keyframes", () => _curve.EditKeyframes(this, location));
                            cm.AddButton("Remove keyframes", _curve.RemoveKeyframes);
                        }
                        if (selectionCount != 0)
                        {
                            cm.AddSeparator();
                            cm.AddButton("Linear", _curve.SetTangentsLinear);
                            cm.AddButton("Smooth", _curve.SetTangentsSmooth);
                        }
                        if (_curve.EnableZoom && _curve.EnablePanning)
                        {
                            cm.AddSeparator();
                            cm.AddButton("Show whole curve", _curve.ShowWholeCurve);
                            cm.AddButton("Reset view", _curve.ResetView);
                        }
                        cm.Show(this, location);
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
                if (_curve.EnableZoom && IsMouseOver && !_leftMouseDown)
                {
                    // TODO: preserve the view center point for easier zooming
                    _curve.ViewScale += delta * 0.1f;
                    return true;
                }

                return false;
            }

            /// <inheritdoc />
            protected override void SetScaleInternal(ref Vector2 scale)
            {
                base.SetScaleInternal(ref scale);

                _curve.UpdateKeyframes();
            }

            /// <summary>
            /// Converts the input point from curve editor contents control space into the keyframes time/value coordinates.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <param name="point">The curve contents area bounds.</param>
            /// <returns>The result.</returns>
            private Vector2 PointToKeyframes(Vector2 point, ref Rectangle curveContentAreaBounds)
            {
                // Contents -> Keyframes
                return new Vector2(
                    (point.X + Location.X) / UnitsPerSecond,
                    (point.Y + Location.Y - curveContentAreaBounds.Height) / -UnitsPerSecond
                );
            }
        }

        /// <summary>
        /// The single keyframe control.
        /// </summary>
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

            /// <summary>
            /// The component index.
            /// </summary>
            public int Component;

            /// <summary>
            /// Flag for selected keyframes.
            /// </summary>
            public bool IsSelected;

            /// <inheritdoc />
            public override void Draw()
            {
                var rect = new Rectangle(Vector2.Zero, Size);
                var color = Colors[Component];
                if (IsSelected)
                    color = Color.YellowGreen;
                if (IsMouseOver)
                    color *= 1.1f;
                Render2D.FillRectangle(rect, color);
            }

            /// <inheritdoc />
            protected override void SetLocationInternal(ref Vector2 location)
            {
                base.SetLocationInternal(ref location);

                var k = Curve._keyframes[Index];
                TooltipText = string.Format("Time: {0}, Value: {1}", k.Time, Curve.Accessor.GetCurveValue(ref k.Value, Component));
            }

            public void Select()
            {
                IsSelected = true;
            }

            public void Deselect()
            {
                IsSelected = false;
            }
        }

        /// <summary>
        /// The single keyframe tangent control.
        /// </summary>
        private class TangentPoint : Control
        {
            /// <summary>
            /// The parent curve editor.
            /// </summary>
            public CurveEditor<T> Curve;

            /// <summary>
            /// The keyframe index.
            /// </summary>
            public int Index;

            /// <summary>
            /// The component index.
            /// </summary>
            public int Component;

            /// <summary>
            /// True if tangent is `In`, otherwise it's `Out`.
            /// </summary>
            public bool IsIn;

            /// <summary>
            /// The keyframe.
            /// </summary>
            public KeyframePoint Point;

            /// <summary>
            /// Gets the tangent value on curve.
            /// </summary>
            public float TangentValue
            {
                get
                {
                    var k = Curve._keyframes[Index];
                    var value = IsIn ? k.TangentIn : k.TangentOut;
                    return Curve.Accessor.GetCurveValue(ref value, Component);
                }
                set
                {
                    var k = Curve._keyframes[Index];
                    if (IsIn)
                        Curve.Accessor.SetCurveValue(value, ref k.TangentIn, Component);
                    else
                        Curve.Accessor.SetCurveValue(value, ref k.TangentOut, Component);
                    Curve._keyframes[Index] = k;
                }
            }

            /// <inheritdoc />
            public override void Draw()
            {
                var pointPos = PointFromParent(Point.Center);
                Render2D.DrawLine(Size * 0.5f, pointPos, Color.Gray);

                var rect = new Rectangle(Vector2.Zero, Size);
                var color = Color.MediumVioletRed;
                if (IsMouseOver)
                    color *= 1.1f;
                Render2D.FillRectangle(rect, color);
            }

            /// <inheritdoc />
            protected override void SetLocationInternal(ref Vector2 location)
            {
                base.SetLocationInternal(ref location);

                TooltipText = string.Format("Tangent {0}: {1}", IsIn ? "in" : "out", TangentValue);
            }
        }

        /// <summary>
        /// The timeline intervals metric area size (in pixels).
        /// </summary>
        private static readonly float LabelsSize = 10.0f;

        /// <summary>
        /// The timeline units per second (on time axis).
        /// </summary>
        public static readonly float UnitsPerSecond = 100.0f;

        /// <summary>
        /// The colors for the keyframes,
        /// </summary>
        private static readonly Color[] Colors =
        {
            Color.OrangeRed,
            Color.ForestGreen,
            Color.CornflowerBlue,
            Color.White,
        };

        /// <summary>
        /// The time/value axes tick steps.
        /// </summary>
        private static readonly float[] TickSteps =
        {
            0.0000001f, 0.0000005f, 0.000001f, 0.000005f, 0.00001f,
            0.00005f, 0.0001f, 0.0005f, 0.001f, 0.005f,
            0.01f, 0.05f, 0.1f, 0.5f, 1,
            5, 10, 50, 100, 500,
            1000, 5000, 10000, 50000, 100000,
            500000, 1000000, 5000000, 10000000, 100000000
        };

        /// <summary>
        /// The keyframes size.
        /// </summary>
        private static readonly Vector2 KeyframesSize = new Vector2(5.0f);

        private Contents _contents;
        private Panel _mainPanel;
        private readonly List<KeyframePoint> _points = new List<KeyframePoint>();
        private readonly TangentPoint[] _tangents = new TangentPoint[2];
        private readonly float[] _tickStrengths = new float[TickSteps.Length];
        private bool _refreshAfterEdit;
        private KeyframesEditor _keyframesEditor;
        private float? _fps;

        private Color _contentsColor;
        private Color _linesColor;
        private Color _labelsColor;
        private Font _labelsFont;

        /// <summary>
        /// The keyframes collection.
        /// </summary>
        protected readonly List<Curve<T>.Keyframe> _keyframes = new List<Curve<T>.Keyframe>();

        /// <summary>
        /// Occurs when keyframes collection gets changed (keyframe added or removed).
        /// </summary>
        public event Action KeyframesChanged;

        /// <summary>
        /// Gets the keyframes collection (read-only).
        /// </summary>
        public IReadOnlyList<Curve<T>.Keyframe> Keyframes => _keyframes;

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
            set => _contents.Scale = Vector2.Clamp(value, new Vector2(0.02f), new Vector2(10.0f));
        }

        /// <summary>
        /// The keyframes data accessor.
        /// </summary>
        public readonly IKeyframeAccess<T> Accessor = new KeyframeAccess() as IKeyframeAccess<T>;

        /// <summary>
        /// Occurs when curve gets edited.
        /// </summary>
        public event Action Edited;

        /// <summary>
        /// The maximum amount of keyframes to use in a single curve.
        /// </summary>
        public int MaxKeyframes = ushort.MaxValue;

        /// <summary>
        /// True if enable view zooming. Otherwise user won't be able to zoom in or out.
        /// </summary>
        public bool EnableZoom = true;

        /// <summary>
        /// True if enable view panning. Otherwise user won't be able to move the view area.
        /// </summary>
        public bool EnablePanning = true;

        /// <summary>
        /// Gets a value indicating whether user is editing the curve.
        /// </summary>
        public bool IsUserEditing => _keyframesEditor != null || _contents._leftMouseDown;

        /// <summary>
        /// Gets or sets the scroll bars usage.
        /// </summary>
        public ScrollBars ScrollBars
        {
            get => _mainPanel.ScrollBars;
            set => _mainPanel.ScrollBars = value;
        }

        /// <summary>
        /// The default value.
        /// </summary>
        public T DefaultValue;

        /// <summary>
        /// Enables drawing start/end values continuous lines.
        /// </summary>
        public bool ShowStartEndLines;

        /// <summary>
        /// The amount of frames per second of the curve animation (optional). Can be sued to restrict the keyframes time values to the given time quantization rate.
        /// </summary>
        public float? FPS
        {
            get => _fps;
            set
            {
                if (_fps.HasValue == value.HasValue && (!value.HasValue || Mathf.NearEqual(_fps.Value, value.Value)))
                    return;

                _fps = value;

                UpdateFPS();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveEditor{T}"/> class.
        /// </summary>
        public CurveEditor()
        {
            Accessor.GetDefaultValue(out DefaultValue);

            var style = Style.Current;
            _contentsColor = style.Background.RGBMultiplied(0.7f);
            _linesColor = style.ForegroundDisabled.RGBMultiplied(0.7f);
            _labelsColor = style.ForegroundDisabled;
            _labelsFont = style.FontSmall;

            _mainPanel = new Panel(ScrollBars.Both)
            {
                ScrollMargin = new Margin(150.0f),
                AlwaysShowScrollbars = true,
                DockStyle = DockStyle.Fill,
                Parent = this
            };
            _contents = new Contents(this)
            {
                ClipChildren = false,
                AutoFocus = false,
                Parent = _mainPanel
            };

            for (int i = 0; i < _tangents.Length; i++)
            {
                _tangents[i] = new TangentPoint
                {
                    AutoFocus = false,
                    Size = KeyframesSize,
                    Curve = this,
                    Component = i / 2,
                    Parent = _contents,
                    Visible = false,
                    IsIn = false,
                };
            }
            for (int i = 0; i < _tangents.Length; i += 2)
            {
                _tangents[i].IsIn = true;
            }

            UpdateKeyframes();
        }

        private void MarkAsEdited()
        {
            Edited?.Invoke();
        }

        /// <summary>
        /// Sets the keyframes collection.
        /// </summary>
        /// <param name="keyframes">The keyframes.</param>
        public void SetKeyframes(IEnumerable<Curve<T>.Keyframe> keyframes)
        {
            if (keyframes == null)
                throw new ArgumentNullException(nameof(keyframes));
            var keyframesArray = keyframes as Curve<T>.Keyframe[] ?? keyframes.ToArray();
            if (_keyframes.SequenceEqual(keyframesArray))
                return;
            if (keyframesArray.Length > MaxKeyframes)
            {
                var tmp = keyframesArray;
                keyframesArray = new Curve<T>.Keyframe[MaxKeyframes];
                Array.Copy(tmp, keyframesArray, MaxKeyframes);
            }

            _keyframes.Clear();
            _keyframes.AddRange(keyframesArray);
            _keyframes.Sort((a, b) => a.Time > b.Time ? 1 : 0);

            UpdateFPS();

            OnKeyframesChanged();
        }

        private void UpdateFPS()
        {
            if (FPS.HasValue)
            {
                float fps = FPS.Value;
                for (int i = 0; i < _keyframes.Count; i++)
                {
                    var k = _keyframes[i];
                    k.Time = Mathf.Floor(k.Time * fps) / fps;
                    _keyframes[i] = k;
                }
            }
        }

        /// <summary>
        /// Called when keyframes collection gets changed (keyframe added or removed).
        /// </summary>
        protected virtual void OnKeyframesChanged()
        {
            var components = Accessor.GetCurveComponents();
            while (_points.Count > _keyframes.Count * components)
            {
                var last = _points.Count - 1;
                _points[last].Dispose();
                _points.RemoveAt(last);
            }

            while (_points.Count < _keyframes.Count * components)
            {
                _points.Add(new KeyframePoint
                {
                    AutoFocus = false,
                    Size = KeyframesSize,
                    Curve = this,
                    Index = _points.Count / components,
                    Component = _points.Count % components,
                    Parent = _contents,
                });

                _refreshAfterEdit = true;
            }

            UpdateKeyframes();

            KeyframesChanged?.Invoke();
        }

        private void AddKeyframe(Vector2 keyframesPos)
        {
            if (FPS.HasValue)
            {
                float fps = FPS.Value;
                keyframesPos.X = Mathf.Floor(keyframesPos.X * fps) / fps;
            }
            int pos = 0;
            while (pos < _keyframes.Count && _keyframes[pos].Time < keyframesPos.X)
                pos++;
            var k = new Curve<T>.Keyframe
            {
                Time = keyframesPos.X,
            };
            var components = Accessor.GetCurveComponents();
            for (int component = 0; component < components; component++)
            {
                Accessor.SetCurveValue(keyframesPos.Y, ref k.Value, component);
                Accessor.SetCurveValue(0.0f, ref k.TangentIn, component);
                Accessor.SetCurveValue(0.0f, ref k.TangentOut, component);
            }
            _keyframes.Insert(pos, k);

            OnKeyframesChanged();
            MarkAsEdited();
        }

        class KeyframesEditor : ContextMenuBase
        {
            private CustomEditorPresenter _editor;
            public CurveEditor<T> Curve;
            public List<int> KeyframeIndices;
            public bool IsDirty;

            public KeyframesEditor(List<Curve<T>.Keyframe> keyframes)
            {
                const float width = 280.0f;
                const float height = 120.0f;
                Size = new Vector2(width, height);

                var panel1 = new Panel(ScrollBars.Vertical)
                {
                    Bounds = new Rectangle(0, 0.0f, width, height),
                    Parent = this
                };
                var editor = new CustomEditorPresenter(null);
                editor.Panel.DockStyle = DockStyle.Top;
                editor.Panel.IsScrollable = true;
                editor.Panel.Parent = panel1;
                editor.Modified += OnModified;

                var selection = new object[keyframes.Count];
                for (int i = 0; i < keyframes.Count; i++)
                    selection[i] = keyframes[i];
                editor.Select(selection);

                _editor = editor;
            }

            private void OnModified()
            {
                IsDirty = true;

                for (int i = 0; i < _editor.SelectionCount; i++)
                {
                    var keyframe = (Curve<T>.Keyframe)_editor.Selection[i];
                    var index = KeyframeIndices[i];
                    Curve._keyframes[index] = keyframe;
                }

                Curve.UpdateFPS();
                Curve.UpdateKeyframes();
            }

            /// <inheritdoc />
            protected override void OnShow()
            {
                Focus();

                base.OnShow();
            }

            /// <inheritdoc />
            public override void Hide()
            {
                if (!Visible)
                    return;

                Focus(null);

                if (IsDirty)
                {
                    Curve.MarkAsEdited();
                }

                if (Curve._keyframesEditor == this)
                    Curve._keyframesEditor = null;
                _editor = null;

                base.Hide();
            }

            /// <inheritdoc />
            public override bool OnKeyDown(Keys key)
            {
                if (key == Keys.Escape)
                {
                    Hide();
                    return true;
                }

                return base.OnKeyDown(key);
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                Curve = null;

                base.Dispose();
            }
        }

        private void EditKeyframes(Control control, Vector2 pos)
        {
            var keyframes = new List<Curve<T>.Keyframe>();
            var indices = new List<int>();
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                if (!p.IsSelected || indices.Contains(p.Index))
                    continue;

                indices.Add(p.Index);
                keyframes.Add(_keyframes[p.Index]);
            }

            _keyframesEditor = new KeyframesEditor(keyframes)
            {
                Curve = this,
                KeyframeIndices = indices,
            };
            _keyframesEditor.Show(control, pos);
        }

        private void RemoveKeyframes()
        {
            bool edited = false;
            var keyframes = new Dictionary<int, Curve<T>.Keyframe>(_keyframes.Count);
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                if (!p.IsSelected)
                {
                    keyframes[p.Index] = _keyframes[p.Index];
                }
                else
                {
                    p.Deselect();
                    edited = true;
                }
            }
            if (!edited)
                return;
            UpdateTangents();
            _keyframes.Clear();
            _keyframes.AddRange(keyframes.Values);

            OnKeyframesChanged();
            MarkAsEdited();
        }

        private void SetTangentsLinear()
        {
            bool edited = false;
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                if (!p.IsSelected)
                    continue;
                edited = true;

                var k = _keyframes[p.Index];
                var value = Accessor.GetCurveValue(ref k.Value, p.Component);

                if (p.Index > 0)
                {
                    var o = _keyframes[p.Index - 1];
                    var oValue = Accessor.GetCurveValue(ref o.Value, p.Component);

                    var slope = (value - oValue) / (k.Time - o.Time);
                    Accessor.SetCurveValue(slope, ref k.TangentIn, p.Component);
                    Accessor.SetCurveValue(slope, ref o.TangentOut, p.Component);

                    _keyframes[p.Index - 1] = o;
                }

                if (p.Index < _keyframes.Count - 1)
                {
                    var o = _keyframes[p.Index + 1];
                    var oValue = Accessor.GetCurveValue(ref o.Value, p.Component);

                    var slope = (oValue - value) / (o.Time - k.Time);
                    Accessor.SetCurveValue(slope, ref k.TangentOut, p.Component);
                    Accessor.SetCurveValue(slope, ref o.TangentIn, p.Component);

                    _keyframes[p.Index + 1] = o;
                }

                _keyframes[p.Index] = k;
            }
            if (!edited)
                return;

            UpdateTangents();
            MarkAsEdited();
        }

        private void SetTangentsSmooth()
        {
            bool edited = false;
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                if (!p.IsSelected)
                    continue;
                edited = true;

                var k = _keyframes[p.Index];

                if (p.Index > 0)
                {
                    var o = _keyframes[p.Index - 1];

                    var slope = 0.0f;
                    Accessor.SetCurveValue(slope, ref k.TangentIn, p.Component);
                    Accessor.SetCurveValue(slope, ref o.TangentOut, p.Component);

                    _keyframes[p.Index - 1] = o;
                }

                if (p.Index < _keyframes.Count - 1)
                {
                    var o = _keyframes[p.Index + 1];

                    var slope = 0.0f;
                    Accessor.SetCurveValue(slope, ref k.TangentOut, p.Component);
                    Accessor.SetCurveValue(slope, ref o.TangentIn, p.Component);

                    _keyframes[p.Index + 1] = o;
                }

                _keyframes[p.Index] = k;
            }
            if (!edited)
                return;

            UpdateTangents();
            MarkAsEdited();
        }

        /// <summary>
        /// Shows the whole curve.
        /// </summary>
        public void ShowWholeCurve()
        {
            ViewScale = _mainPanel.Size / _contents.Size;
            ViewOffset = -_mainPanel.ControlsBounds.Location;
        }

        /// <summary>
        /// Resets the view.
        /// </summary>
        public void ResetView()
        {
            ViewScale = Vector2.One;
            ViewOffset = Vector2.Zero;
        }

        private void UpdateTangents()
        {
            // Find selected keyframe
            Rectangle curveContentAreaBounds = _mainPanel.GetClientArea();
            var selectedCount = 0;
            var selectedIndex = -1;
            KeyframePoint selectedKeyframe = null;
            var selectedComponent = -1;
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                if (p.IsSelected)
                {
                    selectedIndex = p.Index;
                    selectedKeyframe = p;
                    selectedComponent = p.Component;
                    selectedCount++;
                }
            }

            // Place tangents (only for a single selected keyframe)
            if (selectedCount == 1)
            {
                var posOffset = _contents.Location;
                var k = _keyframes[selectedIndex];
                for (int i = 0; i < _tangents.Length; i++)
                {
                    var t = _tangents[i];

                    t.Index = selectedIndex;
                    t.Point = selectedKeyframe;
                    t.Component = selectedComponent;

                    var tangent = t.TangentValue;
                    var direction = t.IsIn ? -1.0f : 1.0f;
                    var offset = 30.0f * direction;
                    var location = GetKeyframePoint(ref k, selectedComponent);
                    t.Size = KeyframesSize / ViewScale;
                    t.Location = new Vector2
                    (
                        location.X * UnitsPerSecond - t.Width * 0.5f + offset,
                        location.Y * -UnitsPerSecond - t.Height * 0.5f + curveContentAreaBounds.Height - offset * tangent
                    );

                    var isFirst = selectedIndex == 0 && t.IsIn;
                    var isLast = selectedIndex == _keyframes.Count - 1 && !t.IsIn;
                    t.Visible = !isFirst && !isLast;

                    if (t.Visible)
                        _tangents[i].Location -= posOffset;
                }
            }
            else
            {
                for (int i = 0; i < _tangents.Length; i++)
                {
                    _tangents[i].Visible = false;
                }
            }
        }

        private void UpdateKeyframes()
        {
            if (_points.Count == 0)
            {
                // No keyframes
                _contents.Bounds = Rectangle.Empty;
                return;
            }

            var wasLocked = _mainPanel.IsLayoutLocked;
            _mainPanel.IsLayoutLocked = true;

            // Place keyframes
            Rectangle curveContentAreaBounds = _mainPanel.GetClientArea();
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                var k = _keyframes[p.Index];

                var location = GetKeyframePoint(ref k, p.Component);
                p.Size = KeyframesSize / ViewScale;
                p.Location = new Vector2
                (
                    location.X * UnitsPerSecond - p.Width * 0.5f,
                    location.Y * -UnitsPerSecond - p.Height * 0.5f + curveContentAreaBounds.Height
                );
            }

            // Calculate bounds
            var bounds = _points[0].Bounds;
            for (var i = 1; i < _points.Count; i++)
            {
                bounds = Rectangle.Union(bounds, _points[i].Bounds);
            }

            // Adjust contents bounds to fill the curve area
            if (EnablePanning)
                _contents.Bounds = bounds;

            // Offset the keyframes (parent container changed its location)
            var posOffset = _contents.Location;
            for (var i = 0; i < _points.Count; i++)
            {
                _points[i].Location -= posOffset;
            }

            UpdateTangents();

            _mainPanel.IsLayoutLocked = wasLocked;
            _mainPanel.PerformLayout();
        }

        private int SelectionCount
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _points.Count; i++)
                    if (_points[i].IsSelected)
                        result++;
                return result;
            }
        }

        private void ClearSelection()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i].Deselect();
            }
        }

        private void SelectAll()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i].Select();
            }
        }

        /// <summary>
        /// Gets the keyframe point (in keyframes space).
        /// </summary>
        /// <param name="k">The keyframe.</param>
        /// <param name="component">The keyframe value component index.</param>
        /// <returns>The point in time/value space.</returns>
        private Vector2 GetKeyframePoint(ref Curve<T>.Keyframe k, int component)
        {
            return new Vector2(k.Time, Accessor.GetCurveValue(ref k.Value, component));
        }

        /// <summary>
        /// Converts the input point from curve editor control space into the keyframes time/value coordinates.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="point">The curve contents area bounds.</param>
        /// <returns>The result.</returns>
        private Vector2 PointToKeyframes(Vector2 point, ref Rectangle curveContentAreaBounds)
        {
            // Curve Editor -> Main Panel
            point = _mainPanel.PointFromParent(point);

            // Main Panel -> Contents
            point = _contents.PointFromParent(point);

            // Contents -> Keyframes
            return new Vector2(
                (point.X + _contents.Location.X) / UnitsPerSecond,
                (point.Y + _contents.Location.Y - curveContentAreaBounds.Height) / -UnitsPerSecond
            );
        }

        /// <summary>
        /// Converts the input point from the keyframes time/value coordinates into the curve editor control space.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="point">The curve contents area bounds.</param>
        /// <returns>The result.</returns>
        private Vector2 PointFromKeyframes(Vector2 point, ref Rectangle curveContentAreaBounds)
        {
            // Keyframes -> Contents
            point = new Vector2(
                point.X * UnitsPerSecond - _contents.Location.X,
                point.Y * -UnitsPerSecond + curveContentAreaBounds.Height - _contents.Location.Y
            );

            // Contents -> Main Panel
            point = _contents.PointToParent(point);

            // Main Panel -> Curve Editor
            return _mainPanel.PointToParent(point);
        }

        private void DrawAxis(ref Vector2 axis, ref Rectangle viewRect, float min, float max, float pixelRange)
        {
            int minDistanceBetweenTicks = 20;
            int maxDistanceBetweenTicks = 60;
            var range = max - min;

            // Find the strength for each modulo number tick marker
            int smallestTick = 0;
            int biggestTick = TickSteps.Length - 1;
            for (int i = TickSteps.Length - 1; i >= 0; i--)
            {
                // Calculate how far apart these modulo tick steps are spaced
                float tickSpacing = TickSteps[i] * pixelRange / range;

                // Calculate the strength of the tick markers based on the spacing
                _tickStrengths[i] = Mathf.Saturate((tickSpacing - minDistanceBetweenTicks) / (maxDistanceBetweenTicks - minDistanceBetweenTicks));

                // Beyond threshold the ticks don't get any bigger or fatter
                if (_tickStrengths[i] >= 1)
                    biggestTick = i;

                // Do not show small tick markers
                if (tickSpacing <= minDistanceBetweenTicks)
                {
                    smallestTick = i;
                    break;
                }
            }

            // Draw all tick levels
            int tickLevels = biggestTick - smallestTick + 1;
            for (int level = 0; level < tickLevels; level++)
            {
                float strength = _tickStrengths[smallestTick + level];
                if (strength <= Mathf.Epsilon)
                    continue;

                // Draw all ticks
                int l = Mathf.Clamp(smallestTick + level, 0, TickSteps.Length - 1);
                int startTick = Mathf.FloorToInt(min / TickSteps[l]);
                int endTick = Mathf.CeilToInt(max / TickSteps[l]);
                for (int i = startTick; i <= endTick; i++)
                {
                    if (l < biggestTick && (i % Mathf.RoundToInt(TickSteps[l + 1] / TickSteps[l]) == 0))
                        continue;

                    var tick = i * TickSteps[l];
                    var p = PointFromKeyframes(axis * tick, ref viewRect);

                    // Draw line
                    var lineRect = new Rectangle
                    (
                        viewRect.Location + (p - 0.5f) * axis,
                        Vector2.Lerp(viewRect.Size, Vector2.One, axis)
                    );
                    Render2D.FillRectangle(lineRect, _linesColor.AlphaMultiplied(strength));

                    // Draw label
                    string label = tick.ToString();
                    var labelRect = new Rectangle
                    (
                        viewRect.X + 4.0f + (p.X * axis.X),
                        viewRect.Y - LabelsSize + (p.Y * axis.Y) + (viewRect.Size.Y * axis.X),
                        50,
                        LabelsSize
                    );
                    Render2D.DrawText(_labelsFont, label, labelRect, _labelsColor.AlphaMultiplied(strength), TextAlignment.Near, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, 0.7f);
                }
            }
        }

        private void DrawLine(Curve<T>.Keyframe startK, Curve<T>.Keyframe endK, int component, ref Rectangle viewRect)
        {
            var start = GetKeyframePoint(ref startK, component);
            var end = GetKeyframePoint(ref endK, component);

            var p1 = PointFromKeyframes(start, ref viewRect);
            var p2 = PointFromKeyframes(end, ref viewRect);

            var color = Colors[component].RGBMultiplied(0.6f);
            Render2D.DrawLine(p1, p2, color, 1.6f);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Hack to refresh UI after keyframes edit
            if (_refreshAfterEdit)
            {
                _refreshAfterEdit = false;
                UpdateKeyframes();
            }

            var style = Style.Current;
            var rect = new Rectangle(Vector2.Zero, Size);
            var viewRect = _mainPanel.GetClientArea();

            // Draw background
            Render2D.FillRectangle(rect, _contentsColor);

            // Draw time and values axes
            {
                var upperLeft = PointToKeyframes(viewRect.Location, ref viewRect);
                var bottomRight = PointToKeyframes(viewRect.Size, ref viewRect);

                var min = Vector2.Min(upperLeft, bottomRight);
                var max = Vector2.Max(upperLeft, bottomRight);
                var pixelRange = (max - min) * ViewScale * UnitsPerSecond;

                Render2D.PushClip(ref viewRect);

                var axisX = Vector2.UnitX;
                var axisY = Vector2.UnitY;
                DrawAxis(ref axisX, ref viewRect, min.X, max.X, pixelRange.X);
                DrawAxis(ref axisY, ref viewRect, min.Y, max.Y, pixelRange.Y);

                Render2D.PopClip();
            }

            // Draw curve
            {
                Render2D.PushClip(ref rect);

                var components = Accessor.GetCurveComponents();
                for (int component = 0; component < components; component++)
                {
                    if (ShowStartEndLines)
                    {
                        var start = new Curve<T>.Keyframe
                        {
                            Value = DefaultValue,
                            Time = -10000000.0f,
                        };
                        var end = new Curve<T>.Keyframe
                        {
                            Value = DefaultValue,
                            Time = 10000000.0f,
                        };

                        if (_keyframes.Count == 0)
                        {
                            DrawLine(start, end, component, ref viewRect);
                        }
                        else
                        {
                            DrawLine(start, _keyframes[0], component, ref viewRect);
                            DrawLine(_keyframes[_keyframes.Count - 1], end, component, ref viewRect);
                        }
                    }

                    var color = Colors[component];
                    for (int i = 1; i < _keyframes.Count; i++)
                    {
                        var startK = _keyframes[i - 1];
                        var endK = _keyframes[i];

                        var start = GetKeyframePoint(ref startK, component);
                        var end = GetKeyframePoint(ref endK, component);

                        var startTangent = Accessor.GetCurveValue(ref startK.TangentOut, component);
                        var endTangent = Accessor.GetCurveValue(ref endK.TangentIn, component);

                        var offset = (end.X - start.X) * 0.5f;

                        var p1 = PointFromKeyframes(start, ref viewRect);
                        var p2 = PointFromKeyframes(start + new Vector2(offset, startTangent * offset), ref viewRect);
                        var p3 = PointFromKeyframes(end - new Vector2(offset, endTangent * offset), ref viewRect);
                        var p4 = PointFromKeyframes(end, ref viewRect);

                        Render2D.DrawBezier(p1, p2, p3, p4, color);
                    }
                }

                Render2D.PopClip();
            }

            // Draw selection rectangle
            if (_contents._leftMouseDown && !_contents._isMovingSelection && !_contents._isMovingTangent)
            {
                var selectionRect = Rectangle.FromPoints
                (
                    _mainPanel.PointToParent(_contents.PointToParent(_contents._leftMouseDownPos)),
                    _mainPanel.PointToParent(_contents.PointToParent(_contents._mousePos))
                );
                Render2D.FillRectangle(selectionRect, Color.Orange * 0.4f);
                Render2D.DrawRectangle(selectionRect, Color.Orange);
            }

            base.Draw();

            // Draw border
            if (ContainsFocus)
            {
                Render2D.DrawRectangle(rect, style.BackgroundSelected);
            }
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (base.OnKeyDown(key))
                return true;

            if (key == Keys.Delete)
            {
                RemoveKeyframes();
                return true;
            }

            if (Root.GetKey(Keys.Control))
            {
                switch (key)
                {
                case Keys.A:
                    SelectAll();
                    UpdateTangents();
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            // Clear references to the controls
            _mainPanel = null;
            _contents = null;
            _keyframesEditor = null;

            // Cleanup
            _points.Clear();
            _keyframes.Clear();
            _labelsFont = null;

            base.Dispose();
        }
    }
}
