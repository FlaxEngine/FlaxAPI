// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Viewport.Previews;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="Model"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class ModelProxy : BinaryAssetProxy
    {
        private ModelPreview _preview;

        /// <inheritdoc />
        public override string Name => "Model";

        /// <inheritdoc />
        public override bool CanReimport(ContentItem item)
        {
            return true;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new ModelWindow(editor, item as AssetItem);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0xe67e22);

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Model;

        /// <inheritdoc />
        public override Type AssetType => typeof(Model);

        /// <inheritdoc />
        public override void OnThumbnailDrawPrepare(ThumbnailRequest request)
        {
            if (_preview == null)
            {
                _preview = new ModelPreview(false);
                _preview.RenderOnlyWithWindow = false;
                _preview.Task.Enabled = false;
                _preview.PostFxVolume.Settings.Eye_Technique = EyeAdaptationTechnique.None;
                _preview.PostFxVolume.Settings.Eye_Exposure = 0.1f;
                _preview.PostFxVolume.Settings.data.Flags4 |= 0b1001;
                _preview.Size = new Vector2(PreviewsCache.AssetIconSize, PreviewsCache.AssetIconSize);
                _preview.SyncBackbufferSize();
            }

            // TODO: disable streaming for asset during thumbnail rendering (and restore it after)
        }

        /// <inheritdoc />
        public override bool CanDrawThumbnail(ThumbnailRequest request)
        {
            if (!_preview.HasLoadedAssets)
                return false;

            // Check if asset is streamed enough
            var asset = (Model)request.Asset;
            return asset.LoadedLODs >= Mathf.Max(1, (int)(asset.LODs.Length * ThumbnailsModule.MinimumRequriedResourcesQuality));
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            _preview.Model = (Model)request.Asset;
            _preview.Parent = guiRoot;

            _preview.Task.Internal_Render(context);
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawEnd(ThumbnailRequest request, ContainerControl guiRoot)
        {
            _preview.Model = null;
            _preview.Parent = null;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (_preview != null)
            {
                _preview.Dispose();
                _preview = null;
            }

            base.Dispose();
        }
    }
}
