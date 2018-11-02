// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Viewport;
using FlaxEditor.Viewport.Modes;

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

        /// <inheritdoc />
        public override void Init(MainEditorGizmoViewport viewport)
        {
            base.Init(viewport);

            Gizmo = new EditTerrainGizmo(viewport);
        }

        /// <inheritdoc />
        public override void OnActivated()
        {
            base.OnActivated();

            Viewport.Gizmos.Active = Gizmo;
        }
    }
}
