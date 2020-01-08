// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public static partial class DebugDraw
    {
        /// <summary>
        /// Draws the debug data.
        /// </summary>
        /// <param name="task">The calling rendering task.</param>
        /// <param name="selectedActors">The selected actors.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void Draw(RenderTask task, Actor[] selectedActors)
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
