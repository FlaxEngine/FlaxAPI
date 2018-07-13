// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEditor.Content;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.GUI;
using Utils = FlaxEditor.Utilities.Utils;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Opening/editing script source file and project.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class CodeEditingModule : EditorModule
    {
        private readonly List<ScriptItem> _scripts = new List<ScriptItem>();
        private readonly List<Type> _controlTypes = new List<Type>();
        private bool _hasValidScripts, _hasValidControlTypes;

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

        /// <inheritdoc />
        public override void OnInit()
        {
            Editor.ContentDatabase.ItemAdded += ContentDatabaseOnItemAdded;
            Editor.ContentDatabase.ItemRemoved += ContentDatabaseOnItemRemoved;
            ScriptsBuilder.ScriptsReload += OnScriptsReload;
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            Editor.ContentDatabase.ItemAdded -= ContentDatabaseOnItemAdded;
            Editor.ContentDatabase.ItemRemoved -= ContentDatabaseOnItemRemoved;
            ScriptsBuilder.ScriptsReload -= OnScriptsReload;
        }

        private void OnScriptsReload()
        {
            // Invalidate cached script items
            _hasValidScripts = false;
            _scripts.Clear();
            _hasValidControlTypes = false;
            _controlTypes.Clear();
        }

        private void ContentDatabaseOnItemAdded(ContentItem contentItem)
        {
            if (_hasValidScripts && contentItem is ScriptItem script && script.IsValid)
                _scripts.Add(script);
        }

        private void ContentDatabaseOnItemRemoved(ContentItem contentItem)
        {
            if (contentItem is ScriptItem script)
                _scripts.Remove(script);
        }

        private void FindScripts(ContentFolder folder)
        {
            for (int i = 0; i < folder.Children.Count; i++)
            {
                if (folder.Children[i] is ContentFolder subFolder)
                    FindScripts(subFolder);
                else if (folder.Children[i] is ScriptItem script && script.IsValid)
                    _scripts.Add(script);
            }
        }

        /// <summary>
        /// Gets the scripts from the project (valid ones).
        /// </summary>
        /// <returns>The scripts collection (readonly).</returns>
        public List<ScriptItem> GetScripts()
        {
            if (!_hasValidScripts)
            {
                _scripts.Clear();
                _hasValidScripts = true;

                FindScripts(Editor.ContentDatabase.ProjectSource.Folder);
            }

            return _scripts;
        }

        /// <summary>
        /// Gets the collection of the Control types that can be spawned in the game (valid ones).
        /// </summary>
        /// <returns>The Control types collection (readonly).</returns>
        public List<Type> GetControlTypes()
        {
            if (!_hasValidControlTypes)
            {
                _controlTypes.Clear();
                _hasValidControlTypes = true;

                Utils.GetDerivedTypes(typeof(Control), _controlTypes, IsTypeValidControlType, HasAssemblyValidControlTypes);
            }

            return _controlTypes;
        }

        private bool HasAssemblyValidControlTypes(Assembly a)
        {
            return a.GetName().Name != "FlaxEditor";
        }

        private bool IsTypeValidControlType(Type t)
        {
            return !t.IsAbstract && !Attribute.IsDefined(t, typeof(HideInEditorAttribute), false);
        }
    }
}
