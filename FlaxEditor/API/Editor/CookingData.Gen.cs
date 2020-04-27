// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Game building options. Used as flags.
    /// </summary>
    [Flags]
    [Tooltip("Game building options. Used as flags.")]
    public enum BuildOptions
    {
        /// <summary>
        /// No special options declared.
        /// </summary>
        [Tooltip("No special options declared.")]
        None = 0,

        /// <summary>
        /// Shows the output directory folder on building end.
        /// </summary>
        [Tooltip("Shows the output directory folder on building end.")]
        ShowOutput = 1 << 0,
    }
}

namespace FlaxEditor
{
    /// <summary>
    /// Game build target platform.
    /// </summary>
    [Tooltip("Game build target platform.")]
    public enum BuildPlatform
    {
        /// <summary>
        /// Windows (32-bit architecture)
        /// </summary>
        [EditorDisplay(null, "Windows 32bit")]
        [Tooltip("Windows (32-bit architecture)")]
        Windows32 = 1,

        /// <summary>
        /// Windows (64-bit architecture)
        /// </summary>
        [EditorDisplay(null, "Windows 64bit")]
        [Tooltip("Windows (64-bit architecture)")]
        Windows64 = 2,

        /// <summary>
        /// Universal Windows Platform (UWP) (x86 architecture)
        /// </summary>
        [EditorDisplay(null, "Windows Store x86")]
        [Tooltip("Universal Windows Platform (UWP) (x86 architecture)")]
        WindowsStoreX86 = 3,

        /// <summary>
        /// Universal Windows Platform (UWP) (x64 architecture)
        /// </summary>
        [EditorDisplay(null, "Windows Store x64")]
        [Tooltip("Universal Windows Platform (UWP) (x64 architecture)")]
        WindowsStoreX64 = 4,

        /// <summary>
        /// Xbox One
        /// </summary>
        [EditorDisplay(null, "Xbox One")]
        [Tooltip("Xbox One")]
        XboxOne = 5,

        /// <summary>
        /// Linux (64-bit architecture)
        /// </summary>
        [EditorDisplay(null, "Linux x64")]
        [Tooltip("Linux (64-bit architecture)")]
        LinuxX64 = 6,

        /// <summary>
        /// PlayStation 4
        /// </summary>
        [EditorDisplay(null, "PlayStation 4")]
        [Tooltip("PlayStation 4")]
        PS4 = 7,
    }
}

namespace FlaxEditor
{
    /// <summary>
    /// Game build configuration modes.
    /// </summary>
    [Tooltip("Game build configuration modes.")]
    public enum BuildConfiguration
    {
        /// <summary>
        /// Debug configuration. Without optimizations but with full debugging information.
        /// </summary>
        [Tooltip("Debug configuration. Without optimizations but with full debugging information.")]
        Debug = 0,

        /// <summary>
        /// Development configuration. With basic optimizations and partial debugging data.
        /// </summary>
        [Tooltip("Development configuration. With basic optimizations and partial debugging data.")]
        Development = 1,

        /// <summary>
        /// Shipping configuration. With full optimization and no debugging data.
        /// </summary>
        [Tooltip("Shipping configuration. With full optimization and no debugging data.")]
        Release = 2,
    }
}
