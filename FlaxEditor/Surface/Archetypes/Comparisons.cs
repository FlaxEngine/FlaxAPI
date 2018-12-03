// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Comparisons group.
    /// </summary>
    public static class Comparisons
    {
        private static NodeArchetype Op(ushort id, string title, string desc, ConnectionType inputTypes)
        {
            return new NodeArchetype
            {
                TypeID = id,
                Title = title,
                Description = desc,
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(100, 40),
                DefaultType = inputTypes,
                IndependentBoxes = new[]
                {
                    0,
                    1
                },
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, inputTypes, 0, 0),
                    NodeElementArchetype.Factory.Input(1, string.Empty, true, inputTypes, 1, 1),
                    NodeElementArchetype.Factory.Output(0, title, ConnectionType.Bool, 2)
                }
            };
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            Op(1, "==", "Determines whether two values are equal", ConnectionType.Variable),
            Op(2, "!=", "Determines whether two values are not equal", ConnectionType.Variable),
            Op(3, ">", "Determines whether the first value is greater than the other", ConnectionType.Variable),
            Op(4, "<", "Determines whether the first value is less than the other", ConnectionType.Variable),
            Op(5, "<=", "Determines whether the first value is less or equal to the other", ConnectionType.Variable),
            Op(6, ">=", "Determines whether the first value is greater or equal to the other", ConnectionType.Variable),
        };
    }
}
