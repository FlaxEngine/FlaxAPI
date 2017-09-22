////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FlaxEngine.GUI;

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
        SizeWE,

        /// <summary>
        /// The cursor is hidden.
        /// </summary>
        Hidden
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
        internal static List<Window> Windows = new List<Window>();
        
        /// <summary>
        /// Window closing delegate.
        /// </summary>
        /// <param name="reason">The closing reason.</param>
        /// <param name="cancel">If set to <c>true</c> operation will be cancelled, otherwise window will be closed.</param>
        public delegate void ClosingDelegate(ClosingReason reason, ref bool cancel);

        /// <summary>
        /// Perform window hit test delegate.
        /// </summary>
        /// <param name="mouse">The mouse position.</param>
        /// <returns>Hit result.</returns>
        public delegate WindowHitCodes HitTestDelegate(Vector2 mouse);

        /// <summary>
        /// Perform mouse buttons action.
        /// </summary>
        /// <param name="mouse">The mouse position.</param>
        /// <param name="buttons">The mouse buttons state.</param>
        public delegate void MouseButtonsDelegate(Vector2 mouse, MouseButtons buttons);

        /// <summary>
        /// Perform mouse move action.
        /// </summary>
        /// <param name="mouse">The mouse position.</param>
        public delegate void MouseMoveDelegate(Vector2 mouse);

        /// <summary>
        /// Perform mouse wheel action.
        /// </summary>
        /// <param name="mouse">The mouse position.</param>
        /// <param name="delta">The mouse wheel move delta (can be positive or negative).</param>
        public delegate void MouseWheelDelegate(Vector2 mouse, int delta);

        /// <summary>
        /// Perform keyboard action.
        /// </summary>
        /// <param name="key">The key.</param>
        public delegate void KeyboardDelegate(KeyCode key);

        /// <summary>
        /// Event fired on key pressed.
        /// </summary>
        public event KeyboardDelegate OnKeyDown;

        /// <summary>
        /// Event fired on key released.
        /// </summary>
        public event KeyboardDelegate OnKeyUp;

        /// <summary>
        /// Event fired when mouse goes down.
        /// </summary>
        public event MouseButtonsDelegate OnMouseDown;

        /// <summary>
        /// Event fired when mouse goes up.
        /// </summary>
        public event MouseButtonsDelegate OnMouseUp;

        /// <summary>
        /// Event fired when mouse double clicks.
        /// </summary>
        public event MouseButtonsDelegate OnMouseDoubleClick;

        /// <summary>
        /// Event fired when mouse wheel is scrolling.
        /// </summary>
        public event MouseWheelDelegate OnMouseWheel;

        /// <summary>
        /// Event fired when mouse moves
        /// </summary>
        public event MouseMoveDelegate OnMouseMove;

        /// <summary>
        /// Event fired when mouse leaves window.
        /// </summary>
        public event Action OnMouseLeave;

        /// <summary>
        /// Event fired when window gets focus.
        /// </summary>
        public event Action OnGotFocus;

        /// <summary>
        /// Event fired when window losts focus.
        /// </summary>
        public event Action OnLostFocus;

        /// <summary>
        /// Event fired when window performs hit test, parameter is a mouse position
        /// </summary>
        public HitTestDelegate OnHitTest;

        /// <summary>
        /// Event fired when left mouse button goes down (hit test performed etc.).
        /// Returns true if event has been processed and further actions should be canceled, otherwise false.
        /// </summary>
        public Func<WindowHitCodes, bool> OnLButtonHit;

        /// <summary>
        /// Event fired when windows wants to be closed. Should return true if suspend window closing, otherwise returns false
        /// </summary>
        public event ClosingDelegate OnClosing;

        /// <summary>
        /// Event fired when gets closed and deleted, all references to the window object should be removed at this point.
        /// </summary>
        public event Action OnClosed;

        /// <summary>
        /// Gets a value indicating whether this window is in windowed mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is in windowed mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsWindowed => !IsFullscreen;

        /// <summary>
        /// The window GUI root object.
        /// </summary>
        public readonly GUI.Window GUI;

        // Hidden constructor. Object created from C++ side.
        private Window()
        {
            GUI = new GUI.Window(this);
        }

        /// <summary>
        /// Gets the mouse tracking offset.
        /// </summary>
        [UnmanagedCall]
        public Vector2 TrackingMouseOffset
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get
            {
                Vector2 result;
                Internal_GetTrackingMouseOffset(unmanagedPtr, out result);
                return result;
            }
