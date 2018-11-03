// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI;
using FlaxEditor.Modules;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;
using FlaxEngine.GUI;

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

                // TODO: editing terrain chunk OverrideMaterial

                // Update UI to match the current state
                OnSelectionChanged();
                OnGizmoModeChanged();
            }

            private void OnSelectionChanged()
            {
                var terrain = CarveTab.SelectedTerrain;
                if (terrain == null)
                {
                    _selectionInfoLabel.Text = "Select a terrain to modify its properties.";
                }
                else
                {
                    var patchCoord = Gizmo.SelectedPatchCoord;
                    if (Gizmo.EditMode == EditTerrainGizmoMode.Modes.Edit)
                    {
                        var chunkCoord = Gizmo.SelectedChunkCoord;
                        _selectionInfoLabel.Text = string.Format(
                            "Selected terrain: {0}\nPatch: {1}x{2}\nChunk: {3}x{4}",
                            terrain.Name,
                            patchCoord.X, patchCoord.Y,
                            chunkCoord.X, chunkCoord.Y
                        );
                    }
                    else
                    {
                        _selectionInfoLabel.Text = string.Format(
                            "Selected terrain: {0}\nPatch: {1}x{2}",
                            terrain.Name,
                            patchCoord.X, patchCoord.Y
                        );
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
