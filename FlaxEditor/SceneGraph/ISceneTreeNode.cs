////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Base interface for all node objects which belong to scene graph used by the Editor.
    /// Scene Graph is directional graph without cyclic references. It's a tree.
    /// A <see cref="SceneTreeWindow"/> is responsible for Scene Graph management.
    /// </summary>
    public interface ISceneTreeNode : ITransformable
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ISceneTreeNode"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        bool IsActive { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ISceneTreeNode"/> is active and all parent nodes are also active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active in hierarchy; otherwise, <c>false</c>.
        /// </value>
        bool IsActiveInHierarchy { get; }

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        ISceneTreeNode ParentNode { get; set; }

        /// <summary>
        /// Gets the children list.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        List<ISceneTreeNode> ChildNodes { get; }
        
        /// <summary>
        /// Determines whether the specified object is in a hierarchy (one of the children or lower).
        /// </summary>
        /// <param name="node">The node to check,</param>
        /// <returns>True if given actor is part of the hierarchy, otherwise false.</returns>
        bool ContainsInHierarchy(ISceneTreeNode node);

        /// <summary>
        /// Determines whether the specified object is one of the children.
        /// </summary>
        /// <param name="node">The node to check,</param>
        /// <returns>True if given object is a child, otherwise false.</returns>
        bool ContainsChild(ISceneTreeNode node);

        /// <summary>
        /// Performs raycasting over nodes hierarchy trying to get the closest object hited by the given ray.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="distance">The result distance.</param>
        /// <returns>Hitted object or null if there is no interseciotn at all.</returns>
        ISceneTreeNode RayCast(ref Ray ray, ref float distance);
    }
}