#endif
        }
        
        /// <summary>
        /// Starts the drag and drop operation.
        /// </summary>
        /// <param name="data">The data.</param>
        public void DoDragDrop(DragData data)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            if (data is DragDataText text)
                Internal_DoDragDropText(unmanagedPtr, text.Text);
            else
                throw new NotImplementedException("Only DragDataText drag and drop is supported.");
#endif
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetTrackingMouseOffset(IntPtr obj, out Vector2 result);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DoDragDropText(IntPtr obj, string text);
#endif

        #endregion

        #region Internal Events

        internal void Internal_OnShow()
        {
            Windows.Add(this);

            GUI.UnlockChildrenRecursive();
            GUI.PerformLayout();
        }

        internal void Internal_OnUpdate(float dt)
        {
            GUI.Update(dt);
        }

        internal void Internal_OnDraw()
        {
            GUI.Draw();
        }

        internal void Internal_OnResize(int width, int height)
        {
            GUI.SetSize(width, height);
        }

        internal void Internal_OnMouseDown(ref Vector2 mousePos, MouseButtons buttons)
        {
            OnMouseDown?.Invoke(mousePos, buttons);
            GUI.OnMouseDown(mousePos, buttons);
        }

        internal void Internal_OnMouseUp(ref Vector2 mousePos, MouseButtons buttons)
        {
            OnMouseUp?.Invoke(mousePos, buttons);
            GUI.OnMouseUp(mousePos, buttons);
        }

        internal void Internal_OnMouseDoubleClick(ref Vector2 mousePos, MouseButtons buttons)
        {
            OnMouseDoubleClick?.Invoke(mousePos, buttons);
            GUI.OnMouseDoubleClick(mousePos, buttons);
        }

        internal void Internal_OnMouseWheel(ref Vector2 mousePos, int delta)
        {
            OnMouseWheel?.Invoke(mousePos, delta);
            GUI.OnMouseWheel(mousePos, delta);
        }

        internal void Internal_OnMouseMove(ref Vector2 mousePos)
        {
            OnMouseMove?.Invoke(mousePos);
            GUI.OnMouseMove(mousePos);
        }

        internal void Internal_OnMouseLeave()
        {
            OnMouseLeave?.Invoke();
            GUI.OnMouseLeave();
        }

        internal void Internal_OnGotFocus()
        {
            OnGotFocus?.Invoke();
            GUI.OnGotFocus();
        }

        internal void Internal_OnLostFocus()
        {
            OnLostFocus?.Invoke();
            GUI.OnLostFocus();
        }

        internal void Internal_OnHitTest(ref Vector2 mousePos, ref WindowHitCodes result, ref bool handled)
        {
            if (OnHitTest != null)
            {
                result = OnHitTest(mousePos);
                handled = true;
            }
        }

        internal void Internal_OnLButtonHit(WindowHitCodes hit, ref bool result)
        {
            if (OnLButtonHit != null)
            {
                result = OnLButtonHit(hit);
            }
        }

        internal DragDropEffect Internal_OnDragEnter(ref Vector2 mousePos, bool isText, string[] data)
        {
            DragData dragData;
            if (isText)
                dragData = new DragDataText(data[0]);
            else
                dragData = new DragDataFiles(data);
            return GUI.OnDragEnter(ref mousePos, dragData);
        }

        internal DragDropEffect Internal_OnDragOver(ref Vector2 mousePos, bool isText, string[] data)
        {
            DragData dragData;
            if (isText)
                dragData = new DragDataText(data[0]);
            else
                dragData = new DragDataFiles(data);
            return GUI.OnDragMove(ref mousePos, dragData);
        }

        internal DragDropEffect Internal_OnDragDrop(ref Vector2 mousePos, bool isText, string[] data)
        {
            DragData dragData;
            if (isText)
                dragData = new DragDataText(data[0]);
            else
                dragData = new DragDataFiles(data);
            return GUI.OnDragDrop(ref mousePos, dragData);
        }

        internal void Internal_OnDragLeave()
        {
            GUI.OnDragLeave();
        }

        internal void Internal_OnClosing(ClosingReason reason, ref bool cancel)
        {
            OnClosing?.Invoke(reason, ref cancel);
        }

        internal void Internal_OnClosed()
        {
            OnClosed?.Invoke();
            
            Windows.Remove(this);

            // Force clear all events (we cannot use window after close)
            OnKeyDown = null;
            OnKeyUp = null;
            OnMouseLeave = null;
            OnMouseDown = null;
            OnMouseUp = null;
            OnMouseDoubleClick = null;
            OnMouseWheel = null;
            OnMouseMove = null;
            OnGotFocus = null;
            OnLostFocus = null;
            OnLButtonHit = null;
            OnHitTest = null;
            OnClosing = null;
            OnClosed = null;
        }

        #endregion
    }
}
