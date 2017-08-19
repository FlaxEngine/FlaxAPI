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
        private class ItemInfo : IComparable
        {
            public PropertyInfo Info;
            public EditorOrderAttribute Order;
            public EditorDisplayAttribute Display;

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
            /// Initializes a new instance of the <see cref="ItemInfo"/> class.
            /// </summary>
            /// <param name="info">The information.</param>
            /// <param name="attributes">The attributes.</param>
            public ItemInfo(PropertyInfo info, object[] attributes)
            {
                Info = info;
                Order = (EditorOrderAttribute)attributes.FirstOrDefault(x => x is EditorOrderAttribute);
                Display = (EditorDisplayAttribute)attributes.FirstOrDefault(x => x is EditorDisplayAttribute);
                // TODO: support custom editor type via CustomClassEditorAttribute

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
        }

        private List<ItemInfo> GetItemsForType(Type type)
        {
            // TODO: cache this per type?

            // Process the properties
            var properties = type.GetProperties();
            var items = new List<ItemInfo>(properties.Length);
            for (int i = 0; i < properties.Length; i++)
            {
                var p = properties[i];

                // Skip hidden properties and only set properties
                var getter = p.GetMethod;
                if (getter == null || !getter.IsPublic || p.GetIndexParameters().GetLength(0) != 0)
                    continue;

                // Handle HideInEditorAttribute
                var attributes = p.GetCustomAttributes(true);
                if (attributes.Any(x => x is HideInEditorAttribute))
                    continue;

                var item = new ItemInfo(p, attributes);
                items.Add(item);
            }

            return items;
        }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (Values == null)
                return;

            // TODO: for structures get all public fields
            // TODO: for objects get all public properties
            // TODO: support attribues
            // TODO: spawn custom editors for every editable thing
            // TODO; use shared properties/fields across all selected objects values
            
            // Collect items to edit
            List<ItemInfo> items;
            if (!HasDiffrentTypes)
            {
                items = GetItemsForType(Values[0].GetType());
            }
            else
            {
                var types = ValuesTypes;
                items = GetItemsForType(types[0]);
                for (int i = 1; i < types.Length; i++)
                {
                    var otherItems = GetItemsForType(types[i]);

                    // TODO: merge items and items
                }
            }

            // TODO: promote children to other base class like CustomEditorContainer ?

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
                    itemValues = new ValueContainer(Values.Count);
                    for (int j = 0; j < Values.Count; j++)
                        itemValues.Add(item.Info.GetValue(Values[j]));
                }
                catch (Exception ex)
                {
                    Editor.LogWarning("Failed to get object values for item " + item);
                    Editor.LogWarning(ex.Message);
                    Editor.LogWarning(ex.StackTrace);
                    return;
                }

                // Spawn child editor
                // TODO: remove test code
                if (item.Info.PropertyType == typeof(string) || item.Info.PropertyType == typeof(bool))
                    itemLayout.Property(item.DisplayName, item.Info, itemValues);
                else
                    itemLayout.Button(item.DisplayName + " order: " + (item.Order != null ? item.Order.Order.ToString() : "?"));
                //Debug.Log("Child item " + item);
            }
        }
    }
}
