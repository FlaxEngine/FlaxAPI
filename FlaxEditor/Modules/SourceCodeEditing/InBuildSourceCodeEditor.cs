// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEditor.Modules.SourceCodeEditing
{
    /// <summary>
    /// In-build source code editor.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.SourceCodeEditing.ISourceCodeEditor" />
    internal class InBuildSourceCodeEditor : ISourceCodeEditor
    {
        /// <summary>
        /// The type of the editor.
        /// </summary>
        public readonly CodeEditorTypes Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="InBuildSourceCodeEditor"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public InBuildSourceCodeEditor(CodeEditorTypes type)
        {
            Type = type;

            switch (type)
            {
            case CodeEditorTypes.Custom:
                Name = "Custom";
                break;
            case CodeEditorTypes.SystemDefault:
                Name = "System Default";
                break;
            case CodeEditorTypes.VS2008:
                Name = "Visual Studio 2008";
                break;
            case CodeEditorTypes.VS2010:
                Name = "Visual Studio 2010";
                break;
            case CodeEditorTypes.VS2012:
                Name = "Visual Studio 2012";
                break;
            case CodeEditorTypes.VS2013:
                Name = "Visual Studio 2013";
                break;
            case CodeEditorTypes.VS2015:
                Name = "Visual Studio 2015";
                break;
            case CodeEditorTypes.VS2017:
                Name = "Visual Studio 2017";
                break;
            case CodeEditorTypes.VS2019:
                Name = "Visual Studio 2019";
                break;
            default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public void OpenSolution()
        {
            CodeEditingManager.OpenSolution(Type);
        }

        /// <inheritdoc />
        public void OpenFile(string path, int line)
        {
            CodeEditingManager.OpenFile(Type, path, line);
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
        }

        /// <inheritdoc />
        public void OnRemoved(Editor editor)
        {
        }
    }
}
