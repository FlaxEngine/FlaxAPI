// Flax Engine scripting API

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.Utilities;

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state editor is changing loaded scenes collection.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class ChangingScenesState : EditorState
    {
        private readonly List<Guid> _scenesToLoad = new List<Guid>();
        private readonly List<Scene> _scenesToUnload = new List<Scene>();
        private Guid _lastSceneFromRequest;

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
            if (!additive)
            {
                _scenesToUnload.AddRange(SceneManager.Scenes);
            }

            // Request state change
            StateMachine.GoToState(this);
        }

        /// <summary>
        /// Unloades the scene.
        /// </summary>
        /// <param name="scene">The scene to unload.</param>
        public void UnloadScene(Scene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            // Clear request
            _scenesToLoad.Clear();
            _scenesToUnload.Clear();

            // Setup request
            _scenesToUnload.Add(scene);

            // Request state change
            StateMachine.GoToState(this);
        }
        
        /// <summary>
        /// Changes the scenes.
        /// </summary>
        /// <param name="toLoad">Scenes to load.</param>
        /// <param name="toUnload">Scenes to unload.</param>
        public void ChangeScenes(IEnumerable<Guid> toLoad, IEnumerable<Scene> toUnload)
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
            Assert.AreEqual(Guid.Empty, _lastSceneFromRequest, "Invalid state.");

            // Bind events
            SceneManager.OnSceneLoaded += onSceneEvent;
            SceneManager.OnSceneLoadError += onSceneEvent;
            SceneManager.OnSceneUnloaded += onSceneEvent;
            
            // Push scenes changing requests
            for (int i = 0; i < _scenesToUnload.Count; i++)
            {
                SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                _lastSceneFromRequest = _scenesToUnload[i].ID;
            }
            for (int i = 0; i < _scenesToLoad.Count; i++)
            {
                SceneManager.LoadSceneAsync(_scenesToLoad[i]);
                _lastSceneFromRequest = _scenesToLoad[i];
            }

            // Note: we user _lastSceneFromRequest to store id of the scene used for the last request (async scene requests are performed in an calling order)
            // Later we can detect this scene event and assume that all previous actions has been done.

            // Clear request
            _scenesToLoad.Clear();
            _scenesToUnload.Clear();
        }

        /// <inheritdoc />
        public override void OnExit(State nextState)
        {
            // Validate (but skip if next state is exit)
            if (!(nextState is ClosingState))
            {
                Assert.AreEqual(Guid.Empty, _lastSceneFromRequest, "Invalid state.");
            }

            // Unbind events
            SceneManager.OnSceneLoaded -= onSceneEvent;
            SceneManager.OnSceneLoadError -= onSceneEvent;
            SceneManager.OnSceneUnloaded -= onSceneEvent;
        }

        private void onSceneEvent(Scene scene, Guid sceneId)
        {
            // Check if it's scene from the last request
            if (sceneId == _lastSceneFromRequest)
            {
                _lastSceneFromRequest = Guid.Empty;

                StateMachine.GoToState<EditingSceneState>();
            }
        }
    }
}
