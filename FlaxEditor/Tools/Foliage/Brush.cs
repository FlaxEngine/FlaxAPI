// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Foliage painting brush.
    /// </summary>
    public class Brush
    {
        /// <summary>
        /// The cached material instance for the brush usage.
        /// </summary>
        protected MaterialInstance _material;

        /// <summary>
        /// The brush size (in world units). Within this area, the brush will have effect.
        /// </summary>
        [EditorOrder(0), Limit(0.0f, 1000000.0f, 10.0f), Tooltip("The brush size (in world units). Within this area, the brush will have effect.")]
        public float Size = 800.0f;

        /// <summary>
        /// Gets the brush material for the terrain chunk rendering. It must have domain set to Terrain. Setup material parameters within this call.
        /// </summary>
        /// <param name="position">The world-space brush position.</param>
        /// <param name="color">The brush position.</param>
        /// <returns>The ready to render material for terrain chunks overlay on top of the terrain.</returns>
        public MaterialInstance GetBrushMaterial(ref Vector3 position, ref Color color)
        {
            if (!_material)
            {
                var material = FlaxEngine.Content.LoadAsyncInternal<Material>(EditorAssets.FoliageBrushMaterial);
                material.WaitForLoaded();
                _material = material.CreateVirtualInstance();
            }

            if (_material)
            {
                // TODO: cache parameters
                _material.GetParam("Color").Value = color;
                _material.GetParam("Radius").Value = Size * 0.5f;
            }

            return _material;
        }
    }
}
