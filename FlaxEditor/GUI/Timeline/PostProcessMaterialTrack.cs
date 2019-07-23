// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline media that represents a post-process material media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class PostProcessMaterialTrackMedia : Media
    {
        /// <summary>
        /// The material asset id.
        /// </summary>
        public Guid Material;
    }

    /// <summary>
    /// The timeline track that represents a post-process material playback.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class PostProcessMaterialTrack : Track
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 2,
                Name = "Post Process Material",
                Create = options => new PostProcessMaterialTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (PostProcessMaterialTrack)track;
            Guid id = new Guid(stream.ReadBytes(16));
            e.Material = FlaxEngine.Content.LoadAsync<MaterialBase>(ref id);
            var m = e.Media[0];
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (PostProcessMaterialTrack)track;
            var materialId = e.Material?.ID ?? Guid.Empty;

            stream.Write(materialId.ToByteArray());

            if (e.Media.Count != 0)
            {
                var m = e.Media[0];
                stream.Write(m.StartFrame);
                stream.Write(m.DurationFrames);
            }
            else
            {
                stream.Write(0);
                stream.Write(track.Timeline.DurationFrames);
            }
        }

        private readonly AssetPicker _picker;

        /// <summary>
        /// Gets or sets the material asset.
        /// </summary>
        public MaterialBase Material
        {
            get => Media.Count > 0 ? FlaxEngine.Content.LoadAsync<MaterialBase>(((PostProcessMaterialTrackMedia)Media[0]).Material) : null;
            set
            {
                PostProcessMaterialTrackMedia media;
                if (Media.Count == 0)
                {
                    media = new PostProcessMaterialTrackMedia
                    {
                        StartFrame = 0,
                        DurationFrames = Timeline?.DurationFrames ?? 60,
                    };
                    AddMedia(media);
                }
                else
                {
                    media = (PostProcessMaterialTrackMedia)Media[0];
                }
                var prev = media.Material;
                media.Material = value?.ID ?? Guid.Empty;
                if (prev != media.Material)
                {
                    _picker.SelectedAsset = value;
                    Timeline?.MarkAsEdited();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostProcessMaterialTrack"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public PostProcessMaterialTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            _picker = new AssetPicker(typeof(MaterialBase), Vector2.Zero)
            {
                Size = new Vector2(50.0f, 36.0f),
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            _picker.Location = new Vector2(Width - _picker.Width - 2, 2);
            _picker.SelectedItemChanged += OnPickerSelectedItemChanged;
            Height = 4 + _picker.Height;

            const float buttonSize = 14;
            var muteButton = new CheckBox(_picker.Left - buttonSize - 2.0f, 0, !Mute, buttonSize)
            {
                TooltipText = "Mute track",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Parent = this
            };
            muteButton.StateChanged += OnMuteButtonStateChanged;
        }

        private void OnMuteButtonStateChanged(CheckBox checkBox)
        {
            Mute = !checkBox.Checked;
            Timeline.MarkAsEdited();
        }

        private void OnPickerSelectedItemChanged()
        {
            Material = (MaterialBase)_picker.SelectedAsset;
        }
    }
}
