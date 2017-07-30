// Flax Engine scripting API

using System;
using System.IO;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Scene/prefabs/actors management module.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class SceneModule : EditorModule
    {
        /// <summary>
        /// The root tree node for the whole scene graph.
        /// </summary>
        public readonly RootNode Root = new RootNode();

        /// <summary>
        /// The scene graph nodes factory.
        /// </summary>
        public readonly SceneGraphFactory Factory = new SceneGraphFactory();

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
        /// Creates the new scene file. The default scene contains set of simple actors.
        /// </summary>
        /// <param name="path">The path.</param>
        public void CreateSceneFile(string path)
        {
            Editor.Log(string.Format("Creating new scene to \'{0}\'", path));

            // Create a sample scene
            var scene = Scene.New();
            var sky = Sky.New();
            var sun = DirectionalLight.New();
            var floor = BoxBrush.New();
            //
            scene.AddChild(sky);
            scene.AddChild(sun);
            scene.AddChild(floor);
            //
            sky.Name = "Sky";
            sky.LocalPosition = new Vector3(0, 40, 0);
            sky.SunLight = sun;
            //
            sun.Name = "Sun";
            sun.LocalPosition = new Vector3(0, 30, 0);
            sun.LocaEulerAngles = new Vector3(45, 0, 0);
            //
            floor.Name = "Floor";
            floor.Size = new Vector3(200, 10, 200);

            // Serialize
            var bytes = SceneManager.SaveSceneToBytes(scene);

            // Cleanup
            Object.Destroy(scene);

            if (bytes == null || bytes.Length == 0)
                throw new Exception("Failed to serialize scene.");

            // Write to file
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                fileStream.Write(bytes, 0, bytes.Length);
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

        private void OnSceneLoaded(Scene scene, Guid sceneId)
        {
            var startTime = DateTime.UtcNow;

            // Build scene tree
            var sceneNode = Factory.BuildSceneTree(scene);
            sceneNode.TreeNode.Expand();

            // TODO: cache expanded/colapsed nodes per scene tree

            // Add to the tree
            var rootNode = Root.TreeNode;
            bool wasLayoutLocked = rootNode.IsLayoutLocked;
            rootNode.IsLayoutLocked = true;
            sceneNode.ParentNode = Root;
            rootNode.SortChildren();
            rootNode.IsLayoutLocked = wasLayoutLocked;
            rootNode.PerformLayout();

            var endTime = DateTime.UtcNow;
            var milliseconds = (int)(endTime - startTime).TotalMilliseconds;
            Editor.Log($"Created UI tree for scene \'{scene.Name}\' in {milliseconds} ms");
        }

        private void OnSceneUnloading(Scene scene, Guid sceneId)
        {
            // Find scene tree node
            var node = Root.FindChild(scene);
            if (node != null)
            {
                Editor.Log($"Cleanup UI tree for scene \'{scene.Name}\'");

                // Cleanup
                node.TreeNode.Dispose();
            }
        }

        private void OnActorSpawned(Actor actor)
        {
            var parent = actor.Parent;
            if (parent == null)
                return;

            var parentNode = GetActorNode(parent);
            if (parentNode == null)
            {
                // Error
                Debug.LogError("Failed to find parent node for actor " + actor.Name);
                return;
            }

            var node = Factory.BuildActorNode(actor);
            node.ParentNode = parentNode;
        }

        private void OnActorDeleted(Actor actor)
        {
            var node = GetActorNode(actor);
            if (node != null)
            {
                // Cleanup part of the graph
                node.Dispose();
            }
        }

        private void OnActorParentChanged(Actor actor, Actor prevParent)
        {
            ActorNode node = null;

            // Try use previous parent actor to find actor node
            var prevParentNode = GetActorNode(prevParent);
            if (prevParentNode != null)
            {
                // If should be one of the children
                node = prevParentNode.FindChild(actor);

                // Search whole tree if node was not found
                if (node == null)
                {
                    node = Root.Find(actor);
                    if (node == null)
                        return;
                }
            }
            else
            {
                // Create new node for that actor (user may unlink it from the scene before and now link it)
                node = Factory.BuildActorNode(actor);
            }

            // Get the new parent node (may be missing)
            var parentNode = GetActorNode(actor.Parent);
            if (parentNode != null)
            {
                // Change parent
                node.ParentNode = parentNode;
            }
            else
            {
                // Remove node (user may unlink actor from the scene but not destroy the actor)
                node.Dispose();
            }
        }

        /// <summary>
        /// Gets the actor node.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>Foudn actor node or null if missing. Actor may not be linked to the scene tree so node won't be found in that case.</returns>
        public ActorNode GetActorNode(Actor actor)
        {
            if (actor == null)
                return null;

            // Special case if actor is a scene
            if (actor is Scene)
                return Root.FindChild(actor) as SceneNode;

            // Get scene node
            var scene = actor.Scene;
            var sceneNode = Root.FindChild(scene);
            if (sceneNode == null)
                return null;

            // Early out for not linked actors - only scene actors may have missing parent
            if (actor.Parent == null)
                return null;

            // TODO: if it's a bottleneck use some actor nodes caching or sth (cache dictionary per scene tree, use Actor.ID for lookup)

            // Find actural actor node
            return sceneNode.Find(actor);
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            // Bind events
            SceneManager.OnSceneLoaded += OnSceneLoaded;
            SceneManager.OnSceneUnloading += OnSceneUnloading;
            SceneManager.OnActorSpawned += OnActorSpawned;
            SceneManager.OnActorDeleted += OnActorDeleted;
            SceneManager.OnActorParentChanged += OnActorParentChanged;
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

        /// <inheritdoc />
        public override void OnExit()
        {
            // Unbind events
            SceneManager.OnSceneLoaded -= OnSceneLoaded;
            SceneManager.OnSceneUnloading -= OnSceneUnloading;
            SceneManager.OnActorSpawned -= OnActorSpawned;
            SceneManager.OnActorDeleted -= OnActorDeleted;
            SceneManager.OnActorParentChanged -= OnActorParentChanged;

            // Cleanup graph
            Root.Dispose();
        }
    }
}
