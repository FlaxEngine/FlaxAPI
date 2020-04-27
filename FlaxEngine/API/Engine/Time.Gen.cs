// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Game ticking and timing system.
    /// </summary>
    [Tooltip("Game ticking and timing system.")]
    public static unsafe partial class Time
    {
        /// <summary>
        /// The time at which the game started (UTC local).
        /// </summary>
        [Tooltip("The time at which the game started (UTC local).")]
        public static DateTime StartupTime
        {
            get { Internal_GetStartupTime(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetStartupTime(out DateTime resultAsRef);

        /// <summary>
        /// The target amount of the game logic updates per second (script updates frequency).
        /// </summary>
        [Tooltip("The target amount of the game logic updates per second (script updates frequency).")]
        public static float UpdateFPS
        {
            get { return Internal_GetUpdateFPS(); }
            set { Internal_SetUpdateFPS(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetUpdateFPS();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUpdateFPS(float value);

        /// <summary>
        /// The target amount of the physics simulation updates per second (also fixed updates frequency).
        /// </summary>
        [Tooltip("The target amount of the physics simulation updates per second (also fixed updates frequency).")]
        public static float PhysicsFPS
        {
            get { return Internal_GetPhysicsFPS(); }
            set { Internal_SetPhysicsFPS(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPhysicsFPS();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPhysicsFPS(float value);

        /// <summary>
        /// The target amount of the frames rendered per second (actual game FPS).
        /// </summary>
        [Tooltip("The target amount of the frames rendered per second (actual game FPS).")]
        public static float DrawFPS
        {
            get { return Internal_GetDrawFPS(); }
            set { Internal_SetDrawFPS(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDrawFPS();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrawFPS(float value);

        /// <summary>
        /// The game time scale factor. Default is 1.
        /// </summary>
        [Tooltip("The game time scale factor. Default is 1.")]
        public static float TimeScale
        {
            get { return Internal_GetTimeScale(); }
            set { Internal_SetTimeScale(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTimeScale();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTimeScale(float value);

        /// <summary>
        /// Gets or sets the value indicating whenever game logic is paused (physics, script updates, etc.).
        /// </summary>
        [Tooltip("The value indicating whenever game logic is paused (physics, script updates, etc.).")]
        public static bool GamePaused
        {
            get { return Internal_GetGamePaused(); }
            set { Internal_SetGamePaused(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetGamePaused();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetGamePaused(bool value);

        /// <summary>
        /// Gets time in seconds it took to complete the last frame, <see cref="TimeScale"/> dependent.
        /// </summary>
        [Tooltip("Gets time in seconds it took to complete the last frame, <see cref=\"TimeScale\"/> dependent.")]
        public static float DeltaTime
        {
            get { return Internal_GetDeltaTime(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDeltaTime();

        /// <summary>
        /// Gets time at the beginning of this frame. This is the time in seconds since the start of the game.
        /// </summary>
        [Tooltip("Gets time at the beginning of this frame. This is the time in seconds since the start of the game.")]
        public static float GameTime
        {
            get { return Internal_GetGameTime(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetGameTime();

        /// <summary>
        /// Gets timeScale-independent time in seconds it took to complete the last frame.
        /// </summary>
        [Tooltip("Gets timeScale-independent time in seconds it took to complete the last frame.")]
        public static float UnscaledDeltaTime
        {
            get { return Internal_GetUnscaledDeltaTime(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetUnscaledDeltaTime();

        /// <summary>
        /// Gets timeScale-independent time at the beginning of this frame. This is the time in seconds since the start of the game.
        /// </summary>
        [Tooltip("Gets timeScale-independent time at the beginning of this frame. This is the time in seconds since the start of the game.")]
        public static float UnscaledGameTime
        {
            get { return Internal_GetUnscaledGameTime(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetUnscaledGameTime();

        /// <summary>
        /// Gets the time since startup in seconds (unscaled).
        /// </summary>
        [Tooltip("The time since startup in seconds (unscaled).")]
        public static float TimeSinceStartup
        {
            get { return Internal_GetTimeSinceStartup(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTimeSinceStartup();
    }
}
