// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEditor.Tools.Terrain.Brushes;
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
            public enum Modes
            {
                Sculpt,
                Smooth,
            }

            public enum Brushes
            {
                CircleBrush,
            }

            private readonly Mode[] _modes =
            {
                new SculptMode(),
                new SmoothMode(),
            };

            private readonly Brush[] _brushes =
            {
                new CircleBrush(),
            };

            private Modes _mode = Modes.Sculpt;
            private Brushes _brush = Brushes.CircleBrush;

            [HideInEditor]
            public Mode CurrentMode => _modes[(int)_mode];

            [HideInEditor]
            public Brush CurrentBrush => _brushes[(int)_brush];

            [EditorOrder(0), EditorDisplay("Tool"), Tooltip("Sculpt tool mode to use.")]
            public Modes ToolMode
            {
                get => _mode;
                set
                {
                    if (_mode != value)
                    {
                        _mode = value;
                    }
                }
            }

            [EditorOrder(100), EditorDisplay("Tool", EditorDisplayAttribute.InlineStyle), VisibleIf("IsSculptMode")]
            public SculptMode AsSculptMode
            {
                get => (SculptMode)_modes[(int)Modes.Sculpt];
                set { }
            }

            private bool IsSculptMode => _mode == Modes.Sculpt;

            [EditorOrder(100), EditorDisplay("Tool", EditorDisplayAttribute.InlineStyle), VisibleIf("IsSmoothMode")]
            public SmoothMode AsSmoothMode
            {
                get => (SmoothMode)_modes[(int)Modes.Smooth];
                set { }
            }

            private bool IsSmoothMode => _mode == Modes.Smooth;

            [EditorOrder(1000), EditorDisplay("Brush"), Tooltip("Sculpt brush type to use.")]
            public Brushes BrushType
            {
                get => _brush;
                set
                {
                    if (_brush != value)
                    {
                        _brush = value;
                    }
                }
            }

            [EditorOrder(1100), EditorDisplay("Brush", EditorDisplayAttribute.InlineStyle), VisibleIf("IsCircleBrush")]
            public CircleBrush AsCircleBrush
            {
                get => (CircleBrush)_brushes[(int)Brushes.CircleBrush];
                set { }
            }

            private bool IsCircleBrush => _brush == Brushes.CircleBrush;
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
        /// Gets the current tool mode.
        /// </summary>
        public Mode CurrentMode => _proxy.CurrentMode;

        /// <summary>
        /// Gets the current brush type.
        /// </summary>
        public Brush CurrentBrush => _proxy.CurrentBrush;

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
            _proxy = new ProxyObject();

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
