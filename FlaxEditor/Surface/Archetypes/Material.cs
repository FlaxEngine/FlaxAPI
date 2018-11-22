// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Surface.Elements;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Material group.
    /// </summary>
    public static class Material
    {
        /// <summary>
        /// Customized <see cref="SurfaceNode"/> for main material node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class SurfaceNodeMaterial : SurfaceNode
        {
            /// <summary>
            /// Material node input boxes (each enum item value maps to box ID).
            /// </summary>
            public enum MaterialNodeBoxes
            {
                /// <summary>
                /// The layer input.
                /// </summary>
                Layer = 0,

                /// <summary>
                /// The color input.
                /// </summary>
                Color = 1,

                /// <summary>
                /// The mask input.
                /// </summary>
                Mask = 2,

                /// <summary>
                /// The emissive input.
                /// </summary>
                Emissive = 3,

                /// <summary>
                /// The metalness input.
                /// </summary>
                Metalness = 4,

                /// <summary>
                /// The specular input.
                /// </summary>
                Specular = 5,

                /// <summary>
                /// The roughness input.
                /// </summary>
                Roughness = 6,

                /// <summary>
                /// The ambient occlusion input.
                /// </summary>
                AmbientOcclusion = 7,

                /// <summary>
                /// The normal input.
                /// </summary>
                Normal = 8,

                /// <summary>
                /// The opacity input.
                /// </summary>
                Opacity = 9,

                /// <summary>
                /// The refraction input.
                /// </summary>
                Refraction = 10,

                /// <summary>
                /// The position offset input.
                /// </summary>
                PositionOffset = 11,

                /// <summary>
                /// The tessellation multiplier input.
                /// </summary>
                TessellationMultiplier = 12,

                /// <summary>
                /// The world displacement input.
                /// </summary>
                WorldDisplacement = 13,

                /// <summary>
                /// The subsurface color input.
                /// </summary>
                SubsurfaceColor = 14,
            };

            /// <inheritdoc />
            public SurfaceNodeMaterial(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <summary>
            /// Gets the material box.
            /// </summary>
            /// <param name="box">The input type.</param>
            /// <returns>The box</returns>
            public Box GetBox(MaterialNodeBoxes box)
            {
                return GetBox((int)box);
            }

            /// <summary>
            /// Update material node boxes
            /// </summary>
            public void UpdateBoxes()
            {
                // Try get parent material window
                // Maybe too hacky :D
                if (!(Surface.Owner is MaterialWindow materialWindow) || materialWindow.Item == null)
                    return;

                // Layered material
                if (GetBox(MaterialNodeBoxes.Layer).HasAnyConnection)
                {
                    GetBox(MaterialNodeBoxes.Color).Enabled = false;
                    GetBox(MaterialNodeBoxes.Mask).Enabled = false;
                    GetBox(MaterialNodeBoxes.Emissive).Enabled = false;
                    GetBox(MaterialNodeBoxes.Metalness).Enabled = false;
                    GetBox(MaterialNodeBoxes.Specular).Enabled = false;
                    GetBox(MaterialNodeBoxes.Roughness).Enabled = false;
                    GetBox(MaterialNodeBoxes.AmbientOcclusion).Enabled = false;
                    GetBox(MaterialNodeBoxes.Normal).Enabled = false;
                    GetBox(MaterialNodeBoxes.Opacity).Enabled = false;
                    GetBox(MaterialNodeBoxes.Refraction).Enabled = false;
                    GetBox(MaterialNodeBoxes.PositionOffset).Enabled = false;
                    GetBox(MaterialNodeBoxes.TessellationMultiplier).Enabled = false;
                    GetBox(MaterialNodeBoxes.WorldDisplacement).Enabled = false;
                    GetBox(MaterialNodeBoxes.SubsurfaceColor).Enabled = false;
                    return;
                }

                // Get material info
                MaterialInfo info;
                materialWindow.FillMaterialInfo(out info);

                // Update boxes
                switch (info.Domain)
                {
                case MaterialDomain.Surface:
                case MaterialDomain.Terrain:
                {
                    bool isNotUnlit = info.ShadingModel != MaterialShadingModel.Unlit;
                    bool isTransparent = info.BlendMode == MaterialBlendMode.Transparent;
                    bool withTess = info.TessellationMode != TessellationMethod.None;
                    bool withSubsurface = info.ShadingModel == MaterialShadingModel.Subsurface;

                    GetBox(MaterialNodeBoxes.Color).Enabled = isNotUnlit;
                    GetBox(MaterialNodeBoxes.Mask).Enabled = true;
                    GetBox(MaterialNodeBoxes.Emissive).Enabled = true;
                    GetBox(MaterialNodeBoxes.Metalness).Enabled = isNotUnlit;
                    GetBox(MaterialNodeBoxes.Specular).Enabled = isNotUnlit;
                    GetBox(MaterialNodeBoxes.Roughness).Enabled = isNotUnlit;
                    GetBox(MaterialNodeBoxes.AmbientOcclusion).Enabled = isNotUnlit;
                    GetBox(MaterialNodeBoxes.Normal).Enabled = isNotUnlit;
                    GetBox(MaterialNodeBoxes.Opacity).Enabled = isTransparent || withSubsurface;
                    GetBox(MaterialNodeBoxes.Refraction).Enabled = isTransparent;
                    GetBox(MaterialNodeBoxes.PositionOffset).Enabled = true;
                    GetBox(MaterialNodeBoxes.TessellationMultiplier).Enabled = withTess;
                    GetBox(MaterialNodeBoxes.WorldDisplacement).Enabled = withTess;
                    GetBox(MaterialNodeBoxes.SubsurfaceColor).Enabled = withSubsurface;
                    break;
                }
                case MaterialDomain.PostProcess:
                {
                    GetBox(MaterialNodeBoxes.Color).Enabled = false;
                    GetBox(MaterialNodeBoxes.Mask).Enabled = false;
                    GetBox(MaterialNodeBoxes.Emissive).Enabled = true;
                    GetBox(MaterialNodeBoxes.Metalness).Enabled = false;
                    GetBox(MaterialNodeBoxes.Specular).Enabled = false;
                    GetBox(MaterialNodeBoxes.Roughness).Enabled = false;
                    GetBox(MaterialNodeBoxes.AmbientOcclusion).Enabled = false;
                    GetBox(MaterialNodeBoxes.Normal).Enabled = false;
                    GetBox(MaterialNodeBoxes.Opacity).Enabled = true;
                    GetBox(MaterialNodeBoxes.Refraction).Enabled = false;
                    GetBox(MaterialNodeBoxes.PositionOffset).Enabled = false;
                    GetBox(MaterialNodeBoxes.TessellationMultiplier).Enabled = false;
                    GetBox(MaterialNodeBoxes.WorldDisplacement).Enabled = false;
                    GetBox(MaterialNodeBoxes.SubsurfaceColor).Enabled = false;
                    break;
                }
                case MaterialDomain.Decal:
                {
                    var mode = info.DecalBlendingMode;

                    GetBox(MaterialNodeBoxes.Color).Enabled = mode == MaterialDecalBlendingMode.Translucent || mode == MaterialDecalBlendingMode.Stain;
                    GetBox(MaterialNodeBoxes.Mask).Enabled = true;
                    GetBox(MaterialNodeBoxes.Emissive).Enabled = mode == MaterialDecalBlendingMode.Translucent || mode == MaterialDecalBlendingMode.Emissive;
                    GetBox(MaterialNodeBoxes.Metalness).Enabled = mode == MaterialDecalBlendingMode.Translucent;
                    GetBox(MaterialNodeBoxes.Specular).Enabled = mode == MaterialDecalBlendingMode.Translucent;
                    GetBox(MaterialNodeBoxes.Roughness).Enabled = mode == MaterialDecalBlendingMode.Translucent;
                    GetBox(MaterialNodeBoxes.AmbientOcclusion).Enabled = false;
                    GetBox(MaterialNodeBoxes.Normal).Enabled = mode == MaterialDecalBlendingMode.Translucent || mode == MaterialDecalBlendingMode.Normal;
                    GetBox(MaterialNodeBoxes.Opacity).Enabled = true;
                    GetBox(MaterialNodeBoxes.Refraction).Enabled = false;
                    GetBox(MaterialNodeBoxes.PositionOffset).Enabled = false;
                    GetBox(MaterialNodeBoxes.TessellationMultiplier).Enabled = false;
                    GetBox(MaterialNodeBoxes.WorldDisplacement).Enabled = false;
                    GetBox(MaterialNodeBoxes.SubsurfaceColor).Enabled = false;
                    break;
                }
                case MaterialDomain.GUI:
                {
                    GetBox(MaterialNodeBoxes.Color).Enabled = false;
                    GetBox(MaterialNodeBoxes.Mask).Enabled = true;
                    GetBox(MaterialNodeBoxes.Emissive).Enabled = true;
                    GetBox(MaterialNodeBoxes.Metalness).Enabled = false;
                    GetBox(MaterialNodeBoxes.Specular).Enabled = false;
                    GetBox(MaterialNodeBoxes.Roughness).Enabled = false;
                    GetBox(MaterialNodeBoxes.AmbientOcclusion).Enabled = false;
                    GetBox(MaterialNodeBoxes.Normal).Enabled = false;
                    GetBox(MaterialNodeBoxes.Opacity).Enabled = true;
                    GetBox(MaterialNodeBoxes.Refraction).Enabled = false;
                    GetBox(MaterialNodeBoxes.PositionOffset).Enabled = false;
                    GetBox(MaterialNodeBoxes.TessellationMultiplier).Enabled = false;
                    GetBox(MaterialNodeBoxes.WorldDisplacement).Enabled = false;
                    GetBox(MaterialNodeBoxes.SubsurfaceColor).Enabled = false;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
                }
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                // Fix emissive box (it's a strange error)
                GetBox(3).CurrentType = ConnectionType.Vector3;

                UpdateBoxes();
            }

            /// <inheritdoc />
            public override void ConnectionTick(Box box)
            {
                base.ConnectionTick(box);

                UpdateBoxes();
            }
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Create = (id, context, arch, groupArch) => new SurfaceNodeMaterial(id, context, arch, groupArch),
                Title = "Material",
                Description = "Main material node",
                Flags = NodeFlags.MaterialOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI | NodeFlags.NoCloseButton,
                Size = new Vector2(150, 300),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "", true, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(1, "Color", true, ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Input(2, "Mask", true, ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Input(3, "Emissive", true, ConnectionType.Vector3, 3),
                    NodeElementArchetype.Factory.Input(4, "Metalness", true, ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Input(5, "Specular", true, ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Input(6, "Roughness", true, ConnectionType.Float, 6),
                    NodeElementArchetype.Factory.Input(7, "Ambient Occlusion", true, ConnectionType.Float, 7),
                    NodeElementArchetype.Factory.Input(8, "Normal", true, ConnectionType.Vector3, 8),
                    NodeElementArchetype.Factory.Input(9, "Opacity", true, ConnectionType.Float, 9),
                    NodeElementArchetype.Factory.Input(10, "Refraction", true, ConnectionType.Float, 10),
                    NodeElementArchetype.Factory.Input(11, "Position Offset", true, ConnectionType.Vector3, 11),
                    NodeElementArchetype.Factory.Input(12, "Tessellation Multiplier", true, ConnectionType.Float, 12),
                    NodeElementArchetype.Factory.Input(13, "World Displacement", true, ConnectionType.Vector3, 13),
                    NodeElementArchetype.Factory.Input(14, "Subsurface Color", true, ConnectionType.Vector3, 14),
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Title = "World Position",
                Description = "Absolute world space position",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "XYZ", ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 3,
                Title = "View",
                Description = "View properties",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Position", ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(1, "Direction", ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Output(2, "Far Plane", ConnectionType.Float, 2),
                }
            },
            new NodeArchetype
            {
                TypeID = 4,
                Title = "Normal Vector",
                Description = "World space normal vector",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Normal", ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 5,
                Title = "Camera Vector",
                Description = "Calculates camera vector",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Vector", ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 6,
                Title = "Screen Position",
                Description = "Gathers screen position or texcoord",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(140, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Position", ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(1, "Texcoord", ConnectionType.Vector2, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 7,
                Title = "Screen Size",
                Description = "Gathers screen size",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(120, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Size", ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(1, "Inv Size", ConnectionType.Vector2, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 8,
                Title = "Custom Code",
                Description = "Custom HLSL shader code expression",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(300, 200),
                DefaultValues = new object[]
                {
                    "// Here you can add HLSL code\nOutput0 = Input0;"
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Input0", true, ConnectionType.Vector4, 0),
                    NodeElementArchetype.Factory.Input(1, "Input1", true, ConnectionType.Vector4, 1),
                    NodeElementArchetype.Factory.Input(2, "Input2", true, ConnectionType.Vector4, 2),
                    NodeElementArchetype.Factory.Input(3, "Input3", true, ConnectionType.Vector4, 3),
                    NodeElementArchetype.Factory.Input(4, "Input4", true, ConnectionType.Vector4, 4),
                    NodeElementArchetype.Factory.Input(5, "Input5", true, ConnectionType.Vector4, 5),
                    NodeElementArchetype.Factory.Input(6, "Input6", true, ConnectionType.Vector4, 6),
                    NodeElementArchetype.Factory.Input(7, "Input7", true, ConnectionType.Vector4, 7),

                    NodeElementArchetype.Factory.Output(0, "Output0", ConnectionType.Vector4, 8),
                    NodeElementArchetype.Factory.Output(1, "Output1", ConnectionType.Vector4, 9),
                    NodeElementArchetype.Factory.Output(2, "Output2", ConnectionType.Vector4, 10),
                    NodeElementArchetype.Factory.Output(3, "Output3", ConnectionType.Vector4, 11),

                    NodeElementArchetype.Factory.TextBox(60, 0, 175, 200, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 9,
                Title = "Object Position",
                Description = "Absolute world space object position",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "XYZ", ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 10,
                Title = "Two Sided Sign",
                Description = "Scalar value with surface side sign. 1 for normal facing, -1 for inverted",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 11,
                Title = "Camera Depth Fade",
                Description = "Creates a gradient of 0 near the camera to white at fade length. Useful for preventing particles from camera clipping.",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(200, 60),
                DefaultValues = new object[]
                {
                    500.0f,
                    24.0f
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Fade Length", true, ConnectionType.Float, 0, 0),
                    NodeElementArchetype.Factory.Input(1, "Fade Offset", true, ConnectionType.Float, 1, 1),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Float, 2),
                }
            },
            new NodeArchetype
            {
                TypeID = 12,
                Title = "Vertex Color",
                Description = "Per vertex color",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Color", ConnectionType.Vector4, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 13,
                Title = "Pre-skinned Local Position",
                Description = "Per vertex local position (before skinning)",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(230, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 14,
                Title = "Pre-skinned Local Normal",
                Description = "Per vertex local normal (before skinning)",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(230, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 15,
                Title = "Depth",
                Description = "Current pixel/vertex linear distance to the camera",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(100, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Float, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 16,
                Title = "Tangent Vector",
                Description = "World space tangent vector",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(160, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Tangent", ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 17,
                Title = "Bitangent Vector",
                Description = "World space bitangent vector",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(160, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Bitangent", ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 18,
                Title = "Camera Position",
                Description = "World space camera location",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(160, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "XYZ", ConnectionType.Vector3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 19,
                Title = "Per Instance Random",
                Description = "Per object instance random value (normalized to range 0-1)",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(200, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 20,
                Title = "Interpolate VS To PS",
                Description = "Helper node used to pass data from Vertex Shader to Pixel Shader",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(220, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Vertex Shader", true, ConnectionType.Vector4, 0),
                    NodeElementArchetype.Factory.Output(0, "Pixel Shader", ConnectionType.Vector4, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 21,
                Title = "Terrain Holes Mask",
                Description = "Scalar terrain visibility mask used mostly for creating holes in terrain",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(200, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 22,
                Title = "Terrain Layer Weight",
                Description = "Terrain layer weight mask used for blending terrain layers",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(220, 30),
                DefaultValues = new object[]
                {
                    0,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.ComboBox(0, 0, 70.0f, 0, FlaxEditor.Tools.Terrain.PaintTerrainGizmoMode.TerrainLayerNames),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 0),
                }
            },
        };
    }
}
