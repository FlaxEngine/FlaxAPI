////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.CustomEditors;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Window used to present collection of selected object(s) properties in a grid. Supports Undo/Redo operations.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.SceneEditorWindow" />
    public class PropertiesWindow : SceneEditorWindow
    {
        /// <summary>
        /// The editor.
        /// </summary>
        public readonly CustomEditorPresenter Presenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public PropertiesWindow(Editor editor)
            : base(editor, true, ScrollBars.Vertical)
        {
            Title = "Properties";

            Presenter = new CustomEditorPresenter();
            Presenter.Panel.DockStyle = DockStyle.Top;
            Presenter.Panel.Parent = this;

            Editor.SceneEditing.OnSelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            // Update selected objects
            var objects = Editor.SceneEditing.Selection.ConvertAll(x => x.EditableObject);
            Presenter.Select(objects);
        }
    }
}
