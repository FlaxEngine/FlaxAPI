////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Material window allows to view and edit <see cref="MaterialInstance"/> asset.
    /// Note: it uses actual asset to modify so changes are visible live in the game/editor preview.
    /// </summary>
    /// <seealso cref="MaterialInstance" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class MaterialInstanceWindow : AssetEditorWindowBase<MaterialInstance>
    {
        /// <summary>
        /// The material properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            private Material _restoreBase;
            private Dictionary<string, object> _restoreParams;

            [EditorOrder(10), EditorDisplay("General"), Tooltip("The base material used to override it's properties")]
            public Material BaseMaterial
            {
                get => MaterialWinRef?.Asset?.BaseMaterial;
                set
                {
                    var asset = MaterialWinRef?.Asset;
                    if (asset)
                        asset.BaseMaterial = value;
                }
            }
            
            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor))]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public MaterialInstanceWindow MaterialWinRef { get; set; }

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
                    var materialWin = Values[0] as MaterialInstanceWindow;
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
                        Type pType;
                        switch (p.Type)
                        {
                            case MaterialParameterType.CubeTexture:
                                pType = typeof(CubeTexture);
                                break;
                            case MaterialParameterType.Texture:
                            case MaterialParameterType.NormalMap:
                                pType = typeof(Texture);
                                break;
                            case MaterialParameterType.RenderTarget:
                                pType = typeof(RenderTarget);
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
                                var win = (MaterialInstanceWindow)instance;
                                return win.Asset.Parameters[pIndex].Value;
                            },
                            (instance, index, value) =>
                            {
                                // Set material parameter and surface parameter
                                var win = (MaterialInstanceWindow)instance;
                                win.Asset.Parameters[pIndex].Value = value;
                                win._paramValueChange = true;
                            }
                        );

                        layout.Property(p.Name, propertyValue);
                    }
                }
                
                /// <inheritdoc />
                public override void Refresh()
                {
                    var materialWin = Values[0] as MaterialInstanceWindow;
                    var material = materialWin?.Asset;
                    int parametersHash = -1;
                    if (material != null)
                    {
                        if (material.IsLoaded)
                        {
                            var parameters = material.Parameters;// need to ask for params here to sync valid hash   
                            parametersHash = material._parametersHash;
                        }
                        else
                        {
                            parametersHash = -2;
                        }
                    }

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
            public void OnLoad(MaterialInstanceWindow materialWin)
            {
                // Link
                MaterialWinRef = materialWin;

                // Prepare restore data
                PeekState();
            }

            /// <summary>
            /// Records the current state to restore it on DiscardChanges.
            /// </summary>
            public void PeekState()
            {
                if (MaterialWinRef == null)
                    return;

                var material = MaterialWinRef.Asset;
                _restoreBase = material.BaseMaterial;
                var parameters = material.Parameters;
                _restoreParams = new Dictionary<string, object>();
                for (int i = 0; i < parameters.Length; i++)
                    _restoreParams[parameters[i].Name] = parameters[i].Value;
            }

            /// <summary>
            /// On discard changes
            /// </summary>
            public void DiscardChanges()
            {
                if (MaterialWinRef == null)
                    return;

                var material = MaterialWinRef.Asset;
                material.BaseMaterial = _restoreBase;
                var parameters = material.Parameters;
                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = parameters[i];
                    if (p.IsPublic && _restoreParams.TryGetValue(p.Name, out var value))
                    {
                        p.Value = value;
                    }
                }
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

	    private readonly SplitPanel _split;
		private readonly MaterialPreview _preview;
	    private readonly ToolStripButton _saveButton;

        private readonly PropertiesProxy _properties;
        private bool _isWaitingForLoad;
        internal bool _paramValueChange;

        /// <inheritdoc />
        public MaterialInstanceWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
			// Toolstrip
	        _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.UI.GetIcon("Save32"), Save).LinkTooltip("Save");

            // Split Panel
            _split = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Material preview
            _preview = new MaterialPreview(true)
            {
                Parent = _split.Panel1
            };
            
            // Material properties editor
            var propertiesEditor = new CustomEditorPresenter(null);
            propertiesEditor.Panel.Parent = _split.Panel2;
            _properties = new PropertiesProxy();
            propertiesEditor.Select(_properties);
            propertiesEditor.Modified += OnMaterialPropertyEdited;
        }

        private void OnMaterialPropertyEdited()
        {
            _paramValueChange = false;
            MarkAsEdited();
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the orginal asset
            if (!IsEdited)
                return;

            // Save
            if (Asset.Save())
            {
                // Error
                Editor.LogError("Cannot save material instance.");
                return;
            }

            // Update
            _properties.PeekState();
            ClearEditedFlag();
            _item.RefreshThumbnail();
        }
		
        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            _saveButton.Enabled = IsEdited;

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Material = null;
            _isWaitingForLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Material = _asset;
            _isWaitingForLoad = true;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        protected override void OnClose()
        {
            // Discard unsaved changes
            _properties.DiscardChanges();

            base.OnClose();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            // Check if need to load
            if (_isWaitingForLoad && _asset.IsLoaded && (_asset.BaseMaterial == null || _asset.BaseMaterial.IsLoaded))
            {
                // Clear flag
                _isWaitingForLoad = false;

                // Init material properties and parameters proxy
                _properties.OnLoad(this);

                // Setup
                ClearEditedFlag();
            }
        }

	    /// <inheritdoc />
	    public override bool UseLayoutData => true;

	    /// <inheritdoc />
	    public override void OnLayoutSerialize(XmlWriter writer)
	    {
		    writer.WriteAttributeString("Split", _split.SplitterValue.ToString());
	    }

	    /// <inheritdoc />
	    public override void OnLayoutDeserialize(XmlElement node)
	    {
		    float value1;

		    if (float.TryParse(node.GetAttribute("Split"), out value1))
			    _split.SplitterValue = value1;
	    }

	    /// <inheritdoc />
	    public override void OnLayoutDeserialize()
	    {
		    _split.SplitterValue = 0.7f;
	    }
	}
}
