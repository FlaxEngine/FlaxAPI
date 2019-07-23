// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline media that represents a media event with an asset reference.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class SingleMediaAssetTrackMedia : Media
    {
        /// <summary>
        /// The asset id.
        /// </summary>
        public Guid Asset;
    }

    public abstract class SingleMediaAssetTrack<TAsset, TMediaEvent> : Track
    where TAsset : Asset
    where TMediaEvent : SingleMediaAssetTrackMedia, new()
    {
        /// <summary>
        /// The asset reference picker control.
        /// </summary>
        protected readonly AssetPicker _picker;

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        public TAsset Asset
        {
            get => FlaxEngine.Content.LoadAsync<TAsset>(TrackMedia.Asset);
            set
            {
                TMediaEvent media = TrackMedia;
                if (media.Asset == value?.ID)
                    return;

                media.Asset = value?.ID ?? Guid.Empty;
                _picker.SelectedAsset = value;
                Timeline?.MarkAsEdited();
            }
        }
        
        /// <summary>
        /// Gets the track media.
        /// </summary>
        public TMediaEvent TrackMedia
        {
            get
            {
                TMediaEvent media;
                if (Media.Count == 0)
                {
                    media = new TMediaEvent
                    {
                        StartFrame = 0,
                        DurationFrames = Timeline?.DurationFrames ?? 60,
                    };
                    AddMedia(media);
                }
                else
                {
                    media = (TMediaEvent)Media[0];
                }
                return media;
            }
        }
        
        /// <inheritdoc />
        protected SingleMediaAssetTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            _picker = new AssetPicker(typeof(SceneAnimation), Vector2.Zero)
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
            Asset = (TAsset)_picker.SelectedAsset;
        }
    }
}
