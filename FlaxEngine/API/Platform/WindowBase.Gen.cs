// This code was auto-generated. Do not modify it.

using System;
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

namespace FlaxEngine
{
    /// <summary>
    /// Native platform window object.
    /// </summary>
    [Tooltip("Native platform window object.")]
    public unsafe partial class Window : FlaxEngine.Object
    {
        /// <summary>
        /// Gets or sets a value that indicates whether a window is in a fullscreen mode.
        /// </summary>
        [Tooltip("Gets a value that indicates whether a window is in a fullscreen mode.")]
        public bool IsFullscreen
        {
            get { return Internal_IsFullscreen(unmanagedPtr); }
            set { Internal_SetIsFullscreen(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsFullscreen(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsFullscreen(IntPtr obj, bool isFullscreen);

        /// <summary>
        /// Gets a value that indicates whether a window is not in a fullscreen mode.
        /// </summary>
        [Tooltip("Gets a value that indicates whether a window is not in a fullscreen mode.")]
        public bool IsWindowed
        {
            get { return Internal_IsWindowed(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsWindowed(IntPtr obj);

        /// <summary>
        /// Gets or sets a value that indicates whether a window is visible (hidden or shown).
        /// </summary>
        [Tooltip("Gets a value that indicates whether a window is visible (hidden or shown).")]
        public bool IsVisible
        {
            get { return Internal_IsVisible(unmanagedPtr); }
            set { Internal_SetIsVisible(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsVisible(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsVisible(IntPtr obj, bool isVisible);

        /// <summary>
        /// Gets a value that indicates whether a window is minimized.
        /// </summary>
        [Tooltip("Gets a value that indicates whether a window is minimized.")]
        public bool IsMinimized
        {
            get { return Internal_IsMinimized(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsMinimized(IntPtr obj);

        /// <summary>
        /// Gets a value that indicates whether a window is maximized.
        /// </summary>
        [Tooltip("Gets a value that indicates whether a window is maximized.")]
        public bool IsMaximized
        {
            get { return Internal_IsMaximized(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsMaximized(IntPtr obj);

        /// <summary>
        /// Gets the native window handle.
        /// </summary>
        [Tooltip("The native window handle.")]
        public IntPtr NativePtr
        {
            get { return Internal_GetNativePtr(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IntPtr Internal_GetNativePtr(IntPtr obj);

        /// <summary>
        /// Checks if window is closed.
        /// </summary>
        [Tooltip("Checks if window is closed.")]
        public bool IsClosed
        {
            get { return Internal_IsClosed(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsClosed(IntPtr obj);

        /// <summary>
        /// Checks if window is foreground (the window with which the user is currently working).
        /// </summary>
        [Tooltip("Checks if window is foreground (the window with which the user is currently working).")]
        public bool IsForegroundWindow
        {
            get { return Internal_IsForegroundWindow(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsForegroundWindow(IntPtr obj);

        /// <summary>
        /// Gets or sets the client bounds of the window (client area not including border).
        /// </summary>
        [Tooltip("The client bounds of the window (client area not including border).")]
        public Rectangle ClientBounds
        {
            get { Internal_GetClientBounds(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetClientBounds(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetClientBounds(IntPtr obj, out Rectangle resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetClientBounds(IntPtr obj, ref Rectangle clientArea);

        /// <summary>
        /// Gets or sets the window position (in screen coordinates).
        /// </summary>
        [Tooltip("The window position (in screen coordinates).")]
        public Vector2 Position
        {
            get { Internal_GetPosition(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetPosition(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPosition(IntPtr obj, out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPosition(IntPtr obj, ref Vector2 position);

        /// <summary>
        /// Gets or sets the client position of the window (client area not including border).
        /// </summary>
        [Tooltip("The client position of the window (client area not including border).")]
        public Vector2 ClientPosition
        {
            get { Internal_GetClientPosition(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetClientPosition(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetClientPosition(IntPtr obj, out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetClientPosition(IntPtr obj, ref Vector2 position);

        /// <summary>
        /// Gets the window size (including border).
        /// </summary>
        [Tooltip("The window size (including border).")]
        public Vector2 Size
        {
            get { Internal_GetSize(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSize(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets or sets the size of the client area of the window (not including border).
        /// </summary>
        [Tooltip("The size of the client area of the window (not including border).")]
        public Vector2 ClientSize
        {
            get { Internal_GetClientSize(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetClientSize(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetClientSize(IntPtr obj, out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetClientSize(IntPtr obj, ref Vector2 size);

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        [Tooltip("The window title.")]
        public string Title
        {
            get { return Internal_GetTitle(unmanagedPtr); }
            set { Internal_SetTitle(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetTitle(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTitle(IntPtr obj, string title);

        /// <summary>
        /// Gets or sets window opacity value (valid only for windows created with SupportsTransparency flag). Opacity values are normalized to range [0;1].
        /// </summary>
        [Tooltip("Gets window opacity value (valid only for windows created with SupportsTransparency flag). Opacity values are normalized to range [0;1].")]
        public float Opacity
        {
            get { return Internal_GetOpacity(unmanagedPtr); }
            set { Internal_SetOpacity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetOpacity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOpacity(IntPtr obj, float opacity);

        /// <summary>
        /// Determines whether this window is focused.
        /// </summary>
        [Tooltip("Determines whether this window is focused.")]
        public bool IsFocused
        {
            get { return Internal_IsFocused(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsFocused(IntPtr obj);

        /// <summary>
        /// Gets the mouse tracking offset.
        /// </summary>
        [Tooltip("The mouse tracking offset.")]
        public Vector2 TrackingMouseOffset
        {
            get { Internal_GetTrackingMouseOffset(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetTrackingMouseOffset(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets or sets the mouse cursor.
        /// </summary>
        [Tooltip("The mouse cursor.")]
        public CursorType Cursor
        {
            get { return Internal_GetCursor(unmanagedPtr); }
            set { Internal_SetCursor(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CursorType Internal_GetCursor(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCursor(IntPtr obj, CursorType type);

        /// <summary>
        /// Gets or sets the value indicating whenever rendering to this window enabled.
        /// </summary>
        [Tooltip("The value indicating whenever rendering to this window enabled.")]
        public bool RenderingEnabled
        {
            get { return Internal_GetRenderingEnabled(unmanagedPtr); }
            set { Internal_SetRenderingEnabled(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetRenderingEnabled(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRenderingEnabled(IntPtr obj, bool value);

        /// <summary>
        /// Gets the text entered during the current frame (Unicode).
        /// </summary>
        [Tooltip("The text entered during the current frame (Unicode).")]
        public string InputText
        {
            get { return Internal_GetInputText(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetInputText(IntPtr obj);

        /// <summary>
        /// Gets or sets the mouse position in window coordinates.
        /// </summary>
        [Tooltip("The mouse position in window coordinates.")]
        public Vector2 MousePosition
        {
            get { Internal_GetMousePosition(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetMousePosition(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMousePosition(IntPtr obj, out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMousePosition(IntPtr obj, ref Vector2 position);

        /// <summary>
        /// Gets the mouse position change during the last frame.
        /// </summary>
        [Tooltip("The mouse position change during the last frame.")]
        public Vector2 MousePositionDelta
        {
            get { Internal_GetMousePositionDelta(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMousePositionDelta(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets the mouse wheel change during the last frame.
        /// </summary>
        [Tooltip("The mouse wheel change during the last frame.")]
        public float MouseScrollDelta
        {
            get { return Internal_GetMouseScrollDelta(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMouseScrollDelta(IntPtr obj);

        /// <summary>
        /// Shows the window.
        /// </summary>
        public void Show()
        {
            Internal_Show(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Show(IntPtr obj);

        /// <summary>
        /// Hides the window.
        /// </summary>
        public void Hide()
        {
            Internal_Hide(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Hide(IntPtr obj);

        /// <summary>
        /// Minimizes the window.
        /// </summary>
        public void Minimize()
        {
            Internal_Minimize(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Minimize(IntPtr obj);

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        public void Maximize()
        {
            Internal_Maximize(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Maximize(IntPtr obj);

        /// <summary>
        /// Restores the window state before minimizing or maximizing.
        /// </summary>
        public void Restore()
        {
            Internal_Restore(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Restore(IntPtr obj);

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="reason">The closing reason.</param>
        public void Close(ClosingReason reason = ClosingReason.CloseEvent)
        {
            Internal_Close(unmanagedPtr, reason);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Close(IntPtr obj, ClosingReason reason);

        /// <summary>
        /// Converts screen space location into window space coordinates.
        /// </summary>
        /// <param name="screenPos">The screen position.</param>
        /// <returns>The client space position.</returns>
        public Vector2 ScreenToClient(Vector2 screenPos)
        {
            Internal_ScreenToClient(unmanagedPtr, ref screenPos, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ScreenToClient(IntPtr obj, ref Vector2 screenPos, out Vector2 resultAsRef);

        /// <summary>
        /// Converts window space location into screen space coordinates.
        /// </summary>
        /// <param name="clientPos">The client position.</param>
        /// <returns>The screen space position.</returns>
        public Vector2 ClientToScreen(Vector2 clientPos)
        {
            Internal_ClientToScreen(unmanagedPtr, ref clientPos, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClientToScreen(IntPtr obj, ref Vector2 clientPos, out Vector2 resultAsRef);

        /// <summary>
        /// Focuses this window.
        /// </summary>
        public void Focus()
        {
            Internal_Focus(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Focus(IntPtr obj);

        /// <summary>
        /// Brings window to the front of the Z order.
        /// </summary>
        /// <param name="force">True if move to the front by force, otherwise false.</param>
        public void BringToFront(bool force = false)
        {
            Internal_BringToFront(unmanagedPtr, force);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_BringToFront(IntPtr obj, bool force);

        /// <summary>
        /// Flashes the window to bring use attention.
        /// </summary>
        public void FlashWindow()
        {
            Internal_FlashWindow(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_FlashWindow(IntPtr obj);

        /// <summary>
        /// Starts drag and drop operation
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The result.</returns>
        public DragDropEffect DoDragDrop(string data)
        {
            return Internal_DoDragDrop(unmanagedPtr, data);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DragDropEffect Internal_DoDragDrop(IntPtr obj, string data);

        /// <summary>
        /// Starts the mouse tracking.
        /// </summary>
        /// <param name="useMouseScreenOffset">If set to <c>true</c> will use mouse screen offset.</param>
        public void StartTrackingMouse(bool useMouseScreenOffset)
        {
            Internal_StartTrackingMouse(unmanagedPtr, useMouseScreenOffset);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_StartTrackingMouse(IntPtr obj, bool useMouseScreenOffset);

        /// <summary>
        /// Ends the mouse tracking.
        /// </summary>
        public void EndTrackingMouse()
        {
            Internal_EndTrackingMouse(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_EndTrackingMouse(IntPtr obj);

        /// <summary>
        /// Gets the key state (true if key is being pressed during this frame).
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True while the user holds down the key identified by id</returns>
        public bool GetKey(KeyboardKeys key)
        {
            return Internal_GetKey(unmanagedPtr, key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKey(IntPtr obj, KeyboardKeys key);

        /// <summary>
        /// Gets the key 'down' state (true if key was pressed in this frame).
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True during the frame the user starts pressing down the key</returns>
        public bool GetKeyDown(KeyboardKeys key)
        {
            return Internal_GetKeyDown(unmanagedPtr, key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyDown(IntPtr obj, KeyboardKeys key);

        /// <summary>
        /// Gets the key 'up' state (true if key was released in this frame).
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True during the frame the user releases the key</returns>
        public bool GetKeyUp(KeyboardKeys key)
        {
            return Internal_GetKeyUp(unmanagedPtr, key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyUp(IntPtr obj, KeyboardKeys key);

        /// <summary>
        /// Gets the mouse button state.
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True while the user holds down the button</returns>
        public bool GetMouseButton(MouseButton button)
        {
            return Internal_GetMouseButton(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButton(IntPtr obj, MouseButton button);

        /// <summary>
        /// Gets the mouse button down state.
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True during the frame the user starts pressing down the button</returns>
        public bool GetMouseButtonDown(MouseButton button)
        {
            return Internal_GetMouseButtonDown(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButtonDown(IntPtr obj, MouseButton button);

        /// <summary>
        /// Gets the mouse button up state.
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True during the frame the user releases the button</returns>
        public bool GetMouseButtonUp(MouseButton button)
        {
            return Internal_GetMouseButtonUp(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButtonUp(IntPtr obj, MouseButton button);
    }
}
