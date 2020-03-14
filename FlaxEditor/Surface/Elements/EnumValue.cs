// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Combo box for enum element.
    /// </summary>
    /// <seealso cref="EnumComboBox" />
    /// <seealso cref="ISurfaceNodeElement" />
    public class EnumValue : EnumComboBox, ISurfaceNodeElement
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
        public EnumValue(SurfaceNode parentNode, NodeElementArchetype archetype)
        : base(Utilities.Utils.GetType(archetype.Text))
        {
            X = archetype.ActualPositionX;
            Y = archetype.ActualPositionY;
            Width = archetype.Size.X;
            ParentNode = parentNode;
            Archetype = archetype;
            Value = (int)ParentNode.Values[Archetype.ValueIndex];
        }

        /// <inheritdoc />
        protected override void OnValueChanged()
        {
            if ((int)ParentNode.Values[Archetype.ValueIndex] != Value)
            {
                // Edit value
                ParentNode.SetValue(Archetype.ValueIndex, Value);
            }

            base.OnValueChanged();
        }
    }
}
