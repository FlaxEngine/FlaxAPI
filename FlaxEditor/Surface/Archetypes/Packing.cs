// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Surface.Elements;
using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Packing group.
    /// </summary>
    public static class Packing
    {
        private class AppendNode : SurfaceNode
        {
            private InputBox _in0;
            private InputBox _in1;
            private OutputBox _out;

            /// <inheritdoc />
            public AppendNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                _in0 = (InputBox)GetBox(0);
                _in1 = (InputBox)GetBox(1);
                _out = (OutputBox)GetBox(2);

                _in0.CurrentTypeChanged += UpdateOutputType;
                _in1.CurrentTypeChanged += UpdateOutputType;

                UpdateOutputType(null);
            }

            private static int CountComponents(ConnectionType type)
            {
                switch (type)
                {
                case ConnectionType.Bool:
                case ConnectionType.Integer:
                case ConnectionType.Float:
                case ConnectionType.UnsignedInteger: return 1;
                case ConnectionType.Vector2: return 2;
                case ConnectionType.Vector3: return 3;
                case ConnectionType.Vector4: return 4;
                default: return 0;
                }
            }

            private void UpdateOutputType(Box box)
            {
                if (!_in0.HasAnyConnection || !_in1.HasAnyConnection)
                {
                    _out.CurrentType = ConnectionType.Vector;
                    return;
                }

                var count0 = CountComponents(_in0.CurrentType);
                var count1 = CountComponents(_in1.CurrentType);
                var count = count0 + count1;
                ConnectionType outType = ConnectionType.Vector4;
                switch (count)
                {
                case 1:
                    outType = ConnectionType.Float;
                    break;
                case 2:
                    outType = ConnectionType.Vector2;
                    break;
                case 3:
                    outType = ConnectionType.Vector3;
                    break;
                case 4:
                    outType = ConnectionType.Vector4;
                    break;
                }
                _out.CurrentType = outType;
            }
        }

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
                Flags = NodeFlags.AllGraphs,
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
                Flags = NodeFlags.AllGraphs,
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
                Flags = NodeFlags.AllGraphs,
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
            new NodeArchetype
            {
                TypeID = 23,
                Title = "Pack Rotation",
                Description = "Pack components to Rotation",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(150, 80),
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f,
                    0.0f
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Rotation, 0),
                    NodeElementArchetype.Factory.Input(0, "Pitch", true, ConnectionType.Float, 1, 0),
                    NodeElementArchetype.Factory.Input(1, "Yaw", true, ConnectionType.Float, 2, 1),
                    NodeElementArchetype.Factory.Input(2, "Roll", true, ConnectionType.Float, 3, 2),
                }
            },
            new NodeArchetype
            {
                TypeID = 24,
                Title = "Pack Transform",
                Description = "Pack components to Transform",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(150, 80),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Transform, 0),
                    NodeElementArchetype.Factory.Input(0, "Translation", true, ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Input(1, "Orientation", true, ConnectionType.Rotation, 2),
                    NodeElementArchetype.Factory.Input(2, "Scale", true, ConnectionType.Vector3, 3),
                }
            },
            // TODO: 25 - Pack box

            // Unpacking
            new NodeArchetype
            {
                TypeID = 30,
                Title = "Unpack Vector2",
                Description = "Unpack components from Vector2",
                Flags = NodeFlags.AllGraphs,
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
                Flags = NodeFlags.AllGraphs,
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
                Flags = NodeFlags.AllGraphs,
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
            new NodeArchetype
            {
                TypeID = 33,
                Title = "Unpack Rotation",
                Description = "Unpack components from Rotation",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(170, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Rotation, 0),
                    NodeElementArchetype.Factory.Output(0, "Pitch", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(1, "Yaw", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(2, "Roll", ConnectionType.Float, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 34,
                Title = "Unpack Transform",
                Description = "Unpack components from Transform",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(170, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Transform, 0),
                    NodeElementArchetype.Factory.Output(0, "Translation", ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.Output(1, "Orientation", ConnectionType.Rotation, 2),
                    NodeElementArchetype.Factory.Output(2, "Scale", ConnectionType.Vector3, 3)
                }
            },
            // TODO: 35 - Unpack box

            // Mask X,Y,Z,W
            new NodeArchetype
            {
                TypeID = 40,
                Title = "Mask X",
                Description = "Unpack X component from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "X", ConnectionType.Float, 1)
                }
            },
            new NodeArchetype
            {
                TypeID = 41,
                Title = "Mask Y",
                Description = "Unpack Y component from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "Y", ConnectionType.Float, 1)
                }
            },
            new NodeArchetype
            {
                TypeID = 42,
                Title = "Mask Z",
                Description = "Unpack Z component from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "Z", ConnectionType.Float, 1)
                }
            },
            new NodeArchetype
            {
                TypeID = 43,
                Title = "Mask W",
                Description = "Unpack W component from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "W", ConnectionType.Float, 1)
                }
            },

            // Mask XY, YZ, XZ,...
            new NodeArchetype
            {
                TypeID = 44,
                Title = "Mask XY",
                Description = "Unpack XY components from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "XY", ConnectionType.Vector2, 1)
                }
            },
            new NodeArchetype
            {
                TypeID = 45,
                Title = "Mask XZ",
                Description = "Unpack XZ components from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "XZ", ConnectionType.Vector2, 1)
                }
            },
            new NodeArchetype
            {
                TypeID = 46,
                Title = "Mask YZ",
                Description = "Unpack YZ components from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "YZ", ConnectionType.Vector2, 1)
                }
            },

            // Mask XYZ
            new NodeArchetype
            {
                TypeID = 70,
                Title = "Mask XYZ",
                Description = "Unpack XYZ components from Vector",
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(160, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "XYZ", ConnectionType.Vector3, 1)
                }
            },

            new NodeArchetype
            {
                TypeID = 100,
                Title = "Append",
                Description = "Appends vector or scalar value into vector",
                Create = (id, context, nodeArch, groupArch) => new AppendNode(id, context, nodeArch, groupArch),
                Flags = NodeFlags.AllGraphs,
                Size = new Vector2(140, 50),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.Scalar | ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Input(1, string.Empty, true, ConnectionType.Variable | ConnectionType.Vector, 1),
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Vector, 2)
                }
            },
        };
    }
}
