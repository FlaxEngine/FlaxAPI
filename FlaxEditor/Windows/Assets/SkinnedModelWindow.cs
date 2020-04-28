// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.Content.Import;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.Viewport.Cameras;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="SkinnedModel"/> asset.
    /// </summary>
    /// <seealso cref="SkinnedModel" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class SkinnedModelWindow : AssetEditorWindowBase<SkinnedModel>
    {
        // TODO: debug model UVs channel
        // TODO: refresh material slots comboboxes on material slot rename
        // TODO: add button to draw model/bone bounds

        private sealed class Preview : AnimatedModelPreview
        {
            private readonly SkinnedModelWindow _window;
            private ContextMenuButton _showFloorButton;
            private ContextMenuButton _showCurrentLODButton;
            private StaticModel _floorModel;
            private bool _showCurrentLOD;

            public Preview(SkinnedModelWindow window)
            : base(true)
            {
                _window = window;

                // Show floor widget
                _showFloorButton = ViewWidgetShowMenu.AddButton("Floor", OnShowFloorModelClicked);
                _showFloorButton.IndexInParent = 1;

                // Show current LOD widget
                _showCurrentLODButton = ViewWidgetShowMenu.AddButton("Current LOD", OnShowCurrentLODClicked);
                _showCurrentLODButton.IndexInParent = 2;

                // Floor model
                _floorModel = StaticModel.New();
                _floorModel.Position = new Vector3(0, -25, 0);
                _floorModel.Scale = new Vector3(5, 0.5f, 5);
                _floorModel.Model = FlaxEngine.Content.LoadAsync<Model>(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Cube.flax"));
                _floorModel.IsActive = false;
                Task.AddCustomActor(_floorModel);

                // Enable shadows
                PreviewLight.ShadowsMode = ShadowsCastingMode.All;
                PreviewLight.CascadeCount = 3;
                PreviewLight.ShadowsDistance = 2000.0f;
                Task.ViewFlags |= ViewFlags.Shadows;
            }

            private void OnShowFloorModelClicked(ContextMenuButton obj)
            {
                _floorModel.IsActive = !_floorModel.IsActive;
                _showFloorButton.Icon = _floorModel.IsActive ? Style.Current.CheckBoxTick : SpriteHandle.Invalid;
            }

            private void OnShowCurrentLODClicked(ContextMenuButton obj)
            {
                _showCurrentLOD = !_showCurrentLOD;
                _showCurrentLODButton.Icon = _showCurrentLOD ? Style.Current.CheckBoxTick : SpriteHandle.Invalid;
            }

            private int ComputeLODIndex(SkinnedModel model)
            {
                if (PreviewActor.ForcedLOD != -1)
                    return PreviewActor.ForcedLOD;

                // Based on RenderTools::ComputeModelLOD
                CreateProjectionMatrix(out var projectionMatrix);
                float screenMultiple = 0.5f * Mathf.Max(projectionMatrix.M11, projectionMatrix.M22);
                var sphere = PreviewActor.Sphere;
                var viewOrigin = ViewPosition;
                float distSqr = Vector3.DistanceSquared(ref sphere.Center, ref viewOrigin);
                var screenRadiusSquared = Mathf.Square(screenMultiple * sphere.Radius) / Mathf.Max(1.0f, distSqr);

                // Check if model is being culled
                if (Mathf.Square(model.MinScreenSize * 0.5f) > screenRadiusSquared)
                    return -1;

                // Skip if no need to calculate LOD
                if (model.LoadedLODs == 0)
                    return -1;
                var lods = model.LODs;
                if (lods.Length == 0)
                    return -1;
                if (lods.Length == 1)
                    return 0;

                // Iterate backwards and return the first matching LOD
                for (int lodIndex = lods.Length - 1; lodIndex >= 0; lodIndex--)
                {
                    if (Mathf.Square(lods[lodIndex].ScreenSize * 0.5f) >= screenRadiusSquared)
                    {
                        return lodIndex + PreviewActor.LODBias;
                    }
                }

                return 0;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = Style.Current;
                var asset = _window.Asset;
                if (asset == null || !asset.IsLoaded)
                {
                    Render2D.DrawText(style.FontLarge, "Loading...", new Rectangle(Vector2.Zero, Size), style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);
                }

                if (_showCurrentLOD)
                {
                    var lodIndex = ComputeLODIndex(asset);
                    string text = string.Format("Current LOD: {0}", lodIndex);
                    if (lodIndex != -1)
                    {
                        var lods = asset.LODs;
                        lodIndex = Mathf.Clamp(lodIndex + PreviewActor.LODBias, 0, lods.Length - 1);
                        var lod = lods[lodIndex];
                        int triangleCount = 0, vertexCount = 0;
                        for (int meshIndex = 0; meshIndex < lod.Meshes.Length; meshIndex++)
                        {
                            var mesh = lod.Meshes[meshIndex];
                            triangleCount += mesh.TriangleCount;
                            vertexCount += mesh.VertexCount;
                        }
                        text += string.Format("\nTriangles: {0}\nVertices: {1}", triangleCount, vertexCount);
                    }
                    var font = Style.Current.FontMedium;
                    var pos = new Vector2(10, 50);
                    Render2D.DrawText(font, text, new Rectangle(pos + Vector2.One, Size), Color.Black);
                    Render2D.DrawText(font, text, new Rectangle(pos, Size), Color.White);
                }
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                Object.Destroy(ref _floorModel);
                _showFloorButton = null;
                _showCurrentLODButton = null;

                base.OnDestroy();
            }
        }

        /// <summary>
        /// The model properties proxy object.
        /// </summary>
        [CustomEditor(typeof(ProxyEditor))]
        private sealed class PropertiesProxy
        {
            [MemberCollection(CanReorderItems = true, NotNullItems = true, OverrideEditorTypeName = "FlaxEditor.CustomEditors.Editors.GenericEditor")]
            [EditorOrder(10), EditorDisplay("Materials", EditorDisplayAttribute.InlineStyle)]
            public MaterialSlot[] MaterialSlots
            {
                get => Asset?.MaterialSlots;
                set
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

            [EditorOrder(20), CustomEditor(typeof(MeshesEditor))]
            public PropertiesProxy Meshes
            {
                get => this;
                set { }
            }

            private SkinnedModelWindow Window;
            private SkinnedModel Asset;
            private ModelImportSettings ImportSettings = new ModelImportSettings();

            [HideInEditor]
            public int IsolateIndex = -1;

            [HideInEditor]
            public int HighlightIndex = -1;

            private bool _skipEffectsGuiEvents;
            private readonly List<ComboBox> _materialSlotComboBoxes = new List<ComboBox>();
            private readonly List<CheckBox> _isolateCheckBoxes = new List<CheckBox>();
            private readonly List<CheckBox> _highlightCheckBoxes = new List<CheckBox>();

            public void OnLoad(SkinnedModelWindow window)
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
                    checkBox.Checked = IsolateIndex == ((SkinnedMesh)checkBox.Tag).MaterialSlotIndex;
                }

                for (int i = 0; i < _highlightCheckBoxes.Count; i++)
                {
                    var checkBox = _highlightCheckBoxes[i];
                    checkBox.Checked = HighlightIndex == ((SkinnedMesh)checkBox.Tag).MaterialSlotIndex;
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
                    comboBox.SelectedIndex = ((SkinnedMesh)comboBox.Tag).MaterialSlotIndex;
                }

                _skipEffectsGuiEvents = false;
            }

            /// <summary>
            /// Sets the material slot index to the mesh.
            /// </summary>
            /// <param name="mesh">The mesh.</param>
            /// <param name="newSlotIndex">New index of the material slot to use.</param>
            public void SetMaterialSlot(SkinnedMesh mesh, int newSlotIndex)
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
            public void SetIsolate(SkinnedMesh mesh)
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
            public void SetHighlight(SkinnedMesh mesh)
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

            private class MeshesEditor : CustomEditor
            {
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                public override void Initialize(LayoutElementsContainer layout)
                {
                    var proxy = (PropertiesProxy)Values[0];
                    proxy._materialSlotComboBoxes.Clear();
                    proxy._isolateCheckBoxes.Clear();
                    proxy._highlightCheckBoxes.Clear();
                    var lods = proxy.Asset.LODs;
                    var loadedLODs = proxy.Asset.LoadedLODs;
                    var nodes = proxy.Asset.Nodes;
                    var bones = proxy.Asset.Bones;

                    // General properties
                    {
                        var group = layout.Group("General");
                        group.Label("Nodes: " + nodes.Length);
                        group.Label("Bones: " + bones.Length);

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
                        var group = layout.Group("LOD " + lodIndex);
                        if (lodIndex < lods.Length - loadedLODs)
                        {
                            group.Label("Loading LOD...");
                            continue;
                        }
                        var lod = lods[lodIndex];
                        var meshes = lod.Meshes;

                        int triangleCount = 0, vertexCount = 0;
                        for (int meshIndex = 0; meshIndex < meshes.Length; meshIndex++)
                        {
                            var mesh = meshes[meshIndex];
                            triangleCount += mesh.TriangleCount;
                            vertexCount += mesh.VertexCount;
                        }

                        group.Label(string.Format("Triangles: {0:N0}   Vertices: {1:N0}", triangleCount, vertexCount));
                        group.Label("Size: " + lod.Box.Size);
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
                        for (int meshIndex = 0; meshIndex < meshes.Length; meshIndex++)
                        {
                            var mesh = meshes[meshIndex];
                            group.Label($"Mesh {meshIndex} (tris: {mesh.TriangleCount:N0}, verts: {mesh.VertexCount:N0})");

                            // Material Slot
                            var materialSlot = group.ComboBox("Material Slot", "Material slot used by this mesh during rendering");
                            materialSlot.ComboBox.Tag = mesh;
                            materialSlot.ComboBox.SelectedIndexChanged += comboBox => proxy.SetMaterialSlot((SkinnedMesh)comboBox.Tag, comboBox.SelectedIndex);
                            proxy._materialSlotComboBoxes.Add(materialSlot.ComboBox);

                            // Isolate
                            var isolate = group.Checkbox("Isolate", "Shows only this mesh (and meshes using the same material slot)");
                            isolate.CheckBox.Tag = mesh;
                            isolate.CheckBox.StateChanged += (box) => proxy.SetIsolate(box.Checked ? (SkinnedMesh)box.Tag : null);
                            proxy._isolateCheckBoxes.Add(isolate.CheckBox);

                            // Highlight
                            var highlight = group.Checkbox("Highlight", "Highlights this mesh with a tint color (and meshes using the same material slot)");
                            highlight.CheckBox.Tag = mesh;
                            highlight.CheckBox.StateChanged += (box) => proxy.SetHighlight(box.Checked ? (SkinnedMesh)box.Tag : null);
                            proxy._highlightCheckBoxes.Add(highlight.CheckBox);
                        }
                    }

                    // Skeleton Bones
                    {
                        var group = layout.Group("Skeleton Bones");
                        group.Panel.Close(false);

                        var tree = group.Tree();
                        for (int i = 0; i < bones.Length; i++)
                        {
                            if (bones[i].ParentIndex == -1)
                            {
                                var node = tree.Node(nodes[bones[i].NodeIndex].Name);
                                BuildSkeletonBonesTree(nodes, bones, node, i);
                                node.TreeNode.ExpandAll(true);
                            }
                        }
                    }

                    // Skeleton Nodes
                    {
                        var group = layout.Group("Skeleton Nodes");
                        group.Panel.Close(false);

                        var tree = group.Tree();
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            if (nodes[i].ParentIndex == -1)
                            {
                                var node = tree.Node(nodes[i].Name);
                                BuildSkeletonNodesTree(nodes, node, i);
                                node.TreeNode.ExpandAll(true);
                            }
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

                private void BuildSkeletonBonesTree(SkeletonNode[] nodes, SkeletonBone[] bones, TreeNodeElement layout, int boneIndex)
                {
                    for (int i = 0; i < bones.Length; i++)
                    {
                        if (bones[i].ParentIndex == boneIndex)
                        {
                            var node = layout.Node(nodes[bones[i].NodeIndex].Name);
                            BuildSkeletonBonesTree(nodes, bones, node, i);
                        }
                    }
                }

                private void BuildSkeletonNodesTree(SkeletonNode[] nodes, TreeNodeElement layout, int nodeIndex)
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        if (nodes[i].ParentIndex == nodeIndex)
                        {
                            var node = layout.Node(nodes[i].Name);
                            BuildSkeletonNodesTree(nodes, node, i);
                        }
                    }
                }
            }
        }

        private readonly SplitPanel _split;
        private readonly AnimatedModelPreview _preview;
        private readonly CustomEditorPresenter _propertiesPresenter;
        private readonly PropertiesProxy _properties;
        private readonly ToolStripButton _saveButton;
        private AnimatedModel _highlightActor;
        private bool _refreshOnLODsLoaded;

        /// <inheritdoc />
        public SkinnedModelWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Save32, Save).LinkTooltip("Save asset to the file");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Bone32, () => _preview.ShowNodes = !_preview.ShowNodes).SetAutoCheck(true).LinkTooltip("Show animated model nodes debug view");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Platform.OpenUrl(Utilities.Constants.DocsUrl + "manual/animation/skinned-model/index.html")).LinkTooltip("See documentation to learn more");

            // Split Panel
            _split = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                AnchorPreset = AnchorPresets.StretchAll,
                Offsets = new Margin(0, 0, _toolstrip.Bottom, 0),
                SplitterValue = 0.7f,
                Parent = this
            };

            // Model preview
            _preview = new Preview(this)
            {
                ViewportCamera = new FPSCamera(),
                ScaleToFit = false,
                Parent = _split.Panel1
            };

            // Model properties
            _propertiesPresenter = new CustomEditorPresenter(null);
            _propertiesPresenter.Panel.Parent = _split.Panel2;
            _properties = new PropertiesProxy();
            _propertiesPresenter.Select(_properties);
            _propertiesPresenter.Modified += MarkAsEdited;

            // Highlight actor (used to highlight selected material slot, see UpdateEffectsOnAsset)
            _highlightActor = AnimatedModel.New();
            _highlightActor.IsActive = false;
            _preview.Task.AddCustomActor(_highlightActor);
        }

        /// <summary>
        /// Updates the highlight/isolate effects on a model asset.
        /// </summary>
        private void UpdateEffectsOnAsset()
        {
            var entries = _preview.PreviewActor.Entries;
            if (entries != null)
            {
                for (int i = 0; i < entries.Length; i++)
                {
                    entries[i].Visible = _properties.IsolateIndex == -1 || _properties.IsolateIndex == i;
                }
                _preview.PreviewActor.Entries = entries;
            }

            if (_properties.HighlightIndex != -1)
            {
                _highlightActor.IsActive = true;

                var highlightMaterial = EditorAssets.Cache.HighlightMaterialInstance;
                entries = _highlightActor.Entries;
                if (entries != null)
                {
                    for (int i = 0; i < entries.Length; i++)
                    {
                        entries[i].Material = highlightMaterial;
                        entries[i].Visible = _properties.HighlightIndex == i;
                    }
                }
                _highlightActor.Entries = entries;
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
                _highlightActor.Transform = _preview.PreviewActor.Transform;
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

            if (_asset.Save())
            {
                Editor.LogError("Cannot save asset.");
                return;
            }

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
            _preview.SkinnedModel = null;
            _highlightActor.SkinnedModel = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.SkinnedModel = _asset;
            _highlightActor.SkinnedModel = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _properties.OnLoad(this);
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();
            _refreshOnLODsLoaded = true;
            _preview.ViewportCamera.SerArcBallView(Asset.GetBox());

            // Reset any root motion
            _preview.PreviewActor.ResetLocalTransform();

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        public override void OnItemReimported(ContentItem item)
        {
            // Refresh the properties (will get new data in OnAssetLoaded)
            _properties.OnClean();
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();
            _refreshOnLODsLoaded = true;

            base.OnItemReimported(item);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            base.OnDestroy();

            FlaxEngine.Object.Destroy(ref _highlightActor);
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
