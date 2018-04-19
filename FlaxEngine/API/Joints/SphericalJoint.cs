// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Flags that control spherical joint options.
    /// </summary>
    [Flags]
    public enum SphericalJointFlag
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The joint cone range limit is enabled.
        /// </summary>
        Limit = 0x1
    }

    public sealed partial class SphericalJoint
    {
    }
}
