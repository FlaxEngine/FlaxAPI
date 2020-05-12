// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Input;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Tools group.
    /// </summary>
    public static class Tools
    {
        private class ColorGradientNode : SurfaceNode
        {
            private class Gradient : ContainerControl
            {
                public ColorGradientNode Node;

                /// <inheritdoc />
                public override void Draw()
                {
                    base.Draw();

                    var style = Style.Current;
                    var bounds = new Rectangle(Vector2.Zero, Size);
                    var count = (int)Node.Values[0];
                    if (count == 0)
                    {
                        Render2D.FillRectangle(bounds, Color.Black);
                    }
                    else if (count == 1)
                    {
                        Render2D.FillRectangle(bounds, (Color)Node.Values[2]);
                    }
                    else
                    {
                        var prevTime = (float)Node.Values[1];
                        var prevColor = (Color)Node.Values[2];
                        var width = Width;
                        var height = Height;

                        if (prevTime > 0.0f)
                        {
                            Render2D.FillRectangle(new Rectangle(Vector2.Zero, prevTime * width, height), prevColor);
                        }

                        for (int i = 1; i < count; i++)
                        {
                            var curTime = (float)Node.Values[i * 2 + 1];
                            var curColor = (Color)Node.Values[i * 2 + 2];

                            Render2D.FillRectangle(new Rectangle(prevTime * width, 0, (curTime - prevTime) * width, height), prevColor, curColor, curColor, prevColor);

                            prevTime = curTime;
                            prevColor = curColor;
                        }

                        if (prevTime < 1.0f)
                        {
                            Render2D.FillRectangle(new Rectangle(prevTime * width, 0, (1.0f - prevTime) * width, height), prevColor);
                        }
                    }
                    Render2D.DrawRectangle(bounds, IsMouseOver ? style.BackgroundHighlighted : style.Background);
                }

                /// <inheritdoc />
                public override void OnDestroy()
                {
                    Node = null;

                    base.OnDestroy();
                }
            }

            private class GradientStop : Control
            {
                private bool _isMoving;
                private Vector2 _startMovePos;

                public ColorGradientNode Node;
                public Color Color;

                /// <inheritdoc />
                public override void Draw()
                {
                    base.Draw();

                    var isSelected = Node._selected == this;
                    var arrowRect = new Rectangle(0, 0, 16.0f, 16.0f);
                    var arrowTransform = Matrix3x3.Translation2D(new Vector2(-16.0f, -8.0f)) * Matrix3x3.RotationZ(-Mathf.PiOverTwo) * Matrix3x3.Translation2D(new Vector2(8.0f, 0));
                    var color = Color;
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
                public override bool OnMouseDown(Vector2 location, MouseButton button)
                {
                    if (button == MouseButton.Left)
                    {
                        Node.Select(this);
                        _isMoving = true;
                        _startMovePos = location;
                        StartMouseCapture();
                        return true;
                    }

                    return base.OnMouseDown(location, button);
                }

                /// <inheritdoc />
                public override bool OnMouseUp(Vector2 location, MouseButton button)
                {
                    if (button == MouseButton.Left && _isMoving)
                    {
                        _isMoving = false;
                        EndMouseCapture();
                    }

                    return base.OnMouseUp(location, button);
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
                        var index = Node._stops.IndexOf(this);
                        var time = (PointToParent(location).X - Node._gradient.BottomLeft.X) / Node._gradient.Width;
                        Node.SetStopTime(index, time);
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

            private Gradient _gradient;
            private readonly List<GradientStop> _stops = new List<GradientStop>();
            private GradientStop _selected;
            private Button _addButton;
            private Button _removeButton;
            private Label _labelValue;
            private FloatValueBox _timeValue;
            private ColorValueBox _colorValue;
            private const int MaxStops = 8;

            /// <inheritdoc />
            public ColorGradientNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                var upperLeft = GetBox(0).BottomLeft;
                var upperRight = GetBox(1).BottomRight;
                float gradientMargin = 20.0f;

                _gradient = new Gradient
                {
                    Node = this,
                    Bounds = new Rectangle(upperLeft + new Vector2(gradientMargin, 10.0f), upperRight.X - upperLeft.X - gradientMargin * 2.0f, 40.0f),
                    Parent = this,
                };

                var controlsLevel = _gradient.Bottom + 4.0f + 20.0f + 40.0f;

                _labelValue = new Label(_gradient.Left, controlsLevel - 20.0f, 70.0f, 20.0f)
                {
                    Text = "Selected:",
                    VerticalAlignment = TextAlignment.Center,
                    HorizontalAlignment = TextAlignment.Near,
                    Parent = this
                };

                _timeValue = new FloatValueBox(0.0f, _gradient.Left, controlsLevel, 100.0f, 0.0f, 1.0f, 0.001f)
                {
                    Parent = this
                };
                _timeValue.ValueChanged += OnTimeValueChanged;

                _colorValue = new ColorValueBox(Color.Black, _timeValue.Right + 4.0f, controlsLevel)
                {
                    Height = _timeValue.Height,
                    Parent = this
                };
                _colorValue.ValueChanged += OnColorValueChanged;

                _removeButton = new Button(_colorValue.Right + 4.0f, controlsLevel, 20, 20)
                {
                    Text = "-",
                    TooltipText = "Remove selected gradient stop",
                    Parent = this
                };
                _removeButton.Clicked += OnRemoveButtonClicked;

                _addButton = new Button(_gradient.Right - 20.0f, controlsLevel, 20, 20)
                {
                    Text = "+",
                    TooltipText = "Add gradient stop",
                    Parent = this
                };
                _addButton.Clicked += OnAddButtonClicked;

                UpdateStops();
            }

            private void OnAddButtonClicked()
            {
                var values = (object[])Values.Clone();
                var count = (int)values[0];

                var time = 0.0f;
                var color = Color.Black;
                var index = 0;
                if (count == 1)
                {
                    index = 1;
                    time = 1.0f;
                    color = Color.White;
                }
                else if (count > 1)
                {
                    index = 1;
                    var left = 1;
                    var right = 3;
                    time = ((float)values[left] + (float)values[right]) / 2;
                    color = Color.Lerp((Color)values[left + 1], (Color)values[right + 1], time);

                    // Shift higher stops to have empty place at stop 1
                    Array.Copy(values, 3, values, 5, count * 2 - 2);
                }

                // Insert
                values[1 + index * 2] = time;
                values[1 + index * 2 + 1] = color;

                values[0] = count + 1;

                SetValues(values);
            }

            private void OnRemoveButtonClicked()
            {
                var values = (object[])Values.Clone();
                var index = _stops.IndexOf(_selected);
                _selected = null;

                var count = (int)values[0];
                if (count > 0)
                    Array.Copy(values, 1 + index * 2 + 2, values, 1 + index * 2, (count - index - 1) * 2);
                values[0] = count - 1;

                SetValues(values);
            }

            private void OnTimeValueChanged()
            {
                var index = _stops.IndexOf(_selected);
                SetStopTime(index, _timeValue.Value);
            }

            private void OnColorValueChanged()
            {
                var index = _stops.IndexOf(_selected);
                SetStopColor(index, _colorValue.Value);
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                UpdateStops();
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _gradient = null;
                _stops.Clear();

                base.OnDestroy();
            }

            private void Select(GradientStop stop)
            {
                _selected = stop;
                UpdateStops();
            }

            private void SetStopTime(int index, float time)
            {
                time = Mathf.Saturate(time);
                if (index != 0)
                {
                    time = Mathf.Max(time, (float)Values[1 + index * 2 - 2]);
                }
                if (index != _stops.Count - 1)
                {
                    time = Mathf.Min(time, (float)Values[1 + index * 2 + 2]);
                }
                SetValue(1 + index * 2, time);
            }

            private void SetStopColor(int index, Color color)
            {
                SetValue(1 + index * 2 + 1, color);
            }

            private void UpdateStops()
            {
                var count = (int)Values[0];

                // Remove unused stops
                while (_stops.Count > count)
                {
                    var last = _stops.Count - 1;
                    _stops[last].Dispose();
                    _stops.RemoveAt(last);
                }

                // Add missing stops
                while (_stops.Count < count)
                {
                    var stop = new GradientStop
                    {
                        AutoFocus = false,
                        Node = this,
                        Size = new Vector2(16.0f, 16.0f),
                        Parent = this,
                    };
                    _stops.Add(stop);
                }

                // Update stops
                for (var i = 0; i < _stops.Count; i++)
                {
                    var stop = _stops[i];
                    var time = (float)Values[i * 2 + 1];
                    stop.Location = _gradient.BottomLeft + new Vector2(time * _gradient.Width - stop.Width * 0.5f, 0.0f);
                    stop.Color = (Color)Values[i * 2 + 2];
                    stop.TooltipText = stop.Color + " at " + time;
                }

                // Update UI
                if (_selected != null)
                {
                    var index = _stops.IndexOf(_selected);
                    _timeValue.Value = (float)Values[index * 2 + 1];
                    _colorValue.Value = (Color)Values[index * 2 + 1 + 1];

                    _labelValue.Visible = true;
                    _timeValue.Visible = true;
                    _colorValue.Visible = true;
                    _removeButton.Visible = true;
                }
                else
                {
                    _labelValue.Visible = false;
                    _timeValue.Visible = false;
                    _colorValue.Visible = false;
                    _removeButton.Visible = false;
                }
                _addButton.Enabled = count < MaxStops;
            }
        }

        private class CurveNode<T> : SurfaceNode where T : struct
        {
            private CurveEditor<T> _curve;
            private bool _isSavingCurve;

            public static NodeArchetype GetArchetype(ushort typeId, string title, ConnectionType valueType, T zero, T one)
            {
                return new NodeArchetype
                {
                    TypeID = typeId,
                    Title = title,
                    Create = (id, context, arch, groupArch) => new CurveNode<T>(id, context, arch, groupArch),
                    Description = "An animation spline represented by a set of keyframes, each representing an endpoint of a Bezier curve.",
                    Flags = NodeFlags.AllGraphs,
                    Size = new Vector2(400, 180.0f),
                    DefaultValues = new object[]
                    {
                        // Keyframes count
                        2,

                        // Key 0
                        0.0f, // Time
                        zero, // Value
                        zero, // Tangent In
                        zero, // Tangent Out

                        // Key 1
                        1.0f, // Time
                        one, // Value
                        zero, // Tangent In
                        zero, // Tangent Out

                        // Empty keys zero-6
                        0.0f, zero, zero, zero,
                        0.0f, zero, zero, zero,
                        0.0f, zero, zero, zero,
                        0.0f, zero, zero, zero,
                        0.0f, zero, zero, zero,
                    },
                    Elements = new[]
                    {
                        NodeElementArchetype.Factory.Input(0, "Time", true, ConnectionType.Float, 0),
                        NodeElementArchetype.Factory.Output(0, "Value", valueType, 1),
                    }
                };
            }

            /// <inheritdoc />
            public CurveNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <inheritdoc />
            public override void OnLoaded()
            {
                base.OnLoaded();

                var upperLeft = GetBox(0).BottomLeft;
                var upperRight = GetBox(1).BottomRight;
                float curveMargin = 20.0f;

                _curve = new CurveEditor<T>
                {
                    MaxKeyframes = 7,
                    Bounds = new Rectangle(upperLeft + new Vector2(curveMargin, 10.0f), upperRight.X - upperLeft.X - curveMargin * 2.0f, 140.0f),
                    Parent = this
                };
                _curve.Edited += OnCurveEdited;
                _curve.UnlockChildrenRecursive();
                _curve.PerformLayout();

                UpdateCurveKeyframes();
            }

            private void OnCurveEdited()
            {
                if (_isSavingCurve)
                    return;

                _isSavingCurve = true;

                var values = (object[])Values.Clone();
                var keyframes = _curve.Keyframes;
                var count = keyframes.Count;
                values[0] = count;
                for (int i = 0; i < count; i++)
                {
                    var k = keyframes[i];

                    values[i * 4 + 1] = k.Time;
                    values[i * 4 + 2] = k.Value;
                    values[i * 4 + 3] = k.TangentIn;
                    values[i * 4 + 4] = k.TangentOut;
                }
                SetValues(values);

                _isSavingCurve = false;
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                if (!_isSavingCurve)
                {
                    UpdateCurveKeyframes();
                }
            }

            private void UpdateCurveKeyframes()
            {
                var count = (int)Values[0];
                var keyframes = new Curve<T>.Keyframe[count];
                for (int i = 0; i < count; i++)
                {
                    keyframes[i] = new Curve<T>.Keyframe
                    {
                        Time = (float)Values[i * 4 + 1],
                        Value = (T)Values[i * 4 + 2],
                        TangentIn = (T)Values[i * 4 + 3],
                        TangentOut = (T)Values[i * 4 + 4],
                    };
                }
                _curve.SetKeyframes(keyframes);
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _curve = null;

                base.OnDestroy();
            }
        }

        /// <summary>
        /// Surface node type for Gameplay Globals get.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        private class GetGameplayGlobalNode : SurfaceNode
        {
            private ComboBoxElement _combobox;
            private bool _isUpdating;

            /// <inheritdoc />
            public GetGameplayGlobalNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            private void UpdateCombo()
            {
                if (_isUpdating)
                    return;
                _isUpdating = true;

                // Cache combo box
                if (_combobox == null)
                {
                    _combobox = (ComboBoxElement)_children[0];
                    _combobox.SelectedIndexChanged += OnSelectedChanged;
                }

                // Update items
                Type type = null;
                var toSelect = (string)Values[1];
                var asset = FlaxEngine.Content.Load<GameplayGlobals>((Guid)Values[0]);
                _combobox.ClearItems();
                if (asset)
                {
                    var values = asset.DefaultValues;
                    var tooltips = new string[values.Count];
                    var i = 0;
                    foreach (var e in values)
                    {
                        _combobox.AddItem(e.Key);
                        tooltips[i++] = "Type: " + CustomEditorsUtil.GetTypeNameUI(e.Value.GetType()) + ", default value: " + e.Value;
                        if (toSelect == e.Key)
                        {
                            type = e.Value.GetType();
                        }
                    }
                    _combobox.Tooltips = tooltips;
                }

                // Preserve selected item
                _combobox.SelectedItem = toSelect;

                // Update output value type
                var box = GetBox(0);
                if (type == null)
                {
                    box.Enabled = false;
                }
                else
                {
                    box.Enabled = true;
                    box.CurrentType = VisjectSurface.GetValueTypeConnectionType(type);
                }

                _isUpdating = false;
            }

            private void OnSelectedChanged(ComboBox cb)
            {
                if (_isUpdating)
                    return;

                var selected = _combobox.SelectedItem;
                if (selected != (string)Values[1])
                {
                    SetValue(1, selected);
                }
            }

            /// <inheritdoc />
            public override void OnLoaded()
            {
                base.OnLoaded();

                UpdateCombo();
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                UpdateCombo();
            }
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                // [Deprecated]
                TypeID = 1,
                Title = "Fresnel",
                Description = "Calculates a falloff based on the dot product of the surface normal and the direction to the camera",
                Flags = NodeFlags.MaterialGraph | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(140, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Exponent", true, ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Input(1, "Base Reflect Fraction", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Input(2, "Normal", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Title = "Desaturation",
                Description = "Desaturates input, or converts the colors of its input into shades of gray, based a certain percentage",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(140, 130),
                DefaultValues = new object[]
                {
                    new Vector3(0.3f, 0.59f, 0.11f)
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Input", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Input(1, "Scale", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 2 + 5, "Luminance Factors"),
                    NodeElementArchetype.Factory.Vector_X(0, Surface.Constants.LayoutOffsetY * 3 + 5, 0),
                    NodeElementArchetype.Factory.Vector_Y(0, Surface.Constants.LayoutOffsetY * 4 + 5, 0),
                    NodeElementArchetype.Factory.Vector_Z(0, Surface.Constants.LayoutOffsetY * 5 + 5, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 3,
                Title = "Time",
                Description = "Game time constant",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(110, 20),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 4,
                Title = "Fresnel",
                Description = "Calculates a falloff based on the dot product of the surface normal and the direction to the camera",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(200, 60),
                DefaultValues = new object[]
                {
                    5.0f,
                    0.04f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Exponent", true, ConnectionType.Float, 0, 0),
                    NodeElementArchetype.Factory.Input(1, "Base Reflect Fraction", true, ConnectionType.Float, 1, 1),
                    NodeElementArchetype.Factory.Input(2, "Normal", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 5,
                Title = "Time",
                Description = "Game time constant",
                Flags = NodeFlags.AnimGraph,
                Size = new Vector2(140, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Animation Time", ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Output(1, "Delta Seconds", ConnectionType.Float, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 6,
                Title = "Panner",
                Description = "Animates UVs over time",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(170, 80),
                DefaultValues = new object[]
                {
                    false
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UV", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Input(1, "Time", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Input(2, "Speed", true, ConnectionType.Vector2, 2),
                    NodeElementArchetype.Factory.Text(18, Surface.Constants.LayoutOffsetY * 3 + 5, "Fractional Part"),
                    NodeElementArchetype.Factory.Bool(0, Surface.Constants.LayoutOffsetY * 3 + 5, 0),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Vector2, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 7,
                Title = "Linearize Depth",
                Description = "Scene depth buffer texture lookup node",
                Flags = NodeFlags.MaterialGraph | NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(240, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Hardware Depth", true, ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Output(0, "Linear Depth", ConnectionType.Float, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 8,
                Title = "Time",
                Description = "Simulation time and update delta time access",
                Flags = NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(140, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Simulation Time", ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Output(1, "Delta Seconds", ConnectionType.Float, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 9,
                Title = "Transform Position To Screen UV",
                Description = "Transforms world-space position into screen space coordinates (normalized)",
                Flags = NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(300, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "World Space", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(0, "Screen Space UV", ConnectionType.Vector2, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 10,
                Title = "Color Gradient",
                Create = (id, context, arch, groupArch) => new ColorGradientNode(id, context, arch, groupArch),
                Description = "Linear color gradient sampler",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(400, 150.0f),
                DefaultValues = new object[]
                {
                    // Stops count
                    2,

                    // Stop 0
                    0.1f,
                    Color.CornflowerBlue,

                    // Stop 1
                    0.9f,
                    Color.GreenYellow,

                    // Empty stops 2-7
                    0.0f, Color.Black,
                    0.0f, Color.Black,
                    0.0f, Color.Black,
                    0.0f, Color.Black,
                    0.0f, Color.Black,
                    0.0f, Color.Black,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Time", true, ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Vector4, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 11,
                Title = "Comment",
                AlternativeTitles = new[] { "//" },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    if (filterText.StartsWith("//"))
                    {
                        data = new object[]
                        {
                            filterText.Substring(2),
                            new Color(1.0f, 1.0f, 1.0f, 0.2f),
                            new Vector2(400.0f, 400.0f),
                        };
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                },
                Create = (id, context, arch, groupArch) => new SurfaceComment(id, context, arch, groupArch),
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(400.0f, 400.0f),
                DefaultValues = new object[]
                {
                    "Comment", // Title
                    new Color(1.0f, 1.0f, 1.0f, 0.2f), // Color
                    new Vector2(400.0f, 400.0f), // Size
                },
            },
            CurveNode<float>.GetArchetype(12, "Curve", ConnectionType.Float, 0.0f, 1.0f),
            CurveNode<Vector2>.GetArchetype(13, "Curve Vector2", ConnectionType.Vector2, Vector2.Zero, Vector2.One),
            CurveNode<Vector3>.GetArchetype(14, "Curve Vector3", ConnectionType.Vector3, Vector3.Zero, Vector3.One),
            CurveNode<Vector4>.GetArchetype(15, "Curve Vector4", ConnectionType.Vector4, Vector4.Zero, Vector4.One),
            new NodeArchetype
            {
                TypeID = 16,
                Create = (id, context, arch, groupArch) => new GetGameplayGlobalNode(id, context, arch, groupArch),
                Title = "Get Gameplay Global",
                Description = "Gets the Gameplay Global variable value",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(220, 90),
                DefaultValues = new object[]
                {
                    Guid.Empty,
                    string.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.ComboBox(0, 70, 120),
                    NodeElementArchetype.Factory.Asset(0, 0, 0, typeof(GameplayGlobals)),
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Variable, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 17,
                Title = "Platform Switch",
                Description = "Gets the input value based on the runtime-platform type",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(220, 130),
                DefaultType = ConnectionType.Variable,
                IndependentBoxes = new[] { 1, 2, 3, 4, 5, 6 },
                DependentBoxes = new[] { 0 },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Variable, 0),
                    NodeElementArchetype.Factory.Input(0, "Default", true, ConnectionType.Variable, 1),
                    NodeElementArchetype.Factory.Input(1, "Windows", true, ConnectionType.Variable, 2),
                    NodeElementArchetype.Factory.Input(2, "Xbox One", true, ConnectionType.Variable, 3),
                    NodeElementArchetype.Factory.Input(3, "Windows Store", true, ConnectionType.Variable, 4),
                    NodeElementArchetype.Factory.Input(4, "Linux", true, ConnectionType.Variable, 5),
                    NodeElementArchetype.Factory.Input(5, "PlayStation 4", true, ConnectionType.Variable, 6),
                }
            },
        };
    }
}
