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
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The top edge.
        /// </summary>
        Top = 1,

        /// <summary>
        /// The bottom edge.
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// The left edge.
        /// </summary>
        Left = 3,

        /// <summary>
        /// The right edge
        /// </summary>
        Right = 4,
        
        /// <summary>
        /// The whole area.
        /// </summary>
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
        /// <summary>
        /// Don't use scroll bars.
        /// </summary>
        None = 0,

        /// <summary>
        /// Use horizontal scrolllbar.
        /// </summary>
        Horizontal = 1,

        /// <summary>
        /// Use vertical scrolllbar.
        /// </summary>
        Vertical = 2,

        /// <summary>
        /// Use horizontal and vertical scrolllbar.
        /// </summary>
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
