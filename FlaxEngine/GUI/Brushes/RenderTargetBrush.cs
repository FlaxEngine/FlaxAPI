// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.Rendering;

namespace FlaxEngine.GUI
{
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
        public void Draw(Rectangle rect, Color color)
        {
            Render2D.DrawRenderTarget(RenderTarget, rect, color);
        }
    }
}
