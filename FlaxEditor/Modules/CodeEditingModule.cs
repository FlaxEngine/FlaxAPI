////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Scripting;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Opening/editing script source file and project.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class CodeEditingModule : EditorModule
    {
        internal CodeEditingModule(Editor editor)
            : base(editor)
        {
        }

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The source file path.</param>
        public void OpenFile(string path)
        {
            ScriptsBuilder.OpenFile(path);
        }
    }
}
