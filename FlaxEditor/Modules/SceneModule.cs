// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
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
        /// Occurs when actor gets removed. Editor and all submodules should remove references to that actor.
        /// </summary>
        public event Action<ActorNode> ActorRemoved;

        internal SceneModule(Editor editor)
            : base(editor)
        {
        }
        
        /// <summary>
        /// Marks the scene as modified.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public void MarkSceneEdited(Scene scene)
        {
            MarkSceneEdited(GetActorNode(scene) as SceneNode);
        }

        /// <summary>
        /// Marks the scene as modified.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public void MarkSceneEdited(SceneNode scene)
        {
            if (scene != null)
                scene.IsEdited = true;
        }

        /// <summary>
        /// Marks the scenes as modified.
        /// </summary>
        /// <param name="scenes">The scenes.</param>
        public void MarkSceneEdited(IEnumerable<Scene> scenes)
        {
            foreach (var scene in scenes)
                MarkSceneEdited(scene);
        }

        /// <summary>
        /// Marks all the scenes as modified.
        /// </summary>
        public void MarkAllScenesEdited()
        {
            MarkSceneEdited(SceneManager.Scenes);
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
            var node = GetActorNode(scene) as SceneNode;
            return node?.IsEdited ?? false;
        }

        /// <summary>
        /// Determines whether any scene is edited.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if any scene is edited; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEdited()
        {
            foreach (var scene in Root.ChildNodes)
            {
                if (scene is SceneNode node && node.IsEdited)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether every scene is edited.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if every scene is edited; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEverySceneEdited()
        {
            foreach (var scene in Root.ChildNodes)
            {
                if (scene is SceneNode node && !node.IsEdited)
                    return false;
            }
            return true;
        }

	    /// <summary>
	    /// Creates the new scene file. The default scene contains set of simple actors.
	    /// </summary>
	    /// <param name="path">The path.</param>
	    public void CreateSceneFile(string path)
	    {
		    // Create a sample scene
		    var scene = Scene.New();
		    var sky = Sky.New();
		    var sun = DirectionalLight.New();
		    var skyLight = SkyLight.New();
		    var floor = ModelActor.New();
		    var cam = Camera.New();
		    //
		    scene.StaticFlags = StaticFlags.FullyStatic;
		    scene.AddChild(sky);
		    scene.AddChild(sun);
		    scene.AddChild(skyLight);
		    scene.AddChild(floor);
		    scene.AddChild(cam);
		    //
		    sky.Name = "Sky";
		    sky.LocalPosition = new Vector3(40, 150, 0);
		    sky.SunLight = sun;
		    sky.StaticFlags = StaticFlags.FullyStatic;
		    //
		    sun.Name = "Sun";
		    sun.Brightness = 4.0f;
		    sun.LocalPosition = new Vector3(40, 160, 0);
		    sun.LocalEulerAngles = new Vector3(45, 0, 0);
		    sun.StaticFlags = StaticFlags.FullyStatic;
		    //
		    skyLight.Mode = SkyLight.Modes.CustomTexture;
		    skyLight.Brightness = 1.8f;
		    skyLight.CustomTexture = FlaxEngine.Content.LoadAsyncInternal<CubeTexture>(EditorAssets.DefaultSkyCubeTexture);
		    skyLight.StaticFlags = StaticFlags.FullyStatic;
		    //
		    floor.Name = "Floor";
		    floor.Scale = new Vector3(4, 0.5f, 4);
		    floor.Model = FlaxEngine.Content.LoadAsync<Model>(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Cube.flax"));
		    if (floor.Model)
		    {
			    floor.Model.WaitForLoaded();
			    floor.Entries[0].Material = FlaxEngine.Content.LoadAsync<MaterialBase>(StringUtils.CombinePaths(Globals.EngineFolder, "WhiteMaterial.flax"));
		    }
		    floor.StaticFlags = StaticFlags.FullyStatic;
		    //
		    cam.Name = "Camera";
		    cam.Position = new Vector3(0, 150, -300);

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
            SaveScene(GetActorNode(scene) as SceneNode);
        }

        /// <summary>
        /// Saves scene (async).
        /// </summary>
        /// <param name="scene">Scene to save.</param>
        public void SaveScene(SceneNode scene)
        {
            if (!scene.IsEdited)
                return;

            scene.IsEdited = false;
            SceneManager.SaveSceneAsync(scene.Scene);
        }

        /// <summary>
        /// Saves all open scenes (async).
        /// </summary>
        public void SaveScenes()
        {
            if (!IsEdited())
                return;

            foreach (var scene in Root.ChildNodes)
            {
                if (scene is SceneNode node)
                    node.IsEdited = false;
            }
            SceneManager.SaveAllScenesAsync();
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

            if (!additive)
            {
                // Ensure to save all pending changes
                if (CheckSaveBeforeClose())
                    return;
            }

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
        /// Closes all opened scene (async).
        /// </summary>
        public void CloseAllScenes()
        {
            // Check if cannot change scene now
            if (!Editor.StateMachine.CurrentState.CanChangeScene)
                return;

            // Ensure to save all pending changes
            if (CheckSaveBeforeClose())
                return;
            
            // Unload scenes
            Editor.StateMachine.ChangingScenesState.UnloadScene(SceneManager.Scenes);
        }

        /// <summary>
        /// Show save before scene load/unload action.
        /// </summary>
        /// <param name="scene">The scene that will be closed.</param>
        /// <returns>True if action has been canceled, otherwise false</returns>
        public bool CheckSaveBeforeClose(SceneNode scene)
        {
            // Suspend auto saves
            SuspendAutoSave();
            
            // Check if scene was edited after last saving
            if (scene.IsEdited)
            {
                // Ask user for futher action
                var result = MessageBox.Show(
                    string.Format("Scene \'{0}\' has been edited. Save before closing?", scene.Name),
                    "Close without saving?",
                    MessageBox.Buttons.YesNoCancel,
                    MessageBox.Icon.Question
                );
                if (result == DialogResult.OK || result == DialogResult.Yes)
                {
                    // Save and close
                    SaveScene(scene);
                }
                else if (result == DialogResult.Cancel || result == DialogResult.Abort)
                {
                    // Cancel closing
                    return true;
                }
            }

            ClearRefsToSceneObjects();

            return false;
        }

        /// <summary>
        /// Show save before scene load/unload action.
        /// </summary>
        /// <returns>True if action has been canceled, otherwise false</returns>
        public bool CheckSaveBeforeClose()
        {
            // Suspend auto saves
            SuspendAutoSave();

            // Check if scene was edited after last saving
            if (IsEdited())
            {
                // Ask user for futher action
                var scenes = SceneManager.Scenes;
                var result = MessageBox.Show(
                    scenes.Length == 1 ? string.Format("Scene \'{0}\' has been edited. Save before closing?", scenes[0].Name) : string.Format("{0} scenes have been edited. Save before closing?", scenes.Length),
                    "Close without saving?",
                    MessageBox.Buttons.YesNoCancel,
                    MessageBox.Icon.Question
                );
                if (result == DialogResult.OK || result == DialogResult.Yes)
                {
                    // Save and close
                    SaveScenes();
                }
                else if (result == DialogResult.Cancel || result == DialogResult.Abort)
                {
                    // Cancel closing
                    return true;
                }
            }

            ClearRefsToSceneObjects();

            return false;
        }

        /// <summary>
        /// Suspends auto saving for a while.
        /// </summary>
        public void SuspendAutoSave()
        {
            // TODO: finish auto save from old c++ editor code
        }

        /// <summary>
        /// Clears references to the scene objects by the editor. Deselects objects. Clear undo history.
        /// </summary>
        public void ClearRefsToSceneObjects()
        {
            // Clear Editor's data
            Editor.SceneEditing.Deselect();
			// TODO: test using editor (scripts reload, play enter/leave) without clearing the undo - current design is good and should handle this
            Undo.Clear(); // note: undo actions serialize ids to the objects (not direct refs) but cache reflection meta so we need to clean it
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
            var sceneNode = SceneGraphFactory.BuildSceneTree(scene);
            var treeNode = sceneNode.TreeNode;
            treeNode.IsLayoutLocked = true;
            treeNode.Expand();
            treeNode.EndAnimation();

            // TODO: cache expanded/colapsed nodes per scene tree

            // Add to the tree
            var rootNode = Root.TreeNode;
            bool wasLayoutLocked = rootNode.IsLayoutLocked;
            rootNode.IsLayoutLocked = true;
            //
            sceneNode.ParentNode = Root;
            rootNode.SortChildren();
            //
            treeNode.UnlockChildrenRecursive();
            rootNode.IsLayoutLocked = wasLayoutLocked;
            rootNode.Parent.PerformLayout();
            
            var endTime = DateTime.UtcNow;
            var milliseconds = (int)(endTime - startTime).TotalMilliseconds;
            Editor.Log($"Created graph for scene \'{scene.Name}\' in {milliseconds} ms");
        }

        private void OnSceneUnloading(Scene scene, Guid sceneId)
        {
            // Find scene tree node
            var node = Root.FindChildActor(scene);
            if (node != null)
            {
                Editor.Log($"Cleanup graph for scene \'{scene.Name}\'");

                // Cleanup
                node.Dispose();
            }
        }

        private void OnActorSpawned(Actor actor)
        {
            // Skip for not loaded scenes (spawning actors during scene loading in script Start function)
            var sceneNode = GetActorNode(actor.Scene);
            if(sceneNode == null)
                return;

            // Skip for missing parent
            var parent = actor.Parent;
            if (parent == null)
                return;

            var parentNode = GetActorNode(parent);
            if (parentNode == null)
            {
                // Missing parent node when adding child actor to not spawned or unlinked actor
                return;
            }
            
            var node = SceneGraphFactory.BuildActorNode(actor);
            if (node != null)
                node.ParentNode = parentNode;
        }

        private void OnActorDeleted(Actor actor)
        {
            var node = GetActorNode(actor);
            if (node != null)
            {
                ActorRemoved?.Invoke(node);

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
                node = prevParentNode.FindChildActor(actor);

                // Search whole tree if node was not found
                if (node == null)
                {
                    node = GetActorNode(actor);
                }
            }
            else
            {
                // Create new node for that actor (user may unlink it from the scene before and now link it)
                node = SceneGraphFactory.BuildActorNode(actor);
            }
            if (node == null)
                return;

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

        private void OnActorOrderInParentChanged(Actor actor)
        {
            ActorNode node = GetActorNode(actor);
            node?.TreeNode.OnOrderInParentChanged();
        }

        private void OnActorNameChanged(Actor actor)
        {
            ActorNode node = GetActorNode(actor);
            node?.TreeNode.OnNameChanged();
        }

        private void OnActorActiveChanged(Actor actor)
        {
            //ActorNode node = GetActorNode(actor);
            //node?.TreeNode.OnActiveChanged();
        }

        /// <summary>
        /// Gets the actor node.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>Found actor node or null if missing. Actor may not be linked to the scene tree so node won't be found in that case.</returns>
        public ActorNode GetActorNode(Actor actor)
        {
            if (actor == null)
                return null;

            // ActorNode has the same ID as actor does
            return SceneGraphFactory.FindNode(actor.ID) as ActorNode;
        }

        /// <summary>
        /// Gets the actor node.
        /// </summary>
        /// <param name="actorId">The actor id.</param>
        /// <returns>Found actor node or null if missing. Actor may not be linked to the scene tree so node won't be found in that case.</returns>
        public ActorNode GetActorNode(Guid actorId)
        {
            // ActorNode has the same ID as actor does
            return SceneGraphFactory.FindNode(actorId) as ActorNode;
        }

        /// <summary>
        /// Executes the custom action on the graph nodes.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void ExecuteOnGraph(SceneGraphTools.GraphExecuteCallbackDelegate callback)
        {
            Root.ExecuteOnGraph(callback);
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
            SceneManager.OnActorOrderInParentChanged += OnActorOrderInParentChanged;
            SceneManager.OnActorNameChanged += OnActorNameChanged;
            SceneManager.OnActorActiveChanged += OnActorActiveChanged;
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
            SceneManager.OnActorOrderInParentChanged -= OnActorOrderInParentChanged;
            SceneManager.OnActorNameChanged -= OnActorNameChanged;
            SceneManager.OnActorActiveChanged -= OnActorActiveChanged;
            
            // Cleanup graph
            Root.Dispose();

            if (SceneGraphFactory.Nodes.Count > 0)
            {
                Editor.LogWarning("Not all scene graph nodes has been disposed!");
            }
        }
    }
}
