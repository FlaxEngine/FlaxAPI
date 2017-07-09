////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
/*
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
            _material = FlaxEngine.Content.CreateVirtualAsset<MaterialInstance>();
            _material->GetMaterialInstance()->Init();
            _material.BaseMaterial = FlaxEngine.Content.LoadAsyncInternal<Material>("Editor/CubeTexturePreviewMaterial");

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
            if (_material.WaitForLoaded())
            {
                // Error
                LOG_EDITOR(EditorError, 109, "Cannot load it.");
                Close();
                return;
            }
            var baseMaterial = _material.BaseMaterial;
            if (!materialInstance->HasBaseMaterial())
            {
                // Error
                LOG_EDITOR(EditorError, 109, "Missing base material.");
                Close();
                return;
            }
            materialInstance->GetBaseMaterial()->WaitForLoaded();
            if (!materialInstance->HasBaseMaterial() || materialInstance->GetBaseMaterial()->LastLoadFailed())
            {
                // Error
                LOG_EDITOR(EditorError, 109, "Cannot load base material.");
                Close();
                return;
            }
            auto params = materialInstance->GetParams();
            if (params->Count() != 1 || params->Get(0)->GetType() != MaterialParamType::CubeTexture)
            {
                // Error
                LOG_EDITOR(EditorError, 109, "Invalid parameters.");
                Close();
                return;
            }
            params->Get(0)->SetValue(_element->GetID());
            
            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _preview.Material = null;

            Destroy instance

            base.OnDestroy();
        }
    }
}
*/