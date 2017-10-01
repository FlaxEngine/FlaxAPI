////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Object hide state description flags. Control object appearance.
    /// </summary>
    [Flags]
    public enum HideFlags
    {
        /// <summary>
        /// The default state.
        /// </summary>
        None = 0,

        /// <summary>
        /// The object will not be visible in the hierarchy.
        /// </summary>
        HideInHierarchy = 1,

        /// <summary>
        /// The object will not be saved.
        /// </summary>
        DontSave = 2,
    }
}
