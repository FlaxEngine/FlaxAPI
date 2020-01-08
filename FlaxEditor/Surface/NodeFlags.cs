// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

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
        /// Node can be used in the material graphs.
        /// </summary>
        MaterialGraph = 8,

        /// <summary>
        /// Node can be used in the particle emitter graphs.
        /// </summary>
        ParticleEmitterGraph = 16,

        /// <summary>
        /// Disables removing that node from the graph.
        /// </summary>
        NoRemove = 32,

        /// <summary>
        /// Node can be used in the animation graphs.
        /// </summary>
        AnimGraph = 64,

        /// <summary>
        /// Disables moving node (by user).
        /// </summary>
        NoMove = 128,

        /// <summary>
        /// Node can be used in the all visual graphs.
        /// </summary>
        AllGraphs = MaterialGraph | ParticleEmitterGraph | AnimGraph,
    }
}
