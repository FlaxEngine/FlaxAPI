// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public sealed partial class SkyLight
    {
        /// <summary>
        /// Sky light source mode.
        /// </summary>
        public enum Modes
        {
            /// <summary>
            /// The captured scene will be used as a light source.
            /// </summary>
            CaptureScene = 0,

            /// <summary>
            /// The custom cube texture will be used as a light source.
            /// </summary>
            CustomTexture = 1,
        }
    }
}
