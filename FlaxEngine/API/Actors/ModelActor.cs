////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class ModelActor
    {
        private ModelEntryInfo[] _entries;

        /// <summary>
        /// Gets the model entries collection. Each <see cref="ModelEntryInfo"/> contains data how to render meshes using this entry (transformation, material, shadows casting, etc.).
        /// </summary>
        /// <remarks>
        /// It's null if the <see cref="Model"/> property is null or asset is not loaded yet.
        /// </remarks>
        [Serialize]
        [EditorOrder(100), EditorDisplay("Entries", EditorDisplayAttribute.InlineStyle)]
        [MemberCollection(CanReorderItems = false, NotNullItems = true, ReadOnly = true)]
        public ModelEntryInfo[] Entries
        {
            get
            {
                // Check if has cached data
                if (_entries != null)
                    return _entries;

                // Cache data
                var model = Model;
                if (model && model.IsLoaded)
                {
                    var meshesCount = model.LODs[0].Meshes.Length;
                    _entries = new ModelEntryInfo[meshesCount];
                    for (int i = 0; i < meshesCount; i++)
                    {
                        _entries[i] = new ModelEntryInfo(this, i);
                    }
                }

                return _entries;
            }
            internal set
            {
                // Used by the serialization system
                
                _entries = value;

                EntriesChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Occurs when entries collection gets changed.
        /// It's called on <see cref="ModelActor"/> model changed or when model asset gets reloaded, etc.
        /// </summary>
        public event Action<ModelActor> EntriesChanged;

        internal void Internal_OnModelChanged()
        {
            // Clear cached data
            _entries = null;

            EntriesChanged?.Invoke(this);
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMeshTransform(IntPtr obj, int index, out Transform result);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshTransform(IntPtr obj, int index, ref Transform value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMeshMaterial(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshMaterial(IntPtr obj, int index, IntPtr value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMeshScaleInLightmap(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshScaleInLightmap(IntPtr obj, int index, float value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMeshVisible(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshVisible(IntPtr obj, int index, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetMeshShadowsMode(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshShadowsMode(IntPtr obj, int index, ShadowsCastingMode value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsEntry(IntPtr obj, int index, ref Ray ray, out float distance);
#endif
    }
}
