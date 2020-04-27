// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Provides CPU performance measuring methods.
    /// </summary>
    [Tooltip("Provides CPU performance measuring methods.")]
    public static unsafe partial class ProfilerCPU
    {
        /// <summary>
        /// Represents single CPU profiling event data.
        /// </summary>
        [Tooltip("Represents single CPU profiling event data.")]
        [StructLayout(LayoutKind.Sequential)]
        public unsafe partial struct Event
        {
            /// <summary>
            /// The start time (in milliseconds).
            /// </summary>
            [Tooltip("The start time (in milliseconds).")]
            public double Start;

            /// <summary>
            /// The end time (in milliseconds).
            /// </summary>
            [Tooltip("The end time (in milliseconds).")]
            public double End;

            /// <summary>
            /// The event depth. Value 0 is used for the root event.
            /// </summary>
            [Tooltip("The event depth. Value 0 is used for the root event.")]
            public int Depth;

            /// <summary>
            /// The native dynamic memory allocation size during this event (excluding the child events). Given value is in bytes.
            /// </summary>
            [Tooltip("The native dynamic memory allocation size during this event (excluding the child events). Given value is in bytes.")]
            public int NativeMemoryAllocation;

            /// <summary>
            /// The managed memory allocation size during this event (excluding the child events). Given value is in bytes.
            /// </summary>
            [Tooltip("The managed memory allocation size during this event (excluding the child events). Given value is in bytes.")]
            public int ManagedMemoryAllocation;

            /// <summary>
            /// The name of the event.
            /// </summary>
            [Tooltip("The name of the event.")]
            public char* Name;
        }
    }
}
