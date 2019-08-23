// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
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
        /// Maps the basic type to it's UI name.
        /// </summary>
        protected static readonly Dictionary<Type, string> BasicTypesNames = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
        };

        /// <summary>
        /// Maps the type to the default track archetype for it.
        /// </summary>
        protected static readonly Dictionary<Type, TrackArchetype> BasicTypesTrackArchetypes = new Dictionary<Type, TrackArchetype>
        {
            { typeof(bool), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(byte), KeyframesObjectPropertyTrack.GetArchetype() },
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
        };
    }
}
