////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace FlaxEditor
{
    public partial class ProfilingTools
    {
        /// <summary>
        /// Engine profiling data header. Contains main info and stats.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MainStats
        {
            /// <summary>
            /// The process CPU memory stats. Amount of used physical memory in bytes.
            /// </summary>
            public ulong ProcessMemory_UsedPhysicalMemory;

            /// <summary>
            /// The process CPU memory stats.  Amount of used virtual memory in bytes.
            /// </summary>
            public ulong ProcessMemory_UsedVirtualMemory;

            /// <summary>
            /// The CPU memory stats. Total amount of physical memory in bytes.
            /// </summary>
            public ulong MemoryCPU_TotalPhysicalMemory;

            /// <summary>
            /// The CPU memory stats. Amount of used physical memory in bytes.
            /// </summary>
            public ulong MemoryCPU_UsedPhysicalMemory;

            /// <summary>
            /// The CPU memory stats. Total amount of virtual memory in bytes.
            /// </summary>
            public ulong MemoryCPU_TotalVrtualMemory;

            /// <summary>
            /// The CPU memory stats. Amount of used virtual memory in bytes.
            /// </summary>
            public ulong MemoryCPU_UsedVirtualMemory;

            /// <summary>
            /// The GPU memory stats. Total amount of memory in bytes (as reported by the driver).
            /// </summary>
            public ulong MemoryGPU_Total;

            /// <summary>
            /// The GPU memory stats. Used by the game amount of memory in bytes (estimated).
            /// </summary>
            public ulong MemoryGPU_Used;

            /// <summary>
            /// The frames per second (fps counter).
            /// </summary>
            public int FPS;

            /// <summary>
            /// The update time on CPU (in miliseconds).
            /// </summary>
            public float UpdateTimeMs;

            /// <summary>
            /// The fixed update time on CPU (in miliseconds).
            /// </summary>
            public float PhysicsTimeMs;

            /// <summary>
            /// The draw time on CPU (in miliseconds).
            /// </summary>
            public float DrawTimeMs;
        }

        /// <summary>
        /// The CPU profiling event data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct EventCPU
        {
            /// <summary>
            /// The start time (in miliseconds).
            /// </summary>
            public double Start;

            /// <summary>
            /// The end time (in miliseconds).
            /// </summary>
            public double End;

            /// <summary>
            /// The event depth (0 for root events).
            /// </summary>
            public int Depth;

            /// <summary>
            /// The event name.
            /// </summary>
            public string Name;
        }
    }
}
