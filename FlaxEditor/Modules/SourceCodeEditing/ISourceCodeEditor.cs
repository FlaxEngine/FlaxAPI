// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Modules.SourceCodeEditing
{
    /// <summary>
    /// Interface for source code editing plugins.
    /// </summary>
    public interface ISourceCodeEditor
    {
        /// <summary>
        /// Gets the editor name. Used to show in the UI.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Opens the solution file (source code project file).
        /// </summary>
        void OpenSolution();

        /// <summary>
        /// Opens the source file.
        /// </summary>
        /// <param name="path">The file path to open.</param>
        /// <param name="line">The line number to navigate to. Use 0 to not use it.</param>
        void OpenFile(string path, int line);

        /// <summary>
        /// Called when editor gets selected.
        /// </summary>
        /// <param name="editor">The editor.</param>
        void OnSelected(Editor editor);

        /// <summary>
        /// Called when editor gets deselected.
        /// </summary>
        /// <param name="editor">The editor.</param>
        void OnDeselected(Editor editor);

        /// <summary>
        /// Called when editor gets added.
        /// </summary>
        /// <param name="editor">The editor.</param>
        void OnAdded(Editor editor);

        /// <summary>
        /// Called when editor gets removed.
        /// </summary>
        /// <param name="editor">The editor.</param>
        void OnRemoved(Editor editor);
    }
}
