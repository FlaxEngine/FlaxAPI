// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Context Menu separator control that visually separate chunks of the popup menu items.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContextMenuItem" />
    [HideInEditor]
    public class ContextMenuSeparator : ContextMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuSeparator"/> class.
        /// </summary>
        /// <param name="parent">The parent context menu.</param>
        public ContextMenuSeparator(ContextMenu parent)
        : base(parent, 8, 4)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Draw separator line
            Render2D.FillRectangle(new Rectangle(0, 1, Width - 4, 1), Style.Current.LightBackground);
        }
    }
}
