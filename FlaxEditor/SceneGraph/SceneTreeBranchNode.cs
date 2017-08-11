////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Implementation of <see cref="SceneTreeNode"/> that can contain child nodes.
    /// </summary>
    /// <seealso cref="FlaxEditor.SceneGraph.SceneTreeNode" />
    public abstract class SceneTreeBranchNode : SceneTreeNode
    {
        /// <summary>
        /// Gets the children list.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public List<SceneTreeNode> ChildNodes { get; } = new List<SceneTreeNode>();

        /// <inheritdoc />
        public override bool ContainsInHierarchy(SceneTreeNode node)
        {
            if (ChildNodes.Contains(node))
                return true;

            return ChildNodes.Any(x => x.ContainsInHierarchy(node));
        }

        /// <inheritdoc />
        public override bool ContainsChild(SceneTreeNode node)
        {
            return ChildNodes.Contains(node);
        }

        /// <inheritdoc />
        public override SceneTreeNode RayCast(ref Ray ray, ref float distance)
        {
            if (!IsActive)
                return null;

            // Check itself
            SceneTreeNode minTarget = null;
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

        /// <inheritdoc />
        public override void OnDispose()
        {
            // Call deeper
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                ChildNodes[i].OnDispose();
            }

            base.OnDispose();
        }
    }
}
