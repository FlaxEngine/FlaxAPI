// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class ParticleSystem
    {
        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Vector3 position, bool autoDestroy = false)
        {
            return Spawn(null, new Transform(position), autoDestroy);
        }

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Vector3 position, Quaternion rotation, bool autoDestroy = false)
        {
            return Spawn(null, new Transform(position, rotation), autoDestroy);
        }

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="transform">The spawn transform.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Transform transform, bool autoDestroy = false)
        {
            return Spawn(null, transform, autoDestroy);
        }

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Vector3 position, bool autoDestroy = false)
        {
            return Spawn(parent, new Transform(position), autoDestroy);
        }

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
            return Spawn(parent, new Transform(position, rotation), autoDestroy);
        }

        /// <summary>
        /// Spawns the particles at the given location.
        /// </summary>
        /// <param name="parent">The parent actor (can be null to link it to the first loaded scene).</param>
        /// <param name="transform">The spawn transform.</param>
        /// <param name="autoDestroy">If set to <c>true</c> effect be be auto-destroyed after duration.</param>
        /// <returns>The spawned effect.</returns>
        public ParticleEffect Spawn(Actor parent, Transform transform, bool autoDestroy = false)
        {
            if (WaitForLoaded())
                throw new Exception("Failed to load " + ToString() + '.');

            var effect = ParticleEffect.New();
            effect.Transform = transform;
            effect.ParticleSystem = this;

            SceneManager.SpawnActor(effect, parent);

            if (autoDestroy)
                Destroy(effect, Duration);

            return effect;
        }
    }
}
