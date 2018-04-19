// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Base interface for elements that can be added to the <see cref="SurfaceNode"/>.
    /// </summary>
    public interface ISurfaceNodeElement
    {
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        SurfaceNode ParentNode { get; }

        /// <summary>
        /// Gets the element archetype.
        /// </summary>
        /// <value>
        /// The archetype.
        /// </value>
        NodeElementArchetype Archetype { get; }
    }
}
