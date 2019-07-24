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
    public class SingleMediaAssetMedia : Media
    {
        /// <summary>
        /// The asset id.
        /// </summary>
        public Guid Asset;
    }

    /// <summary>
    /// The base class for timeline tracks that use single media with an asset reference.
    /// </summary>
    /// <typeparam name="TAsset">The type of the asset.</typeparam>
    /// <typeparam name="TMedia">The type of the media event.</typeparam>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public abstract class SingleMediaAssetTrack<TAsset, TMedia> : SingleMediaTrack<TMedia>
    where TAsset : Asset
    where TMedia : SingleMediaAssetMedia, new()
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
                TMedia media = TrackMedia;
                if (media.Asset == value?.ID)
                    return;

                media.Asset = value?.ID ?? Guid.Empty;
                _picker.SelectedAsset = value;
                Timeline?.MarkAsEdited();
            }
        }

        /// <inheritdoc />
        protected SingleMediaAssetTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            _picker = new AssetPicker(typeof(TAsset), Vector2.Zero)
            {
                Size = new Vector2(50.0f, 36.0f),
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            _picker.Location = new Vector2(_muteCheckbox.Left - _picker.Width - 2, 2);
            _picker.SelectedItemChanged += OnPickerSelectedItemChanged;
            Height = 4 + _picker.Height;
        }

        private void OnPickerSelectedItemChanged()
        {
            Asset = (TAsset)_picker.SelectedAsset;
        }
    }
}
