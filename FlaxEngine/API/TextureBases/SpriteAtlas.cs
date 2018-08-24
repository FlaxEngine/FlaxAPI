// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        /// Initializes a new instance of the <see cref="Sprite"/> struct.
        /// </summary>
        /// <param name="atlas">The atlas.</param>
        /// <param name="index">The index.</param>
        public Sprite(SpriteAtlas atlas, int index)
        {
            Atlas = atlas;
            Index = index;
        }

        /// <summary>
        /// Returns true if sprite is valid.
        /// </summary>
        /// <returns>True if this sprite handle is valid, otherwise false.</returns>
        public bool IsValid => Atlas != null && Index != -1;

        /// <summary>
        /// Gets or sets the sprite name.
        /// </summary>
        [NoSerialize]
        public string Name
        {
            get
            {
                if (Atlas == null)
                    throw new InvalidOperationException("Cannot use invalid sprite.");
                return SpriteAtlas.Internal_GetSpriteName(Atlas.unmanagedPtr, Index);
            }
            set
            {
                if (Atlas == null)
                    throw new InvalidOperationException("Cannot use invalid sprite.");
                SpriteAtlas.Internal_SetSpriteName(Atlas.unmanagedPtr, Index, value);
            }
        }

        /// <summary>
        /// Gets or sets the sprite location (in pixels).
        /// </summary>
        [NoSerialize]
        public Vector2 Location
        {
            get => Area.Location * Atlas.Size;
            set
            {
                var area = Area;
                area.Location = value / Atlas.Size;
                Area = area;
            }
        }

        /// <summary>
        /// Gets or sets the sprite size (in pixels).
        /// </summary>
        [NoSerialize]
        public Vector2 Size
        {
            get => Area.Size * Atlas.Size;
            set
            {
                var area = Area;
                area.Size = value / Atlas.Size;
                Area = area;
            }
        }

        /// <summary>
        /// Gets or sets the sprite area in atlas (in normalized atlas coordinates [0;1]).
        /// </summary>
        [NoSerialize]
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
            set
            {
                if (Atlas == null)
                    throw new InvalidOperationException("Cannot use invalid sprite.");
                SpriteAtlas.Internal_SetSpriteArea(Atlas.unmanagedPtr, Index, ref value);
            }
        }
    }

    public partial class SpriteAtlas
    {
        /// <summary>
        /// Gets the sprite at the given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The sprite</returns>
        public Sprite GetSprite(int index)
        {
            return new Sprite(this, index);
        }

        /// <summary>
        /// Gets the sprite area (normalized).
        /// </summary>
        /// <param name="index">The sprite index.</param>
        /// <param name="spriteArea">The result sprite area.</param>
        public void GetSpriteArea(int index, out Rectangle spriteArea)
        {
            Internal_GetSpriteArea(unmanagedPtr, index, out spriteArea);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetSpriteName(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSpriteName(IntPtr obj, int index, string value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSpriteArea(IntPtr obj, int index, out Rectangle resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSpriteArea(IntPtr obj, int index, ref Rectangle value);
#endif

        #endregion
    }
}
