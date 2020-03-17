// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// General identifiers for potential force feedback channels. These will be mapped according to the platform specific implementation.
    /// </summary>
    [Tooltip("General identifiers for potential force feedback channels. These will be mapped according to the platform specific implementation.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct GamepadVibrationState
    {
        /// <summary>
        /// The left large motor vibration.
        /// </summary>
        [Tooltip("The left large motor vibration.")]
        public float LeftLarge;

        /// <summary>
        /// The left small motor vibration.
        /// </summary>
        [Tooltip("The left small motor vibration.")]
        public float LeftSmall;

        /// <summary>
        /// The right large motor vibration.
        /// </summary>
        [Tooltip("The right large motor vibration.")]
        public float RightLarge;

        /// <summary>
        /// The right small motor vibration.
        /// </summary>
        [Tooltip("The right small motor vibration.")]
        public float RightSmall;
    }
}
