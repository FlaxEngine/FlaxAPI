////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Sprite handle contains basic information about a sprite.
    /// </summary>
    public struct Sprite
    {
        /// <summary>
        /// Invalid sprite handle.
        /// </summary>
        public static Sprite Invalid;

        /// <summary>
        /// The parent sprite atlas.
        /// </summary>
        public SpriteAtlas Atlas;

        /// <summary>
        /// The sprite index.
        /// </summary>
        public int Index;

        /// <summary>
        /// Returns true if sprite is valid.
        /// </summary>
        /// <returns>True if this sprite handle is valid, otherwise false.</returns>
        public bool IsValid => Atlas != null && Index != -1;
    }

    public partial class SpriteAtlas
    {
        // TODO: This was auto-generated template made to keep file structure flow strict. Please add your custom implementations if needed.
    }
}
