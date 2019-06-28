// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
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
                public override void Dispose()
                {
                    Node = null;

                    base.Dispose();
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
                        Node.Select(this);
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

                _addButton = new Button(_gradient.Right - 20.0f, controlsLevel, 20, 20)
                {
                    Text = "+",
                    TooltipText = "Add gradient stop",
                    Parent = this
                };
                _addButton.Clicked += OnAddButtonClicked;
                _removeButton = new Button(_addButton.Left - 24.0f, _addButton.Y, 20, 20)
                {
                    Text = "-",
                    TooltipText = "Remove selected gradient stop",
                    Parent = this
                };
                _removeButton.Clicked += OnRemoveButtonClicked;

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

                UpdateStops();
            }

            private void OnAddButtonClicked()
            {
                var count = (int)Values[0];

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
                    time = ((float)Values[left] + (float)Values[right]) / 2;
                    color = Color.Lerp((Color)Values[left + 1], (Color)Values[right + 1], time);

                    // Shift higher stops to have empty place at stop 1
                    Array.Copy(Values, 3, Values, 5, count * 2 - 2);
                }

                // Insert
                Values[1 + index * 2] = time;
                Values[1 + index * 2 + 1] = color;

                Values[0] = count + 1;
                OnValuesChanged();
            }

            private void OnRemoveButtonClicked()
            {
                var index = _stops.IndexOf(_selected);
                _selected = null;

                var count = (int)Values[0];
                if (count > 0)
                    Array.Copy(Values, 1 + index * 2 + 2, Values, 1 + index * 2, (count - index - 1) * 2);
                Values[0] = count - 1;
                OnValuesChanged();
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
            public override void Dispose()
            {
                _gradient = null;
                _stops.Clear();

                base.Dispose();
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
                        CanFocus = false,
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

                // Update selected stop
                if (_selected != null)
                {
                    var index = _stops.IndexOf(_selected);
                    _timeValue.Value = (float)Values[index * 2 + 1];
                    _colorValue.Value = (Color)Values[index * 2 + 1 + 1];

                    _labelValue.Visible = true;
                    _timeValue.Visible = true;
                    _colorValue.Visible = true;
                }
                else
                {
                    _labelValue.Visible = false;
                    _timeValue.Visible = false;
                    _colorValue.Visible = false;
                }

                // Update buttons
                _addButton.Enabled = count < MaxStops;
                _removeButton.Enabled = count > 0 && _selected != null;
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
                    NodeElementArchetype.Factory.Vector_X(0, 1 * Surface.Constants.LayoutOffsetY * 3 + 5, 0),
                    NodeElementArchetype.Factory.Vector_Y(0, 2 * Surface.Constants.LayoutOffsetY * 3 + 5, 0),
                    NodeElementArchetype.Factory.Vector_Z(0, 3 * Surface.Constants.LayoutOffsetY * 3 + 5, 0)
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
                Flags = NodeFlags.ParticleEmitterGraph,
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
        };
    }
}
