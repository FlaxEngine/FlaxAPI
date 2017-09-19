////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEditor.Content;
using FlaxEditor.Content.Import;
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
            public PropertiesProxy LevelOfDetails
            {
                get => this;
                set { }
            }

            private ModelWindow Window;
            private Model Asset;
            private ModelImportSettings ImportSettings = new ModelImportSettings();

            [HideInEditor]
            public int IsolateSlotIndex = -1;

            [HideInEditor]
            public int HighlightMeshIndex = -1;

            private bool _skipEffectsGuiEvents;
            private readonly List<CheckBox> _isolateCheckBoxes = new List<CheckBox>();
            private readonly List<CheckBox> _highlightCheckBoxes = new List<CheckBox>();

            public void OnLoad(ModelWindow window)
            {
                // Link
                Window = window;
                Asset = window.Asset;
                IsolateSlotIndex = -1;
                HighlightMeshIndex = -1;
                Window.UpdateEffectsOnAsset();

                // Try to restore target asset import options (usefull for fast reimport)
                ModelImportSettings.TryRestore(ref ImportSettings, window.Item.Path);
            }

            public void OnClean()
            {
                // Unlink
                Window = null;
                Asset = null;
                IsolateSlotIndex = -1;
                HighlightMeshIndex = -1;
            }
            
            public void Reimport()
            {
                Editor.Instance.ContentImporting.Reimport((BinaryAssetItem)Window.Item, ImportSettings);
            }

            /// <summary>
            /// Updates the highlight/isolate effects on UI.
            /// </summary>
            public void UpdateEffectsOnUI()
            {
                _skipEffectsGuiEvents = true;

                for (int i = 0; i < _isolateCheckBoxes.Count; i++)
                {
                    var checkBox = _isolateCheckBoxes[i];
                    checkBox.Checked = IsolateSlotIndex == ((Mesh)checkBox.Tag).MaterialSlotIndex;
                }

                for (int i = 0; i < _highlightCheckBoxes.Count; i++)
                {
                    var checkBox = _highlightCheckBoxes[i];
                    checkBox.Checked = HighlightMeshIndex == ((Mesh)checkBox.Tag).MeshIndex;
                }

                _skipEffectsGuiEvents = false;
            }

            /// <summary>
            /// Sets the material slot to isolate.
            /// </summary>
            /// <param name="mesh">The mesh.</param>
            public void SetIsolate(Mesh mesh)
            {
                if (_skipEffectsGuiEvents)
                    return;
                
                IsolateSlotIndex = mesh?.MaterialSlotIndex ?? -1;
                Window.UpdateEffectsOnAsset();
                UpdateEffectsOnUI();
            }

            /// <summary>
            /// Sets the mesh index to highlight.
            /// </summary>
            /// <param name="mesh">The mesh.</param>
            public void SetHighlight(Mesh mesh)
            {
                if (_skipEffectsGuiEvents)
                    return;

                HighlightMeshIndex = mesh?.MeshIndex ?? -1;
                Window.UpdateEffectsOnAsset();
                UpdateEffectsOnUI();
            }

            private class LodsEditor : CustomEditor
            {
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                public override void Initialize(LayoutElementsContainer layout)
                {
                    var proxy = (PropertiesProxy)Values[0];
                    proxy._isolateCheckBoxes.Clear();
                    proxy._highlightCheckBoxes.Clear();
                    if (proxy.Asset == null)
                        return;
                    var lods = proxy.Asset.LODs;

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
                        group.Label(string.Format("Triangles: {0:N0}   Vertices: {1:N0}", triangleCount, vertexCount));

                        for (int meshIndex = 0; meshIndex < lod.Meshes.Length; meshIndex++)
                        {
                            var mesh = lod.Meshes[meshIndex];
                            group.Label("Mesh " + meshIndex);

                            // Material Slot

                            // Isolate
                            var isolate = group.Checkbox("Isolate", "Show only meshes that are using material slot linked by mesh");
                            isolate.CheckBox.Tag = mesh;
                            isolate.CheckBox.CheckChanged += () => proxy.SetIsolate(isolate.CheckBox.Checked ? (Mesh)isolate.CheckBox.Tag : null);
                            proxy._isolateCheckBoxes.Add(isolate.CheckBox);
                            
                            // Highlight
                            var highlight = group.Checkbox("Highlight", "Highlights this mesh with a tint color");
                            highlight.CheckBox.Tag = mesh;
                            highlight.CheckBox.CheckChanged += () => proxy.SetHighlight(highlight.CheckBox.Checked ? (Mesh)highlight.CheckBox.Tag : null);
                            proxy._highlightCheckBoxes.Add(highlight.CheckBox);
                        }
                    }

                    // LOD Settings
                    {
                        // TODO: show LODs settings
                    }

                    // Import Settings
                    {
                        var group = layout.Group("Import Settings");
                        
                        var importSettingsField = typeof(PropertiesProxy).GetField("ImportSettings", BindingFlags.NonPublic | BindingFlags.Instance);
                        var importSettingsValues = new ValueContainer(importSettingsField) { proxy.ImportSettings };
                        group.Object(importSettingsValues);
                        
                        layout.Space(5);
                        var reimportButton = group.Button("Reimport");
                        reimportButton.Button.Clicked += () => ((PropertiesProxy)Values[0]).Reimport();
                    }
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

        /// <summary>
        /// Updates the highlight/isolate effects on a model asset.
        /// </summary>
        private void UpdateEffectsOnAsset()
        {
            var entries = _preview.PreviewModelActor.Entries;
            if (entries != null)
            {
                for (int i = 0; i < entries.Length; i++)
                {
                    entries[i].Visible = _properties.IsolateSlotIndex == -1 || _properties.IsolateSlotIndex == i;
                }
            }

            // TODO: highlight mesh
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
