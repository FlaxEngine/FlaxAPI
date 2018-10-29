// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Scripting;

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
        public readonly ScriptsBuilder.InBuildEditorTypes Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="InBuildSourceCodeEditor"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public InBuildSourceCodeEditor(ScriptsBuilder.InBuildEditorTypes type)
        {
            Type = type;

            switch (type)
            {
            case ScriptsBuilder.InBuildEditorTypes.Custom:
                Name = "Custom";
                break;
            case ScriptsBuilder.InBuildEditorTypes.Text:
                Name = "Text Editor";
                break;
            case ScriptsBuilder.InBuildEditorTypes.VS2008:
                Name = "Visual Studio 2008";
                break;
            case ScriptsBuilder.InBuildEditorTypes.VS2010:
                Name = "Visual Studio 2010";
                break;
            case ScriptsBuilder.InBuildEditorTypes.VS2012:
                Name = "Visual Studio 2012";
                break;
            case ScriptsBuilder.InBuildEditorTypes.VS2013:
                Name = "Visual Studio 2013";
                break;
            case ScriptsBuilder.InBuildEditorTypes.VS2015:
                Name = "Visual Studio 2015";
                break;
            case ScriptsBuilder.InBuildEditorTypes.VS2017:
                Name = "Visual Studio 2017";
                break;
            default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public void OpenSolution()
        {
            ScriptsBuilder.Internal_OpenSolution(Type);
        }

        /// <inheritdoc />
        public void OpenFile(string path, int line)
        {
            ScriptsBuilder.Internal_OpenFile(Type, path, line);
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
