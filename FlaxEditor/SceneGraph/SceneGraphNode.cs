////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Modules;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Base class for all leaf node objects which belong to scene graph used by the Editor.
    /// Scene Graph is directional graph without cyclic references. It's a tree.
    /// A <see cref="SceneModule"/> class is responsible for Scene Graph management.
    /// </summary>
    public abstract class SceneGraphNode : ITransformable
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        protected SceneGraphNode parentNode;

        /// <summary>
        /// Gets the children list.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public List<SceneGraphNode> ChildNodes { get; } = new List<SceneGraphNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneGraphNode"/> class.
        /// </summary>
        /// <param name="id">The unique node identifier. Cannot be changed at runtime.</param>
        protected SceneGraphNode(Guid id)
        {
            ID = id;
            SceneGraphFactory.Nodes.Add(id, this);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the identifier. Must be unique and immutable.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid ID { get; }

        /// <summary>
        /// Gets the parent scene.
        /// </summary>
        /// <value>
        /// The scene.
        /// </value>
        public abstract SceneNode ParentScene { get; }

        /// <inheritdoc />
        public abstract Transform Transform { get; set; }
        
        /// <summary>
        /// Gets a value indicating whether this instance can be copied or/and pasted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can be used for copy and paste; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanCopyPaste => true;
        
        /// <summary>
        /// Gets a value indicating whether this node can be deleted by the user.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can be deleted; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanDelete => true;

        /// <summary>
        /// Gets a value indicating whether this node can be dragged by the user.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can be dragged; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanDrag => true;

        /// <summary>
        /// Gets a value indicating whether this <see cref="SceneGraphNode"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsActive { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SceneGraphNode"/> is active and all parent nodes are also active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active in hierarchy; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsActiveInHierarchy { get; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public virtual SceneGraphNode ParentNode
        {
            get => parentNode;
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

        /// <summary>
        /// Gets the object to edit via properties editor when this node is being selected.
        /// </summary>
        /// <value>
        /// The editable object.
        /// </value>
        public virtual object EditableObject => this;

        /// <summary>
        /// Determines whether the specified object is in a hierarchy (one of the children or lower).
        /// </summary>
        /// <param name="node">The node to check,</param>
        /// <returns>True if given actor is part of the hierarchy, otherwise false.</returns>
        public virtual bool ContainsInHierarchy(SceneGraphNode node)
        {
            if (ChildNodes.Contains(node))
                return true;

            return ChildNodes.Any(x => x.ContainsInHierarchy(node));
        }

        /// <summary>
        /// Determines whether the specified object is one of the children.
        /// </summary>
        /// <param name="node">The node to check,</param>
        /// <returns>True if given object is a child, otherwise false.</returns>
        public virtual bool ContainsChild(SceneGraphNode node)
        {
            return ChildNodes.Contains(node);
        }

        /// <summary>
        /// Performs raycasting over nodes hierarchy trying to get the closest object hited by the given ray.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="distance">The result distance.</param>
        /// <returns>Hitted object or null if there is no interseciotn at all.</returns>
        public SceneGraphNode RayCast(ref Ray ray, ref float distance)
        {
            if (!IsActive)
                return null;

            // Check itself
            SceneGraphNode minTarget = null;
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

        /// <summary>
        /// Releases the node and the child tree. Disposed all GUI parts and used resources.
        /// </summary>
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

        /// <summary>
        /// Called when node or parent node is disposing.
        /// </summary>
        public virtual void OnDispose()
        {
            // Call deeper
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                ChildNodes[i].OnDispose();
            }

            SceneGraphFactory.Nodes.Remove(ID);
        }

        /// <summary>
        /// Called when parent node gets changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
        }
    }
}
