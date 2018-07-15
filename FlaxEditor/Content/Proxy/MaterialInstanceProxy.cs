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
    /// A <see cref="MaterialInstance"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class MaterialInstanceProxy : BinaryAssetProxy
    {
        private MaterialPreview _preview;

        /// <inheritdoc />
        public override string Name => "Material Instance";

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new MaterialInstanceWindow(editor, item as AssetItem);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x2c3e50);

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Material;

        /// <inheritdoc />
        public override Type AssetType => typeof(MaterialInstance);

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath, object arg)
        {
            if (Editor.CreateAsset(Editor.NewAssetType.MaterialInstance, outputPath))
                throw new Exception("Failed to create new asset.");
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawPrepare(ThumbnailRequest request)
        {
            if (_preview == null)
            {
                _preview = new MaterialPreview(false);
                _preview.RenderOnlyWithWindow = false;
                _preview.Task.Enabled = false;
                _preview.PostFxVolume.Settings.Eye_Technique = EyeAdaptationTechnique.None;
                _preview.PostFxVolume.Settings.Eye_Exposure = 0.1f;
                _preview.PostFxVolume.Settings.data.Flags4 |= 0b1001;
                _preview.Size = new Vector2(PreviewsCache.AssetIconSize, PreviewsCache.AssetIconSize);
                _preview.SyncBackbufferSize();
            }

            // TODO: disable streaming for dependant assets during thumbnail rendering (and restore it after)
        }

        /// <inheritdoc />
        public override bool CanDrawThumbnail(ThumbnailRequest request)
        {
            return _preview.HasLoadedAssets;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            _preview.Material = (MaterialInstance)request.Asset;
            _preview.Parent = guiRoot;

            _preview.Task.Internal_Render(context);
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawEnd(ThumbnailRequest request, ContainerControl guiRoot)
        {
            _preview.Material = null;
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
