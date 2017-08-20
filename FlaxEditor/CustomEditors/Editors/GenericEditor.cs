////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used when no specified inspector is provided for the type. Inspector 
    /// displays GUI for all the inspectable fields in the object.
    /// </summary>
    public sealed class GenericEditor : CustomEditor
    {
        /// <summary>
        /// Describies object property/field information for custom editors pipeline.
        /// </summary>
        /// <seealso cref="System.IComparable" />
        private class ItemInfo : IComparable
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
            /// The custom editor attribute.
            /// </summary>
            public CustomEditorAttribute CustomEditor;

            /// <summary>
            /// Gets the display name.
            /// </summary>
            /// <value>
            /// The display name.
            /// </value>
            public string DisplayName { get; }

            /// <summary>
            /// Gets a value indicating whether use dedicated group.
            /// </summary>
            /// <value>
            ///   <c>true</c> if use group; otherwise, <c>false</c>.
            /// </value>
            public bool UseGroup => Display?.Group != null;

            /// <summary>
            /// Gets the overrided custom editor for item editing.
            /// </summary>
            /// <value>
            /// The overrided editor.
            /// </value>
            public CustomEditor OverrideEditor => CustomEditor != null ? (CustomEditor)Activator.CreateInstance(CustomEditor.Type) : null;

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
                CustomEditor = (CustomEditorAttribute)attributes.FirstOrDefault(x => x is CustomEditorAttribute);

                if (Display?.Name != null)
                {
                    // Use name provided by the attribute
                    DisplayName = Display.Name;
                }
                else
                {
                    // Process member name to make it more user-friendly
                    string name = info.Name;
                    int length = name.Length;
                    StringBuilder sb = new StringBuilder(length + 4);
                    for (int i = 0; i < length; i++)
                    {
                        var c = name[i];

                        if (char.IsUpper(c))
                        {
                            if (i + 2 < length && !char.IsUpper(name[i + 1]) && !char.IsUpper(name[i + 2]))
                                sb.Append(' ');
                        }
                        else if (c == '_')
                        {
                            if (sb.Length > 0)
                                sb.Append(' ');
                            continue;
                        }

                        sb.Append(c);
                    }

                    DisplayName = sb.ToString();
                }
            }

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

        private List<ItemInfo> GetItemsForType(Type type)
        {
            // TODO: cache this per type?

            var items = new List<ItemInfo>();

            if (type.IsClass)
            {
                // Process properties
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                items.Capacity = Math.Max(items.Capacity, items.Count + properties.Length);
                for (int i = 0; i < properties.Length; i++)
                {
                    var p = properties[i];

                    // Skip hidden properties and only set properties
                    var getter = p.GetMethod;
                    if (!p.CanRead || !p.CanWrite || getter == null || !getter.IsPublic || p.GetIndexParameters().GetLength(0) != 0)
                        continue;

                    // Handle HideInEditorAttribute
                    var attributes = p.GetCustomAttributes(true);
                    if (attributes.Any(x => x is HideInEditorAttribute))
                        continue;

                    var item = new ItemInfo(p, attributes);
                    items.Add(item);
                }
            }

            // Process fields
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            items.Capacity = Math.Max(items.Capacity, items.Count + fields.Length);
            for (int i = 0; i < fields.Length; i++)
            {
                var f = fields[i];

                // Skip hidden fields
                if (!f.IsPublic)
                    continue;

                // Handle HideInEditorAttribute
                var attributes = f.GetCustomAttributes(true);
                if (attributes.Any(x => x is HideInEditorAttribute))
                    continue;

                var item = new ItemInfo(f, attributes);
                items.Add(item);
            }

            return items;
        }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (Values == null)
                return;

            // Collect items to edit
            List<ItemInfo> items;
            if (!HasDiffrentTypes)
            {
                items = GetItemsForType(Values[0].GetType());
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

                            if(ItemInfo.CanMerge(a, b))
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
                    if (lastGroup == null || lastGroup.Panel.Name != item.Display.Group)
                        lastGroup = layout.Group(item.Display.Group);
                    itemLayout = lastGroup;
                }
                else
                {
                    lastGroup = null;
                    itemLayout = layout;
                }

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
                itemLayout.Property(item.DisplayName, item.Info, itemValues, item.OverrideEditor);
            }
        }
    }
}
