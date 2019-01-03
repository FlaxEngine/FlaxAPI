// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public sealed partial class PointLight
    {
        /// <summary>
        /// Gets the light scaled radius parameter.
        /// </summary>
        public float ScaledRadius => Scale.MaxValue * Radius;
    }
}
