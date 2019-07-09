// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

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
        SurfaceNode ParentNode { get; }

        /// <summary>
        /// Gets the element archetype.
        /// </summary>
        NodeElementArchetype Archetype { get; }
    }
}
