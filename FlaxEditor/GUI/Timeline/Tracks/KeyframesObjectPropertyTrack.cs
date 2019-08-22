// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
            LoadTrackBase(version, track, stream);

            var e = (KeyframesObjectPropertyTrack)track;
            int count = stream.ReadInt32();
            var keyframes = new KeyframesEditor.Keyframe[count];
            var dataBuffer = new byte[e.ValueSize];
            var propertyType = Utilities.Utils.GetType(e.PropertyTypeName);
            if (propertyType == null)
            {
                e.Keyframes.ResetKeyframes();
                stream.ReadBytes(count * (sizeof(float) + e.ValueSize));
                if (!string.IsNullOrEmpty(e.PropertyTypeName))
                    Editor.LogError("Cannot load track " + e.PropertyName + " of type " + e.PropertyTypeName + ". Failed to find the value type information.");
                return;
            }

            GCHandle handle = GCHandle.Alloc(dataBuffer, GCHandleType.Pinned);
            for (int i = 0; i < count; i++)
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
            SaveTrackBase(track, stream);

            var e = (KeyframesObjectPropertyTrack)track;
            var keyframes = e.Keyframes.Keyframes;
            int count = keyframes.Count;
            stream.Write(count);
            var dataBuffer = new byte[e.ValueSize];
            IntPtr ptr = Marshal.AllocHGlobal(e.ValueSize);
            for (int i = 0; i < count; i++)
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
        }

        private void UpdateCurve()
        {
            if (Keyframes == null)
                return;

            Keyframes.Bounds = new Rectangle(Timeline.StartOffset, Y + 1.0f, Timeline.Duration * Timeline.UnitsPerSecond * Timeline.Zoom, Height - 2.0f);
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
            // TODO: update preview value
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(PropertyInfo p)
        {
            base.OnPropertyChanged(p);

            Keyframes.ResetKeyframes();
            if (p != null)
            {
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
            base.OnTimelineChanged(timeline);

            Keyframes.Parent = timeline?.MediaPanel;
            Keyframes.FPS = timeline?.FramesPerSecond;
            UpdateCurve();
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
