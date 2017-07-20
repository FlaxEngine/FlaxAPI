////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Constants group.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Title = "Bool",
                Description = "Constant boolean value",
                Flags = NodeFlags.CloseButton,
                Size = new Vector2(110, 20),
                DefaultValues = new object[]
                {
                    false
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Bool, 0),
                    NodeElementArchetype.Factory.Bool(0, 0, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Title = "Inteager",
                Description = "Constant inteager value",
                Flags = NodeFlags.CloseButton,
                Size = new Vector2(110, 20),
                DefaultValues = new object[]
                {
                    0
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Integer, 0),
                    NodeElementArchetype.Factory.Inteager(0, 0, 0)
                }
            }
        };
    }
}
