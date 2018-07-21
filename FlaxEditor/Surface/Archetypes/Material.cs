// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
            /// <inheritdoc />
            public SurfaceNodeMaterial(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, surface, nodeArch, groupArch)
            {
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

                // Get material info
                MaterialInfo info;
                materialWindow.FillMaterialInfo(out info);
                bool isPostFx = info.Domain == MaterialDomain.PostProcess;
                bool isDecal = info.Domain == MaterialDomain.Decal;
                bool isGUI = info.Domain == MaterialDomain.GUI;
                bool isntLayered = !GetBox(0).HasAnyConnection;
                bool isSurface = info.Domain == MaterialDomain.Surface && isntLayered;
                bool isLitSurface = isSurface && info.BlendMode != MaterialBlendMode.Unlit;
                bool isTransparent = isSurface && info.BlendMode == MaterialBlendMode.Transparent;

                // Update boxes
                if (isDecal)
                {
                    var mode = info.DecalBlendingMode;
                    GetBox(1).Enabled = mode == MaterialDecalBlendingMode.Translucent || mode == MaterialDecalBlendingMode.Stain; // Color
                    GetBox(2).Enabled = true; // Mask
                    GetBox(3).Enabled = mode == MaterialDecalBlendingMode.Translucent || mode == MaterialDecalBlendingMode.Emissive; // Emissive
                    GetBox(4).Enabled = mode == MaterialDecalBlendingMode.Translucent; // Metalness
                    GetBox(5).Enabled = mode == MaterialDecalBlendingMode.Translucent; // Specular
                    GetBox(6).Enabled = mode == MaterialDecalBlendingMode.Translucent; // Roughness
                    GetBox(7).Enabled = false; // Ambient Occlusion
                    GetBox(8).Enabled = mode == MaterialDecalBlendingMode.Translucent || mode == MaterialDecalBlendingMode.Normal; // Normal
                    GetBox(9).Enabled = true; // Opacity
                    GetBox(10).Enabled = false; // Refraction
                    GetBox(11).Enabled = false; // Position Offset
                }
                else
                {
                    GetBox(1).Enabled = isLitSurface; // Color
                    GetBox(2).Enabled = isSurface || isGUI; // Mask
                    GetBox(3).Enabled = isSurface || isPostFx || isGUI; // Emissive
                    GetBox(4).Enabled = isLitSurface; // Metalness
                    GetBox(5).Enabled = isLitSurface; // Specular
                    GetBox(6).Enabled = isLitSurface; // Roughness
                    GetBox(7).Enabled = isLitSurface; // Ambient Occlusion
                    GetBox(8).Enabled = isLitSurface; // Normal
                    GetBox(9).Enabled = isTransparent || isPostFx || isGUI; // Opacity
                    GetBox(10).Enabled = isTransparent; // Refraction
                    GetBox(11).Enabled = isSurface; // Position Offset
                }
            }

            /// <inheritdoc />
            public override void OnLoaded()
            {
                base.OnLoaded();

                UpdateBoxes();
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                // Fix emissive box (it's a strange error)
                GetBox(3).CurrentType = ConnectionType.Vector3;
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
                Create = (id, surface, arch, groupArch) => new SurfaceNodeMaterial(id, surface, arch, groupArch),
                Title = "Material",
                Description = "Main material node",
                Flags = NodeFlags.MaterialOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(150, 240),
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
        };
    }
}
