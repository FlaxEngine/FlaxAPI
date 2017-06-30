// Flax Engine scripting API

using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Scene/prefabs/actors management module.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class SceneModule : EditorModule
    {
        internal SceneModule(Editor editor)
            : base(editor)
        {
        }
        /*
        /// <summary>
        /// Marks the scene as modified.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public void MarkSceneEdited(Scene scene)
        {
            editedScenes.Add(scene);
        }

        /// <summary>
        /// Marks all the scenes as modified.
        /// </summary>
        public void MarkAllScenesEdited()
        {
            var scenes = SceneManager.Scenes;
            for (int i = 0; i < scenes.Length; i++)
                editedScenes.Add(scenes[i]);
        }

        /// <summary>
        /// Determines whether the specified scene is edited.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns>
        ///   <c>true</c> if the specified scene is edited; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEdited(Scene scene)
        {
            return editedScenes.Contains(scene);
        }

        /// <summary>
        /// Determines whether any scene is edited.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if any scene is edited; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEdited()
        {
            return editedScenes.Count != 0;
        }*/

        /// <summary>
        /// Creates the scene file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void CreateSceneFile(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves scene (async).
        /// </summary>
        /// <param name="scene">Scene to save.</param>
        public void SaveScene(Scene scene)
        {
            /*// Early out
            if (IsEdited(scene) == false)
                return;

            SceneManager.SaveSceneAsync(scene);*/
        }
        
        /// <summary>
        /// Saves all open scenes (async).
        /// </summary>
        public void SaveScenes()
        {
            /*// Early out
            if (IsEdited() == false)
                return;

            SceneManager.SaveAllScenesAsync();*/
        }

        /// <summary>
        /// Opens scene (async).
        /// </summary>
        /// <param name="sceneId">Scene ID</param>
        /// <param name="additive">True if don't close opened scenes and jus tadd new scene to them, otherwise will release current scenes and load single one.</param>
        public void OpenScene(Guid sceneId, bool additive = false)
        {
            // Check if cannot change scene now
            if (!Editor.StateMachine.CurrentState.CanChangeScene)
                return;

            // Ensure to save all pending changes
            if (CheckSaveBeforeClose())
                return;

            // Load scene
            Editor.StateMachine.ChangingScenesState.LoadScene(sceneId, additive);
        }

        /// <summary>
        /// Closes scene (async).
        /// </summary>
        /// <param name="scene">The scene.</param>
        public void CloseScene(Scene scene)
        {
            // Check if cannot change scene now
            if (!Editor.StateMachine.CurrentState.CanChangeScene)
                return;

            // Ensure to save all pending changes
            if (CheckSaveBeforeClose())
                return;

            // Unload scene
            Editor.StateMachine.ChangingScenesState.UnloadScene(scene);
        }

        /// <summary>
        /// Show save before scene load/unload action.
        /// </summary>
        /// <returns>True if action has been canceled, otherwise false</returns>
        public bool CheckSaveBeforeClose()
        {
            //throw new NotImplementedException();
            // TODO: suspend auto save for a while
            // Suspend auto saves
            //SuspendAutoSave();
            /*
            // TODO: restore old code
            LOG(Warning, 0, TEXT("SceneModule::CheckSaveBeforeClose()"));
            // Check if scene was edited after last saving
            if (CEditor->IsEdited() && SceneManager::Instance()->IsAnySceneLoaded())
            {
                // Ask user for futher action
                auto result = MessageBox::Show(
                    LocalizationData::FormatEditorMessage(136, SceneManager::Instance()->GetLastSceneFilename()),
                    LocalizationData::GetEditorMessage(134),
                    MessageBoxButtons::YesNoCancel,
                    MessageBoxIcon::Question
                );
                if (result == DialogResult::OK || result == DialogResult::Yes)
                {
                    // Save and close
                    SaveScene();
                }
                else if (result == DialogResult::Cancel || result == DialogResult::Abort)
                {
                    // Cancel closing
                    return true;
                }
            }

            // Clear Editor's data
            auto gizmo = CEditor->GetMainGizmo();
            if (gizmo)
                gizmo->Deselect();
            CEditor->UndoRedo.ClearHistory();
            */
            return false;
        }

        /// <summary>
        /// Saves the opened scenes collection to editor cache.
        /// </summary>
        private void saveOpeneScenes()
        {
            //throw new NotImplementedException();
            // TODO: save collection of last scenes instead of single scene, use scene IDs
            /*MemoryWriteStream lastSceneStream(PLATFORM_MAX_FILEPATH_LENGTH);
	        lastSceneStream.WriteString(SceneManager::Instance()->GetLastScenePath());
	        EditorCache::Instance()->Set(TEXT("LastScene"), false, &lastSceneStream);*/
        }
        
        /// <inheritdoc />
        public override void OnEndInit()
        {
            //throw new NotImplementedException();
            // Check last opened scenes
            // TODO: load collection of last scenes instead of single scene, use scene IDs
            /*MemoryReadStream lastSceneStream;
            if (EditorCache::Instance()->Get(TEXT("LastScene"), false, &lastSceneStream))
            {
                // Get scene name and try to load
                String lastScene;
                lastSceneStream.ReadString(&lastScene);
                if (lastScene.HasChars())
                {
                    // Check if file stil exists
                    if (FileSystem::FileExists(lastScene))
                    {
                        // Change scene
                        CEditor->StateMachine->ChangingSceneState.LoadScene(lastScene);
                    }
                    else
                    {
                        // Warning
                        LOG(Warning, 121, lastScene);
                    }
                }
            }*/
        }
    }
}
