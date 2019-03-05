// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Particles group.
    /// </summary>
    public static class Particles
    {
        /// <summary>
        /// Customized <see cref="SurfaceNode"/> for main particle emitter node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class ParticleEmitterNode : SurfaceNode
        {
            /// <summary>
            /// Gets the particle emitter surface.
            /// </summary>
            public ParticleEmitterSurface ParticleSurface => (ParticleEmitterSurface)Surface;

            /// <inheritdoc />
            public ParticleEmitterNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                ParticleSurface._rootNode = this;
                ParticleSurface.ArrangeModulesNodes();
            }

            /// <inheritdoc />
            protected override void SetLocationInternal(ref Vector2 location)
            {
                base.SetLocationInternal(ref location);

                if (Surface != null && ParticleSurface._rootNode == this)
                {
                    ParticleSurface.ArrangeModulesNodes();
                }
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                ParticleSurface._rootNode = null;

                base.Dispose();
            }
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Create = (id, context, arch, groupArch) => new ParticleEmitterNode(id, context, arch, groupArch),
                Title = "Particle Emitter",
                Description = "Main particle emitter node",
                Flags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI | NodeFlags.NoCloseButton,
                Size = new Vector2(300, 600),
                DefaultValues = new object[]
                {
                    1024, // Capacity
                    (int)ParticlesSimulationMode.Default, // Simulation Mode
                    (int)ParticlesSimulationSpace.World, // Simulation Space
                    (int)ParticlesUpdateMode.Realtime, // Update Mode
                    60.0f, // Fixed Timestep
                    true, // Enable Pooling
                    BoundingBox.Zero, // Custom Bounds
                },
                Elements = new[]
                {
                    // Capacity
                    NodeElementArchetype.Factory.Text(0, 0, "Capacity", 100.0f, 16.0f, "The particle system capacity (the maximum amount of particles to simulate at once)."),
                    NodeElementArchetype.Factory.Integer(110, 0, 0),

                    // Simulation Mode
                    NodeElementArchetype.Factory.Text(0, 1 * Surface.Constants.LayoutOffsetY, "Simulation Mode", 100.0f, 16.0f, "The particles simulation execution mode."),
                    NodeElementArchetype.Factory.ComboBox(110, 1 * Surface.Constants.LayoutOffsetY, 80, 1, typeof(ParticlesSimulationMode)),

                    // Simulation Space
                    NodeElementArchetype.Factory.Text(0, 2 * Surface.Constants.LayoutOffsetY, "Simulation Space", 100.0f, 16.0f, "The particles simulation space."),
                    NodeElementArchetype.Factory.ComboBox(110, 2 * Surface.Constants.LayoutOffsetY, 80, 2, typeof(ParticlesSimulationSpace)),

                    // Update Mode
                    NodeElementArchetype.Factory.Text(0, 3 * Surface.Constants.LayoutOffsetY, "Update Mode", 100.0f, 16.0f, "The particles simulation update mode."),
                    NodeElementArchetype.Factory.ComboBox(110, 3 * Surface.Constants.LayoutOffsetY, 80, 3, typeof(ParticlesUpdateMode)),

                    // Fixed Timestep
                    NodeElementArchetype.Factory.Text(0, 4 * Surface.Constants.LayoutOffsetY, "Fixed Timestep", 100.0f, 16.0f, "The fixed timestep. Used only if Update Mode is set to FixedTimestep."),
                    NodeElementArchetype.Factory.Float(110, 4 * Surface.Constants.LayoutOffsetY, 4),

                    // Enable Pooling
                    NodeElementArchetype.Factory.Text(0, 5 * Surface.Constants.LayoutOffsetY, "Enable Pooling", 100.0f, 16.0f, "True if enable pooling emitter instance data, otherwise immediately dispose. Pooling can improve performance and reduce memory usage."),
                    NodeElementArchetype.Factory.Bool(110, 5 * Surface.Constants.LayoutOffsetY, 5),

                    // Custom Bounds
                    NodeElementArchetype.Factory.Text(0, 6 * Surface.Constants.LayoutOffsetY, "Custom Bounds", 100.0f, 16.0f, "The custom bounds to use for the particles. Set to zero to use automatic bounds (valid only for CPU particles)."),
                    NodeElementArchetype.Factory.Box(110, 6 * Surface.Constants.LayoutOffsetY, 6),
                }
            },
        };
    }
}
