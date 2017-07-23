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
            },
            new NodeArchetype
            {
                TypeID = 3,
                Title = "Float",
                Description = "Constant floating point",
                Size = new Vector2(110, 20),
                DefaultValues = new object[]
                {
                    0.0f
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Float(0, 0, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 4,
                Title = "Vector2",
                Description = "Constant Vector2",
                Size = new Vector2(130, 60),
                DefaultValues = new object[]
                {
                    Vector2.Zero
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(1, "X", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(2, "Y", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Vector_X(0, Surface.Constants.LayoutOffsetY, 0),
                    NodeElementArchetype.Factory.Vector_Y(0, Surface.Constants.LayoutOffsetY, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 5,
                Title = "Vector3",
                Description = "Constant Vector3",
                Size = new Vector2(130, 80),
                DefaultValues = new object[]
                {
                    Vector3.Zero
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(1, "X", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(2, "Y", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "Z", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Vector_X(0, Surface.Constants.LayoutOffsetY, 0),
                    NodeElementArchetype.Factory.Vector_Y(0, Surface.Constants.LayoutOffsetY, 0),
                    NodeElementArchetype.Factory.Vector_Z(0, Surface.Constants.LayoutOffsetY, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 6,
                Title = "Vector4",
                Description = "Constant Vector4",
                Size = new Vector2(130, 100),
                DefaultValues = new object[]
                {
                    Vector4.Zero
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Vector4, 0),
                    NodeElementArchetype.Factory.Output(1, "X", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(2, "Y", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "Z", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "W", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Vector_X(0, Surface.Constants.LayoutOffsetY, 0),
                    NodeElementArchetype.Factory.Vector_Y(0, Surface.Constants.LayoutOffsetY, 0),
                    NodeElementArchetype.Factory.Vector_Z(0, Surface.Constants.LayoutOffsetY, 0),
                    NodeElementArchetype.Factory.Vector_W(0, Surface.Constants.LayoutOffsetY, 0)
                }
            },
        };
    }
}
