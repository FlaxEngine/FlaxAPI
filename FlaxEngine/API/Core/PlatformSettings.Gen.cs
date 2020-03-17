// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies the display mode of a game window.
    /// </summary>
    [Tooltip("Specifies the display mode of a game window.")]
    public enum GameWindowMode
    {
        /// <summary>
        /// The window has borders and does not take up the full screen.
        /// </summary>
        [Tooltip("The window has borders and does not take up the full screen.")]
        Windowed = 0,

        /// <summary>
        /// The window takes up the full screen exclusively.
        /// </summary>
        [Tooltip("The window takes up the full screen exclusively.")]
        Fullscreen = 1,

        /// <summary>
        /// The window behaves like in Windowed mode but has no borders.
        /// </summary>
        [Tooltip("The window behaves like in Windowed mode but has no borders.")]
        Borderless = 2,

        /// <summary>
        /// Same as in Borderless, but is of the size of the screen.
        /// </summary>
        [Tooltip("Same as in Borderless, but is of the size of the screen.")]
        FullscreenBorderless = 3
,
    }
}
