// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// The particles simulation execution mode.
    /// </summary>
    public enum ParticlesSimulationMode
    {
        /// <summary>
        /// The default model. Select the best simulation mode based on a target platform.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Runs particles simulation on a CPU (always supported).
        /// </summary>
        CPU = 1,

        /// <summary>
        /// Runs particles simulation on a GPU (if supported).
        /// </summary>
        GPU = 2,
    }

    /// <summary>
    /// The particles simulation space modes.
    /// </summary>
    public enum ParticlesSimulationSpace
    {
        /// <summary>
        /// Simulates particles in the world space.
        /// </summary>
        World = 0,

        /// <summary>
        /// Simulates particles in the local space of the actor.
        /// </summary>
        Local = 1,
    }

    public sealed partial class ParticleEmitter
    {
        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Vector3 position, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Spawn(null, new Transform(position), duration, autoDestroy);
        }

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
            return Spawn(null, new Transform(position, rotation), duration, autoDestroy);
        }

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="transform">The spawn transform.</param>
        /// <param name="duration">The effect playback duration (in seconds).</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Transform transform, float duration = float.MaxValue, bool autoDestroy = false)
        {
            return Spawn(null, transform, duration, autoDestroy);
        }

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
            return Spawn(parent, new Transform(position), duration, autoDestroy);
        }

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
            return Spawn(parent, new Transform(position, rotation), duration, autoDestroy);
        }

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
            if (WaitForLoaded())
                throw new Exception("Failed to load " + ToString() + '.');

            var system = Content.CreateVirtualAsset<ParticleSystem>();
            if (!system)
                throw new Exception("Failed to create virtual particle system.");
            system.Init(this, duration);

            var effect = ParticleEffect.New();
            effect.Transform = transform;
            effect.ParticleSystem = system;

            SceneManager.SpawnActor(effect, parent);

            if (autoDestroy && duration < float.MaxValue)
                Destroy(effect, duration);

            return effect;
        }
    }
}
