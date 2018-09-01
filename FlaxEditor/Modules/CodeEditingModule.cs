// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly List<Type> _scripts = new List<Type>();
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
            ScriptsBuilder.ScriptsReload += OnScriptsReload;
        }

        /// <inheritdoc />
        public override void OnExit()
        {
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
