// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI.Input;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Visject Surface input box element.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.Elements.Box" />
    public class InputBox : Box
    {
        private Control _defaultValueEditor;

        /// <inheritdoc />
        public InputBox(SurfaceNode parentNode, NodeElementArchetype archetype)
        : base(parentNode, archetype, archetype.Position)
        {
        }

        /// <inheritdoc />
        public override bool IsOutput => false;

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Box
            DrawBox();

            // Draw text
            var style = Style.Current;
            var rect = new Rectangle(Width + 4, 0, 1410, Height);
            Render2D.DrawText(style.FontSmall, Archetype.Text, rect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center);
        }

        /// <inheritdoc />
        protected override void OnCurrentTypeChanged()
        {
            base.OnCurrentTypeChanged();

            if (_defaultValueEditor != null)
            {
                bool isValid = false;
                switch (CurrentType)
                {
                case ConnectionType.Bool:
                    isValid = _defaultValueEditor is CheckBox;
                    break;
                case ConnectionType.Integer:
                case ConnectionType.UnsignedInteger:
                    isValid = _defaultValueEditor is IntValueBox;
                    break;
                case ConnectionType.Float:
                    isValid = _defaultValueEditor is FloatValue;
                    break;
                case ConnectionType.Vector2:
                {
                    isValid = _defaultValueEditor is ContainerControl vec2
                              && vec2.ChildrenCount == 2
                              && vec2.Children[0] is FloatValue
                              && vec2.Children[1] is FloatValue;
                    break;
                }
                case ConnectionType.Vector3:
                {
                    isValid = _defaultValueEditor is ContainerControl vec3
                              && vec3.ChildrenCount == 3
                              && vec3.Children[0] is FloatValue
                              && vec3.Children[1] is FloatValue
                              && vec3.Children[2] is FloatValue;
                    break;
                }
                case ConnectionType.Vector4:
                {
                    isValid = _defaultValueEditor is ContainerControl vec4
                              && vec4.ChildrenCount == 4
                              && vec4.Children[0] is FloatValue
                              && vec4.Children[1] is FloatValue
                              && vec4.Children[2] is FloatValue
                              && vec4.Children[3] is FloatValue;
                    break;
                }
                }

                if (!isValid)
                {
                    _defaultValueEditor.Dispose();
                    _defaultValueEditor = null;
                }
            }

            if (Connections.Count == 0)
            {
                CreateDefaultEditor();
            }
        }

        /// <inheritdoc />
        public override void OnConnectionsChanged()
        {
            bool showEditor = Connections.Count == 0 && Archetype.ValueIndex != -1;
            if (showEditor)
            {
                CreateDefaultEditor();
            }

            if (_defaultValueEditor != null)
            {
                _defaultValueEditor.Enabled = showEditor;
                _defaultValueEditor.Visible = showEditor;
            }
        }

        /// <summary>
        /// Creates the default value editor control.
        /// </summary>
        private void CreateDefaultEditor()
        {
            if (_defaultValueEditor != null || Archetype.ValueIndex == -1)
                return;

            var style = Style.Current;
            float x = X + Width + 8 + style.FontSmall.MeasureText(Archetype.Text).X;
            float y = Y;
            float height = Height;

            switch (CurrentType)
            {
            case ConnectionType.Bool:
            {
                bool value = BoolValue.Get(ParentNode, Archetype);
                var control = new CheckBox(x, y, value, height)
                {
                    Parent = Parent
                };
                control.StateChanged += OnCheckBoxChanged;
                _defaultValueEditor = control;
                break;
            }
            case ConnectionType.Integer:
            case ConnectionType.UnsignedInteger:
            {
                int value = IntegerValue.Get(ParentNode, Archetype);
                var control = new IntValueBox(value, x, y, 40, CurrentType == ConnectionType.UnsignedInteger ? 0 : int.MinValue, int.MaxValue, 0.01f)
                {
                    Height = height,
                    Parent = Parent
                };
                control.ValueChanged += OnIntValueBoxChanged;
                _defaultValueEditor = control;
                break;
            }
            case ConnectionType.Float:
            {
                float value = FloatValue.Get(ParentNode, Archetype);
                var control = new FloatValueBox(value, x, y, 40, float.MinValue, float.MaxValue, 0.01f)
                {
                    Height = height,
                    Parent = Parent
                };
                control.ValueChanged += OnFloatValueBoxChanged;
                _defaultValueEditor = control;
                break;
            }
            case ConnectionType.Vector2:
            {
                Vector2 value = Vector2.Zero;
                var v = ParentNode.Values[Archetype.ValueIndex];
                if (v is Vector2 vec2)
                {
                    value = vec2;
                }
                else if (v is Vector3 vec3)
                {
                    value = new Vector2(vec3);
                }
                else if (v is Vector4 vec4)
                {
                    value = new Vector2(vec4);
                }
                else if (v is Color col)
                {
                    value = new Vector2(col.R, col.G);
                }
                else if (v is float f)
                {
                    value = new Vector2(f);
                }
                else if (v is int i)
                {
                    value = new Vector2(i);
                }

                var control = new ContainerControl(x, y, 22 * 2 - 2, height)
                {
                    ClipChildren = false,
                    CanFocus = false,
                    Parent = Parent
                };
                var floatX = new FloatValueBox(value.X, 0, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatX.ValueChanged += OnVector2ValueChanged;
                var floatY = new FloatValueBox(value.Y, 22, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatY.ValueChanged += OnVector2ValueChanged;
                _defaultValueEditor = control;
                break;
            }
            case ConnectionType.Vector3:
            {
                Vector3 value = Vector3.Zero;
                var v = ParentNode.Values[Archetype.ValueIndex];
                if (v is Vector2 vec2)
                {
                    value = new Vector3(vec2, 0.0f);
                }
                else if (v is Vector3 vec3)
                {
                    value = vec3;
                }
                else if (v is Vector4 vec4)
                {
                    value = new Vector3(vec4);
                }
                else if (v is Color col)
                {
                    value = col;
                }
                else if (v is float f)
                {
                    value = new Vector3(f);
                }
                else if (v is int i)
                {
                    value = new Vector3(i);
                }

                var control = new ContainerControl(x, y, 22 * 3 - 2, height)
                {
                    ClipChildren = false,
                    CanFocus = false,
                    Parent = Parent
                };
                var floatX = new FloatValueBox(value.X, 0, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatX.ValueChanged += OnVector3ValueChanged;
                var floatY = new FloatValueBox(value.Y, 22, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatY.ValueChanged += OnVector3ValueChanged;
                var floatZ = new FloatValueBox(value.Z, 44, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatZ.ValueChanged += OnVector3ValueChanged;
                _defaultValueEditor = control;
                break;
            }
            case ConnectionType.Vector4:
            {
                Vector4 value = Vector4.Zero;
                var v = ParentNode.Values[Archetype.ValueIndex];
                if (v is Vector2 vec2)
                {
                    value = new Vector4(vec2, 0.0f, 0.0f);
                }
                else if (v is Vector3 vec3)
                {
                    value = new Vector4(vec3, 0.0f);
                }
                else if (v is Vector4 vec4)
                {
                    value = vec4;
                }
                else if (v is Color col)
                {
                    value = col;
                }
                else if (v is float f)
                {
                    value = new Vector4(f);
                }
                else if (v is int i)
                {
                    value = new Vector4(i);
                }

                var control = new ContainerControl(x, y, 22 * 4 - 2, height)
                {
                    ClipChildren = false,
                    CanFocus = false,
                    Parent = Parent
                };
                var floatX = new FloatValueBox(value.X, 0, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatX.ValueChanged += OnVector4ValueChanged;
                var floatY = new FloatValueBox(value.Y, 22, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatY.ValueChanged += OnVector4ValueChanged;
                var floatZ = new FloatValueBox(value.Z, 44, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatZ.ValueChanged += OnVector4ValueChanged;
                var floatW = new FloatValueBox(value.W, 66, 0, 20, float.MinValue, float.MaxValue, 0.0f)
                {
                    Height = height,
                    Parent = control
                };
                floatW.ValueChanged += OnVector4ValueChanged;
                _defaultValueEditor = control;
                break;
            }
            }
        }

        private void OnCheckBoxChanged(CheckBox checkBox)
        {
            ParentNode.SetValue(Archetype.ValueIndex, checkBox.Checked);
        }

        private void OnIntValueBoxChanged()
        {
            IntegerValue.Set(ParentNode, Archetype, ((IntValueBox)_defaultValueEditor).Value);
        }

        private void OnFloatValueBoxChanged()
        {
            FloatValue.Set(ParentNode, Archetype, ((FloatValueBox)_defaultValueEditor).Value);
        }

        private void OnVector2ValueChanged()
        {
            var x = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[0]).Value;
            var y = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[1]).Value;
            ParentNode.SetValue(Archetype.ValueIndex, new Vector2(x, y));
        }

        private void OnVector3ValueChanged()
        {
            var x = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[0]).Value;
            var y = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[1]).Value;
            var z = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[2]).Value;
            ParentNode.SetValue(Archetype.ValueIndex, new Vector3(x, y, z));
        }

        private void OnVector4ValueChanged()
        {
            var x = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[0]).Value;
            var y = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[1]).Value;
            var z = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[2]).Value;
            var w = ((FloatValueBox)((ContainerControl)_defaultValueEditor).Children[3]).Value;
            ParentNode.SetValue(Archetype.ValueIndex, new Vector4(x, y, z, w));
        }
    }
}
