// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Contains a contact point data for the collision location.
    /// </summary>
    [Tooltip("Contains a contact point data for the collision location.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct ContactPoint
    {
        /// <summary>
        /// The contact point location in the world space.
        /// </summary>
        [Tooltip("The contact point location in the world space.")]
        public Vector3 Point;

        /// <summary>
        /// The separation value (negative implies penetration).
        /// </summary>
        [Tooltip("The separation value (negative implies penetration).")]
        public float Separation;

        /// <summary>
        /// The contact normal.
        /// </summary>
        [Tooltip("The contact normal.")]
        public Vector3 Normal;
    }
}
