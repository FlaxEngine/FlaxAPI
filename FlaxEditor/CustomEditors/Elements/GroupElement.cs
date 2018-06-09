// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The layout group element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class GroupElement : LayoutElementsContainer
    {
        /// <summary>
        /// The drop panel.
        /// </summary>
        public readonly DropPanel Panel = new DropPanel();

        /// <summary>
        /// Initializes the element.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Init(string text)
        {
            Panel.HeaderText = text;
        }

        /// <inheritdoc />
        public override ContainerControl ContainerControl => Panel;
    }
}
