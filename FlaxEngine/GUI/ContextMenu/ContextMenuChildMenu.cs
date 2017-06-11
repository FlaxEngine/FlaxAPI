////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Context Menu control that cn be expanded to the child popup menu.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContextMenuItem" />
    public class ContextMenuChildMenu : ContextMenuItem
    {
        /// <summary>
        /// The item text.
        /// </summary>
        public string Text;

        /// <summary>
        /// The child context menu.
        /// </summary>
        public readonly ContextMenu ContextMenu = new ContextMenu();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuChildMenu"/> class.
        /// </summary>
        /// <param name="parent">The parent cotext menu.</param>
        /// <param name="text">The text.</param>
        public ContextMenuChildMenu(ContextMenu parent, string text)
            : base(parent, false, 8, 2)
        {
            Text = text;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            var backgroundRect = new Rectangle(-X + 3, 0, Parent.Width - 6, Height);
            var clientRect = new Rectangle(0, 0, Width, Height);
            bool isCMopened = ContextMenu.IsOpened;

            // Draw background
            if (isCMopened || (IsMouseOver && Enabled))
                Render2D.FillRectangle(backgroundRect, style.LightBackground);

            // Draw text
            Render2D.DrawText(style.FontMedium, Text, clientRect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center);

            // Draw arrow
            //if (ContextMenu.HasChildren)
            //    Render2D.DrawSprite(style.ArrowRight, new Rectangle(Width - 15, (Height - 12) / 2, 12, 12), Enabled ? isCMopened ? style.BackgroundSelected : Color.White : style.ForegroundDisabled);
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            base.OnMouseEnter(location);

            // Skip if has no children
            if (ContextMenu.HasChildren == false)
                return;

            // Skip if already shown
            var parentContextMenu = ParentContextMenu;
            if (parentContextMenu == ContextMenu)
                return;

            // Hide parent CM popups and set itself as child
            parentContextMenu.ShowChild(ContextMenu, new Vector2(Right + ContextMenuBase.DefaultItemsMargin - 1, Y));
        }

        /// <inheritdoc />
        public override void PerformLayout()
        {
            var style = Style.Current;
            float width = 16;

            if (style.FontMedium)
                width += style.FontMedium.MeasureText(Text).X;

            Width = width;
        }
    }
}
