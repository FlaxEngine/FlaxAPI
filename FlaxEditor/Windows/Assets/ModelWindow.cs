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
    /// Editor window to view/modify <see cref="Model"/> asset.
    /// </summary>
    /// <seealso cref="Model" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class ModelWindow : AssetEditorWindowBase<Model>
    {
        // TODO: missing features to port from c++ editor:
        // - debug model uv channels
        // - drawing model bounds - sphere/box
        // - editing per mesh settings (material, shadows casting, etc.)
        // - easy reimport via 1 click
        // - refesh properties on asset loaded/reimported
        // - link for content pipeline and handle asset reimport event (refresh data, etc.)

        private readonly SplitPanel _splitPanel;
        private readonly ModelPreview _preview;

        private int _uvDebugIndex = -1;

        /// <inheritdoc />
        public ModelWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, editor.UI.GetIcon("Save32"));//->LinkTooltip(GetSharedTooltip(), TEXT("Save")); // TODO: tooltips
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(2, editor.UI.GetIcon("UV32"));//->LinkTooltip(GetSharedTooltip(), TEXT("Show model UVs")); // TODO: tooltips

            // Split Panel
            _splitPanel = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical);
            _splitPanel.SplitterValue = 0.7f;
            _splitPanel.Parent = this;

            // Model preview
            _preview = new ModelPreview(true);
            _preview.Parent = _splitPanel.Panel1;
        }

        private void CacheMeshData()
        {
            // TODO: finish mesh data gather from c# API
        }

        /// <inheritdoc />
        public override void Save()
        {
            if (!IsEdited)
                return;

            // Wait until model asset file be fully loaded
            if (_asset.WaitForLoaded())
            {
                // Error
                return;
            }

            throw new NotImplementedException("Saving edited model asset");
            // Call asset saving
            /*if (_asset.Save())
            {
                // Error
                LOG_EDITOR(Error, 63, _element->GetPath());
                return;
            }

            // Update
            _proxy.OnSave();
            clearEditedFlag();
            _element->RefreshPreview();*/
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "Model";

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                // Save
                case 1:
                    Save();
                    break;

                // Show model UVs
                case 2:
                    CacheMeshData();
                    _uvDebugIndex++;
                    if (_uvDebugIndex >= 2)
                        _uvDebugIndex = -1;
                    break;

                default:
                    base.OnToolstripButtonClicked(id);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            _toolstrip.GetButton(1).Enabled = IsEdited;
            
            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _preview.Model = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Model = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            // Init model properties panel
            //_proxy.OnLoaded();

            ClearEditedFlag();

            base.OnAssetLoaded();
        }
    }
}
