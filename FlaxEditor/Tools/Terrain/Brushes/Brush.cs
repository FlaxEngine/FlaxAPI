// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Brushes
{
    /// <summary>
    /// Terrain sculpture or paint brush logic descriptor.
    /// </summary>
    public abstract class Brush
    {
        /// <summary>
        /// The brush size (in world units). Within this area, the brush will have at least some effect.
        /// </summary>
        [EditorOrder(0), Limit(0.0f, 1000000.0f, 10.0f), Tooltip("The brush size (in world units). Within this area, the brush will have at least some effect.")]
        public float Size = 1000.0f;
    }
}
