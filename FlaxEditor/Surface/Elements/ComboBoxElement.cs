////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Combo box element.
    /// </summary>
    /// <seealso cref="ComboBox" />
    /// <seealso cref="ISurfaceNodeElement" />
    public sealed class ComboBoxElement : ComboBox, ISurfaceNodeElement
    {
        private bool _isAutoSelect;

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
                _selectedIndicies.Clear();
                _selectedIndicies.Add((int)ParentNode.Values[Archetype.ValueIndex]);
            }
        }

        /// <inheritdoc />
        protected override void OnSelectedIndexChanged()
        {
            if (_isAutoSelect)
            {
                // Edit value
                ParentNode.Values[Archetype.ValueIndex] = SelectedIndex;
                ParentNode.Surface.MarkAsEdited();
            }

            base.OnSelectedIndexChanged();
        }
    }
}
