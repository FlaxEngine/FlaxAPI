// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Linq;
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
    public abstract class PropertyTrack : Track
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
        /// Gets or sets the object property. Performs the value validation on set. This can be <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.
        /// </summary>
        public MemberInfo Property
        {
            get
            {
                if (ParentTrack is IObjectTrack objectTrack)
                {
                    var obj = objectTrack.Object;
                    if (obj != null)
                    {
                        return GetMember(obj.GetType(), PropertyName);
                    }
                }
                return null;
            }
            set
            {
                if (value != null && ParentTrack is IObjectTrack objectTrack)
                {
                    var obj = objectTrack.Object;
                    if (obj != null)
                    {
                        if (GetMember(obj.GetType(), value.Name) == null)
                            throw new Exception("Cannot use property " + value + " for object of type " + obj.GetType());
                    }
                }

                var p = value as PropertyInfo;
                var f = value as FieldInfo;
                var type = p?.PropertyType ?? f?.FieldType;

                if (value != null)
                {
                    PropertyName = value.Name;
                    PropertyTypeName = type.FullName;
                    ValueSize = GetValueDataSize(type);
                }
                else
                {
                    PropertyName = string.Empty;
                    PropertyTypeName = string.Empty;
                    ValueSize = 0;
                }

                OnPropertyChanged(value, type);
            }
        }

        protected Label _previewValue;
        protected Image _rightKey;
        protected Image _addKey;
        protected Image _leftKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTrack"/> class.
        /// </summary>
        /// <param name="options">The track initial options.</param>
        /// <param name="useNavigationButtons">True if show keyframe navigation buttons, otherwise false.</param>
        /// <param name="useValuePreview">True if show current value preview, otherwise false.</param>
        protected PropertyTrack(ref TrackCreateOptions options, bool useNavigationButtons = true, bool useValuePreview = true)
        : base(ref options)
        {
            var uiLeft = _muteCheckbox.Left;

            if (useNavigationButtons)
            {
                // Navigation buttons
                const float buttonSize = 14;
                var icons = Editor.Instance.Icons;
                _rightKey = new Image(uiLeft - buttonSize - 2.0f, 0, buttonSize, buttonSize)
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
                uiLeft = _leftKey.Left;
            }

            if (useValuePreview)
            {
                // Value preview
                var previewWidth = 100.0f;
                _previewValue = new Label(uiLeft - previewWidth - 2.0f, 0, previewWidth, TextBox.DefaultHeight)
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
        }

        /// <summary>
        /// Gets the member (field or property) from the given type.
        /// </summary>
        /// <param name="type">The declaring type.</param>
        /// <param name="name">The member name.</param>
        /// <returns>The member or null if not found.</returns>
        protected static MemberInfo GetMember(Type type, string name)
        {
            return type.GetMember(name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();
        }

        /// <summary>
        /// Tries the get current value from the assigned object property.
        /// </summary>
        /// <param name="value">The result value. Valid only if methods returns true.</param>
        /// <returns>True if got value, otherwise false.</returns>
        protected virtual bool TryGetValue(out object value)
        {
            if (!string.IsNullOrEmpty(PropertyName) && ParentTrack is IObjectTrack objectTrack)
            {
                var obj = objectTrack.Object;
                if (obj != null)
                {
                    var member = GetMember(obj.GetType(), PropertyName);
                    if (member is PropertyInfo p)
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
                    if (member is FieldInfo f)
                    {
                        try
                        {
                            value = f.GetValue(obj);
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

        /// <summary>
        /// Gets the size of the value data type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The value data size (in bytes).</returns>
        protected virtual int GetValueDataSize(Type type)
        {
            return type.IsValueType ? (Marshal.SizeOf(type.IsEnum ? Enum.GetUnderlyingType(type) : type)) : 0;
        }

        /// <inheritdoc />
        protected override bool CanDrag => false;

        /// <inheritdoc />
        protected override bool CanRename => false;

        /// <summary>
        /// Called when property gets changed.
        /// </summary>
        /// <param name="value">The property value assigned.</param>
        /// <param name="type">The property type assigned.</param>
        protected virtual void OnPropertyChanged(MemberInfo value, Type type)
        {
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var p = Property;
            TitleTintColor = p != null ? Color.White : Color.Red;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _previewValue = null;
            _rightKey = null;
            _addKey = null;
            _leftKey = null;

            base.Dispose();
        }
    }
}
