// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Enumeration that determines the way in which two material properties will be combined to yield a friction or restitution coefficient for a collision.
    /// </summary>
    /// <remarks>
    /// Physics doesn't have any inherent combinations because the coefficients are determined empirically on a case by case basis.
    /// However, simulating this with a pairwise lookup table is often impractical.
    /// The effective combine mode for the pair is maximum(material0.combineMode, material1.combineMode).
    /// </remarks>
    public enum PhysicsCombineMode
    {
        /// <summary>
        /// Uses the average value of the touching materials: (a+b)/2.
        /// </summary>
        [Tooltip("Uses the average value of the touching materials: (a+b)/2.")]
        Average = 0,

        /// <summary>
        /// Uses the smaller value of the touching materials: min(a,b).
        /// </summary>
        [Tooltip("Uses the smaller value of the touching materials: min(a,b).")]
        Minimum = 1,

        /// <summary>
        /// Multiplies the values of the touching materials: a*b.
        /// </summary>
        [Tooltip("Multiplies the values of the touching materials: a*b.")]
        Multiply = 2,

        /// <summary>
        /// Uses the larger value of the touching materials: max(a, b).
        /// </summary>
        [Tooltip("Uses the larger value of the touching materials: max(a, b).")]
        Maximum = 3,
    }
}
