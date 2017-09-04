////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Texture window allows to view and edit <see cref="Texture"/> asset.
    /// </summary>
    /// <seealso cref="Texture" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class TextureWindow : AssetEditorWindowBase<Texture>
    {
        /// <summary>
        /// The texture properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            [EditorOrder(10), EditorDisplay("General"), Tooltip("")]
            public int Value { get; set; }

            /// <summary>
            /// Gathers parameters from the specified texture.
            /// </summary>
            /// <param name="win">The texture window.</param>
            public void OnLoad(TextureWindow win)
            {
                // Link
                // TODO: finish

                // Prepare restore data
                PeekState();
            }

            /// <summary>
            /// Records the current state to restore it on DiscardChanges.
            /// </summary>
            public void PeekState()
            {
                // TODO: finish
            }

            /// <summary>
            /// On discard changes
            /// </summary>
            public void DiscardChanges()
            {
                // TODO: finish
            }

            /// <summary>
            /// Clears temporary data.
            /// </summary>
            public void OnClean()
            {
                // Unlink
                // TODO: finish
            }
        }

        private readonly TexturePreview _preview;

        private readonly PropertiesProxy _properties;
        private bool _isWaitingForLoad;
        internal bool _paramValueChange;

        /// <inheritdoc />
        public TextureWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, Editor.UI.GetIcon("Save32")).LinkTooltip("Save");
            _toolstrip.AddButton(2, Editor.UI.GetIcon("Import32")).LinkTooltip("Reimport");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(5, Editor.UI.GetIcon("PageScale32")).LinkTooltip("Center view");

            // Split Panel
            var splitPanel = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Texture preview
            _preview = new TexturePreview
            {
                Parent = splitPanel.Panel1
            };

            // Texture properties editor
            var propertiesEditor = new CustomEditorPresenter(null);
            propertiesEditor.Panel.Parent = splitPanel.Panel2;
            _properties = new PropertiesProxy();
            propertiesEditor.Select(_properties);
            propertiesEditor.Modified += OnPropertyEdited;
        }

        private void OnPropertyEdited()
        {
            _paramValueChange = false;
            MarkAsEdited();
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the orginal asset
            if (!IsEdited)
                return;

            throw new NotImplementedException("save texture");

            // Update
            _properties.PeekState();
            ClearEditedFlag();
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "Texture";

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                    Save();
                    break;
                case 2:
                    Editor.ContentImporting.Reimport((BinaryAssetItem)Item);
                    break;
                case 5:
                    _preview.CenterView();
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
            _properties.OnClean();
            _preview.Texture = null;
            _isWaitingForLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Texture = _asset;
            _isWaitingForLoad = true;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        protected override void OnClose()
        {
            // Discard unsaved changes
            _properties.DiscardChanges();

            base.OnClose();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Check if need to load
            if (_isWaitingForLoad && _asset.IsLoaded)
            {
                // Clear flag
                _isWaitingForLoad = false;

                // Init properties and parameters proxy
                _properties.OnLoad(this);

                // Setup
                ClearEditedFlag();
            }
        }
    }
}
