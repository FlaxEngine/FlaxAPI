// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Loads and manages assets.
    /// </summary>
    [Tooltip("Loads and manages assets.")]
    public static unsafe partial class Content
    {
        /// <summary>
        /// Gets amount of the assets (in memory).
        /// </summary>
        [Tooltip("Gets amount of the assets (in memory).")]
        public static int AssetCount
        {
            get { return Internal_GetAssetCount(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetAssetCount();

        /// <summary>
        /// Gets the assets (loaded or during load).
        /// </summary>
        [Tooltip("The assets (loaded or during load).")]
        public static Asset[] Assets
        {
            get { return Internal_GetAssets(typeof(Asset)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset[] Internal_GetAssets(System.Type resultArrayItemType0);

        /// <summary>
        /// Finds the asset info by id.
        /// </summary>
        /// <param name="id">The asset id.</param>
        /// <param name="info">The output asset info. Filled with valid values if method returns true.</param>
        /// <returns>True if found any asset, otherwise false.</returns>
        public static bool GetAssetInfo(Guid id, out AssetInfo info)
        {
            return Internal_GetAssetInfo(ref id, out info);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAssetInfo(ref Guid id, out AssetInfo info);

        /// <summary>
        /// Finds the asset info by path.
        /// </summary>
        /// <param name="path">The asset path.</param>
        /// <param name="info">The output asset info. Filled with valid values if method returns true.</param>
        /// <returns>True if found any asset, otherwise false.</returns>
        public static bool GetAssetInfo(string path, out AssetInfo info)
        {
            return Internal_GetAssetInfo1(path, out info);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAssetInfo1(string path, out AssetInfo info);

        /// <summary>
        /// Generates temporary asset path.
        /// </summary>
        /// <returns>Asset path for a temporary usage.</returns>
        public static string CreateTemporaryAssetPath()
        {
            return Internal_CreateTemporaryAssetPath();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_CreateTemporaryAssetPath();

        /// <summary>
        /// Loads asset and holds it until it won't be referenced by any object. Returns null if asset is missing. Actual asset data loading is performed on a other thread in async.
        /// </summary>
        /// <param name="id">Asset unique ID</param>
        /// <param name="type">The asset type. If loaded object has different type (excluding types derived from the given) the loading fails.</param>
        /// <returns>Loaded asset or null if cannot</returns>
        public static Asset LoadAsync(Guid id, System.Type type)
        {
            return Internal_LoadAsync(ref id, type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_LoadAsync(ref Guid id, System.Type type);

        /// <summary>
        /// Loads asset and holds it until it won't be referenced by any object. Returns null if asset is missing. Actual asset data loading is performed on a other thread in async.
        /// </summary>
        /// <param name="path">The path of the asset (absolute or relative to the current workspace directory).</param>
        /// <param name="type">The asset type. If loaded object has different type (excluding types derived from the given) the loading fails.</param>
        /// <returns>Loaded asset or null if cannot</returns>
        public static Asset LoadAsync(string path, System.Type type)
        {
            return Internal_LoadAsync1(path, type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_LoadAsync1(string path, System.Type type);

        /// <summary>
        /// Loads internal engine asset and holds it until it won't be referenced by any object. Returns null if asset is missing. Actual asset data loading is performed on a other thread in async.
        /// </summary>
        /// <param name="internalPath">The path of the asset relative to the engine internal content (excluding the extension).</param>
        /// <param name="type">The asset type. If loaded object has different type (excluding types derived from the given) the loading fails.</param>
        /// <returns>The loaded asset or null if failed.</returns>
        public static Asset LoadAsyncInternal(string internalPath, System.Type type)
        {
            return Internal_LoadAsyncInternal(internalPath, type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_LoadAsyncInternal(string internalPath, System.Type type);

        /// <summary>
        /// Finds the asset with at given path. Checks all loaded assets.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The found asset or null if not loaded.</returns>
        public static Asset GetAsset(string path)
        {
            return Internal_GetAsset(path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_GetAsset(string path);

        /// <summary>
        /// Finds the asset with given ID. Checks all loaded assets.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The found asset or null if not loaded.</returns>
        public static Asset GetAsset(Guid id)
        {
            return Internal_GetAsset1(ref id);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_GetAsset1(ref Guid id);

        /// <summary>
        /// Deletes the specified asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public static void DeleteAsset(Asset asset)
        {
            Internal_DeleteAsset(FlaxEngine.Object.GetUnmanagedPtr(asset));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DeleteAsset(IntPtr asset);

        /// <summary>
        /// Deletes the asset at the specified path.
        /// </summary>
        /// <param name="path">The asset path.</param>
        public static void DeleteAsset(string path)
        {
            Internal_DeleteAsset1(path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DeleteAsset1(string path);

        /// <summary>
        /// Renames the asset.
        /// </summary>
        /// <param name="oldPath">The old asset path.</param>
        /// <param name="newPath">The new asset path.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool RenameAsset(string oldPath, string newPath)
        {
            return Internal_RenameAsset(oldPath, newPath);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RenameAsset(string oldPath, string newPath);

        /// <summary>
        /// Unloads the specified asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public static void UnloadAsset(Asset asset)
        {
            Internal_UnloadAsset(FlaxEngine.Object.GetUnmanagedPtr(asset));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UnloadAsset(IntPtr asset);

        /// <summary>
        /// Creates temporary and virtual asset of thr given type.
        /// </summary>
        /// <param name="type">The asset type klass.</param>
        /// <returns>Created asset or null if failed.</returns>
        public static Asset CreateVirtualAsset(System.Type type)
        {
            return Internal_CreateVirtualAsset(type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_CreateVirtualAsset(System.Type type);
    }
}
