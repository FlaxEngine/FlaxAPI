// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Cube Texture asset preview editor viewport.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.Previews.MaterialPreview" />
    public class CubeTexturePreview : MaterialPreview
    {
        private MaterialInstance _material;

        /// <summary>
        /// Sets the cube texture to preview.
        /// </summary>
        /// <value>
        /// The cube texture.
        /// </value>
        public CubeTexture CubeTexture
        {
            set
            {
                // Prepare material and assign texture asset as a parameter
                if (_material == null || _material.WaitForLoaded())
                {
                    // Error
                    Debug.LogError("Cannot load preview material.");
                    return;
                }
                var baseMaterial = _material.BaseMaterial;
                if (baseMaterial == null || baseMaterial.WaitForLoaded())
                {
                    // Error
                    Debug.LogError("Cannot load base material for preview material.");
                    return;
                }
                var parameters = _material.Parameters;
                if (parameters.Length != 1 || parameters[0].Type != MaterialParameterType.CubeTexture)
                {
                    // Error
                    Debug.LogError("Invalid preview material parameters.");
                    return;
                }
                parameters[0].Value = value;
            }
        }

        /// <inheritdoc />
        public CubeTexturePreview(bool useWidgets)
        : base(useWidgets)
        {
            // Create virtual material material
            _material = FlaxEngine.Content.CreateVirtualAsset<MaterialInstance>();
            if (_material != null)
                _material.BaseMaterial = FlaxEngine.Content.LoadAsyncInternal<Material>("Editor/CubeTexturePreviewMaterial");

            // Link it
            Material = _material;
        }

        /// <inheritdoc />
        public override bool HasLoadedAssets => base.HasLoadedAssets && _material.IsLoaded && _material.BaseMaterial.IsLoaded;

        /// <inheritdoc />
        public override void OnDestroy()
        {
            Material = null;
            Object.Destroy(ref _material);

            base.OnDestroy();
        }
    }
}
