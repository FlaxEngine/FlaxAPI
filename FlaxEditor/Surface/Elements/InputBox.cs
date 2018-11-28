// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
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
                    isValid = _defaultValueEditor is IntValueBox;
                    break;

                case ConnectionType.Float:
                    isValid = _defaultValueEditor is FloatValue;
                    break;
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
            {
                int value = IntegerValue.Get(ParentNode, Archetype);
                var control = new IntValueBox(value, x, y, 40, int.MinValue, int.MaxValue, 0.01f)
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
            case ConnectionType.Vector3:
            case ConnectionType.Vector4:
            case ConnectionType.Vector:
            {
                float value = FloatValue.Get(ParentNode, Archetype);
                var control = new FloatValueBox(value, x, y, 40, float.MinValue, float.MaxValue, 0.01f)
                {
                    Height = height,
                    Parent = Parent
                };
                control.ValueChanged += OnVectorValueBoxChanged;
                _defaultValueEditor = control;
                break;
            }
            }
        }

        private void OnCheckBoxChanged(CheckBox checkBox)
        {
            ParentNode.SetValue(Archetype.ValueIndex, checkBox.Checked);
        }

        private void OnVectorValueBoxChanged()
        {
            FloatValue.SetAllValues(ParentNode, Archetype, ((FloatValueBox)_defaultValueEditor).Value);
        }

        private void OnFloatValueBoxChanged()
        {
            FloatValue.Set(ParentNode, Archetype, ((FloatValueBox)_defaultValueEditor).Value);
        }

        private void OnIntValueBoxChanged()
        {
            IntegerValue.Set(ParentNode, Archetype, ((IntValueBox)_defaultValueEditor).Value);
        }
    }
}
