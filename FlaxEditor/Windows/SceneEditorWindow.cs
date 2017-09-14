////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Base class for editor windows dedicated to scene editing.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public abstract class SceneEditorWindow : EditorWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SceneEditorWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="hideOnClose">True if hide window on closing, otherwise it will be destroyed.</param>
        /// <param name="scrollBars">The scroll bars.</param>
        protected SceneEditorWindow(Editor editor, bool hideOnClose, ScrollBars scrollBars)
            : base(editor, hideOnClose, scrollBars)
        {
        }

        /// <summary>
        /// Saves all changes.
        /// </summary>
        public void SaveAll()
        {
            Editor.SaveAll();
        }

        /// <inheritdoc />
        public override bool OnKeyPressed(KeyCodeMap key)
        {
            // Base
            if (base.OnKeyPressed(key)) return true;

            if(key.InvokeFirstCommand(KeyCode.Control,
                new KeyCodeMap.KeyCommand(KeyCode.S, Editor.SaveAll),
                new KeyCodeMap.KeyCommand(KeyCode.Z, Editor.PerformUndo),
                new KeyCodeMap.KeyCommand(KeyCode.Y, () => { Editor.PerformRedo(); Focus(); }),
                new KeyCodeMap.KeyCommand(KeyCode.X, Editor.SceneEditing.Cut),
                new KeyCodeMap.KeyCommand(KeyCode.C, Editor.SceneEditing.Copy),
                new KeyCodeMap.KeyCommand(KeyCode.V, () => { Editor.SceneEditing.Paste(); }),
                new KeyCodeMap.KeyCommand(KeyCode.D, Editor.SceneEditing.Duplicate),
                new KeyCodeMap.KeyCommand(KeyCode.A, Editor.SceneEditing.SelectAllScenes),
                new KeyCodeMap.KeyCommand(KeyCode.F, Editor.Windows.SceneWin.Search)
            )){ return true; }
            return key.InvokeFirstCommand(new KeyCodeMap.KeyCommand(KeyCode.Delete, Editor.SceneEditing.Delete));
        }
    }
}
