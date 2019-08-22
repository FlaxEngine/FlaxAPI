// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating object property (managed object).
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public abstract class ObjectPropertyTrack : Track
    {
        /// <summary>
        /// Loads the track.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="track">The track.</param>
        /// <param name="stream">The stream.</param>
        protected static void LoadTrackBase(int version, Track track, BinaryReader stream)
        {
            var e = (ObjectPropertyTrack)track;

            int propertyNameLength = stream.ReadInt32();
            var propertyName = stream.ReadBytes(propertyNameLength);
            e.PropertyName = Encoding.UTF8.GetString(propertyName, 0, propertyNameLength);

            int propertyTypeNameLength = stream.ReadInt32();
            var propertyTypeName = stream.ReadBytes(propertyTypeNameLength);
            e.PropertyTypeName = Encoding.UTF8.GetString(propertyTypeName, 0, propertyTypeNameLength);

            e.ValueSize = stream.ReadInt32();
        }

        /// <summary>
        /// Saves the track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="stream">The stream.</param>
        protected static void SaveTrackBase(Track track, BinaryWriter stream)
        {
            var e = (ObjectPropertyTrack)track;

            var propertyName = e.PropertyName ?? string.Empty;
            var propertyNameData = Encoding.UTF8.GetBytes(propertyName);
            if (propertyNameData.Length != propertyName.Length)
                throw new Exception(string.Format("The object property name bytes data has different size as UTF8 bytes. Type {0}.", propertyName));
            stream.Write(propertyNameData.Length);
            stream.Write(propertyNameData);

            var propertyTypeName = e.PropertyTypeName ?? string.Empty;
            var propertyTypeNameData = Encoding.UTF8.GetBytes(propertyTypeName);
            if (propertyTypeNameData.Length != propertyTypeName.Length)
                throw new Exception(string.Format("The object property typename bytes data has different size as UTF8 bytes. Type {0}.", propertyTypeName));
            stream.Write(propertyTypeNameData.Length);
            stream.Write(propertyTypeNameData);

            stream.Write(e.ValueSize);
        }

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
        /// Gets or sets the object property. Performs the value validation on set.
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

                if (value != null)
                {
                    var type = value.PropertyType;
                    PropertyName = value.Name;
                    PropertyTypeName = type.FullName;
                    ValueSize = Marshal.SizeOf(type);
                }
                else
                {
                    PropertyName = string.Empty;
                    PropertyTypeName = string.Empty;
                    ValueSize = 0;
                }

                OnPropertyChanged(value);
            }
        }

        /// <inheritdoc />
        protected ObjectPropertyTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }

        /// <inheritdoc />
        protected override bool CanDrag => false;

        /// <inheritdoc />
        protected override bool CanRename => false;

        /// <summary>
        /// Called when property gets changed.
        /// </summary>
        /// <param name="p">The property value assigned.</param>
        protected virtual void OnPropertyChanged(PropertyInfo p)
        {
        }
    }
}
