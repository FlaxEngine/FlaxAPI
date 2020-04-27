// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a source for emitting audio. Audio can be played spatially (gun shot), or normally (music). Each audio source must have an AudioClip to play - back, and it can also have a position in the case of spatial(3D) audio.
    /// </summary>
    /// <remarks>
    /// Whether or not an audio source is spatial is controlled by the assigned AudioClip.The volume and the pitch of a spatial audio source is controlled by its position and the AudioListener's position/direction/velocity.
    /// </remarks>
    [Tooltip("Represents a source for emitting audio. Audio can be played spatially (gun shot), or normally (music). Each audio source must have an AudioClip to play - back, and it can also have a position in the case of spatial(3D) audio.")]
    public unsafe partial class AudioSource : Actor
    {
        /// <inheritdoc />
        protected AudioSource() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="AudioSource"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static AudioSource New()
        {
            return Internal_Create(typeof(AudioSource)) as AudioSource;
        }

        /// <summary>
        /// The audio clip asset used as a source of the sound.
        /// </summary>
        [EditorOrder(10), DefaultValue(null), EditorDisplay("Audio Source")]
        [Tooltip("The audio clip asset used as a source of the sound.")]
        public AudioClip Clip
        {
            get { return Internal_GetClip(unmanagedPtr); }
            set { Internal_SetClip(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AudioClip Internal_GetClip(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetClip(IntPtr obj, IntPtr value);

        /// <summary>
        /// Gets the velocity of the source. Determines pitch in relation to AudioListener's position. Only relevant for spatial (3D) sources.
        /// </summary>
        [Tooltip("The velocity of the source. Determines pitch in relation to AudioListener's position. Only relevant for spatial (3D) sources.")]
        public Vector3 Velocity
        {
            get { Internal_GetVelocity(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVelocity(IntPtr obj, out Vector3 resultAsRef);

        /// <summary>
        /// Gets or sets the volume of the audio played from this source, in [0, 1] range.
        /// </summary>
        [EditorOrder(20), DefaultValue(1.0f), Limit(0, 1, 0.01f), EditorDisplay("Audio Source")]
        [Tooltip("The volume of the audio played from this source, in [0, 1] range.")]
        public float Volume
        {
            get { return Internal_GetVolume(unmanagedPtr); }
            set { Internal_SetVolume(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetVolume(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolume(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the pitch of the played audio. The default is 1.
        /// </summary>
        [EditorOrder(30), DefaultValue(1.0f), Limit(0.5f, 2.0f, 0.01f), EditorDisplay("Audio Source")]
        [Tooltip("The pitch of the played audio. The default is 1.")]
        public float Pitch
        {
            get { return Internal_GetPitch(unmanagedPtr); }
            set { Internal_SetPitch(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPitch(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPitch(IntPtr obj, float value);

        /// <summary>
        /// Determines whether the audio clip should loop when it finishes playing.
        /// </summary>
        [EditorOrder(40), DefaultValue(false), EditorDisplay("Audio Source")]
        [Tooltip("Determines whether the audio clip should loop when it finishes playing.")]
        public bool IsLooping
        {
            get { return Internal_GetIsLooping(unmanagedPtr); }
            set { Internal_SetIsLooping(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsLooping(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsLooping(IntPtr obj, bool value);

        /// <summary>
        /// Determines whether the audio clip should auto play on level start.
        /// </summary>
        [EditorOrder(50), DefaultValue(false), EditorDisplay("Audio Source", "Play On Start")]
        [Tooltip("Determines whether the audio clip should auto play on level start.")]
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
        /// Gets or sets the minimum distance at which audio attenuation starts. When the listener is closer to the source than this value, audio is heard at full volume. Once farther away the audio starts attenuating.
        /// </summary>
        [EditorOrder(60), DefaultValue(1.0f), Limit(0, float.MaxValue, 0.1f), EditorDisplay("Audio Source")]
        [Tooltip("The minimum distance at which audio attenuation starts. When the listener is closer to the source than this value, audio is heard at full volume. Once farther away the audio starts attenuating.")]
        public float MinDistance
        {
            get { return Internal_GetMinDistance(unmanagedPtr); }
            set { Internal_SetMinDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMinDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMinDistance(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the attenuation that controls how quickly does audio volume drop off as the listener moves further from the source.
        /// </summary>
        [EditorOrder(70), DefaultValue(1.0f), Limit(0, float.MaxValue, 0.1f), EditorDisplay("Audio Source")]
        [Tooltip("The attenuation that controls how quickly does audio volume drop off as the listener moves further from the source.")]
        public float Attenuation
        {
            get { return Internal_GetAttenuation(unmanagedPtr); }
            set { Internal_SetAttenuation(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetAttenuation(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAttenuation(IntPtr obj, float value);

        /// <summary>
        /// Gets the the current state of the audio playback (playing/paused/stopped).
        /// </summary>
        [Tooltip("The the current state of the audio playback (playing/paused/stopped).")]
        public States State
        {
            get { return Internal_GetState(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern States Internal_GetState(IntPtr obj);

        /// <summary>
        /// Gets or sets the current time of playback. If playback has not yet started, it specifies the time at which playback will start at. The time is in seconds, in range [0, ClipLength].
        /// </summary>
        [HideInEditor]
        [Tooltip("The current time of playback. If playback has not yet started, it specifies the time at which playback will start at. The time is in seconds, in range [0, ClipLength].")]
        public float Time
        {
            get { return Internal_GetTime(unmanagedPtr); }
            set { Internal_SetTime(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTime(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTime(IntPtr obj, float time);

        /// <summary>
        /// Returns true if the sound source is three dimensional (volume and pitch varies based on listener distance and velocity).
        /// </summary>
        [Tooltip("Returns true if the sound source is three dimensional (volume and pitch varies based on listener distance and velocity).")]
        public bool Is3D
        {
            get { return Internal_Is3D(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Is3D(IntPtr obj);

        /// <summary>
        /// Returns true if audio clip is valid, loaded and uses dynamic data streaming.
        /// </summary>
        [Tooltip("Returns true if audio clip is valid, loaded and uses dynamic data streaming.")]
        public bool UseStreaming
        {
            get { return Internal_UseStreaming(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UseStreaming(IntPtr obj);

        /// <summary>
        /// Determines whether this audio source started playing audio via audio backend. After audio play it may wait for audio clip data to be loaded or streamed.
        /// </summary>
        [Tooltip("Determines whether this audio source started playing audio via audio backend. After audio play it may wait for audio clip data to be loaded or streamed.")]
        public bool IsActuallyPlayingSth
        {
            get { return Internal_IsActuallyPlayingSth(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsActuallyPlayingSth(IntPtr obj);

        /// <summary>
        /// Starts playing the currently assigned audio clip.
        /// </summary>
        public void Play()
        {
            Internal_Play(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Play(IntPtr obj);

        /// <summary>
        /// Pauses the audio playback.
        /// </summary>
        public void Pause()
        {
            Internal_Pause(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Pause(IntPtr obj);

        /// <summary>
        /// Stops audio playback, rewinding it to the start.
        /// </summary>
        public void Stop()
        {
            Internal_Stop(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Stop(IntPtr obj);

        /// <summary>
        /// Valid states in which AudioSource can be in.
        /// </summary>
        [Tooltip("Valid states in which AudioSource can be in.")]
        public enum States
        {
            /// <summary>
            /// The source is currently playing.
            /// </summary>
            [Tooltip("The source is currently playing.")]
            Playing = 0,

            /// <summary>
            /// The source is currently paused (play will resume from paused point).
            /// </summary>
            [Tooltip("The source is currently paused (play will resume from paused point).")]
            Paused = 1,

            /// <summary>
            /// The source is currently stopped (play will resume from start).
            /// </summary>
            [Tooltip("The source is currently stopped (play will resume from start).")]
            Stopped = 2
    ,
        }
    }
}
