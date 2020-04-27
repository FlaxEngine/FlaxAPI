// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Particle system contains a composition of particle emitters and playback information.
    /// </summary>
    [Tooltip("Particle system contains a composition of particle emitters and playback information.")]
    public unsafe partial class ParticleSystem : BinaryAsset
    {
        /// <inheritdoc />
        protected ParticleSystem() : base()
        {
        }

        /// <summary>
        /// The frames amount per second of the timeline animation.
        /// </summary>
        [Tooltip("The frames amount per second of the timeline animation.")]
        public float FramesPerSecond
        {
            get { return Internal_GetFramesPerSecond(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFramesPerSecond(IntPtr obj);

        /// <summary>
        /// The animation duration (in frames).
        /// </summary>
        [Tooltip("The animation duration (in frames).")]
        public int DurationFrames
        {
            get { return Internal_GetDurationFrames(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetDurationFrames(IntPtr obj);

        /// <summary>
        /// The animation duration (in seconds).
        /// </summary>
        [Tooltip("The animation duration (in seconds).")]
        public float Duration
        {
            get { return Internal_GetDuration(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDuration(IntPtr obj);

        /// <summary>
        /// Initializes the particle system that plays a single particles emitter. This can be used only for virtual assets.
        /// </summary>
        /// <param name="emitter">The emitter to playback.</param>
        /// <param name="duration">The timeline animation duration in seconds.</param>
        /// <param name="fps">The amount of frames per second of the timeline animation.</param>
        public void Init(ParticleEmitter emitter, float duration, float fps = 60.0f)
        {
            Internal_Init(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(emitter), duration, fps);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Init(IntPtr obj, IntPtr emitter, float duration, float fps);

        /// <summary>
        /// Loads the serialized timeline data.
        /// </summary>
        /// <returns>The output surface data, or empty if failed to load.</returns>
        public byte[] LoadTimeline()
        {
            return Internal_LoadTimeline(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_LoadTimeline(IntPtr obj);

        /// <summary>
        /// Saves the serialized timeline data to the asset.
        /// </summary>
        /// <param name="data">The timeline data container.</param>
        /// <returns><c>true</c> failed to save data; otherwise, <c>false</c>.</returns>
        public bool SaveTimeline(byte[] data)
        {
            return Internal_SaveTimeline(unmanagedPtr, data);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveTimeline(IntPtr obj, byte[] data);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Vector3 position, bool autoDestroy = false)
        {
            return Internal_Spawn(unmanagedPtr, ref position, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn(IntPtr obj, ref Vector3 position, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Vector3 position, Quaternion rotation, bool autoDestroy = false)
        {
            return Internal_Spawn1(unmanagedPtr, ref position, ref rotation, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn1(IntPtr obj, ref Vector3 position, ref Quaternion rotation, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="transform">The spawn transform.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Transform transform, bool autoDestroy = false)
        {
            return Internal_Spawn2(unmanagedPtr, ref transform, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn2(IntPtr obj, ref Transform transform, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Vector3 position, bool autoDestroy = false)
        {
            return Internal_Spawn3(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(parent), ref position, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn3(IntPtr obj, IntPtr parent, ref Vector3 position, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Vector3 position, Quaternion rotation, bool autoDestroy = false)
        {
            return Internal_Spawn4(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(parent), ref position, ref rotation, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn4(IntPtr obj, IntPtr parent, ref Vector3 position, ref Quaternion rotation, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="transform">The spawn transform.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Transform transform, bool autoDestroy = false)
        {
            return Internal_Spawn5(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(parent), ref transform, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn5(IntPtr obj, IntPtr parent, ref Transform transform, bool autoDestroy);
    }
}
