////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public static partial class Content
    {
        // TODO: expose import files methods (only in editor?)
        // TODO: expose create assets methods (only in editor?)
        // TODO: expose create temporary asset path method
        // TODO: AssetsCount property
        // TODO: assets list get
        // TODO: GetAsset() methods
        // TODO: Delete, Rename, Unload APIs
        // TODO: CreateVirtualAsset method

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// </summary>
        /// <param name="id">Asset unique ID.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Asset LoadAsync(Guid id)
        {
            return LoadAsync<Asset>(id);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// </summary>
        /// <param name="path">Path to the asset.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Asset LoadAsync(string path)
        {
            return LoadAsync<Asset>(path);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// </summary>
        /// <param name="internalPath">Intenral path to the asset. Relative to the Engine startup folder.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Asset LoadAsyncInternal(string internalPath)
        {
            return LoadAsyncInternal<Asset>(internalPath);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// Waits until asset will be loaded. It's equivalent to LoadAsync + WaitForLoaded.
        /// </summary>
        /// <param name="id">Asset unique ID.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in miliseconds.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Asset Load(Guid id, double timeoutInMiliseconds = 10000.0)
        {
            return Load<Asset>(id, timeoutInMiliseconds);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// Waits until asset will be loaded. It's equivalent to LoadAsync + WaitForLoaded.
        /// </summary>
        /// <param name="path">Path to the asset.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in miliseconds.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Asset Load(string path, double timeoutInMiliseconds = 10000.0)
        {
            return Load<Asset>(path, timeoutInMiliseconds);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// Waits until asset will be loaded. It's equivalent to LoadAsync + WaitForLoaded.
        /// </summary>
        /// <param name="internalPath">Intenral path to the asset. Relative to the Engine startup folder.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in miliseconds.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Asset LoadInternal(string internalPath, double timeoutInMiliseconds = 10000.0)
        {
            return LoadInternal<Asset>(internalPath, timeoutInMiliseconds);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// Waits until asset will be loaded. It's equivalent to LoadAsync + WaitForLoaded.
        /// </summary>
        /// <param name="id">Asset unique ID.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in miliseconds.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        public static T Load <T>(Guid id, double timeoutInMiliseconds = 10000.0) where T : Asset
        {
            var asset = LoadAsync<T>(id);
            if (asset && asset.WaitForLoaded(timeoutInMiliseconds) == false)
                return asset;
            return null;
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// Waits until asset will be loaded. It's equivalent to LoadAsync + WaitForLoaded.
        /// </summary>
        /// <param name="path">Path to the asset.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in miliseconds.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        public static T Load <T>(string path, double timeoutInMiliseconds = 10000.0) where T : Asset
        {
            var asset = LoadAsync<T>(path);
            if (asset && asset.WaitForLoaded(timeoutInMiliseconds) == false)
                return asset;
            return null;
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// Waits until asset will be loaded. It's equivalent to LoadAsync + WaitForLoaded.
        /// </summary>
        /// <param name="internalPath">Intenral path to the asset. Relative to the Engine startup folder and without an asset file extension.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in miliseconds.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        public static T LoadInternal <T>(string internalPath, double timeoutInMiliseconds = 10000.0) where T : Asset
        {
            var asset = LoadAsyncInternal<T>(internalPath);
            if (asset && asset.WaitForLoaded(timeoutInMiliseconds) == false)
                return asset;
            return null;
        }

        /// <summary>
        /// Find asset info by id.
        /// </summary>
        /// <param name="id">The asset path (full path).</param>
        /// <param name="typeId">If method returns true, this contains found asset type id.</param>
        /// <param name="path">If method returns true, this contains found asset path.</param>
        /// <returns>True if found any asset, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool GetAssetInfo(Guid id, out int typeId, out string path)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetAssetInfo1(ref id, out typeId, out path);
#endif
        }

        /// <summary>
        /// Find asset info by path.
        /// </summary>
        /// <param name="path">The asset id.</param>
        /// <param name="typeId">If method returns true, this contains found asset type id.</param>
        /// <param name="id">If method returns true, this contains found asset id.</param>
        /// <returns>True if found any asset, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool GetAssetInfo(string path, out int typeId, out Guid id)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetAssetInfo2(path, out typeId, out id);
#endif
        }

        /// <summary>
        /// Creates temporary and virtual asset of the given type. Virtual assets have limited usage but allow to use custom assets data at runtime.
        /// </summary>
        /// <typeparam name="T">Type of the asset to create. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if created, null otherwise. See log for error message if need to.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static T CreateVirtualAsset<T>() where T : Asset
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            int typeId;
            if (typeof(T) == typeof(MaterialInstance))
                typeId = MaterialInstance.TypeID;
            else
                throw new InvalidOperationException("Asset type " + typeof(T).FullName + " does not support virtual assets.");

            return (T)Internal_CreateVirtualAsset(typeof(T), typeId);
#endif
        }

        #region Internal Calls
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAssetInfo1(ref Guid id, out int typeId, out string path);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAssetInfo2(string path, out int typeId, out Guid id);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_CreateVirtualAsset(Type type, int typeId);
#endif
        #endregion
    }
}
