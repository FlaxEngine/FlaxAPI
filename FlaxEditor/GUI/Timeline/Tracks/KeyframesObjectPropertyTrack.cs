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
    /// <seealso cref="ObjectPropertyTrack" />
    public sealed class KeyframesObjectPropertyTrack : ObjectPropertyTrack
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
                Create = options => new KeyframesObjectPropertyTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (KeyframesObjectPropertyTrack)track;

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

            e.Keyframes.DefaultValue = Activator.CreateInstance(propertyType);
            e.Keyframes.SetKeyframes(keyframes);
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (KeyframesObjectPropertyTrack)track;

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

        private Label _previewValue;

        /// <inheritdoc />
        public KeyframesObjectPropertyTrack(ref TrackCreateOptions options)
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

            // Navigation buttons
            const float buttonSize = 14;
            var icons = Editor.Instance.Icons;
            var rightKey = new Image(_muteCheckbox.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                TooltipText = "Sets the time to the next key",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.8f),
                Margin = new Margin(1),
                Brush = new SpriteBrush(icons.ArrowRight32),
                Parent = this
            };
            rightKey.Clicked += OnRightKeyClicked;
            var addKey = new Image(rightKey.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                TooltipText = "Adds a new key at the current time",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.8f),
                Margin = new Margin(3),
                Brush = new SpriteBrush(icons.Add48),
                Parent = this
            };
            addKey.Clicked += OnAddKeyClicked;
            var leftKey = new Image(addKey.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                TooltipText = "Sets the time to the previous key",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.8f),
                Margin = new Margin(1),
                Brush = new SpriteBrush(icons.ArrowLeft32),
                Parent = this
            };
            leftKey.Clicked += OnLeftKeyClicked;

            // Value preview
            var previewWidth = 100.0f;
            _previewValue = new Label(leftKey.Left - previewWidth - 2.0f, 0, previewWidth, TextBox.DefaultHeight)
            {
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                AutoFitTextRange = new Vector2(0.01f, 1.0f),
                AutoFitText = true,
                TextColor = new Color(0.8f),
                Margin = new Margin(1),
                Parent = this
            };
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
                var time = Timeline.CurrentTime;
                for (int i = Keyframes.Keyframes.Count - 1; i >= 0; i--)
                {
                    var k = Keyframes.Keyframes[i];
                    var frame = Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond);
                    if (frame == Timeline.CurrentFrame)
                    {
                        // Already added
                        return;
                    }
                }

                var value = Keyframes.Evaluate(time);
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
            _previewValue.Text = value?.ToString() ?? string.Empty;
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(PropertyInfo p)
        {
            base.OnPropertyChanged(p);

            Keyframes.ResetKeyframes();
            if (p != null)
            {
                // TODO: pick the default value from property attribute via DefaultValueAttribute if available
                Keyframes.DefaultValue = Activator.CreateInstance(p.PropertyType);
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
