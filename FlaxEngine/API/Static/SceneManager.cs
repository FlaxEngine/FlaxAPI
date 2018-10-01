// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        /// Checks if any scene has been loaded. Loaded scene means deserialized and added to the scenes collection.
        /// </summary>
        public static bool IsAnySceneLoaded => ScenesCount != 0;

        /// <summary>
        /// Fired when scene starts saving.
        /// </summary>
        public static event SceneDelegate SceneSaving;

        /// <summary>
        /// Fired when scene gets saved.
        /// </summary>
        public static event SceneDelegate SceneSaved;

        /// <summary>
        /// Fired when scene gets saving error.
        /// </summary>
        public static event SceneDelegate SceneSaveError;

        /// <summary>
        /// Fired when scene starts loading.
        /// </summary>
        public static event SceneDelegate SceneLoading;

        /// <summary>
        /// Fired when scene gets loaded.
        /// </summary>
        public static event SceneDelegate SceneLoaded;

        /// <summary>
        /// Fired when scene cannot be loaded.
        /// </summary>
        public static event SceneDelegate SceneLoadError;

        /// <summary>
        /// Fired when scene gets unloading.
        /// </summary>
        public static event SceneDelegate SceneUnloading;

        /// <summary>
        /// Fired when scene gets unloaded.
        /// </summary>
        public static event SceneDelegate SceneUnloaded;

        /// <summary>
        /// Occurs when new actor gets spawned to the game.
        /// </summary>
        public static event ActorDelegate ActorSpawned;

        /// <summary>
        /// Occurs when actor is removed from the game.
        /// </summary>
        public static event ActorDelegate ActorDeleted;

        /// <summary>
        /// Occurs when actor parent gets changed.
        /// </summary>
        public static event ActorParentChangedDelegate ActorParentChanged;

        /// <summary>
        /// Occurs when actor order in parent gets changed.
        /// </summary>
        public static event ActorDelegate ActorOrderInParentChanged;

        /// <summary>
        /// Occurs when actor name gets changed.
        /// </summary>
        public static event ActorDelegate ActorNameChanged;

        /// <summary>
        /// Occurs when actor IsActive state gets changed.
        /// </summary>
        public static event ActorDelegate ActorActiveChanged;

        // Called internally from C++
        internal enum SceneEventType
        {
            Saving = 0,
            Saved = 1,
            SaveError = 2,
            Loading = 3,
            Loaded = 4,
            LoadError = 5,
            Unloading = 6,
            Unloaded = 7,
        }

        internal static void Internal_OnSceneEvent(SceneEventType eventType, Scene scene, ref Guid sceneId)
        {
            switch (eventType)
            {
            case SceneEventType.Saving:
                SceneSaving?.Invoke(scene, sceneId);
                break;
            case SceneEventType.Saved:
                SceneSaved?.Invoke(scene, sceneId);
                break;
            case SceneEventType.SaveError:
                SceneSaveError?.Invoke(scene, sceneId);
                break;
            case SceneEventType.Loading:
                SceneLoading?.Invoke(scene, sceneId);
                break;
            case SceneEventType.Loaded:
                SceneLoaded?.Invoke(scene, sceneId);
                break;
            case SceneEventType.LoadError:
                SceneLoadError?.Invoke(scene, sceneId);
                break;
            case SceneEventType.Unloading:
                SceneUnloading?.Invoke(scene, sceneId);
                break;
            case SceneEventType.Unloaded:
                SceneUnloaded?.Invoke(scene, sceneId);
                break;
            }
        }

        internal enum ActorEventType
        {
            Spawned = 0,
            Deleted = 1,
            ParentChanged = 2,
            OrderInParentChanged = 3,
            NameChanged = 4,
            ActiveChanged = 5,
        }

        internal static void Internal_OnActorEvent(ActorEventType eventType, Actor a, Actor b)
        {
            switch (eventType)
            {
            case ActorEventType.Spawned:
                ActorSpawned?.Invoke(a);
                break;
            case ActorEventType.Deleted:
                ActorDeleted?.Invoke(a);
                break;
            case ActorEventType.ParentChanged:
                ActorParentChanged?.Invoke(a, b);
                break;
            case ActorEventType.OrderInParentChanged:
                ActorOrderInParentChanged?.Invoke(a);
                break;
            case ActorEventType.NameChanged:
                ActorNameChanged?.Invoke(a);
                break;
            case ActorEventType.ActiveChanged:
                ActorActiveChanged?.Invoke(a);
                break;
            }
        }

        /// <summary>
        /// Unlaods all active scenes and loads the given scene (in the background).
        /// </summary>
        /// <param name="sceneAssetId">The scene asset identifier (scene to load).</param>
        /// <returns>True if action fails (given asset is not a scene asset, missing data, scene loading error), otherwise false.</returns>
        public static bool ChangeSceneAsync(Guid sceneAssetId)
        {
            UnloadAllScenesAsync();
            return LoadSceneAsync(sceneAssetId);
        }

        /// <summary>
        /// Unlaods all active scenes and loads the given scene (in the background).
        /// </summary>
        /// <param name="sceneAsset">The asset with the scene to load.</param>
        /// <returns>True if action fails (given asset is not a scene asset, missing data, scene loading error), otherwise false.</returns>
        public static bool ChangeSceneAsync(SceneReference sceneAsset)
        {
            return ChangeSceneAsync(sceneAsset.ID);
        }

        /// <summary>
        /// Loads scene from the asset.
        /// </summary>
        /// <param name="sceneAsset">The asset with the scene to load.</param>
        /// <returns>True if action fails (given asset is not a scene asset, missing data, scene loading error), otherwise false.</returns>
        public static bool LoadScene(SceneReference sceneAsset)
        {
            return LoadScene(sceneAsset.ID);
        }

        /// <summary>
        /// Loads scene from the asset. Done in the background.
        /// </summary>
        /// <param name="sceneAsset">The asset with the scene to load.</param>
        /// <returns>True if failed (given asset is not a scene asset, missing data), otherwise false.</returns>
        public static bool LoadSceneAsync(SceneReference sceneAsset)
        {
            return LoadSceneAsync(sceneAsset.ID);
        }
    }
}
