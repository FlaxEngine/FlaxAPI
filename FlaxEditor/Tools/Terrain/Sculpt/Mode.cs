// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Tools.Terrain.Brushes;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Sculpt
{
    /// <summary>
    /// The base class for terran sculpt tool modes.
    /// </summary>
    public abstract class Mode
    {
        /// <summary>
        /// The options container for the terrain editing apply.
        /// </summary>
        public struct Options
        {
            /// <summary>
            /// If checked, modification apply method should be inverted.
            /// </summary>
            public bool Invert;

            /// <summary>
            /// The master strength parameter to apply when editing the terrain.
            /// </summary>
            public float Strength;

            /// <summary>
            /// The delta time (in seconds) for the terrain modification apply. Used to scale the strength. Adjusted to handle low-FPS.
            /// </summary>
            public float DeltaTime;
        }

        /// <summary>
        /// The tool strength (normalized to range 0-1). Defines the intensity of the sculpt operation to make it stronger or mre subtle.
        /// </summary>
        [EditorOrder(0), Limit(0, 1, 0.01f), Tooltip("The tool strength (normalized to range 0-1). Defines the intensity of the sculpt operation to make it stronger or mre subtle.")]
        public float Strength = 0.3f;

        public void Apply(Brush brush, ref Options options)
        {
            // Combine final apply strength
            float strength = Strength * options.Strength * options.DeltaTime;
            if (strength <= 0.0f)
                return;
        }
    }
}
