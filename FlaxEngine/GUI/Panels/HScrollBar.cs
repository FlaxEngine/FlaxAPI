// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Horizontal scroll bar control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ScrollBar" />
    [HideInEditor]
    public class HScrollBar : ScrollBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        /// <param name="parent">The parent control.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        public HScrollBar(ContainerControl parent, float y, float width)
        : base(Orientation.Horizontal)
        {
            AnchorPreset = AnchorPresets.HorizontalStretchBottom;
            Parent = parent;
            Bounds = new Rectangle(0, y, width, DefaultSize);
        }

        /// <inheritdoc />
        protected override float TrackSize => Width;
    }
}
