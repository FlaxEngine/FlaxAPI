// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Provides GPU performance measuring methods.
    /// </summary>
    [Tooltip("Provides GPU performance measuring methods.")]
    public static unsafe partial class ProfilerGPU
    {
        /// <summary>
        /// Represents single CPU profiling event data.
        /// </summary>
        [Tooltip("Represents single CPU profiling event data.")]
        [StructLayout(LayoutKind.Sequential)]
        public unsafe partial struct Event
        {
            /// <summary>
            /// The name of the event.
            /// </summary>
            [Tooltip("The name of the event.")]
            public char* Name;

            /// <summary>
            /// The timer query used to get the exact event time on a GPU. Assigned and managed by the internal profiler layer.
            /// </summary>
            [Tooltip("The timer query used to get the exact event time on a GPU. Assigned and managed by the internal profiler layer.")]
            public IntPtr Timer;

            /// <summary>
            /// The rendering stats for this event. When event is active it holds the stats on event begin.
            /// </summary>
            [Tooltip("The rendering stats for this event. When event is active it holds the stats on event begin.")]
            public RenderStatsData Stats;

            /// <summary>
            /// The event execution time on a GPU (in milliseconds).
            /// </summary>
            [Tooltip("The event execution time on a GPU (in milliseconds).")]
            public float Time;

            /// <summary>
            /// The event depth. Value 0 is used for the root events.
            /// </summary>
            [Tooltip("The event depth. Value 0 is used for the root events.")]
            public int Depth;
        }
    }
}
