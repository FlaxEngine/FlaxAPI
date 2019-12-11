// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI.Tabs;
using FlaxEditor.Modules;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain carving tab. Supports different modes for terrain editing including: carving, painting and managing tools.
    /// </summary>
    /// <seealso cref="Tab" />
    public class CarveTab : Tab
    {
        private readonly Tabs _modes;
        private readonly ContainerControl _noTerrainPanel;

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
        /// The sculpt tab;
        /// </summary>
        public SculptTab Sculpt;

        /// <summary>
        /// The paint tab;
        /// </summary>
        public PaintTab Paint;

        /// <summary>
        /// The edit tab;
        /// </summary>
        public EditTab Edit;

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

            Selected += OnSelected;

            _modes = new Tabs
            {
                Orientation = Orientation.Vertical,
                UseScroll = true,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(50, 32),
                Parent = this
            };

            // Init tool modes
            InitSculptMode();
            InitPaintMode();
            InitEditMode();

            _modes.SelectedTabIndex = 0;

            _noTerrainPanel = new ContainerControl
            {
                DockStyle = DockStyle.Fill,
                BackgroundColor = Style.Current.Background,
                Parent = this
            };
            var noTerrainLabel = new Label
            {
                Text = "Select terrain to edit\nor\n\n\n\n",
                DockStyle = DockStyle.Fill,
                Parent = _noTerrainPanel
            };
            var noTerrainButton = new Button
            {
                Text = "Create new terrain",
                AnchorStyle = AnchorStyle.Center,
                Parent = _noTerrainPanel
            };
            noTerrainButton.Clicked += OnCreateNewTerrainClicked;
        }

        private void OnSelected(Tab tab)
        {
            // Auto select first terrain actor to make usage easier
            var actor = Actor.Find<FlaxEngine.Terrain>();
            if (actor)
            {
                Editor.SceneEditing.Select(actor);
            }
        }

        private void OnCreateNewTerrainClicked()
        {
            Editor.UI.CreateTerrain();
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

            _noTerrainPanel.Visible = terrain == null;
        }

        private void InitSculptMode()
        {
            var tab = _modes.AddTab(Sculpt = new SculptTab(this, Editor.Windows.EditWin.Viewport.SculptTerrainGizmo));
            tab.Selected += OnTabSelected;
        }

        private void InitPaintMode()
        {
            var tab = _modes.AddTab(Paint = new PaintTab(this, Editor.Windows.EditWin.Viewport.PaintTerrainGizmo));
            tab.Selected += OnTabSelected;
        }

        private void InitEditMode()
        {
            var tab = _modes.AddTab(Edit = new EditTab(this, Editor.Windows.EditWin.Viewport.EditTerrainGizmo));
            tab.Selected += OnTabSelected;
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
                Editor.Windows.EditWin.Viewport.SetActiveMode<SculptTerrainGizmoMode>();
                break;
            case 1:
                Editor.Windows.EditWin.Viewport.SetActiveMode<PaintTerrainGizmoMode>();
                break;
            case 2:
                Editor.Windows.EditWin.Viewport.SetActiveMode<EditTerrainGizmoMode>();
                break;
            default: throw new IndexOutOfRangeException("Invalid carve tab mode.");
            }
        }
    }
}
