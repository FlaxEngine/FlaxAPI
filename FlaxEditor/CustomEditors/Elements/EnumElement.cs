// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The enum editor element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class EnumElement : LayoutElement
    {
        private Type _underlyingType;

        /// <summary>
        /// The combo box used to show enum values.
        /// </summary>
        protected readonly EnumComboBox _comboBox;

        /// <summary>
        /// The enum type.
        /// </summary>
        protected readonly Type _enumType;

        /// <summary>
        /// The enum entries. The same order as items in combo box.
        /// </summary>
        protected readonly List<Entry> _entries = new List<Entry>();

        /// <summary>
        /// The cached value from the UI.
        /// </summary>
        protected int _cachedValue;

        /// <summary>
        /// True if has value cached, otherwise false.
        /// </summary>
        protected bool _hasValueCached;

        /// <summary>
        /// The enum entry.
        /// </summary>
        public struct Entry
        {
            /// <summary>
            /// The name.
            /// </summary>
            public string Name;

            /// <summary>
            /// The value.
            /// </summary>
            public int Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> struct.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public Entry(string name, int value)
            {
                Name = name;
                Value = value;
            }
        }

        /// <summary>
        /// The custom combobox for enum editing. Supports some special cases for flag enums.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ComboBox" />
        protected class EnumComboBox : ComboBox
        {
            private readonly EnumElement _element;

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumComboBox"/> class.
            /// </summary>
            /// <param name="element">The element.</param>
            public EnumComboBox(EnumElement element)
            {
                _element = element;
            }

            /// <inheritdoc />
            protected override void OnItemClicked(int index)
            {
                if (_element.IsFlags)
                {
                    var entries = _element._entries;

                    // Special case if clicked enum with zero value
                    if (entries[index].Value == 0)
                    {
                        SelectedIndex = index;
                        return;
                    }

                    // Calculate value that will be set after change
                    int valueAfter = 0;
                    for (int i = 0; i < _selectedIndices.Count; i++)
                    {
                        int selectedIndex = _selectedIndices[i];
                        if (selectedIndex != index)
                            valueAfter |= entries[selectedIndex].Value;
                    }
                    bool contains = _selectedIndices.Contains(index);
                    if (!contains)
                        valueAfter |= entries[index].Value;

                    // Skip if value won't change
                    if (_element.Value == valueAfter)
                    {
                        return;
                    }

                    // Build new selection
                    for (int i = 0; i < entries.Count; i++)
                    {
                        if (entries[i].Value == valueAfter)
                        {
                            SelectedIndex = i;
                            return;
                        }
                    }
                    _selectedIndices.Clear();
                    for (int i = 0; i < entries.Count; i++)
                    {
                        var e = entries[i].Value;
                        if (e != 0 && (e & valueAfter) == e)
                        {
                            _selectedIndices.Add(i);
                        }
                    }
                    OnSelectedIndexChanged();
                    return;
                }

                base.OnItemClicked(index);
            }
        }

        /// <summary>
        /// Custom extension delegate used to build enum element entries layout.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="entries">The output entries collection.</param>
        public delegate void BuildEntriesDelegate(Type type, List<Entry> entries);

        /// <summary>
        /// Gets a value indicating whether this enum has flags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this enum has flags; otherwise, <c>false</c>.
        /// </value>
        public bool IsFlags { get; }

        /// <summary>
        /// Gets or sets the value of the enum (may not be int).
        /// </summary>
        /// <value>
        /// The enum type value.
        /// </value>
        public object EnumTypeValue
        {
            get => Convert.ChangeType(Value, _underlyingType);
            set => Value = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value
        {
            get => _cachedValue;
            set
            {
                // Skip if won't change
                if (_cachedValue == value && _hasValueCached)
                    return;

                // Single value
                for (int i = 0; i < _entries.Count; i++)
                {
                    if (_entries[i].Value == value)
                    {
                        _comboBox.SelectedIndex = i;
                        return;
                    }
                }

                if (IsFlags)
                {
                    // Collection of flags
                    var selection = new List<int>();
                    for (int i = 0; i < _entries.Count; i++)
                    {
                        var e = _entries[i].Value;
                        if (e != 0 && (e & value) == e)
                        {
                            selection.Add(i);
                        }
                    }
                    _comboBox.Selection = selection;
                }
                else
                {
                    _comboBox.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Caches the selected UI enum value.
        /// </summary>
        protected void CacheValue()
        {
            int value = 0;
            if (IsFlags)
            {
                var selection = _comboBox.Selection;
                for (int i = 0; i < selection.Count; i++)
                {
                    int index = selection[i];
                    value |= _entries[index].Value;
                }
            }
            else
            {
                var selectedIndex = _comboBox.SelectedIndex;
                if (selectedIndex != -1)
                    value = _entries[selectedIndex].Value;
            }
            _cachedValue = value;
            _hasValueCached = true;
        }

        /// <summary>
        /// Occurs when value gets changed.
        /// </summary>
        public event Action ValueChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumElement"/> class.
        /// </summary>
        /// <param name="type">The enum type.</param>
        /// <param name="customBuildEntriesDelegate">The custom entries layout builder. Allows to hide existing or add different enum values to editor.</param>
        /// <param name="formatMode">The formatting mode.</param>
        public EnumElement(Type type, BuildEntriesDelegate customBuildEntriesDelegate = null, EnumDisplayAttribute.FormatMode formatMode = EnumDisplayAttribute.FormatMode.Default)
        {
            if (type == null || !type.IsEnum)
                throw new ArgumentException("Invalid enum type.");
            _enumType = type;
            _underlyingType = Enum.GetUnderlyingType(_enumType);
            IsFlags = _enumType.GetCustomAttribute<FlagsAttribute>() != null;

            _comboBox = new EnumComboBox(this)
            {
                SupportMultiSelect = IsFlags,
                MaximumItemsInViewCount = 15,
            };
            _comboBox.SelectedIndexChanged += ComboBoxOnSelectedIndexChanged;

            if (customBuildEntriesDelegate != null)
                customBuildEntriesDelegate(type, _entries);
            else
                BuildEntriesDefault(type, _entries, formatMode);

            for (int i = 0; i < _entries.Count; i++)
            {
                _comboBox.AddItem(_entries[i].Name);
            }
        }

        private void ComboBoxOnSelectedIndexChanged(ComboBox comboBox)
        {
            CacheValue();
            ValueChanged?.Invoke();
        }

        /// <summary>
        /// Builds the default entries for the given enum type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="entries">The output entries.</param>
        /// <param name="formatMode">The formatting mode.</param>
        public static void BuildEntriesDefault(Type type, List<Entry> entries, EnumDisplayAttribute.FormatMode formatMode = EnumDisplayAttribute.FormatMode.Default)
        {
            FieldInfo[] fields = type.GetFields();
            entries.Capacity = fields.Length - 1;
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (field.Name.Equals("value__"))
                    continue;

                string name;
                switch (formatMode)
                {
                case EnumDisplayAttribute.FormatMode.Default:
                    name = CustomEditorsUtil.GetPropertyNameUI(field.Name);
                    break;
                case EnumDisplayAttribute.FormatMode.None:
                    name = field.Name;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(formatMode), formatMode, null);
                }
                entries.Add(new Entry(name, Convert.ToInt32(field.GetRawConstantValue())));
            }
        }

        /// <inheritdoc />
        public override Control Control => _comboBox;
    }
}
