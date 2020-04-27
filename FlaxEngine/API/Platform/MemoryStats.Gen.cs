// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Contains information about current memory usage and capacity.
    /// </summary>
    [Tooltip("Contains information about current memory usage and capacity.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct MemoryStats
    {
        /// <summary>
        /// Total amount of physical memory in bytes.
        /// </summary>
        [Tooltip("Total amount of physical memory in bytes.")]
        public ulong TotalPhysicalMemory;

        /// <summary>
        /// Amount of used physical memory in bytes.
        /// </summary>
        [Tooltip("Amount of used physical memory in bytes.")]
        public ulong UsedPhysicalMemory;

        /// <summary>
        /// Total amount of virtual memory in bytes.
        /// </summary>
        [Tooltip("Total amount of virtual memory in bytes.")]
        public ulong TotalVirtualMemory;

        /// <summary>
        /// Amount of used virtual memory in bytes.
        /// </summary>
        [Tooltip("Amount of used virtual memory in bytes.")]
        public ulong UsedVirtualMemory;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains information about current memory usage by the process.
    /// </summary>
    [Tooltip("Contains information about current memory usage by the process.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct ProcessMemoryStats
    {
        /// <summary>
        /// Amount of used physical memory in bytes.
        /// </summary>
        [Tooltip("Amount of used physical memory in bytes.")]
        public ulong UsedPhysicalMemory;

        /// <summary>
        /// Amount of used virtual memory in bytes.
        /// </summary>
        [Tooltip("Amount of used virtual memory in bytes.")]
        public ulong UsedVirtualMemory;
    }
}
