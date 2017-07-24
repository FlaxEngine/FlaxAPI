////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Scene actions delegate type.
    /// </summary>
    /// <param name="scene">The scene object. It may be null!</param>
    /// <param name="sceneId">The scene ID.</param>
    public delegate void SceneDelegate(Scene scene, Guid sceneId);

    /// <summary>
    /// Actor actors delegate type.
    /// </summary>
    /// <param name="actor">The actor.</param>
    public delegate void ActorDelegate(Actor actor);

    /// <summary>
    /// Actor parent changed delegate.
    /// </summary>
    /// <param name="actor">The actor.</param>
    /// <param name="prevParent">The previous parent.</param>
    public delegate void ActorParentChangedDelegate(Actor actor, Actor prevParent);

    public static partial class SceneManager
	{
        /// <summary>
        /// Fired when scene starts saving
        /// </summary>
        public static event SceneDelegate OnSceneSaving;

        /// <summary>
        /// Fired when scene gets saved
        /// </summary>
        public static event SceneDelegate OnSceneSaved;

        /// <summary>
        /// Fired when scene gets saving error
        /// </summary>
        public static event SceneDelegate OnSceneSaveError;

        /// <summary>
        /// Fired when scene starts loading
        /// </summary>
        public static event SceneDelegate OnSceneLoading;

        /// <summary>
        /// Fired when scene gets loaded
        /// </summary>
        public static event SceneDelegate OnSceneLoaded;

	    /// <summary>
	    /// Fired when scene cannot be loaded
	    /// </summary>
	    public static event SceneDelegate OnSceneLoadError;

        /// <summary>
        /// Fired when scene gets unloading
        /// </summary>
        public static event SceneDelegate OnSceneUnloading;

        /// <summary>
        /// Fired when scene gets unloaded
        /// </summary>
        public static event SceneDelegate OnSceneUnloaded;

        /// <summary>
        /// Occurs when new actor gets spawned to the game.
        /// </summary>
        public static event ActorDelegate OnActorSpawned;

        /// <summary>
        /// Occurs when actor is removed from the game.
        /// </summary>
        public static event ActorDelegate OnActorDeleted;

        /// <summary>
        /// Occurs when actor parent gets changed.
        /// </summary>
        public static event ActorParentChangedDelegate OnActorParentChanged;

        // Called internally from C++
        internal enum SceneEventType
	    {
	        OnSceneSaving = 0,
	        OnSceneSaved = 1,
	        OnSceneSaveError = 2,
	        OnSceneLoading = 3,
	        OnSceneLoaded = 4,
	        OnSceneLoadError = 5,
	        OnSceneUnloading = 6,
	        OnSceneUnloaded = 7,
	    }
        internal static void Internal_OnSceneEvent(SceneEventType eventType, Scene scene, ref Guid sceneId)
        {
            switch (eventType)
            {
                case SceneEventType.OnSceneSaving: OnSceneSaving?.Invoke(scene, sceneId); break;
                case SceneEventType.OnSceneSaved: OnSceneSaved?.Invoke(scene, sceneId); break;
                case SceneEventType.OnSceneSaveError: OnSceneSaveError?.Invoke(scene, sceneId); break;
                case SceneEventType.OnSceneLoading: OnSceneLoading?.Invoke(scene, sceneId); break;
                case SceneEventType.OnSceneLoaded: OnSceneLoaded?.Invoke(scene, sceneId); break;
                case SceneEventType.OnSceneLoadError: OnSceneLoadError?.Invoke(scene, sceneId); break;
                case SceneEventType.OnSceneUnloading: OnSceneUnloading?.Invoke(scene, sceneId); break;
                case SceneEventType.OnSceneUnloaded: OnSceneUnloaded?.Invoke(scene, sceneId); break;
            }
        }
	    internal enum ActorEventType
	    {
	        OnActorSpawned = 0,
	        OnActorDeleted = 1,
	        OnActorParentChanged = 2,
        }
        internal static void Internal_OnActorEvent(ActorEventType eventType, Actor a, Actor b)
	    {
	        switch (eventType)
	        {
	            case ActorEventType.OnActorSpawned: OnActorSpawned?.Invoke(a); break;
	            case ActorEventType.OnActorDeleted: OnActorDeleted?.Invoke(a); break;
	            case ActorEventType.OnActorParentChanged: OnActorParentChanged?.Invoke(a, b); break;
	        }
	    }
    }
}
