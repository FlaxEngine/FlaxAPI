// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Math group.
    /// </summary>
    public static class Math
    {
        private static NodeArchetype Op1(ushort id, string title, string desc, ConnectionType type = ConnectionType.Variable)
        {
            return new NodeArchetype
            {
                TypeID = id,
                Title = title,
                Description = desc,
                Size = new Vector2(110, 20),
                DefaultType = type,
                IndependentBoxes = new[] { 0 },
                DependentBoxes = new[] { 1 },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "A", true, type, 0),
                    NodeElementArchetype.Factory.Output(0, "Result", type, 1)
                }
            };
        }

        private static NodeArchetype Op2(ushort id, string title, string desc, ConnectionType inputType = ConnectionType.Variable, ConnectionType outputType = ConnectionType.Variable, bool isOutputDependant = true, object[] defaultValues = null)
        {
            return Op2(id, title, desc, null, inputType, outputType, isOutputDependant, defaultValues);
        }

        private static NodeArchetype Op2(ushort id, string title, string desc, string[] altTitles, ConnectionType inputType = ConnectionType.Variable, ConnectionType outputType = ConnectionType.Variable, bool isOutputDependant = true, object[] defaultValues = null)
        {
            return new NodeArchetype
            {
                TypeID = id,
                Title = title,
                Description = desc,
                AlternativeTitles = altTitles,
                Size = new Vector2(110, 40),
                DefaultType = inputType,
                IndependentBoxes = new[]
                {
                    0,
                    1
                },
                DependentBoxes = isOutputDependant ? new[] { 2 } : null,
                DefaultValues = defaultValues ?? new object[]
                {
                    0.0f,
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "A", true, inputType, 0, 0),
                    NodeElementArchetype.Factory.Input(1, "B", true, inputType, 1, 1),
                    NodeElementArchetype.Factory.Output(0, "Result", outputType, 2)
                }
            };
        }

        private static readonly string[] VectorTransformSpaces =
        {
            "World",
            "Tangent",
            "View",
            "Local",
        };

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            Op2(1, "Add", "Result is sum A and B", new[] { "+" }),
            Op2(2, "Subtract", "Result is difference A and B", new[] { "-" }),
            Op2(3, "Multiply", "Result is A times B", new[] { "*" }, defaultValues: new object[] { 1.0f, 1.0f }),
            Op2(4, "Modulo", "Result is remainder A from A divided by B B", new[] { "%" }),
            Op2(5, "Divide", "Result is A divided by B", new[] { "/" }, defaultValues: new object[] { 1.0f, 1.0f }),
            Op1(7, "Absolute", "Result is absolute value of A"),
            Op1(8, "Ceil", "Returns the smallest integer value greater than or equal to A"),
            Op1(9, "Cosine", "Returns cosine of A"),
            Op1(10, "Floor", "Returns the largest integer value less than or equal to A"),
            new NodeArchetype
            {
                TypeID = 11,
                Title = "Length",
                Description = "Returns the length of A vector",
                Size = new Vector2(110, 20),
                DefaultType = ConnectionType.Vector,
                IndependentBoxes = new[] { 0 },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "A", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Float, 1)
                }
            },
            Op1(12, "Normalize", "Returns normalized A vector", ConnectionType.Vector),
            Op1(13, "Round", "Rounds A to the nearest integer"),
            Op1(14, "Saturate", "Clamps A to the range [0, 1]"),
            Op1(15, "Sine", "Returns sine of A"),
            Op1(16, "Sqrt", "Returns square root of A"),
            Op1(17, "Tangent", "Returns tangent of A"),
            Op2(18, "Cross", "Returns the cross product of A and B", ConnectionType.Vector3),
            Op2(19, "Distance", "Returns a distance scalar between A and B", ConnectionType.Vector, ConnectionType.Float, false),
            Op2(20, "Dot", "Returns the dot product of A and B", ConnectionType.Vector, ConnectionType.Float, false),
            Op2(21, "Max", "Selects the greater of A and B"),
            Op2(22, "Min", "Selects the lesser of A and B"),
            Op2(23, "Power", "Returns A raised to the specified at B power", new[]
            {
                "^",
                "**"
            }),
            //
            new NodeArchetype
            {
                TypeID = 24,
                Title = "Clamp",
                Description = "Clamps value to the specified range",
                Size = new Vector2(110, 60),
                DefaultType = ConnectionType.Variable,
                IndependentBoxes = new[] { 0 },
                DependentBoxes = new[]
                {
                    1,
                    2,
                    3
                },
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Input", true, ConnectionType.Variable, 0),
                    NodeElementArchetype.Factory.Input(1, "Min", true, ConnectionType.Variable, 1, 0),
                    NodeElementArchetype.Factory.Input(2, "Max", true, ConnectionType.Variable, 2, 1),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Variable, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 25,
                Title = "Lerp",
                Description = "Performs a linear interpolation",
                Size = new Vector2(110, 60),
                DefaultType = ConnectionType.Variable,
                IndependentBoxes = new[]
                {
                    0,
                    1
                },
                DependentBoxes = new[] { 3 },
                DefaultValues = new object[]
                {
                    0.0f,
                    1.0f,
                    0.5f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "A", true, ConnectionType.Variable, 0, 0),
                    NodeElementArchetype.Factory.Input(1, "B", true, ConnectionType.Variable, 1, 1),
                    NodeElementArchetype.Factory.Input(2, "Alpha", true, ConnectionType.Float, 2, 2),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Variable, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 26,
                Title = "Reflect",
                Description = "Returns reflected vector over the normal",
                Size = new Vector2(110, 40),
                DefaultType = ConnectionType.Variable,
                IndependentBoxes = new[]
                {
                    0,
                    1
                },
                DependentBoxes = new[] { 2 },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Vector", true, ConnectionType.Vector, 0),
                    NodeElementArchetype.Factory.Input(1, "Normal", true, ConnectionType.Vector, 1),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Vector, 2)
                }
            },
            //
            Op1(27, "Negate", "Returns opposite value"),
            Op1(28, "One Minus", "Returns 1 - value"),
            //
            new NodeArchetype
            {
                TypeID = 29,
                Title = "Derive Normal Z",
                Description = "Derives the Z component of a tangent space normal given the X and Y components and outputs the resulting three-channel tangent space normal",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(170, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "In XY", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Vector3, 1)
                }
            },
            new NodeArchetype
            {
                TypeID = 30,
                Title = "Vector Transform",
                Description = "Transform vector from source space to destination space",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(170, 40),
                DefaultValues = new object[]
                {
                    (int)TransformCoordinateSystem.World,
                    (int)TransformCoordinateSystem.Tangent,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Input", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(0, "Output", ConnectionType.Vector3, 1),
                    NodeElementArchetype.Factory.ComboBox(0, 22, 70, 0, VectorTransformSpaces),
                    NodeElementArchetype.Factory.ComboBox(100, 22, 70, 1, VectorTransformSpaces),
                }
            },
            new NodeArchetype
            {
                TypeID = 31,
                Title = "Mad",
                Description = "Performs value multiplication and addition at once",
                Size = new Vector2(110, 60),
                DefaultType = ConnectionType.Variable,
                IndependentBoxes = new[]
                {
                    0,
                    1,
                    2
                },
                DependentBoxes = new[] { 3 },
                DefaultValues = new object[]
                {
                    1.0f,
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Variable, 0),
                    NodeElementArchetype.Factory.Input(1, "Multiply", true, ConnectionType.Variable, 1, 0),
                    NodeElementArchetype.Factory.Input(2, "Add", true, ConnectionType.Variable, 2, 1),
                    NodeElementArchetype.Factory.Output(0, "Result", ConnectionType.Variable, 3)
                }
            },
            new NodeArchetype
            {
                TypeID = 32,
                Title = "Largest Component Mask",
                Description = "Gets the largest component mask from the input vector",
                Size = new Vector2(220, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Value", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(0, "Mask", ConnectionType.Vector3, 1)
                }
            },
            Op1(33, "Asin", "Returns arcus sine of A"),
            Op1(34, "Acos", "Returns arcus cosinus of A"),
            Op1(35, "Atan", "Returns arcus tangent of A"),
            new NodeArchetype
            {
                TypeID = 36,
                Title = "Bias and Scale",
                Description = "Adds a constant to input and scales it",
                Size = new Vector2(200, 60),
                IndependentBoxes = new[] { 0 },
                DependentBoxes = new[] { 1 },
                DefaultType = ConnectionType.Variable,
                DefaultValues = new object[]
                {
                    0.0f,
                    1.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Input", true, ConnectionType.Variable, 0),
                    NodeElementArchetype.Factory.Output(0, "Output", ConnectionType.Variable, 1),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 1, "Bias"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 2, "Scale"),
                    NodeElementArchetype.Factory.Float(40, Surface.Constants.LayoutOffsetY * 1, 0),
                    NodeElementArchetype.Factory.Float(40, Surface.Constants.LayoutOffsetY * 2, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 37,
                Title = "Rotate About Axis",
                Description = "Rotates given vector using the rotation axis, a point on the axis, and the angle to rotate",
                Size = new Vector2(200, 80),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Normalized Rotation Axis", true, ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Input(1, "Rotation Angle", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Input(2, "Pivot Point", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Input(3, "Position", true, ConnectionType.Vector3, 3),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Vector3, 4),
                }
            },
            Op1(38, "Trunc", "Truncates a floating-point value to the integer component"),
            Op1(39, "Frac", "Returns fractional part of the value"),
            Op2(40, "Fmod", "Returns the floating-point remainder of A/B"),
            Op2(41, "Atan2", "Returns arctangent tangent of two values (A, B)"),
            new NodeArchetype
            {
                TypeID = 42,
                Flags = NodeFlags.AnimGraphOnly,
                Title = "Near Equal",
                Description = "Determines if two values are nearly equal within a given epsilon",
                Size = new Vector2(200, 80),
                IndependentBoxes = new[]
                {
                    0,
                    1,
                },
                DefaultType = ConnectionType.Variable,
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f,
                    0.0001f
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "A", true, ConnectionType.Variable, 0, 0),
                    NodeElementArchetype.Factory.Input(1, "B", true, ConnectionType.Variable, 1, 1),
                    NodeElementArchetype.Factory.Input(2, "Epsilon", true, ConnectionType.Float, 2, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Bool, 3),
                }
            },
        };
    }
}
