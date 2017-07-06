////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEngine;

namespace FlaxEditor.Windows.Assets
{
    /*
    /// <summary>
    /// Editor window to view/modify <see cref="IESProfile"/> asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class IESProfileWindow : AssetEditorWindowBase<IESProfile>
    {
        private readonly IESProfileView _view;

        /// <inheritdoc />
        public IESProfileWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, editor.UI.GetIcon("Import32")); //->LinkTooltip(GetSharedTooltip(), TEXT("Reimport"));// Reimport // TODO: tooltips
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(2, editor.UI.GetIcon("PageScale32")); //->LinkTooltip(GetSharedTooltip(), TEXT("Center view"));// Center view // TODO: tooltips

            // Texture viewer
            _view = new IESProfileView();
            _view.DockStyle = DockStyle.Fill;
            _view.Parent = this;
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "IES Profile";

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                // Reimport
                case 1:
                    Editor.ContentEditing.Reimport(_item as BinaryAssetItem);
                    break;

                // Center view
                case 2:
                    _view.CenterView();
                    break;

                default:
                    base.OnToolstripButtonClicked(id);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _view.Texture = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _view.Texture = _asset;
            
            base.OnAssetLoaded();
        }
    }*/
}
