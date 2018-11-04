// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Viewport;
using FlaxEditor.Viewport.Modes;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain carving tool mode.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.Modes.EditorGizmoMode" />
    public class SculptTerrainGizmoMode : EditorGizmoMode
    {
        /// <summary>
        /// The terrain carving gizmo.
        /// </summary>
        public SculptTerrainGizmo Gizmo;

        /// <summary>
        /// The last valid cursor position of the brush (in world space).
        /// </summary>
        public Vector3 CursorPosition { get; private set; }

        /// <summary>
        /// Flag used to indicate whenever last cursor position of the brush is valid.
        /// </summary>
        public bool HasValidHit { get; private set; }

        /// <inheritdoc />
        public override void Init(MainEditorGizmoViewport viewport)
        {
            base.Init(viewport);

            Gizmo = new SculptTerrainGizmo(viewport, this);
        }

        /// <inheritdoc />
        public override void OnActivated()
        {
            base.OnActivated();

            Viewport.Gizmos.Active = Gizmo;
            ClearCursor();
        }

        /// <summary>
        /// Clears the cursor location information cached within the gizmo mode.
        /// </summary>
        public void ClearCursor()
        {
            HasValidHit = false;
        }

        /// <summary>
        /// Sets the cursor location in the world space. Updates the brush location and cached affected chunks.
        /// </summary>
        /// <param name="hitPosition">The cursor hit location on the selected terrain.</param>
        public void SetCursor(ref Vector3 hitPosition)
        {
            HasValidHit = true;
            CursorPosition = hitPosition;
        }
    }
}
