// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Contains short information about an asset.
    /// </summary>
    [Tooltip("Contains short information about an asset.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct AssetInfo
    {
        /// <summary>
        /// Unique ID.
        /// </summary>
        [Tooltip("Unique ID.")]
        public Guid ID;

        /// <summary>
        /// The stored data full typename. Used to recognize asset type.
        /// </summary>
        [Tooltip("The stored data full typename. Used to recognize asset type.")]
        public string TypeName;

        /// <summary>
        /// Cached path.
        /// </summary>
        [Tooltip("Cached path.")]
        public string Path;
    }
}
