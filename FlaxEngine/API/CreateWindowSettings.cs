// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Settings for new window creation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CreateWindowSettings
    {
        /// <summary>
        /// The native parent window pointer.
        /// </summary>
        public IntPtr ParentPtr;

        /// <summary>
        /// The parent window.
        /// </summary>
        public Window Parent
        {
            set => ParentPtr = Object.GetUnmanagedPtr(value);
        }

        /// <summary>
        /// The title.
        /// </summary>
        public string Title;

        /// <summary>
        /// The position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The size.
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// The minimum size.
        /// </summary>
        public Vector2 MinimumSize;

        /// <summary>
        /// The maximum size.
        /// </summary>
        public Vector2 MaximumSize;

        /// <summary>
        /// The start position.
        /// </summary>
        public WindowStartPosition StartPosition;

        /// <summary>
        /// True if show window fullscreen on show.
        /// </summary>
        public bool Fullscreen;

        /// <summary>
        /// Enable/disable window border.
        /// </summary>
        public bool HasBorder;

        /// <summary>
        /// Enable/disable window transparency support. Required to change window opacity property.
        /// </summary>
        public bool SupportsTransparency;

        /// <summary>
        /// True if show window on taskbar, otherwise it will be hidden.
        /// </summary>
        public bool ShowInTaskbar;

        /// <summary>
        /// Auto activate window after show.
        /// </summary>
        public bool ActivateWhenFirstShown;

        /// <summary>
        /// Allow window to capture input.
        /// </summary>
        public bool AllowInput;

        /// <summary>
        /// Allow window minimize action.
        /// </summary>
        public bool AllowMinimize;

        /// <summary>
        /// Allow window maximize action.
        /// </summary>
        public bool AllowMaximize;

        /// <summary>
        /// Enable/disable drag and drop actions over the window.
        /// </summary>
        public bool AllowDragAndDrop;

        /// <summary>
        /// True if window topmost.
        /// </summary>
        public bool IsTopmost;

        /// <summary>
        /// True if it's a regular window.
        /// </summary>
        public bool IsRegularWindow;

        /// <summary>
        /// Enable/disable window sizing frame.
        /// </summary>
        public bool HasSizingFrame;

        /// <summary>
        /// Enable/disable window auto-show after the first paint.
        /// </summary>
        public bool ShowAfterFirstPaint;

        /// <summary>
        /// Gets the default settings.
        /// </summary>
        /// <value>
        /// The default settings.
        /// </value>
        public static CreateWindowSettings Default => new CreateWindowSettings
        {
            Position = new Vector2(100, 100),
            Size = new Vector2(640, 480),
            MinimumSize = Vector2.One,
            MaximumSize = new Vector2(4100, 4100),
            StartPosition = WindowStartPosition.CenterParent,
            HasBorder = true,
            ShowInTaskbar = true,
            ActivateWhenFirstShown = true,
            AllowInput = true,
            AllowMinimize = true,
            AllowMaximize = true,
            AllowDragAndDrop = true,
            IsRegularWindow = true,
            HasSizingFrame = true,
        };
    }
}
