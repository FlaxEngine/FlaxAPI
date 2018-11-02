// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Gizmo;
using FlaxEditor.Viewport.Cameras;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Viewport with free camera and gizmo tools.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorViewport" />
    /// <seealso cref="IGizmoOwner" />
    public class EditorGizmoViewport : EditorViewport, IGizmoOwner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorGizmoViewport"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="undo">The undo.</param>
        public EditorGizmoViewport(SceneRenderTask task, Undo undo)
        : base(task, new FPSCamera(), true)
        {
            Undo = undo;
        }

        /// <inheritdoc />
        public GizmosCollection Gizmos { get; } = new GizmosCollection();

        /// <inheritdoc />
        public SceneRenderTask RenderTask => Task;

        /// <inheritdoc />
        public float ViewFarPlane => FarPlane;

        /// <inheritdoc />
        public bool IsLeftMouseButtonDown => _input.IsMouseLeftDown;

        /// <inheritdoc />
        public bool IsRightMouseButtonDown => _input.IsMouseRightDown;

        /// <inheritdoc />
        public bool IsAltKeyDown => _input.IsAltDown;

        /// <inheritdoc />
        public bool IsControlDown => _input.IsControlDown;

        /// <inheritdoc />
        public Vector2 MouseDelta => _mouseDeltaLeft * 1000;

        /// <inheritdoc />
        public bool UseSnapping => Root.GetKey(Keys.Control);

        /// <inheritdoc />
        public bool UseDuplicate => Root.GetKey(Keys.Shift);

        /// <inheritdoc />
        public Undo Undo { get; }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            for (int i = 0; i < Gizmos.Count; i++)
            {
                Gizmos[i].Update(deltaTime);
            }
        }
    }
}
