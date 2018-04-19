// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.IO;
using System.Text;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Context proxy object for script files (represented by <see cref="ScriptItem"/>).
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentProxy" />
    public class ScriptProxy : ContentProxy
    {
        /// <summary>
        /// The script files extension.
        /// </summary>
        public static readonly string Extension = "cs";
        
        /// <summary>
        /// The script files extension filter.
        /// </summary>
        public static readonly string ExtensionFiler = "*.cs";

        /// <inheritdoc />
        public override string Name => "Script";

        /// <inheritdoc />
        public override bool IsProxyFor(ContentItem item)
        {
            return item is ScriptItem;
        }

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            return targetLocation.CanHaveScripts;
        }

        /// <inheritdoc />
        public override void Create(string outputPath)
        {
            // Load template
            var templatePath = StringUtils.CombinePaths(Globals.EditorFolder, "Scripting/ScriptTemplate.cs");
            var scriptTemplate = File.ReadAllText(templatePath);
            var scriptNamespace = Editor.Instance.ProjectInfo.Name.Replace(" ", "");
            
            // Format
            var scriptName = ScriptItem.CreateScriptName(outputPath);
            scriptTemplate = scriptTemplate.Replace("%class%", scriptName);
            scriptTemplate = scriptTemplate.Replace("%namespace%", scriptNamespace);

            // Save
            File.WriteAllText(outputPath, scriptTemplate, Encoding.UTF8);
        }

        /// <inheritdoc />
        public override string FileExtension => Extension;

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            Editor.Instance.CodeEditing.OpenFile(item.Path);
            return null;
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x1c9c2b);
    }
}
