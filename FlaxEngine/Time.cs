// Flax Engine scripting API

using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// The interface to get time information from Celelej.
    /// </summary>
    public static class Time
    {
        // TODO: cache these values in CLR world and refresh every frame to reduce amount of Mono invokes

        /// <summary>
        /// The time in seconds it took to complete the last frame
        /// </summary>
        public static float deltaTime
        {
            get { return Internal_GetDeltaTime(); }
        }

        /// <summary>
        /// The real time in seconds since the game started
        /// </summary>
        public static float realtimeSinceStartup
        {
            get { return Internal_GetRealtimeSinceStartup(); }
        }

        /// <summary>
        /// The time at the beginning of this frame (Read Only). This is the time in seconds since the start of the game.
        /// </summary>
        public static float time
        {
            get { return Internal_GetTime(); }
        }

        /// <summary>
        /// The scale at which the time is passing. This can be used for slow motion effects.
        /// </summary>
        public static float timeScale
        {
            get { return Internal_GetTimeScale(); }
            set { Internal_SetTimeScale(value); }
        }

        /// <summary>
        /// The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded.
        /// </summary>
        public static float timeSinceLevelLoad
        {
            get { return Internal_GetTimeSinceLevelLoad(); }
        }

        /// <summary>
        /// The timeScale-independent time in seconds it took to complete the last frame (Read Only).
        /// </summary>
        public static float unscaledDeltaTime
        {
            get { return Internal_GetUnscaledDeltaTime(); }
        }

        /// <summary>
        /// The timeScale-independant time at the beginning of this frame (Read Only). This is the time in seconds since the
        /// start of the game.
        /// </summary>
        public static float unscaledTime
        {
            get { return Internal_GetUnscaledTime(); }
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDeltaTime();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetRealtimeSinceStartup();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTime();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTimeScale();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTimeScale(float value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTimeSinceLevelLoad();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetUnscaledDeltaTime();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetUnscaledTime();

        #endregion
    }
}
