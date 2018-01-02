////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Visject Surface input box element.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.Elements.Box" />
    public class InputBox : Box
    {
        /// <inheritdoc />
        public InputBox(SurfaceNode parentNode, NodeElementArchetype archetype)
            : base(parentNode, archetype, archetype.Position)
        {
        }

        /// <inheritdoc />
        public override bool IsOutput => false;

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Box
            DrawBox();

            // Draw text
            var style = Style.Current;
            var rect = new Rectangle(Width + 4, 0, 1410, Height);
            Render2D.DrawText(style.FontSmall, Archetype.Text, rect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center);
        }
    }
}
