////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Preview control for <see cref="IESProfile"/> asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.Previews.TexturePreviewBase" />
    public class IESProfilePreview : TexturePreviewBase
    {
        private IESProfile _asset;
        private MaterialInstance _previewMaterial;

        /// <summary>
        /// Gets or sets the asset to preview.
        /// </summary>
        public IESProfile Asset
        {
            get => _asset;
            set
            {
                if (_asset != value)
                {
                    _asset = value;
                    _previewMaterial.GetParam("Texture").Value = value;
                    UpdateTextureRect();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IESProfilePreview"/> class.
        /// </summary>
        public IESProfilePreview()
        {
            // Create preview material (virtual)
            var baseMaterial = FlaxEngine.Content.LoadAsyncInternal<Material>("Editor/IesProfilePreviewMaterial");
            if (baseMaterial == null)
                throw new FlaxException("Cannot load IES Profile preview material.");
            _previewMaterial = baseMaterial.CreateVirtualInstance();

            // Wait for base (don't want to async material parameters set due to async loading)
            baseMaterial.WaitForLoaded();
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            Object.Destroy(ref _previewMaterial);

            base.OnDestroy();
        }

        /// <inheritdoc />
        protected override void CalculateTextureRect(out Rectangle rect)
        {
            CalculateTextureRect(new Vector2(256), Size, out rect);
        }

        /// <inheritdoc />
        protected override void DrawTexture(ref Rectangle rect)
        {
            // Check if has loaded asset
            if (_asset && _asset.IsLoaded)
            {
                Render2D.DrawMaterial(_previewMaterial, rect);
            }
        }
    }
}
