// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public static partial class Content
    {
        // TODO: assets list get

        /// <summary>
        /// Occurs when asset is being disposed and will be unloaded (by force). All references to it should be released.
        /// </summary>
        public static event Action<Asset> AssetDisposing;

        internal static void Internal_AssetDisposing(Asset asset)
        {
            try
            {
                AssetDisposing?.Invoke(asset);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not created (see log for error info).
        /// </summary>
        /// <param name="id">Asset unique ID.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static T LoadAsync<T>(ref Guid id) where T : Asset
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return (T)Internal_LoadAsync1(ref id, typeof(T));
#endif
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// </summary>
        /// <param name="id">Asset unique ID.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Asset LoadAsync(Guid id)
        {
            return LoadAsync<Asset>(ref id);
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
        /// <param name="internalPath">Internal path to the asset. Relative to the Engine startup folder.</param>
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
        /// <param name="timeoutInMiliseconds">Custom timeout value in milliseconds.</param>
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
        /// <param name="timeoutInMiliseconds">Custom timeout value in milliseconds.</param>
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
        /// <param name="internalPath">Internal path to the asset. Relative to the Engine startup folder.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in milliseconds.</param>
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
        /// <param name="timeoutInMiliseconds">Custom timeout value in milliseconds.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        public static T Load<T>(Guid id, double timeoutInMiliseconds = 10000.0) where T : Asset
        {
            var asset = LoadAsync<T>(ref id);
            if (asset && asset.WaitForLoaded(timeoutInMiliseconds) == false)
                return asset;
            return null;
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// Waits until asset will be loaded. It's equivalent to LoadAsync + WaitForLoaded.
        /// </summary>
        /// <param name="path">Path to the asset.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in milliseconds.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        public static T Load<T>(string path, double timeoutInMiliseconds = 10000.0) where T : Asset
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
        /// <param name="internalPath">Internal path to the asset. Relative to the Engine startup folder and without an asset file extension.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in milliseconds.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        public static T LoadInternal<T>(string internalPath, double timeoutInMiliseconds = 10000.0) where T : Asset
        {
            var asset = LoadAsyncInternal<T>(internalPath);
            if (asset && asset.WaitForLoaded(timeoutInMiliseconds) == false)
                return asset;
            return null;
        }

        /// <summary>
        /// Find asset info by id.
        /// </summary>
        /// <param name="id">The unique asset ID.</param>
        /// <param name="typeName">If method returns true, this contains found asset type name.</param>
        /// <param name="path">If method returns true, this contains found asset path.</param>
        /// <returns>True if found any asset, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool GetAssetInfo(Guid id, out string typeName, out string path)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetAssetInfo1(ref id, out typeName, out path);
#endif
        }

        /// <summary>
        /// Find asset info by path.
        /// </summary>
        /// <param name="path">The asset file path (full path).</param>
        /// <param name="typeName">If method returns true, this contains found asset type name.</param>
        /// <param name="id">If method returns true, this contains found asset id.</param>
        /// <returns>True if found any asset, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool GetAssetInfo(string path, out string typeName, out Guid id)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetAssetInfo2(path, out typeName, out id);
#endif
        }

        /// <summary>
        /// Creates temporary and virtual asset of the given type.
        /// Virtual assets have limited usage but allow to use custom assets data at runtime.
        /// Virtual assets are temporary and exist until application exit.
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
            string typeName = typeof(T).FullName;
            if (typeof(T) != typeof(MaterialInstance) &&
                typeof(T) != typeof(Texture) &&
                typeof(T) != typeof(CubeTexture) &&
                typeof(T) != typeof(SpriteAtlas) &&
                typeof(T) != typeof(IESProfile) &&
                typeof(T) != typeof(SkinnedModel) &&
                typeof(T) != typeof(CollisionData) &&
                typeof(T) != typeof(Model))
                throw new InvalidOperationException("Asset type " + typeName + " does not support virtual assets.");

            return (T)Internal_CreateVirtualAsset(typeof(T), typeName);
#endif
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAssetInfo1(ref Guid id, out string typeName, out string path);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAssetInfo2(string path, out string typeName, out Guid id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Asset Internal_CreateVirtualAsset(Type type, string typeName);
#endif

        #endregion
    }
}
