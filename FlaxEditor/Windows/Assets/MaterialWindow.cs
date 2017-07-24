////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Material window allows to view and edit <see cref="Material"/> asset.
    /// Note: it uses ClonedAssetEditorWindowBase which is creating cloned asset to edit/preview.
    /// </summary>
    /// <seealso cref="Material" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    /// <seealso cref="FlaxEditor.Surface.IVisjectSurfaceOwner" />
    public sealed class MaterialWindow : ClonedAssetEditorWindowBase<Material>, IVisjectSurfaceOwner
    {
        // TODO: things to finish
        // - material parameters and properties proxy
        // - saving surface
        
        private readonly SplitPanel _splitPanel1;
        private readonly SplitPanel _splitPanel2;
        private readonly MaterialPreview _preview;
        private readonly VisjectSurface _surface;

        private bool _isWaitingForSurfaceLoad;
        private bool _tmpMaterialIsDirty;

        /// <inheritdoc />
        public MaterialWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, Editor.UI.GetIcon("Save32"));// .LinkTooltip(GetSharedTooltip(), TEXT("Save"));// Save material
            // TODO: option to center view to main node (use ScrollViewToMain function)
            // TODO: tooltips support!

            // Split Panel 1
            _splitPanel1 = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None);
            _splitPanel1.SplitterValue = 0.7f;
            _splitPanel1.Parent = this;

            // Split Panel 2
            _splitPanel2 = new SplitPanel(Orientation.Vertical, ScrollBars.None, ScrollBars.Vertical);
            _splitPanel2.SplitterValue = 0.4f;
            _splitPanel2.Parent = _splitPanel1.Panel2;

            // Material preview
            _preview = new MaterialPreview(true);
            _preview.Parent = _splitPanel2.Panel1;

            // TODO: Properies Editor for material properties and parameters editing

            // Surface
            _surface = new VisjectSurface(this, SurfaceType.Material);
            _surface.Parent = _splitPanel1.Panel1;
            _surface.Enabled = false;
        }

        /// <summary>
        /// Refreshes temporary material to see changes live when editing the surface.
        /// </summary>
        /// <returns>True if cannot refresh it, otherwise false.</returns>
        public bool RefreshTempMaterial()
        {
            // Esnure material is loaded
            if (_asset == null || !_asset.IsLoaded)
            {
                // Error
                return true;
            }
            if (_isWaitingForSurfaceLoad)
                return true;

            //throw new NotImplementedException("TODO: finish material surface saving");
            // Check if surface has been edited
            /*if (_surface.IsEdited)
            {
                // Save surface
                MemoryWriteStream surfaceStream(2 * 1024);
                if (_surface.Save(&surfaceStream))
                {
                    // Error
                    LOG_EDITOR(Error, 62, _element.ToString());
                    return true;
                }

                // Create material info
                MaterialInfo info;
                FillMaterialInfo(&info);

                // Save data to the temporary material
                _preview.Disable();
                _isWaitingForLoad = true;
                if (_tmpMaterial.SaveSurface(&surfaceStream, &info))
                {
                    // Error
                    _surface.MarkAsEdited();
                    LOG_EDITOR(Error, 63, _element.ToString());
                    return true;
                }
            }*/

            return false;
        }

        /// <summary>
        /// Gets material info from UI.
        /// </summary>
        /// <param name="info">Output info.</param>
        public void FillMaterialInfo(out MaterialInfo info)
        {
            info = new MaterialInfo();
            info.Flags = MaterialFlags.None;
            info.BlendMode = MaterialBlendMode.Opaque;
            info.Domain = MaterialDomain.Surface;
            info.TransparentLighting = MaterialTransparentLighting.None;
            //_proxy.OnSave(info); // TODO: finish material proxy like in c++ editor
        }

        /// <summary>
        /// Gets or sets the main material node.
        /// </summary>
        /// <value>
        /// The main node.
        /// </value>
        private Surface.Archetypes.Material.SurfaceNodeMaterial MainNode
        {
            get
            {
                var mainNode = _surface.FindNode(1, 1) as Surface.Archetypes.Material.SurfaceNodeMaterial;
                if (mainNode == null)
                {
                    // Error
                    Debug.LogError("Failed to find main material node.");
                }
                return mainNode;
            }
        }

        /// <summary>
        /// Refreshes the main material node.
        /// </summary>
        public void RefreshMainNode()
        {
            // Find main node
            var mainNode = MainNode;
            if (mainNode == null)
                return;

            // Refresh it
            mainNode.UpdateBoxes();
        }

        /// <summary>
        /// Scrolls the view to the main material node.
        /// </summary>
        public void ScrollViewToMain()
        {
            // Find main node
            var mainNode = MainNode;
            if (mainNode == null)
                return;

            // Change scale and position
            _surface.ViewScale = 1.0f;
            _surface.ViewCenterPosition = mainNode.Center;
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the orginal asset
            if (!IsEdited)
                return;

            // Just in case refresh data
            if (RefreshTempMaterial())
            {
                // Error
                return;
            }

            // Update original material so user can see changes in the scene
            SaveToOriginal();

            // Copy shader cache from the temporary material (will skip compilation on Reload - faster)
            throw new NotImplementedException("copy temp material cache to the original one");
            //ShaderCacheManager.Instance().CopyCache(materialId, _tmpMaterial.GetID());

            // Clear flag
            ClearEditedFlag();

            // Update
            OnSurfaceEditedChanged();
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override string WindowTitleName => "Material";

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                    Save();
                    break;
                default:
                    base.OnToolstripButtonClicked(id);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            if (_toolstrip != null)
            {
                _toolstrip.GetButton(1).Enabled = IsEdited;
            }

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _preview.Material = null;
            _isWaitingForSurfaceLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Material = _asset;
            _isWaitingForSurfaceLoad = true;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public void OnSurfaceSave()
        {
            Save();
        }

        /// <inheritdoc />
        public void OnSurfaceEditedChanged()
        {
            if (_surface.IsEdited)
                MarkAsEdited();
        }

        /// <inheritdoc />
        public void OnSurfaceGraphEdited()
        {
            // Mark as dirty
            _tmpMaterialIsDirty = true;
        }

        /// <inheritdoc />
        public Texture GetSurfaceBackground()
        {
            return Editor.UI.VisjectSurfaceBackground;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Check if temporary material need to be updated
            if (_tmpMaterialIsDirty)
            {
                // Clear flag
                _tmpMaterialIsDirty = false;

                // Update
                RefreshTempMaterial();
            }

            // Check if need to load surface
            if (_isWaitingForSurfaceLoad && _asset.IsLoaded)
            {
                // Clear flag
                _isWaitingForSurfaceLoad = false;

                // Init material properties and parameters panel
                //_proxy.OnLoaded(_asset.Info, _asset.Parameters);
                
                // Load surface data from the asset
                byte[] data = _asset.LoadSurface(true);
                if (data == null)
                {
                    // Error
                    Debug.LogError("Failed to load material surface data.");
                    Close();
                    return;
                }
                
                // Load surface graph
                if (_surface.Load(data))
                {
                    // Error
                    Debug.LogError("Failed to load material surface.");
                    Close();
                    return;
                }
                
                // Setup
                _surface.Enabled = true;
                ClearEditedFlag();
            }
        }
    }
}
