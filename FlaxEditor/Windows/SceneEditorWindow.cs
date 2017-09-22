////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    ///     Base class for editor windows dedicated to scene editing.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public abstract class SceneEditorWindow : EditorWindow
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SceneEditorWindow" /> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="hideOnClose">True if hide window on closing, otherwise it will be destroyed.</param>
        /// <param name="scrollBars">The scroll bars.</param>
        protected SceneEditorWindow(Editor editor, bool hideOnClose, ScrollBars scrollBars)
            : base(editor, hideOnClose, scrollBars)
        {
            AddCommandsToController();
        }

        /// <summary>
        ///     Saves all changes.
        /// </summary>
        public void SaveAll()
        {
            Editor.SaveAll();
        }

        /// <summary>
        /// Adds prepared list <see cref="InputCommand"/> to <see cref="InputCommandsController"/>
        /// </summary>
        protected virtual void AddCommandsToController()
        {
            CommandsController.Add(new InputCommand(Editor.SaveAll, new InputChord(KeyCode.Control, KeyCode.S)));
            CommandsController.Add(new InputCommand(Editor.PerformUndo, new InputChord(KeyCode.Control, KeyCode.Z)));
            CommandsController.Add(new InputCommand(() =>
            {
                Editor.PerformRedo();
                Focus();
            }, new InputChord(KeyCode.Control, KeyCode.Y)));
            CommandsController.Add(new InputCommand(Editor.SceneEditing.Cut, new InputChord(KeyCode.Control, KeyCode.X)));
            CommandsController.Add(new InputCommand(Editor.SceneEditing.Copy, new InputChord(KeyCode.Control, KeyCode.C)));
            CommandsController.Add(new InputCommand(() => { Editor.SceneEditing.Paste(); }, new InputChord(KeyCode.Control, KeyCode.V)));
            CommandsController.Add(new InputCommand(Editor.SceneEditing.Duplicate, new InputChord(KeyCode.Control, KeyCode.D)));
            CommandsController.Add(new InputCommand(Editor.SceneEditing.SelectAllScenes, new InputChord(KeyCode.Control, KeyCode.A)));
            CommandsController.Add(new InputCommand(() => { Editor.Windows.SceneWin.Search(); }, new InputChord(KeyCode.Control, KeyCode.F)));
            CommandsController.Add(new InputCommand(Editor.SceneEditing.Delete, new InputChord(KeyCode.Delete)));
        }

        /// <inheritdoc />
        public override bool OnKeyHold(InputChord key)
        {
            // Base
            if (base.OnKeyHold(key) || !IsFocused)
            {
                return true;
            }
            return CommandsController.KeyHold(key);
        }

        /// <inheritdoc />
        public override bool OnKeyPressed(InputChord key)
        {
            // Base
            if (base.OnKeyPressed(key) || !IsFocused)
            {
                return true;
            }
            return CommandsController.KeyPressed(key);
        }
    }
}