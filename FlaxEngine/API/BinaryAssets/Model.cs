////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class Model
    {
        /// <summary>
        /// The model asset type unique ID.
        /// </summary>
        public const int TypeID = 3;

        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.Model;

        private MaterialSlot[] _slots;
        private ModelLOD[] _lods;

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
        /// <returns>The material slot with the given name or null if cannot find it (asset may be not loaed yet).</returns>
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
        internal static extern int Internal_GetMeshMaterialSlotIndex(IntPtr obj, int lodIndex, int meshIndex);
#endif
    }
}
