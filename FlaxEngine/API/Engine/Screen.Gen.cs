// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Helper class to access display information.
    /// </summary>
    [Tooltip("Helper class to access display information.")]
    public static unsafe partial class Screen
    {
        /// <summary>
        /// Gets or sets the fullscreen mode.
        /// </summary>
        [Tooltip("The fullscreen mode.")]
        public static bool IsFullscreen
        {
            get { return Internal_GetIsFullscreen(); }
            set { Internal_SetIsFullscreen(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsFullscreen();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsFullscreen(bool value);

        /// <summary>
        /// Gets or sets the window size.
        /// </summary>
        [Tooltip("The window size.")]
        public static Vector2 Size
        {
            get { Internal_GetSize(out var resultAsRef); return resultAsRef; }
            set { Internal_SetSize(ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSize(out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSize(ref Vector2 value);

        /// <summary>
        /// Gets or sets the cursor visible flag.
        /// </summary>
        [Tooltip("The cursor visible flag.")]
        public static bool CursorVisible
        {
            get { return Internal_GetCursorVisible(); }
            set { Internal_SetCursorVisible(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetCursorVisible();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCursorVisible(bool value);

        /// <summary>
        /// Gets or sets the cursor lock mode.
        /// </summary>
        [Tooltip("The cursor lock mode.")]
        public static CursorLockMode CursorLock
        {
            get { return Internal_GetCursorLock(); }
            set { Internal_SetCursorLock(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CursorLockMode Internal_GetCursorLock();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCursorLock(CursorLockMode mode);
    }
}
