// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class SkinnedModel
    {
        private MaterialSlot[] _slots;
        private SkinnedMesh[] _meshes;
        private SkeletonBone[] _bones;

        /// <summary>
        /// Gets the material slots colelction. Each slot contains information how to render mesh or meshes using it.
        /// </summary>
        public MaterialSlot[] MaterialSlots
        {
            get
            {
                if (_slots == null)
                {
                    // Ask unmanaged world for amount of material slots
                    int count = Model.Internal_GetSlots(unmanagedPtr);
                    if (count > 0)
                    {
                        _slots = new MaterialSlot[count];
                        for (int i = 0; i < count; i++)
                            _slots[i] = new MaterialSlot(this, i);
                    }
                }

                return _slots;
            }
            internal set
            {
                // TODO: implement setter and allow to modify the collection
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
        /// Gets the skinned meshes collection.
        /// </summary>
        public SkinnedMesh[] Meshes
        {
            get
            {
                if (_meshes == null)
                {
                    // Ask unmanaged world for amount of meshes
                    int count = MeshesCount;
                    if (count > 0)
                    {
                        _meshes = new SkinnedMesh[count];
                        for (int i = 0; i < count; i++)
                            _meshes[i] = new SkinnedMesh(this, i);
                    }
                }

                return _meshes;
            }
        }

        /// <summary>
        /// Gets the skeleton bones hierarchy.
        /// </summary>
        public SkeletonBone[] Skeleton
        {
            get
            {
                if (_bones == null)
                {
                    _bones = Internal_SetupBones(unmanagedPtr, typeof(SkeletonBone));
                }

                return _bones;
            }
        }

        internal void Internal_OnUnload()
        {
            // Clear cached data
            _slots = null;
            _meshes = null;
            _bones = null;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkeletonBone[] Internal_SetupBones(IntPtr obj, Type type);
#endif

        #endregion
    }
}
