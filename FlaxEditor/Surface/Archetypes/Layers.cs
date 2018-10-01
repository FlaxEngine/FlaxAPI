// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Layers group.
    /// </summary>
    public static class Layers
    {
        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Title = "Sample Layer",
                Description = "Sample material or material instance",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(160, 100),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Asset(0, Surface.Constants.LayoutOffsetY, 0, ContentDomain.Material)
                }
            },
            new NodeArchetype
            {
                // [Deprecated]
                TypeID = 2,
                Title = "Linear Layer Blend",
                Description = "Create blended layer using linear math",
                Flags = NodeFlags.MaterialOnly | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(170, 80),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Bottom", true, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(1, "Top", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(2, "Alpha", true, ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 3,
                Title = "Pack Material Layer",
                Description = "Pack material properties",
                Flags = NodeFlags.MaterialOnly | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(200, 240),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Color", true, ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Input(1, "Mask", true, ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Input(2, "Emissive", true, ConnectionType.Vector3, 3),
                    NodeElementArchetype.Factory.Input(3, "Metalness", true, ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Input(4, "Specular", true, ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Input(5, "Roughness", true, ConnectionType.Float, 6),
                    NodeElementArchetype.Factory.Input(6, "Ambient Occlusion", true, ConnectionType.Float, 7),
                    NodeElementArchetype.Factory.Input(7, "Normal", true, ConnectionType.Vector3, 8),
                    NodeElementArchetype.Factory.Input(8, "Opacity", true, ConnectionType.Float, 9),
                    NodeElementArchetype.Factory.Input(9, "Refraction", true, ConnectionType.Float, 10),
                    NodeElementArchetype.Factory.Input(10, "Position Offset", true, ConnectionType.Vector3, 11),
                }
            },
            new NodeArchetype
            {
                TypeID = 4,
                Title = "Unpack Material Layer",
                Description = "Unpack material properties",
                Flags = NodeFlags.MaterialOnly | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(210, 240),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "", true, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Output(0, "Color", ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Output(1, "Mask", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(2, "Emissive", ConnectionType.Vector3, 3),
                    NodeElementArchetype.Factory.Output(3, "Metalness", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Output(4, "Specular", ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Output(5, "Roughness", ConnectionType.Float, 6),
                    NodeElementArchetype.Factory.Output(6, "Ambient Occlusion", ConnectionType.Float, 7),
                    NodeElementArchetype.Factory.Output(7, "Normal", ConnectionType.Vector3, 8),
                    NodeElementArchetype.Factory.Output(8, "Opacity", ConnectionType.Float, 9),
                    NodeElementArchetype.Factory.Output(9, "Refraction", ConnectionType.Float, 10),
                    NodeElementArchetype.Factory.Output(10, "Position Offset", ConnectionType.Vector3, 11),
                }
            },
            new NodeArchetype
            {
                TypeID = 5,
                Title = "Linear Layer Blend",
                Description = "Create blended layer using linear math",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(170, 80),
                DefaultValues = new object[]
                {
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Bottom", true, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(1, "Top", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(2, "Alpha", true, ConnectionType.Float, 2, 0),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 6,
                Title = "Pack Material Layer",
                Description = "Pack material properties",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(200, 280),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Color", true, ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Input(1, "Mask", true, ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Input(2, "Emissive", true, ConnectionType.Vector3, 3),
                    NodeElementArchetype.Factory.Input(3, "Metalness", true, ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Input(4, "Specular", true, ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Input(5, "Roughness", true, ConnectionType.Float, 6),
                    NodeElementArchetype.Factory.Input(6, "Ambient Occlusion", true, ConnectionType.Float, 7),
                    NodeElementArchetype.Factory.Input(7, "Normal", true, ConnectionType.Vector3, 8),
                    NodeElementArchetype.Factory.Input(8, "Opacity", true, ConnectionType.Float, 9),
                    NodeElementArchetype.Factory.Input(9, "Refraction", true, ConnectionType.Float, 10),
                    NodeElementArchetype.Factory.Input(10, "Position Offset", true, ConnectionType.Vector3, 11),
                    NodeElementArchetype.Factory.Input(11, "Tessellation Multiplier", true, ConnectionType.Float, 12),
                    NodeElementArchetype.Factory.Input(12, "World Displacement", true, ConnectionType.Vector3, 13),
                    NodeElementArchetype.Factory.Input(13, "Subsurface Color", true, ConnectionType.Vector3, 14),
                }
            },
            new NodeArchetype
            {
                TypeID = 7,
                Title = "Unpack Material Layer",
                Description = "Unpack material properties",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(210, 280),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "", true, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Output(0, "Color", ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Output(1, "Mask", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(2, "Emissive", ConnectionType.Vector3, 3),
                    NodeElementArchetype.Factory.Output(3, "Metalness", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Output(4, "Specular", ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Output(5, "Roughness", ConnectionType.Float, 6),
                    NodeElementArchetype.Factory.Output(6, "Ambient Occlusion", ConnectionType.Float, 7),
                    NodeElementArchetype.Factory.Output(7, "Normal", ConnectionType.Vector3, 8),
                    NodeElementArchetype.Factory.Output(8, "Opacity", ConnectionType.Float, 9),
                    NodeElementArchetype.Factory.Output(9, "Refraction", ConnectionType.Float, 10),
                    NodeElementArchetype.Factory.Output(10, "Position Offset", ConnectionType.Vector3, 11),
                    NodeElementArchetype.Factory.Output(11, "Tessellation Multiplier", ConnectionType.Float, 12),
                    NodeElementArchetype.Factory.Output(12, "World Displacement", ConnectionType.Vector3, 13),
                    NodeElementArchetype.Factory.Output(13, "Subsurface Color", ConnectionType.Vector3, 14),
                }
            },
        };
    }
}
