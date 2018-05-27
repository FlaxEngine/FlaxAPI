// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Text drawing element.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.SurfaceNodeElementControl" />
    public sealed class TextView : SurfaceNodeElementControl
    {
        /// <inheritdoc />
        public TextView(SurfaceNode parentNode, NodeElementArchetype archetype)
        : base(parentNode, archetype, 100, 16, false)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            Render2D.DrawText(style.FontSmall, Archetype.Text, new Rectangle(Vector2.Zero, Size), Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center);
        }
    }
}
