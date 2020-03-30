// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Contains information about CPU (Central Processing Unit).
    /// </summary>
    [Tooltip("Contains information about CPU (Central Processing Unit).")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct CPUInfo
    {
        /// <summary>
        /// The number of physical processor packages.
        /// </summary>
        [Tooltip("The number of physical processor packages.")]
        public uint ProcessorPackageCount;

        /// <summary>
        /// The number of processor cores (physical).
        /// </summary>
        [Tooltip("The number of processor cores (physical).")]
        public uint ProcessorCoreCount;

        /// <summary>
        /// The number of logical processors (including hyper-threading).
        /// </summary>
        [Tooltip("The number of logical processors (including hyper-threading).")]
        public uint LogicalProcessorCount;

        /// <summary>
        /// The size of processor L1 caches (in bytes).
        /// </summary>
        [Tooltip("The size of processor L1 caches (in bytes).")]
        public uint L1CacheSize;

        /// <summary>
        /// The size of processor L2 caches (in bytes).
        /// </summary>
        [Tooltip("The size of processor L2 caches (in bytes).")]
        public uint L2CacheSize;

        /// <summary>
        /// The size of processor L3 caches (in bytes).
        /// </summary>
        [Tooltip("The size of processor L3 caches (in bytes).")]
        public uint L3CacheSize;

        /// <summary>
        /// The CPU memory page size (in bytes).
        /// </summary>
        [Tooltip("The CPU memory page size (in bytes).")]
        public uint PageSize;

        /// <summary>
        /// The CPU clock speed (in Hz).
        /// </summary>
        [Tooltip("The CPU clock speed (in Hz).")]
        public ulong ClockSpeed;

        /// <summary>
        /// The CPU cache line size (in bytes).
        /// </summary>
        [Tooltip("The CPU cache line size (in bytes).")]
        public uint CacheLineSize;
    }
}
