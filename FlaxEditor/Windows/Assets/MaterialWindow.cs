////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
// ReSharper disable MemberCanBePrivate.Local

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
        /// <summary>
        /// The material properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            [EditorOrder(10), Tooltip("Material domain type")]
            public MaterialDomain Domain { get; set; }

            [EditorOrder(20), Tooltip("Determinates how materials' color should be blended with the background colors")]
            public MaterialBlendMode BlendMode { get; set; }

            [EditorOrder(30), Tooltip("Indicates that material should be renered without backface culling and normals should be fliped for the backfaces")]
            public bool TwoSided { get; set; }

            [EditorOrder(40), Tooltip("True if render in wireframe mode")]
            public bool Wireframe { get; set; }

            [EditorOrder(100), EditorDisplay("Transparency"), Tooltip("Transparent materials lighting mode")]
            public MaterialTransparentLighting Lighting { get; set; }

            [EditorOrder(110), EditorDisplay("Transparency"), Tooltip("True if disable depth test when rendering material")]
            public bool DisableDepthTest { get; set; }

            [EditorOrder(120), EditorDisplay("Transparency"), Tooltip("True if disable reflections when rendering material")]
            public bool DisableReflections { get; set; }

            [HideInEditor, EditorOrder(130), EditorDisplay("Transparency"), Tooltip("True if disable atmosphere fog when rendering material")]
            public bool DisableFog { get; set; }

            [EditorOrder(140), EditorDisplay("Transparency"), Tooltip("True if disable distortion effect when rendering material")]
            public bool DisableDistortion { get; set; }

            [EditorOrder(200), EditorDisplay("Misc"), Tooltip("True if disable depth buffer write when rendering material")]
            public bool DisableDepthWrite { get; set; }

            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor))]
            public MaterialWindow MaterialWinRef { get; set; }

            /// <summary>
            /// Custom editor for editing material parameters collection.
            /// </summary>
            /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
            public class ParametersEditor : CustomEditor
            {
                private int _parametersHash;

                /// <inheritdoc />
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    var materialWin = Values[0] as MaterialWindow;
                    var material = materialWin?.Asset;
                    if (material == null)
                    {
                        _parametersHash = -1;
                        layout.Label("No parameters");
                        return;
                    }
                    if (!material.IsLoaded)
                    {
                        _parametersHash = -2;
                        layout.Label("Loading...");
                        return;
                    }
                    _parametersHash = material._parametersHash;
                    var parameters = material.Parameters;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];
                        if (!p.IsPublic)
                            continue;

                        var pIndex = i;
                        var pValue = p.Value;
                        var pGuidType = false;
                        Type pType;
                        switch (p.Type)
                        {
                            case MaterialParameterType.CubeTexture:
                                pType = typeof(CubeTexture);
                                pGuidType = true;
                                break;
                            case MaterialParameterType.Texture:
                            case MaterialParameterType.NormalMap:
                                pType = typeof(Texture);
                                pGuidType = true;
                                break;
                            default:
                                pType = p.Value.GetType();
                                break;
                        }

                        var propertyValue = new CustomValueContainer(
                            pType,
                            pValue,
                            (instance, index) =>
                            {
                                // Get material parameter
                                var win = (MaterialWindow)instance;
                                return win.Asset.Parameters[pIndex].Value;
                            },
                            (instance, index, value) =>
                            {
                                // Set material parameter and surface parameter
                                var win = (MaterialWindow)instance;

                                // Visject surface paramaters are only value type objects so convert value if need to (eg. instead of texture ref write texture id)
                                var surfaceParam = value;
                                if (pGuidType)
                                    surfaceParam = (value as FlaxEngine.Object)?.ID ?? Guid.Empty;

                                win.Asset.Parameters[pIndex].Value = value;
                                win.Surface.Parameters[pIndex].Value = surfaceParam;
                            }
                        );

                        layout.Property(p.Name, propertyValue);
                    }
                }

                /// <inheritdoc />
                public override void Refresh()
                {
                    var materialWin = Values[0] as MaterialWindow;
                    var material = materialWin?.Asset;
                    int parametersHash = -1;
                    if (material != null)
                        parametersHash = material.IsLoaded ? material._parametersHash : -2;

                    if (parametersHash != _parametersHash)
                    {
                        // Parameters has been modifed (loaded/unloaded/edited)
                        RebuildLayout();
                    }
                }
            }

            /// <summary>
            /// Gathers parameters from the specified material.
            /// </summary>
            /// <param name="materialWin">The material window.</param>
            public void OnLoad(MaterialWindow materialWin)
            {
                // Update cache
                var material = materialWin.Asset;
                var info = material.Info;
                Wireframe = (info.Flags & MaterialFlags.Wireframe) != 0;
                TwoSided = (info.Flags & MaterialFlags.TwoSided) != 0;
                DisableDepthTest = (info.Flags & MaterialFlags.TransparentDisableDepthTest) != 0;
                DisableReflections = (info.Flags & MaterialFlags.TransparentDisableReflections) != 0;
                DisableFog = (info.Flags & MaterialFlags.TransparentDisableFog) != 0;
                DisableDepthWrite = (info.Flags & MaterialFlags.DisableDepthWrite) != 0;
                DisableDistortion = (info.Flags & MaterialFlags.TransparentDisableDistortion) != 0;
                BlendMode = info.BlendMode;
                Lighting = info.TransparentLighting;
                Domain = info.Domain;

                // Link
                MaterialWinRef = materialWin;
            }

            /// <summary>
            /// Saves the material properties to the material info structure.
            /// </summary>
            /// <param name="info">The material info.</param>
            public void OnSave(ref MaterialInfo info)
            {
                // Update flags
                if (Wireframe)
                    info.Flags |= MaterialFlags.Wireframe;
                if (TwoSided)
                    info.Flags |= MaterialFlags.TwoSided;
                if (DisableDepthTest)
                    info.Flags |= MaterialFlags.TransparentDisableDepthTest;
                if (DisableReflections)
                    info.Flags |= MaterialFlags.TransparentDisableReflections;
                if (DisableFog)
                    info.Flags |= MaterialFlags.TransparentDisableFog;
                if (DisableDepthWrite)
                    info.Flags |= MaterialFlags.DisableDepthWrite;
                if (DisableDistortion)
                    info.Flags |= MaterialFlags.TransparentDisableDistortion;
                info.BlendMode = BlendMode;
                info.TransparentLighting = Lighting;
                info.Domain = Domain;
            }

            /// <summary>
            /// Clears temporary data.
            /// </summary>
            public void OnClean()
            {
                // Unlink
                MaterialWinRef = null;
            }
        }

        private readonly MaterialPreview _preview;
        private readonly VisjectSurface _surface;

        private readonly PropertiesProxy _properties;
        private bool _isWaitingForSurfaceLoad;
        private bool _tmpMaterialIsDirty;

        /// <summary>
        /// Gets the material surface.
        /// </summary>
        public VisjectSurface Surface => _surface;

        /// <inheritdoc />
        public MaterialWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, Editor.UI.GetIcon("Save32"));// .LinkTooltip(GetSharedTooltip(), TEXT("Save"));// Save material
            // TODO: option to center view to main node (use ScrollViewToMain function)
            // TODO: tooltips support!

            // Split Panel 1
            var splitPanel1 = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Split Panel 2
            var splitPanel2 = new SplitPanel(Orientation.Vertical, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.4f,
                Parent = splitPanel1.Panel2
            };

            // Material preview
            _preview = new MaterialPreview(true)
            {
                Parent = splitPanel2.Panel1
            };

            // Material properties editor
            var propertiesEditor = new CustomEditorPresenter(null);
            propertiesEditor.Panel.Width = splitPanel2.Panel2.Width;
            propertiesEditor.Panel.AnchorStyle = AnchorStyle.Upper;
            propertiesEditor.Panel.Parent = splitPanel2.Panel2;
            _properties = new PropertiesProxy();
            propertiesEditor.Select(_properties);
            propertiesEditor.OnModify += OnMaterialPropertyEdited;

            // Surface
            _surface = new VisjectSurface(this, SurfaceType.Material)
            {
                Parent = splitPanel1.Panel1,
                Enabled = false
            };
        }

        private void OnMaterialPropertyEdited()
        {
            _surface.MarkAsEdited();
            RefreshMainNode();
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

            // Check if surface has been edited
            if (_surface.IsEdited)
            {
                // Save surface
                var data = _surface.Save();
                if (data == null)
                {
                    // Error
                    Editor.LogError("Failed to save material surface");
                    return true;
                }

                // Create material info
                MaterialInfo info;
                FillMaterialInfo(out info);

                // Save data to the temporary material
                if (_asset.SaveSurface(data, info))
                {
                    // Error
                    _surface.MarkAsEdited();
                    Editor.LogError("Failed to save material surface data");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets material info from UI.
        /// </summary>
        /// <param name="info">Output info.</param>
        public void FillMaterialInfo(out MaterialInfo info)
        {
            info = new MaterialInfo
            {
                Flags = MaterialFlags.None,
                BlendMode = MaterialBlendMode.Opaque,
                Domain = MaterialDomain.Surface,
                TransparentLighting = MaterialTransparentLighting.None
            };
            _properties.OnSave(ref info);
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
            if (SaveToOriginal())
            {
                // Error
                return;
            }

            // Copy shader cache from the temporary material (will skip compilation on Reload - faster)
            Guid dstId = _item.ID;
            Guid srcId = _asset.ID;
            Editor.Internal_CopyCache(ref dstId, ref srcId);
            
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
            _properties.OnClean();
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

                // Init material properties and parameters proxy
                _properties.OnLoad(this);
                
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
