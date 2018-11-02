// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI;
using FlaxEditor.Viewport.Modes;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain carving tab. Supports different modes for terrain editing including: carving, painting and managing tools.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    internal class CarveTab : Tab
    {
        private readonly Editor _editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarveTab"/> class.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <param name="editor">The editor instance.</param>
        public CarveTab(Sprite icon, Editor editor)
        : base(string.Empty, icon)
        {
            _editor = editor;
        }

        /// <inheritdoc />
        public override void OnSelected()
        {
            base.OnSelected();

            UpdateGizmoMode();
        }

        /// <summary>
        /// Updates the active viewport gizmo mode based on the current mode.
        /// </summary>
        private void UpdateGizmoMode()
        {
            _editor.Windows.EditWin.Viewport.SetActiveMode<PaintTerrainGizmoMode>();
        }
    }
}
