// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
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
                },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    if (filterText == "true")
                    {
                        data = new object[] { true };
                        return true;
                    }
                    else if (filterText == "false")
                    {
                        data = new object[] { false };
                        return true;
                    }
                    return false;
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Title = "Integer",
                Description = "Constant integer value",
                Size = new Vector2(110, 20),
                DefaultValues = new object[]
                {
                    0
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Value", ConnectionType.Integer, 0),
                    NodeElementArchetype.Factory.Integer(0, 0, 0)
                },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    int number;
                    if (int.TryParse(filterText, out number))
                    {
                        data = new object[] { number };
                        return true;
                    }
                    return false;
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
                },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    float[] values;
                    if (TryParseValues(filterText, out values) && values.Length < 2)
                    {
                        data = new object[] { values[0] };
                        return true;
                    }
                    return false;
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
                },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    float[] values;
                    if (TryParseValues(filterText, out values) && values.Length < 3)
                    {
                        data = new object[] { new Vector2(ValuesToVector4(values)) };
                        return true;
                    }
                    return false;
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
                },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    float[] values;
                    if (TryParseValues(filterText, out values) && values.Length < 4)
                    {
                        data = new object[] { new Vector3(ValuesToVector4(values)) };
                        return true;
                    }
                    return false;
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
                },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    float[] values;
                    if (TryParseValues(filterText, out values))
                    {
                        data = new object[] { ValuesToVector4(values) };
                        return true;
                    }
                    return false;
                }
            },
            new NodeArchetype
            {
                TypeID = 7,
                Title = "Color",
                Description = "RGBA color",
                Size = new Vector2(70, 100),
                DefaultValues = new object[]
                {
                    Color.White
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Vector4, 0),
                    NodeElementArchetype.Factory.Output(1, "R", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(2, "G", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "B", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "A", ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Color(0, 0, 0)
                },
                TryParseText = (string filterText, out object[] data) =>
                {
                    data = null;
                    if (!filterText.StartsWith("#")) return false;
                    Color color;
                    if (Color.TryParseHex(filterText, out color))
                    {
                        data = new object[] { color };
                        return true;
                    }
                    return false;
                }
            },
            new NodeArchetype
            {
                TypeID = 8,
                Title = "Rotation",
                Description = "Euler angle rotation",
                Size = new Vector2(110, 60),
                DefaultValues = new object[]
                {
                    0.0f,
                    0.0f,
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Rotation, 0),
                    NodeElementArchetype.Factory.Float(32, 0, 0),
                    NodeElementArchetype.Factory.Float(32, Surface.Constants.LayoutOffsetY, 1),
                    NodeElementArchetype.Factory.Float(32, Surface.Constants.LayoutOffsetY * 2, 2),
                    NodeElementArchetype.Factory.Text(0, 0, "Pitch:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY, "Yaw:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 2, "Roll:"),
                }
            },
            // TODO: 9 - String
            new NodeArchetype
            {
                TypeID = 10,
                Title = "PI",
                Description = "A value specifying the approximation of π which is 180 degrees",
                Size = new Vector2(50, 20),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "π", ConnectionType.Float, 0),
                }
            },
        };

        /// <summary>
        /// Tries to parse a list of numbers separated by commas
        /// </summary>
        private static bool TryParseValues(string filterText, out float[] values)
        {
            float[] vec = new float[4];
            int count = 0;
            if (ExtractNumber(ref filterText, out vec[count]))
            {
                count = count + 1;
                while (count < 4)
                {
                    if (ExtractComma(ref filterText) && ExtractNumber(ref filterText, out vec[count]))
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                // If the user inputted something like 3+2.2, it can't be turned into a single node
                if (filterText.TrimEnd().Length > 0)
                {
                    values = null;
                    return false;
                }

                // And return the values
                values = new float[count];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = vec[i];
                }
                return true;
            }

            values = null;
            return false;
        }

        private static bool ExtractNumber(ref string filterText, out float number)
        {
            var numberMatcher = new System.Text.RegularExpressions.Regex(@"^([+-]?([0-9]+(\.[0-9]*)?)|(\.[0-9]*))");
            var match = numberMatcher.Match(filterText);
            if (match.Success && float.TryParse(match.Value, out number))
            {
                filterText = filterText.Substring(match.Length);
                return true;
            }
            number = 0;
            return false;
        }

        private static bool ExtractComma(ref string filterText)
        {
            var commaMatcher = new System.Text.RegularExpressions.Regex(@"^([ ]*,[ ]*)");
            var match = commaMatcher.Match(filterText);
            if (match.Success)
            {
                filterText = filterText.Substring(match.Length);
                return true;
            }

            return false;
        }

        private static Vector4 ValuesToVector4(float[] values)
        {
            if (values.Length > 4)
            {
                throw new ArgumentException("Too many values");
            }
            Vector4 vector = new Vector4();
            for (int i = 0; i < values.Length; i++)
            {
                vector[i] = values[i];
            }

            return vector;
        }
    }
}
