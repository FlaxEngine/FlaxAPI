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
        [Tooltip("Freeze motion along the X-axis.")]
        LockPositionX = (1 << 0),

        /// <summary>
        /// Freeze motion along the Y-axis.
        /// </summary>
        [Tooltip("Freeze motion along the Y-axis.")]
        LockPositionY = (1 << 1),

        /// <summary>
        /// Freeze motion along the Z-axis.
        /// </summary>
        [Tooltip("Freeze motion along the Z-axis.")]
        LockPositionZ = (1 << 2),

        /// <summary>
        /// Freeze rotation along the X-axis.
        /// </summary>
        [Tooltip("Freeze rotation along the X-axis.")]
        LockRotationX = (1 << 3),

        /// <summary>
        /// Freeze rotation along the Y-axis.
        /// </summary>
        [Tooltip("Freeze rotation along the Y-axis.")]
        LockRotationY = (1 << 4),

        /// <summary>
        /// Freeze rotation along the Z-axis.
        /// </summary>
        [Tooltip("Freeze rotation along the Z-axis.")]
        LockRotationZ = (1 << 5),

        /// <summary>
        /// Freeze motion along all axes.
        /// </summary>
        [Tooltip("Freeze motion along all axes.")]
        LockPosition = LockPositionX | LockPositionY | LockPositionZ,

        /// <summary>
        /// Freeze rotation along all axes.
        /// </summary>
        [Tooltip("Freeze rotation along all axes.")]
        LockRotation = LockRotationX | LockRotationY | LockRotationZ,

        /// <summary>
        /// Freeze rotation and motion along all axes.
        /// </summary>
        [Tooltip("Freeze rotation and motion along all axes.")]
        LockAll = LockPosition | LockRotation,
    }
}
