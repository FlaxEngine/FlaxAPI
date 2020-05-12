// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Json;
using Utils = FlaxEditor.Utilities.Utils;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit key-value dictionaries.
    /// </summary>
    public class DictionaryEditor : CustomEditor
    {
        /// <summary>
        /// The custom implementation of the dictionary items labels that can be used to remove items or edit keys.
        /// </summary>
        /// <seealso cref="FlaxEditor.CustomEditors.GUI.PropertyNameLabel" />
        private class DictionaryItemLabel : PropertyNameLabel
        {
            private DictionaryEditor _editor;
            private object _key;

            /// <summary>
            /// Initializes a new instance of the <see cref="DictionaryItemLabel"/> class.
            /// </summary>
            /// <param name="editor">The editor.</param>
            /// <param name="key">The key.</param>
            public DictionaryItemLabel(DictionaryEditor editor, object key)
            : base(key?.ToString() ?? "<null>")
            {
                _editor = editor;
                _key = key;

                SetupContextMenu += OnSetupContextMenu;
            }

            private void OnSetupContextMenu(PropertyNameLabel label, ContextMenu menu, CustomEditor linkedEditor)
            {
                menu.AddSeparator();

                menu.AddButton("Remove", OnRemoveClicked).Enabled = !_editor._readOnly;
                menu.AddButton("Edit", OnEditClicked).Enabled = _editor._canEditKeys;
            }

            private void OnRemoveClicked(ContextMenuButton button)
            {
                _editor.Remove(_key);
            }

            private void OnEditClicked(ContextMenuButton button)
            {
                var keyType = _editor.Values.Type.GetGenericArguments()[0];
                if (keyType == typeof(string) || keyType.IsPrimitive)
                {
                    var popup = RenamePopup.Show(Parent, Bounds, Text, false);
                    popup.Validate += (renamePopup, value) =>
                    {
                        object newKey;
                        if (keyType.IsPrimitive)
                            newKey = JsonSerializer.Deserialize(value, keyType);
                        else
                            newKey = value;
                        return !((IDictionary)_editor.Values[0]).Contains(newKey);
                    };
                    popup.Renamed += renamePopup =>
                    {
                        object newKey;
                        if (keyType.IsPrimitive)
                            newKey = JsonSerializer.Deserialize(renamePopup.Text, keyType);
                        else
                            newKey = renamePopup.Text;

                        _editor.ChangeKey(_key, newKey);
                        _key = newKey;
                        Text = _key.ToString();
                    };
                }
                else if (keyType.IsEnum)
                {
                    var popup = RenamePopup.Show(Parent, Bounds, Text, false);
                    var picker = new EnumComboBox(keyType)
                    {
                        AnchorPreset = AnchorPresets.StretchAll,
                        Offsets = Margin.Zero,
                        Parent = popup,
                        EnumTypeValue = _key,
                    };
                    picker.ValueChanged += () =>
                    {
                        popup.Hide();
                        object newKey = picker.EnumTypeValue;
                        if (!((IDictionary)_editor.Values[0]).Contains(newKey))
                        {
                            _editor.ChangeKey(_key, newKey);
                            _key = newKey;
                            Text = _key.ToString();
                        }
                    };
                }
                else
                {
                    throw new NotImplementedException("Missing editing for dictionary key type " + keyType);
                }
            }

            /// <inheritdoc />
            public override bool OnMouseDoubleClick(Vector2 location, MouseButton button)
            {
                if (button == MouseButton.Left)
                {
                    OnEditClicked(null);
                    return true;
                }

                return base.OnMouseDoubleClick(location, button);
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _editor = null;
                _key = null;

                base.OnDestroy();
            }
        }

        private IntegerValueElement _size;
        private int _elementsCount;
        private bool _readOnly;
        private bool _notNullItems;
        private bool _canEditKeys;

        /// <summary>
        /// Determines whether this editor[can edit the specified dictionary type.
        /// </summary>
        /// <param name="type">Type of the dictionary.</param>
        /// <returns>True if can edit, otherwise false.</returns>
        public static bool CanEditType(Type type)
        {
            // Ensure it's a generic dictionary type
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the length of the collection.
        /// </summary>
        public int Count => (Values[0] as IDictionary)?.Count ?? 0;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _readOnly = false;
            _notNullItems = false;

            // No support for different collections for now
            if (HasDifferentValues || HasDifferentTypes)
                return;

            var type = Values.Type;
            var size = Count;
            var argTypes = type.GetGenericArguments();
            var keyType = argTypes[0];
            var valueType = argTypes[1];
            _canEditKeys = keyType == typeof(string) || keyType.IsPrimitive || keyType.IsEnum;

            // Try get CollectionAttribute for collection editor meta
            var attributes = Values.GetAttributes();
            Type overrideEditorType = null;
            if (attributes != null)
            {
                var collection = (CollectionAttribute)attributes.FirstOrDefault(x => x is CollectionAttribute);
                if (collection != null)
                {
                    // TODO: handle ReadOnly and NotNullItems by filtering child editors SetValue

                    _readOnly = collection.ReadOnly;
                    _notNullItems = collection.NotNullItems;
                    overrideEditorType = Utils.GetType(collection.OverrideEditorTypeName);
                }
            }

            // Size
            if (_readOnly || !_canEditKeys)
            {
                layout.Label("Size", size.ToString());
            }
            else
            {
                _size = layout.IntegerValue("Size");
                _size.IntValue.MinValue = 0;
                _size.IntValue.MaxValue = ushort.MaxValue;
                _size.IntValue.Value = size;
                _size.IntValue.ValueChanged += OnSizeChanged;
            }

            // Elements
            if (size > 0)
            {
                var keysEnumerable = ((IDictionary)Values[0]).Keys.OfType<object>();
                var keys = keysEnumerable as object[] ?? keysEnumerable.ToArray();
                for (int i = 0; i < size; i++)
                {
                    var key = keys.ElementAt(i);
                    var overrideEditor = overrideEditorType != null ? (CustomEditor)Activator.CreateInstance(overrideEditorType) : null;
                    layout.Object(new DictionaryItemLabel(this, key), new DictionaryValueContainer(valueType, key, Values), overrideEditor);
                }
            }
            _elementsCount = size;

            // Add/Remove buttons
            if (!_readOnly && _canEditKeys)
            {
                var area = layout.Space(20);
                var addButton = new Button(area.ContainerControl.Width - (16 + 16 + 2 + 2), 2, 16, 16)
                {
                    Text = "+",
                    TooltipText = "Add new item",
                    AnchorPreset = AnchorPresets.TopRight,
                    Parent = area.ContainerControl
                };
                addButton.Clicked += () =>
                {
                    if (IsSetBlocked)
                        return;

                    Resize(Count + 1);
                };
                var removeButton = new Button(addButton.Right + 2, addButton.Y, 16, 16)
                {
                    Text = "-",
                    TooltipText = "Remove last item",
                    AnchorPreset = AnchorPresets.TopRight,
                    Parent = area.ContainerControl,
                    Enabled = size > 0
                };
                removeButton.Clicked += () =>
                {
                    if (IsSetBlocked)
                        return;

                    Resize(Count - 1);
                };
            }
        }

        private void OnSizeChanged()
        {
            if (IsSetBlocked)
                return;

            Resize(_size.IntValue.Value);
        }

        /// <summary>
        /// Removes the item of the specified key. It supports undo.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        private void Remove(object key)
        {
            if (IsSetBlocked)
                return;

            // Allocate new collection
            var dictionary = Values[0] as IDictionary;
            var type = Values.Type;
            var newValues = (IDictionary)Activator.CreateInstance(type);

            // Copy all keys/values except the specified one
            if (dictionary != null)
            {
                foreach (var e in dictionary.Keys)
                {
                    if (e == key)
                        continue;
                    newValues[e] = dictionary[e];
                }
            }

            SetValue(newValues);
        }

        /// <summary>
        /// Changes the key of the item.
        /// </summary>
        /// <param name="oldKey">The old key value.</param>
        /// <param name="newKey">The new key value.</param>
        protected void ChangeKey(object oldKey, object newKey)
        {
            var dictionary = (IDictionary)Values[0];
            var newValues = (IDictionary)Activator.CreateInstance(Values.Type);
            foreach (var e in dictionary.Keys)
            {
                if (Equals(e, oldKey))
                    newValues[newKey] = dictionary[e];
                else
                    newValues[e] = dictionary[e];
            }
            SetValue(newValues);
        }

        /// <summary>
        /// Resizes collection to the specified new size.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        protected void Resize(int newSize)
        {
            var dictionary = Values[0] as IDictionary;
            var oldSize = dictionary?.Count ?? 0;

            if (oldSize == newSize)
                return;

            // Allocate new collection
            var type = Values.Type;
            var argTypes = type.GetGenericArguments();
            var keyType = argTypes[0];
            var valueType = argTypes[1];
            var newValues = (IDictionary)Activator.CreateInstance(type);

            // Copy all keys/values
            int itemsLeft = newSize;
            if (dictionary != null)
            {
                foreach (var e in dictionary.Keys)
                {
                    if (itemsLeft == 0)
                        break;
                    newValues[e] = dictionary[e];
                    itemsLeft--;
                }
            }

            // Insert new items (find unique keys)
            int newItemsLeft = newSize - oldSize;
            while (newItemsLeft-- > 0)
            {
                if (keyType.IsPrimitive)
                {
                    long uniqueKey = 0;
                    bool isUnique;
                    do
                    {
                        isUnique = true;
                        foreach (var e in newValues.Keys)
                        {
                            var asLong = Convert.ToInt64(e);
                            if (asLong == uniqueKey)
                            {
                                uniqueKey++;
                                isUnique = false;
                                break;
                            }
                        }
                    } while (!isUnique);

                    newValues[Convert.ChangeType(uniqueKey, keyType)] = Utils.GetDefaultValue(valueType);
                }
                else if (keyType.IsEnum)
                {
                    var enumValues = Enum.GetValues(keyType);
                    int uniqueKeyIndex = 0;
                    bool isUnique;
                    do
                    {
                        isUnique = true;
                        foreach (var e in newValues.Keys)
                        {
                            if (Equals(e, enumValues.GetValue(uniqueKeyIndex)))
                            {
                                uniqueKeyIndex++;
                                isUnique = false;
                                break;
                            }
                        }
                    } while (!isUnique && uniqueKeyIndex < enumValues.Length);

                    newValues[enumValues.GetValue(uniqueKeyIndex)] = Utils.GetDefaultValue(valueType);
                }
                else if (keyType == typeof(string))
                {
                    string uniqueKey = "Key";
                    bool isUnique;
                    do
                    {
                        isUnique = true;
                        foreach (var e in newValues.Keys)
                        {
                            if ((string)e == uniqueKey)
                            {
                                uniqueKey += "*";
                                isUnique = false;
                                break;
                            }
                        }
                    } while (!isUnique);

                    newValues[uniqueKey] = Utils.GetDefaultValue(valueType);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            SetValue(newValues);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            // No support for different collections for now
            if (HasDifferentValues || HasDifferentTypes)
                return;

            // Check if collection has been resized (by UI or from external source)
            if (Count != _elementsCount)
            {
                RebuildLayout();
            }
        }
    }
}
