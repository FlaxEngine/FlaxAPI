////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.Modules;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Base interface for all node objects which belong to scene graph used by the Editor.
    /// Scene Graph is directional graph without cyclic references. It's a tree.
    /// A <see cref="SceneModule"/> is responsible for Scene Graph management.
    /// </summary>
    public interface ISceneTreeNode : ITransformable
    {
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        ISceneTreeNode ParentNode { get; }

        /// <summary>
        /// Gets the children list.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        List<ISceneTreeNode> ChildNodes { get; }
    }
}
