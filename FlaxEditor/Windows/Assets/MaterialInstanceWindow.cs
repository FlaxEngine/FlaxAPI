// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
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
        [CustomEditor(typeof(ParametersEditor))]
        private sealed class PropertiesProxy
        {
            private Material _restoreBase;
            private Dictionary<string, object> _restoreParams;

            [EditorDisplay("General"), Tooltip("The base material used to override it's properties")]
            public Material BaseMaterial
            {
                get => Window?.Asset?.BaseMaterial;
                set
                {
                    var asset = Window?.Asset;
                    if (asset)
                    {
                        asset.BaseMaterial = value;
                        //Window._editor.BuildLayoutOnUpdate();
                    }
                }
            }

            /// <summary>
            /// The window reference. Used to handle some special logic.
            /// </summary>
            [NoSerialize, HideInEditor]
            public MaterialInstanceWindow Window;

            /// <summary>
            /// The material parameter values collection. Used to record undo changes.
            /// </summary>
            /// <remarks>
            /// Contains only items with raw values excluding Flax Objects.
            /// </remarks>
            [HideInEditor]
            public object[] Values
            {
                get => Window?.Asset?.Parameters.Select(x => x.Value).ToArray();
                set
                {
                    var parameters = Window?.Asset?.Parameters;
                    if (value != null && parameters != null)
                    {
                        if (value.Length != parameters.Length)
                            return;

                        for (int i = 0; i < value.Length; i++)
                        {
                            var p = parameters[i].Value;
                            if (p is FlaxEngine.Object || p == null)
                                continue;

                            parameters[i].Value = value[i];
                        }
                    }
                }
            }

            /// <summary>
            /// The material parameter values collection. Used to record undo changes.
            /// </summary>
            /// <remarks>
            /// Contains only items with references to Flax Objects identified by ID.
            /// </remarks>
            [HideInEditor]
            public FlaxEngine.Object[] ValuesRef
            {
                get => Window?.Asset?.Parameters.Select(x => x.Value as FlaxEngine.Object).Cast<FlaxEngine.Object>().ToArray();
                set
                {
                    var parameters = Window?.Asset?.Parameters;
                    if (value != null && parameters != null)
                    {
                        if (value.Length != parameters.Length)
                            return;

                        for (int i = 0; i < value.Length; i++)
                        {
                            var p = parameters[i].Value;
                            if (!(p is FlaxEngine.Object || p == null))
                                continue;

                            parameters[i].Value = value[i];
                        }
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
                Window = materialWin;

                // Prepare restore data
                PeekState();
            }

            /// <summary>
            /// Records the current state to restore it on DiscardChanges.
            /// </summary>
            public void PeekState()
            {
                if (Window == null)
                    return;

                var material = Window.Asset;
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
                if (Window == null)
                    return;

                var material = Window.Asset;
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
                Window = null;
            }
        }

        /// <summary>
        /// Custom editor for editing material parameters collection.
        /// </summary>
        /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
        public class ParametersEditor : GenericEditor
        {
            private static readonly object[] DefaultAttributes = { new LimitAttribute(float.MinValue, float.MaxValue, 0.1f) };
            private int _parametersHash;

            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                // Prepare
                var proxy = (PropertiesProxy)Values[0];
                var material = proxy.Window?.Asset;
                if (material == null)
                {
                    _parametersHash = -1;
                    layout.Label("No parameters");
                    return;
                }
                if (!material.IsLoaded || (material.BaseMaterial && !material.BaseMaterial.IsLoaded))
                {
                    _parametersHash = -2;
                    layout.Label("Loading...");
                    return;
                }
                _parametersHash = material._parametersHash;
                var parameters = material.Parameters;

                // Base
                base.Initialize(layout);

                // Show parameters
                if (parameters.Length != 0)
                {
                    var parametersGroup = layout.Group("Parameters");
                    InitializeProperties(parametersGroup, parameters, material.BaseMaterial);
                }
            }

            private void InitializeProperties(LayoutElementsContainer layout, MaterialParameter[] parameters, Material baseMaterial)
            {
                var baseMaterialParameters = baseMaterial?.Parameters;
                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = parameters[i];
                    if (!p.IsPublic)
                        continue;

                    var pIndex = i;
                    var pValue = p.Value;
                    Type pType;
                    object[] attributes = null;

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
                    case MaterialParameterType.RenderTargetArray:
                    case MaterialParameterType.RenderTargetCube:
                    case MaterialParameterType.RenderTargetVolume:
                        pType = typeof(RenderTarget);
                        break;
                    default:
                        pType = p.Value.GetType();
                        // TODO: support custom attributes with defined value range for parameter (min, max)
                        attributes = DefaultAttributes;
                        break;
                    }

                    var propertyValue = new CustomValueContainer(
                        pType,
                        pValue,
                        (instance, index) =>
                        {
                            // Get material parameter
                            var proxy = (PropertiesProxy)instance;
                            var array = proxy.Window.Asset.Parameters;
                            if (array == null || array.Length <= pIndex)
                                throw new TargetException("Material parameters collection has been changed.");
                            return array[pIndex].Value;
                        },
                        (instance, index, value) =>
                        {
                            // Set material parameter and surface parameter
                            var proxy = (PropertiesProxy)instance;
                            proxy.Window.Asset.Parameters[pIndex].Value = value;
                            proxy.Window._paramValueChange = true;
                        },
                        attributes
                    );

                    // Try to get default value (from the base material)
                    if (baseMaterialParameters != null && baseMaterialParameters.Length > i && baseMaterialParameters[i].Type == p.Type)
                    {
                        var defaultValue = baseMaterialParameters[i].Value;
                        propertyValue.SetDefaultValue(defaultValue);
                    }

                    layout.Property(p.Name, propertyValue);
                }
            }

            /// <inheritdoc />
            public override void Refresh()
            {
                base.Refresh();

                var proxy = Values[0] as PropertiesProxy;
                var material = proxy?.Window?.Asset;
                int parametersHash = -1;
                if (material != null)
                {
                    if (material.IsLoaded)
                    {
                        var parameters = material.Parameters; // need to ask for params here to sync valid hash   
                        parametersHash = material._parametersHash;
                    }
                    else
                    {
                        parametersHash = -2;
                    }
                }

                if (parametersHash != _parametersHash)
                {
                    // Parameters has been modified (loaded/unloaded/edited)
                    RebuildLayout();
                }
            }
        }

        private readonly SplitPanel _split;
        private readonly MaterialPreview _preview;
        private readonly ToolStripButton _saveButton;
        private readonly ToolStripButton _undoButton;
        private readonly ToolStripButton _redoButton;

        private readonly CustomEditorPresenter _editor;
        private readonly Undo _undo;
        private readonly PropertiesProxy _properties;
        private bool _isWaitingForLoad;
        internal bool _paramValueChange;

        /// <inheritdoc />
        public MaterialInstanceWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Undo
            _undo = new Undo();
            _undo.UndoDone += OnUndo;
            _undo.RedoDone += OnUndo;
            _undo.ActionDone += OnUndo;

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Save32, Save).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _undoButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Undo32, _undo.PerformUndo).LinkTooltip("Undo (Ctrl+Z)");
            _redoButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Redo32, _undo.PerformRedo).LinkTooltip("Redo (Ctrl+Y)");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/graphics/materials/instanced-materials/index.html")).LinkTooltip("See documentation to learn more");

            // Split Panel
            _split = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.5f,
                Parent = this
            };

            // Material preview
            _preview = new MaterialPreview(true)
            {
                Parent = _split.Panel1
            };

            // Material properties editor
            _editor = new CustomEditorPresenter(_undo);
            _editor.Panel.Parent = _split.Panel2;
            _properties = new PropertiesProxy();
            _editor.Select(_properties);
            _editor.Modified += OnMaterialPropertyEdited;
        }

        private void OnUndo(IUndoAction action)
        {
            _paramValueChange = false;
            MarkAsEdited();
            UpdateToolstrip();
            _editor.BuildLayoutOnUpdate();
        }

        private void OnMaterialPropertyEdited()
        {
            _paramValueChange = false;
            MarkAsEdited();
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the original asset
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
            _undoButton.Enabled = _undo.CanUndo;
            _redoButton.Enabled = _undo.CanRedo;

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

            // Cleanup
            _undo.Clear();

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
                _undo.Clear();
            }
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            // Base
            bool result = base.OnKeyDown(key);
            if (!result)
            {
                if (Root.GetKey(Keys.Control))
                {
                    switch (key)
                    {
                    case Keys.Z:
                        _undo.PerformUndo();
                        return true;
                    case Keys.Y:
                        _undo.PerformRedo();
                        return true;
                    }
                }
            }

            return result;
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
            _split.SplitterValue = 0.5f;
        }
    }
}
