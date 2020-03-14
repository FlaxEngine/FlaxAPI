// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The features exposed for a particular format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FormatFeatures
    {
        /// <summary>
        /// The format.
        /// </summary>
        public PixelFormat Format;

        /// <summary>
        /// Gets the maximum MSAA sample count for a particular <see cref="PixelFormat"/>.
        /// </summary>
        public MSAALevel MSAALevelMax;

        /// <summary>
        /// Support of a given format on the installed video device.
        /// </summary>
        public FormatSupport Support;

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Format: {0}, MSAALevelMax: {1}, FormatSupport: 0x{2:x}", Format, MSAALevelMax, (int)Support);
        }
    }
}
