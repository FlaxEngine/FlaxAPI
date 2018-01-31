////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    public sealed partial class BoxBrush
    {
        private BrushSurface[] _surfaces;

        /// <summary>
        /// Gets the brush surfaces collection.
        /// </summary>
        [Serialize]
        [EditorOrder(100), EditorDisplay("Surfaces", EditorDisplayAttribute.InlineStyle)]
        [MemberCollection(CanReorderItems = false, NotNullItems = true, ReadOnly = true)]
        public BrushSurface[] Surfaces
        {
            get
            {
                if (_surfaces == null)
                {
                    _surfaces = new BrushSurface[6];
                    for (int i = 0; i < _surfaces.Length; i++)
                    {
                        _surfaces[i] = new BrushSurface(this, i);
                    }
                }
                return _surfaces;
            }
            internal set
            {
                // Used by the serialization system

                _surfaces = value;
            }
        }
    }
}
