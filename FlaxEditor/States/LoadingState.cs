// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Scripting;
using FlaxEngine.Utilities;

namespace FlaxEditor.States
{
    /// <summary>
    /// Editor loading initial state
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class LoadingState : EditorState
    {
        private bool _loadScritpsFlag;
        private bool _compilationFailed;

        /// <inheritdoc />
        public override bool CanEditContent => false;

        /// <inheritdoc />
        public override bool IsEditorReady => false;

        /// <inheritdoc />
        public override bool CanReloadScripts => true;

        /// <inheritdoc />
        public override string Status => "Loading...";

        internal LoadingState(Editor editor)
        : base(editor)
        {
        }

        /// <summary>
        /// Starts the Editor initialization process ending.
        /// </summary>
        internal void StartInitEnding()
        {
            ScriptsBuilder.CompilationEnd += OnCompilationEnd;

            // Check source code has been compiled on start
            if (ScriptsBuilder.CompilationsCount > 0)
            {
                // Check if compilation has been ended
                if (ScriptsBuilder.IsReady)
                {
                    // We assume source code has been compiled before Editor init
                    OnCompilationEnd(true);
                }
            }
            else
            {
                // Compile scripts before loading any scenes
                ScriptsBuilder.Compile();

                // Note:
                // Here we wait for scripts compilation end
                // Later we want to load scripts
                // Finally enter normal state and load last opened scene
            }
        }

        private void OnCompilationEnd(bool success)
        {
            // Check if compilation success
            if (success)
            {
                // Request loading scripts (we need to do this on main thread)
                _loadScritpsFlag = true;
            }
            else
            {
                // Cannot compile user scripts, let's end init but on main thread in Update
                _compilationFailed = true;
            }
        }

        /// <inheritdoc />
        public override void Update()
        {
            // Check flag
            if (_loadScritpsFlag)
            {
                _loadScritpsFlag = false;

                // End init
                Editor.EndInit();
            }
            else if (_compilationFailed)
            {
                _compilationFailed = false;

                // Compilation failed so just end init
                Editor.EndInit();
            }
        }

        /// <inheritdoc />
        public override void OnExit(State nextState)
        {
            ScriptsBuilder.CompilationEnd -= OnCompilationEnd;
        }
    }
}
