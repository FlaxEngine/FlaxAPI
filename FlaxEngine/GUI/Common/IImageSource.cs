// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.Rendering;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Interface that unifies input source textures, sprites and render targets to be used in a more generic way.
    /// </summary>
    public interface IImageSource
    {
        /// <summary>
        /// Gets the size of the image in pixels.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        Vector2 Size { get; }

        /// <summary>
        /// Draws the specified image using <see cref="Render2D"/> graphics backend.
        /// </summary>
        /// <param name="rect">The draw area rectangle.</param>
        /// <param name="color">The color.</param>
        /// <param name="withAlpha">True if use alpha blending, otherwise it will be disabled.</param>
        void Draw(Rectangle rect, Color color, bool withAlpha = false);
    }

    /// <summary>
    /// Implementation of <see cref="IImageSource"/> for <see cref="FlaxEngine.Texture"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.IImageSource" />
    public sealed class TextureImageSource : IImageSource
    {
        /// <summary>
        /// The texture.
        /// </summary>
        [ExpandGroups, Tooltip("The texture asset.")]
        public Texture Texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureImageSource"/> class.
        /// </summary>
        public TextureImageSource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureImageSource"/> struct.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public TextureImageSource(Texture texture)
        {
            Texture = texture;
        }

        /// <inheritdoc />
        public Vector2 Size => Texture ? Texture.Size : Vector2.Zero;

        /// <inheritdoc />
        public void Draw(Rectangle rect, Color color, bool withAlpha = false)
        {
            Render2D.DrawTexture(Texture, rect, color, withAlpha);
        }
    }

    /// <summary>
    /// Implementation of <see cref="IImageSource"/> for <see cref="FlaxEngine.Rendering.RenderTarget"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.IImageSource" />
    public sealed class RenderTargetImageSource : IImageSource
    {
        /// <summary>
        /// The render target.
        /// </summary>
        [HideInEditor]
        public RenderTarget RenderTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetImageSource"/> class.
        /// </summary>
        public RenderTargetImageSource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetImageSource"/> struct.
        /// </summary>
        /// <param name="renderTarget">The render Target.</param>
        public RenderTargetImageSource(RenderTarget renderTarget)
        {
            RenderTarget = renderTarget;
        }

        /// <inheritdoc />
        public Vector2 Size => RenderTarget ? RenderTarget.Size : Vector2.Zero;

        /// <inheritdoc />
        public void Draw(Rectangle rect, Color color, bool withAlpha = false)
        {
            Render2D.DrawRenderTarget(RenderTarget, rect, color, withAlpha);
        }
    }

    /// <summary>
    /// Implementation of <see cref="IImageSource"/> for <see cref="FlaxEngine.Sprite"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.IImageSource" />
    public sealed class SpriteImageSource : IImageSource
    {
        /// <summary>
        /// The sprite.
        /// </summary>
        [ExpandGroups]
        public Sprite Sprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteImageSource"/> class.
        /// </summary>
        public SpriteImageSource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteImageSource"/> struct.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public SpriteImageSource(Sprite sprite)
        {
            Sprite = sprite;
        }

        /// <inheritdoc />
        public Vector2 Size => Sprite.IsValid ? Sprite.Size : Vector2.Zero;

        /// <inheritdoc />
        public void Draw(Rectangle rect, Color color, bool withAlpha = false)
        {
            Render2D.DrawSprite(Sprite, rect, color, withAlpha);
        }
    }
}
