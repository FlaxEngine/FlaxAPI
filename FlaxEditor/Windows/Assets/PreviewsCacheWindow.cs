////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEditor.Viewport.Previews;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window that allows to view <see cref="PreviewsCache"/> asset.
    /// </summary>
    /// <seealso cref="PreviewsCache" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class PreviewsCacheWindow : AssetEditorWindowBase<PreviewsCache>
    {
        private readonly TexturePreview _preview;

        /// <inheritdoc />
        public PreviewsCacheWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(5, Editor.UI.GetIcon("PageScale32")).LinkTooltip("Center view");

            // Texture preview
            _preview = new TexturePreview(true)
            {
                Parent = this
            };
        }

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                case 5:
                    _preview.CenterView();
                    break;
                default:
                    base.OnToolstripButtonClicked(id);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _preview.Asset = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Asset = _asset;

            base.OnAssetLinked();
        }
    }
}
