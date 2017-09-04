////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Base class for texture previews. Draws a surface in the UI and supports view moving/zomming.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public abstract class TexturePreviewBase : ContainerControl
    {
        private Rectangle _textureRect;
        private Vector2 _lastMosuePos;
        private Vector2 _viewPos;
        private float _viewScale = 1.0f;
        private bool _isMouseDown;
        
        /// <summary>
        /// Gets a value indicating whether this viewport has loaded dependant assets.
        /// </summary>
        public virtual bool HasLoadedAssets => true;

        /// <inheritdoc />
        protected TexturePreviewBase()
        {
            DockStyle = DockStyle.Fill;
        }

        /// <summary>
        /// Moves the view to the center.
        /// </summary>
        public void CenterView()
        {
            _viewScale = 1.0f;
            _viewPos = Vector2.Zero;
        }

        /// <summary>
        /// Updates the texture rectangle.
        /// </summary>
        protected void UpdateTextureRect()
        {
            CalculateTextureRect(out _textureRect);
        }

        /// <summary>
        /// Calculates the texture rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        protected abstract void CalculateTextureRect(out Rectangle rect);

        /// <summary>
        /// Calculates the texture rect fr the given texture and the view size.
        /// </summary>
        /// <param name="textureSize">Size of the texture.</param>
        /// <param name="viewSize">Size of the view.</param>
        /// <param name="result">The result.</param>
        protected static void CalculateTextureRect(Vector2 textureSize, Vector2 viewSize, out Rectangle result)
        {
            Vector2 size = Vector2.Max(textureSize, Vector2.One);
            float aspectRatio = size.X / size.Y;
            float h = viewSize.X / aspectRatio;
            float w = viewSize.Y * aspectRatio;
            if (w > h)
            {
                float diff = (viewSize.Y - h) * 0.5f;
                result = new Rectangle(0, diff, viewSize.X, h);
            }
            else
            {
                float diff = (viewSize.X - w) * 0.5f;
                result = new Rectangle(diff, 0, w, viewSize.Y);
            }
        }
        
        /// <summary>
        /// Draws the texture.
        /// </summary>
        /// <param name="rect">The target texture view rectangle.</param>
        protected abstract void DrawTexture(ref Rectangle rect);

        /// <summary>
        /// Gets the texture view rect (scaled and offseted).
        /// </summary>
        protected Rectangle TextureViewRect => (_textureRect + _viewPos) * _viewScale;

        /// <inheritdoc />
        public override void Draw()
        {
            Render2D.PushClip(new Rectangle(Vector2.Zero, Size));

            // Calculate texture view rectangle
            UpdateTextureRect();
            var textureRect = TextureViewRect;

            // Call drawing
            DrawTexture(ref textureRect);

            Render2D.PopClip();

            base.Draw();
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            // Store mouse position
            _lastMosuePos = location;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check if mouse is down
            if (_isMouseDown)
            {
                // Calculate mouse delta
                Vector2 delta = location - _lastMosuePos;

                // Move view
                _viewPos += delta;
            }

            // Store mouse position
            _lastMosuePos = location;

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flag
            _isMouseDown = false;
            Cursor = CursorType.Default;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, int delta)
        {
            if (base.OnMouseWheel(location, delta))
                return true;

            // Zoom
            float prevScale = _viewScale;
            _viewScale = Mathf.Clamp(_viewScale + delta * 0.002f, 0.001f, 20.0f);

            // Move view to make use of the control much more soother
            //float coeff = (prevScale + (_viewScale - prevScale)) / prevScale;
            //_viewPos += (location * coeff - location) / _viewScale;
            //_viewPos += location / _viewScale;
            Vector2 sizeDelta = (_viewScale - prevScale) * _textureRect.Size;
            _viewPos += sizeDelta * 0.5f;

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Set flag
            _isMouseDown = true;
            _lastMosuePos = location;
            Cursor = CursorType.SizeAll;

            base.OnMouseDown(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Clear flag
            _isMouseDown = false;
            Cursor = CursorType.Default;

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(Vector2 size)
        {
            base.SetSizeInternal(size);

            // Update texture rectangle and move view to the center
            UpdateTextureRect();
            CenterView();
        }
    }

    /// <summary>
    /// Texture preview GUI control. Draws <see cref="FlaxEngine.Texture"/> in the UI and supports view moving/zomming.
    /// </summary>
    /// <seealso cref="TexturePreviewBase" />
    public class TexturePreview : TexturePreviewBase
    {
        private Texture _asset;

        /// <summary>
        /// Gets or sets the texture being previews.
        /// </summary>
        public Texture Texture
        {
            get => _asset;
            set
            {
                if (_asset != value)
                {
                    _asset = value;
                    UpdateTextureRect();
                }
            }
        }
        
        /// <inheritdoc />
        protected override void CalculateTextureRect(out Rectangle rect)
        {
            CalculateTextureRect(_asset?.Size ?? new Vector2(100), Size, out rect);
        }

        /// <inheritdoc />
        protected override void DrawTexture(ref Rectangle rect)
        {
            // Background
            Render2D.FillRectangle(rect, Color.Gray);

            // Check if has loaded asset
            if (_asset && _asset.IsLoaded)
            {
                var texture = _asset;

                Render2D.DrawTexture(texture, rect, Color.White);

                // Check if texture is fully loaded
                // TODO: we should request full res texture during preview
                /*if (asset->IsLoaded())
                {
                    // Draw texture
                    render->DrawCustom(texture, rect, _psDefault, Color::White);
                }
                else
                {
                    // Texture is not fully loaded
                    render->DrawTexture(texture, rect, Color::White, true);
                }*/
            }
        }
    }
}
