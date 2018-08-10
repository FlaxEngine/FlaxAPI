// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Surface node archetype description.
    /// </summary>
    public sealed class NodeArchetype
    {
        /// <summary>
        /// Create custom node callback.
        /// </summary>
        public delegate SurfaceNode CreateCustomNodeFunc(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch);

        /// <summary>
        /// Unique node type ID within a single group.
        /// </summary>
        public ushort TypeID;

        /// <summary>
        /// Custom create function (may be null).
        /// </summary>
        public CreateCustomNodeFunc Create;

        /// <summary>
        /// Default initial size of the node.
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// Custom set of flags.
        /// </summary>
        public NodeFlags Flags;

        /// <summary>
        /// Title text.
        /// </summary>
        public string Title;

        /// <summary>
        /// Short node description.
        /// </summary>
        public string Description;

        /// <summary>
        /// Alternative node titles.
        /// </summary>
        public string[] AlternativeTitles;

        /// <summary>
        /// Default node values.
        /// </summary>
        public object[] DefaultValues;

        /// <summary>
        /// Default connections type for dependant boxes.
        /// </summary>
        public ConnectionType DefaultType;

        /// <summary>
        /// Array with independent boxes IDs.
        /// </summary>
        public int[] IndependentBoxes;

        /// <summary>
        /// Array with dependent boxes IDs.
        /// </summary>
        public int[] DependentBoxes;

        /// <summary>
        /// Array with default elements descriptions.
        /// </summary>
        public NodeElementArchetype[] Elements;
    }
}
