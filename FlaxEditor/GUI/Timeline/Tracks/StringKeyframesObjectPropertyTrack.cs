// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating object string property via keyframes collection.
    /// </summary>
    /// <seealso cref="ObjectPropertyTrack" />
    /// <seealso cref="KeyframesObjectPropertyTrack" />
    public sealed class StringKeyframesObjectPropertyTrack : KeyframesObjectPropertyTrack
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 11,
                Name = "Object Property",
                DisableSpawnViaGUI = true,
                Create = options => new StringKeyframesObjectPropertyTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (StringKeyframesObjectPropertyTrack)track;

            e.ValueSize = stream.ReadInt32();
            int propertyNameLength = stream.ReadInt32();
            int propertyTypeNameLength = stream.ReadInt32();
            int keyframesCount = stream.ReadInt32();

            var propertyName = stream.ReadBytes(propertyNameLength);
            e.PropertyName = Encoding.UTF8.GetString(propertyName, 0, propertyNameLength);
            if (stream.ReadChar() != 0)
                throw new Exception("Invalid track data.");

            var propertyTypeName = stream.ReadBytes(propertyTypeNameLength);
            e.PropertyTypeName = Encoding.UTF8.GetString(propertyTypeName, 0, propertyTypeNameLength);
            if (stream.ReadChar() != 0)
                throw new Exception("Invalid track data.");

            var keyframes = new KeyframesEditor.Keyframe[keyframesCount];
            var dataBuffer = new byte[e.ValueSize];
            var propertyType = Utilities.Utils.GetType(e.PropertyTypeName);
            if (propertyType == null)
            {
                e.Keyframes.ResetKeyframes();
                stream.ReadBytes(keyframesCount * (sizeof(float) + e.ValueSize));
                if (!string.IsNullOrEmpty(e.PropertyTypeName))
                    Editor.LogError("Cannot load track " + e.PropertyName + " of type " + e.PropertyTypeName + ". Failed to find the value type information.");
                return;
            }

            for (int i = 0; i < keyframesCount; i++)
            {
                var time = stream.ReadSingle();
                var length = stream.ReadInt32();
                var data = stream.ReadBytes(length * 2);
                var value = Encoding.Unicode.GetString(data, 0, data.Length);

                keyframes[i] = new KeyframesEditor.Keyframe
                {
                    Time = time,
                    Value = value,
                };
            }

            e.Keyframes.DefaultValue = string.Empty;
            e.Keyframes.SetKeyframes(keyframes);
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (StringKeyframesObjectPropertyTrack)track;

            var propertyName = e.PropertyName ?? string.Empty;
            var propertyNameData = Encoding.UTF8.GetBytes(propertyName);
            if (propertyNameData.Length != propertyName.Length)
                throw new Exception(string.Format("The object property name bytes data has different size as UTF8 bytes. Type {0}.", propertyName));

            var propertyTypeName = e.PropertyTypeName ?? string.Empty;
            var propertyTypeNameData = Encoding.UTF8.GetBytes(propertyTypeName);
            if (propertyTypeNameData.Length != propertyTypeName.Length)
                throw new Exception(string.Format("The object property typename bytes data has different size as UTF8 bytes. Type {0}.", propertyTypeName));

            var keyframes = e.Keyframes.Keyframes;

            stream.Write(e.ValueSize);
            stream.Write(propertyNameData.Length);
            stream.Write(propertyTypeNameData.Length);
            stream.Write(keyframes.Count);

            stream.Write(propertyNameData);
            stream.Write('\0');

            stream.Write(propertyTypeNameData);
            stream.Write('\0');

            for (int i = 0; i < keyframes.Count; i++)
            {
                var keyframe = keyframes[i];
                stream.Write(keyframe.Time);
                if (keyframe.Value is string value)
                {
                    var valueData = Encoding.Unicode.GetBytes(value);
                    stream.Write(value.Length);
                    if (valueData.Length != value.Length * 2)
                        throw new Exception("Invalid string value data length.");
                    stream.Write(valueData);
                }
                else
                {
                    stream.Write(0);
                }
            }
        }

        /// <inheritdoc />
        public StringKeyframesObjectPropertyTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(PropertyInfo p)
        {
            //base.OnPropertyChanged(p);

            Keyframes.ResetKeyframes();
            if (p != null)
            {
                // TODO: pick the default value from property attribute via DefaultValueAttribute if available
                Keyframes.DefaultValue = string.Empty;
            }
        }

        /// <inheritdoc />
        protected override bool TryGetValue(out object value)
        {
            if (base.TryGetValue(out value))
            {
                if (value == null)
                    value = string.Empty;
                return true;
            }
            return false;
        }
    }
}
