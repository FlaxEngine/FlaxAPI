// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI;
using FlaxEditor.Modules;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain carving tab. Supports different modes for terrain editing including: carving, painting and managing tools.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    public class CarveTab : Tab
    {
        private readonly Tabs _modes;
        private SculpTerrainGizmoMode _sculpTerrainGizmo;
        private PaintTerrainGizmoMode _paintTerrainGizmo;

        /// <summary>
        /// The editor instance.
        /// </summary>
        public readonly Editor Editor;

        /// <summary>
        /// The cached selected terrain. It's synchronized with <see cref="SceneEditingModule.Selection"/>.
        /// </summary>
        public FlaxEngine.Terrain SelectedTerrain;

        /// <summary>
        /// Occurs when selected terrain gets changed (to a different value).
        /// </summary>
        public event Action SelectedTerrainChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarveTab"/> class.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <param name="editor">The editor instance.</param>
        public CarveTab(Sprite icon, Editor editor)
        : base(string.Empty, icon)
        {
            Editor = editor;
            Editor.SceneEditing.SelectionChanged += OnSelectionChanged;

            _sculpTerrainGizmo = editor.Windows.EditWin.Viewport.SculpTerrainGizmo;
            _paintTerrainGizmo = editor.Windows.EditWin.Viewport.PaintTerrainGizmo;

            _modes = new Tabs
            {
                Orientation = Orientation.Vertical,
                UseScroll = true,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(50, 32),
                Parent = this
            };

            // Init tool modes
            InitSculpMode();
            InitPaintMode();
            _modes.AddTab(new EditModeTab(this, editor.Windows.EditWin.Viewport.EditTerrainGizmo));

            _modes.SelectedTabIndex = 0;
        }

        private void OnSelectionChanged()
        {
            var terrainNode = Editor.SceneEditing.SelectionCount > 0 ? Editor.SceneEditing.Selection[0] as TerrainNode : null;
            var terrain = terrainNode?.Actor as FlaxEngine.Terrain;
            if (terrain != SelectedTerrain)
            {
                SelectedTerrain = terrain;
                SelectedTerrainChanged?.Invoke();
            }
        }

        private void InitSculpMode()
        {
            var tab = _modes.AddTab(new Tab("Sculp"));
            tab.Selected += OnTabSelected;
            var panel = new Panel(ScrollBars.Both)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };

            var info = panel.AddChild<Label>();
            info.Text = "Sculp Mode";
            info.DockStyle = DockStyle.Fill;
        }

        private void InitPaintMode()
        {
            var tab = _modes.AddTab(new Tab("Paint"));
            tab.Selected += OnTabSelected;
            var panel = new Panel(ScrollBars.Both)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };

            var info = panel.AddChild<Label>();
            info.Text = "Paint Mode";
            info.DockStyle = DockStyle.Fill;
        }

        /// <summary>
        /// Carve tab related to terrain editing. Allows to pick a terrain patch and remove it or add new patches. Can be used to modify selected chunk properties.
        /// </summary>
        /// <seealso cref="FlaxEditor.GUI.Tab" />
        private class EditModeTab : Tab
        {
            /// <summary>
            /// The parent carve tab.
            /// </summary>
            public readonly CarveTab CarveTab;

            /// <summary>
            /// The related edit terrain gizmo.
            /// </summary>
            public readonly EditTerrainGizmoMode Gizmo;

            private readonly ComboBox _modeComboBox;
            private readonly Label _selectionInfoLabel;
            private readonly Button _deletePatchButton;
            private readonly ContainerControl _chunkProperties;
            private readonly AssetPicker _chunkOverrideMaterial;
            private bool _isUpdatingUI;

            public EditModeTab(CarveTab tab, EditTerrainGizmoMode gizmo)
            : base("Edit")
            {
                CarveTab = tab;
                Gizmo = gizmo;
                CarveTab.SelectedTerrainChanged += OnSelectionChanged;
                Gizmo.SelectedChunkCoordChanged += OnSelectionChanged;

                // Main panel
                var panel = new Panel(ScrollBars.Both)
                {
                    DockStyle = DockStyle.Fill,
                    Parent = this
                };

                // Mode
                var modeLabel = new Label(4, 4, 40, 18)
                {
                    HorizontalAlignment = TextAlignment.Near,
                    Text = "Mode:",
                    Parent = panel,
                };
                _modeComboBox = new ComboBox(modeLabel.Right + 4, 4, 110)
                {
                    Parent = panel,
                };
                _modeComboBox.AddItem("Edit Chunk");
                _modeComboBox.AddItem("Add Patch");
                _modeComboBox.AddItem("Remove Patch");
                _modeComboBox.SelectedIndex = 0;
                _modeComboBox.SelectedIndexChanged += (combobox) => Gizmo.EditMode = (EditTerrainGizmoMode.Modes)combobox.SelectedIndex;
                Gizmo.ModeChanged += OnGizmoModeChanged;

                // Info
                _selectionInfoLabel = new Label(modeLabel.X, modeLabel.Bottom + 4, 40, 18 * 3)
                {
                    VerticalAlignment = TextAlignment.Near,
                    HorizontalAlignment = TextAlignment.Near,
                    Parent = panel,
                };

                // Chunk Properties
                _chunkProperties = new Panel(ScrollBars.None)
                {
                    Location = new Vector2(_selectionInfoLabel.X, _selectionInfoLabel.Bottom + 4),
                    Parent = panel,
                };
                var chunkOverrideMaterialLabel = new Label(0, 0, 90, 64)
                {
                    HorizontalAlignment = TextAlignment.Near,
                    Text = "Override Material",
                    Parent = _chunkProperties,
                };
                _chunkOverrideMaterial = new AssetPicker(typeof(MaterialBase), new Vector2(chunkOverrideMaterialLabel.Right + 4, 0))
                {
                    Width = 300.0f,
                    Parent = _chunkProperties,
                };
                _chunkOverrideMaterial.SelectedItemChanged += OnSelectedChunkOverrideMaterialChanged;
                _chunkProperties.Size = new Vector2(_chunkOverrideMaterial.Right + 4, _chunkOverrideMaterial.Bottom + 4);

                // Delete patch
                _deletePatchButton = new Button(_selectionInfoLabel.X, _selectionInfoLabel.Bottom + 4)
                {
                    Text = "Delete Patch",
                    Parent = panel,
                };
                _deletePatchButton.Clicked += OnDeletePatchButtonClicked;

                // Update UI to match the current state
                OnSelectionChanged();
                OnGizmoModeChanged();
            }

            private class DeletePatchAction : IUndoAction
            {
                private readonly Editor _editor;
                private Guid _terrainId;
                private Int2 _patchCoord;
                private string _data;

                /// <inheritdoc />
                public string ActionString => "Delete terrain patch";

                public DeletePatchAction(Editor editor, FlaxEngine.Terrain terrain, ref Int2 patchCoord)
                {
                    if (terrain == null)
                        throw new ArgumentException(nameof(terrain));

                    _editor = editor ?? throw new ArgumentException(nameof(editor));
                    _terrainId = terrain.ID;
                    _patchCoord = patchCoord;
                    _data = TerrainTools.SerializePatch(terrain, ref patchCoord);
                }

                /// <inheritdoc />
                public void Do()
                {
                    var terrain = Object.Find<FlaxEngine.Terrain>(ref _terrainId);
                    if (terrain == null)
                    {
                        Editor.LogError("Missing terrain actor.");
                        return;
                    }

                    terrain.RemovePatch(ref _patchCoord);

                    _editor.Scene.MarkSceneEdited(terrain.Scene);
                }

                /// <inheritdoc />
                public void Undo()
                {
                    var terrain = Object.Find<FlaxEngine.Terrain>(ref _terrainId);
                    if (terrain == null)
                    {
                        Editor.LogError("Missing terrain actor.");
                        return;
                    }

                    terrain.AddPatch(ref _patchCoord);
                    TerrainTools.DeserializePatch(terrain, ref _patchCoord, _data);

                    _editor.Scene.MarkSceneEdited(terrain.Scene);
                }
            }

            private void OnDeletePatchButtonClicked()
            {
                if (_isUpdatingUI)
                    return;

                var patchCoord = Gizmo.SelectedPatchCoord;
                if (!CarveTab.SelectedTerrain.HasPatch(ref patchCoord))
                    return;

                var action = new DeletePatchAction(CarveTab.Editor, CarveTab.SelectedTerrain, ref patchCoord);
                action.Do();
                CarveTab.Editor.Undo.AddAction(action);
            }

            private class EditChunkMaterialAction : IUndoAction
            {
                private readonly Editor _editor;
                private Guid _terrainId;
                private Int2 _patchCoord;
                private Int2 _chunkCoord;
                private Guid _beforeMaterial;
                private Guid _afterMaterial;

                /// <inheritdoc />
                public string ActionString => "Edit terrain chunk material";

                public EditChunkMaterialAction(Editor editor, FlaxEngine.Terrain terrain, ref Int2 patchCoord, ref Int2 chunkCoord, MaterialBase toSet)
                {
                    if (terrain == null)
                        throw new ArgumentException(nameof(terrain));

                    _editor = editor ?? throw new ArgumentException(nameof(editor));
                    _terrainId = terrain.ID;
                    _patchCoord = patchCoord;
                    _chunkCoord = chunkCoord;
                    _beforeMaterial = terrain.GetChunkOverrideMaterial(ref patchCoord, ref chunkCoord)?.ID ?? Guid.Empty;
                    _afterMaterial = toSet?.ID ?? Guid.Empty;
                }

                /// <inheritdoc />
                public void Do()
                {
                    Set(ref _afterMaterial);
                }

                /// <inheritdoc />
                public void Undo()
                {
                    Set(ref _beforeMaterial);
                }

                private void Set(ref Guid id)
                {
                    var terrain = Object.Find<FlaxEngine.Terrain>(ref _terrainId);
                    if (terrain == null)
                    {
                        Editor.LogError("Missing terrain actor.");
                        return;
                    }

                    terrain.SetChunkOverrideMaterial(ref _patchCoord, ref _chunkCoord, FlaxEngine.Content.LoadAsync<MaterialBase>(ref id));

                    _editor.Scene.MarkSceneEdited(terrain.Scene);
                }
            }

            private void OnSelectedChunkOverrideMaterialChanged()
            {
                if (_isUpdatingUI)
                    return;

                var patchCoord = Gizmo.SelectedPatchCoord;
                var chunkCoord = Gizmo.SelectedChunkCoord;
                var action = new EditChunkMaterialAction(CarveTab.Editor, CarveTab.SelectedTerrain, ref patchCoord, ref chunkCoord, _chunkOverrideMaterial.SelectedAsset as MaterialBase);
                action.Do();
                CarveTab.Editor.Undo.AddAction(action);
            }

            private void OnSelectionChanged()
            {
                var terrain = CarveTab.SelectedTerrain;
                if (terrain == null)
                {
                    _selectionInfoLabel.Text = "Select a terrain to modify its properties.";
                    _chunkProperties.Visible = false;
                    _deletePatchButton.Visible = false;
                }
                else
                {
                    var patchCoord = Gizmo.SelectedPatchCoord;
                    switch (Gizmo.EditMode)
                    {
                    case EditTerrainGizmoMode.Modes.Edit:
                    {
                        var chunkCoord = Gizmo.SelectedChunkCoord;
                        _selectionInfoLabel.Text = string.Format(
                            "Selected terrain: {0}\nPatch: {1}x{2}\nChunk: {3}x{4}",
                            terrain.Name,
                            patchCoord.X, patchCoord.Y,
                            chunkCoord.X, chunkCoord.Y
                        );
                        _chunkProperties.Visible = true;
                        _deletePatchButton.Visible = false;

                        _isUpdatingUI = true;
                        _chunkOverrideMaterial.SelectedAsset = terrain.GetChunkOverrideMaterial(ref patchCoord, ref chunkCoord);
                        _isUpdatingUI = false;
                        break;
                    }
                    case EditTerrainGizmoMode.Modes.Add:
                    {
                        if (terrain.HasPatch(ref patchCoord))
                        {
                            _selectionInfoLabel.Text = string.Format(
                                "Selected terrain: {0}\nMove mouse cursor at location without a patch.",
                                terrain.Name
                            );
                        }
                        else
                        {
                            _selectionInfoLabel.Text = string.Format(
                                "Selected terrain: {0}\nPatch to add: {1}x{2}\nTo add a new patch press the left mouse button.",
                                terrain.Name,
                                patchCoord.X, patchCoord.Y
                            );
                        }
                        _chunkProperties.Visible = false;
                        _deletePatchButton.Visible = false;
                        break;
                    }
                    case EditTerrainGizmoMode.Modes.Remove:
                    {
                        _selectionInfoLabel.Text = string.Format(
                            "Selected terrain: {0}\nPatch: {1}x{2}",
                            terrain.Name,
                            patchCoord.X, patchCoord.Y
                        );
                        _chunkProperties.Visible = false;
                        _deletePatchButton.Visible = true;
                        break;
                    }
                    }
                }
            }

            private void OnGizmoModeChanged()
            {
                _modeComboBox.SelectedIndex = (int)Gizmo.EditMode;
                OnSelectionChanged();
            }

            /// <inheritdoc />
            public override void OnSelected()
            {
                base.OnSelected();

                CarveTab.Editor.Windows.EditWin.Viewport.SetActiveMode<EditTerrainGizmoMode>();
            }
        }

        /// <inheritdoc />
        public override void OnSelected()
        {
            base.OnSelected();

            UpdateGizmoMode();
        }

        private void OnTabSelected(Tab tab)
        {
            UpdateGizmoMode();
        }

        /// <summary>
        /// Updates the active viewport gizmo mode based on the current mode.
        /// </summary>
        private void UpdateGizmoMode()
        {
            switch (_modes.SelectedTabIndex)
            {
            case 0:
                Editor.Windows.EditWin.Viewport.SetActiveMode<SculpTerrainGizmoMode>();
                break;
            case 1:
                Editor.Windows.EditWin.Viewport.SetActiveMode<PaintTerrainGizmoMode>();
                break;
            case 2: break;
            default: throw new IndexOutOfRangeException("Invalid carve tab mode.");
            }
        }
    }
}
