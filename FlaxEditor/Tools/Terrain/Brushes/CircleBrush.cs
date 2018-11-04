// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Brushes
{
    /// <summary>
    /// Terrain brush that has circle shape and uses radial falloff.
    /// </summary>
    /// <seealso cref="FlaxEditor.Tools.Terrain.Brushes.Brush" />
    public class CircleBrush : Brush
    {
        /// <summary>
        /// Circle brush falloff types.
        /// </summary>
        public enum FalloffTypes
        {
            /// <summary>
            /// A linear falloff that has been smoothed to round off the sharp edges where the falloff begins and ends.
            /// </summary>
            Smooth = 0,

            /// <summary>
            /// A sharp linear falloff, without rounded edges.
            /// </summary>
            Linear = 1,

            /// <summary>
            /// A half-ellipsoid-shaped falloff that begins smoothly and ends sharply.
            /// </summary>
            Spherical = 2,

            /// <summary>
            ///  falloff with an abrupt start and a smooth ellipsoidal end. The opposite of the Sphere falloff.
            /// </summary>
            Tip = 3,
        }

        /// <summary>
        /// The brush falloff that defines the percentage from the brush's extents where the falloff should begin. Essentially, this describes how hard the brush's edges are. A falloff of 0.0 means the brush will have full effect throughout with hard edges. A falloff of 1.0 means the brush will only have full effect at its center, and the effect will be reduced throughout its entire area to the edge.
        /// </summary>
        [EditorOrder(10), Limit(0, 1, 0.01f), Tooltip("The brush falloff that defines the percentage from the brush's extents where the falloff should begin. Essentially, this describes how hard the brush's edges are. A falloff of 0.0 means the brush will have full effect throughout with hard edges. A falloff of 1.0 means the brush will only have full effect at its center, and the effect will be reduced throughout its entire area to the edge.")]
        public float Falloff = 0.5f;

        /// <summary>
        /// The brush falloff type. Defines circle brush falloff mode.
        /// </summary>
        [EditorOrder(20), Tooltip("The brush falloff type. Defines circle brush falloff mode.")]
        public FalloffTypes FalloffType = FalloffTypes.Smooth;

        private delegate float CalculateFalloffDelegate(float distance, float radius, float falloff);

        private float CalculateFalloff_Smooth(float distance, float radius, float falloff)
        {
            // Smooth-step linear falloff
            float y = CalculateFalloff_Linear(distance, radius, falloff);
            return y * y * (3 - 2 * y);
        }

        private float CalculateFalloff_Linear(float distance, float radius, float falloff)
        {
            return distance < radius ? 1.0f : falloff > 0.0f ? Mathf.Max(0.0f, 1.0f - (distance - radius) / falloff) : 0.0f;
        }

        private float CalculateFalloff_Spherical(float distance, float radius, float falloff)
        {
            if (distance <= radius)
            {
                return 1.0f;
            }

            if (distance > radius + falloff)
            {
                return 0.0f;
            }

            // Elliptical falloff
            return Mathf.Sqrt(1.0f - Mathf.Square((distance - radius) / falloff));
        }

        private float CalculateFalloff_Tip(float distance, float radius, float falloff)
        {
            if (distance <= radius)
            {
                return 1.0f;
            }

            if (distance > radius + falloff)
            {
                return 0.0f;
            }

            // Inverse elliptical falloff
            return 1.0f - Mathf.Sqrt(1.0f - Mathf.Square((falloff + radius - distance) / falloff));
        }
    }
}
