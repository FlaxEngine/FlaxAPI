// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The platform the game is running.
    /// </summary>
    [Tooltip("The platform the game is running.")]
    public enum PlatformType
    {
        /// <summary>
        /// Running on Windows.
        /// </summary>
        [Tooltip("Running on Windows.")]
        Windows = 1,

        /// <summary>
        /// Running on Xbox One.
        /// </summary>
        [Tooltip("Running on Xbox One.")]
        XboxOne = 2,

        /// <summary>
        /// Running Windows Store App (Universal Windows Platform).
        /// </summary>
        [Tooltip("Running Windows Store App (Universal Windows Platform).")]
        WindowsStore = 3,

        /// <summary>
        /// Running on Linux system.
        /// </summary>
        [Tooltip("Running on Linux system.")]
        Linux = 4,

        /// <summary>
        /// Running on PlayStation 4.
        /// </summary>
        [Tooltip("Running on PlayStation 4.")]
        PS4 = 5,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The platform architecture types.
    /// </summary>
    [Tooltip("The platform architecture types.")]
    public enum ArchitectureType
    {
        /// <summary>
        /// Anything or not important.
        /// </summary>
        [Tooltip("Anything or not important.")]
        AnyCPU = 0,

        /// <summary>
        /// 32-bit.
        /// </summary>
        [Tooltip("32-bit.")]
        x86 = 1,

        /// <summary>
        /// 64-bit.
        /// </summary>
        [Tooltip("64-bit.")]
        x64 = 2,

        /// <summary>
        /// The ARM 32-bit.
        /// </summary>
        [Tooltip("The ARM 32-bit.")]
        ARM = 3,

        /// <summary>
        /// The ARM 64-bit.
        /// </summary>
        [Tooltip("The ARM 64-bit.")]
        ARM64 = 4,
    }
}
