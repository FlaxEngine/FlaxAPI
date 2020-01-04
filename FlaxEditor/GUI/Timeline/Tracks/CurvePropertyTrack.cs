// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

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
    /// <seealso cref="MemberTrack" />
    public sealed class CurvePropertyTrack : MemberTrack
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
                Name = "Property",
                DisableSpawnViaGUI = true,
                Create = options => new CurvePropertyTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (CurvePropertyTrack)track;

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
            e.MemberName = Encoding.UTF8.GetString(propertyName, 0, propertyNameLength);
            if (stream.ReadChar() != 0)
                throw new Exception("Invalid track data.");

            var propertyTypeName = stream.ReadBytes(propertyTypeNameLength);
            e.MemberTypeName = Encoding.UTF8.GetString(propertyTypeName, 0, propertyTypeNameLength);
            if (stream.ReadChar() != 0)
                throw new Exception("Invalid track data.");

            var keyframes = new Curve<object>.Keyframe[keyframesCount];
            var dataBuffer = new byte[e.ValueSize];
            var propertyType = Utilities.Utils.GetType(e.MemberTypeName);
            if (propertyType == null)
            {
                stream.ReadBytes(keyframesCount * (sizeof(float) + e.ValueSize * 3));
                if (!string.IsNullOrEmpty(e.MemberTypeName))
                    Editor.LogError("Cannot load track " + e.MemberName + " of type " + e.MemberTypeName + ". Failed to find the value type information.");
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
            var e = (CurvePropertyTrack)track;

            var propertyName = e.MemberName ?? string.Empty;
            var propertyNameData = Encoding.UTF8.GetBytes(propertyName);
            if (propertyNameData.Length != propertyName.Length)
                throw new Exception(string.Format("The object member name bytes data has different size as UTF8 bytes. Type {0}.", propertyName));

            var propertyTypeName = e.MemberTypeName ?? string.Empty;
            var propertyTypeNameData = Encoding.UTF8.GetBytes(propertyTypeName);
            if (propertyTypeNameData.Length != propertyTypeName.Length)
                throw new Exception(string.Format("The object member typename bytes data has different size as UTF8 bytes. Type {0}.", propertyTypeName));

            var keyframes = e.Curve?.GetKeyframes() ?? Utils.GetEmptyArray<Curve<object>.Keyframe>();

            stream.Write(e.ValueSize);
            stream.Write(propertyNameData.Length);
            stream.Write(propertyTypeNameData.Length);
            stream.Write(keyframes.Length);

            stream.Write(propertyNameData);
            stream.Write('\0');

            stream.Write(propertyTypeNameData);
            stream.Write('\0');

            if (keyframes.Length == 0)
                return;
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

        private const float CollapsedHeight = 20.0f;
        private const float ExpandedHeight = 120.0f;

        /// <inheritdoc />
        public CurvePropertyTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            Height = CollapsedHeight;

            _addKey.Clicked += OnAddKeyClicked;
            _leftKey.Clicked += OnLeftKeyClicked;
            _rightKey.Clicked += OnRightKeyClicked;
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
                // Evaluate a value
                var time = Timeline.CurrentTime;
                if (!TryGetValue(out var value))
                    Curve.Evaluate(out value, time);

                // Find keyframe at the current location
                var keyframes = Curve.GetKeyframes();
                for (int i = keyframes.Length - 1; i >= 0; i--)
                {
                    var k = keyframes[i];
                    var frame = Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond);
                    if (frame == Timeline.CurrentFrame)
                    {
                        // Skip if value is the same
                        if (k.Value == value)
                            return;

                        // Update existing key value
                        Curve.SetKeyframe(i, value);
                        UpdatePreviewValue();
                        return;
                    }
                }

                // Add a new key
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
            var expanded = IsExpanded;
            Curve.ViewScale = new Vector2(Timeline.Zoom, Curve.ViewScale.Y);
            Curve.ShowCollapsed = !expanded;
            Curve.ShowBackground = expanded;
            Curve.ShowAxes = expanded;
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
            _previewValue.Text = GetValueText(value);
        }

        private void CreateCurve(Type propertyType)
        {
            var curveEditorType = typeof(CurveEditor<>).MakeGenericType(propertyType);
            Curve = (CurveEditorBase)Activator.CreateInstance(curveEditorType);
            Curve.EnableZoom = CurveEditorBase.UseMode.Vertical;
            Curve.EnablePanning = CurveEditorBase.UseMode.Vertical;
            Curve.ScrollBars = ScrollBars.Vertical;
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
        public override object Evaluate(float time)
        {
            if (Curve != null)
            {
                Curve.Evaluate(out var result, time);
                return result;
            }

            return base.Evaluate(time);
        }

        /// <inheritdoc />
        protected override bool CanExpand => true;

        /// <inheritdoc />
        protected override void OnMemberChanged(MemberInfo value, Type type)
        {
            base.OnMemberChanged(value, type);

            DisposeCurve();

            if (type != null)
            {
                CreateCurve(type);
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
        protected override void OnExpandedChanged()
        {
            Height = IsExpanded ? ExpandedHeight : CollapsedHeight;
            UpdateCurve();

            base.OnExpandedChanged();
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

            if (Curve != null)
            {
                Curve.Parent = timeline?.MediaPanel;
                Curve.FPS = timeline?.FramesPerSecond;
                UpdateCurve();
            }
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
        public override void OnDestroy()
        {
            DisposeCurve();

            base.OnDestroy();
        }
    }
}
