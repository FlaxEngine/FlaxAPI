// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The prefab manager handles the prefabs creation, synchronization and serialization.
    /// </summary>
    [Tooltip("The prefab manager handles the prefabs creation, synchronization and serialization.")]
    public static unsafe partial class PrefabManager
    {
        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab)
        {
            return Internal_SpawnPrefab(FlaxEngine.Object.GetUnmanagedPtr(prefab));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_SpawnPrefab(IntPtr prefab);

        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="position">The spawn position in the world space.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Vector3 position)
        {
            return Internal_SpawnPrefab1(FlaxEngine.Object.GetUnmanagedPtr(prefab), ref position);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_SpawnPrefab1(IntPtr prefab, ref Vector3 position);

        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="position">The spawn position in the world space.</param>
        /// <param name="rotation">The spawn rotation (in world space).</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Vector3 position, Quaternion rotation)
        {
            return Internal_SpawnPrefab2(FlaxEngine.Object.GetUnmanagedPtr(prefab), ref position, ref rotation);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_SpawnPrefab2(IntPtr prefab, ref Vector3 position, ref Quaternion rotation);

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
            return Internal_SpawnPrefab3(FlaxEngine.Object.GetUnmanagedPtr(prefab), ref position, ref rotation, ref scale);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_SpawnPrefab3(IntPtr prefab, ref Vector3 position, ref Quaternion rotation, ref Vector3 scale);

        /// <summary>
        /// Spawns the instance of the prefab objects. Prefab will be spawned to the first loaded scene.
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="transform">The spawn transformation in the world space.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Transform transform)
        {
            return Internal_SpawnPrefab4(FlaxEngine.Object.GetUnmanagedPtr(prefab), ref transform);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_SpawnPrefab4(IntPtr prefab, ref Transform transform);

        /// <summary>
        /// Spawns the instance of the prefab objects. If parent actor is specified then created actors are fully initialized (OnLoad event and BeginPlay is called if parent actor is already during gameplay).
        /// </summary>
        /// <param name="prefab">The prefab asset.</param>
        /// <param name="parent">The parent actor to add spawned object instance. Can be null to just deserialize contents of the prefab.</param>
        /// <returns>The created actor (root) or null if failed.</returns>
        public static Actor SpawnPrefab(Prefab prefab, Actor parent)
        {
            return Internal_SpawnPrefab5(FlaxEngine.Object.GetUnmanagedPtr(prefab), FlaxEngine.Object.GetUnmanagedPtr(parent));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_SpawnPrefab5(IntPtr prefab, IntPtr parent);

        /// <summary>
        /// Creates the prefab asset from the given root actor. Saves it to the output file path.
        /// </summary>
        /// <param name="targetActor">The target actor (prefab root). It cannot be a scene but it can contain a scripts and/or full hierarchy of objects to save.</param>
        /// <param name="outputPath">The output asset path.</param>
        /// <param name="autoLink">True if auto-connect the target actor and related objects to the created prefab.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool CreatePrefab(Actor targetActor, string outputPath, bool autoLink)
        {
            return Internal_CreatePrefab(FlaxEngine.Object.GetUnmanagedPtr(targetActor), outputPath, autoLink);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CreatePrefab(IntPtr targetActor, string outputPath, bool autoLink);

        /// <summary>
        /// Applies the difference from the prefab object instance, saves the changes and synchronizes them with the active instances of the prefab asset.
        /// </summary>
        /// <remarks>
        /// Applies all the changes from not only the given actor instance but all actors created within that prefab instance.
        /// </remarks>
        /// <param name="instance">The modified instance.</param>
        /// <returns>True if data is failed to apply the changes, otherwise false.</returns>
        public static bool ApplyAll(Actor instance)
        {
            return Internal_ApplyAll(FlaxEngine.Object.GetUnmanagedPtr(instance));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ApplyAll(IntPtr instance);
    }
}
