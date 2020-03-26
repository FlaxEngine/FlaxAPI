// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Profiler tools for development. Allows to gather profiling data and events from the engine.
    /// </summary>
    [Tooltip("Profiler tools for development. Allows to gather profiling data and events from the engine.")]
    public static unsafe partial class ProfilingTools
    {
        /// <summary>
        /// The current collected main stats by the profiler from the local session. Updated every frame.
        /// </summary>
        [Tooltip("The current collected main stats by the profiler from the local session. Updated every frame.")]
        public static MainStats Stats
        {
            get { Internal_GetStats(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetStats(out MainStats resultAsRef);

        /// <summary>
        /// The CPU threads profiler events.
        /// </summary>
        [Tooltip("The CPU threads profiler events.")]
        public static ThreadStats[] EventsCPU
        {
            get { return Internal_GetEventsCPU(typeof(ThreadStats)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ThreadStats[] Internal_GetEventsCPU(System.Type resultArrayItemType0);

        /// <summary>
        /// The GPU rendering profiler events.
        /// </summary>
        [Tooltip("The GPU rendering profiler events.")]
        public static ProfilerGPU.Event[] EventsGPU
        {
            get { return Internal_GetEventsGPU(typeof(ProfilerGPU.Event)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ProfilerGPU.Event[] Internal_GetEventsGPU(System.Type resultArrayItemType0);

        /// <summary>
        /// The GPU memory stats.
        /// </summary>
        [Tooltip("The GPU memory stats.")]
        [StructLayout(LayoutKind.Sequential)]
        public unsafe partial struct MemoryStatsGPU
        {
            /// <summary>
            /// The total amount of memory in bytes (as reported by the driver).
            /// </summary>
            [Tooltip("The total amount of memory in bytes (as reported by the driver).")]
            public ulong Total;

            /// <summary>
            /// The used by the game amount of memory in bytes (estimated).
            /// </summary>
            [Tooltip("The used by the game amount of memory in bytes (estimated).")]
            public ulong Used;
        }

        /// <summary>
        /// Engine profiling data header. Contains main info and stats.
        /// </summary>
        [Tooltip("Engine profiling data header. Contains main info and stats.")]
        [StructLayout(LayoutKind.Sequential)]
        public unsafe partial struct MainStats
        {
            /// <summary>
            /// The process memory stats.
            /// </summary>
            [Tooltip("The process memory stats.")]
            public ProcessMemoryStats ProcessMemory;

            /// <summary>
            /// The CPU memory stats.
            /// </summary>
            [Tooltip("The CPU memory stats.")]
            public MemoryStats MemoryCPU;

            /// <summary>
            /// The GPU memory stats.
            /// </summary>
            [Tooltip("The GPU memory stats.")]
            public MemoryStatsGPU MemoryGPU;

            /// <summary>
            /// The frames per second (fps counter).
            /// </summary>
            [Tooltip("The frames per second (fps counter).")]
            public int FPS;

            /// <summary>
            /// The update time on CPU (in milliseconds).
            /// </summary>
            [Tooltip("The update time on CPU (in milliseconds).")]
            public float UpdateTimeMs;

            /// <summary>
            /// The fixed update time on CPU (in milliseconds).
            /// </summary>
            [Tooltip("The fixed update time on CPU (in milliseconds).")]
            public float PhysicsTimeMs;

            /// <summary>
            /// The draw time on CPU (in milliseconds).
            /// </summary>
            [Tooltip("The draw time on CPU (in milliseconds).")]
            public float DrawCPUTimeMs;

            /// <summary>
            /// The draw time on GPU (in milliseconds).
            /// </summary>
            [Tooltip("The draw time on GPU (in milliseconds).")]
            public float DrawGPUTimeMs;

            /// <summary>
            /// The last rendered frame stats.
            /// </summary>
            [Tooltip("The last rendered frame stats.")]
            public RenderStatsData DrawStats;
        }

        /// <summary>
        /// The CPU thread stats.
        /// </summary>
        [Tooltip("The CPU thread stats.")]
        [StructLayout(LayoutKind.Sequential)]
        public unsafe partial struct ThreadStats
        {
            /// <summary>
            /// The thread name.
            /// </summary>
            [Tooltip("The thread name.")]
            public string Name;

            /// <summary>
            /// The events list.
            /// </summary>
            [Tooltip("The events list.")]
            public ProfilerCPU.Event[] Events;
        }
    }
}
