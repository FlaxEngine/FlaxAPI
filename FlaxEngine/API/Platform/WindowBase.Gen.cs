// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Window closing reasons.
    /// </summary>
    [Tooltip("Window closing reasons.")]
    public enum ClosingReason
    {
        /// <summary>
        /// The unknown.
        /// </summary>
        [Tooltip("The unknown.")]
        Unknown = 0,

        /// <summary>
        /// The user.
        /// </summary>
        [Tooltip("The user.")]
        User,

        /// <summary>
        /// The engine exit.
        /// </summary>
        [Tooltip("The engine exit.")]
        EngineExit,

        /// <summary>
        /// The close event.
        /// </summary>
        [Tooltip("The close event.")]
        CloseEvent,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Types of default cursors.
    /// </summary>
    [Tooltip("Types of default cursors.")]
    public enum CursorType
    {
        /// <summary>
        /// The default.
        /// </summary>
        [Tooltip("The default.")]
        Default = 0,

        /// <summary>
        /// The cross.
        /// </summary>
        [Tooltip("The cross.")]
        Cross,

        /// <summary>
        /// The hand.
        /// </summary>
        [Tooltip("The hand.")]
        Hand,

        /// <summary>
        /// The help icon
        /// </summary>
        [Tooltip("The help icon")]
        Help,

        /// <summary>
        /// The I beam.
        /// </summary>
        [Tooltip("The I beam.")]
        IBeam,

        /// <summary>
        /// The blocking image.
        /// </summary>
        [Tooltip("The blocking image.")]
        No,

        /// <summary>
        /// The wait.
        /// </summary>
        [Tooltip("The wait.")]
        Wait,

        /// <summary>
        /// The size all sides.
        /// </summary>
        [Tooltip("The size all sides.")]
        SizeAll,

        /// <summary>
        /// The size NE-SW.
        /// </summary>
        [Tooltip("The size NE-SW.")]
        SizeNESW,

        /// <summary>
        /// The size NS.
        /// </summary>
        [Tooltip("The size NS.")]
        SizeNS,

        /// <summary>
        /// The size NW-SE.
        /// </summary>
        [Tooltip("The size NW-SE.")]
        SizeNWSE,

        /// <summary>
        /// The size WE.
        /// </summary>
        [Tooltip("The size WE.")]
        SizeWE,

        /// <summary>
        /// The cursor is hidden.
        /// </summary>
        [Tooltip("The cursor is hidden.")]
        Hidden,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Data drag and drop effects.
    /// </summary>
    [Tooltip("Data drag and drop effects.")]
    public enum DragDropEffect
    {
        /// <summary>
        /// The none.
        /// </summary>
        [Tooltip("The none.")]
        None = 0,

        /// <summary>
        /// The copy.
        /// </summary>
        [Tooltip("The copy.")]
        Copy,

        /// <summary>
        /// The move.
        /// </summary>
        [Tooltip("The move.")]
        Move,

        /// <summary>
        /// The link.
        /// </summary>
        [Tooltip("The link.")]
        Link,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Window hit test codes. Note: they are 1:1 mapping for Win32 values.
    /// </summary>
    [Tooltip("Window hit test codes. Note: they are 1:1 mapping for Win32 values.")]
    public enum WindowHitCodes
    {
        /// <summary>
        /// The transparent area.
        /// </summary>
        [Tooltip("The transparent area.")]
        Transparent = -1,

        /// <summary>
        /// The no hit.
        /// </summary>
        [Tooltip("The no hit.")]
        NoWhere = 0,

        /// <summary>
        /// The client area.
        /// </summary>
        [Tooltip("The client area.")]
        Client = 1,

        /// <summary>
        /// The caption area.
        /// </summary>
        [Tooltip("The caption area.")]
        Caption = 2,

        /// <summary>
        /// The system menu.
        /// </summary>
        [Tooltip("The system menu.")]
        SystemMenu = 3,

        /// <summary>
        /// The grow box
        /// </summary>
        [Tooltip("The grow box")]
        GrowBox = 4,

        /// <summary>
        /// The menu.
        /// </summary>
        [Tooltip("The menu.")]
        Menu = 5,

        /// <summary>
        /// The horizontal scroll.
        /// </summary>
        [Tooltip("The horizontal scroll.")]
        HScroll = 6,

        /// <summary>
        /// The vertical scroll.
        /// </summary>
        [Tooltip("The vertical scroll.")]
        VScroll = 7,

        /// <summary>
        /// The minimize button.
        /// </summary>
        [Tooltip("The minimize button.")]
        MinButton = 8,

        /// <summary>
        /// The maximize button.
        /// </summary>
        [Tooltip("The maximize button.")]
        MaxButton = 9,

        /// <summary>
        /// The left side;
        /// </summary>
        [Tooltip("The left side;")]
        Left = 10,

        /// <summary>
        /// The right side.
        /// </summary>
        [Tooltip("The right side.")]
        Right = 11,

        /// <summary>
        /// The top side.
        /// </summary>
        [Tooltip("The top side.")]
        Top = 12,

        /// <summary>
        /// The top left corner.
        /// </summary>
        [Tooltip("The top left corner.")]
        TopLeft = 13,

        /// <summary>
        /// The top right corner.
        /// </summary>
        [Tooltip("The top right corner.")]
        TopRight = 14,

        /// <summary>
        /// The bottom side.
        /// </summary>
        [Tooltip("The bottom side.")]
        Bottom = 15,

        /// <summary>
        /// The bottom left corner.
        /// </summary>
        [Tooltip("The bottom left corner.")]
        BottomLeft = 16,

        /// <summary>
        /// The bottom right corner.
        /// </summary>
        [Tooltip("The bottom right corner.")]
        BottomRight = 17,

        /// <summary>
        /// The border.
        /// </summary>
        [Tooltip("The border.")]
        Border = 18,

        /// <summary>
        /// The object.
        /// </summary>
        [Tooltip("The object.")]
        Object = 19,

        /// <summary>
        /// The close button.
        /// </summary>
        [Tooltip("The close button.")]
        Close = 20,

        /// <summary>
        /// The help button.
        /// </summary>
        [Tooltip("The help button.")]
        Help = 21,
    }
}
