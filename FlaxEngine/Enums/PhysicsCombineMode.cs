////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    /// <summary>
    /// Physical material properties combine modes.
    /// </summary>
    public enum PhysicsCombineMode
    {
        /// <summary>
        /// Uses the average value of the touching materials: (a+b)/2.
        /// </summary>
        Average = 0,

        /// <summary>
        /// Uses the smaller value of the touching materials: min(a,b)
        /// </summary>
        Mininum = 1,

        /// <summary>
        /// Multiplies the values of the touching materials: a*b
        /// </summary>
        Multiply = 2,

        /// <summary>
        /// Uses the larger value of the touching materials: max(a, b)
        /// </summary>
        Maximum = 3,
    }
}
