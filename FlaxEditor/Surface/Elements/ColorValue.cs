// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Color value picking element.
    /// </summary>
    /// <seealso cref="ColorValueBox" />
    /// <seealso cref="ISurfaceNodeElement" />
    public sealed class ColorValue : ColorValueBox, ISurfaceNodeElement
    {
        /// <inheritdoc />
        public SurfaceNode ParentNode { get; }

        /// <inheritdoc />
        public NodeElementArchetype Archetype { get; }

        /// <summary>
        /// Gets the surface.
        /// </summary>
        /// <value>
        /// The surface.
        /// </value>
        public VisjectSurface Surface => ParentNode.Surface;

        /// <inheritdoc />
        public ColorValue(SurfaceNode parentNode, NodeElementArchetype archetype)
        : base(Get(parentNode, archetype), archetype.Position.X, archetype.Position.Y)
        {
            ParentNode = parentNode;
            Archetype = archetype;
        }

        /// <inheritdoc />
        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            Set(ParentNode, Archetype, ref _value);
        }

        private static Color Get(SurfaceNode parentNode, NodeElementArchetype arch)
        {
            if (arch.ValueIndex < 0)
                return Color.White;

            Color result;
            var value = parentNode.Values[arch.ValueIndex];

            if (value is Color valueColor)
            {
                result = valueColor;
            }
            else if (value is Vector3 valueVec3)
            {
                result = valueVec3;
            }
            else if (value is Vector4 valueVec4)
            {
                result = valueVec4;
            }
            else
            {
                result = Color.White;
            }

            return result;
        }

        private static void Set(SurfaceNode parentNode, NodeElementArchetype arch, ref Color toSet)
        {
            if (arch.ValueIndex < 0)
                return;

            var value = parentNode.Values[arch.ValueIndex];

            if (value is Color)
            {
                value = toSet;
            }
            else if (value is Vector3)
            {
                value = (Vector3)toSet;
            }
            else if (value is Vector4)
            {
                value = (Vector4)toSet;
            }

            parentNode.SetValue(arch.ValueIndex, value);
        }
    }
}
