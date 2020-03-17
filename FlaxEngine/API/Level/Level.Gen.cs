// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The scene manager that contains the loaded scenes collection and spawns/deleted actors.
    /// </summary>
    [Tooltip("The scene manager that contains the loaded scenes collection and spawns/deleted actors.")]
    public static unsafe partial class Level
    {
        /// <summary>
        /// The loaded scenes collection.
        /// </summary>
        [Tooltip("The loaded scenes collection.")]
        public static Scene[] Scenes
        {
            get { return Internal_GetScenes(typeof(Scene)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Scene[] Internal_GetScenes(System.Type resultArrayItemType0);

        /// <summary>
        /// Checks if any scene has been loaded. Loaded scene means deserialized and added to the scenes collection.
        /// </summary>
        [Tooltip("Checks if any scene has been loaded. Loaded scene means deserialized and added to the scenes collection.")]
        public static bool IsAnySceneLoaded
        {
            get { return Internal_IsAnySceneLoaded(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAnySceneLoaded();

        /// <summary>
        /// Checks if any scene has any actor
        /// </summary>
        [Tooltip("Checks if any scene has any actor")]
        public static bool IsAnyActorInGame
        {
            get { return Internal_IsAnyActorInGame(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAnyActorInGame();

        /// <summary>
        /// Checks if any scene action is pending
        /// </summary>
        [Tooltip("Checks if any scene action is pending")]
        public static bool IsAnyActionPending
        {
            get { return Internal_IsAnyActionPending(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAnyActionPending();

        /// <summary>
        /// Gets the last scene load time (in UTC).
        /// </summary>
        [Tooltip("The last scene load time (in UTC).")]
        public static DateTime LastSceneLoadTime
        {
            get { Internal_GetLastSceneLoadTime(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLastSceneLoadTime(out DateTime resultAsRef);

        /// <summary>
        /// Gets the scenes count.
        /// </summary>
        [Tooltip("The scenes count.")]
        public static int ScenesCount
        {
            get { return Internal_GetScenesCount(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetScenesCount();

        /// <summary>
        /// Gets the scene.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The scene object (loaded).</returns>
        public static Scene GetScene(int index)
        {
            return Internal_GetScene(index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Scene Internal_GetScene(int index);

        /// <summary>
        /// Spawn actor on the scene
        /// </summary>
        /// <param name="actor">Actor to spawn</param>
        /// <returns>True if action cannot be done, otherwise false.</returns>
        public static bool SpawnActor(Actor actor)
        {
            return Internal_SpawnActor(FlaxEngine.Object.GetUnmanagedPtr(actor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SpawnActor(IntPtr actor);

        /// <summary>
        /// Spawns actor on the scene.
        /// </summary>
        /// <param name="actor">The actor to spawn.</param>
        /// <param name="parent">The parent actor (will link spawned actor with this parent).</param>
        /// <returns>True if action cannot be done, otherwise false.</returns>
        public static bool SpawnActor(Actor actor, Actor parent)
        {
            return Internal_SpawnActor1(FlaxEngine.Object.GetUnmanagedPtr(actor), FlaxEngine.Object.GetUnmanagedPtr(parent));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SpawnActor1(IntPtr actor, IntPtr parent);

        /// <summary>
        /// Saves scene to the asset.
        /// </summary>
        /// <param name="scene">Scene to serialize.</param>
        /// <param name="prettyJson">True if use pretty Json format writer, otherwise will use the compact Json format writer that packs data to use less memory and perform the action faster.</param>
        /// <returns>True if action cannot be done, otherwise false.</returns>
        public static bool SaveScene(Scene scene, bool prettyJson = true)
        {
            return Internal_SaveScene(FlaxEngine.Object.GetUnmanagedPtr(scene), prettyJson);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveScene(IntPtr scene, bool prettyJson);

        /// <summary>
        /// Saves scene to the bytes.
        /// </summary>
        /// <param name="scene">Scene to serialize.</param>
        /// <param name="prettyJson">True if use pretty Json format writer, otherwise will use the compact Json format writer that packs data to use less memory and perform the action faster.</param>
        /// <returns>The result data or empty if failed.</returns>
        public static byte[] SaveSceneToBytes(Scene scene, bool prettyJson = true)
        {
            return Internal_SaveSceneToBytes(FlaxEngine.Object.GetUnmanagedPtr(scene), prettyJson, typeof(byte));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_SaveSceneToBytes(IntPtr scene, bool prettyJson, System.Type resultArrayItemType0);

        /// <summary>
        /// Saves scene to the asset. Done in the background.
        /// </summary>
        /// <param name="scene">Scene to serialize.</param>
        public static void SaveSceneAsync(Scene scene)
        {
            Internal_SaveSceneAsync(FlaxEngine.Object.GetUnmanagedPtr(scene));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SaveSceneAsync(IntPtr scene);

        /// <summary>
        /// Saves all scenes to the assets.
        /// </summary>
        /// <returns>True if action cannot be done, otherwise false.</returns>
        public static bool SaveAllScenes()
        {
            return Internal_SaveAllScenes();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveAllScenes();

        /// <summary>
        /// Saves all scenes to the assets. Done in the background.
        /// </summary>
        public static void SaveAllScenesAsync()
        {
            Internal_SaveAllScenesAsync();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SaveAllScenesAsync();

        /// <summary>
        /// Loads scene from the asset.
        /// </summary>
        /// <param name="id">Scene ID</param>
        /// <param name="autoInitialize">Enable/disable auto scene initialization, otherwise user should do it (in that situation scene is registered but not in a gameplay, call OnBeginPlay to start logic for it; it will deserialize scripts and references to the other objects).</param>
        /// <returns>True if loading cannot be done, otherwise false.</returns>
        public static bool LoadScene(Guid id, bool autoInitialize = true)
        {
            return Internal_LoadScene(ref id, autoInitialize);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_LoadScene(ref Guid id, bool autoInitialize);

        /// <summary>
        /// Loads scene from the bytes.
        /// </summary>
        /// <param name="data">The scene data to load.</param>
        /// <param name="autoInitialize">Enable/disable auto scene initialization, otherwise user should do it (in that situation scene is registered but not in a gameplay, call OnBeginPlay to start logic for it; it will deserialize scripts and references to the other objects).</param>
        /// <returns>Loaded scene object, otherwise null if cannot load data (then see log for more information).</returns>
        public static Scene LoadSceneFromBytes(byte[] data, bool autoInitialize = true)
        {
            return Internal_LoadSceneFromBytes(data, autoInitialize);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Scene Internal_LoadSceneFromBytes(byte[] data, bool autoInitialize);

        /// <summary>
        /// Loads scene from the asset. Done in the background.
        /// </summary>
        /// <param name="id">Scene ID</param>
        /// <returns>True if loading cannot be done, otherwise false.</returns>
        public static bool LoadSceneAsync(Guid id)
        {
            return Internal_LoadSceneAsync(ref id);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_LoadSceneAsync(ref Guid id);

        /// <summary>
        /// Unloads given scene.
        /// </summary>
        /// <returns>True if action cannot be done, otherwise false.</returns>
        public static bool UnloadScene(Scene scene)
        {
            return Internal_UnloadScene(FlaxEngine.Object.GetUnmanagedPtr(scene));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UnloadScene(IntPtr scene);

        /// <summary>
        /// Unloads given scene. Done in the background.
        /// </summary>
        public static void UnloadSceneAsync(Scene scene)
        {
            Internal_UnloadSceneAsync(FlaxEngine.Object.GetUnmanagedPtr(scene));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UnloadSceneAsync(IntPtr scene);

        /// <summary>
        /// Unloads all scenes.
        /// </summary>
        /// <returns>True if action cannot be done, otherwise false.</returns>
        public static bool UnloadAllScenes()
        {
            return Internal_UnloadAllScenes();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UnloadAllScenes();

        /// <summary>
        /// Unloads all scenes. Done in the background.
        /// </summary>
        public static void UnloadAllScenesAsync()
        {
            Internal_UnloadAllScenesAsync();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UnloadAllScenesAsync();

        /// <summary>
        /// Tries to find actor with the given ID. It's very fast O(1) lookup.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Found actor or null.</returns>
        public static Actor FindActor(Guid id)
        {
            return Internal_FindActor(ref id);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_FindActor(ref Guid id);

        /// <summary>
        /// Tries to find the actor with the given name.
        /// </summary>
        /// <param name="name">The name of the actor.</param>
        /// <returns>Found actor or null.</returns>
        public static Actor FindActor(string name)
        {
            return Internal_FindActor1(name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_FindActor1(string name);

        /// <summary>
        /// Tries to find the actor of the given type in all the loaded scenes.
        /// </summary>
        /// <param name="type">Type of the actor to search for. Includes any actors derived from the type.</param>
        /// <returns>Found actor or null.</returns>
        public static Actor FindActor(System.Type type)
        {
            return Internal_FindActor2(type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_FindActor2(System.Type type);

        /// <summary>
        /// Tries to find the script of the given type in all the loaded scenes.
        /// </summary>
        /// <param name="type">Type of the scri[t to search for. Includes any scripts derived from the type.</param>
        /// <returns>Found script or null.</returns>
        public static Script FindScript(System.Type type)
        {
            return Internal_FindScript(type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script Internal_FindScript(System.Type type);

        /// <summary>
        /// Tries to find scene with given ID.
        /// </summary>
        /// <param name="id">Scene id.</param>
        /// <returns>Found scene or null.</returns>
        public static Scene FindScene(Guid id)
        {
            return Internal_FindScene(ref id);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Scene Internal_FindScene(ref Guid id);
    }
}
