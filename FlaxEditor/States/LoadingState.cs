// Flax Engine scripting API

using System;

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

        /// <summary>
        /// Starts the Editor initialization process ending.
        /// </summary>
        internal void StartInitEnding()
        {
            // TODO: finish this
            throw new NotImplementedException();
            /*auto builder = ScriptsBuilder::Instance();
            builder->OnCompilationEnd.Bind<LoadingState, &LoadingState::onCompilationEnd>(this);

            // Check source code has been cmpilled on start
            if (builder->GetCompilationsCount() > 0)
            {
                // Check compilation has been ended
                if (builder->IsReady())
                {
                    // We assume source code has been compilled before Editor init
                    onCompilationEnd(true);
                }
            }
            else
            {
                // Compile scripts before loading scene
                builder->Compile();

                // Note:
                // Here we wait for scripts compilation end
                // Later we want to load scripts
                // Finally enter normal state and load last opened scene
            }*/
        }

        private void onCompilationEnd(bool success)
        {
            // TODO: finish this
            throw new NotImplementedException();
            // Check if compilation success
            /*if (success)
            {
                // Request loading scripts (we need to do this on main thread)
                _loadScritpsFlag = true;
            }
            else
            {
                // Compilation failed so just end init
                CEditor->OnEndInit();
            }*/
        }

        /// <inheritdoc />
        public override void Update()
        {
            // Check flag
            if (_loadScritpsFlag)
            {
                _loadScritpsFlag = false;

                // TODO: finish this
                throw new NotImplementedException();
                // Load scripts (we need use main thread for that action)
                //bool result = ScriptingEngine::Instance()->Load();

                // End init
                //CEditor->OnEndInit();
            }
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // TODO: finish this
            throw new NotImplementedException();
            //ScriptsBuilder::Instance()->OnCompilationEnd.Unbind<LoadingState, &LoadingState::onCompilationEnd>(this);
        }
    }
}
