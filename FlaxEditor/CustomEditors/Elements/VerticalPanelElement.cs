////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The vertical panel element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class VerticalPanelElement : LayoutElementsContainer
    {
        /// <summary>
        /// The panel.
        /// </summary>
        public readonly VerticalPanel Panel = new VerticalPanel();

        /// <summary>
        /// Initializes the element.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Init(string text)
        {
            Panel.Name = text;
        }

        /// <inheritdoc />
        public override ContainerControl ContainerControl => Panel;
    }
}
