// Flax Engine scripting API

using FlaxEditor.Scripting;

namespace FlaxEditor.States
{
    /// <summary>
    /// Editor loading initial state
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class LoadingState : EditorState
    {
        private bool _loadScritpsFlag;

        /// <inheritdoc />
        public override bool CanEditContent => false;

        /// <inheritdoc />
        public override bool IsEditorReady => false;

        /// <inheritdoc />
        public override bool CanReloadScripts => true;

        internal LoadingState(Editor editor)
            : base(editor)
        {
        }

        /// <summary>
        /// Starts the Editor initialization process ending.
        /// </summary>
        internal void StartInitEnding()
        {
            ScriptsBuilder.OnCompilationEnd += onCompilationEnd;

            // Check source code has been cmpilled on start
            if (ScriptsBuilder.CompilationsCount > 0)
            {
                // Check if compilation has been ended
                if (ScriptsBuilder.IsReady)
                {
                    // We assume source code has been compilled before Editor init
                    onCompilationEnd(true);
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

        private void onCompilationEnd(bool success)
        {
            // Check if compilation success
            if (success)
            {
                // Request loading scripts (we need to do this on main thread)
                _loadScritpsFlag = true;
            }
            else
            {
                // Compilation failed so just end init
                Editor.EndInit();
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
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            ScriptsBuilder.OnCompilationEnd -= onCompilationEnd;
        }
    }
}
