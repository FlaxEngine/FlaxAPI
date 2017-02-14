////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
    /// Specifies which scroll bars will be visible on a control
    /// </summary>
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

    /// <summary>
    /// Specifies identifiers to indicate the return value of a dialog box
    /// </summary>
    public enum DialogResult
    {
        Abort = 0,
        Cancel,
        Ignore,
        No,
        None,
        OK,
        Retry,
        Yes
    }
}
