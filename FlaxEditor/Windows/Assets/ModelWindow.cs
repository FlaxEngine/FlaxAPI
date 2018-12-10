// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.Content.Import;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEditor.Viewport.Cameras;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="Model"/> asset.
    /// </summary>
    /// <seealso cref="Model" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class ModelWindow : AssetEditorWindowBase<Model>
    {
        // TODO: debug model UVs channels
        // TODO: refresh material slots comboboxes on material slot rename
        // TODO: add button to draw model bounds

        private sealed class Preview : ModelPreview
        {
            private readonly ModelWindow _window;
            private ContextMenuButton _showFloorButton;
            private StaticModel _floorModel;

            public Preview(ModelWindow window)
            : base(true)
            {
                _window = window;

                // Show floor widget
                _showFloorButton = ViewWidgetButtonMenu.AddButton("Show floor", OnShowFloorModelClicked);
                _showFloorButton.IndexInParent = 1;

                // Floor model
                _floorModel = StaticModel.New();
                _floorModel.Position = new Vector3(0, -25, 0);
                _floorModel.Scale = new Vector3(5, 0.5f, 5);
                _floorModel.Model = FlaxEngine.Content.LoadAsync<Model>(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Cube.flax"));
                _floorModel.IsActive = false;
                Task.CustomActors.Add(_floorModel);

                // Enable shadows
                PreviewLight.ShadowsMode = ShadowsCastingMode.All;
                PreviewLight.CascadeCount = 2;
                PreviewLight.ShadowsDistance = 1000.0f;
                Task.Flags |= ViewFlags.Shadows;
            }

            private void OnShowFloorModelClicked(ContextMenuButton obj)
            {
                _floorModel.IsActive = !_floorModel.IsActive;
                _showFloorButton.Icon = _floorModel.IsActive ? Style.Current.CheckBoxTick : Sprite.Invalid;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = Style.Current;
                if (_window.Asset == null || !_window.Asset.IsLoaded)
                {
                    Render2D.DrawText(style.FontLarge, "Loading...", new Rectangle(Vector2.Zero, Size), Color.White, TextAlignment.Center, TextAlignment.Center);
                }
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                FlaxEngine.Object.Destroy(ref _floorModel);
                _showFloorButton = null;

                base.Dispose();
            }
        }

        /// <summary>
        /// The model properties proxy object.
        /// </summary>
        [CustomEditor(typeof(ProxyEditor))]
        private sealed class PropertiesProxy
        {
            [EditorOrder(10), EditorDisplay("Materials", EditorDisplayAttribute.InlineStyle), MemberCollection(CanReorderItems = true, NotNullItems = true)]
            public MaterialSlot[] MaterialSlots
            {
                get => Asset?.MaterialSlots;
                set
                {
                    if (Asset != null)
                    {
                        if (Asset.MaterialSlots.Length != value.Length)
                        {
                            MaterialBase[] materials = new MaterialBase[value.Length];
                            string[] names = new string[value.Length];
                            ShadowsCastingMode[] shadowsModes = new ShadowsCastingMode[value.Length];
                            for (int i = 0; i < value.Length; i++)
                            {
                                materials[i] = value[i].Material;
                                names[i] = value[i].Name;
                                shadowsModes[i] = value[i].ShadowsMode;
                            }

                            Asset.SetupMaterialSlots(value.Length);

                            var slots = Asset.MaterialSlots;
                            for (int i = 0; i < slots.Length; i++)
                            {
                                slots[i].Material = materials[i];
                                slots[i].Name = names[i];
                                slots[i].ShadowsMode = shadowsModes[i];
                            }

                            UpdateMaterialSlotsUI();
                        }
                    }
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
            public int IsolateIndex = -1;

            [HideInEditor]
            public int HighlightIndex = -1;

            private bool _skipEffectsGuiEvents;
            private readonly List<ComboBox> _materialSlotComboBoxes = new List<ComboBox>();
            private readonly List<CheckBox> _isolateCheckBoxes = new List<CheckBox>();
            private readonly List<CheckBox> _highlightCheckBoxes = new List<CheckBox>();

            public void OnLoad(ModelWindow window)
            {
                // Link
                Window = window;
                Asset = window.Asset;
                IsolateIndex = -1;
                HighlightIndex = -1;
                Window.UpdateEffectsOnAsset();

                // Try to restore target asset import options (useful for fast reimport)
                ModelImportSettings.TryRestore(ref ImportSettings, window.Item.Path);
            }

            public void OnClean()
            {
                // Unlink
                Window = null;
                Asset = null;
                IsolateIndex = -1;
                HighlightIndex = -1;
            }

            public void Reimport()
            {
                Editor.Instance.ContentImporting.Reimport((BinaryAssetItem)Window.Item, ImportSettings, true);
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
                    checkBox.Checked = IsolateIndex == ((Mesh)checkBox.Tag).MaterialSlotIndex;
                }

                for (int i = 0; i < _highlightCheckBoxes.Count; i++)
                {
                    var checkBox = _highlightCheckBoxes[i];
                    checkBox.Checked = HighlightIndex == ((Mesh)checkBox.Tag).MaterialSlotIndex;
                }

                _skipEffectsGuiEvents = false;
            }

            /// <summary>
            /// Updates the material slots UI parts. Should be called after material slot rename.
            /// </summary>
            public void UpdateMaterialSlotsUI()
            {
                _skipEffectsGuiEvents = true;

                // Generate material slots labels (with index prefix)
                var slots = Asset.MaterialSlots;
                var slotsLabels = new string[slots.Length];
                for (int i = 0; i < slots.Length; i++)
                {
                    slotsLabels[i] = string.Format("[{0}] {1}", i, slots[i].Name);
                }

                // Update comboboxes
                for (int i = 0; i < _materialSlotComboBoxes.Count; i++)
                {
                    var comboBox = _materialSlotComboBoxes[i];
                    comboBox.SetItems(slotsLabels);
                    comboBox.SelectedIndex = ((Mesh)comboBox.Tag).MaterialSlotIndex;
                }

                _skipEffectsGuiEvents = false;
            }

            /// <summary>
            /// Sets the material slot index to the mesh.
            /// </summary>
            /// <param name="mesh">The mesh.</param>
            /// <param name="newSlotIndex">New index of the material slot to use.</param>
            public void SetMaterialSlot(Mesh mesh, int newSlotIndex)
            {
                if (_skipEffectsGuiEvents)
                    return;

                mesh.MaterialSlotIndex = newSlotIndex == -1 ? 0 : newSlotIndex;
                Window.UpdateEffectsOnAsset();
                UpdateEffectsOnUI();
                Window.MarkAsEdited();
            }

            /// <summary>
            /// Sets the material slot to isolate.
            /// </summary>
            /// <param name="mesh">The mesh.</param>
            public void SetIsolate(Mesh mesh)
            {
                if (_skipEffectsGuiEvents)
                    return;

                IsolateIndex = mesh?.MaterialSlotIndex ?? -1;
                Window.UpdateEffectsOnAsset();
                UpdateEffectsOnUI();
            }

            /// <summary>
            /// Sets the material slot index to highlight.
            /// </summary>
            /// <param name="mesh">The mesh.</param>
            public void SetHighlight(Mesh mesh)
            {
                if (_skipEffectsGuiEvents)
                    return;

                HighlightIndex = mesh?.MaterialSlotIndex ?? -1;
                Window.UpdateEffectsOnAsset();
                UpdateEffectsOnUI();
            }

            private class ProxyEditor : GenericEditor
            {
                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    var proxy = (PropertiesProxy)Values[0];

                    if (proxy.Asset == null || !proxy.Asset.IsLoaded)
                    {
                        layout.Label("Loading...");
                        return;
                    }

                    base.Initialize(layout);
                }

                /// <inheritdoc />
                internal override void RefreshInternal()
                {
                    // Skip updates when model is not loaded
                    var proxy = (PropertiesProxy)Values[0];
                    if (proxy.Asset == null || !proxy.Asset.IsLoaded)
                        return;

                    base.RefreshInternal();
                }
            }

            private class LodsEditor : CustomEditor
            {
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                public override void Initialize(LayoutElementsContainer layout)
                {
                    var proxy = (PropertiesProxy)Values[0];
                    proxy._materialSlotComboBoxes.Clear();
                    proxy._isolateCheckBoxes.Clear();
                    proxy._highlightCheckBoxes.Clear();
                    var lods = proxy.Asset.LODs;

                    // General properties
                    {
                        var group = layout.Group("General");

                        var minScreenSize = group.FloatValue("Min Screen Size", "The minimum screen size to draw model (the bottom limit). Used to cull small models. Set to 0 to disable this feature.");
                        minScreenSize.FloatValue.MinValue = 0.0f;
                        minScreenSize.FloatValue.MaxValue = 1.0f;
                        minScreenSize.FloatValue.Value = proxy.Asset.MinScreenSize;
                        minScreenSize.FloatValue.ValueChanged += () =>
                        {
                            proxy.Asset.MinScreenSize = minScreenSize.FloatValue.Value;
                            proxy.Window.MarkAsEdited();
                        };
                    }

                    // Group per LOD
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
                        group.Label("Size: " + lod.Bounds.Size);
                        var screenSize = group.FloatValue("Screen Size", "The screen size to switch LODs. Bottom limit of the model screen size to render this LOD.");
                        screenSize.FloatValue.MinValue = 0.0f;
                        screenSize.FloatValue.MaxValue = 10.0f;
                        screenSize.FloatValue.Value = lod.ScreenSize;
                        screenSize.FloatValue.ValueChanged += () =>
                        {
                            lod.ScreenSize = screenSize.FloatValue.Value;
                            proxy.Window.MarkAsEdited();
                        };

                        // Every mesh properties
                        for (int meshIndex = 0; meshIndex < lod.Meshes.Length; meshIndex++)
                        {
                            var mesh = lod.Meshes[meshIndex];
                            group.Label("Mesh " + meshIndex);

                            // Material Slot
                            var materialSlot = group.ComboBox("Material Slot", "Material slot used by this mesh during rendering");
                            materialSlot.ComboBox.Tag = mesh;
                            materialSlot.ComboBox.SelectedIndexChanged += comboBox => proxy.SetMaterialSlot((Mesh)comboBox.Tag, comboBox.SelectedIndex);
                            proxy._materialSlotComboBoxes.Add(materialSlot.ComboBox);

                            // Isolate
                            var isolate = group.Checkbox("Isolate", "Shows only this mesh (and meshes using the same material slot)");
                            isolate.CheckBox.Tag = mesh;
                            isolate.CheckBox.StateChanged += (box) => proxy.SetIsolate(box.Checked ? (Mesh)box.Tag : null);
                            proxy._isolateCheckBoxes.Add(isolate.CheckBox);

                            // Highlight
                            var highlight = group.Checkbox("Highlight", "Highlights this mesh with a tint color (and meshes using the same material slot)");
                            highlight.CheckBox.Tag = mesh;
                            highlight.CheckBox.StateChanged += (box) => proxy.SetHighlight(box.Checked ? (Mesh)box.Tag : null);
                            proxy._highlightCheckBoxes.Add(highlight.CheckBox);
                        }
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

                    // Refresh UI
                    proxy.UpdateMaterialSlotsUI();
                }
            }
        }

        private readonly SplitPanel _split;
        private readonly ModelPreview _preview;
        private readonly CustomEditorPresenter _propertiesPresenter;
        private readonly PropertiesProxy _properties;
        private readonly ToolStripButton _saveButton;
        private StaticModel _highlightActor;
        private bool _refreshOnLODsLoaded;

        /// <inheritdoc />
        public ModelWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Save32, Save).LinkTooltip("Save");
            //_toolstrip.AddSeparator();
            //_toolstrip.AddButton(editor.Icons.UV32, () => {CacheMeshData(); _uvDebugIndex++; if (_uvDebugIndex >= 2) _uvDebugIndex = -1; }).LinkTooltip("Show model UVs (toggles across all channels)"); // TODO: support gather mesh data
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/graphics/models/index.html")).LinkTooltip("See documentation to learn more");

            // Split Panel
            _split = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Model preview
            _preview = new Preview(this)
            {
                ViewportCamera = new FPSCamera(),
                Parent = _split.Panel1
            };

            // Model properties
            _propertiesPresenter = new CustomEditorPresenter(null);
            _propertiesPresenter.Panel.Parent = _split.Panel2;
            _properties = new PropertiesProxy();
            _propertiesPresenter.Select(_properties);
            _propertiesPresenter.Modified += MarkAsEdited;

            // Highlight actor (used to highlight selected material slot, see UpdateEffectsOnAsset)
            _highlightActor = StaticModel.New();
            _highlightActor.IsActive = false;
            _preview.Task.CustomActors.Add(_highlightActor);
        }

        private void CacheMeshData()
        {
            // TODO: finish mesh data gather from c# API (use async task)
        }

        /// <summary>
        /// Updates the highlight/isolate effects on a model asset.
        /// </summary>
        private void UpdateEffectsOnAsset()
        {
            var entries = _preview.PreviewStaticModel.Entries;
            if (entries != null)
            {
                for (int i = 0; i < entries.Length; i++)
                {
                    entries[i].Visible = _properties.IsolateIndex == -1 || _properties.IsolateIndex == i;
                }
            }

            if (_properties.HighlightIndex != -1)
            {
                _highlightActor.IsActive = true;

                var highlightMaterial = FlaxEngine.Content.LoadAsyncInternal<MaterialBase>(EditorAssets.HighlightMaterial);
                entries = _highlightActor.Entries;
                if (entries != null)
                {
                    for (int i = 0; i < entries.Length; i++)
                    {
                        entries[i].Material = highlightMaterial;
                        entries[i].Visible = _properties.HighlightIndex == i;
                    }
                }
            }
            else
            {
                _highlightActor.IsActive = false;
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Sync highlight actor size with actual preview model (preview scales model for better usage experience)
            if (_highlightActor && _highlightActor.IsActive)
            {
                _highlightActor.Transform = _preview.PreviewStaticModel.Transform;
            }

            // Model is loaded but LODs data may be during streaming so refresh properties on fully loaded
            if (_refreshOnLODsLoaded && _asset && _asset.LoadedLODs == _asset.LODs.Length)
            {
                _refreshOnLODsLoaded = false;
                _propertiesPresenter.BuildLayout();
            }

            base.Update(deltaTime);
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

            // Call asset saving
            if (_asset.Save())
            {
                // Error
                Editor.LogError("Failed to save model " + _item.Path);
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
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Model = null;
            _highlightActor.Model = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Model = _asset;
            _highlightActor.Model = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _properties.OnLoad(this);
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();
            _refreshOnLODsLoaded = true;

            // TODO: disable streaming for this model

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

        /// <inheritdoc />
        public override void OnDestroy()
        {
            base.OnDestroy();

            Object.Destroy(ref _highlightActor);
        }

        /// <inheritdoc />
        public override bool UseLayoutData => true;

        /// <inheritdoc />
        public override void OnLayoutSerialize(XmlWriter writer)
        {
            writer.WriteAttributeString("Split", _split.SplitterValue.ToString());
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize(XmlElement node)
        {
            float value1;

            if (float.TryParse(node.GetAttribute("Split"), out value1))
                _split.SplitterValue = value1;
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize()
        {
            _split.SplitterValue = 0.7f;
        }
    }
}
