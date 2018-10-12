// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEditor.Profiling
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
        /// The update time on CPU (in milliseconds).
        /// </summary>
        public float UpdateTimeMs;

        /// <summary>
        /// The fixed update time on CPU (in milliseconds).
        /// </summary>
        public float PhysicsTimeMs;

        /// <summary>
        /// The draw time on CPU (in milliseconds).
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
        /// The start time (in milliseconds).
        /// </summary>
        public double Start;

        /// <summary>
        /// The end time (in milliseconds).
        /// </summary>
        public double End;

        /// <summary>
        /// The event depth (0 for root events).
        /// </summary>
        public int Depth;

        /// <summary>
        /// The dynamic memory allocation size during this event (excluding the child events). Given value is in bytes.
        /// </summary>
        public int MemoryAllocation;

        /// <summary>
        /// The event name.
        /// </summary>
        public string Name;
    }

    /// <summary>
    /// The CPU thread profiling data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ThreadStats
    {
        /// <summary>
        /// The thread name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The events recorded since last profiling update.
        /// </summary>
        public EventCPU[] Events;
    }

    /// <summary>
    /// The GPU profiling event data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EventGPU
    {
        /// <summary>
        /// The event execution time on a GPU (in milliseconds).
        /// </summary>
        public float Time;

        /// <summary>
        /// The event depth (0 for root events).
        /// </summary>
        public int Depth;

        /// <summary>
        /// The rendering stats for this event.
        /// </summary>
        public RenderStatsData Stats;

        /// <summary>
        /// The event name.
        /// </summary>
        public string Name;
    }

    /// <summary>
    /// Object that stores various render statistics.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderStatsData
    {
        /// <summary>
        /// The draw calls count.
        /// </summary>
        public long DrawCalls;

        /// <summary>
        /// The compute shader calls count.
        /// </summary>
        public long ComputeCalls;

        /// <summary>
        /// The vertices drawn count.
        /// </summary>
        public long Vertices;

        /// <summary>
        /// The triangles drawn count.
        /// </summary>
        public long Triangles;

        /// <summary>
        /// The pipeline state changes count.
        /// </summary>
        public long PipelineStateChanges;
    }
}
