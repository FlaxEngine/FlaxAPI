// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating object property (managed object).
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public sealed class ObjectPropertyTrack : Track
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 9,
                Name = "Object Property",
                DisableSpawnViaGUI = true,
                Create = options => new ObjectPropertyTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (ObjectPropertyTrack)track;
            int propertyNameLength = stream.ReadInt32();
            var propertyName = stream.ReadBytes(propertyNameLength);
            e.PropertyName = Encoding.UTF8.GetString(propertyName, 0, propertyNameLength);
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (ObjectPropertyTrack)track;
            var propertyName = e.PropertyName ?? string.Empty;
            var propertyNameData = Encoding.UTF8.GetBytes(propertyName);
            if (propertyNameData.Length != propertyName.Length)
                throw new Exception(string.Format("The object property typename bytes data has different size as UTF8 bytes. Type {0}.", e.PropertyName));
            stream.Write(propertyNameData.Length);
            stream.Write(propertyNameData);
        }

        /// <summary>
        /// Gets or sets the object property name. Does not validate the value.
        /// </summary>
        public string PropertyName
        {
            get => Title;
            set => Title = value;
        }

        /// <summary>
        /// Gets or sets the object property. Performs the value validation.
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

                PropertyName = value?.Name ?? Name;
            }
        }

        /// <inheritdoc />
        public ObjectPropertyTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }

        /// <inheritdoc />
        protected override bool CanDrag => false;

        /// <inheritdoc />
        protected override bool CanRename => false;
    }
}
