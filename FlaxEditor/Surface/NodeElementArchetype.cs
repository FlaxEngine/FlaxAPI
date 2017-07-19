////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Surface node element archetype description.
    /// </summary>
    public class NodeElementArchetype
    {
        /// <summary>
        /// The element type.
        /// </summary>
        public NodeElementType Type;

        /// <summary>
        /// Default element position in node that has default size.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Custom text value.
        /// </summary>
        public string Text;

        /// <summary>
        /// True if use single connections (for Input element).
        /// </summary>
        public bool Single;

        /// <summary>
        /// Index of the node value that is conencted with that element.
        /// </summary>
        public int ValueIndex;

        /// <summary>
        /// Unique ID of the box in the graph data to link it to this element (Output/Input elements).
        /// </summary>
        public int BoxID;

        /// <summary>
        /// Default connections type for that element (can be set of types).
        /// </summary>
        public ConnectionType ConnectionsType;
    }
}
