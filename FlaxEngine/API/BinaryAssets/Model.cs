// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class Model
    {
        private MaterialSlot[] _slots;
        private ModelLOD[] _lods;

        /// <summary>
        /// The maximum amount of levels of detail for the model.
        /// </summary>
        public const int MaxLODs = 6;

        /// <summary>
        /// The maximum amount of meshes per model LOD.
        /// </summary>
        public const int MaxMeshes = 4096;

        /// <summary>
        /// Gets the material slots colelction. Each slot contains information how to render mesh or meshes using it. See <see cref="Mesh.MaterialSlotIndex"/>.
        /// </summary>
        public MaterialSlot[] MaterialSlots
        {
            get
            {
                if (_slots == null)
                    CacheData();
                return _slots;
            }
            internal set
            {
                // TODO: implement setter and allow to modify the colelction
            }
        }

        /// <summary>
        /// Gets the material slot by the name.
        /// </summary>
        /// <param name="name">The slot name.</param>
        /// <returns>The material slot with the given name or null if cannot find it (asset may be not loaded yet).</returns>
        public MaterialSlot GetSlot(string name)
        {
            MaterialSlot result = null;
            var slots = MaterialSlots;
            if (slots != null)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (string.Equals(slots[i].Name, name, StringComparison.Ordinal))
                    {
                        result = slots[i];
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the model level of details collection. Each level of detail contains array of meshes.
        /// </summary>
        public ModelLOD[] LODs
        {
            get
            {
                if (_lods == null)
                    CacheData();
                return _lods;
            }
        }

        /// <summary>
        /// Setups the model LODs collection including meshes creation.
        /// </summary>
        /// <remarks>
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// </remarks>
        /// <param name="meshesCountPerLod">The meshes count per LOD. Each model LOD contains a collection of meshes which has to be specified.</param>
        public void SetupLODs(params int[] meshesCountPerLod)
        {
            // Validate state and input
            if (!IsVirtual)
                throw new InvalidOperationException("Only virtual models can be modified at runtime.");
            if (meshesCountPerLod == null || meshesCountPerLod.Length == 0 || meshesCountPerLod.Length > MaxLODs)
                throw new ArgumentOutOfRangeException(nameof(meshesCountPerLod));
            for (int lodIndex = 0; lodIndex < meshesCountPerLod.Length; lodIndex++)
            {
                if (meshesCountPerLod[lodIndex] <= 0 || meshesCountPerLod[lodIndex] > MaxMeshes)
                    throw new ArgumentException("Too many meshes per LOD.");
            }

            // Cleanup data
            _lods = null;

            // Call backend
            if (Internal_SetupLODs(unmanagedPtr, meshesCountPerLod))
                throw new FlaxException("Failed to update model LODs collection.");
        }

        private void CacheData()
        {
            // Ask unmanaged world for amount of material slots
            int slotsCount = Internal_GetSlots(unmanagedPtr);
            if (slotsCount > 0)
            {
                _slots = new MaterialSlot[slotsCount];
                for (int i = 0; i < slotsCount; i++)
                    _slots[i] = new MaterialSlot(this, i);
            }

            // Ask unmanaged world for array with mesh count per lod
            var lodsSizes = Internal_GetLODs(unmanagedPtr);
            if (lodsSizes != null)
            {
                _lods = new ModelLOD[lodsSizes.Length];
                for (int i = 0; i < lodsSizes.Length; i++)
                {
                    _lods[i] = new ModelLOD(this, i, lodsSizes[i]);
                }
            }
        }

        internal void Internal_OnUnload()
        {
            // Clear cached data
            _slots = null;
            _lods = null;
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetSlots(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int[] Internal_GetLODs(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupLODs(IntPtr obj, int[] meshesCountPerLod);
#endif
    }
}
