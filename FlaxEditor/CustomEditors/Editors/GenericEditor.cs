// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.CustomEditors.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used when no specified inspector is provided for the type. Inspector 
    /// displays GUI for all the inspectable fields in the object.
    /// </summary>
    public class GenericEditor : CustomEditor
    {
        /// <summary>
        /// Describes object property/field information for custom editors pipeline.
        /// </summary>
        /// <seealso cref="System.IComparable" />
        protected class ItemInfo : IComparable
        {
            /// <summary>
            /// The member information from reflection.
            /// </summary>
            public MemberInfo Info;

            /// <summary>
            /// The order attribute.
            /// </summary>
            public EditorOrderAttribute Order;

            /// <summary>
            /// The display attribute.
            /// </summary>
            public EditorDisplayAttribute Display;

            /// <summary>
            /// The tooltip attribute.
            /// </summary>
            public TooltipAttribute Tooltip;

            /// <summary>
            /// The custom editor attribute.
            /// </summary>
            public CustomEditorAttribute CustomEditor;

            /// <summary>
            /// The custom editor alias attribute.
            /// </summary>
            public CustomEditorAliasAttribute CustomEditorAlias;

            /// <summary>
            /// The space attribute.
            /// </summary>
            public SpaceAttribute Space;

            /// <summary>
            /// The header attribute.
            /// </summary>
            public HeaderAttribute Header;

            /// <summary>
            /// The visible if attribute.
            /// </summary>
            public VisibleIfAttribute VisibleIf;

            /// <summary>
            /// The read-only attribute usage flag.
            /// </summary>
            public bool IsReadOnly;

            /// <summary>
            /// The expand groups flag.
            /// </summary>
            public bool ExpandGroups;

            /// <summary>
            /// Gets the display name.
            /// </summary>
            public string DisplayName { get; }

            /// <summary>
            /// Gets a value indicating whether use dedicated group.
            /// </summary>
            public bool UseGroup => Display?.Group != null;

            /// <summary>
            /// Gets the overridden custom editor for item editing.
            /// </summary>
            public CustomEditor OverrideEditor
            {
                get
                {
                    if (CustomEditor != null)
                        return (CustomEditor)Activator.CreateInstance(CustomEditor.Type);
                    if (CustomEditorAlias != null)
                        return (CustomEditor)Utilities.Utils.CreateInstance(CustomEditorAlias.TypeName);
                    return null;
                }
            }

            /// <summary>
            /// Gets the tooltip text (may be null if not provided).
            /// </summary>
            public string TooltipText => Tooltip?.Text;

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemInfo"/> class.
            /// </summary>
            /// <param name="info">The reflection information.</param>
            public ItemInfo(MemberInfo info)
            : this(info, info.GetCustomAttributes(true))
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemInfo"/> class.
            /// </summary>
            /// <param name="info">The reflection information.</param>
            /// <param name="attributes">The attributes.</param>
            public ItemInfo(MemberInfo info, object[] attributes)
            {
                Info = info;
                Order = (EditorOrderAttribute)attributes.FirstOrDefault(x => x is EditorOrderAttribute);
                Display = (EditorDisplayAttribute)attributes.FirstOrDefault(x => x is EditorDisplayAttribute);
                Tooltip = (TooltipAttribute)attributes.FirstOrDefault(x => x is TooltipAttribute);
                CustomEditor = (CustomEditorAttribute)attributes.FirstOrDefault(x => x is CustomEditorAttribute);
                CustomEditorAlias = (CustomEditorAliasAttribute)attributes.FirstOrDefault(x => x is CustomEditorAliasAttribute);
                Space = (SpaceAttribute)attributes.FirstOrDefault(x => x is SpaceAttribute);
                Header = (HeaderAttribute)attributes.FirstOrDefault(x => x is HeaderAttribute);
                VisibleIf = (VisibleIfAttribute)attributes.FirstOrDefault(x => x is VisibleIfAttribute);
                IsReadOnly = attributes.FirstOrDefault(x => x is ReadOnlyAttribute) != null;
                ExpandGroups = attributes.FirstOrDefault(x => x is ExpandGroupsAttribute) != null;

                if (!IsReadOnly && info is FieldInfo fieldInfo && fieldInfo.IsInitOnly)
                {
                    // Field declared with `readonly` keyword
                    IsReadOnly = true;
                }
                if (!IsReadOnly && info is PropertyInfo propertyInfo && !propertyInfo.CanWrite)
                {
                    // Property without a setter
                    IsReadOnly = true;
                }
                if (Display?.Name != null)
                {
                    // Use name provided by the attribute
                    DisplayName = Display.Name;
                }
                else
                {
                    // Use filtered member name
                    DisplayName = CustomEditorsUtil.GetPropertyNameUI(info.Name);
                }
            }

            /// <summary>
            /// Gets the values.
            /// </summary>
            /// <param name="instanceValues">The instance values.</param>
            /// <returns>The values container.</returns>
            public ValueContainer GetValues(ValueContainer instanceValues)
            {
                return new ValueContainer(Info, instanceValues);
            }

            /// <inheritdoc />
            public int CompareTo(object obj)
            {
                if (obj is ItemInfo other)
                {
                    // By order
                    if (Order != null)
                    {
                        if (other.Order != null)
                            return Order.Order - other.Order.Order;
                        return -1;
                    }
                    else if (other.Order != null)
                        return 1;

                    // By group name
                    if (Display?.Group != null)
                    {
                        if (other.Display?.Group != null)
                            return string.Compare(Display.Group, other.Display.Group, StringComparison.InvariantCulture);
                    }

                    // By name
                    return string.Compare(Info.Name, other.Info.Name, StringComparison.InvariantCulture);
                }

                return 0;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return Info.Name;
            }

            /// <summary>
            /// Determines whether can merge two item infos to present them at once.
            /// </summary>
            /// <param name="a">The a.</param>
            /// <param name="b">The b.</param>
            /// <returns>
            ///   <c>true</c> if can merge two item infos to present them at once; otherwise, <c>false</c>.
            /// </returns>
            public static bool CanMerge(ItemInfo a, ItemInfo b)
            {
                if (a.Info.DeclaringType != b.Info.DeclaringType)
                    return false;
                return a.Info.Name == b.Info.Name;
            }
        }

        private struct VisibleIfCache
        {
            public MemberInfo Target;
            public MemberInfo Source;
            public PropertiesListElement PropertiesList;
            public int LabelIndex;

            public bool GetValue(object instance)
            {
                if (Source is FieldInfo fieldInfo)
                    return (bool)fieldInfo.GetValue(instance);
                return (bool)((PropertyInfo)Source).GetValue(instance, null);
            }
        }

        private VisibleIfCache[] _visibleIfCaches;

        /// <summary>
        /// Gets the items for the type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The items.</returns>
        protected virtual List<ItemInfo> GetItemsForType(Type type)
        {
            return GetItemsForType(type, type.IsClass, true);
        }

        /// <summary>
        /// Gets the items for the type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="useProperties">True if use type properties.</param>
        /// <param name="useFields">True if use type fields.</param>
        /// <returns>The items.</returns>
        protected List<ItemInfo> GetItemsForType(Type type, bool useProperties, bool useFields)
        {
            var items = new List<ItemInfo>();

            if (useProperties)
            {
                // TODO: cache properties items array per type?

                // Process properties
                var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                items.Capacity = Math.Max(items.Capacity, items.Count + properties.Length);
                for (int i = 0; i < properties.Length; i++)
                {
                    var p = properties[i];

                    // Skip only set properties and special cases
                    var getter = p.GetMethod;
                    if (getter == null || p.GetIndexParameters().GetLength(0) != 0)
                        continue;

                    var attributes = p.GetCustomAttributes(true);

                    // Skip hidden or get only properties, handle special attributes
                    if (((!getter.IsPublic || !p.CanWrite) && !attributes.Any(x => x is ShowInEditorAttribute)) || attributes.Any(x => x is HideInEditorAttribute))
                        continue;

                    var item = new ItemInfo(p, attributes);
                    items.Add(item);
                }
            }

            if (useFields)
            {
                // TODO: cache fields items array per type?

                // Process fields
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                items.Capacity = Math.Max(items.Capacity, items.Count + fields.Length);
                for (int i = 0; i < fields.Length; i++)
                {
                    var f = fields[i];

                    var attributes = f.GetCustomAttributes(true);

                    // Skip hidden fields, handle special attributes
                    if ((!f.IsPublic && !attributes.Any(x => x is ShowInEditorAttribute)) || attributes.Any(x => x is HideInEditorAttribute))
                        continue;

                    var item = new ItemInfo(f, attributes);
                    items.Add(item);
                }
            }

            return items;
        }

        private static MemberInfo GetVisibleIfSource(Type type, VisibleIfAttribute visibleIf)
        {
            var property = type.GetProperty(visibleIf.MemberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (property != null)
            {
                if (property.GetMethod == null)
                {
                    Debug.LogError("Invalid VisibleIf rule. Property has missing getter " + visibleIf.MemberName);
                    return null;
                }

                if (property.GetMethod.ReturnType != typeof(bool))
                {
                    Debug.LogError("Invalid VisibleIf rule. Property has to return bool type " + visibleIf.MemberName);
                    return null;
                }

                return property;
            }

            var field = type.GetField(visibleIf.MemberName);
            if (field != null)
            {
                if (field.FieldType != typeof(bool))
                {
                    Debug.LogError("Invalid VisibleIf rule. Field has to be bool type " + visibleIf.MemberName);
                    return null;
                }

                return field;
            }

            Debug.LogError("Invalid VisibleIf rule. Cannot find member " + visibleIf.MemberName);
            return null;
        }

        /// <summary>
        /// Spawns the property for the given item.
        /// </summary>
        /// <param name="itemLayout">The item layout.</param>
        /// <param name="itemValues">The item values.</param>
        /// <param name="item">The item.</param>
        protected virtual void SpawnProperty(LayoutElementsContainer itemLayout, ValueContainer itemValues, ItemInfo item)
        {
            int labelIndex = 0;
            if ((item.IsReadOnly || item.VisibleIf != null) &&
                itemLayout.Children.Count > 0 &&
                itemLayout.Children[itemLayout.Children.Count - 1] is PropertiesListElement propertiesListElement)
            {
                labelIndex = propertiesListElement.Labels.Count;
            }

            itemLayout.Property(item.DisplayName, itemValues, item.OverrideEditor, item.TooltipText);

            if (item.IsReadOnly && itemLayout.Children.Count > 0)
            {
                PropertiesListElement list = null;
                int firstChildControlIndex = 0;
                bool disableSingle = true;
                var control = itemLayout.Children[itemLayout.Children.Count - 1];
                if (control is GroupElement group && group.Children.Count > 0)
                {
                    list = group.Children[0] as PropertiesListElement;
                    disableSingle = false; // Disable all nested editors
                }
                else if (control is PropertiesListElement list1)
                {
                    list = list1;
                    firstChildControlIndex = list.Labels[labelIndex].FirstChildControlIndex;
                }

                if (list != null)
                {
                    // Disable controls added to the editor
                    var count = list.Properties.Children.Count;
                    for (int j = firstChildControlIndex; j < count; j++)
                    {
                        var child = list.Properties.Children[j];
                        if (disableSingle && child is PropertyNameLabel)
                            break;

                        child.Enabled = false;
                    }
                }
            }
            if (item.VisibleIf != null)
            {
                PropertiesListElement list;
                if (itemLayout.Children.Count > 0 && itemLayout.Children[itemLayout.Children.Count - 1] is PropertiesListElement list1)
                {
                    list = list1;
                }
                else
                {
                    // TODO: support inlined objects hiding?
                    return;
                }

                // Get source member used to check rule
                var sourceMember = GetVisibleIfSource(item.Info.DeclaringType, item.VisibleIf);
                if (sourceMember == null)
                    return;

                // Find the target control to show/hide

                // Resize cache
                if (_visibleIfCaches == null)
                    _visibleIfCaches = new VisibleIfCache[8];
                int count = 0;
                while (count < _visibleIfCaches.Length && _visibleIfCaches[count].Target != null)
                    count++;
                if (count >= _visibleIfCaches.Length)
                    Array.Resize(ref _visibleIfCaches, count * 2);

                // Add item
                _visibleIfCaches[count] = new VisibleIfCache
                {
                    Target = item.Info,
                    Source = sourceMember,
                    PropertiesList = list,
                    LabelIndex = labelIndex,
                };
            }
        }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _visibleIfCaches = null;

            // Collect items to edit
            List<ItemInfo> items;
            if (!HasDifferentTypes)
            {
                var value = Values[0];
                if (value == null)
                {
                    // Check if it's an object type that can be created in editor
                    var type = Values.Type;
                    if (type != null && type.GetConstructor(Type.EmptyTypes) != null)
                    {
                        layout = layout.Space(20);

                        const float ButtonSize = 14.0f;
                        var button = new Button
                        {
                            Text = "+",
                            TooltipText = "Create a new instance of the object",
                            Height = ButtonSize,
                            Width = ButtonSize,
                            X = layout.ContainerControl.Width - ButtonSize - 4,
                            AnchorStyle = AnchorStyle.CenterRight,
                            Parent = layout.ContainerControl
                        };
                        button.Clicked += () =>
                        {
                            var newType = Values.Type;
                            SetValue(Activator.CreateInstance(newType));
                            RebuildLayoutOnRefresh();
                        };
                    }

                    layout.Label("<null>");
                    return;
                }

                items = GetItemsForType(value.GetType());
            }
            else
            {
                var types = ValuesTypes;
                items = new List<ItemInfo>(GetItemsForType(types[0]));
                for (int i = 1; i < types.Length && items.Count > 0; i++)
                {
                    var otherItems = GetItemsForType(types[i]);

                    // Merge items
                    for (int j = 0; j < items.Count && items.Count > 0; j++)
                    {
                        bool isInvalid = true;
                        for (int k = 0; k < otherItems.Count; k++)
                        {
                            var a = items[j];
                            var b = otherItems[k];

                            if (ItemInfo.CanMerge(a, b))
                            {
                                isInvalid = false;
                                break;
                            }
                        }

                        if (isInvalid)
                        {
                            items.RemoveAt(j--);
                        }
                    }
                }
            }

            // Sort items
            items.Sort();

            // Add items
            GroupElement lastGroup = null;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                // Check if use group
                LayoutElementsContainer itemLayout;
                if (item.UseGroup)
                {
                    if (lastGroup == null || lastGroup.Panel.HeaderText != item.Display.Group)
                        lastGroup = layout.Group(item.Display.Group);
                    itemLayout = lastGroup;
                }
                else
                {
                    lastGroup = null;
                    itemLayout = layout;
                }

                // Space
                if (item.Space != null)
                    itemLayout.Space(item.Space.Height);

                // Header
                if (item.Header != null)
                    itemLayout.Header(item.Header.Text);

                // Peek values
                ValueContainer itemValues;
                try
                {
                    itemValues = item.GetValues(Values);
                }
                catch (Exception ex)
                {
                    Editor.LogWarning("Failed to get object values for item " + item);
                    Editor.LogWarning(ex.Message);
                    Editor.LogWarning(ex.StackTrace);
                    return;
                }

                // Spawn property editor
                SpawnProperty(itemLayout, itemValues, item);

                // Expand all parent groups if need to
                if (item.ExpandGroups)
                {
                    var c = itemLayout.ContainerControl;
                    do
                    {
                        if (c is DropPanel dropPanel)
                            dropPanel.Open(false);
                        else if (c is CustomEditorPresenter.PresenterPanel)
                            break;
                        c = c.Parent;
                    } while (c != null);
                }
            }
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (_visibleIfCaches != null)
            {
                try
                {
                    for (int i = 0; i < _visibleIfCaches.Length; i++)
                    {
                        var c = _visibleIfCaches[i];

                        if (c.Target == null)
                            break;

                        // Check rule (all objects must allow to show this property)
                        bool visible = true;
                        for (int j = 0; j < Values.Count; j++)
                        {
                            if (Values[j] != null && !c.GetValue(Values[j]))
                            {
                                visible = false;
                                break;
                            }
                        }

                        // Apply the visibility (note: there may be no label)
                        if (c.LabelIndex != -1 && c.PropertiesList.Labels.Count > c.LabelIndex)
                        {
                            var label = c.PropertiesList.Labels[c.LabelIndex];
                            label.Visible = visible;
                            for (int j = label.FirstChildControlIndex; j < c.PropertiesList.Properties.Children.Count; j++)
                            {
                                var child = c.PropertiesList.Properties.Children[j];
                                if (child is PropertyNameLabel)
                                    break;

                                child.Visible = visible;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Editor.LogWarning(ex);
                    Editor.LogError("Failed to update VisibleIf rules. " + ex.Message);

                    // Remove rules to prevent error in loop
                    _visibleIfCaches = null;
                }
            }

            base.Refresh();
        }
    }
}
