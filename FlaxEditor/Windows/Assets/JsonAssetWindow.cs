// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="JsonAsset"/> asset.
    /// </summary>
    /// <seealso cref="JsonAsset" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class JsonAssetWindow : AssetEditorWindowBase<JsonAsset>
    {
        private readonly CustomEditorPresenter _presenter;
        private readonly ToolStripButton _saveButton;
        private object _object;

        /// <inheritdoc />
        public JsonAssetWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Save32, Save).LinkTooltip("Save");

            // Panel
            var panel = new Panel(ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            // Properties
            _presenter = new CustomEditorPresenter(null, "Loading...");
            _presenter.Panel.Parent = panel;
            _presenter.Modified += MarkAsEdited;
        }

        /// <inheritdoc />
        public override void Save()
        {
            if (!IsEdited)
                return;

            // Wait until asset file be fully loaded
            if (_asset.WaitForLoaded())
            {
                // Error
                return;
            }

            // Save
            if (Editor.SaveJsonAsset(_item.Path, _object))
            {
                // Error
                Editor.LogError("Failed to save " + _item);
                return;
            }

            // Update
            ClearEditedFlag();
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            _saveButton.Enabled = IsEdited;

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _object = Asset.CreateInstance();
            _presenter.Select(_object);
            ClearEditedFlag();

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        public override void OnItemReimported(ContentItem item)
        {
            // Refresh the properties (will get new data in OnAssetLoaded)
            _presenter.Deselect();
            ClearEditedFlag();

            base.OnItemReimported(item);
        }
    }
}
