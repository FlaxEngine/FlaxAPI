// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain carving tab. Supports different modes for terrain editing including: carving, painting and managing tools.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    internal class CarveTab : Tab
    {
        private readonly Editor _editor;
        private readonly Tabs _modes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarveTab"/> class.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <param name="editor">The editor instance.</param>
        public CarveTab(Sprite icon, Editor editor)
        : base(string.Empty, icon)
        {
            _editor = editor;

            _modes = new Tabs
            {
                Orientation = Orientation.Vertical,
                UseScroll = true,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(50, 32),
                Parent = this
            };

            InitSculpMode();
            InitPaintMode();
            InitEditMode();

            _modes.SelectedTabIndex = 0;
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

        private void InitEditMode()
        {
            var tab = _modes.AddTab(new Tab("Edit"));
            tab.Selected += OnTabSelected;
            var panel = new Panel(ScrollBars.Both)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };

            // Mode: Edit, Add, Remove
            // Patch
            // - x - z - terrain actor name
            // - bounds
            // TODO: moving patch
            // Chunk
            // - x - y
            // - bounds
            // - OverrideMaterial
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
                _editor.Windows.EditWin.Viewport.SetActiveMode<SculpTerrainGizmoMode>();
                break;
            case 1:
                _editor.Windows.EditWin.Viewport.SetActiveMode<PaintTerrainGizmoMode>();
                break;
            case 2:
                _editor.Windows.EditWin.Viewport.SetActiveMode<EditTerrainGizmoMode>();
                break;
            default: throw new IndexOutOfRangeException("Invalid carve tab mode.");
            }
        }
    }
}
