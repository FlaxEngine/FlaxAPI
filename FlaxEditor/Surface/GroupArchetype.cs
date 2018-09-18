// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Surface nodes group archetype description.
    /// </summary>
    public sealed class GroupArchetype
    {
        /// <summary>
        /// Unique group ID.
        /// </summary>
        public ushort GroupID;

        /// <summary>
        /// The group name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Primary color for the group nodes.
        /// </summary>
        public Color Color;

        /// <summary>
        /// The custom tag.
        /// </summary>
        public object Tag;

        /// <summary>
        /// All nodes descriptions.
        /// </summary>
        public NodeArchetype[] Archetypes;
    }
}
