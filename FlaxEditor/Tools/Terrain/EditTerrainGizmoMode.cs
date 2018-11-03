// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Viewport;
using FlaxEditor.Viewport.Modes;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain management and editing tool. 
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.Modes.EditorGizmoMode" />
    internal class EditTerrainGizmoMode : EditorGizmoMode
    {
        /// <summary>
        /// The terrain properties editing modes.
        /// </summary>
        public enum Modes
        {
            /// <summary>
            /// Terrain chunks editing mode.
            /// </summary>
            Edit,

            /// <summary>
            /// Terrain patches adding mode.
            /// </summary>
            Add,

            /// <summary>
            /// Terrain patches removing mode.
            /// </summary>
            Remove
        }

        /// <summary>
        /// The terrain editing gizmo.
        /// </summary>
        public EditTerrainGizmo Gizmo;

        /// <summary>
        /// The patch coordinates of the last picked patch.
        /// </summary>
        public Int2 PatchCoord;

        /// <summary>
        /// The chunk coordinates (relative to the patch) of the last picked chunk.
        /// </summary>
        public Int2 ChunkCoord;

        /// <summary>
        /// The active edit mode.
        /// </summary>
        public Modes EditMode;

        /// <inheritdoc />
        public override void Init(MainEditorGizmoViewport viewport)
        {
            base.Init(viewport);

            EditMode = Modes.Edit;
            Gizmo = new EditTerrainGizmo(viewport, this);
        }

        /// <inheritdoc />
        public override void OnActivated()
        {
            base.OnActivated();

            Viewport.Gizmos.Active = Gizmo;
        }
    }
}
