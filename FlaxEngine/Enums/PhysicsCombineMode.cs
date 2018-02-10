////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        Average = 0,

        /// <summary>
        /// Uses the smaller value of the touching materials: min(a,b).
        /// </summary>
        Mininum = 1,

        /// <summary>
        /// Multiplies the values of the touching materials: a*b.
        /// </summary>
        Multiply = 2,

        /// <summary>
        /// Uses the larger value of the touching materials: max(a, b).
        /// </summary>
        Maximum = 3,
    }
}
