// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The font line info generated during text processing.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FontLineCache
    {
        /// <summary>
        /// The root position of the line (upper left corner).
        /// </summary>
        public Vector2 RootPos;

        /// <summary>
        /// The maximum line extent on the Y axis (down from RootPos).
        /// </summary>
        public Vector2 MaxYExtent;

        /// <summary>
        /// The line width.
        /// </summary>
        public float Width;

        /// <summary>
        /// The first character index (from the input text).
        /// </summary>
        public int FirstCharIndex;

        /// <summary>
        /// The last character index (from the input text).
        /// </summary>
        public int LastCharIndex;
    };

    /// <summary>
    /// Represents font object that can be using during text rendering (it uses Font Asset but with precached data for chosen font properties).
    /// </summary>
    public partial class Font
    {
        // TODO: provide GetCharacter APIs
        // TODO: expose CharacterEntry and ability to get per font character info
    }
}
