// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating object property via keyframes collection.
    /// </summary>
    /// <seealso cref="PropertyTrack" />
    class KeyframesPropertyTrack : PropertyTrack
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
                Name = "Property",
                DisableSpawnViaGUI = true,
                Create = options => new KeyframesPropertyTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (KeyframesPropertyTrack)track;

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

            GCHandle handle = GCHandle.Alloc(dataBuffer, GCHandleType.Pinned);
            for (int i = 0; i < keyframesCount; i++)
            {
                var time = stream.ReadSingle();
                stream.Read(dataBuffer, 0, e.ValueSize);
                var value = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), propertyType);

                keyframes[i] = new KeyframesEditor.Keyframe
                {
                    Time = time,
                    Value = value,
                };
            }
            handle.Free();

            e.Keyframes.DefaultValue = e.GetDefaultValue(propertyType);
            e.Keyframes.SetKeyframes(keyframes);
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (KeyframesPropertyTrack)track;

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

            var dataBuffer = new byte[e.ValueSize];
            IntPtr ptr = Marshal.AllocHGlobal(e.ValueSize);
            for (int i = 0; i < keyframes.Count; i++)
            {
                var keyframe = keyframes[i];
                Marshal.StructureToPtr(keyframe.Value, ptr, true);
                Marshal.Copy(ptr, dataBuffer, 0, e.ValueSize);
                stream.Write(keyframe.Time);
                stream.Write(dataBuffer);
            }
            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// The keyframes editor.
        /// </summary>
        public KeyframesEditor Keyframes;

        /// <inheritdoc />
        public KeyframesPropertyTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            Height = 20.0f;

            // Keyframes editor
            Keyframes = new KeyframesEditor
            {
                EnableZoom = false,
                EnablePanning = false,
                ScrollBars = ScrollBars.None,
            };
            Keyframes.Edited += OnKeyframesEdited;
            Keyframes.UnlockChildrenRecursive();

            _addKey.Clicked += OnAddKeyClicked;
            _leftKey.Clicked += OnLeftKeyClicked;
            _rightKey.Clicked += OnRightKeyClicked;
        }

        private void OnRightKeyClicked(Image image, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                var time = Timeline.CurrentTime;
                for (int i = 0; i < Keyframes.Keyframes.Count; i++)
                {
                    var k = Keyframes.Keyframes[i];
                    if (k.Time > time)
                    {
                        Timeline.OnSeek(Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond));
                        break;
                    }
                }
            }
        }

        private void OnAddKeyClicked(Image image, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                // Evaluate a value
                var time = Timeline.CurrentTime;
                if (!TryGetValue(out var value))
                    value = Keyframes.Evaluate(time);

                // Find keyframe at the current location
                for (int i = Keyframes.Keyframes.Count - 1; i >= 0; i--)
                {
                    var k = Keyframes.Keyframes[i];
                    var frame = Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond);
                    if (frame == Timeline.CurrentFrame)
                    {
                        // Skip if value is the same
                        if (k.Value == value)
                            return;

                        // Update existing key value
                        Keyframes.SetKeyframe(i, value);
                        UpdatePreviewValue();
                        return;
                    }
                }

                // Add a new key
                Keyframes.AddKeyframe(new KeyframesEditor.Keyframe(time, value));
            }
        }

        private void OnLeftKeyClicked(Image image, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                var time = Timeline.CurrentTime;
                for (int i = Keyframes.Keyframes.Count - 1; i >= 0; i--)
                {
                    var k = Keyframes.Keyframes[i];
                    if (k.Time < time)
                    {
                        Timeline.OnSeek(Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond));
                        break;
                    }
                }
            }
        }

        private void UpdateKeyframes()
        {
            if (Keyframes == null)
                return;

            Keyframes.Bounds = new Rectangle(Timeline.StartOffset, Y + 1.0f, Timeline.Duration * Timeline.UnitsPerSecond * Timeline.Zoom, Height - 2.0f);
            Keyframes.ViewScale = new Vector2(Timeline.Zoom, 1.0f);
            Keyframes.Visible = Visible;
            Keyframes.UpdateKeyframes();
        }

        private void OnKeyframesEdited()
        {
            UpdatePreviewValue();
            Timeline.MarkAsEdited();
        }

        private void UpdatePreviewValue()
        {
            var time = Timeline.CurrentTime;
            var value = Keyframes.Evaluate(time);
            _previewValue.Text = GetValueText(value);
        }

        /// <summary>
        /// Gets the default value for the given property type.
        /// </summary>
        /// <param name="propertyType">The type of the property.</param>
        /// <returns>The default value.</returns>
        protected virtual object GetDefaultValue(Type propertyType)
        {
            return Activator.CreateInstance(propertyType);
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(MemberInfo value, Type type)
        {
            base.OnPropertyChanged(value, type);

            Keyframes.ResetKeyframes();
            if (type != null)
            {
                Keyframes.DefaultValue = GetDefaultValue(type);
            }
        }

        /// <inheritdoc />
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            Keyframes.Visible = Visible;
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Timeline timeline)
        {
            if (Timeline != null)
            {
                Timeline.ShowPreviewValuesChanged -= OnTimelineShowPreviewValuesChanged;
            }

            base.OnTimelineChanged(timeline);

            if (Timeline != null)
            {
                _previewValue.Visible = Timeline.ShowPreviewValues;
                Timeline.ShowPreviewValuesChanged += OnTimelineShowPreviewValuesChanged;
            }

            Keyframes.Parent = timeline?.MediaPanel;
            Keyframes.FPS = timeline?.FramesPerSecond;

            UpdateKeyframes();
            UpdatePreviewValue();
        }

        private void OnTimelineShowPreviewValuesChanged()
        {
            _previewValue.Visible = Timeline.ShowPreviewValues;
        }

        /// <inheritdoc />
        public override void OnTimelineZoomChanged()
        {
            base.OnTimelineZoomChanged();

            UpdateKeyframes();
        }

        /// <inheritdoc />
        public override void OnTimelineArrange()
        {
            base.OnTimelineArrange();

            UpdateKeyframes();
        }

        /// <inheritdoc />
        public override void OnTimelineFpsChanged(float before, float after)
        {
            base.OnTimelineFpsChanged(before, after);

            Keyframes.FPS = after;
            UpdatePreviewValue();
        }

        /// <inheritdoc />
        public override void OnTimelineCurrentFrameChanged(int frame)
        {
            base.OnTimelineCurrentFrameChanged(frame);

            UpdatePreviewValue();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (Keyframes != null)
            {
                Keyframes.Dispose();
                Keyframes = null;
            }

            base.Dispose();
        }
    }
}
