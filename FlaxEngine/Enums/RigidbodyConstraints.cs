// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Dynamic rigidbodies movement and rotation locking flags. Provide a mechanism to lock motion along/around a specific axis or set of axes to constrain object motion.
    /// </summary>
    [Flags]
    public enum RigidbodyConstraints
    {
        /// <summary>
        /// No constraints.
        /// </summary>
        None = 0,

        /// <summary>
        /// Freeze motion along the X-axis.
        /// </summary>
        LockPositionX = (1 << 0),

        /// <summary>
        /// Freeze motion along the Y-axis.
        /// </summary>
        LockPositionY = (1 << 1),

        /// <summary>
        /// Freeze motion along the Z-axis.
        /// </summary>
        LockPositionZ = (1 << 2),

        /// <summary>
        /// Freeze rotation along the X-axis.
        /// </summary>
        LockRotationX = (1 << 3),

        /// <summary>
        /// Freeze rotation along the Y-axis.
        /// </summary>
        LockRotationY = (1 << 4),

        /// <summary>
        /// Freeze rotation along the Z-axis.
        /// </summary>
        LockRotationZ = (1 << 5),

        /// <summary>
        /// Freeze motion along all axes.
        /// </summary>
        LockPosition = LockPositionX | LockPositionY | LockPositionZ,

        /// <summary>
        /// Freeze rotation along all axes.
        /// </summary>
        LockRotation = LockRotationX | LockRotationY | LockRotationZ,

        /// <summary>
        /// Freeze rotation and motion along all axes.
        /// </summary>
        LockAll = LockPosition | LockRotation,
    }
}
