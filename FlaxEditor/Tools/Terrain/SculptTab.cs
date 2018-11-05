// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEditor.Tools.Terrain.Sculpt;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Carve tab related to terrain carving. Allows to modify terrain height and visibility using brush.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    internal class SculptTab : Tab
    {
        /// <summary>
        /// The object for sculp mode settings adjusting via Custom Editor.
        /// </summary>
        private sealed class ProxyObject
        {
            private readonly SculptTerrainGizmoMode _mode;

            public ProxyObject(SculptTerrainGizmoMode mode)
            {
                _mode = mode;
            }

            [EditorOrder(0), EditorDisplay("Tool"), Tooltip("Sculpt tool mode to use.")]
            public SculptTerrainGizmoMode.ModeTypes ToolMode
            {
                get => _mode.ToolModeType;
                set => _mode.ToolModeType = value;
            }

            [EditorOrder(100), EditorDisplay("Tool", EditorDisplayAttribute.InlineStyle), VisibleIf("IsSculptMode")]
            public SculptMode AsSculptMode
            {
                get => _mode.SculptMode;
                set { }
            }

            private bool IsSculptMode => _mode.ToolModeType == SculptTerrainGizmoMode.ModeTypes.Sculpt;

            [EditorOrder(100), EditorDisplay("Tool", EditorDisplayAttribute.InlineStyle), VisibleIf("IsSmoothMode")]
            public SmoothMode AsSmoothMode
            {
                get => _mode.SmoothMode;
                set { }
            }

            private bool IsSmoothMode => _mode.ToolModeType == SculptTerrainGizmoMode.ModeTypes.Smooth;

            [EditorOrder(1000), EditorDisplay("Brush"), Tooltip("Sculpt brush type to use.")]
            public SculptTerrainGizmoMode.BrushTypes BrushTypeType
            {
                get => _mode.ToolBrushType;
                set => _mode.ToolBrushType = value;
            }

            [EditorOrder(1100), EditorDisplay("Brush", EditorDisplayAttribute.InlineStyle), VisibleIf("IsCircleBrush")]
            public Brushes.CircleBrush AsCircleBrush
            {
                get => _mode.CircleBrush;
                set { }
            }

            private bool IsCircleBrush => _mode.ToolBrushType == SculptTerrainGizmoMode.BrushTypes.CircleBrush;
        }

        private readonly ProxyObject _proxy;

        /// <summary>
        /// The parent carve tab.
        /// </summary>
        public readonly CarveTab CarveTab;

        /// <summary>
        /// The related sculp terrain gizmo.
        /// </summary>
        public readonly SculptTerrainGizmoMode Gizmo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SculptTab"/> class.
        /// </summary>
        /// <param name="tab">The parent tab.</param>
        /// <param name="gizmo">The related gizmo.</param>
        public SculptTab(CarveTab tab, SculptTerrainGizmoMode gizmo)
        : base("Sculpt")
        {
            CarveTab = tab;
            Gizmo = gizmo;
            _proxy = new ProxyObject(gizmo);

            // Main panel
            var panel = new Panel(ScrollBars.Both)
            {
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            // Options editor
            // TODO: use editor undo for changing brush options
            var editor = new CustomEditorPresenter(null);
            editor.Panel.Parent = panel;
            editor.Select(_proxy);
        }
    }
}
