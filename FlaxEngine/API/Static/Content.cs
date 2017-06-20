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
        public static T Load<T>(Guid id, double timeoutInMiliseconds = 10000.0) where T : Asset
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
        /// <param name="internalPath">Intenral path to the asset. Relative to the Engine startup folder and without an asset file extension.</param>
        /// <param name="timeoutInMiliseconds">Custom timeout value in miliseconds.</param>
        /// <typeparam name="T">Type of the asset to load. Includes any asset types derived from the type.</typeparam>
        /// <returns>Asset instance if loaded, null otherwise</returns>
        public static T LoadInternal<T>(string internalPath, double timeoutInMiliseconds = 10000.0) where T : Asset
        {
            var asset = LoadAsyncInternal<T>(internalPath);
            if (asset && asset.WaitForLoaded(timeoutInMiliseconds) == false)
                return asset;
            return null;
        }
    }
}
