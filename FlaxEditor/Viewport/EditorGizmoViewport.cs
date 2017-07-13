////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Gizmo;
using FlaxEngine;
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
            task.OnDraw += OnDraw;
        }

        private void OnDraw(DrawCallsCollector collector)
        {
            // Draw gizmo
            Gizmos.Active?.Draw(collector);
        }

        /// <inheritdoc />
        public GizmosCollection Gizmos { get; } = new GizmosCollection();

        /// <inheritdoc />
        public float ViewFarPlane => _farPlane;

        /// <inheritdoc />
        public bool IsLeftMouseButtonDown => _input.IsMouseLeftDown;

        /// <inheritdoc />
        public bool IsRightMouseButtonDown => _input.IsMouseRightDown;

        /// <inheritdoc />
        public Vector2 MouseDelta => _mouseDeltaLeft * 1000;

        /// <inheritdoc />
        public bool UseSnapping => ParentWindow.GetKey(KeyCode.CONTROL);

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Update gizmo
            Gizmos.Active?.Update(deltaTime);
        }
    }
}
