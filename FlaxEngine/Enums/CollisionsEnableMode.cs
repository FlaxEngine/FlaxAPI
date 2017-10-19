////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    /// <summary>
    /// Physical shape collisions detection and handling modes.
    /// </summary>
    public enum CollisionsEnableMode
    {
        /// <summary>
        /// Will not create any representation in the physics engine.
        /// </summary>
        /// <remarks>
        /// Cannot be used for spatial queries (raycasts, sweeps, overlaps) or simulation (rigid body, constraints).
        /// Best performance possible (especially for moving objects).
        /// </remarks>
        NoCollision = 0,

        /// <summary>
        /// Only used for spatial queries (raycasts, sweeps, and overlaps).
        /// </summary>
        /// <remarks>
        /// Cannot be used for simulation (rigid body, constraints).
        /// Useful for character movement and things that do not need physical simulation.
        /// Performance gains by keeping data out of simulation tree.
        /// </remarks>
        QueryOnly = 1,

        /// <summary>
        /// Only used only for physics simulation (rigid body, constraints).
        /// </summary>
        /// <remarks>
        /// Cannot be used for spatial queries (raycasts, sweeps, overlaps).
        /// Useful for jiggly bits on characters that do not need per bone detection.
        /// Performance gains by keeping data out of query tree.
        /// </remarks>
        PhysicsOnly = 2,

        /// <summary>
        /// Can be used for both spatial queries (raycasts, sweeps, overlaps) and simulation (rigid body, constraints).
        /// </summary>
        /// <remarks>
        /// The default option for the most cases.
        /// </remarks>
        QueryAndPhysics = 3,
    }
}
