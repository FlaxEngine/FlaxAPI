////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="CubeTexture"/> asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class CubeTextureWindow : AssetEditorWindowBase<CubeTexture>
    {
        private readonly MaterialPreview _preview;
        private MaterialInstance _material;

        /// <inheritdoc />
        public CubeTextureWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Create virtual material material
            throw new NotImplementedException("expose api to create virtual assets to c#");
            /*_material = FlaxEngine.Content.CreateVirtualAsset<MaterialInstance>();
            _material->GetMaterialInstance()->Init();
            _material.BaseMaterial = FlaxEngine.Content.LoadAsyncInternal<Material>("Editor/CubeTexturePreviewMaterial");
            */
            // Material preview
            _preview = new MaterialPreview(true);
            _preview.Material = _material;
            _preview.Parent = this;
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "Cube Texture";

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _preview.Material = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            // Prepare material and assign texture asset as a parameter
            if (_material == null || _material.WaitForLoaded())
            {
                // Error
                Debug.LogError("Cannot load preview material.");
                Close();
                return;
            }
            var baseMaterial = _material.BaseMaterial;
            if (baseMaterial == null || baseMaterial.WaitForLoaded())
            {
                // Error
                Debug.LogError("Cannot load base material for preview material.");
                Close();
                return;
            }
            var parameters = _material.Parameters;
            if (parameters.Length != 1 || parameters[0].Type != MaterialParameterType.CubeTexture)
            {
                // Error
                Debug.LogError("Invalid preview material parameters.");
                Close();
                return;
            }
            parameters[0].Value = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _preview.Material = null;
            Object.Destroy(ref _material);

            base.OnDestroy();
        }
    }
}
