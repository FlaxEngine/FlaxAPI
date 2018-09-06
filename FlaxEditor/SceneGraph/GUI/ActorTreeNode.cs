// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Content;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.SceneGraph.GUI
{
    /// <summary>
    /// Tree node GUI control used as a proxy object for actors hierarchy.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.TreeNode" />
    public class ActorTreeNode : TreeNode
    {
        private int _orderInParent;
        private DragActors _dragActors;
        private DragAssets _dragAssets;
        private DragActorType _dragActorType;
        private DragHandlers _dragHandlers;

        /// <summary>
        /// The actor node that owns this node.
        /// </summary>
        protected ActorNode _actorNode;

        /// <summary>
        /// Gets the actor.
        /// </summary>
        public Actor Actor => _actorNode.Actor;

        /// <summary>
        /// Gets the actor node.
        /// </summary>
        public ActorNode ActorNode => _actorNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTreeNode"/> class.
        /// </summary>
        public ActorTreeNode()
        : base(true)
        {
        }

        internal virtual void LinkNode(ActorNode node)
        {
            _actorNode = node;
            if (node.Actor != null)
            {
                _orderInParent = node.Actor.OrderInParent;

                var id = node.Actor.ID;
                if (Editor.Instance.ProjectCache.IsExpandedActor(ref id))
                {
                    Expand(true);
                }
            }
            else
            {
                _orderInParent = 0;
            }

            UpdateText();
        }

        internal void OnOrderInParentChanged()
        {
            if (Parent is ActorTreeNode parent)
            {
                for (int i = 0; i < parent.ChildrenCount; i++)
                {
                    if (parent.Children[i] is ActorTreeNode child)
                        child._orderInParent = child.Actor.OrderInParent;
                }
                parent.SortChildren();
            }
        }

        internal void OnNameChanged()
        {
            UpdateText();
        }

        /// <summary>
        /// Updates the tree node text.
        /// </summary>
        public virtual void UpdateText()
        {
            Text = _actorNode.Name;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Update hidden state
            var actor = Actor;
            if (actor != null)
            {
                Visible = (actor.HideFlags & HideFlags.HideInHierarchy) == 0;
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        protected override Color CacheTextColor()
        {
            // Update node text color (based on ActorNode.IsActiveInHierarchy but with optimized logic a little)
            if (Parent is ActorTreeNode)
            {
                Color color = Color.White;
                var actor = Actor;
                if (actor != null && actor.HasPrefabLink)
                {
                    // Prefab
                    color = Style.Current.ProgressNormal;
                }

                if (actor != null && !actor.IsActiveInHierarchy)
                {
                    // Inactive
                    return color * 0.6f;
                }

                if (actor?.Scene != null && Editor.Instance.StateMachine.IsPlayMode && actor.IsStatic)
                {
                    // Static
                    return color * 0.85f;
                }

                // Default
                return color;
            }

            return base.CacheTextColor();
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            var actor = Actor;
            if (actor && TestHeaderHit(ref location))
            {
                StartRenaming();
                return true;
            }

            return base.OnMouseDoubleClick(location, buttons);
        }

        /// <inheritdoc />
        public override int Compare(Control other)
        {
            if (other is ActorTreeNode node)
            {
                return _orderInParent - node._orderInParent;
            }
            return base.Compare(other);
        }

        /// <summary>
        /// Starts the actor renaming action.
        /// </summary>
        public void StartRenaming()
        {
            // Block renaming during scripts reload
            if (Editor.Instance.ProgressReporting.CompileScripts.IsActive)
                return;

            Select();

            // Start renaming the actor
            var dialog = RenamePopup.Show(this, HeaderRect, _actorNode.Name, false);
            dialog.Renamed += OnRenamed;
        }

        private void OnRenamed(RenamePopup renamePopup)
        {
            using (new UndoBlock(ActorNode.Root.Undo, Actor, "Rename"))
                Actor.Name = renamePopup.Text;
        }

        /// <inheritdoc />
        protected override void OnExpandedChanged()
        {
            base.OnExpandedChanged();

            if (!IsLayoutLocked && Actor)
            {
                var id = Actor.ID;
                Editor.Instance.ProjectCache.SetExpandedActor(ref id, IsExpanded);
            }
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (IsFocused)
            {
                if (key == Keys.F2)
                {
                    StartRenaming();
                    return true;
                }
            }

            return base.OnKeyDown(key);
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragEnterHeader(DragData data)
        {
            // Check if cannot edit scene or there is no scene loaded (handle case for actors in prefab editor)
            if (_actorNode?.ParentScene != null)
            {
                if (!Editor.Instance.StateMachine.CurrentState.CanEditScene || !SceneManager.IsAnySceneLoaded)
                    return DragDropEffect.None;
            }
            else
            {
                if (!Editor.Instance.StateMachine.CurrentState.CanEditContent)
                    return DragDropEffect.None;
            }

            if (_dragHandlers == null)
                _dragHandlers = new DragHandlers();

            // Check if drop actors
            if (_dragActors == null)
            {
                _dragActors = new DragActors(ValidateDragActor);
                _dragHandlers.DragHelpers.Add(_dragActors);
            }
            if (_dragActors.OnDragEnter(data))
                return _dragActors.Effect;

            // Check if drag assets
            if (_dragAssets == null)
            {
                _dragAssets = new DragAssets(ValidateDragAsset);
                _dragHandlers.DragHelpers.Add(_dragAssets);
            }
            if (_dragAssets.OnDragEnter(data))
                return _dragAssets.Effect;

            // Check if drag actor type
            if (_dragActorType == null)
            {
                _dragActorType = new DragActorType(ValidateDragActorType);
                _dragHandlers.DragHelpers.Add(_dragActorType);
            }
            if (_dragActorType.OnDragEnter(data))
                return _dragActorType.Effect;

            return DragDropEffect.None;
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragMoveHeader(DragData data)
        {
            return _dragHandlers.Effect() ?? DragDropEffect.None;
        }

        /// <inheritdoc />
        protected override void OnDragLeaveHeader()
        {
            _dragHandlers.OnDragLeave();
        }

        private class ReparentAction : IUndoAction
        {
            private Guid[] _ids;
            private int _actorsCount;
            private Guid[] _prefabIds;
            private Guid[] _prefabObjectIds;

            public ReparentAction(Actor actor)
            : this(new List<Actor> { actor })
            {
            }

            public ReparentAction(List<Actor> actors)
            {
                var allActors = new List<Actor>();
                allActors.Capacity = Mathf.NextPowerOfTwo(actors.Count);
                for (int i = 0; i < actors.Count; i++)
                {
                    GetAllActors(allActors, actors[i]);
                }

                var allScripts = new List<Script>();
                allScripts.Capacity = allActors.Capacity;
                GetAllScripts(allActors, allScripts);

                int allCount = allActors.Count + allScripts.Count;
                _actorsCount = allActors.Count;
                _ids = new Guid[allCount];
                _prefabIds = new Guid[allCount];
                _prefabObjectIds = new Guid[allCount];

                for (int i = 0; i < allActors.Count; i++)
                {
                    _ids[i] = allActors[i].ID;
                    _prefabIds[i] = allActors[i].PrefabID;
                    _prefabObjectIds[i] = allActors[i].PrefabObjectID;
                }
                for (int i = 0; i < allScripts.Count; i++)
                {
                    int j = _actorsCount + i;
                    _ids[j] = allScripts[i].ID;
                    _prefabIds[j] = allScripts[i].PrefabID;
                    _prefabObjectIds[j] = allScripts[i].PrefabObjectID;
                }
            }

            private void GetAllActors(List<Actor> allActors, Actor actor)
            {
                allActors.Add(actor);

                for (int i = 0; i < actor.ChildrenCount; i++)
                {
                    var child = actor.GetChild(i);
                    if (!allActors.Contains(child))
                    {
                        GetAllActors(allActors, child);
                    }
                }
            }

            private void GetAllScripts(List<Actor> allActors, List<Script> allScripts)
            {
                for (int i = 0; i < allActors.Count; i++)
                {
                    var actor = allActors[i];
                    for (int j = 0; j < actor.ScriptsCount; j++)
                    {
                        allScripts.Add(actor.GetScript(j));
                    }
                }
            }

            /// <inheritdoc />
            public string ActionString => string.Empty;

            /// <inheritdoc />
            public void Do()
            {
                // Note: prefab links are broken by the C++ backend on actor reparenting
            }

            /// <inheritdoc />
            public void Undo()
            {
                // Restore links
                for (int i = 0; i < _actorsCount; i++)
                {
                    var actor = Object.Find<Actor>(ref _ids[i]);
                    if (actor != null && _prefabIds[i] != Guid.Empty)
                    {
                        Actor.Internal_LinkPrefab(actor.unmanagedPtr, ref _prefabIds[i], ref _prefabObjectIds[i]);
                    }
                }
                for (int i = _actorsCount; i < _ids.Length; i++)
                {
                    var script = Object.Find<Script>(ref _ids[i]);
                    if (script != null && _prefabIds[i] != Guid.Empty)
                    {
                        Script.Internal_LinkPrefab(script.unmanagedPtr, ref _prefabIds[i], ref _prefabObjectIds[i]);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragDropHeader(DragData data)
        {
            var result = DragDropEffect.None;

            Actor myActor = Actor;
            Actor newParent;
            int newOrder = -1;

            // Check if has no actor (only for Root Actor)
            if (myActor == null)
            {
                // Append to the last scene
                var scenes = SceneManager.Scenes;
                if (scenes == null || scenes.Length == 0)
                    throw new InvalidOperationException("No scene loaded.");
                newParent = scenes[scenes.Length - 1];
            }
            else
            {
                newParent = myActor;

                // Use drag positioning to change target parent and index
                if (DragOverMode == DragItemPositioning.Above)
                {
                    if (myActor.HasParent)
                    {
                        newParent = myActor.Parent;
                        newOrder = myActor.OrderInParent;
                    }
                }
                else if (DragOverMode == DragItemPositioning.Below)
                {
                    if (myActor.HasParent)
                    {
                        newParent = myActor.Parent;
                        newOrder = myActor.OrderInParent + 1;
                    }
                }
            }
            if (newParent == null)
                throw new InvalidOperationException("Missing parent actor.");

            // Drag actors
            if (_dragActors != null && _dragActors.HasValidDrag)
            {
                bool worldPositionLock = Root.GetKey(Keys.Control) == false;
                var singleObject = _dragActors.Objects.Count == 1;
                if (singleObject)
                {
                    var targetActor = _dragActors.Objects[0].Actor;
                    var customAction = targetActor.HasPrefabLink ? new ReparentAction(targetActor) : null;
                    using (new UndoBlock(ActorNode.Root.Undo, targetActor, "Change actor parent", customAction))
                    {
                        targetActor.SetParent(newParent, worldPositionLock);
                        targetActor.OrderInParent = newOrder;
                    }
                }
                else
                {
                    var targetActors = _dragActors.Objects.ConvertAll(x => x.Actor);
                    var customAction = targetActors.Any(x => x.HasPrefabLink) ? new ReparentAction(targetActors) : null;
                    using (new UndoMultiBlock(ActorNode.Root.Undo, targetActors, "Change actors parent", customAction))
                    {
                        for (int i = 0; i < targetActors.Count; i++)
                        {
                            var targetActor = targetActors[i];
                            targetActor.SetParent(newParent, worldPositionLock);
                            targetActor.OrderInParent = newOrder;
                        }
                    }
                }

                result = DragDropEffect.Move;
            }
            // Drag assets
            else if (_dragAssets != null && _dragAssets.HasValidDrag)
            {
                for (int i = 0; i < _dragAssets.Objects.Count; i++)
                {
                    var item = _dragAssets.Objects[i];

                    switch (item.ItemDomain)
                    {
                    case ContentDomain.Model:
                    {
                        if (item.TypeName == typeof(SkinnedModel).FullName)
                        {
                            // Create actor
                            var model = FlaxEngine.Content.LoadAsync<SkinnedModel>(item.ID);
                            var actor = AnimatedModel.New();
                            actor.StaticFlags = Actor.StaticFlags;
                            actor.Name = item.ShortName;
                            actor.SkinnedModel = model;
                            actor.Transform = Actor.Transform;

                            // Spawn
                            ActorNode.Root.Spawn(actor, Actor);
                        }
                        else
                        {
                            // Create actor
                            var model = FlaxEngine.Content.LoadAsync<Model>(item.ID);
                            var actor = ModelActor.New();
                            actor.StaticFlags = Actor.StaticFlags;
                            actor.Name = item.ShortName;
                            actor.Model = model;
                            actor.Transform = Actor.Transform;

                            // Spawn
                            ActorNode.Root.Spawn(actor, Actor);
                        }

                        break;
                    }
                    case ContentDomain.Other:
                    {
                        if (item.TypeName == typeof(CollisionData).FullName)
                        {
                            // Create actor
                            var actor = MeshCollider.New();
                            actor.StaticFlags = Actor.StaticFlags;
                            actor.Name = item.ShortName;
                            actor.CollisionData = FlaxEngine.Content.LoadAsync<CollisionData>(item.ID);
                            actor.Transform = Actor.Transform;

                            // Spawn
                            ActorNode.Root.Spawn(actor, Actor);
                        }

                        break;
                    }
                    case ContentDomain.Audio:
                    {
                        // Create actor
                        var actor = AudioSource.New();
                        actor.StaticFlags = Actor.StaticFlags;
                        actor.Name = item.ShortName;
                        actor.Clip = FlaxEngine.Content.LoadAsync<AudioClip>(item.ID);
                        actor.Transform = Actor.Transform;

                        // Spawn
                        ActorNode.Root.Spawn(actor, Actor);

                        break;
                    }
                    case ContentDomain.Prefab:
                    {
                        // Create prefab instance
                        var prefab = FlaxEngine.Content.LoadAsync<Prefab>(item.ID);
                        var actor = PrefabManager.SpawnPrefab(prefab, null);
                        actor.StaticFlags = Actor.StaticFlags;
                        actor.Name = item.ShortName;
                        actor.Transform = Actor.Transform;

                        // Spawn
                        ActorNode.Root.Spawn(actor, Actor);

                        break;
                    }
                    }
                }

                result = DragDropEffect.Move;
            }
            // Drag actor type
            else if (_dragActorType != null && _dragActorType.HasValidDrag)
            {
                for (int i = 0; i < _dragActorType.Objects.Count; i++)
                {
                    var item = _dragActorType.Objects[i];

                    // Create actor
                    var actor = FlaxEngine.Object.New(item) as Actor;
                    if (actor == null)
                    {
                        Editor.LogWarning("Failed to spawn actor of type " + item.FullName);
                        continue;
                    }
                    actor.StaticFlags = Actor.StaticFlags;
                    actor.Name = item.Name;
                    actor.Transform = Actor.Transform;

                    // Spawn
                    ActorNode.Root.Spawn(actor, Actor);
                }

                result = DragDropEffect.Move;
            }

            // Clear cache
            _dragHandlers.OnDragDrop();

            // Check if scene has been modified
            if (result != DragDropEffect.None)
            {
                var node = SceneGraphFactory.FindNode(newParent.ID) as ActorNode;
                node?.TreeNode.Expand();
            }

            return result;
        }

        private bool ValidateDragActor(ActorNode actorNode)
        {
            // Reject dragging actors not linked to scene (eg. from prefab) or in the opposite way
            var thisHasScene = ActorNode.ParentScene != null;
            var otherHasScene = actorNode.ParentScene != null;
            if (thisHasScene != otherHasScene)
                return false;

            // Reject dragging parents and itself
            return actorNode.Actor != null && actorNode != ActorNode && actorNode.Find(Actor) == null;
        }

        private bool ValidateDragAsset(AssetItem item)
        {
            switch (item.ItemDomain)
            {
            case ContentDomain.Model:
            case ContentDomain.Audio:
            case ContentDomain.Prefab: return true;
            case ContentDomain.Other:
            {
                if (item.TypeName == typeof(CollisionData).FullName)
                    return true;
                return false;
            }
            default: return false;
            }
        }

        private bool ValidateDragActorType(Type actorType)
        {
            return true;
        }

        /// <inheritdoc />
        protected override void DoDragDrop()
        {
            DragData data;
            var tree = ParentTree;

            // Check if this node is selected
            if (tree.Selection.Contains(this))
            {
                // Get selected actors
                var actors = new List<ActorNode>();
                for (var i = 0; i < tree.Selection.Count; i++)
                {
                    var e = tree.Selection[i];
                    if (e is ActorTreeNode node && node.ActorNode.CanDrag)
                        actors.Add(node.ActorNode);
                }
                data = DragActors.GetDragData(actors);
            }
            else
            {
                data = DragActors.GetDragData(ActorNode);
            }

            // Start drag operation
            DoDragDrop(data);
        }
    }
}
