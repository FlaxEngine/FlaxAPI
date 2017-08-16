////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        private bool _isActive;
        private int _orderInParent;
        private DragActors _dragActors;

        /// <summary>
        /// The actor node that owns this node.
        /// </summary>
        protected ActorNode actorNode;

        /// <summary>
        /// Gets the actor.
        /// </summary>
        /// <value>
        /// The actor.
        /// </value>
        public Actor Actor => actorNode.Actor;

        /// <summary>
        /// Gets the actor node.
        /// </summary>
        /// <value>
        /// The actor node.
        /// </value>
        public ActorNode ActorNode => actorNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTreeNode"/> class.
        /// </summary>
        public ActorTreeNode()
            : base(true)
        {
        }

        internal virtual void LinkNode(ActorNode node)
        {
            actorNode = node;
            if (node.Actor != null)
            {
                _isActive = node.Actor.IsActive;
                _orderInParent = node.Actor.OrderInParent;
            }
            else
            {
                _isActive = true;
                _orderInParent = 0;
            }
            UpdateText();
        }

        internal void OnActiveChanged()
        {
            _isActive = actorNode.Actor.IsActive;
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
            Text = actorNode.Name;
        }

        /// <inheritdoc />
        protected override Color CacheTextColor()
        {
            // Update node text color (based on ActorNode.IsActiveInHierarchy but with optimized logic a little)
            if (Parent is ActorTreeNode parent)
            {
                var style = Style.Current;
                if (parent._isActive)
                {
                    if (_isActive)
                        return style.Foreground;
                }

                return style.ForegroundDisabled;
            }

            return base.CacheTextColor();
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButtons buttons)
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
            var dialog = RenamePopup.Show(this, _headerRect, actorNode.Name, false);
            dialog.Renamed += OnRenamed;
        }

        private void OnRenamed(RenamePopup renamePopup)
        {
            using (new UndoBlock(Editor.Instance.Undo, Actor, "Rename"))
                Actor.Name = renamePopup.Text;
        }
        /*
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

            return DragDropEffect.None;
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragMoveHeader(DragData data)
        {
            if (_dragActors != null && _dragActors.HasValidDrag)
                return _dragActors.Effect;

            return DragDropEffect.None;
        }

        /// <inheritdoc />
        protected override void OnDragLeaveHeader()
        {
            _dragActors?.OnDragLeave();
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragDropHeader(DragData data)
        {
            var result = DragDropEffect.None;
            
            Actor myActor = Actor;
            Actor newParent = myActor;
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
                // Use drag positioning to change target parent and index
                if (_dragOverMode == DragItemPositioning.Above)
                {
                    if (newParent.HasParent)
                    {
                        newParent = newParent.Parent;
                        newOrder = newParent->GetChildren()->IndexOf(myActor);
                    }
                }
                else if (_dragOverMode == DragItemPositioning.Below)
                {
                    if (newParent.HasParent)
                    {
                        newParent = newParent.Parent;
                        newOrder = newParent->GetChildren()->IndexOf(myActor) + 1;
                    }
                }
            }
            if(newParent == null)
                throw new InvalidOperationException("Missing parent actor.");

            // Drag actors
            if (_dragActors.HasValidDrag)
            {
                using (new UndoBlock(Editor.Instance.Undo, , "Change actor(s) parent"))
                {
                    for (int i = 0; i < _dragActors.Objects.Count; i++)
                    {
                        var actor = _dragActors.Objects[i].Actor;
                        actor.Parent = newParent;
                        actor.OrderInParent = newOrder;
                    }
                }

                result = DragDropEffect.Move;
            }

            // Clear cache
            _dragActors.OnDragDrop();

            // Check if scene has been modified
            if (result != DragDropEffect.None)
            {
                // Expand if drag was over this node
                if (_dragOverMode == DragItemPositioning.Above)
                    Expand();

                // Editor.Instance.Scene TODO: mark as edited
            }

            return result;
        }

        private bool ValidateDragActor(ActorNode actorNode)
        {
            // Reject dragging parents and itself
            return actorNode.Actor != null && actorNode != ActorNode && actorNode.Find(Actor) == null;
        }*/
    }
}
