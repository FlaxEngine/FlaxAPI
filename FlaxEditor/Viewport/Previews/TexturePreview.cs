////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Viewport.Widgets;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

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
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            if (base.OnMouseWheel(location, delta))
                return true;

            // Zoom
            float prevScale = _viewScale;
            _viewScale = Mathf.Clamp(_viewScale + delta * 0.24f, 0.001f, 20.0f);

            // Move view to make use of the control much more soother
            //float coeff = (prevScale + (_viewScale - prevScale)) / prevScale;
            //_viewPos += (location * coeff - location) / _viewScale;
            //_viewPos += location / _viewScale;
            Vector2 sizeDelta = (_viewScale - prevScale) * _textureRect.Size;
            _viewPos += sizeDelta * 0.5f;

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            // Set flag
            _isMouseDown = true;
            _lastMosuePos = location;
            Cursor = CursorType.SizeAll;

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;

            // Clear flag
            _isMouseDown = false;
            Cursor = CursorType.Default;

            return true;
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
    /// Base class for texture previews with custom drawing features. Uses in-build postFx material to render a texture.
    /// </summary>
    /// <seealso cref="TexturePreviewBase" />
    public abstract class TexturePreviewCustomBase : TexturePreviewBase
    {
        /// <summary>
        /// Texture channel flags.
        /// </summary>
        [Flags]
        public enum ChannelFlags
        {
            /// <summary>
            /// The none.
            /// </summary>
            None = 0,

            /// <summary>
            /// The red channel.
            /// </summary>
            Red = 1,

            /// <summary>
            /// The green channel.
            /// </summary>
            Green = 2,

            /// <summary>
            /// The blue channel.
            /// </summary>
            Blue = 4,

            /// <summary>
            /// The alpha channel.
            /// </summary>
            Alpha = 8,

            /// <summary>
            /// All texture channels.
            /// </summary>
            All = Red | Green | Blue | Alpha
        }

        private ChannelFlags _channelFlags = ChannelFlags.All;

        /// <summary>
        /// The preview material instance used to draw texture.
        /// </summary>
        protected MaterialInstance _previewMaterial;

        /// <summary>
        /// Gets or sets the view channels to show.
        /// </summary>
        public ChannelFlags ViewChannels
        {
            get => _channelFlags;
            set
            {
                if (_channelFlags != value)
                {
                    _channelFlags = value;
                    UpdateMask();
                }
            }
        }

        /// <inheritdoc />
        /// <param name="useWidgets">True if show viewport widgets.</param>
        protected TexturePreviewCustomBase(bool useWidgets)
        {
            // Create preview material (virtual)
            var baseMaterial = FlaxEngine.Content.LoadAsyncInternal<Material>("Editor/TexturePreviewMaterial");
            if (baseMaterial == null)
                throw new FlaxException("Cannot load texture preview material.");
            _previewMaterial = baseMaterial.CreateVirtualInstance();

            // Add widgets
            if (useWidgets)
            {
                // Channels widget
                var channelsWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperLeft);
                //
                var channelR = new ViewportWidgetButton("R", Sprite.Invalid, null, true)
                {
                    Checked = true,
                    TooltipText = "Show/hide texture red channel",
                    Parent = channelsWidget
                };
                channelR.OnToggle += button => ViewChannels = button.Checked ? ViewChannels | ChannelFlags.Red : (ViewChannels & ~ChannelFlags.Red);
                var channelG = new ViewportWidgetButton("G", Sprite.Invalid, null, true)
                {
                    Checked = true,
                    TooltipText = "Show/hide texture green channel",
                    Parent = channelsWidget
                };
                channelG.OnToggle += button => ViewChannels = button.Checked ? ViewChannels | ChannelFlags.Green : (ViewChannels & ~ChannelFlags.Green);
                var channelB = new ViewportWidgetButton("B", Sprite.Invalid, null, true)
                {
                    Checked = true,
                    TooltipText = "Show/hide texture blue channel",
                    Parent = channelsWidget
                };
                channelB.OnToggle += button => ViewChannels = button.Checked ? ViewChannels | ChannelFlags.Blue : (ViewChannels & ~ChannelFlags.Blue);
                var channelA = new ViewportWidgetButton("A", Sprite.Invalid, null, true)
                {
                    Checked = true,
                    TooltipText = "Show/hide texture alpha channel",
                    Parent = channelsWidget
                };
                channelA.OnToggle += button => ViewChannels = button.Checked ? ViewChannels | ChannelFlags.Alpha : (ViewChannels & ~ChannelFlags.Alpha);
                //
                channelsWidget.Parent = this;
            }

            // Wait for base (don't want to async material parameters set due to async loading)
            baseMaterial.WaitForLoaded();
        }

        /// <summary>
        /// Sets the texture to draw (material parameter).
        /// </summary>
        /// <param name="value">The value.</param>
        protected void SetTexture(object value)
        {
            _previewMaterial.GetParam("Texture").Value = value;
            UpdateTextureRect();
        }

        private void UpdateMask()
        {
            Vector4 mask = Vector4.One;
            if ((_channelFlags & ChannelFlags.Red) == 0)
                mask.X = 0;
            if ((_channelFlags & ChannelFlags.Green) == 0)
                mask.Y = 0;
            if ((_channelFlags & ChannelFlags.Blue) == 0)
                mask.Z = 0;
            if ((_channelFlags & ChannelFlags.Alpha) == 0)
                mask.W = 0;
            _previewMaterial.GetParam("Mask").Value = mask;
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();
            ViewportWidgetsContainer.ArrangeWidgets(this);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            Object.Destroy(ref _previewMaterial);

            base.OnDestroy();
        }
    }

    /// <summary>
    /// Texture preview GUI control. Draws <see cref="FlaxEngine.Texture"/> in the UI and supports view moving/zomming.
    /// </summary>
    /// <seealso cref="TexturePreviewBase" />
    public class SimpleTexturePreview : TexturePreviewBase
    {
        private Texture _asset;

        /// <summary>
        /// Gets or sets the asset to preview.
        /// </summary>
        public Texture Asset
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

        /// <summary>
        /// Gets or sets the color used to multiply texture colors.
        /// </summary>
        public Color Color { get; set; } = Color.White;

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
                Render2D.DrawTexture(_asset, rect, Color, true);
            }
        }
    }

    /// <summary>
    /// Sprite atlas preview GUI control. Draws <see cref="SpriteAtlas"/> in the UI and supports view moving/zomming.
    /// </summary>
    /// <seealso cref="TexturePreviewBase" />
    public class SimpleSpriteAtlasPreview : TexturePreviewBase
    {
        private SpriteAtlas _asset;

        /// <summary>
        /// Gets or sets the asset to preview.
        /// </summary>
        public SpriteAtlas Asset
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

        /// <summary>
        /// Gets or sets the color used to multiply texture colors.
        /// </summary>
        public Color Color { get; set; } = Color.White;

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
                Render2D.DrawTexture(_asset, rect, Color, true);
            }
        }
    }

    /// <summary>
    /// Texture preview GUI control. Draws <see cref="FlaxEngine.Texture"/> in the UI and supports view moving/zomming.
    /// Supports texture channels masking and color transformations.
    /// </summary>
    /// <seealso cref="TexturePreviewCustomBase" />
    public class TexturePreview : TexturePreviewCustomBase
    {
        private TextureBase _asset;

        /// <summary>
        /// Gets or sets the texture to preview.
        /// </summary>
        public TextureBase Asset
        {
            get => _asset;
            set
            {
                if (_asset != value)
                {
                    _asset = value;
                    SetTexture(_asset);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TexturePreview"/> class.
        /// </summary>
        /// <param name="useWidgets">True if show viewport widgets.</param>
        /// <inheritdoc />
        public TexturePreview(bool useWidgets)
            : base(useWidgets)
        {
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
                Render2D.DrawMaterial(_previewMaterial, rect);
            }
        }
    }

    /// <summary>
    /// Sprite atlas preview GUI control. Draws <see cref="FlaxEngine.SpriteAtlas"/> in the UI and supports view moving/zomming.
    /// Supports texture channels masking and color transformations.
    /// </summary>
    /// <seealso cref="TexturePreviewCustomBase" />
    public class SpriteAtlasPreview : TexturePreviewCustomBase
    {
        private SpriteAtlas _asset;

        /// <summary>
        /// Gets or sets the sprite atlas to preview.
        /// </summary>
        public SpriteAtlas Asset
        {
            get => _asset;
            set
            {
                if (_asset != value)
                {
                    _asset = value;
                    SetTexture(_asset);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAtlasPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">True if show viewport widgets.</param>
        /// <inheritdoc />
        public SpriteAtlasPreview(bool useWidgets)
            : base(useWidgets)
        {
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
                Render2D.DrawMaterial(_previewMaterial, rect);
            }
        }
    }
}
