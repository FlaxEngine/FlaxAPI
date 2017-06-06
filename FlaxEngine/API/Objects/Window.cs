////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a dialog box.
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
    /// Window closing reasons.
    /// </summary>
    public enum ClosingReason
    {
        Unknown = 0,
        User,
        EngineExit,
        CloseEvent
    }

    /// <summary>
    /// Types of default cursors.
    /// </summary>
    public enum CursorType
    {
        Default = 0,
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
    /// Data drag and drop effects.
    /// </summary>
    public enum DragDropEffect
    {
        None = 0,
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
        NoWhere = 0,
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
        CenterParent = 0,

        /// <summary>
        /// The windows is centered on the current display, and has the dimensions specified in the windows's size.
        /// </summary>
        CenterScreen = 1,

        /// <summary>
        /// The position of the form is determined by the Position property.
        /// </summary>
        Manual = 2,
    }

    public partial class Window
    {
        /// <summary>
        /// Gets a value indicating whether this window is in widowed mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is in widowed mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsWidowed => !IsFullscreen;

        /// <summary>
        /// The window GUI root object.
        /// </summary>
        public readonly GUI.Window GUI;

        // Hidden constructor. Object created from C++ side.
        private Window()
        {
            GUI = new GUI.Window(this);
        }
    }
}
