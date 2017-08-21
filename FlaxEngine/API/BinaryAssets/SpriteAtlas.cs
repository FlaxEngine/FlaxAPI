////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Gets the sprite size (in pixels).
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Vector2 Size
        {
            get
            {
                if (Atlas == null)
                    throw new InvalidOperationException("Cannot use invalid sprite.");
                Vector2 result;
                SpriteAtlas.Internal_GetSpriteSize(Atlas.unmanagedPtr, Index, out result);
                return result;
            }
        }

        /// <summary>
        /// Gets the sprite area in atlas (in normalized atlas coordinaes [0;1]).
        /// </summary>
        /// <value>
        /// The sprite area in atlas (normalized).
        /// </value>
        public Rectangle Area
        {
            get
            {
                if (Atlas == null)
                    throw new InvalidOperationException("Cannot use invalid sprite.");
                Rectangle result;
                SpriteAtlas.Internal_GetSpriteArea(Atlas.unmanagedPtr, Index, out result);
                return result;
            }
        }
    }

    public partial class SpriteAtlas
    {
        /// <summary>
        /// The sprite atlas asset type unique ID.
        /// </summary>
        public const int TypeID = 10;

        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.Texture;

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSpriteSize(IntPtr obj, int index, out Vector2 resultAsRef);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSpriteArea(IntPtr obj, int index, out Rectangle resultAsRef);
#endif

        #endregion
    }
}
