// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlaxEditor.GUI.ContextMenu;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating managed objects.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public abstract class ObjectTrack : Track
    {
        /// <summary>
        /// The add button.
        /// </summary>
        protected Button _addButton;

        /// <summary>
        /// Gets the object instance (may be null if reference is invalid or data is missing).
        /// </summary>
        public abstract FlaxEngine.Object Object { get; }

        /// <inheritdoc />
        protected ObjectTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            // Add button
            const float buttonSize = 14;
            _addButton = new Button(_muteCheckbox.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                Text = "+",
                TooltipText = "Add sub-tracks",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Parent = this
            };
            _addButton.Clicked += OnAddButtonClicked;
        }

        private void OnAddButtonClicked()
        {
            var menu = new ContextMenu.ContextMenu();
            OnShowAddContextMenu(menu);
            menu.Show(_addButton.Parent, _addButton.BottomLeft);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var obj = Object;
            TitleTintColor = obj ? Color.White : Color.Red;
        }

        /// <summary>
        /// Called when showing the context menu for add button (for sub-tracks adding).
        /// </summary>
        /// <param name="menu">The menu.</param>
        protected virtual void OnShowAddContextMenu(ContextMenu.ContextMenu menu)
        {
        }

        /// <summary>
        /// Called on context menu button click to add new object property animation track. Button should have <see cref="PropertyInfo"/> value assigned to the <see cref="Control.Tag"/> field.
        /// </summary>
        /// <param name="button">The button (with <see cref="PropertyInfo"/> value assigned to the <see cref="Control.Tag"/> field.).</param>
        protected void OnAddObjectPropertyTrack(ContextMenuButton button)
        {
            var p = (PropertyInfo)button.Tag;

            // Detect the type of the track to use
            var valueType = p.PropertyType;
            if (BasicTypesTrackArchetypes.TryGetValue(valueType, out var archetype))
            {
                // Basic type
            }
            else if (valueType.IsEnum)
            {
                // Enum
                archetype = KeyframesObjectPropertyTrack.GetArchetype();
            }
            else if (typeof(FlaxEngine.Object).IsAssignableFrom(valueType))
            {
                // Flax object
                archetype = FlaxKeyframesObjectPropertyTrack.GetArchetype();
            }
            else
            {
                throw new Exception("Invalid property type to create animation track for it. Value type: " + valueType);
            }

            var track = (ObjectPropertyTrack)Timeline.AddTrack(archetype);
            track.ParentTrack = this;
            track.TrackIndex = TrackIndex + 1;
            track.Name = Guid.NewGuid().ToString("N");
            track.Property = p;

            Timeline.OnTracksOrderChanged();
            Timeline.MarkAsEdited();
            Expand();
        }

        /// <summary>
        /// Adds the object properties animation track options to menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="type">The object type.</param>
        /// <returns>The added options count.</returns>
        protected int AddObjectProperties(ContextMenu.ContextMenu menu, Type type)
        {
            int count = 0;

            // TODO: implement editor-wide cache for animated properties per object type (add this in CodeEditingModule)
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                var p = properties[i];

                // Properties with read/write
                if (!(p.CanRead && p.CanWrite && p.GetIndexParameters().GetLength(0) == 0))
                    continue;

                var attributes = p.GetCustomAttributes();

                // Check if has attribute to skip animating
                if (attributes.Any(x => x is NoAnimateAttribute))
                    continue;

                // Validate value type
                var valueType = p.PropertyType;
                if (BasicTypesNames.TryGetValue(valueType, out var name))
                {
                    // Basic type
                }
                else if (valueType.IsValueType)
                {
                    // Enum or Structure
                    name = valueType.Name;
                }
                else if (typeof(FlaxEngine.Object).IsAssignableFrom(valueType))
                {
                    // Flax object
                    name = valueType.Name;
                }
                else
                {
                    // Animating subobjects properties is not supported
                    continue;
                }

                // Prevent from adding the same track twice
                if (SubTracks.Any(x => x is ObjectPropertyTrack y && y.PropertyName == p.Name))
                    continue;

                menu.AddButton(name + " " + p.Name, OnAddObjectPropertyTrack).Tag = p;
                count++;
            }

            return count;
        }

        /// <summary>
        /// Maps the basic type to it's UI name.
        /// </summary>
        protected static readonly Dictionary<Type, string> BasicTypesNames = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(string), "string" },
        };

        /// <summary>
        /// Maps the type to the default track archetype for it.
        /// </summary>
        protected static readonly Dictionary<Type, TrackArchetype> BasicTypesTrackArchetypes = new Dictionary<Type, TrackArchetype>
        {
            { typeof(bool), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(byte), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(sbyte), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(char), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(short), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(ushort), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(int), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(uint), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(long), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(float), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(double), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(Vector2), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(Vector3), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(Vector4), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(Quaternion), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(Color), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(Color32), CurveObjectPropertyTrack.GetArchetype() },
            { typeof(Guid), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(DateTime), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(TimeSpan), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(string), StringKeyframesObjectPropertyTrack.GetArchetype() },
        };
    }
}
