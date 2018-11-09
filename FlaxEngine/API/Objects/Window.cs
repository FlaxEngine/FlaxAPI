// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FlaxEngine.GUI;

namespace FlaxEngine
{
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
        public delegate WindowHitCodes HitTestDelegate(ref Vector2 mouse);

        /// <summary>
        /// Perform mouse buttons action.
        /// </summary>
        /// <param name="mouse">The mouse position.</param>
        /// <param name="buttons">The mouse buttons state.</param>
        /// <param name="handled">The flag that indicated that event has been handled by the custom code and should not be passed further. By default it is set to false.</param>
        public delegate void MouseButtonDelegate(ref Vector2 mouse, MouseButton buttons, ref bool handled);

        /// <summary>
        /// Perform mouse move action.
        /// </summary>
        /// <param name="mouse">The mouse position.</param>
        public delegate void MouseMoveDelegate(ref Vector2 mouse);

        /// <summary>
        /// Perform mouse wheel action.
        /// </summary>
        /// <param name="mouse">The mouse position.</param>
        /// <param name="delta">The mouse wheel move delta (can be positive or negative; normalized to [-1;1] range).</param>
        /// <param name="handled">The flag that indicated that event has been handled by the custom code and should not be passed further. By default it is set to false.</param>
        public delegate void MouseWheelDelegate(ref Vector2 mouse, float delta, ref bool handled);

        /// <summary>
        /// Perform input character action.
        /// </summary>
        /// <param name="c">The input character.</param>
        public delegate void CharDelegate(char c);

        /// <summary>
        /// Perform keyboard action.
        /// </summary>
        /// <param name="key">The key.</param>
        public delegate void KeyboardDelegate(Keys key);

        /// <summary>
        /// Event fired on character input.
        /// </summary>
        public event CharDelegate OnCharInput;

        /// <summary>
        /// Event fired on key pressed.
        /// </summary>
        public event KeyboardDelegate KeyDown;

        /// <summary>
        /// Event fired on key released.
        /// </summary>
        public event KeyboardDelegate KeyUp;

        /// <summary>
        /// Event fired when mouse goes down.
        /// </summary>
        public event MouseButtonDelegate MouseDown;

        /// <summary>
        /// Event fired when mouse goes up.
        /// </summary>
        public event MouseButtonDelegate MouseUp;

        /// <summary>
        /// Event fired when mouse double clicks.
        /// </summary>
        public event MouseButtonDelegate MouseDoubleClick;

        /// <summary>
        /// Event fired when mouse wheel is scrolling.
        /// </summary>
        public event MouseWheelDelegate MouseWheel;

        /// <summary>
        /// Event fired when mouse moves
        /// </summary>
        public event MouseMoveDelegate MouseMove;

        /// <summary>
        /// Event fired when mouse leaves window.
        /// </summary>
        public event Action MouseLeave;

        /// <summary>
        /// Event fired when window gets focus.
        /// </summary>
        public event Action GotFocus;

        /// <summary>
        /// Event fired when window lost focus.
        /// </summary>
        public event Action LostFocus;

        /// <summary>
        /// Event fired when window performs hit test, parameter is a mouse position
        /// </summary>
        public HitTestDelegate HitTest;

        /// <summary>
        /// Event fired when left mouse button goes down (hit test performed etc.).
        /// Returns true if event has been processed and further actions should be canceled, otherwise false.
        /// </summary>
        public Func<WindowHitCodes, bool> LeftButtonHit;

        /// <summary>
        /// Event fired when windows wants to be closed. Should return true if suspend window closing, otherwise returns false
        /// </summary>
        public event ClosingDelegate Closing;

        /// <summary>
        /// Event fired when gets closed and deleted, all references to the window object should be removed at this point.
        /// </summary>
        public event Action Closed;

        /// <summary>
        /// Gets a value indicating whether this window is in windowed mode.
        /// </summary>
        public bool IsWindowed => !IsFullscreen;

        /// <summary>
        /// The window GUI root object.
        /// </summary>
        public readonly WindowRootControl GUI;

        // Hidden constructor. Object created from C++ side.
        private Window()
        {
            GUI = new WindowRootControl(this);
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
            GUI.Size = new Vector2(width, height);
        }

        internal void Internal_OnCharInput(char c)
        {
            OnCharInput?.Invoke(c);
            GUI.OnCharInput(c);
        }

        internal void Internal_OnKeyDown(Keys key)
        {
            KeyDown?.Invoke(key);
            GUI.OnKeyDown(key);
        }

        internal void Internal_OnKeyUp(Keys key)
        {
            KeyUp?.Invoke(key);
            GUI.OnKeyUp(key);
        }

        internal void Internal_OnMouseDown(ref Vector2 mousePos, MouseButton buttons)
        {
            bool handled = false;
            MouseDown?.Invoke(ref mousePos, buttons, ref handled);
            if (handled)
                return;

            GUI.OnMouseDown(mousePos, buttons);
        }

        internal void Internal_OnMouseUp(ref Vector2 mousePos, MouseButton buttons)
        {
            bool handled = false;
            MouseUp?.Invoke(ref mousePos, buttons, ref handled);
            if (handled)
                return;

            GUI.OnMouseUp(mousePos, buttons);
        }

        internal void Internal_OnMouseDoubleClick(ref Vector2 mousePos, MouseButton buttons)
        {
            bool handled = false;
            MouseDoubleClick?.Invoke(ref mousePos, buttons, ref handled);
            if (handled)
                return;

            GUI.OnMouseDoubleClick(mousePos, buttons);
        }

        internal void Internal_OnMouseWheel(ref Vector2 mousePos, float delta)
        {
            bool handled = false;
            MouseWheel?.Invoke(ref mousePos, delta, ref handled);
            if (handled)
                return;

            GUI.OnMouseWheel(mousePos, delta);
        }

        internal void Internal_OnMouseMove(ref Vector2 mousePos)
        {
            MouseMove?.Invoke(ref mousePos);
            GUI.OnMouseMove(mousePos);
        }

        internal void Internal_OnMouseLeave()
        {
            MouseLeave?.Invoke();
            GUI.OnMouseLeave();
        }

        internal void Internal_OnGotFocus()
        {
            GotFocus?.Invoke();
            GUI.OnGotFocus();
        }

        internal void Internal_OnLostFocus()
        {
            LostFocus?.Invoke();
            GUI.OnLostFocus();
        }

        internal void Internal_OnHitTest(ref Vector2 mousePos, ref WindowHitCodes result, ref bool handled)
        {
            if (HitTest != null)
            {
                result = HitTest(ref mousePos);
                handled = true;
            }
        }

        internal void Internal_OnLButtonHit(WindowHitCodes hit, ref bool result)
        {
            if (LeftButtonHit != null)
            {
                result = LeftButtonHit(hit);
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
            Closing?.Invoke(reason, ref cancel);
        }

        internal void Internal_OnClosed()
        {
            Closed?.Invoke();

            GUI.Dispose();

            Windows.Remove(this);

            // Force clear all events (we cannot use window after close)
            KeyDown = null;
            KeyUp = null;
            MouseLeave = null;
            MouseDown = null;
            MouseUp = null;
            MouseDoubleClick = null;
            MouseWheel = null;
            MouseMove = null;
            GotFocus = null;
            LostFocus = null;
            LeftButtonHit = null;
            HitTest = null;
            Closing = null;
            Closed = null;
        }

        #endregion
    }
}
