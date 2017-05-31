// Flax Engine scripting API

using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Tool helper class used to duplicate loaded scenes (backup them) and restore later. Used for play in-editor functionality.
    /// </summary>
    public class DuplicateScenes
    {
        private readonly List<Scene> _scenes = new List<Scene>();

        /// <summary>
        /// Checks if scene data has been gathered.
        /// </summary>
        /// <returns>True if has scene data, oherwise false.</returns>
        public bool HasData => _scenes.Count > 0;

        /// <summary>
        /// Collect loaded scenes data.
        /// </summary>
        public void GatherSceneData()
        {
            if (HasData)
                throw new InvalidOperationException("DuplicateScenes has already gathered scene data.");
            /*
            LOG(Info, "Collecting scene data");

            // Note: old scene is that before play mode and new scene is that during play mode
            // Duplicate scenes for steps:
            // - serialize old scenes
            // - unregister old scene objects (unregister managed objects, physic bodies, etc.)
            // - deserialzie new scene
            // - call onBegin and all events on new scene (also create and register new scene managed objects)
            // Note: old scene will remain live (both managed and unmanaged objects) but will be hidden to engine (unlinked scene actor and unregistred objects)
            // This gives us that benefit of only single scene serialization-deserialization.

            // Get loaded scenes
            var scenes = SceneManager.Scenes;
            int scenesCount = scenes.Length;
            if (scenesCount == 0)
                throw new InvalidOperationException("Cannot gather scene data.No scene loaded.");

            // Cache 'old' scenes
            _scenes.AddRange(scenes);

            // Serialize scenes (to raw bytes - do it quickly!)
            var scenesData = new byte[scenesCount][];
            for (int i = 0; i < scenesCount; i++)
            {
                scenesData[i] = SceneManager.SaveSceneToBytes(scenes[i]);
            }

            // Unregister old scenes (managed objects, unlink scene, etc.)
            for (int i = 0; i < scenesCount; i++)
            {
                scenes[i].Unregister();
            }

            // Ensure that old scenes has been unregistered
            {
                var noScenes = SceneManager.Scenes;
                if (noScenes != null && noScenes.Length != 0)
                    throw new FlaxException("Failed to unregister scene objects.");
            }

            // Deserialize scenes (create clones)
            var duplicatedScenes = new byte[scenesCount][];
            for (int i = 0; i < scenesCount; i++)
            {
                duplicatedScenes[i] = SceneManager.LoadSceneFromBytes(scenesData[i]);
            }

            // Begin play
            for (int i = 0; i < scenesCount; i++)
            {
                duplicatedScenes[i].OnBeginPlay();
            }

            LOG(Info, "Gathered {(scenesCount)} scene(s)!");*/
        }

        /// <summary>
        /// Restore captured scene data.
        /// </summary>
        public void RestoreSceneData()
        {
            if (!HasData)
                throw new InvalidOperationException("DuplicateScenes has not gathered scene data yet.");
            /*
            LOG(Info, "Restoring scene data");

            var duplicatedScenes = SceneManager.Scenes;

            // TODO: here we can keep changes for actors marked to keep their state after simulation

            // Delete duplicated scenes
            for (int i = 0; i < duplicatedScenes.Length; i++)
                SceneManager.DeleteActor(duplicatedScenes[i]);

            // TODO: collect GC?
            // TODO: flush deleted actors?

            // Register old scene objects
            for (int i = 0; i < _scenes.Count; i++)
            {
                _scenes[i].Register();
            }
            _scenes.Clear();

            LOG(Info, "Restored previous scenes");*/
        }
    }
}
