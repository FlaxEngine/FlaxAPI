////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit reference to the <see cref="FlaxEngine.Object"/>.
    /// </summary>
    [CustomEditor(typeof(Object)), DefaultEditor]
    public sealed class FlaxObjectRefEditor : CustomEditor
    {
        /// <summary>
        /// A custom control type used to pick reference to <see cref="Object"/>.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.Control" />
        private class ReferencePickerControl : Control
        {
            private Type _type;
            private Object _value;

            private bool _isMosueDown;
            private Vector2 _mosueDownPos;
            private Vector2 _mousePos;

            /// <summary>
            /// Gets or sets the allowed objects type (given type and all sub classes). Must be <see cref="Object"/> type of any subclass.
            /// </summary>
            /// <value>
            /// The allowed objects type.
            /// </value>
            public Type Type
            {
                get => _type;
                set
                {
                    if (value == null || !value.IsSubclassOf(typeof(Object)))
                        throw new ArgumentException();

                    if (_type != value)
                    {
                        _type = value;

                        // Deselect value if it's not valid now
                        if (!IsValid(_value))
                            Value = null;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the selected object value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public Object Value
            {
                get => _value;
                set
                {
                    if (!IsValid(value))
                        throw new ArgumentException("Invalid object type.");

                    if (_value != value)
                    {
                        _value = value;
                        ValueChanged?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Gets or sets the selected object value by identifier.
            /// </summary>
            /// <value>
            /// The selected object value identifier.
            /// </value>
            public Guid ValueID
            {
                get => _value ? _value.ID : Guid.Empty;
                set => Value = Object.Find<Object>(ref value);
            }

            /// <summary>
            /// Occurs when value gets changed.
            /// </summary>
            public event Action ValueChanged;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReferencePickerControl"/> class.
            /// </summary>
            public ReferencePickerControl()
                : base(true, 0, 0, 50, 16)
            {
                _type = typeof(Object);
            }

            private bool IsValid(object obj)
            {
                // ReSharper disable once UseMethodIsInstanceOfType
                return obj == null || _type.IsAssignableFrom(obj.GetType());
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                // Cache data
                var style = Style.Current;
                bool isSelected = _value != null;
                var frameRect = new Rectangle(0, 0, Width - (isSelected ? 16 : 0), 16);
                var nameRect = new Rectangle(2, 1, Width - (isSelected ? 20 : 4), 14);
                var buttonRect = new Rectangle(nameRect.Right + 3, 1, 14, 14);

                // Draw frame
                Render2D.DrawRectangle(frameRect, style.BorderNormal);

                // Check if has item selected
                if (isSelected)
                {
                    // Draw name
                    Render2D.PushClip(nameRect);
                    Render2D.DrawText(style.FontMedium, _value.ToString(), nameRect, style.Foreground, TextAlignment.Near, TextAlignment.Center);
                    Render2D.PopClip();

                    // Draw button
                    Render2D.DrawSprite(style.Cross, buttonRect, new Color(buttonRect.Contains(_mousePos) ? 1.0f : 0.7f));
                }
                else
                {
                    // Draw info
                    Render2D.DrawText(style.FontMedium, "-", nameRect, Color.OrangeRed, TextAlignment.Near, TextAlignment.Center);
                }
                
                // Check if drag is over
                /*if (IsDragOver && _dragOverActor.HasValidDrag())
                    Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), style.BackgroundSelected * 0.4f, true);*/
            }
        }

        private CustomElement<ReferencePickerControl> element;

        /// <inheritdoc />
        public override bool IsInline => true;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (Values == null)
                return;

            if (!HasDiffrentTypes)
            {
                element = layout.Custom<ReferencePickerControl>();
                element.CustomControl.Type = Values.Type;
                element.CustomControl.ValueChanged += () => SetValue(element.CustomControl.Value);
            }
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (!HasDiffrentValues)
            {
                element.CustomControl.Value = Values[0] as Object;
            }
        }
    }
}
