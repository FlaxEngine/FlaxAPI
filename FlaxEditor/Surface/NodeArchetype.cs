// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Delegate for node data parsing.
    /// </summary>
    /// <param name="filterText">The filter text.</param>
    /// <param name="data">The node data.</param>
    /// <returns>True if requests has been parsed, otherwise false.</returns>
    public delegate bool NodeArchetypeTryParseHandler(string filterText, out object[] data);

    /// <summary>
    /// Surface node archetype description.
    /// </summary>
    public sealed class NodeArchetype
    {
        /// <summary>
        /// Create custom node callback.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <param name="context">The context.</param>
        /// <param name="nodeArch">The node archetype.</param>
        /// <param name="groupArch">The node parent group archetype.</param>
        /// <returns>The created node object.</returns>
        public delegate SurfaceNode CreateCustomNodeFunc(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch);

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
        /// The custom tag.
        /// </summary>
        public object Tag;

        /// <summary>
        /// Default node values.
        /// </summary>
        /// <remarks>
        /// The limit for the node values array is 32 (must match GRAPH_NODE_MAX_VALUES in C++ engine core).
        /// </remarks>
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

        /// <summary>
        /// Tries to parse some text and extract the data from it.
        /// </summary>
        public NodeArchetypeTryParseHandler TryParseText;
    }
}
