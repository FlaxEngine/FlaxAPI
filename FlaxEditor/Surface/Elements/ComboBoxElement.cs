// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Combo box element.
    /// </summary>
    /// <seealso cref="ComboBox" />
    /// <seealso cref="ISurfaceNodeElement" />
    public class ComboBoxElement : ComboBox, ISurfaceNodeElement
    {
        /// <summary>
        /// True if automatic value selecting is active.
        /// </summary>
        protected bool _isAutoSelect;

        /// <inheritdoc />
        public SurfaceNode ParentNode { get; }

        /// <inheritdoc />
        public NodeElementArchetype Archetype { get; }

        /// <summary>
        /// Gets the surface.
        /// </summary>
        public VisjectSurface Surface => ParentNode.Surface;

        /// <inheritdoc />
        public ComboBoxElement(SurfaceNode parentNode, NodeElementArchetype archetype)
        : base(archetype.ActualPositionX, archetype.ActualPositionY, archetype.Size.X)
        {
            ParentNode = parentNode;
            Archetype = archetype;

            // Check if combo box will use auto select feature
            // Note: used provided items we should auto update saved node value on closed
            _isAutoSelect = Archetype.Text != null;
            if (Archetype.Text != null)
            {
                // Get the fucking items xD
                var items = Archetype.Text.Split('\n');
                AddItems(items);

                // Select saved value
                _selectedIndices.Clear();
                _selectedIndices.Add((int)ParentNode.Values[Archetype.ValueIndex]);
            }
        }

        /// <inheritdoc />
        protected override void OnSelectedIndexChanged()
        {
            if (_isAutoSelect)
            {
                // Edit value
                ParentNode.SetValue(Archetype.ValueIndex, SelectedIndex);
            }

            base.OnSelectedIndexChanged();
        }
    }
}
