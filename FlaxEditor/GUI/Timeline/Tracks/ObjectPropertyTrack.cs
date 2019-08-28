// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating object property (managed object).
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public abstract class ObjectPropertyTrack : Track
    {
        /// <summary>
        /// The property value data size (in bytes).
        /// </summary>
        public int ValueSize;

        /// <summary>
        /// Gets or sets the object property name (just a member name). Does not validate the value on set.
        /// </summary>
        public string PropertyName
        {
            get => Title;
            set => Title = value;
        }

        /// <summary>
        /// The property typename (fullname including namespace but not assembly).
        /// </summary>
        public string PropertyTypeName;

        /// <summary>
        /// Gets or sets the object property. Performs the value validation on set.
        /// </summary>
        public PropertyInfo Property
        {
            get
            {
                if (ParentTrack is ObjectTrack objectTrack)
                {
                    var obj = objectTrack.Object;
                    if (obj)
                    {
                        return obj.GetType().GetProperty(PropertyName, BindingFlags.Public | BindingFlags.Instance);
                    }
                }
                return null;
            }
            set
            {
                if (value != null && ParentTrack is ObjectTrack objectTrack)
                {
                    var obj = objectTrack.Object;
                    if (obj)
                    {
                        if (obj.GetType().GetProperty(value.Name, BindingFlags.Public | BindingFlags.Instance) == null)
                            throw new Exception("Cannot use property " + value + " for object of type " + obj.GetType());
                    }
                }

                if (value != null)
                {
                    var type = value.PropertyType;
                    PropertyName = value.Name;
                    PropertyTypeName = type.FullName;
                    ValueSize = type.IsValueType ? (Marshal.SizeOf(type.IsEnum ? Enum.GetUnderlyingType(type) : type)) : 0;
                }
                else
                {
                    PropertyName = string.Empty;
                    PropertyTypeName = string.Empty;
                    ValueSize = 0;
                }

                OnPropertyChanged(value);
            }
        }

        protected Label _previewValue;
        protected Image _rightKey;
        protected Image _addKey;
        protected Image _leftKey;

        /// <inheritdoc />
        protected ObjectPropertyTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            // Navigation buttons
            const float buttonSize = 14;
            var icons = Editor.Instance.Icons;
            _rightKey = new Image(_muteCheckbox.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                TooltipText = "Sets the time to the next key",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.8f),
                Margin = new Margin(1),
                Brush = new SpriteBrush(icons.ArrowRight32),
                Parent = this
            };
            _addKey = new Image(_rightKey.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                TooltipText = "Adds a new key at the current time",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.8f),
                Margin = new Margin(3),
                Brush = new SpriteBrush(icons.Add48),
                Parent = this
            };
            _leftKey = new Image(_addKey.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                TooltipText = "Sets the time to the previous key",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.8f),
                Margin = new Margin(1),
                Brush = new SpriteBrush(icons.ArrowLeft32),
                Parent = this
            };

            // Value preview
            var previewWidth = 100.0f;
            _previewValue = new Label(_leftKey.Left - previewWidth - 2.0f, 0, previewWidth, TextBox.DefaultHeight)
            {
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                AutoFitTextRange = new Vector2(0.01f, 1.0f),
                AutoFitText = true,
                TextColor = new Color(0.8f),
                Margin = new Margin(1),
                Parent = this
            };
        }

        /// <summary>
        /// Tries the get current value from the assigned object property.
        /// </summary>
        /// <param name="value">The result value. Valid only if methods returns true.</param>
        /// <returns>True if got value, otherwise false.</returns>
        protected virtual bool TryGetValue(out object value)
        {
            if (!string.IsNullOrEmpty(PropertyName) && ParentTrack is ObjectTrack objectTrack)
            {
                var obj = objectTrack.Object;
                if (obj)
                {
                    var p = obj.GetType().GetProperty(PropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (p != null)
                    {
                        try
                        {
                            value = p.GetValue(obj);
                            return true;
                        }
                        catch
                        {
                            value = null;
                            return false;
                        }
                    }
                }
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Gets the value text for UI.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The text.</returns>
        protected string GetValueText(object value)
        {
            if (value == null)
                return string.Empty;

            if (value is Quaternion asQuaternion)
                return asQuaternion.EulerAngles.ToString();

            return value.ToString();
        }

        /// <inheritdoc />
        protected override bool CanDrag => false;

        /// <inheritdoc />
        protected override bool CanRename => false;

        /// <summary>
        /// Called when property gets changed.
        /// </summary>
        /// <param name="p">The property value assigned.</param>
        protected virtual void OnPropertyChanged(PropertyInfo p)
        {
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var p = Property;
            TitleTintColor = p != null ? Color.White : Color.Red;
        }
    }
}
