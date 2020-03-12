// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// UI control anchors presets.
    /// </summary>
    public enum AnchorPresets
    {
        /// <summary>
        /// The empty preset.
        /// </summary>
        Custom,

        /// <summary>
        /// The top left corner of the parent control.
        /// </summary>
        TopLeft,

        /// <summary>
        /// The center of the top edge of the parent control.
        /// </summary>
        TopCenter,

        /// <summary>
        /// The top right corner of the parent control.
        /// </summary>
        TopRight,

        /// <summary>
        /// The middle of the left edge of the parent control.
        /// </summary>
        MiddleLeft,

        /// <summary>
        /// The middle center! Right in the middle of the parent control.
        /// </summary>
        MiddleCenter,

        /// <summary>
        /// The middle of the right edge of the parent control.
        /// </summary>
        MiddleRight,

        /// <summary>
        /// The bottom left corner of the parent control.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// The center of the bottom edge of the parent control.
        /// </summary>
        BottomCenter,

        /// <summary>
        /// The bottom right corner of the parent control.
        /// </summary>
        BottomRight,

        VerticalStretchLeft,
        VerticalStretchRight,
        VerticalStretchCenter,

        HorizontalStretchTop,
        HorizontalStretchMiddle,
        HorizontalStretchBottom,

        /// <summary>
        /// All parent control edges.
        /// </summary>
        StretchAll,
    }

    /// <summary>
    /// Specifies which scroll bars will be visible on a control
    /// </summary>
    [Flags]
    public enum ScrollBars
    {
        /// <summary>
        /// Don't use scroll bars.
        /// </summary>
        [Tooltip("Don't use scroll bars.")]
        None = 0,

        /// <summary>
        /// Use horizontal scrollbar.
        /// </summary>
        [Tooltip("Use horizontal scrollbar.")]
        Horizontal = 1,

        /// <summary>
        /// Use vertical scrollbar.
        /// </summary>
        [Tooltip("Use vertical scrollbar.")]
        Vertical = 2,

        /// <summary>
        /// Use horizontal and vertical scrollbar.
        /// </summary>
        [Tooltip("Use horizontal and vertical scrollbar.")]
        Both = Horizontal | Vertical
    }

    /// <summary>
    /// The drag item positioning modes.
    /// </summary>
    public enum DragItemPositioning
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// At the item.
        /// </summary>
        At,

        /// <summary>
        /// Above the item (near the upper/left edge).
        /// </summary>
        Above,

        /// <summary>
        /// Below the item (near the bottom/right edge)
        /// </summary>
        Below
    }

    /// <summary>
    /// Specifies the orientation of controls or elements of controls
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// The horizontal.
        /// </summary>
        Horizontal = 0,

        /// <summary>
        /// The vertical.
        /// </summary>
        Vertical = 1,
    }
}
