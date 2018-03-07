////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content.GUI
{
    /// <summary>
    /// Content window navigation button.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Button" />
    public class NavigationButton : Button
    {
        /// <summary>
        /// The default margin (horizontal).
        /// </summary>
        public const float DefaultMargin = 6.0f;

        /// <summary>
        /// Gets the target node.
        /// </summary>
        public ContentTreeNode TargetNode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationButton"/> class.
        /// </summary>
        /// <param name="targetNode">The target node.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="height">The height.</param>
        public NavigationButton(ContentTreeNode targetNode, float x, float y, float height)
            : base(x, y, 2 * DefaultMargin)
        {
            TargetNode = targetNode;
            Height = height;
            Text = targetNode.NavButtonLabel + "/";
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            var clientRect = new Rectangle(Vector2.Zero, Size);
            var textRect = new Rectangle(4, 0, clientRect.Width - 4, clientRect.Height);

            // Draw background
            if (_mouseDown)
            {
                Render2D.FillRectangle(clientRect, style.BackgroundSelected);
            }
            else if (IsMouseOver)
            {
                Render2D.FillRectangle(clientRect, style.BackgroundHighlighted);
            }

            // Draw text
            Render2D.DrawText(style.FontMedium, Text, textRect, style.Foreground, TextAlignment.Near, TextAlignment.Center);
        }
		
	    /// <inheritdoc />
	    public override void PerformLayout(bool force = false)
        {
            var style = Style.Current;

            if (style.FontMedium)
            {
                Width = style.FontMedium.MeasureText(Text).X + 2 * DefaultMargin;
            }
        }

        /// <inheritdoc />
        protected override void OnClick()
        {
            // Navigate
            Editor.Instance.Windows.ContentWin.Navigate(TargetNode);

            base.OnClick();
        }
    }
}
