// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Binary asset that contains a particle emitter definition graph for running particles simulation on CPU and GPU.
    /// </summary>
    [Tooltip("Binary asset that contains a particle emitter definition graph for running particles simulation on CPU and GPU.")]
    public unsafe partial class ParticleEmitter : BinaryAsset
    {
        /// <inheritdoc />
        protected ParticleEmitter() : base()
        {
        }

        /// <summary>
        /// Tries to load surface graph from the asset.
        /// </summary>
        /// <param name="createDefaultIfMissing">True if create default surface if missing.</param>
        /// <returns>The output surface data, or empty if failed to load.</returns>
        public byte[] LoadSurface(bool createDefaultIfMissing)
        {
            return Internal_LoadSurface(unmanagedPtr, createDefaultIfMissing);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_LoadSurface(IntPtr obj, bool createDefaultIfMissing);

        /// <summary>
        /// Updates surface (saves new one, discard cached data, reloads asset).
        /// </summary>
        /// <param name="data">The surface graph data.</param>
        /// <returns>True if cannot save it, otherwise false.</returns>
        public bool SaveSurface(byte[] data)
        {
            return Internal_SaveSurface(unmanagedPtr, data);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveSurface(IntPtr obj, byte[] data);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Vector3 position, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Internal_Spawn(unmanagedPtr, ref position, duration, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn(IntPtr obj, ref Vector3 position, float duration, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Vector3 position, Quaternion rotation, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Internal_Spawn1(unmanagedPtr, ref position, ref rotation, duration, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn1(IntPtr obj, ref Vector3 position, ref Quaternion rotation, float duration, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="transform">The spawn transform.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Transform transform, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Internal_Spawn2(unmanagedPtr, ref transform, duration, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn2(IntPtr obj, ref Transform transform, float duration, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Vector3 position, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Internal_Spawn3(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(parent), ref position, duration, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn3(IntPtr obj, IntPtr parent, ref Vector3 position, float duration, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Vector3 position, Quaternion rotation, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Internal_Spawn4(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(parent), ref position, ref rotation, duration, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn4(IntPtr obj, IntPtr parent, ref Vector3 position, ref Quaternion rotation, float duration, bool autoDestroy);

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="transform">The spawn transform.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Transform transform, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Internal_Spawn5(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(parent), ref transform, duration, autoDestroy);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect Internal_Spawn5(IntPtr obj, IntPtr parent, ref Transform transform, float duration, bool autoDestroy);
    }
}
