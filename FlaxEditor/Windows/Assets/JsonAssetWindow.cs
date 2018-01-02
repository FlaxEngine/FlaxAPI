////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
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
        private object _object;

        /// <inheritdoc />
        public JsonAssetWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, editor.UI.GetIcon("Save32")).LinkTooltip("Save");

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
                Editor.LogError("Failed to save " + _item.Name);
                return;
            }

            // Update
            ClearEditedFlag();
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                // Save
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
            _toolstrip.GetButton(1).Enabled = IsEdited;

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
