// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlaxEditor.Options;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.GUI;
using Utils = FlaxEditor.Utilities.Utils;

namespace FlaxEditor.Modules.SourceCodeEditing
{
    /// <summary>
    /// Source code editing module.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class CodeEditingModule : EditorModule
    {
        private readonly List<ISourceCodeEditor> _editors = new List<ISourceCodeEditor>(8);
        private ISourceCodeEditor _selectedEditor;
        private readonly List<Type> _scripts = new List<Type>();
        private readonly List<Type> _controlTypes = new List<Type>();
        private bool _hasValidScripts, _hasValidControlTypes;

        /// <summary>
        /// Gets the source code editors registered for usage in editor.
        /// </summary>
        public IReadOnlyList<ISourceCodeEditor> Editors => _editors;

        /// <summary>
        /// Occurs when source code editor gets added.
        /// </summary>
        public event Action<ISourceCodeEditor> EditorAdded;

        /// <summary>
        /// Occurs when source code editor gets removed.
        /// </summary>
        public event Action<ISourceCodeEditor> EditorRemoved;

        /// <summary>
        /// Occurs when selected source code editor gets changed.
        /// </summary>
        public event Action<ISourceCodeEditor> SelectedEditorChanged;

        /// <summary>
        /// Gets or sets the selected editor.
        /// </summary>
        public ISourceCodeEditor SelectedEditor
        {
            get => _selectedEditor;
            set
            {
                if (_selectedEditor != value)
                {
                    if (value != null && !_editors.Contains(value))
                        throw new ArgumentException("Cannot selected editor that has not been added to the editors list.");

                    var prev = _selectedEditor;
                    _selectedEditor = value;

                    Editor.Log("Select code editor " + (_selectedEditor?.Name ?? "null"));

                    prev?.OnDeselected(Editor);
                    _selectedEditor?.OnSelected(Editor);
                    SelectedEditorChanged?.Invoke(_selectedEditor);
                }
            }
        }

        internal CodeEditingModule(Editor editor)
        : base(editor)
        {
        }

        internal InBuildSourceCodeEditor GetInBuildEditor(ScriptsBuilder.InBuildEditorTypes editorType)
        {
            for (var i = 0; i < Editors.Count; i++)
            {
                if (Editors[i] is InBuildSourceCodeEditor inBuild && inBuild.Type == editorType)
                    return inBuild;
            }
            return null;
        }

        /// <summary>
        /// Adds the editor to the collection.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public void AddEditor(ISourceCodeEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException();
            if (_editors.Contains(editor))
                throw new ArgumentException("Editor already added.");

            Editor.Log("Add code editor " + editor.Name);

            _editors.Add(editor);
            editor.OnAdded(Editor);
            EditorAdded?.Invoke(editor);
        }

        /// <summary>
        /// Removes the editor from the collection.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public void RemoveEditor(ISourceCodeEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException();
            if (!_editors.Contains(editor))
                throw new ArgumentException("Editor not added.");

            if (_selectedEditor == editor)
                SelectedEditor = null;

            Editor.Log("Remove code editor " + editor.Name);

            _editors.Remove(editor);
            editor.OnRemoved(Editor);
            EditorRemoved?.Invoke(editor);
        }

        /// <summary>
        /// Opens the solution file using the selected selected code editor.
        /// </summary>
        public void OpenSolution()
        {
            if (_selectedEditor == null)
                return;

            _selectedEditor.OpenSolution();
        }

        /// <summary>
        /// Opens the file using the selected code editor.
        /// </summary>
        /// <param name="path">The file path to open.</param>
        /// <param name="line">The line number to navigate to. Use 0 to not use it.</param>
        public void OpenFile(string path, int line = 0)
        {
            if (_selectedEditor == null)
                return;

            _selectedEditor.OpenFile(path, line);
        }

        private unsafe void GetInBuildEditors()
        {
            var resultArray = stackalloc byte[(int)ScriptsBuilder.InBuildEditorTypes.MAX];
            ScriptsBuilder.Internal_GetExistingEditors(resultArray);
            for (int i = 0; i < (int)ScriptsBuilder.InBuildEditorTypes.MAX; i++)
            {
                if (resultArray[i] != 0)
                {
                    var editor = new InBuildSourceCodeEditor((ScriptsBuilder.InBuildEditorTypes)i);
                    AddEditor(editor);
                }
            }
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            ScriptsBuilder.ScriptsReload += OnScriptsReload;
            Editor.Options.OptionsChanged += OnOptionsChanged;

            // Add default code editors (in-build)
            GetInBuildEditors();
            AddEditor(new DefaultSourceCodeEditor());

            // Editor options are loaded before this module so pick the code editor
            OnOptionsChanged(Editor.Options.Options);
        }

        private void OnOptionsChanged(EditorOptions options)
        {
            // Sync code editor
            ISourceCodeEditor editor = null;
            var codeEditor = options.SourceCode.SourceCodeEditor;
            if (codeEditor != "None")
            {
                foreach (var e in Editor.Instance.CodeEditing.Editors)
                {
                    if (string.Equals(codeEditor, e.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        editor = e;
                        break;
                    }
                }
            }
            Editor.Instance.CodeEditing.SelectedEditor = editor;
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // Cleanup
            SelectedEditor = null;
            _editors.Clear();

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

        /// <summary>
        /// Gets all the script types from the all loaded assemblies (including project scripts and scripts from the plugins).
        /// </summary>
        /// <returns>The script types collection (readonly).</returns>
        public List<Type> GetScripts()
        {
            if (!_hasValidScripts)
            {
                _scripts.Clear();
                _hasValidScripts = true;

                Editor.Log("Searching valid script types");
                var start = DateTime.Now;
                Utils.GetDerivedTypes(typeof(Script), _scripts, IsTypeValidScriptType, HasAssemblyValidScriptTypes);
                var end = DateTime.Now;
                Editor.Log(string.Format("Found {0} script types (in {1} ms)", _scripts.Count, (int)(end - start).TotalMilliseconds));
            }

            return _scripts;
        }

        private bool HasAssemblyValidScriptTypes(Assembly a)
        {
            // Skip editor
            if (a.GetName().Name == "FlaxEditor")
                return false;

            // Skip assemblies not referencing engine
            var references = a.GetReferencedAssemblies();
            return references.Any(x => x.Name == "FlaxEngine");
        }

        private bool IsTypeValidScriptType(Type t)
        {
            return !t.IsGenericType && !t.IsAbstract;
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

                Editor.Log("Searching valid control types");
                var start = DateTime.Now;
                Utils.GetDerivedTypes(typeof(Control), _controlTypes, IsTypeValidControlType, HasAssemblyValidControlTypes);
                var end = DateTime.Now;
                Editor.Log(string.Format("Found {0} control types (in {1} ms)", _controlTypes.Count, (int)(end - start).TotalMilliseconds));
            }

            return _controlTypes;
        }

        private bool HasAssemblyValidControlTypes(Assembly a)
        {
            var name = a.GetName();

            // Skip editor
            if (name.Name == "FlaxEditor")
                return false;

            // Use engine
            if (name.Name == "FlaxEngine")
                return true;

            // Skip assemblies not referencing engine
            var references = a.GetReferencedAssemblies();
            return references.Any(x => x.Name == "FlaxEngine");
        }

        private bool IsTypeValidControlType(Type t)
        {
            return !t.IsGenericType && !t.IsAbstract && !Attribute.IsDefined(t, typeof(HideInEditorAttribute), false);
        }
    }
}
