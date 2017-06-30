////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor
{
    /// <summary>
    /// A tree node used to visalize scene actors structure in <see cref="SceneTreeWindow"/>. It's a ViewModel object for <see cref="Actor"/>.
    /// It's part of the Scene Graph.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.TreeNode" />
    /// <seealso cref="FlaxEditor.ISceneTreeNode" />
    public class ActorTreeNode : TreeNode, ISceneTreeNode
    {
        /// <summary>
        /// The linked actor object.
        /// </summary>
        protected Actor _actor;

        /// <summary>
        /// Gets the actor.
        /// </summary>
        /// <value>
        /// The actor.
        /// </value>
        public Actor Actor => _actor;

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public ActorTreeNode ParentNode => Parent as ActorTreeNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTreeNode"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public ActorTreeNode(Actor actor)
            : base(true)
        {
            _actor = actor;

            if (_actor != null)
            {
                Text = actor.Name;
            }
        }

        /// <summary>
        /// Tries to find the tree node for the specified actor.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>Tree node or null if cannot find it.</returns>
        public ActorTreeNode Find(Actor actor)
        {
            // Check itself
            if (_actor == actor)
                return this;

            // Check deeper
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ActorTreeNode node)
                {
                    var result = node.Find(actor);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Tries to find the tree node for the specified actor in child nodes collection.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>Tree node or null if cannot find it.</returns>
        public ActorTreeNode FindChild(Actor actor)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ActorTreeNode node && node.Actor == actor)
                {
                    return node;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public override int Compare(Control other)
        {
            if (other is ActorTreeNode node)
            {
                if (_actor != null && node._actor != null)
                {
                    return _actor.OrderInParent - node._actor.OrderInParent;
                }
            }
            return base.Compare(other);
        }

        #region [ISceneTreeNode] implementation

        /// <inheritdoc />
        public Transform Transform
        {
            get => _actor.Transform;
            set => _actor.Transform = value;
        }

        /// <inheritdoc />
        public Vector3 Position
        {
            get => _actor.Position;
            set => _actor.Position = value;
        }

        /// <inheritdoc />
        public Quaternion Orientation
        {
            get => _actor.Orientation;
            set => _actor.Orientation = value;
        }

        /// <inheritdoc />
        public Vector3 Scale
        {
            get => _actor.Scale;
            set => _actor.Scale = value;
        }

        /// <inheritdoc />
        ISceneTreeNode ISceneTreeNode.ParentNode
        {
            get { return Parent as ISceneTreeNode; }
        }

        /// <inheritdoc />
        public List<ISceneTreeNode> ChildNodes { get; } = new List<ISceneTreeNode>();

        #endregion
    }
}
