////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Specifies the position and manner in which a control is docked
    /// </summary>
    public enum DockStyle
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4,
        Fill = 5,
    }

    /// <summary>
    /// Specifies the location of the anchor point used to position control in the parent container.
    /// </summary>
    public enum AnchorStyle
    {
        /// <summary>
        /// The upper left corner.
        /// </summary>
        UpperLeft = 0,

        /// <summary>
        /// The center of the upper edge.
        /// </summary>
        UpperCenter,

        /// <summary>
        /// The upper right corner.
        /// </summary>
        UpperRight,

        /// <summary>
        /// The center of the left edge.
        /// </summary>
        CenterLeft,

        /// <summary>
        /// The center.
        /// </summary>
        Center,

        /// <summary>
        /// The cenetr of the right edge.
        /// </summary>
        CenetrRight,

        /// <summary>
        /// The bottom left corner.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// The center of the bottom edge.
        /// </summary>
        BottomCenter,

        /// <summary>
        /// The bottom right corner.
        /// </summary>
        BottomRight
    }
    
    /// <summary>
    /// Specifies which scroll bars will be visible on a control
    /// </summary>
    [Flags]
    public enum ScrollBars
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        Both = Horizontal | Vertical
    }

    /// <summary>
    /// Drag item positioning
    /// </summary>
    public enum DragItemPositioning
    {
        None = 0,
        At,
        Above,
        Below
    }

    /// <summary>
    /// Specifies the orientation of controls or elements of controls
    /// </summary>
    public enum Orientation
    {
        Horizontal = 0,
        Vertical
    }
}
