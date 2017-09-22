////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class BoxBrush
    {
        /// <summary>
        /// Determines if there is an intersection between the brush surface and a ray.
        /// If collision data is available on the CPU performs exact intersection check with the geometry.
        /// Otherwise performs simple <see cref="BoundingBox"/> vs <see cref="Ray"/> test.
        /// For more efficient collisions detection and ray casting use physics.
        /// </summary>
        /// <param name="surfaceIndex">The zero-based index of the brush surface. Each brush has 6 surfaces. One for each side.</param>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection.</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
        public bool IntersectsSurface(int surfaceIndex, ref Ray ray, out float distance)
        {
            if (surfaceIndex < 0 || surfaceIndex >= 6)
                throw new ArgumentOutOfRangeException(nameof(surfaceIndex));
            return Internal_IntersectsSurface(unmanagedPtr, surfaceIndex, ref ray, out distance);
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsSurface(IntPtr obj, int surfaceIndex, ref Ray ray, out float distance);
#endif
    }
}
