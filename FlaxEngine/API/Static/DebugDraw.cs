// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public static partial class DebugDraw
    {
        /// <summary>
        /// Draws the sphere.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void DrawSphere(BoundingSphere sphere, Color color, float duration = 0.0f, bool depthTest = true)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_DrawSphere(ref sphere.Center, sphere.Radius, ref color, duration, depthTest);
#endif
        }

        /// <summary>
        /// Draws the debug data.
        /// </summary>
        /// <param name="task">The calling rendering task.</param>
        /// <param name="selectedActors">The selected actors.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void Draw(FlaxEngine.Rendering.RenderTask task, Actor[] selectedActors)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // Get unmanaged pointers
            IntPtr[] actors = null;
            if (selectedActors != null && selectedActors.Length > 0)
            {
                actors = new IntPtr[selectedActors.Length];
                for (int i = 0; i < selectedActors.Length; i++)
                {
                    actors[i] = Object.GetUnmanagedPtr(selectedActors[i]);
                }
            }

            Internal_Draw(Object.GetUnmanagedPtr(task), actors, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, false);
#endif
        }
    }
}
