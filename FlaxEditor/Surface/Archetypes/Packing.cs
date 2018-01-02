////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Packing group.
    /// </summary>
    public static class Packing
    {
        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            // Packing
            new NodeArchetype
            {
                TypeID = 20,
                Title = "Pack Vector2",
                Description = "Pack components to Vector2",
                Size = new Vector2(150, 40),
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Input(0, "X", true, ConnectionType.Float, 1, 0),
                    NodeElementArchetype.Factory.Input(1, "Y", true, ConnectionType.Float, 2, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 21,
                Title = "Pack Vector3",
                Description = "Pack components to Vector3",
                Size = new Vector2(150, 60),
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f,
                    0.0f
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Input(0, "X", true, ConnectionType.Float, 1, 0),
                    NodeElementArchetype.Factory.Input(1, "Y", true, ConnectionType.Float, 2, 1),
                    NodeElementArchetype.Factory.Input(2, "Z", true, ConnectionType.Float, 3, 2),
                }
            },
            new NodeArchetype
            {
                TypeID = 22,
                Title = "Pack Vector4",
                Description = "Pack components to Vector4",
                Size = new Vector2(150, 80),
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f,
                    0.0f,
                    0.0f
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Vector4, 0),
                    NodeElementArchetype.Factory.Input(0, "X", true, ConnectionType.Float, 1, 0),
                    NodeElementArchetype.Factory.Input(1, "Y", true, ConnectionType.Float, 2, 1),
                    NodeElementArchetype.Factory.Input(2, "Z", true, ConnectionType.Float, 3, 2),
                    NodeElementArchetype.Factory.Input(3, "W", true, ConnectionType.Float, 4, 3),
                }
            },
            // TODO: 23 - Pack rotation
            // TODO: 24 - Pack transform
            // TODO: 25 - Pack box

            // Unpacking
            new NodeArchetype
            {
                TypeID = 30,
                Title = "Unpack Vector2",
                Description = "Unpack components from Vector2",
                Size = new Vector2(150, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, "X", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(1, "Y", ConnectionType.Float, 2)
                }
            },
            new NodeArchetype
            {
                TypeID = 31,
                Title = "Unpack Vector3",
                Description = "Unpack components from Vector3",
                Size = new Vector2(150, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(0, "X", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(1, "Y", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(2, "Z", ConnectionType.Float, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 32,
                Title = "Unpack Vector4",
                Description = "Unpack components from Vector4",
                Size = new Vector2(150, 80),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector4, 0),
                    NodeElementArchetype.Factory.Output(0, "X", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(1, "Y", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(2, "Z", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(3, "W", ConnectionType.Float, 4)
                }
            },
            // TODO: 33 - Unpack rotation
            // TODO: 34 - Unpack transform
            // TODO: 35 - Unpack box
        };
    }
}
