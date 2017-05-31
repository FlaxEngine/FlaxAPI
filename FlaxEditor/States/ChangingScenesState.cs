// Flax Engine scripting API

using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state editor is changing loaded scenes collection.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class ChangingScenesState : EditorState
    {
        private readonly List<Guid> _scenesToLoad = new List<Guid>();
        private readonly List<Guid> _scenesToUnload = new List<Guid>();

        internal ChangingScenesState(Editor editor)
            : base(editor)
        {
        }

        /// <summary>
        /// Loads the scene.
        /// </summary>
        /// <param name="sceneId">The scene asset ID.</param>
        /// <param name="additive">True if don't close opened scenes and just add new scene to the collection, otherwise will release current scenes and load single one.</param>
        public void LoadScene(Guid sceneId, bool additive = false)
        {
            // Clear request
            _scenesToLoad.Clear();
            _scenesToUnload.Clear();

            // Setup request
            _scenesToLoad.Add(sceneId);
            var scenes = SceneManager.Scenes;
            for (int i = 0; i < scenes.Length; i++)
                _scenesToUnload.Add(scenes[i].ID);

            // Request state change
            StateMachine.GoToState(this);
        }

        /// <summary>
        /// Unloades the scene.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public void UnloadScene(Scene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            UnloadScene(scene.ID);
        }

        /// <summary>
        /// Unloades the scene.
        /// </summary>
        /// <param name="sceneId">The scene ID.</param>
        public void UnloadScene(Guid sceneId)
        {
            // Clear request
            _scenesToLoad.Clear();
            _scenesToUnload.Clear();

            // Setup request
            _scenesToUnload.Add(sceneId);

            // Request state change
            StateMachine.GoToState(this);
        }
        
        /// <summary>
        /// Changes the scenes.
        /// </summary>
        /// <param name="toLoad">Scenes to load.</param>
        /// <param name="toUnload">Scenes to unload.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ChangeScenes(IEnumerable<Guid> toLoad, IEnumerable<Guid> toUnload)
        {
            // Clear request
            _scenesToLoad.Clear();
            _scenesToUnload.Clear();

            // Setup request
            _scenesToLoad.AddRange(toLoad);
            _scenesToUnload.AddRange(toUnload);

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
