////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Viewport.Widgets
{
    public enum ViewportWidgetLocation
    {
        UpperLeft,
        UpperRight,
    }

    /// <summary>
    /// Viewport Widgets Container control
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class ViewportWidgetsContainer : ContainerControl
    {
        public const float WidgetsMargin = 4;
        public const float WidgetsHeight = 18;
        public const float WidgetsIconSize = 16;

        public ViewportWidgetLocation WidgetLocation { get; }

        public ViewportWidgetsContainer(ViewportWidgetLocation location)
            : base(false, 0, WidgetsMargin, 64, WidgetsHeight + 2)
        {
            WidgetLocation = location;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            var clientRect = new Rectangle(Vector2.Zero, Size);

            // Draw background
            Render2D.FillRectangle(clientRect, style.LightBackground * (IsMouseOver ? 1.0f : 0.4f), true);

            base.Draw();

            // Draw frame
            Render2D.DrawRectangle(clientRect, style.BackgroundSelected * (IsMouseOver ? 1.0f : 0.4f), true);
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            base.OnChildResized(control);

            PerformLayout();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            float x = 1;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                var w = c.Width;

                c.Bounds = new Rectangle(x, 1, w, Height - 2);

                x += w;
            }

            Width = x + 1;
        }
    }
}
