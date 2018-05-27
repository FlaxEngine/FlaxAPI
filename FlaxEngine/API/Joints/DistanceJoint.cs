// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Controls distance joint options.
    /// </summary>
    [Flags]
    public enum DistanceJointFlag
    {
        /// <summary>
        /// The none limits.
        /// </summary>
        None = 0,

        /// <summary>
        /// The minimum distance limit.
        /// </summary>
        MinDistance = 0x1,

        /// <summary>
        /// Uses the maximum distance limit.
        /// </summary>
        MaxDistance = 0x2,

        /// <summary>
        /// Uses the spring when maintaining limits
        /// </summary>
        Spring = 0x4
    }

    public sealed partial class DistanceJoint
    {
    }
}
