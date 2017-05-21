// Flax Engine scripting API

using System;
using System.Collections.Generic;

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state editor is changing loaded scenes collection.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class ChangingScenesState : EditorState
    {
        private readonly List<String> _scenesToLoad = new List<string>();
        private readonly List<String> _scenesToUnload = new List<string>();
        private bool _unlaodAllScenes;

        /// <summary>
        /// Loads the scene. Unloads all previews scenes.
        /// </summary>
        /// <param name="path">The path to the scene.</param>
        public void LoadScene(String path)
        {
            // TODO: finish this
            throw new NotImplementedException();

            //LOG_EDITOR(Info, 75, path);

            // Set request
            _scenesToLoad.Clear();
            _scenesToLoad.Add(path);
            _scenesToUnload.Clear();
            _unlaodAllScenes = true;

            // Request state change
            StateMachine.GoToState(this);
        }

        /// <summary>
        /// Changes the scenes.
        /// </summary>
        /// <param name="toLoad">To load.</param>
        /// <param name="toUnload">To unload.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ChangeScenes(IEnumerable<String> toLoad, IEnumerable<String> toUnload)
        {
            // TODO: finish this
            throw new NotImplementedException();

            //LOG_EDITOR(Info, 75, path);

            // Set request
            _scenesToLoad.Clear();
            _scenesToLoad.AddRange(toLoad);
            _scenesToUnload.Clear();
            _scenesToUnload.AddRange(toUnload);
            _unlaodAllScenes = false;

            // Request state change
            StateMachine.GoToState(this);
        }

        /// <inheritdoc />
        public override bool CanEditContent => false;

        /// <inheritdoc />
        public override void OnEnter()
        {
            // TODO: finish this
            throw new NotImplementedException();

            // Bind events
            //SceneManager::Instance()->OnSceneLoaded.Bind<ChangingSceneState, &ChangingSceneState::onSceneLoaded>(this);
            //SceneManager::Instance()->OnSceneLoadError.Bind<ChangingSceneState, &ChangingSceneState::onSceneLoadError>(this);
            //SceneManager::Instance()->OnSceneLoading.Bind<ChangingSceneState, &ChangingSceneState::onSceneLoading>(this);
            //SceneManager::Instance()->OnSceneUnloaded.Bind<ChangingSceneState, &ChangingSceneState::onSceneUnloaded>(this);
            //SceneManager::Instance()->OnSceneUnloading.Bind<ChangingSceneState, &ChangingSceneState::onSceneUnloading>(this);

            /*
	// Check if has no scripting loaded
	if (!ScriptingEngine::Instance()->IsEveryAssemblyLoaded() || ScriptingEngine::Instance()->IsAnyAssemblyLoading())
	{
#if USE_EDITOR
		LOG_EDITOR(EditorError, 145);
		return true;
#endif
	}
	*/


            // TODO: load more than one scene
            // TOOD: support ultiscenes
            // Load scene
            /*bool result = SceneManager::Instance()->Load(_sceneToLoad);

            // Cleanup request
            _sceneToLoad.Clear();

            // Check if failed
            if (result)
            {
                // Error
                GetParent()->GoTo(EditorStates::EditingScene);
                LOG(EditorError, 77, 5712);
            }*/
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // TODO: finish this
            throw new NotImplementedException();

            // Unbind events
            //SceneManager::Instance()->OnSceneLoaded.Unbind<ChangingSceneState, &ChangingSceneState::onSceneLoaded>(this);
            //SceneManager::Instance()->OnSceneLoadError.Unbind<ChangingSceneState, &ChangingSceneState::onSceneLoadError>(this);
            //SceneManager::Instance()->OnSceneLoading.Unbind<ChangingSceneState, &ChangingSceneState::onSceneLoading>(this);
            //SceneManager::Instance()->OnSceneUnloaded.Unbind<ChangingSceneState, &ChangingSceneState::onSceneUnloaded>(this);
            //SceneManager::Instance()->OnSceneUnloading.Unbind<ChangingSceneState, &ChangingSceneState::onSceneUnloading>(this);
        }

        // TODO: finsih those events implementation
        /*void onSceneLoading()
        {
        }

        void onSceneLoaded()
        {
            GetParent()->GoTo(EditorStates::EditingScene);
        }

        void onSceneUnloading()
        {
        }

        void onSceneUnloaded()
        {
        }

        void onSceneLoadError(SceneManager::LoadResult result)
        {
            GetParent()->GoTo(EditorStates::EditingScene);
        }*/
    }
}
