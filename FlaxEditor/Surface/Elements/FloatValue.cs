// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Floating point value editing element.
    /// </summary>
    /// <seealso cref="FloatValueBox" />
    /// <seealso cref="ISurfaceNodeElement" />
    public sealed class FloatValue : FloatValueBox, ISurfaceNodeElement
    {
        /// <inheritdoc />
        public SurfaceNode ParentNode { get; }

        /// <inheritdoc />
        public NodeElementArchetype Archetype { get; }

        /// <summary>
        /// Gets the surface.
        /// </summary>
        public VisjectSurface Surface => ParentNode.Surface;

        /// <inheritdoc />
        public FloatValue(SurfaceNode parentNode, NodeElementArchetype archetype)
            : base(Get(parentNode, archetype), archetype.Position.X, archetype.Position.Y, 50, -1000000, 1000000, 0.01f)
        {
            ParentNode = parentNode;
            Archetype = archetype;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Draw border
            if (!IsFocused)
                Render2D.DrawRectangle(new Rectangle(Vector2.Zero, Size), Style.Current.BorderNormal);
        }

        /// <inheritdoc />
        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            Set(ParentNode, Archetype, Value);
        }

        public static float Get(SurfaceNode parentNode, NodeElementArchetype arch)
        {
            if (arch.ValueIndex < 0)
                return 0;

            float result;
            var value = parentNode.Values[arch.ValueIndex];

            // Note: this value box may edit on component of the vector like Vector3.Y, BoxID from Archetype tells which component pick

            if (value is int valueInt)
            {
                result = (float)valueInt;
            }
            else if (value is float valueFloat)
            {
                result = valueFloat;
            }
            else if (value is Vector2 valueVec2)
            {
                result = (arch.BoxID == 0 ? valueVec2.X : valueVec2.Y);
            }
            else if (value is Vector3 valueVec3)
            {
                result = (arch.BoxID == 0 ? valueVec3.X : arch.BoxID == 1 ? valueVec3.Y : valueVec3.Z);
            }
            else if (value is Vector4 valueVec4)
            {
                result = (arch.BoxID == 0 ? valueVec4.X : arch.BoxID == 1 ? valueVec4.Y : arch.BoxID == 2 ? valueVec4.Z : valueVec4.W);
            }
            else
            {
                result = 0;
            }

            return result;
        }

	    public static void Set(SurfaceNode parentNode, NodeElementArchetype arch, float toSet)
        {
            if (arch.ValueIndex < 0)
                return;

            var value = parentNode.Values[arch.ValueIndex];

            if (value is int)
            {
                value = (int)toSet;
            }
            else if (value is float)
            {
                value = toSet;
            }
            else if (value is Vector2 valueVec2)
            {
                if (arch.BoxID == 0)
                    valueVec2.X = toSet;
                else
                    valueVec2.Y = toSet;
                value = valueVec2;
            }
            else if (value is Vector3 valueVec3)
            {
                if (arch.BoxID == 0)
                    valueVec3.X = toSet;
                else if (arch.BoxID == 1)
                    valueVec3.Y = toSet;
                else
                    valueVec3.Z = toSet;
                value = valueVec3;
            }
            else if (value is Vector4 valueVec4)
            {
                if (arch.BoxID == 0)
                    valueVec4.X = toSet;
                else if (arch.BoxID == 1)
                    valueVec4.Y = toSet;
                else if (arch.BoxID == 2)
                    valueVec4.Z = toSet;
                else
                    valueVec4.W = toSet;
                value = valueVec4;
            }
            else
            {
                value = 0;
            }

            parentNode.SetValue(arch.ValueIndex, value);
        }
    }
}
