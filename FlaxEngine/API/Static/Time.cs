// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TimeData
    {
        public float DeltaTime;
        public long DeltaTimeTicks;
        public float Time;
        public long TimeTicks;
        public float UnscaledDeltaTime;
        public long UnscaledDeltaTimeTicks;
        public float UnscaledTime;
        public long UnscaledTimeTicks;
        public float TimeSinceStartup;
        public long TimeSinceStartupTicks;
        public float TimeSinceLevelLoad;
        public long TimeSinceLevelLoadTicks;
    }

    public static partial class Time
    {
        private static TimeData data;

        internal static void SyncData()
        {
            Internal_GetData(ref data);
        }

        /// <summary>
        /// Gets time in seconds it took to complete the last frame, <see cref="TimeScale"/> dependent
        /// </summary>
        public static float DeltaTime => data.DeltaTime;

        /// <summary>
        /// Gets time in seconds it took to complete the last frame in ticks, <see cref="TimeScale"/> dependent
        /// </summary>
        public static long DeltaTimeTicks => data.DeltaTimeTicks;

        /// <summary>
        /// Gets time at the beginning of this frame. This is the time in seconds since the start of the game.
        /// </summary>
        public static float GameTime => data.Time;

        /// <summary>
        /// Gets time at the beginning of this frame in ticks. This is the time in seconds since the start of the game.
        /// </summary>
        public static long GameTimeTicks => data.TimeTicks;

        /// <summary>
        /// Gets real time in seconds since the game started
        /// </summary>
        public static float TimeSinceStartup => data.TimeSinceStartup;

        /// <summary>
        /// Gets real time in seconds since the game started in ticks
        /// </summary>
        public static long TimeSinceStartupTicks => data.TimeSinceStartupTicks;

        /// <summary>
        /// The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded.
        /// </summary>
        public static float TimeSinceLevelLoad => data.TimeSinceLevelLoad;

        /// <summary>
        /// The time this frame has started in ticks. This is the time in seconds since the last level has been loaded.
        /// </summary>
        public static long TimeSinceLevelLoadTicks => data.TimeSinceLevelLoadTicks;

        /// <summary>
        /// Gets timeScale-independent time in seconds it took to complete the last frame.
        /// </summary>
        public static float UnscaledDeltaTime => data.UnscaledDeltaTime;

        /// <summary>
        /// Gets timeScale-independent time in seconds it took to complete the last frame in ticks.
        /// </summary>
        public static long UnscaledDeltaTimeTicks => data.UnscaledDeltaTimeTicks;

        /// <summary>
        /// Gets timeScale-independent time at the beginning of this frame. This is the time in seconds since the start of the game.
        /// </summary>
        public static float UnscaledGameTime => data.UnscaledTime;

        /// <summary>
        /// Gets timeScale-independent time at the beginning of this frame in ticks. This is the time in seconds since the start of the game.
        /// </summary>
        public static long UnscaledGameTimeTicks => data.UnscaledTimeTicks;

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetData(ref TimeData data);
#endif

        #endregion
    }
}
