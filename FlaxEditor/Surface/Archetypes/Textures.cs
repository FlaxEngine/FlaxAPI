// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Textures group.
    /// </summary>
    public static class Textures
    {
        /// <summary>
        /// Common samplers types.
        /// </summary>
        public enum CommonSamplerType
        {
            /// <summary>
            /// The linear clamp
            /// </summary>
            LinearClamp = 0,

            /// <summary>
            /// The point clamp
            /// </summary>
            PointClamp = 1,

            /// <summary>
            /// The linear wrap
            /// </summary>
            LinearWrap = 2,

            /// <summary>
            /// The point wrap
            /// </summary>
            PointWrap = 3,
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Title = "Texture",
                Description = "Two dimensional texture object",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(140, 120),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 6),
                    NodeElementArchetype.Factory.Output(1, "Color", ConnectionType.Vector4, 1),
                    NodeElementArchetype.Factory.Output(2, "R", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "G", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "B", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Output(5, "A", ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Asset(0, 20, 0, ContentDomain.Texture)
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Title = "Texcoords",
                AlternativeTitles = new string[] { "UV", "UVs" },
                Description = "Texture coordinates",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(110, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "UVs", ConnectionType.Vector2, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 3,
                Title = "Cube Texture",
                Description = "Set of 6 textures arranged in a cube",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(140, 120),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 6),
                    NodeElementArchetype.Factory.Output(1, "Color", ConnectionType.Vector4, 1),
                    NodeElementArchetype.Factory.Output(2, "R", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "G", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "B", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Output(5, "A", ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Asset(0, 20, 0, ContentDomain.CubeTexture)
                }
            },
            new NodeArchetype
            {
                TypeID = 4,
                Title = "Normal Map",
                Description = "Two dimensional texture object sampled as a normal map",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(140, 120),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 6),
                    NodeElementArchetype.Factory.Output(1, "Vector", ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "Z", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Asset(0, 20, 0, ContentDomain.Texture)
                }
            },
            new NodeArchetype
            {
                TypeID = 5,
                Title = "Parallax Occlusion Mapping",
                Description = "Parallax occlusion mapping",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(260, 120),
                DefaultValues = new object[]
                {
                    0.05f,
                    8.0f,
                    20.0f,
                    0
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Input(1, "Scale", true, ConnectionType.Float, 1, 0),
                    NodeElementArchetype.Factory.Input(2, "Min Steps", true, ConnectionType.Float, 2, 1),
                    NodeElementArchetype.Factory.Input(3, "Max Steps", true, ConnectionType.Float, 3, 2),
                    NodeElementArchetype.Factory.Input(4, "Heightmap Texture", true, ConnectionType.Object, 4),
                    NodeElementArchetype.Factory.Output(0, "Parallax UVs", ConnectionType.Vector2, 5),
                    NodeElementArchetype.Factory.Text(Surface.Constants.BoxSize + 4, 5 * Surface.Constants.LayoutOffsetY, "Channel"),
                    NodeElementArchetype.Factory.ComboBox(70, 5 * Surface.Constants.LayoutOffsetY, 50, 3, new[]
                    {
                        "R",
                        "G",
                        "B",
                        "A",
                    })
                }
            },
            new NodeArchetype
            {
                TypeID = 6,
                Title = "Scene Texture",
                Description = "Graphics pipeline textures lookup node",
                Flags = NodeFlags.MaterialGraph | NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(170, 120),
                DefaultValues = new object[]
                {
                    (int)MaterialSceneTextures.SceneColor
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 6),
                    NodeElementArchetype.Factory.Output(1, "Color", ConnectionType.Vector4, 1),
                    NodeElementArchetype.Factory.Output(2, "R", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "G", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "B", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Output(5, "A", ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.ComboBox(0, Surface.Constants.LayoutOffsetY, 120, 0, typeof(MaterialSceneTextures))
                }
            },
            new NodeArchetype
            {
                TypeID = 7,
                Title = "Scene Color",
                Description = "Scene color texture lookup node",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(140, 120),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 6),
                    NodeElementArchetype.Factory.Output(1, "Color", ConnectionType.Vector4, 1),
                    NodeElementArchetype.Factory.Output(2, "R", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "G", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "B", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Output(5, "A", ConnectionType.Float, 5)
                }
            },
            new NodeArchetype
            {
                TypeID = 8,
                Title = "Scene Depth",
                Description = "Scene depth buffer texture lookup node",
                Flags = NodeFlags.MaterialGraph | NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(140, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 6),
                    NodeElementArchetype.Factory.Output(1, "Depth", ConnectionType.Float, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 9,
                Title = "Sample Texture",
                Description = "Custom texture sampling",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(160, 110),
                DefaultValues = new object[]
                {
                    0,
                    -1.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Texture", true, ConnectionType.Object, 0),
                    NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector, 1),
                    NodeElementArchetype.Factory.Input(2, "Level", true, ConnectionType.Float, 2, 1),
                    NodeElementArchetype.Factory.Input(3, "Offset", true, ConnectionType.Vector2, 3),
                    NodeElementArchetype.Factory.Output(0, "Color", ConnectionType.Vector4, 4),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 4 + 4, "Sampler"),
                    NodeElementArchetype.Factory.ComboBox(50, Surface.Constants.LayoutOffsetY * 4, 100, 0, typeof(CommonSamplerType))
                }
            },
            new NodeArchetype
            {
                TypeID = 10,
                Title = "Flipbook",
                Description = "Texture sheet animation",
                Flags = NodeFlags.MaterialGraph,
                Size = new Vector2(160, 110),
                DefaultValues = new object[]
                {
                    0.0f,
                    new Vector2(4, 4),
                    false,
                    false
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Input(1, "Frame", true, ConnectionType.Float, 1, 0),
                    NodeElementArchetype.Factory.Input(2, "Frames X&Y", true, ConnectionType.Vector2, 2, 1),
                    NodeElementArchetype.Factory.Input(3, "Invert X", true, ConnectionType.Bool, 3, 2),
                    NodeElementArchetype.Factory.Input(4, "Invert Y", true, ConnectionType.Bool, 4, 3),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Vector2, 5),
                }
            },
            new NodeArchetype
            {
                TypeID = 11,
                Title = "Texture",
                Description = "Two dimensional texture object",
                Flags = NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(140, 80),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 0),
                    NodeElementArchetype.Factory.Asset(0, 0, 0, ContentDomain.Texture)
                }
            },
            /*new NodeArchetype
            {
                TypeID = 12,
                Title = "Cube Texture",
                Description = "Set of 6 textures arranged in a cube",
                Flags = NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(140, 80),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Object, 0),
                    NodeElementArchetype.Factory.Asset(0, 0, 0, ContentDomain.CubeTexture)
                }
            },*/
            new NodeArchetype
            {
                TypeID = 13,
                Title = "Load Texture",
                Description = "Reads data from the texture at the given location (unsigned integer in range [0; size-1] per axis, last component is a mipmap level)",
                Flags = NodeFlags.ParticleEmitterGraph,
                Size = new Vector2(160, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Vector4, 0),
                    NodeElementArchetype.Factory.Input(0, "Texture", true, ConnectionType.Object, 1),
                    NodeElementArchetype.Factory.Input(1, "Location", true, ConnectionType.Vector, 2),
                }
            },
        };
    }
}
