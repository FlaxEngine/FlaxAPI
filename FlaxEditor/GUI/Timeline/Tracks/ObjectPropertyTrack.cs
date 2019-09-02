// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Text;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating sub-object properties via child tracks.
    /// </summary>
    /// <seealso cref="PropertyTrack" />
    class ObjectPropertyTrack : PropertyTrack, IObjectTrack
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 14,
                Name = "Property",
                DisableSpawnViaGUI = true,
                Create = options => new ObjectPropertyTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (ObjectPropertyTrack)track;

            e.ValueSize = stream.ReadInt32();
            int propertyNameLength = stream.ReadInt32();
            int propertyTypeNameLength = stream.ReadInt32();

            var propertyName = stream.ReadBytes(propertyNameLength);
            e.PropertyName = Encoding.UTF8.GetString(propertyName, 0, propertyNameLength);
            if (stream.ReadChar() != 0)
                throw new Exception("Invalid track data.");

            var propertyTypeName = stream.ReadBytes(propertyTypeNameLength);
            e.PropertyTypeName = Encoding.UTF8.GetString(propertyTypeName, 0, propertyTypeNameLength);
            if (stream.ReadChar() != 0)
                throw new Exception("Invalid track data.");

            var propertyType = Utilities.Utils.GetType(e.PropertyTypeName);
            if (propertyType == null)
            {
                if (!string.IsNullOrEmpty(e.PropertyTypeName))
                    Editor.LogError("Cannot load track " + e.PropertyName + " of type " + e.PropertyTypeName + ". Failed to find the value type information.");
                return;
            }
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (ObjectPropertyTrack)track;

            var propertyName = e.PropertyName ?? string.Empty;
            var propertyNameData = Encoding.UTF8.GetBytes(propertyName);
            if (propertyNameData.Length != propertyName.Length)
                throw new Exception(string.Format("The object property name bytes data has different size as UTF8 bytes. Type {0}.", propertyName));

            var propertyTypeName = e.PropertyTypeName ?? string.Empty;
            var propertyTypeNameData = Encoding.UTF8.GetBytes(propertyTypeName);
            if (propertyTypeNameData.Length != propertyTypeName.Length)
                throw new Exception(string.Format("The object property typename bytes data has different size as UTF8 bytes. Type {0}.", propertyTypeName));

            stream.Write(e.ValueSize);
            stream.Write(propertyNameData.Length);
            stream.Write(propertyTypeNameData.Length);

            stream.Write(propertyNameData);
            stream.Write('\0');

            stream.Write(propertyTypeNameData);
            stream.Write('\0');
        }

        private Button _addButton;

        /// <inheritdoc />
        public ObjectPropertyTrack(ref TrackCreateOptions options)
        : base(ref options, false, false)
        {
            // Add track button
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

        /// <inheritdoc />
        public object Object
        {
            get
            {
                TryGetValue(out var value);
                return value;
            }
        }

        private void OnAddButtonClicked()
        {
            var menu = new ContextMenu.ContextMenu();

            var obj = Object;
            if (obj == null)
            {
                menu.AddButton("Missing object");
                return;
            }

            var type = obj.GetType();
            ObjectTrack.AddProperties(this, menu, type);

            menu.Show(_addButton.Parent, _addButton.BottomLeft);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _addButton = null;

            base.Dispose();
        }
    }
}
