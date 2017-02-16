////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Loads and manages assets that can be used by the Engine
    /// </summary>
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
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static Asset Load(Guid id)
        {
            return Load<Asset>(id);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// </summary>
        /// <param name="path">Path to the asset.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public static Asset Load(string path)
        {
            return Load<Asset>(path);
        }

        /// <summary>
        /// Loads asset to the Content Pool and holds it until it won't be referenced by any object. Returns null if asset was not loaded.
        /// </summary>
        /// <param name="internalPath">Intenral path to the asset. Relative to the Engine startup folder.</param>
        /// <returns>Asset instance if loaded, null otherwise</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public static Asset LoadInternal(string internalPath)
        {
            return LoadInternal<Asset>(internalPath);
        }
    }
}