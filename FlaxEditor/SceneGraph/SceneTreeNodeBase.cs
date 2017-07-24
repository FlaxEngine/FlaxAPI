////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Base class for objects implementing <see cref="ISceneTreeNode"/>.
    /// </summary>
    /// <seealso cref="ISceneTreeNode" />
    public abstract class SceneTreeNodeBase : ISceneTreeNode
    {
        /// <summary>
        /// The parent node.
        /// </summary>
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
        public abstract bool IsActive { get; }

        /// <inheritdoc />
        public abstract bool IsActiveInHierarchy { get; }

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
        public ISceneTreeNode RayCast(ref Ray ray, ref float distance)
        {
            if (!IsActive)
                return null;

            // Check itself
            ISceneTreeNode minTarget = null;
            float minDistance = float.MaxValue;
            if (RayCastSelf(ref ray, ref distance))
            {
                minTarget = this;
                minDistance = distance;
            }

            // Check all children
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                var hit = ChildNodes[i].RayCast(ref ray, ref distance);
                if (hit != null)
                {
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        minTarget = hit;
                    }
                }
            }

            // Return result
            distance = minDistance;
            return minTarget;
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            OnDispose();

            // Unlink from the parent
            if (parentNode != null)
            {
                parentNode.ChildNodes.Remove(this);
                parentNode = null;
            }
        }

        /// <inheritdoc />
        public virtual void OnDispose()
        {
            // Call deeper
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                ChildNodes[i].OnDispose();
            }
        }

        /// <summary>
        /// Checks if given ray intersects with the node.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>True ray hits this node, otherwise false.</returns>
        public virtual bool RayCastSelf(ref Ray ray, ref float distance)
        {
            return false;
        }

        /// <summary>
        /// Called when parent node gets changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
        }
    }
}
