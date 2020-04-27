// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies the initial position of a window.
    /// </summary>
    [Tooltip("Specifies the initial position of a window.")]
    public enum WindowStartPosition
    {
        /// <summary>
        /// The window is centered within the bounds of its parent window or center screen if has no parent window specified.
        /// </summary>
        [Tooltip("The window is centered within the bounds of its parent window or center screen if has no parent window specified.")]
        CenterParent,

        /// <summary>
        /// The windows is centered on the current display, and has the dimensions specified in the windows's size.
        /// </summary>
        [Tooltip("The windows is centered on the current display, and has the dimensions specified in the windows's size.")]
        CenterScreen,

        /// <summary>
        /// The position of the form is determined by the Position property.
        /// </summary>
        [Tooltip("The position of the form is determined by the Position property.")]
        Manual,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Settings for new window.
    /// </summary>
    [Tooltip("Settings for new window.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct CreateWindowSettings
    {
        /// <summary>
        /// The native parent window pointer.
        /// </summary>
        [Tooltip("The native parent window pointer.")]
        public Window Parent;

        /// <summary>
        /// The title.
        /// </summary>
        [Tooltip("The title.")]
        public string Title;

        /// <summary>
        /// The custom start position.
        /// </summary>
        [Tooltip("The custom start position.")]
        public Vector2 Position;

        /// <summary>
        /// The client size.
        /// </summary>
        [Tooltip("The client size.")]
        public Vector2 Size;

        /// <summary>
        /// The minimum size.
        /// </summary>
        [Tooltip("The minimum size.")]
        public Vector2 MinimumSize;

        /// <summary>
        /// The maximum size.
        /// </summary>
        [Tooltip("The maximum size.")]
        public Vector2 MaximumSize;

        /// <summary>
        /// The start position mode.
        /// </summary>
        [Tooltip("The start position mode.")]
        public WindowStartPosition StartPosition;

        /// <summary>
        /// True if show window fullscreen on show.
        /// </summary>
        [Tooltip("True if show window fullscreen on show.")]
        public bool Fullscreen;

        /// <summary>
        /// Enable/disable window border.
        /// </summary>
        [Tooltip("Enable/disable window border.")]
        public bool HasBorder;

        /// <summary>
        /// Enable/disable window transparency support. Required to change window opacity property.
        /// </summary>
        [Tooltip("Enable/disable window transparency support. Required to change window opacity property.")]
        public bool SupportsTransparency;

        /// <summary>
        /// True if show window on taskbar, otherwise it will be hidden.
        /// </summary>
        [Tooltip("True if show window on taskbar, otherwise it will be hidden.")]
        public bool ShowInTaskbar;

        /// <summary>
        /// Auto activate window after show.
        /// </summary>
        [Tooltip("Auto activate window after show.")]
        public bool ActivateWhenFirstShown;

        /// <summary>
        /// Allow window to capture input.
        /// </summary>
        [Tooltip("Allow window to capture input.")]
        public bool AllowInput;

        /// <summary>
        /// Allow window minimize action.
        /// </summary>
        [Tooltip("Allow window minimize action.")]
        public bool AllowMinimize;

        /// <summary>
        /// Allow window maximize action.
        /// </summary>
        [Tooltip("Allow window maximize action.")]
        public bool AllowMaximize;

        /// <summary>
        /// Enable/disable drag and drop actions over the window.
        /// </summary>
        [Tooltip("Enable/disable drag and drop actions over the window.")]
        public bool AllowDragAndDrop;

        /// <summary>
        /// True if window topmost.
        /// </summary>
        [Tooltip("True if window topmost.")]
        public bool IsTopmost;

        /// <summary>
        /// True if it's a regular window.
        /// </summary>
        [Tooltip("True if it's a regular window.")]
        public bool IsRegularWindow;

        /// <summary>
        /// Enable/disable window sizing frame.
        /// </summary>
        [Tooltip("Enable/disable window sizing frame.")]
        public bool HasSizingFrame;

        /// <summary>
        /// Enable/disable window auto-show after the first paint.
        /// </summary>
        [Tooltip("Enable/disable window auto-show after the first paint.")]
        public bool ShowAfterFirstPaint;

        /// <summary>
        /// The custom data (platform dependant).
        /// </summary>
        [Tooltip("The custom data (platform dependant).")]
        public IntPtr Data;
    }
}
