// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The particles simulation execution mode.
    /// </summary>
    [Tooltip("The particles simulation execution mode.")]
    public enum ParticlesSimulationMode
    {
        /// <summary>
        /// The default model. Select the best simulation mode based on a target platform.
        /// </summary>
        [Tooltip("The default model. Select the best simulation mode based on a target platform.")]
        Default = 0,

        /// <summary>
        /// Runs particles simulation on a CPU (always supported).
        /// </summary>
        [Tooltip("Runs particles simulation on a CPU (always supported).")]
        CPU = 1,

        /// <summary>
        /// Runs particles simulation on a GPU (if supported).
        /// </summary>
        [Tooltip("Runs particles simulation on a GPU (if supported).")]
        GPU = 2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The particles simulation space modes.
    /// </summary>
    [Tooltip("The particles simulation space modes.")]
    public enum ParticlesSimulationSpace
    {
        /// <summary>
        /// Simulates particles in the world space.
        /// </summary>
        [Tooltip("Simulates particles in the world space.")]
        World = 0,

        /// <summary>
        /// Simulates particles in the local space of the actor.
        /// </summary>
        [Tooltip("Simulates particles in the local space of the actor.")]
        Local = 1,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The sprite rendering facing modes.
    /// </summary>
    [Tooltip("The sprite rendering facing modes.")]
    public enum ParticleSpriteFacingMode
    {
        /// <summary>
        /// Particles will face camera position.
        /// </summary>
        [Tooltip("Particles will face camera position.")]
        FaceCameraPosition,

        /// <summary>
        /// Particles will face camera plane.
        /// </summary>
        [Tooltip("Particles will face camera plane.")]
        FaceCameraPlane,

        /// <summary>
        /// Particles will orient along velocity vector.
        /// </summary>
        [Tooltip("Particles will orient along velocity vector.")]
        AlongVelocity,

        /// <summary>
        /// Particles will use the custom vector for facing.
        /// </summary>
        [Tooltip("Particles will use the custom vector for facing.")]
        CustomFacingVector,

        /// <summary>
        /// Particles will use the custom fixed axis for facing up.
        /// </summary>
        [Tooltip("Particles will use the custom fixed axis for facing up.")]
        FixedAxis,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The model particle rendering facing modes.
    /// </summary>
    [Tooltip("The model particle rendering facing modes.")]
    public enum ParticleModelFacingMode
    {
        /// <summary>
        /// Particles will face camera position.
        /// </summary>
        [Tooltip("Particles will face camera position.")]
        FaceCameraPosition,

        /// <summary>
        /// Particles will face camera plane.
        /// </summary>
        [Tooltip("Particles will face camera plane.")]
        FaceCameraPlane,

        /// <summary>
        /// Particles will orient along velocity vector.
        /// </summary>
        [Tooltip("Particles will orient along velocity vector.")]
        AlongVelocity,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The particles sorting modes.
    /// </summary>
    [Tooltip("The particles sorting modes.")]
    public enum ParticleSortMode
    {
        /// <summary>
        /// Do not perform additional sorting prior to rendering.
        /// </summary>
        [Tooltip("Do not perform additional sorting prior to rendering.")]
        None,

        /// <summary>
        /// Sorts particles by depth to the view's near plane.
        /// </summary>
        [Tooltip("Sorts particles by depth to the view's near plane.")]
        ViewDepth,

        /// <summary>
        /// Sorts particles by distance to the view's origin.
        /// </summary>
        [Tooltip("Sorts particles by distance to the view's origin.")]
        ViewDistance,

        /// <summary>
        /// The custom sorting according to a per particle attribute. Lower values are rendered before higher values.
        /// </summary>
        [Tooltip("The custom sorting according to a per particle attribute. Lower values are rendered before higher values.")]
        CustomAscending,

        /// <summary>
        /// The custom sorting according to a per particle attribute. Higher values are rendered before lower values.
        /// </summary>
        [Tooltip("The custom sorting according to a per particle attribute. Higher values are rendered before lower values.")]
        CustomDescending,
    }
}
