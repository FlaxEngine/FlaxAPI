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
        // TODO: adding/removing material slots

        /// <summary>
        /// The model properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            [EditorOrder(10), EditorDisplay("Materials", "__inline__"), MemberCollection(CanReorderItems = true, NotNullItems = true, ReadOnly = true)]
            public MaterialSlot[] MaterialSlots
            {
                get => Asset?.MaterialSlots;
                set
                {
                    if (Asset != null)
                        Asset.MaterialSlots = value;
                }
            }

            [EditorOrder(20), CustomEditor(typeof(LodsEditor))]
            public Model LevelOfDetails
            {
                get => Asset;
                set { }
            }

            private Model Asset;

            public void OnLoad(ModelWindow window)
            {
                // Link
                Asset = window.Asset;
            }

            public void OnClean()
            {
                // Unlink
                Asset = null;
            }

            private class LodsEditor : CustomEditor
            {
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                public override void Initialize(LayoutElementsContainer layout)
                {
                    if (Values[0] == null)
                        return;
                    var lods = ((Model)Values[0]).LODs;

                    for (int lodIndex = 0; lodIndex < lods.Length; lodIndex++)
                    {
                        var lod = lods[lodIndex];

                        int triangleCount = 0, vertexCount = 0;
                        for (int meshIndex = 0; meshIndex < lod.Meshes.Length; meshIndex++)
                        {
                            var mesh = lod.Meshes[meshIndex];
                            triangleCount += mesh.Triangles;
                            vertexCount += mesh.Vertices;
                        }

                        var group = layout.Group("LOD " + lodIndex);
                        group.Label(string.Format("Triangles: {0:N0}, Vertices: {1:N0}", triangleCount, vertexCount));

                        for (int meshIndex = 0; meshIndex < lod.Meshes.Length; meshIndex++)
                        {
                            var mesh = lod.Meshes[meshIndex];

                            // Material Slot

                            // Isolate

                            // Highlight
                        }
                    }

                    // LOD Settings

                    // Import Settings

                    // Reimport button
                }
            }
        }

        private readonly ModelPreview _preview;
        private readonly CustomEditorPresenter _propertiesPresenter;
        private readonly PropertiesProxy _properties;

        private int _uvDebugIndex = -1;

        /// <inheritdoc />
        public ModelWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, editor.UI.GetIcon("Save32")).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(2, editor.UI.GetIcon("UV32")).LinkTooltip("Show model UVs (toggles across all channels)");

            // Split Panel
            var splitPanel = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Model preview
            _preview = new ModelPreview(true)
            {
                Parent = splitPanel.Panel1
            };
            
            // Model properties
            _propertiesPresenter = new CustomEditorPresenter(null);
            _propertiesPresenter.Panel.Parent = splitPanel.Panel2;
            _properties = new PropertiesProxy();
            _propertiesPresenter.Select(_properties);
            _propertiesPresenter.Modified += MarkAsEdited;
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
            ClearEditedFlag();
            _item.RefreshThumbnail();*/
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
            _properties.OnClean();
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
            _properties.OnLoad(this);
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        public override void OnItemReimported(ContentItem item)
        {
            // Refresh the properties (will get new data in OnAssetLoaded)
            _properties.OnClean();
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();
            
            base.OnItemReimported(item);
        }
    }
}
