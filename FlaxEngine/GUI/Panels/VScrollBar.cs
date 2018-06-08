// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Vertical scroll bar control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ScrollBar" />
    [HideInEditor]
    public class VScrollBar : ScrollBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VScrollBar"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="height">The height.</param>
        public VScrollBar(float x, float height)
        : base(Orientation.Vertical, x, 0, DefaultSize, height)
        {
            DockStyle = DockStyle.Right;
        }

        /// <inheritdoc />
        protected override float TrackSize => Height;
    }
}
