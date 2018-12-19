// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI;
using FlaxEditor.Modules;
using FlaxEditor.SceneGraph.Actors;
using FlaxEditor.Viewport.Modes;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Foliage editing tab. Supports different modes for foliage editing including: spawning, removing, and managing tools.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    public class FoliageTab : Tab
    {
        private readonly Tabs _modes;
        private readonly ContainerControl _noFoliagePanel;

        /// <summary>
        /// The editor instance.
        /// </summary>
        public readonly Editor Editor;

        /// <summary>
        /// The cached selected foliage. It's synchronized with <see cref="SceneEditingModule.Selection"/>.
        /// </summary>
        public FlaxEngine.Foliage SelectedFoliage;

        /// <summary>
        /// Occurs when selected foliage gets changed (to a different value).
        /// </summary>
        public event Action SelectedFoliageChanged;

        /// <summary>
        /// The foliage types tab;
        /// </summary>
        public FoliageTypesTab FoliageTypes;

        /// <summary>
        /// The paint tab;
        /// </summary>
        public PaintTab Paint;

        /// <summary>
        /// The edit tab;
        /// </summary>
        public EditTab Edit;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoliageTab"/> class.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <param name="editor">The editor instance.</param>
        public FoliageTab(Sprite icon, Editor editor)
        : base(string.Empty, icon)
        {
            Editor = editor;
            Editor.SceneEditing.SelectionChanged += OnSelectionChanged;

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

            _noFoliagePanel = new ContainerControl
            {
                DockStyle = DockStyle.Fill,
                BackgroundColor = Style.Current.Background,
                Parent = this
            };
            var noFoliageLabel = new Label
            {
                Text = "Select foliage to edit\nor\n\n\n\n",
                DockStyle = DockStyle.Fill,
                Parent = _noFoliagePanel
            };
            var noFoliageButton = new Button
            {
                Text = "Create new foliage",
                AnchorStyle = AnchorStyle.Center,
                Parent = _noFoliagePanel
            };
            noFoliageButton.Clicked += OnCreateNewFoliageClicked;
        }

        private void OnCreateNewFoliageClicked()
        {
            // Create
            var actor = FlaxEngine.Foliage.New();
            actor.StaticFlags = StaticFlags.FullyStatic;
            actor.Name = "Foliage";

            // Spawn
            Editor.SceneEditing.Spawn(actor);

            // Select
            Editor.SceneEditing.Select(actor);
        }

        private void OnSelectionChanged()
        {
            var terrainNode = Editor.SceneEditing.SelectionCount > 0 ? Editor.SceneEditing.Selection[0] as FoliageNode : null;
            var foliage = terrainNode?.Actor as FlaxEngine.Foliage;
            if (foliage != SelectedFoliage)
            {
                SelectedFoliage = foliage;
                SelectedFoliageChanged?.Invoke();
            }

            _noFoliagePanel.Visible = foliage == null;
        }

        private void InitSculptMode()
        {
            var tab = _modes.AddTab(FoliageTypes = new FoliageTypesTab(this));
            tab.Selected += OnTabSelected;
        }

        private void InitPaintMode()
        {
            var tab = _modes.AddTab(Paint = new PaintTab(this));
            tab.Selected += OnTabSelected;
        }

        private void InitEditMode()
        {
            var tab = _modes.AddTab(Edit = new EditTab(this));
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
                Editor.Windows.EditWin.Viewport.SetActiveMode<NoGizmoMode>();
                break;
            case 1:
                Editor.Windows.EditWin.Viewport.SetActiveMode<NoGizmoMode>();
                break;
            case 2:
                Editor.Windows.EditWin.Viewport.SetActiveMode<NoGizmoMode>();
                break;
            default: throw new IndexOutOfRangeException("Invalid foliage tab mode.");
            }
        }
    }
}
