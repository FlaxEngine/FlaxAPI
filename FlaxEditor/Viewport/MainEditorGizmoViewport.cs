////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Gizmo;
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
        private Editor _editor;

        /// <summary>
        /// The transform gizmo.
        /// </summary>
        public readonly TransformGizmo TransformGizmo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainEditorGizmoViewport"/> class.
        /// </summary>
        /// <param name="editor">Editor instance.</param>
        public MainEditorGizmoViewport(Editor editor)
            : base(RenderTask.Create<SceneRenderTask>())
        {
            _editor = editor;

            Task.Flags = ViewFlags.DefaultEditor;
            TransformGizmo = new TransformGizmo(this);
            Gizmos.Active = TransformGizmo;

            editor.SceneEditing.OnSelectionChanged += SceneEditingOnOnSelectionChanged;
        }

        private void SceneEditingOnOnSelectionChanged()
        {
            var selection = _editor.SceneEditing.Selection;
            Gizmos.ForEach(x => x.OnSelectionChanged(selection));
        }
    }
}
