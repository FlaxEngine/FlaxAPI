// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Describes how a mapped GPU resource will be accessed.
    /// </summary>
    [Flags]
    public enum GPUResourceMapMode
    {
        /// <summary>
        /// The resource is mapped for reading.
        /// </summary>
        Read = 0x01,

        /// <summary>
        /// The resource is mapped for writing.
        /// </summary>
        Write = 0x02,

        /// <summary>
        /// The resource is mapped for reading and writing.
        /// </summary>
        ReadWrite = Read | Write,
    }
}
