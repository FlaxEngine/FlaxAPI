////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Viewport.Previews;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="CubeTexture"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class CubeTextureProxy : BinaryAssetProxy
    {
        private CubeTexturePreview _preview;

        /// <inheritdoc />
        public override string Name => "Cube Texture";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == CubeTexture.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new CubeTextureWindow(editor, item as AssetItem);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x3498db);

        /// <inheritdoc />
        public override ContentDomain Domain => CubeTexture.Domain;

        /// <inheritdoc />
        public override bool CanDrawThumbnail(AssetItem item)
        {
            if (_preview == null)
            {
                _preview = new CubeTexturePreview(false);
                _preview.DockStyle = DockStyle.Fill;
            }
            if (!_preview.HasLoadedAssets)
                return false;

            var asset = FlaxEngine.Content.LoadAsync<CubeTexture>(item.Path);
            return asset.IsLoaded;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(AssetItem item, ContainerControl guiRoot)
        {
            var asset = FlaxEngine.Content.LoadAsync<CubeTexture>(item.Path);
            _preview.CubeTexture = asset;
            _preview.Parent = guiRoot;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawEnd(AssetItem item, ContainerControl guiRoot)
        {
            _preview.CubeTexture = null;
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
