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
    /// The timeline track for animating object property via Bezier Curve.
    /// </summary>
    /// <seealso cref="ObjectPropertyTrack" />
    public sealed class CurveObjectPropertyTrack : ObjectPropertyTrack
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 10,
                Name = "Object Property",
                DisableSpawnViaGUI = true,
                Create = options => new CurveObjectPropertyTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (CurveObjectPropertyTrack)track;

            if (e.Curve != null)
            {
                e.Curve.Dispose();
                e.Curve = null;
            }

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

            var keyframes = new Curve<object>.Keyframe[keyframesCount];
            var dataBuffer = new byte[e.ValueSize];
            var propertyType = Utilities.Utils.GetType(e.PropertyTypeName);
            if (propertyType == null)
            {
                stream.ReadBytes(keyframesCount * (sizeof(float) + e.ValueSize * 3));
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

                stream.Read(dataBuffer, 0, e.ValueSize);
                var tangentIn = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), propertyType);

                stream.Read(dataBuffer, 0, e.ValueSize);
                var tangentOut = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), propertyType);

                keyframes[i] = new Curve<object>.Keyframe
                {
                    Time = time,
                    Value = value,
                    TangentIn = tangentIn,
                    TangentOut = tangentOut,
                };
            }
            handle.Free();

            e.CreateCurve(propertyType);
            e.Curve.SetKeyframes(keyframes);
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (CurveObjectPropertyTrack)track;

            var propertyName = e.PropertyName ?? string.Empty;
            var propertyNameData = Encoding.UTF8.GetBytes(propertyName);
            if (propertyNameData.Length != propertyName.Length)
                throw new Exception(string.Format("The object property name bytes data has different size as UTF8 bytes. Type {0}.", propertyName));

            var propertyTypeName = e.PropertyTypeName ?? string.Empty;
            var propertyTypeNameData = Encoding.UTF8.GetBytes(propertyTypeName);
            if (propertyTypeNameData.Length != propertyTypeName.Length)
                throw new Exception(string.Format("The object property typename bytes data has different size as UTF8 bytes. Type {0}.", propertyTypeName));

            var keyframes = e.Curve.GetKeyframes();

            stream.Write(e.ValueSize);
            stream.Write(propertyNameData.Length);
            stream.Write(propertyTypeNameData.Length);
            stream.Write(keyframes.Length);

            stream.Write(propertyNameData);
            stream.Write('\0');

            stream.Write(propertyTypeNameData);
            stream.Write('\0');

            var dataBuffer = new byte[e.ValueSize];
            IntPtr ptr = Marshal.AllocHGlobal(e.ValueSize);
            for (int i = 0; i < keyframes.Length; i++)
            {
                var keyframe = keyframes[i];
                stream.Write(keyframe.Time);

                Marshal.StructureToPtr(keyframe.Value, ptr, true);
                Marshal.Copy(ptr, dataBuffer, 0, e.ValueSize);
                stream.Write(dataBuffer);

                Marshal.StructureToPtr(keyframe.TangentIn, ptr, true);
                Marshal.Copy(ptr, dataBuffer, 0, e.ValueSize);
                stream.Write(dataBuffer);

                Marshal.StructureToPtr(keyframe.TangentOut, ptr, true);
                Marshal.Copy(ptr, dataBuffer, 0, e.ValueSize);
                stream.Write(dataBuffer);
            }
            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// The curve editor.
        /// </summary>
        public CurveEditorBase Curve;

        private Label _previewValue;

        /// <inheritdoc />
        public CurveObjectPropertyTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            Height = 20.0f;

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
            var previewWidth = 50.0f;
            _previewValue = new Label(leftKey.Left - previewWidth - 2.0f, 0, previewWidth, TextBox.DefaultHeight)
            {
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                HorizontalAlignment = TextAlignment.Near,
                TextColor = new Color(0.8f),
                Margin = new Margin(1),
                Parent = this
            };
        }

        private void OnRightKeyClicked(Image image, MouseButton button)
        {
            if (button == MouseButton.Left && Curve != null)
            {
                var time = Timeline.CurrentTime;
                var keyframes = Curve.GetKeyframes();
                for (int i = 0; i < keyframes.Length; i++)
                {
                    var k = keyframes[i];
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
            if (button == MouseButton.Left && Curve != null)
            {
                var time = Timeline.CurrentTime;
                var keyframes = Curve.GetKeyframes();
                for (int i = keyframes.Length - 1; i >= 0; i--)
                {
                    var k = keyframes[i];
                    var frame = Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond);
                    if (frame == Timeline.CurrentFrame)
                    {
                        // Already added
                        return;
                    }
                }

                Curve.Evaluate(out var value, time);
                Curve.AddKeyframe(time, value);
            }
        }

        private void OnLeftKeyClicked(Image image, MouseButton button)
        {
            if (button == MouseButton.Left && Curve != null)
            {
                var time = Timeline.CurrentTime;
                var keyframes = Curve.GetKeyframes();
                for (int i = keyframes.Length - 1; i >= 0; i--)
                {
                    var k = keyframes[i];
                    if (k.Time < time)
                    {
                        Timeline.OnSeek(Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond));
                        break;
                    }
                }
            }
        }

        private void UpdateCurve()
        {
            if (Curve == null || Timeline == null)
                return;

            Curve.Bounds = new Rectangle(Timeline.StartOffset, Y + 1.0f, Timeline.Duration * Timeline.UnitsPerSecond * Timeline.Zoom, Height - 2.0f);
            Curve.Visible = Visible;
            Curve.UpdateKeyframes();
        }

        private void OnKeyframesEdited()
        {
            UpdatePreviewValue();
            Timeline.MarkAsEdited();
        }

        private void UpdatePreviewValue()
        {
            if (Curve == null)
            {
                _previewValue.Text = string.Empty;
                return;
            }

            var time = Timeline.CurrentTime;
            Curve.Evaluate(out var value, time);
            _previewValue.Text = value?.ToString() ?? string.Empty;
        }

        private void CreateCurve(Type propertyType)
        {
            var curveEditorType = typeof(CurveEditor<>).MakeGenericType(propertyType);
            Curve = (CurveEditorBase)Activator.CreateInstance(curveEditorType);
            Curve.EnableZoom = false;
            Curve.EnablePanning = false;
            Curve.ScrollBars = ScrollBars.None;
            Curve.Parent = Timeline?.MediaPanel;
            Curve.FPS = Timeline?.FramesPerSecond;
            Curve.Edited += OnKeyframesEdited;
            Curve.UnlockChildrenRecursive();
            UpdateCurve();
        }

        private void DisposeCurve()
        {
            if (Curve == null)
                return;

            Curve.Edited -= OnKeyframesEdited;
            Curve.Dispose();
            Curve = null;
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(PropertyInfo p)
        {
            base.OnPropertyChanged(p);

            DisposeCurve();

            if (p != null)
            {
                CreateCurve(p.PropertyType);
            }
        }

        /// <inheritdoc />
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            if (Curve != null)
            {
                Curve.Visible = Visible;
            }
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Timeline timeline)
        {
            base.OnTimelineChanged(timeline);

            if (Curve != null)
            {
                Curve.Parent = timeline?.MediaPanel;
                Curve.FPS = timeline?.FramesPerSecond;
                UpdateCurve();
            }
            UpdatePreviewValue();
        }

        /// <inheritdoc />
        public override void OnTimelineZoomChanged()
        {
            base.OnTimelineZoomChanged();

            UpdateCurve();
        }

        /// <inheritdoc />
        public override void OnTimelineArrange()
        {
            base.OnTimelineArrange();

            UpdateCurve();
        }

        /// <inheritdoc />
        public override void OnTimelineFpsChanged(float before, float after)
        {
            base.OnTimelineFpsChanged(before, after);

            if (Curve != null)
            {
                Curve.FPS = after;
            }
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
            DisposeCurve();

            base.Dispose();
        }
    }
}
