// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The scene animation playback actor.
    /// </summary>
    [Tooltip("The scene animation playback actor.")]
    public unsafe partial class SceneAnimationPlayer : Actor
    {
        /// <inheritdoc />
        protected SceneAnimationPlayer() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SceneAnimationPlayer"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static SceneAnimationPlayer New()
        {
            return Internal_Create(typeof(SceneAnimationPlayer)) as SceneAnimationPlayer;
        }

        /// <summary>
        /// The scene animation to play.
        /// </summary>
        [EditorDisplay("Scene Animation"), EditorOrder(0), DefaultValue(null)]
        [Tooltip("The scene animation to play.")]
        public SceneAnimation Animation
        {
            get { return Internal_GetAnimation(unmanagedPtr); }
            set { Internal_SetAnimation(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SceneAnimation Internal_GetAnimation(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAnimation(IntPtr obj, IntPtr value);

        /// <summary>
        /// The animation playback speed factor. Scales the timeline update delta time. Can be used to speed up or slow down the sequence.
        /// </summary>
        [EditorDisplay("Scene Animation"), EditorOrder(10), DefaultValue(1.0f)]
        [Tooltip("The animation playback speed factor. Scales the timeline update delta time. Can be used to speed up or slow down the sequence.")]
        public float Speed
        {
            get { return Internal_GetSpeed(unmanagedPtr); }
            set { Internal_SetSpeed(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSpeed(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSpeed(IntPtr obj, float value);

        /// <summary>
        /// The animation start time. Can be used to skip part of the sequence on begin.
        /// </summary>
        [EditorDisplay("Scene Animation"), EditorOrder(20), DefaultValue(0.0f)]
        [Tooltip("The animation start time. Can be used to skip part of the sequence on begin.")]
        public float StartTime
        {
            get { return Internal_GetStartTime(unmanagedPtr); }
            set { Internal_SetStartTime(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetStartTime(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStartTime(IntPtr obj, float value);

        /// <summary>
        /// Determines whether the scene animation should take into account the global game time scale for simulation updates.
        /// </summary>
        [EditorDisplay("Scene Animation"), EditorOrder(30), DefaultValue(true)]
        [Tooltip("Determines whether the scene animation should take into account the global game time scale for simulation updates.")]
        public bool UseTimeScale
        {
            get { return Internal_GetUseTimeScale(unmanagedPtr); }
            set { Internal_SetUseTimeScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseTimeScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseTimeScale(IntPtr obj, bool value);

        /// <summary>
        /// Determines whether the scene animation should loop when it finishes playing.
        /// </summary>
        [EditorDisplay("Scene Animation"), EditorOrder(40), DefaultValue(false)]
        [Tooltip("Determines whether the scene animation should loop when it finishes playing.")]
        public bool Loop
        {
            get { return Internal_GetLoop(unmanagedPtr); }
            set { Internal_SetLoop(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetLoop(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLoop(IntPtr obj, bool value);

        /// <summary>
        /// Determines whether the scene animation should auto play on game start.
        /// </summary>
        [EditorDisplay("Scene Animation", "Play On Start"), EditorOrder(50), DefaultValue(false)]
        [Tooltip("Determines whether the scene animation should auto play on game start.")]
        public bool PlayOnStart
        {
            get { return Internal_GetPlayOnStart(unmanagedPtr); }
            set { Internal_SetPlayOnStart(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetPlayOnStart(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPlayOnStart(IntPtr obj, bool value);

        /// <summary>
        /// Determines whether the scene animation should randomize the start time on play begin.
        /// </summary>
        [EditorDisplay("Scene Animation"), EditorOrder(60), DefaultValue(false)]
        [Tooltip("Determines whether the scene animation should randomize the start time on play begin.")]
        public bool RandomStartTime
        {
            get { return Internal_GetRandomStartTime(unmanagedPtr); }
            set { Internal_SetRandomStartTime(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetRandomStartTime(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRandomStartTime(IntPtr obj, bool value);

        /// <summary>
        /// Determines whether the scene animation should restore initial state on playback stop. State is cached when animation track starts play after being stopped (not paused).
        /// </summary>
        [EditorDisplay("Scene Animation", "Restore State On Stop"), EditorOrder(70), DefaultValue(false)]
        [Tooltip("Determines whether the scene animation should restore initial state on playback stop. State is cached when animation track starts play after being stopped (not paused).")]
        public bool RestoreStateOnStop
        {
            get { return Internal_GetRestoreStateOnStop(unmanagedPtr); }
            set { Internal_SetRestoreStateOnStop(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetRestoreStateOnStop(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRestoreStateOnStop(IntPtr obj, bool value);

        /// <summary>
        /// Gets the value that determinates whether the scene animation is playing.
        /// </summary>
        [Tooltip("The value that determinates whether the scene animation is playing.")]
        public bool IsPlaying
        {
            get { return Internal_IsPlaying(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsPlaying(IntPtr obj);

        /// <summary>
        /// Gets the value that determinates whether the scene animation is paused.
        /// </summary>
        [Tooltip("The value that determinates whether the scene animation is paused.")]
        public bool IsPaused
        {
            get { return Internal_IsPaused(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsPaused(IntPtr obj);

        /// <summary>
        /// Gets the value that determinates whether the scene animation is stopped.
        /// </summary>
        [Tooltip("The value that determinates whether the scene animation is stopped.")]
        public bool IsStopped
        {
            get { return Internal_IsStopped(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsStopped(IntPtr obj);

        /// <summary>
        /// Gets or sets the current animation playback time position (seconds).
        /// </summary>
        [NoSerialize, HideInEditor]
        [Tooltip("The current animation playback time position (seconds).")]
        public float Time
        {
            get { return Internal_GetTime(unmanagedPtr); }
            set { Internal_SetTime(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTime(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTime(IntPtr obj, float value);

        /// <summary>
        /// Starts playing the animation. Has no effect if animation is already playing.
        /// </summary>
        public void Play()
        {
            Internal_Play(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Play(IntPtr obj);

        /// <summary>
        /// Pauses the animation. Has no effect if animation is not playing.
        /// </summary>
        public void Pause()
        {
            Internal_Pause(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Pause(IntPtr obj);

        /// <summary>
        /// Stops playing the animation. Has no effect if animation is already stopped.
        /// </summary>
        public void Stop()
        {
            Internal_Stop(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Stop(IntPtr obj);
    }
}
