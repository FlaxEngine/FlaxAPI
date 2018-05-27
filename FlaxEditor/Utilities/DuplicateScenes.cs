// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Tool helper class used to duplicate loaded scenes (backup them) and restore later. Used for play in-editor functionality.
    /// </summary>
    public class DuplicateScenes
    {
        private struct SceneData
        {
            public bool IsDirty;
            public byte[] Bytes;
        }

        private readonly List<SceneData> _scenesData = new List<SceneData>(16);

        /// <summary>
        /// Checks if scene data has been gathered.
        /// </summary>
        public bool HasData => _scenesData.Count > 0;

        /// <summary>
        /// Gets a value indicating whether any scene was dirty before gathering.
        /// </summary>
        public bool WasDirty => _scenesData.Any(x => x.IsDirty);

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
            var sceneIds = scenes.ToList().ConvertAll(x => x.ID);

            // Serialize scenes
            _scenesData.Capacity = scenesCount;
            for (int i = 0; i < scenesCount; i++)
            {
                var scene = scenes[i];
                var data = new SceneData
                {
                    IsDirty = Editor.Instance.Scene.IsEdited(scene),
                    Bytes = SceneManager.SaveSceneToBytes(scene),
                };
                _scenesData.Add(data);
            }

            // Delete old scenes
            if (SceneManager.UnloadAllScenes())
                throw new FlaxException("Failed to unload scenes.");
            FlaxEngine.Scripting.FlushRemovedObjects();

            // Ensure that old scenes has been unregistered
            {
                var noScenes = SceneManager.Scenes;
                if (noScenes != null && noScenes.Length != 0 && sceneIds.TrueForAll(x => Object.Find<Object>(ref x) == null))
                    throw new FlaxException("Failed to unregister scene objects.");
            }

            Editor.Log(string.Format("Gathered {0} scene(s)!", scenesCount));
        }

        /// <summary>
        /// Deserialize captured scenes.
        /// </summary>
        public void CreateScenes()
        {
            if (!HasData)
                throw new InvalidOperationException("DuplicateScenes has not gathered scene data yet.");

            Editor.Log("Creating scenes");

            // Deserialize new scenes
            int scenesCount = _scenesData.Count;
            var duplicatedScenes = new Scene[scenesCount];
            for (int i = 0; i < scenesCount; i++)
            {
                var data = _scenesData[i];
                duplicatedScenes[i] = SceneManager.LoadSceneFromBytes(data.Bytes);
                if (duplicatedScenes[i] == null)
                    throw new FlaxException("Failed to deserialize scene");
            }
        }

        /// <summary>
        /// Deletes the creates scenes for the simulation.
        /// </summary>
        public void DeletedScenes()
        {
            Editor.Log("Restoring scene data");

            // TODO: here we can keep changes for actors marked to keep their state after simulation

            // Delete new scenes
            if (SceneManager.UnloadAllScenes())
                throw new FlaxException("Failed to unload scenes.");
            FlaxEngine.Scripting.FlushRemovedObjects();
        }

        /// <summary>
        /// Restore captured scene data.
        /// </summary>
        public void RestoreSceneData()
        {
            if (!HasData)
                throw new InvalidOperationException("DuplicateScenes has not gathered scene data yet.");

            // Deserialize old scenes
            for (int i = 0; i < _scenesData.Count; i++)
            {
                var data = _scenesData[i];
                var scene = SceneManager.LoadSceneFromBytes(data.Bytes);
                if (scene == null)
                    throw new FlaxException("Failed to deserialize scene");

                // Restore `dirty` state
                if (data.IsDirty)
                    Editor.Instance.Scene.MarkSceneEdited(scene);
            }
            _scenesData.Clear();

            Editor.Log("Restored previous scenes");
        }
    }
}
