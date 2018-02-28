////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.SceneGraph.GUI
{
    /// <summary>
    /// Tree node GUI control used as a proxy object for actors hierarchy.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.TreeNode" />
    public class ActorTreeNode : TreeNode
    {
        private int _orderInParent;
        private DragActors _dragActors;
        private DragAssets _dragAssets;
        private DragActorType _dragActorType;

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
            if (Parent is ActorTreeNode parent)
            {
                var style = Style.Current;
                bool isActive = Actor?.IsActiveInHierarchy ?? true;
                if (!isActive)
                {
                    // Inactive
                    return style.ForegroundDisabled;
                }
                if (Editor.Instance.StateMachine.IsPlayMode && Actor.IsStatic)
                {
                    // Static
                    return style.Foreground * 0.85f;
                }

                // Default
                return style.Foreground;
            }

            return base.CacheTextColor();
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            var actor = Actor;
            if (actor && testHeaderHit(ref location))
            {
                Select();
                Editor.Instance.Windows.EditWin.ShowActor(actor);
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

        /// <inheritdoc />
        protected override void OnLongPress()
        {
            Select();

            // Start renaming the actor
            var dialog = RenamePopup.Show(this, _headerRect, _actorNode.Name, false);
            dialog.Renamed += OnRenamed;
        }

        private void OnRenamed(RenamePopup renamePopup)
        {
            using (new UndoBlock(Editor.Instance.Undo, Actor, "Rename"))
                Actor.Name = renamePopup.Text;
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragEnterHeader(DragData data)
        {
            // Check if cannot edit scene or there is no scene loaded
            if (!Editor.Instance.StateMachine.CurrentState.CanEditScene || !SceneManager.IsAnySceneLoaded)
                return DragDropEffect.None;

            // Check if drop actors
            if (_dragActors == null)
                _dragActors = new DragActors();
            if (_dragActors.OnDragEnter(data, ValidateDragActor))
                return _dragActors.Effect;

            // Check if drag assets
            if(_dragAssets == null)
                _dragAssets = new DragAssets();
            if (_dragAssets.OnDragEnter(data, ValidateDragAsset))
                return _dragAssets.Effect;
            
            // Check if drag actor type
            if(_dragActorType == null)
                _dragActorType = new DragActorType();
            if (_dragActorType.OnDragEnter(data, ValidateDragActorType))
                return _dragActorType.Effect;

            return DragDropEffect.None;
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragMoveHeader(DragData data)
        {
            if (_dragActors != null && _dragActors.HasValidDrag)
                return _dragActors.Effect;
            if (_dragAssets != null && _dragAssets.HasValidDrag)
                return _dragAssets.Effect;
            if (_dragActorType != null && _dragActorType.HasValidDrag)
                return _dragActorType.Effect;

            return DragDropEffect.None;
        }

        /// <inheritdoc />
        protected override void OnDragLeaveHeader()
        {
            _dragActors?.OnDragLeave();
            _dragAssets?.OnDragLeave();
            _dragActorType?.OnDragLeave();
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
                if (_dragOverMode == DragItemPositioning.Above)
                {
                    if (myActor.HasParent)
                    {
                        newParent = myActor.Parent;
                        newOrder = myActor.OrderInParent;
                    }
                }
                else if (_dragOverMode == DragItemPositioning.Below)
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
                bool worldPositionLock = ParentWindow.GetKey(Keys.Control) == false;
                var singleObject = _dragActors.Objects.Count == 1;
                if (singleObject)
                {
                    var targetActor = _dragActors.Objects[0].Actor;
                    using (new UndoBlock(Editor.Instance.Undo, targetActor, "Change actor parent"))
                    {
                        targetActor.SetParent(newParent, worldPositionLock);
                        targetActor.OrderInParent = newOrder;
                    }
                }
                else
                {
                    var targetActors = _dragActors.Objects.ConvertAll(x => x.Actor);
                    using (new UndoMultiBlock(Editor.Instance.Undo, targetActors, "Change actors parent"))
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
                            // Create actor
                            var model = FlaxEngine.Content.LoadAsync<Model>(item.ID);
                            var actor = ModelActor.New();
                            actor.StaticFlags = Actor.StaticFlags;
                            actor.Name = item.ShortName;
                            actor.Model = model;
                            actor.Transform = Actor.Transform;

                            // Spawn
                            Editor.Instance.SceneEditing.Spawn(actor, Actor);
                            
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
                                Editor.Instance.SceneEditing.Spawn(actor, Actor);
                            }

                            break;
                        }
	                    case ContentDomain.Audio:
	                    {
			                var actor = AudioSource.New();
			                actor.StaticFlags = Actor.StaticFlags;
			                actor.Name = item.ShortName;
			                actor.Clip = FlaxEngine.Content.LoadAsync<AudioClip>(item.ID);
			                actor.Transform = Actor.Transform;
			                Editor.Instance.SceneEditing.Spawn(actor, Actor);
							
		                    break;
	                    }
						case ContentDomain.Prefab:
                        {
                            throw new NotImplementedException("Spawning prefabs");
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
                    Editor.Instance.SceneEditing.Spawn(actor, Actor);
                }

                result = DragDropEffect.Move;
            }

            // Clear cache
            _dragActors?.OnDragDrop();
            _dragAssets?.OnDragDrop();
            _dragActorType?.OnDragDrop();

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
