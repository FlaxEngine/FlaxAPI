////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.GUI;
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

        /// <summary>
        /// The actor node that owns this node.
        /// </summary>
        protected readonly ActorNode actorNode;

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
        /// <param name="node">The parent node.</param>
        public ActorTreeNode(ActorNode node)
            : base(true)
        {
            actorNode = node;
            Text = actorNode.Name;
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
            var dialog = RenamePopup.Show(this, _headerRect, Text, false);
            dialog.Renamed += OnRenamed;
        }

        private void OnRenamed(RenamePopup renamePopup)
        {
            using (new UndoBlock(Editor.Instance.Undo, Actor, "Rename"))
                Actor.Name = renamePopup.Text;
        }
    }
}
