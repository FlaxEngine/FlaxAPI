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

    /// <summary>
    /// Window closing reasons
    /// </summary>
    public enum ClosingReason
    {
        Unknown,
        User,
        EngineExit,
        CloseEvent
    }

    /// <summary>
    /// Types of default cursors
    /// </summary>
    public enum CursorType
    {
        Default,
        Cross,
        Hand,
        Help,
        IBeam,
        No,
        Wait,
        SizeAll,
        SizeNESW,
        SizeNS,
        SizeNWSE,
        SizeWE
    }

    /// <summary>
    /// Data drag & drop effects
    /// </summary>
    public enum DragDropEffect
    {
        None,
        Copy,
        Move,
        Link
    }

    /// <summary>
    /// Window hit test codes. Note: they are 1:1 mapping for Win32 values.
    /// </summary>
    public enum WindowHitCodes
    {
        Transparent = -1,
        Nowhere = 0,
        Client = 1,
        Caption = 2,
        SystemMenu = 3,
        GrowBox = 4,
        Size = GrowBox,
        Menu = 5,
        HScrol = 6,
        VScroll = 7,
        MinButton = 8,
        MaxButton = 9,
        Left = 10,
        Right = 11,
        Top = 12,
        TopLeft = 13,
        TopRight = 14,
        Bottom = 15,
        BottomLeft = 16,
        BottomRight = 17,
        Border = 18,
        Reduce = MinButton,
        Zoom = MaxButton,
        SizeFirst = Left,
        SizeLast = BottomRight,
        Object = 19,
        Close = 20,
        Help = 21
    }

    /// <summary>
    /// Specifies the initial position of a window.
    /// </summary>
    public enum WindowStartPosition
    {
        /// <summary>
        /// The window is centered within the bounds of its parent window or center screen if has no parent window specified.
        /// </summary>
        CenterParent,
	
        /// <summary>
        /// The windows is centered on the current display, and has the dimensions specified in the windows's size.
        /// </summary>
        CenterScreen,
	
        /// <summary>
        /// The position of the form is determined by the Position property.
        /// </summary>
        Manual,
    }
}
