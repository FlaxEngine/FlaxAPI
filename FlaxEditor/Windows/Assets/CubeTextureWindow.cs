////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="CubeTexture"/> asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class CubeTextureWindow : AssetEditorWindowBase<CubeTexture>
    {
        private readonly CubeTexturePreview _preview;

        /// <inheritdoc />
        public CubeTextureWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Material preview
            _preview = new CubeTexturePreview(true);
            _preview.Parent = this;
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "Cube Texture";

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _preview.Material = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.CubeTexture = _asset;

            base.OnAssetLinked();
        }
    }
}
