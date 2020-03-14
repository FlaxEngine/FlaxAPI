// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// A navigation bar control. Shows the current location path with UI buttons to navigate around.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Panel" />
    public class NavigationBar : Panel
    {
        /// <summary>
        /// The default buttons margin.
        /// </summary>
        public const float DefaultButtonsMargin = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBar"/> class.
        /// </summary>
        public NavigationBar()
        : base(ScrollBars.Horizontal)
        {
            Height = 32;
        }

        /// <inheritdoc />
        protected override void Arrange()
        {
            // Arrange buttons
            float x = DefaultButtonsMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                child.X = x;
                x += child.Width + DefaultButtonsMargin;
            }
        }

        /// <summary>
        /// Updates the bar bounds and positions it after the last toolstrip button. Ensures to fit the toolstrip area (navigation bar horizontal scroll bar can be used to view the full path).
        /// </summary>
        /// <param name="toolstrip">The toolstrip.</param>
        public void UpdateBounds(ToolStrip toolstrip)
        {
            if (toolstrip == null)
                return;

            var lastToolstripButton = toolstrip.LastButton;
            var bounds = new Rectangle(
                new Vector2(lastToolstripButton.Right + 8.0f, 0),
                new Vector2(Parent.Width - X - 8.0f, Height)
            );
            Bounds = bounds;
        }
    }
}
