// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.Rendering;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Interface that unifies input source textures, sprites, render targets, and any other brushes to be used in a more generic way.
    /// </summary>
    public interface IBrush
    {
        /// <summary>
        /// Gets the size of the image brush in pixels (if relevant).
        /// </summary>
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
    /// Implementation of <see cref="IBrush"/> for <see cref="FlaxEngine.Texture"/>.
    /// </summary>
    /// <seealso cref="IBrush" />
    public sealed class TextureBrush : IBrush
    {
        /// <summary>
        /// The texture.
        /// </summary>
        [ExpandGroups, Tooltip("The texture asset.")]
        public Texture Texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureBrush"/> class.
        /// </summary>
        public TextureBrush()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureBrush"/> struct.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public TextureBrush(Texture texture)
        {
            Texture = texture;
        }

        /// <inheritdoc />
        public Vector2 Size => Texture?.Size ?? Vector2.Zero;

        /// <inheritdoc />
        public void Draw(Rectangle rect, Color color, bool withAlpha = false)
        {
            Render2D.DrawTexture(Texture, rect, color, withAlpha);
        }
    }

    /// <summary>
    /// Implementation of <see cref="IBrush"/> for <see cref="FlaxEngine.Rendering.RenderTarget"/>.
    /// </summary>
    /// <seealso cref="IBrush" />
    public sealed class RenderTargetBrush : IBrush
    {
        /// <summary>
        /// The render target.
        /// </summary>
        [HideInEditor]
        public RenderTarget RenderTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBrush"/> class.
        /// </summary>
        public RenderTargetBrush()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBrush"/> struct.
        /// </summary>
        /// <param name="renderTarget">The render Target.</param>
        public RenderTargetBrush(RenderTarget renderTarget)
        {
            RenderTarget = renderTarget;
        }

        /// <inheritdoc />
        public Vector2 Size => RenderTarget?.Size ?? Vector2.Zero;

        /// <inheritdoc />
        public void Draw(Rectangle rect, Color color, bool withAlpha = false)
        {
            Render2D.DrawRenderTarget(RenderTarget, rect, color, withAlpha);
        }
    }

    /// <summary>
    /// Implementation of <see cref="IBrush"/> for <see cref="FlaxEngine.Sprite"/>.
    /// </summary>
    /// <seealso cref="IBrush" />
    public sealed class SpriteBrush : IBrush
    {
        /// <summary>
        /// The sprite.
        /// </summary>
        [ExpandGroups]
        public Sprite Sprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBrush"/> class.
        /// </summary>
        public SpriteBrush()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBrush"/> struct.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public SpriteBrush(Sprite sprite)
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
