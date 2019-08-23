// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating object property (managed object).
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public abstract class ObjectPropertyTrack : Track
    {
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
