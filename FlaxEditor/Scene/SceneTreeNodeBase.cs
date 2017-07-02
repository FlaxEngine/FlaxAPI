////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Base class for objects implementing <see cref="ISceneTreeNode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.ISceneTreeNode" />
    public abstract class SceneTreeNodeBase : ISceneTreeNode
    {
        protected ISceneTreeNode parentNode;

        /// <inheritdoc />
        public abstract Transform Transform { get; set; }

        /// <inheritdoc />
        public abstract Vector3 Position { get; set; }

        /// <inheritdoc />
        public abstract Quaternion Orientation { get; set; }

        /// <inheritdoc />
        public abstract Vector3 Scale { get; set; }

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract bool Active { get; }

        /// <inheritdoc />
        public virtual ISceneTreeNode ParentNode
        {
            get { return parentNode; }
            set
            {
                if (parentNode != value)
                {
                    if (parentNode != null)
                    {
                        parentNode.ChildNodes.Remove(this);
                    }

                    parentNode = value;

                    if (parentNode != null)
                    {
                        parentNode.ChildNodes.Add(this);
                    }

                    OnParentChanged();
                }
            }
        }

        /// <inheritdoc />
        public List<ISceneTreeNode> ChildNodes { get; } = new List<ISceneTreeNode>();

        /// <inheritdoc />
        public bool ContainsInHierarchy(ISceneTreeNode node)
        {
            if (ChildNodes.Contains(node))
                return true;

            return ChildNodes.Any(x => x.ContainsInHierarchy(node));
        }

        /// <inheritdoc />
        public bool ContainsChild(ISceneTreeNode node)
        {
            return ChildNodes.Contains(node);
        }

        /// <inheritdoc />
        public ISceneTreeNode RayCast(ref Ray ray, ref float minDistance)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnParentChanged()
        {
        }
    }
}
