////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Layers group.
    /// </summary>
    public static class Layers
    {
        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Title = "Sample Layer",
                Description = "Sample material or material instance",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(160, 100),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "UVs", true, ConnectionType.Vector2, 0),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Asset(0, Surface.Constants.LayoutOffsetY, 0, ContentDomain.Material)
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Title = "Linear Layer Blend",
                Description = "Create blended layer using linear math",
                Flags = NodeFlags.MaterialOnly,
                Size = new Vector2(150, 80),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Bottom", true, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(1, "Top", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(2, "Alpha", true, ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 3)
                }
            }
        };
    }
}
