////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Modules;
using FlaxEngine;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Base class for all leaf node objects which belong to scene graph used by the Editor.
    /// Scene Graph is directional graph without cyclic references. It's a tree.
    /// A <see cref="SceneModule"/> class is responsible for Scene Graph management.
    /// </summary>
    public abstract class SceneTreeNode : ITransformable
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        protected SceneTreeBranchNode parentNode;

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
        public abstract Guid ID { get; }

        /// <inheritdoc />
        public abstract Transform Transform { get; set; }

        /// <inheritdoc />
        public abstract Vector3 Position { get; set; }

        /// <inheritdoc />
        public abstract Quaternion Orientation { get; set; }

        /// <inheritdoc />
        public abstract Vector3 Scale { get; set; }
        
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
        /// Gets a value indicating whether this <see cref="SceneTreeNode"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsActive { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SceneTreeNode"/> is active and all parent nodes are also active.
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
        public virtual SceneTreeBranchNode ParentNode
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
        public virtual bool ContainsInHierarchy(SceneTreeNode node)
        {
            return false;
        }

        /// <summary>
        /// Determines whether the specified object is one of the children.
        /// </summary>
        /// <param name="node">The node to check,</param>
        /// <returns>True if given object is a child, otherwise false.</returns>
        public virtual bool ContainsChild(SceneTreeNode node)
        {
            return false;
        }

        /// <summary>
        /// Performs raycasting over nodes hierarchy trying to get the closest object hited by the given ray.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="distance">The result distance.</param>
        /// <returns>Hitted object or null if there is no interseciotn at all.</returns>
        public abstract SceneTreeNode RayCast(ref Ray ray, ref float distance);

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
        }

        /// <summary>
        /// Called when parent node gets changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
        }
    }
}
