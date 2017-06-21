////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Main editor window used to modify scene objects. Provides Gizmos and camera viewport navigation.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public class EditGameWindow : EditorWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditGameWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public EditGameWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
        }
    }
}
