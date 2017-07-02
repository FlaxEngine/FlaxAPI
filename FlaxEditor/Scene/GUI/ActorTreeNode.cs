////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor
{
    /// <summary>
    /// Tree node GUI control used as a proxy object for actors hierarchy.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.TreeNode" />
    public class ActorTreeNode : TreeNode
    {
        private bool _isActive;

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
            _isActive = true;
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
                    _isActive = actorNode.Actor.IsActive;

                    if (_isActive)
                        return style.Foreground;
                }

                _isActive = false;
                return style.ForegroundDisabled;
            }

            return base.CacheTextColor();
        }

        /// <inheritdoc />
        public override int Compare(Control other)
        {
            if (other is ActorTreeNode node)
            {
                var a1 = Actor;
                var a2 = node.Actor;
                if (a1 != null && a2 != null)
                {
                    return a1.OrderInParent - a2.OrderInParent;
                }
            }
            return base.Compare(other);
        }
    }
}
