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
            /// <inheritdoc />
            public ParticleEmitterNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
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
                Size = new Vector2(200, 300),
                /*Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "", true, ConnectionType.Impulse, 0),
                }*/
            },
        };
    }
}
