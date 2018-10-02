// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class StaticModel
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
                    var slotsCount = model.MaterialSlotsCount;
                    _entries = new ModelEntryInfo[slotsCount];
                    for (int i = 0; i < slotsCount; i++)
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
        /// It's called on <see cref="StaticModel"/> model changed or when model asset gets reloaded, etc.
        /// </summary>
        public event Action<StaticModel> EntriesChanged;

        internal void Internal_OnModelChanged()
        {
            // Clear cached data
            _entries = null;

            EntriesChanged?.Invoke(this);
        }
    }
}
