// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
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
