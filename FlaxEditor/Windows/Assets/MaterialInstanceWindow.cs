////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Material window allows to view and edit <see cref="MaterialInstance"/> asset.
    /// Note: it uses actual asset to modify so changes are visible live in the game/editor preview.
    /// </summary>
    /// <seealso cref="MaterialInstance" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class MaterialInstanceWindow : AssetEditorWindowBase<MaterialInstance>
    {
        private readonly SplitPanel _splitPanel;
        private readonly MaterialPreview _preview;

        /// <inheritdoc />
        public MaterialInstanceWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, Editor.UI.GetIcon("Save32"));// .LinkTooltip(GetSharedTooltip(), TEXT("Save"));// Save material
            // TODO: tooltips support!

            // Split Panel
            _splitPanel = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None);
            _splitPanel.DockStyle = DockStyle.Fill;
            _splitPanel.SplitterValue = 0.7f;
            _splitPanel.Parent = this;

            // Material preview
            _preview = new MaterialPreview(true);
            _preview.Parent = _splitPanel.Panel1;

            // TODO: Properies Editor for material instance properties and parameters editing
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the orginal asset
            if (!IsEdited)
                return;

            // TODO: finsih saving material instances
            throw new NotImplementedException("Saving material instance");

            // Clear flag
            ClearEditedFlag();

            // Update
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "Material Instance";

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                    Save();
                    break;
                default:
                    base.OnToolstripButtonClicked(id);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            if (_toolstrip != null)
            {
                _toolstrip.GetButton(1).Enabled = IsEdited;
            }

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _preview.Material = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Material = _asset;

            base.OnAssetLinked();
        }
    }
}
