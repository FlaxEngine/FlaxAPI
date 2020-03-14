// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    partial struct SpriteHandle
    {
        /// <summary>
        /// Invalid sprite handle.
        /// </summary>
        public static SpriteHandle Invalid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> struct.
        /// </summary>
        /// <param name="atlas">The atlas.</param>
        /// <param name="index">The index.</param>
        public SpriteHandle(SpriteAtlas atlas, int index)
        {
            Atlas = atlas;
            Index = index;
        }

        /// <summary>
        /// Returns true if sprite is valid.
        /// </summary>
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
                return Atlas.GetSprite(Index).Name;
            }
            set
            {
                if (Atlas == null)
                    throw new InvalidOperationException("Cannot use invalid sprite.");
                var sprite = Atlas.GetSprite(Index);
                sprite.Name = value;
                Atlas.SetSprite(Index, ref sprite);
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
                return Atlas.GetSprite(Index).Area;
            }
            set
            {
                if (Atlas == null)
                    throw new InvalidOperationException("Cannot use invalid sprite.");
                var sprite = Atlas.GetSprite(Index);
                sprite.Area = value;
                Atlas.SetSprite(Index, ref sprite);
            }
        }
    }

    /*partial class SpriteAtlas
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
    }*/
}
