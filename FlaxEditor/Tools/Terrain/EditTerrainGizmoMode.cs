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

        /// <inheritdoc />
        public override void Init(MainEditorGizmoViewport viewport)
        {
            base.Init(viewport);

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
