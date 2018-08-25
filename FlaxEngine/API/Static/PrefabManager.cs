// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public static partial class PrefabManager
    {
        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab)
        {
            var scene = SceneManager.GetScene(0);
            if (scene == null)
                return null;
            return SpawnPrefab(prefab, scene);
        }

        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="position">The spawn position in the world space.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Vector3 position)
        {
            var instance = SpawnPrefab(prefab);
            if (instance)
            {
                instance.Position = position;
            }
            return instance;
        }

        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="position">The spawn position in the world space.</param>
        /// <param name="rotation">The spawn rotation (in world space).</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Vector3 position, Quaternion rotation)
        {
            var instance = SpawnPrefab(prefab);
            if (instance)
            {
                var transform = instance.Transform;
                transform.Translation = position;
                transform.Orientation = rotation;
                instance.Transform = transform;
            }
            return instance;
        }

        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="position">The spawn position in the world space.</param>
        /// <param name="rotation">The spawn rotation (in world space).</param>
        /// <param name="scale">The spawn scale.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var instance = SpawnPrefab(prefab);
            if (instance)
            {
                Transform transform;
                transform.Translation = position;
                transform.Orientation = rotation;
                transform.Scale = scale;
                instance.Transform = transform;
            }
            return instance;
        }

        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="transform">The spawn transformation in the world space.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Transform transform)
        {
            var instance = SpawnPrefab(prefab);
            if (instance)
            {
                instance.Transform = transform;
            }
            return instance;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ApplyAll(IntPtr instance);
#endif

        #endregion
    }
}
