// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline current position tracking control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class PositionHandle : ContainerControl
    {
        private Timeline _timeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionHandle"/> class.
        /// </summary>
        /// <param name="timeline">The timeline.</param>
        public PositionHandle(Timeline timeline)
        {
            _timeline = timeline;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var icon = Editor.Instance.Icons.VisjectArrowClose;

            Matrix3x3.RotationZ(Mathf.PiOverTwo, out var t);
            Render2D.PushTransform(ref t);
            Render2D.DrawSprite(icon, new Rectangle(new Vector2(4, -Width), Size));
            Render2D.PopTransform();

            Render2D.FillRectangle(new Rectangle(Width * 0.5f, Height, 1, _timeline.MediaPanel.Height), style.Foreground.RGBMultiplied(0.8f));

            base.Draw();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _timeline = null;

            base.Dispose();
        }
    }
}
