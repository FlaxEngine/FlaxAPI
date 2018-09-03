// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Tools group.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                // [Deprecated]
                TypeID = 1,
                Title = "Fresnel",
                Description = "Calculates a falloff based on the dot product of the surface normal and the direction to the camera",
                Flags = NodeFlags.MaterialOnly | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(140, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Exponent", true, ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Input(1, "Base Reflect Fraction", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Input(2, "Normal", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Title = "Desaturation",
                Description = "Desaturates input, or converts the colors of its input into shades of gray, based a certain percentage",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(140, 130),
                DefaultValues = new object[]
                {
                    new Vector3(0.3f, 0.59f, 0.11f)
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Input", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Input(1, "Scale", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 2 + 5, "Luminance Factors"),
                    NodeElementArchetype.Factory.Vector_X(0, Surface.Constants.LayoutOffsetY * 3 + 5, 0),
                    NodeElementArchetype.Factory.Vector_Y(0, Surface.Constants.LayoutOffsetY * 3 + 5, 0),
                    NodeElementArchetype.Factory.Vector_Z(0, Surface.Constants.LayoutOffsetY * 3 + 5, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 3,
                Title = "Time",
                Description = "Game time constant",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(110, 20),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 4,
                Title = "Fresnel",
                Description = "Calculates a falloff based on the dot product of the surface normal and the direction to the camera",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(200, 60),
                DefaultValues = new object[]
                {
                    5.0f,
                    0.04f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Exponent", true, ConnectionType.Float, 0, 0),
                    NodeElementArchetype.Factory.Input(1, "Base Reflect Fraction", true, ConnectionType.Float, 1, 1),
                    NodeElementArchetype.Factory.Input(2, "Normal", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 5,
                Title = "Time",
                Description = "Game time constant",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(140, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Animation Time", ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Output(1, "Delta Seconds", ConnectionType.Float, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 6,
                Title = "Panner",
                Description = "Animates UVs over time",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(170, 80),
                DefaultValues = new object[]
                {
                    false
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UV", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Input(1, "Time", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Input(2, "Speed", true, ConnectionType.Vector2, 2),
                    NodeElementArchetype.Factory.Text(18, Surface.Constants.LayoutOffsetY * 3 + 5, "Fractional Part"),
                    NodeElementArchetype.Factory.Bool(0, Surface.Constants.LayoutOffsetY * 3 + 5, 0),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Vector2, 3)
                }
            },
        };
    }
}
