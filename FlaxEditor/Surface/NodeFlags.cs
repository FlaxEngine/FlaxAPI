// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Custom node archetype flags.
    /// </summary>
    [Flags]
    public enum NodeFlags
    {
        /// <summary>
        /// Nothing at all. Nothing but thieves.
        /// </summary>
        None = 0,

        /// <summary>
        /// Don't adds a close button.
        /// </summary>
        NoCloseButton = 1,

        /// <summary>
        /// Node should use dependant and independent boxes types.
        /// </summary>
        UseDependantBoxes = 2,

        /// <summary>
        /// Node cannot be spawned via GUI interface.
        /// </summary>
        NoSpawnViaGUI = 4,

        /// <summary>
        /// Node only for the materials.
        /// </summary>
        MaterialOnly = 8,

        /// <summary>
        /// Node only for the scripts.
        /// </summary>
        VisjectOnly = 16,

        /// <summary>
        /// Disable removing that node from the graph.
        /// </summary>
        NoRemove = 32,

        /// <summary>
        /// Node only for the animation graphs.
        /// </summary>
        AnimGraphOnly = 64,
    }
}
