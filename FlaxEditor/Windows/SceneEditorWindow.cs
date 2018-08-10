// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        public override bool OnKeyDown(Keys key)
        {
            // Base
            bool result = base.OnKeyDown(key);
            if (!result)
            {
                var parentWin = Root;
                if (parentWin.GetKey(Keys.Control))
                {
                    switch (key)
                    {
                    case Keys.S:
                        Editor.SaveAll();
                        return true;
                    case Keys.Z:
                        Editor.PerformUndo();
                        Focus();
                        return true;
                    case Keys.Y:
                        Editor.PerformRedo();
                        Focus();
                        return true;
                    case Keys.X:
                        Editor.SceneEditing.Cut();
                        break;
                    case Keys.C:
                        Editor.SceneEditing.Copy();
                        break;
                    case Keys.V:
                        Editor.SceneEditing.Paste();
                        break;
                    case Keys.D:
                        Editor.SceneEditing.Duplicate();
                        break;
                    case Keys.A:
                        Editor.SceneEditing.SelectAllScenes();
                        break;
                    case Keys.F:
                        Editor.Windows.SceneWin.Search();
                        break;
                    }
                }
                else
                {
                    switch (key)
                    {
                    case Keys.Delete:
                        Editor.SceneEditing.Delete();
                        break;

                    case Keys.F5:
                        Editor.Simulation.RequestStartPlay();
                        break;
                    case Keys.F6:
                        Editor.Simulation.RequestResumeOrPause();
                        break;
                    case Keys.F11:
                        Editor.Simulation.RequestPlayOneFrame();
                        break;
                    }
                }
            }

            return result;
        }
    }
}
