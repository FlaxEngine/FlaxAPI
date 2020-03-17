// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Contains information about single atlas slot with sprite texture.
    /// </summary>
    [Tooltip("Contains information about single atlas slot with sprite texture.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct Sprite
    {
        /// <summary>
        /// The normalized area of the sprite in the atlas (in range [0;1]).
        /// </summary>
        [Tooltip("The normalized area of the sprite in the atlas (in range [0;1]).")]
        public Rectangle Area;

        /// <summary>
        /// The sprite name.
        /// </summary>
        [Tooltip("The sprite name.")]
        public string Name;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Handle to sprite atlas slot with a single sprite texture.
    /// </summary>
    [Tooltip("Handle to sprite atlas slot with a single sprite texture.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct SpriteHandle
    {
        /// <summary>
        /// The parent atlas.
        /// </summary>
        [Tooltip("The parent atlas.")]
        public SpriteAtlas Atlas;

        /// <summary>
        /// The atlas sprites array index.
        /// </summary>
        [Tooltip("The atlas sprites array index.")]
        public int Index;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Sprite atlas asset that contains collection of sprites combined into a single texture.
    /// </summary>
    /// <seealso cref="TextureBase" />
    [Tooltip("Sprite atlas asset that contains collection of sprites combined into a single texture.")]
    public unsafe partial class SpriteAtlas : TextureBase
    {
        /// <inheritdoc />
        protected SpriteAtlas() : base()
        {
        }

        /// <summary>
        /// List with all tiles in the sprite atlas.
        /// </summary>
        [Tooltip("List with all tiles in the sprite atlas.")]
        public Sprite[] Sprites
        {
            get { return Internal_GetSprites(unmanagedPtr, typeof(Sprite)); }
            set { Internal_SetSprites(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Sprite[] Internal_GetSprites(IntPtr obj, System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSprites(IntPtr obj, Sprite[] value);

        /// <summary>
        /// Gets the sprites count.
        /// </summary>
        [Tooltip("The sprites count.")]
        public int SpritesCount
        {
            get { return Internal_GetSpritesCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetSpritesCount(IntPtr obj);

        /// <summary>
        /// Gets the sprite data.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The sprite data.</returns>
        public Sprite GetSprite(int index)
        {
            Internal_GetSprite(unmanagedPtr, index, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSprite(IntPtr obj, int index, out Sprite resultAsRef);

        /// <summary>
        /// Sets the sprite data.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The sprite data.</param>
        /// <returns>The sprite handle.</returns>
        public void SetSprite(int index, ref Sprite value)
        {
            Internal_SetSprite(unmanagedPtr, index, ref value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSprite(IntPtr obj, int index, ref Sprite value);

        /// <summary>
        /// Finds the sprite by the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The sprite handle.</returns>
        public SpriteHandle FindSprite(string name)
        {
            Internal_FindSprite(unmanagedPtr, name, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_FindSprite(IntPtr obj, string name, out SpriteHandle resultAsRef);

        /// <summary>
        /// Adds the sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <returns>The sprite handle.</returns>
        public SpriteHandle AddSprite(Sprite sprite)
        {
            Internal_AddSprite(unmanagedPtr, ref sprite, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddSprite(IntPtr obj, ref Sprite sprite, out SpriteHandle resultAsRef);

        /// <summary>
        /// Removes the sprite.
        /// </summary>
        /// <param name="index">The sprite index.</param>
        public void RemoveSprite(int index)
        {
            Internal_RemoveSprite(unmanagedPtr, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemoveSprite(IntPtr obj, int index);

        /// <summary>
        /// Save the sprites (texture content won't be modified).
        /// </summary>
        /// <returns>True if cannot save, otherwise false.</returns>
        public bool SaveSprites()
        {
            return Internal_SaveSprites(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveSprites(IntPtr obj);
    }
}
