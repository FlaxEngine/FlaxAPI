// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Assets objects base class.
    /// </summary>
    public partial class Asset
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Path} ({GetType().Name})";
        }
    }
}
