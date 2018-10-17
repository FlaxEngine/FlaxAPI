// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Scripting;

namespace FlaxEditor.Modules.SourceCodeEditing
{
    /// <summary>
    /// Default source code editor. Picks the best available editor on the current the platform.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.SourceCodeEditing.ISourceCodeEditor" />
    internal class DefaultSourceCodeEditor : ISourceCodeEditor
    {
        private ISourceCodeEditor _currentEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSourceCodeEditor"/> class.
        /// </summary>
        public DefaultSourceCodeEditor()
        {
        }

        private void OnEditorAdded(ISourceCodeEditor editor)
        {
            if (editor == this)
                return;

            UpdateCurrentEditor();
        }

        private void OnEditorRemoved(ISourceCodeEditor editor)
        {
            if (editor != _currentEditor)
                return;

            UpdateCurrentEditor();
        }

        private void UpdateCurrentEditor()
        {
            var codeEditing = Editor.Instance.CodeEditing;
            
            // Favor the newest Visual Studio
            for (int i = (int)ScriptsBuilder.InBuildEditorTypes.VS2017; i >= (int)ScriptsBuilder.InBuildEditorTypes.VS2008; i--)
            {
                var visualStudio = codeEditing.GetInBuildEditor((ScriptsBuilder.InBuildEditorTypes)i);
                if (visualStudio != null)
                {
                    _currentEditor = visualStudio;
                    return;
                }
            }

            // Fallback text editor (always valid)
            _currentEditor = codeEditing.GetInBuildEditor(ScriptsBuilder.InBuildEditorTypes.Text);
        }

        /// <inheritdoc />
        public string Name => "Default";

        /// <inheritdoc />
        public void OpenSolution()
        {
            _currentEditor?.OpenSolution();
        }

        /// <inheritdoc />
        public void OpenFile(string path, int line)
        {
            _currentEditor?.OpenFile(path, line);
        }

        /// <inheritdoc />
        public void OnSelected(Editor editor)
        {
        }

        /// <inheritdoc />
        public void OnDeselected(Editor editor)
        {
        }

        /// <inheritdoc />
        public void OnAdded(Editor editor)
        {
            editor.CodeEditing.EditorAdded += OnEditorAdded;
            editor.CodeEditing.EditorRemoved += OnEditorRemoved;

            UpdateCurrentEditor();
        }

        /// <inheritdoc />
        public void OnRemoved(Editor editor)
        {
            _currentEditor = null;

            editor.CodeEditing.EditorAdded -= OnEditorAdded;
            editor.CodeEditing.EditorRemoved -= OnEditorRemoved;
        }
    }
}
