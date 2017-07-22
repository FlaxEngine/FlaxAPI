////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Tools group.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Title = "Fresnel",
                Description = "Calculates a falloff based on the dot product of the surface normal and the direction to the camera",
                Flags = NodeFlags.CloseButton | NodeFlags.MaterialOnly,
                Size = new Vector2(140, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Exponent", true, ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Input(1, "Base Reflect Fraction", true, ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Input(2, "Normal", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Float, 3)
                }
            },
        };
    }
}
