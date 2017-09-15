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
        private readonly List<byte[]> _scenesData = new List<byte[]>();

        /// <summary>
        /// Checks if scene data has been gathered.
        /// </summary>
        /// <returns>True if has scene data, oherwise false.</returns>
        public bool HasData => _scenesData.Count > 0;

        /// <summary>
        /// Collect loaded scenes data.
        /// </summary>
        public void GatherSceneData()
        {
            if (HasData)
                throw new InvalidOperationException("DuplicateScenes has already gathered scene data.");

            Editor.Log("Collecting scene data");

            // Get loaded scenes
            var scenes = SceneManager.Scenes;
            int scenesCount = scenes.Length;
            if (scenesCount == 0)
                throw new InvalidOperationException("Cannot gather scene data. No scene loaded.");

            // Serialize scenes
            _scenesData.Capacity = scenesCount;
            for (int i = 0; i < scenesCount; i++)
            {
                _scenesData.Add(SceneManager.SaveSceneToBytes(scenes[i]));
            }

            // Delete old scenes
            if (SceneManager.UnloadAllScenes())
                throw new FlaxException("Failed to unload scenes.");

            // Ensure that old scenes has been unregistered
            {
                var noScenes = SceneManager.Scenes;
                if (noScenes != null && noScenes.Length != 0)
                    throw new FlaxException("Failed to unregister scene objects.");
            }

            // Deserialize new scenes
            var duplicatedScenes = new Scene[scenesCount];
            for (int i = 0; i < scenesCount; i++)
            {
                duplicatedScenes[i] = SceneManager.LoadSceneFromBytes(_scenesData[i]);
                if (duplicatedScenes[i] == null)
                    throw new FlaxException("Failed to deserialize scene");
            }

            Editor.Log(string.Format("Gathered {0} scene(s)!", scenesCount));
        }

        /// <summary>
        /// Restore captured scene data.
        /// </summary>
        public void RestoreSceneData()
        {
            if (!HasData)
                throw new InvalidOperationException("DuplicateScenes has not gathered scene data yet.");

            Editor.Log("Restoring scene data");

            // TODO: here we can keep changes for actors marked to keep their state after simulation

            // Delete new scenes
            if (SceneManager.UnloadAllScenes())
                throw new FlaxException("Failed to unload scenes.");

            // Deserialize oldd scenes
            for (int i = 0; i < _scenesData.Count; i++)
            {
                var scene = SceneManager.LoadSceneFromBytes(_scenesData[i]);
                if (scene == null)
                    throw new FlaxException("Failed to deserialize scene");
            }
            _scenesData.Clear();

            Editor.Log("Restored previous scenes");
        }
    }
}
