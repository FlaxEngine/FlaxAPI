////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Windows;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Main edior gizmo viewport used by the <see cref="EditGameWindow"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorGizmoViewport" />
    public class MainEditorGizmoViewport : EditorGizmoViewport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainEditorGizmoViewport"/> class.
        /// </summary>
        public MainEditorGizmoViewport()
            : base(RenderTask.Create<SceneRenderTask>())
        {
            Task.Flags = ViewFlags.DefaultEditor;
        }
    }
}
