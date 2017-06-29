////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Gizmo;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Viewport with free camera and gizmo tools.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorViewportFPSCam" />
    public class EditorGizmoViewport : EditorViewportFPSCam, IGizmoOwner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorGizmoViewport"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public EditorGizmoViewport(SceneRenderTask task)
            : base(task, true)
        {
        }

        public GizmosCollection Gizmos { get; } = new GizmosCollection();
        public bool IsLeftMouseButtonDown => _isMouseLeftDown;
        public bool IsRightMouseButtonDown => _isMouseRightDown;


    }
}
