// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Viewport.Previews;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="AudioClip"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class AudioClipProxy : BinaryAssetProxy
    {
        private List<AudioClipPreview> _previews;

        /// <inheritdoc />
        public override string Name => "Audio Clip";

        /// <inheritdoc />
        public override bool CanReimport(ContentItem item)
        {
            return true;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new AudioClipWindow(editor, (AssetItem)item);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0xB3452B);

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Audio;

        /// <inheritdoc />
        public override Type AssetType => typeof(AudioClip);

        /// <inheritdoc />
        public override void OnThumbnailDrawPrepare(ThumbnailRequest request)
        {
            AudioClipPreview preview;
            if (_previews != null && _previews.Count > 0)
            {
                // Reuse preview control
                preview = _previews[_previews.Count - 1];
                _previews.RemoveAt(_previews.Count - 1);
            }
            else
            {
                // Create new preview control
                preview = new AudioClipPreview
                {
                    DrawMode = AudioClipPreview.DrawModes.Fill,
                    DockStyle = DockStyle.Fill,
                    Size = new Vector2(PreviewsCache.AssetIconSize)
                };
            }

            // Cached used preview in request data
            request.Tag = preview;

            // Start loading the audio buffers
            preview.Asset = (AudioClip)request.Asset;
        }

        /// <inheritdoc />
        public override bool CanDrawThumbnail(ThumbnailRequest request)
        {
            // Check if asset is loaded and has audio buffer ready to draw
            var asset = (AudioClip)request.Asset;
            var preview = (AudioClipPreview)request.Tag;
            return asset.IsLoaded && preview.HasData;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            var preview = (AudioClipPreview)request.Tag;
            preview.Parent = guiRoot;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawEnd(ThumbnailRequest request, ContainerControl guiRoot)
        {
            var preview = (AudioClipPreview)request.Tag;
            preview.Parent = null;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawCleanup(ThumbnailRequest request)
        {
            // Unlink asset
            var preview = (AudioClipPreview)request.Tag;
            preview.Asset = null;

            // Return the preview control back to the pool
            if (_previews == null)
                _previews = new List<AudioClipPreview>(8);
            _previews.Add(preview);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (_previews != null)
            {
                for (int i = 0; i < _previews.Count; i++)
                {
                    _previews[i].Dispose();
                }
                _previews.Clear();
            }
            _previews = null;

            base.Dispose();
        }
    }
}
