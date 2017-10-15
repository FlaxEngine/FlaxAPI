////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    public static partial class Time
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct Data
        {
            public float DeltaTime;
            public long DeltaTimeTicks;
            public float Time;
            public long TimeTicks;
            public float UnscaledDeltaTime;
            public long UnscaledDeltaTimeTicks;
            public float UnscaledTime;
            public long UnscaledTimeTicks;
            public float RealtileSinceStartup;
            public long RealtileSinceStartupTicks;
            public float TimeSinceLevelLoad;
            public long TimeSinceLevelLoadTicks;
        }

        private static Data data;

        internal static void SyncData()
        {
            Internal_GetData(ref data);
        }

        /// <summary>
        /// Gets time in seconds it took to complete the last frame, <see cref="TimeScale"/> dependent
        /// </summary>
        public static float DeltaTime => data.DeltaTimeTicks;

        /// <summary>
        /// Gets time in seconds it took to complete the last frame in ticks, <see cref="TimeScale"/> dependent
        /// </summary>
        public static long DeltaTimeTicks => data.DeltaTimeTicks;

        /// <summary>
        /// Gets real time in seconds since the game started
        /// </summary>
        public static float RealtimeSinceStartup => data.DeltaTimeTicks;

        /// <summary>
        /// Gets real time in seconds since the game started in ticks
        /// </summary>
        public static long RealtimeSinceStartupTicks => data.DeltaTimeTicks;

        /// <summary>
        /// Gets time at the beginning of this frame. This is the time in seconds since the start of the game. <see cref="TimeScale"/> dependent
        /// </summary>
        public static float TotalTimeSinceStartup => data.DeltaTimeTicks;

        /// <summary>
        /// Gets time at the beginning of this frame  in ticks. This is the time in seconds since the start of the game. <see cref="TimeScale"/> dependent
        /// </summary>
        public static long TotalTimeSinceStartupTicks => data.DeltaTimeTicks;

        /// <summary>
        /// The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded.
        /// </summary>
        public static float TimeSinceLevelLoad => data.DeltaTimeTicks;

        /// <summary>
        /// The time this frame has started in ticks. This is the time in seconds since the last level has been loaded.
        /// </summary>
        public static long TimeSinceLevelLoadTicks => data.DeltaTimeTicks;

        /// <summary>
        /// Gets timeScale-independent time in seconds it took to complete the last frame.
        /// </summary>
        public static float UnscaledDeltaTime => data.DeltaTimeTicks;

        /// <summary>
        /// Gets timeScale-independent time in seconds it took to complete the last frame in ticks.
        /// </summary>
        public static long UnscaledDeltaTimeTicks => data.DeltaTimeTicks;

        /// <summary>
        /// Gets timeScale-independant time at the beginning of this frame. This is the time in seconds since the start of the game.
        /// </summary>
        public static float UnscaledTime => data.DeltaTimeTicks;

        /// <summary>
        /// Gets timeScale-independant time at the beginning of this frame in ticks. This is the time in seconds since the start of the game.
        /// </summary>
        public static long UnscaledTimeTicks => data.DeltaTimeTicks;

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetData(ref Data data);
#endif

        #endregion
    }
}
