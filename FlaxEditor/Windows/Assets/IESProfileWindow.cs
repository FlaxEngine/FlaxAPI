////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="IESProfile"/> asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class IESProfileWindow : AssetEditorWindowBase<IESProfile>
    {
        private readonly IESProfilePreview _preview;

        /// <inheritdoc />
        public IESProfileWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, editor.UI.GetIcon("Import32")).LinkTooltip("Reimport");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(2, editor.UI.GetIcon("PageScale32")).LinkTooltip("Center view");

            // IES Profile preview
            _preview = new IESProfilePreview
            {
                DockStyle = DockStyle.Fill,
                Parent = this
            };
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "IES Profile";

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                    Editor.ContentImporting.Reimport((BinaryAssetItem)Item);
                    break;
                case 2:
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
        protected override void OnAssetLoaded()
        {
            _preview.Asset = _asset;

            base.OnAssetLoaded();
        }
    }
}
