// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// The particles simulation execution mode.
    /// </summary>
    public enum ParticlesSimulationMode
    {
        /// <summary>
        /// The default model. Select the best simulation mode based on a target platform.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Runs particles simulation on a CPU (always supported).
        /// </summary>
        CPU = 1,

        /// <summary>
        /// Runs particles simulation on a GPU (if supported).
        /// </summary>
        GPU = 2,
    }

    /// <summary>
    /// The particles simulation space modes.
    /// </summary>
    public enum ParticlesSimulationSpace
    {
        /// <summary>
        /// Simulates particles in the world space.
        /// </summary>
        World = 0,

        /// <summary>
        /// Simulates particles in the local space of the actor.
        /// </summary>
        Local = 1,
    }

    /// <summary>
    /// The particles simulation update modes.
    /// </summary>
    public enum ParticlesUpdateMode
    {
        /// <summary>
        /// Use realtime simulation updates. Updates particles during every game logic update.
        /// </summary>
        Realtime = 0,

        /// <summary>
        /// Use fixed timestep delta time to update particles simulation with a custom frequency.
        /// </summary>
        FixedTimestep = 1,
    }

    public sealed partial class ParticleEmitter
    {
    }
}
