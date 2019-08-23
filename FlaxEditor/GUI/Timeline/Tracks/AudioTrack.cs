// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline media that represents an audio clip media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class AudioMedia : SingleMediaAssetMedia
    {
        private bool _loop;

        /// <summary>
        /// True if loop track, otherwise audio clip will stop on the end.
        /// </summary>
        public bool Loop
        {
            get => _loop;
            set
            {
                if (_loop != value)
                {
                    _loop = value;
                    Preview.DrawMode = value ? AudioClipPreview.DrawModes.Looped : AudioClipPreview.DrawModes.Single;
                }
            }
        }

        private sealed class Proxy : ProxyBase<AudioTrack, AudioMedia>
        {
            /// <summary>
            /// Gets or sets the audio clip to play.
            /// </summary>
            [EditorDisplay("General"), EditorOrder(10), Tooltip("The audio clip to play.")]
            public AudioClip Audio
            {
                get => Track.Asset;
                set => Track.Asset = value;
            }

            /// <summary>
            /// Gets or sets the audio clip looping mode.
            /// </summary>
            [EditorDisplay("General"), EditorOrder(20), Tooltip("If checked, the audio clip will loop when playback exceeds its duration. Otherwise it will stop play.")]
            public bool Loop
            {
                get => Track.Loop;
                set => Track.Loop = value;
            }

            /// <inheritdoc />
            public Proxy(AudioTrack track, AudioMedia media)
            : base(track, media)
            {
            }
        }

        /// <summary>
        /// The audio clip preview.
        /// </summary>
        public AudioClipPreview Preview;

        /// <inheritdoc />
        public AudioMedia()
        {
            Preview = new AudioClipPreview
            {
                DockStyle = FlaxEngine.GUI.DockStyle.Fill,
                DrawMode = AudioClipPreview.DrawModes.Single,
                Parent = this,
            };
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Track track)
        {
            base.OnTimelineChanged(track);

            PropertiesEditObject = new Proxy(Track as AudioTrack, this);
        }

        /// <inheritdoc />
        public override void OnTimelineZoomChanged()
        {
            base.OnTimelineZoomChanged();

            Preview.ViewScale = Timeline.UnitsPerSecond / AudioClipPreview.UnitsPerSecond * Timeline.Zoom;
        }
    }

    /// <summary>
    /// The timeline track that represents an audio clip playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class AudioTrack : SingleMediaAssetTrack<AudioClip, AudioMedia>
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 5,
                Name = "Audio",
                Create = options => new AudioTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (AudioTrack)track;
            Guid id = new Guid(stream.ReadBytes(16));
            e.Asset = FlaxEngine.Content.LoadAsync<AudioClip>(ref id);
            var m = e.TrackMedia;
            var tmp = stream.ReadInt32();
            m.Loop = (tmp & 1) == 1;
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (AudioTrack)track;
            var assetId = e.Asset?.ID ?? Guid.Empty;

            stream.Write(assetId.ToByteArray());

            if (e.Media.Count != 0)
            {
                var m = e.TrackMedia;
                var tmp = 0;
                if (e.Loop)
                    tmp |= 1;
                stream.Write(tmp);
                stream.Write(m.StartFrame);
                stream.Write(m.DurationFrames);
            }
            else
            {
                stream.Write(0);
                stream.Write(0);
                stream.Write(track.Timeline.DurationFrames);
            }
        }

        /// <summary>
        /// Gets or sets the audio clip looping mode.
        /// </summary>
        public bool Loop
        {
            get => TrackMedia.Loop;
            set
            {
                AudioMedia media = TrackMedia;
                if (media.Loop == value)
                    return;

                media.Loop = value;
                Timeline?.MarkAsEdited();
            }
        }

        private Button _addButton;

        /// <inheritdoc />
        public AudioTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            // Add button
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
            _picker.Location = new Vector2(_addButton.Left - _picker.Width - 2, 2);
        }

        private void OnAddButtonClicked()
        {
            var cm = new ContextMenu.ContextMenu();
            cm.AddButton("Volume", OnAddVolumeTrack);
            cm.Show(_addButton.Parent, _addButton.BottomLeft);
        }

        private void OnAddVolumeTrack()
        {
            var track = Timeline.AddTrack(AudioVolumeTrack.GetArchetype());
            track.ParentTrack = this;
            track.TrackIndex = TrackIndex + 1;
            track.Name = Guid.NewGuid().ToString();
            Timeline.OnTracksOrderChanged();
            Timeline.MarkAsEdited();
            Expand();
        }

        /// <inheritdoc />
        protected override void OnSubTracksChanged()
        {
            base.OnSubTracksChanged();

            _addButton.Enabled = SubTracks.Count == 0;
        }

        /// <inheritdoc />
        protected override void OnAssetChanged()
        {
            base.OnAssetChanged();

            TrackMedia.Preview.Asset = Asset;
        }
    }

    /// <summary>
    /// The child volume track for audio track. Used to animate audio volume over time.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class AudioVolumeTrack : Track
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 6,
                Name = "Audio Volume",
                DisableSpawnViaGUI = true,
                Create = options => new AudioVolumeTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (AudioVolumeTrack)track;
            int count = stream.ReadInt32();
            var keyframes = new Curve<float>.Keyframe[count];
            for (int i = 0; i < count; i++)
            {
                keyframes[i] = new Curve<float>.Keyframe
                {
                    Time = stream.ReadSingle(),
                    Value = stream.ReadSingle(),
                    TangentIn = stream.ReadSingle(),
                    TangentOut = stream.ReadSingle(),
                };
            }
            e.Curve.SetKeyframes(keyframes);
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (AudioVolumeTrack)track;
            var keyframes = e.Curve.Keyframes;
            int count = keyframes.Count;
            stream.Write(count);
            for (int i = 0; i < count; i++)
            {
                var keyframe = keyframes[i];
                stream.Write(keyframe.Time);
                stream.Write(keyframe.Value);
                stream.Write(keyframe.TangentIn);
                stream.Write(keyframe.TangentOut);
            }
        }

        /// <summary>
        /// The volume curve. Values can be in range 0-1 to animate volume intensity and the track playback starts at the parent audio track media beginning. This curve does not loop.
        /// </summary>
        public CurveEditor<float> Curve;

        private AudioMedia _audioMedia;
        private const float CollapsedHeight = 20.0f;
        private const float ExpandedHeight = 64.0f;
        private Label _previewValue;

        /// <inheritdoc />
        public AudioVolumeTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            Title = "Volume";
            Height = CollapsedHeight;

            // Curve editor
            Curve = new CurveEditor<float>
            {
                Visible = false,
                EnableZoom = false,
                EnablePanning = false,
                ScrollBars = ScrollBars.None,
                DefaultValue = 1.0f,
                ShowStartEndLines = true,
            };
            Curve.Edited += OnCurveEdited;
            Curve.UnlockChildrenRecursive();

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
            if (button == MouseButton.Left && _audioMedia != null)
            {
                var time = (Timeline.CurrentFrame - _audioMedia.StartFrame) / Timeline.FramesPerSecond;
                for (int i = 0; i < Curve.Keyframes.Count; i++)
                {
                    var k = Curve.Keyframes[i];
                    if (k.Time > time)
                    {
                        Timeline.OnSeek(Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond) + _audioMedia.StartFrame);
                        break;
                    }
                }
            }
        }

        private void OnAddKeyClicked(Image image, MouseButton button)
        {
            var currentFrame = Timeline.CurrentFrame;
            if (button == MouseButton.Left && _audioMedia != null && currentFrame >= _audioMedia.StartFrame && currentFrame < _audioMedia.StartFrame + _audioMedia.DurationFrames)
            {
                var time = (currentFrame - _audioMedia.StartFrame) / Timeline.FramesPerSecond;
                for (int i = Curve.Keyframes.Count - 1; i >= 0; i--)
                {
                    var k = Curve.Keyframes[i];
                    var frame = Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond) + _audioMedia.StartFrame;
                    if (frame == Timeline.CurrentFrame)
                    {
                        // Already added
                        return;
                    }
                }

                Curve.AddKeyframe(new Curve<float>.Keyframe(time, 1.0f));
            }
        }

        private void OnLeftKeyClicked(Image image, MouseButton button)
        {
            if (button == MouseButton.Left && _audioMedia != null)
            {
                var time = (Timeline.CurrentFrame - _audioMedia.StartFrame) / Timeline.FramesPerSecond;
                for (int i = Curve.Keyframes.Count - 1; i >= 0; i--)
                {
                    var k = Curve.Keyframes[i];
                    if (k.Time < time)
                    {
                        Timeline.OnSeek(Mathf.FloorToInt(k.Time * Timeline.FramesPerSecond) + _audioMedia.StartFrame);
                        break;
                    }
                }
            }
        }

        private void UpdatePreviewValue()
        {
            if (_audioMedia == null || Curve == null)
                return;

            var time = (Timeline.CurrentFrame - _audioMedia.StartFrame) / Timeline.FramesPerSecond;
            Curve.Evaluate(out var value, time, false);
            _previewValue.Text = Utils.RoundTo2DecimalPlaces(Mathf.Saturate(value)).ToString("0.00");
        }

        private void UpdateCurve()
        {
            if (_audioMedia == null || Curve == null)
                return;

            Curve.Bounds = new Rectangle(_audioMedia.X, Y + 1.0f, _audioMedia.Width, Height - 2.0f);

            var expanded = IsExpanded;
            if (expanded)
            {
                //Curve.ViewScale = new Vector2(1.0f, CurveEditor<float>.UnitsPerSecond / Curve.Height);
                Curve.ViewScale = new Vector2(Timeline.Zoom, 0.4f);
                Curve.ViewOffset = new Vector2(0.0f, 30.0f);
            }
            else
            {
                Curve.ViewScale = Vector2.One;
                Curve.ViewOffset = Vector2.Zero;
            }
            Curve.ShowCollapsed = !expanded;
            Curve.ShowBackground = expanded;
            Curve.ShowAxes = expanded;
            Curve.Visible = Visible;
            Curve.UpdateKeyframes();
        }

        private void OnCurveEdited()
        {
            UpdatePreviewValue();
            Timeline.MarkAsEdited();
        }

        /// <inheritdoc />
        protected override bool CanDrag => false;

        /// <inheritdoc />
        protected override bool CanRename => false;

        /// <inheritdoc />
        protected override bool CanExpand => true;

        /// <inheritdoc />
        public override void OnParentTrackChanged(Track parent)
        {
            base.OnParentTrackChanged(parent);

            if (_audioMedia != null)
            {
                _audioMedia.StartFrameChanged -= UpdateCurve;
                _audioMedia.DurationFramesChanged -= UpdateCurve;
                _audioMedia = null;
            }

            if (parent is AudioTrack audioTrack)
            {
                var media = audioTrack.TrackMedia;
                media.StartFrameChanged += UpdateCurve;
                media.DurationFramesChanged += UpdateCurve;
                _audioMedia = media;
                UpdateCurve();
                UpdatePreviewValue();
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
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            Curve.Visible = Visible;
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Timeline timeline)
        {
            base.OnTimelineChanged(timeline);

            Curve.Parent = timeline?.MediaPanel;
            Curve.FPS = timeline?.FramesPerSecond;
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

            Curve.FPS = after;
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
            if (Curve != null)
            {
                Curve.Dispose();
                Curve = null;
            }

            base.Dispose();
        }
    }
}
