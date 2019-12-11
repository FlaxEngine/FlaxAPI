// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.GUI.Drag;
using FlaxEditor.History;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// The base class for editor windows that use <see cref="FlaxEditor.Surface.VisjectSurface"/> for content editing. 
    /// Note: it uses ClonedAssetEditorWindowBase which is creating cloned asset to edit/preview.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    /// <seealso cref="FlaxEditor.Surface.IVisjectSurfaceOwner" />
    public abstract class VisjectSurfaceWindow<TAsset, TSurface, TPreview> : ClonedAssetEditorWindowBase<TAsset>, IVisjectSurfaceOwner
    where TAsset : Asset
    where TSurface : VisjectSurface
    where TPreview : AssetPreview
    {
        protected class RenameParamAction : IUndoAction
        {
            public VisjectSurfaceWindow<TAsset, TSurface, TPreview> Window;
            public int Index;
            public string Before;
            public string After;

            /// <inheritdoc />
            public string ActionString => "Rename parameter";

            /// <inheritdoc />
            public void Do()
            {
                Set(After);
            }

            /// <inheritdoc />
            public void Undo()
            {
                Set(Before);
            }

            private void Set(string value)
            {
                var param = Window.Surface.Parameters[Index];
                param.Name = value;
                Window.Surface.OnParamRenamed(param);
                Window.OnParamRenameUndo(this);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                Window = null;
                Before = null;
                After = null;
            }
        }

        protected class AddRemoveParamAction : IUndoAction
        {
            public VisjectSurfaceWindow<TAsset, TSurface, TPreview> Window;
            public bool IsAdd;
            public int Index;
            public string Name;
            public ParameterType Type;

            /// <inheritdoc />
            public string ActionString => IsAdd ? "Add parameter" : "Remove parameter";

            /// <inheritdoc />
            public void Do()
            {
                if (IsAdd)
                    Add();
                else
                    Remove();
            }

            /// <inheritdoc />
            public void Undo()
            {
                if (IsAdd)
                    Remove();
                else
                    Add();
            }

            private void Add()
            {
                var param = SurfaceParameter.Create(Type);
                param.Name = Name;
                if (Type == ParameterType.NormalMap)
                {
                    // Use default normal map texture (don't load asset here, just lookup registry for id at path)
                    FlaxEngine.Content.GetAssetInfo(StringUtils.CombinePaths(Globals.EngineFolder, "Textures/NormalTexture.flax"), out _, out var id);
                    param.Value = id;
                }
                Window.Surface.Parameters.Insert(Index, param);
                Window.Surface.OnParamCreated(param);
                Window.OnParamAddUndo(this);
            }

            private void Remove()
            {
                var param = Window.Surface.Parameters[Index];
                Name = param.Name;
                Type = param.Type;
                Window.Surface.Parameters.RemoveAt(Index);
                Window.Surface.OnParamDeleted(param);
                Window.OnParamRemoveUndo(this);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                Window = null;
            }
        }

        /// <summary>
        /// Custom editor for editing material parameters collection.
        /// </summary>
        /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
        protected class ParametersEditor : CustomEditor
        {
            private static readonly object[] DefaultAttributes = { new LimitAttribute(float.MinValue, float.MaxValue, 0.1f) };

            /// <inheritdoc />
            public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                var window = Values[0] as VisjectSurfaceWindow<TAsset, TSurface, TPreview>;
                var asset = window?.Asset;
                if (asset == null)
                {
                    layout.Label("No parameters");
                    return;
                }
                if (!asset.IsLoaded)
                {
                    layout.Label("Loading...");
                    return;
                }
                var parameters = window.Surface.Parameters;

                for (int i = 0; i < parameters.Count; i++)
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
                    case ParameterType.CubeTexture:
                        pType = typeof(CubeTexture);
                        break;
                    case ParameterType.Texture:
                    case ParameterType.NormalMap:
                        pType = typeof(Texture);
                        break;
                    case ParameterType.GPUTexture:
                    case ParameterType.GPUTextureArray:
                    case ParameterType.GPUTextureCube:
                    case ParameterType.GPUTextureVolume:
                        pType = typeof(GPUTexture);
                        break;
                    default:
                        pType = p.Value.GetType();
                        // TODO: support custom attributes with defined value range for parameter (min, max)
                        attributes = DefaultAttributes;
                        break;
                    }

                    var propertyValue = new CustomValueContainer
                    (
                        pType,
                        pValue,
                        (instance, index) => ((VisjectSurfaceWindow<TAsset, TSurface, TPreview>)instance).GetParameter(pIndex),
                        (instance, index, value) => ((VisjectSurfaceWindow<TAsset, TSurface, TPreview>)instance).SetParameter(pIndex, value),
                        attributes
                    );

                    var propertyLabel = new DragablePropertyNameLabel(p.Name)
                    {
                        Tag = pIndex,
                        Drag = DragParameter
                    };
                    propertyLabel.MouseLeftDoubleClick += (label, location) => StartParameterRenaming(pIndex, label);
                    propertyLabel.MouseRightClick += (label, location) => ShowParameterMenu(pIndex, label, ref location);
                    var property = layout.AddPropertyItem(propertyLabel);
                    property.Object(propertyValue);
                }

                // Parameters creating
                var newParameterTypes = window.NewParameterTypes;
                if (newParameterTypes != null)
                {
                    if (parameters.Count > 0)
                        layout.Space(10);

                    var paramType = layout.Enum(newParameterTypes);
                    paramType.EnumComboBox.Value = (int)ParameterType.Float;
                    var newParam = layout.Button("Add parameter");
                    newParam.Button.Clicked += () => AddParameter((ParameterType)paramType.EnumComboBox.Value);
                }
            }

            private DragData DragParameter(DragablePropertyNameLabel label)
            {
                var window = (VisjectSurfaceWindow<TAsset, TSurface, TPreview>)Values[0];
                var parameter = window.Surface.Parameters[(int)label.Tag];
                return DragNames.GetDragData(SurfaceParameter.DragPrefix, parameter.Name);
            }

            /// <summary>
            /// Shows the parameter context menu.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="label">The label control.</param>
            /// <param name="targetLocation">The target location.</param>
            private void ShowParameterMenu(int index, Control label, ref Vector2 targetLocation)
            {
                var contextMenu = new ContextMenu();
                contextMenu.AddButton("Rename", () => StartParameterRenaming(index, label));
                contextMenu.AddButton("Delete", () => DeleteParameter(index));
                contextMenu.Show(label, targetLocation);
            }

            /// <summary>
            /// Adds the parameter.
            /// </summary>
            /// <param name="type">The type.</param>
            private void AddParameter(ParameterType type)
            {
                var window = (VisjectSurfaceWindow<TAsset, TSurface, TPreview>)Values[0];
                var material = window?.Asset;
                if (material == null || !material.IsLoaded)
                    return;
                var action = new AddRemoveParamAction
                {
                    Window = window,
                    IsAdd = true,
                    Name = "New parameter",
                    Type = type,
                    Index = window.Surface.Parameters.Count,
                };
                window.Surface.Undo.AddAction(action);
                action.Do();
            }

            /// <summary>
            /// Starts renaming parameter.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="label">The label control.</param>
            private void StartParameterRenaming(int index, Control label)
            {
                var window = (VisjectSurfaceWindow<TAsset, TSurface, TPreview>)Values[0];
                var parameter = window.Surface.Parameters[(int)label.Tag];
                var dialog = RenamePopup.Show(label, new Rectangle(0, 0, label.Width - 2, label.Height), parameter.Name, false);
                dialog.Tag = index;
                dialog.Renamed += OnParameterRenamed;
            }

            private void OnParameterRenamed(RenamePopup renamePopup)
            {
                var window = (VisjectSurfaceWindow<TAsset, TSurface, TPreview>)Values[0];
                var index = (int)renamePopup.Tag;
                var action = new RenameParamAction
                {
                    Window = window,
                    Index = index,
                    Before = window.Surface.Parameters[index].Name,
                    After = renamePopup.Text,
                };
                window.Surface.Undo.AddAction(action);
                action.Do();
            }

            /// <summary>
            /// Removes the parameter.
            /// </summary>
            /// <param name="index">The index.</param>
            private void DeleteParameter(int index)
            {
                var window = (VisjectSurfaceWindow<TAsset, TSurface, TPreview>)Values[0];
                var action = new AddRemoveParamAction
                {
                    Window = window,
                    IsAdd = false,
                    Index = index,
                };
                window.Surface.Undo.AddAction(action);
                action.Do();
            }
        }

        /// <summary>
        /// The primary split panel.
        /// </summary>
        protected readonly SplitPanel _split1;

        /// <summary>
        /// The secondary split panel.
        /// </summary>
        protected readonly SplitPanel _split2;

        /// <summary>
        /// The asset preview.
        /// </summary>
        protected TPreview _preview;

        /// <summary>
        /// The surface.
        /// </summary>
        protected TSurface _surface;

        private readonly ToolStripButton _saveButton;
        private readonly ToolStripButton _undoButton;
        private readonly ToolStripButton _redoButton;
        private bool _showWholeGraphOnLoad = true;

        /// <summary>
        /// The properties editor.
        /// </summary>
        protected CustomEditorPresenter _propertiesEditor;

        /// <summary>
        /// True if temporary asset is dirty, otherwise false.
        /// </summary>
        protected bool _tmpAssetIsDirty;

        /// <summary>
        /// True if window is waiting for asset load to load surface.
        /// </summary>
        protected bool _isWaitingForSurfaceLoad;

        /// <summary>
        /// True if window is waiting for asset load to refresh properties editor.
        /// </summary>
        protected bool _refreshPropertiesOnLoad;

        /// <summary>
        /// True if parameter value has been changed (special path for handling modifying surface parameters in properties editor).
        /// </summary>
        protected bool _paramValueChange;

        /// <summary>
        /// The undo.
        /// </summary>
        protected Undo _undo;

        /// <summary>
        /// The new parameter types enum type to use. Null to disable adding new parameters. Enum items must have values matching the <see cref="ParameterType"/> enum.
        /// </summary>
        protected Type NewParameterTypes;

        /// <summary>
        /// Gets the Visject Surface.
        /// </summary>
        public TSurface Surface => _surface;

        /// <summary>
        /// Gets the asset preview.
        /// </summary>
        public TPreview Preview => _preview;

        /// <summary>
        /// Gets the undo history context for this window.
        /// </summary>
        public Undo Undo => _undo;

        /// <inheritdoc />
        protected VisjectSurfaceWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Undo
            _undo = new Undo();
            _undo.UndoDone += OnUndo;
            _undo.RedoDone += OnUndo;
            _undo.ActionDone += OnUndo;

            // Split Panel 1
            _split1 = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Split Panel 2
            _split2 = new SplitPanel(Orientation.Vertical, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.4f,
                Parent = _split1.Panel2
            };

            // Properties editor
            _propertiesEditor = new CustomEditorPresenter(_undo);
            _propertiesEditor.Panel.Parent = _split2.Panel2;
            _propertiesEditor.Modified += OnPropertyEdited;

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Save32, Save).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _undoButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Undo32, _undo.PerformUndo).LinkTooltip("Undo (Ctrl+Z)");
            _redoButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Redo32, _undo.PerformRedo).LinkTooltip("Redo (Ctrl+Y)");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.PageScale32, ShowWholeGraph).LinkTooltip("Show whole graph");

            // Setup input actions
            InputActions.Add(options => options.Undo, _undo.PerformUndo);
            InputActions.Add(options => options.Redo, _undo.PerformRedo);
        }

        private void OnUndo(IUndoAction action)
        {
            // Hack for emitter properties proxy object
            if (action is MultiUndoAction multiUndo &&
                multiUndo.Actions.Length == 1 &&
                multiUndo.Actions[0] is UndoActionObject undoActionObject &&
                undoActionObject.Target == _propertiesEditor.Selection[0])
            {
                OnPropertyEdited();
                UpdateToolstrip();
                return;
            }

            _paramValueChange = false;
            MarkAsEdited();
            UpdateToolstrip();
            _propertiesEditor.BuildLayoutOnUpdate();
        }

        /// <summary>
        /// Gets the asset parameter.
        /// </summary>
        /// <param name="index">The zero-based parameter index.</param>
        /// <returns>The value.</returns>
        protected virtual object GetParameter(int index)
        {
            var param = Surface.Parameters[index];
            return param.Value;
        }

        /// <summary>
        /// Sets the asset parameter.
        /// </summary>
        /// <param name="index">The zero-based parameter index.</param>
        /// <param name="value">The value to set.</param>
        protected virtual void SetParameter(int index, object value)
        {
            var param = Surface.Parameters[index];
            var valueToSet = value;

            // Visject surface parameters are only value type objects so convert value if need to (eg. instead of texture ref write texture id)
            switch (param.Type)
            {
            case ParameterType.Asset:
            case ParameterType.Actor:
            case ParameterType.CubeTexture:
            case ParameterType.Texture:
            case ParameterType.NormalMap:
            case ParameterType.GPUTexture:
            case ParameterType.GPUTextureArray:
            case ParameterType.GPUTextureCube:
            case ParameterType.GPUTextureVolume:
                valueToSet = (value as FlaxEngine.Object)?.ID ?? Guid.Empty;
                break;
            }

            param.Value = valueToSet;
            _paramValueChange = true;
        }

        /// <summary>
        /// Called when the asset properties proxy object gets edited.
        /// </summary>
        protected virtual void OnPropertyEdited()
        {
            _surface.MarkAsEdited(!_paramValueChange);
            _paramValueChange = false;
        }

        /// <summary>
        /// Shows the whole surface graph.
        /// </summary>
        public void ShowWholeGraph()
        {
            _surface.ShowWholeGraph();
        }

        /// <summary>
        /// Refreshes temporary asset to see changes live when editing the surface.
        /// </summary>
        /// <returns>True if cannot refresh it, otherwise false.</returns>
        public bool RefreshTempAsset()
        {
            // Early check
            if (_asset == null || _isWaitingForSurfaceLoad)
                return true;

            // Check if surface has been edited
            if (_surface.IsEdited)
            {
                return SaveSurface();
            }

            return false;
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the original asset
            if (!IsEdited)
                return;

            // Just in case refresh data
            if (RefreshTempAsset())
            {
                return;
            }

            // Update original Particle Emitter so user can see changes in the scene
            if (SaveToOriginal())
            {
                return;
            }

            // Setup
            ClearEditedFlag();
            OnSurfaceEditedChanged();
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
            _isWaitingForSurfaceLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _isWaitingForSurfaceLoad = true;
            _refreshPropertiesOnLoad = false;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public abstract string SurfaceName { get; }

        /// <inheritdoc />
        public abstract byte[] SurfaceData { get; set; }

        /// <inheritdoc />
        public void OnContextCreated(VisjectSurfaceContext context)
        {
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
            _tmpAssetIsDirty = true;
        }

        /// <inheritdoc />
        public void OnSurfaceClose()
        {
            Close();
        }

        /// <summary>
        /// Called when parameter rename undo action is performed.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnParamRenameUndo(RenameParamAction action)
        {
        }

        /// <summary>
        /// Called when parameter add undo action is performed.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnParamAddUndo(AddRemoveParamAction action)
        {
            _refreshPropertiesOnLoad = true;
        }

        /// <summary>
        /// Called when parameter remove undo action is performed.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnParamRemoveUndo(AddRemoveParamAction action)
        {
            _refreshPropertiesOnLoad = true;
            //_propertiesEditor.BuildLayoutOnUpdate();
            _propertiesEditor.BuildLayout();
        }

        /// <summary>
        /// Loads the surface from the asset. Called during <see cref="Update"/> when asset is loaded and surface is missing.
        /// </summary>
        /// <returns>True if failed, otherwise false.</returns>
        protected abstract bool LoadSurface();

        /// <summary>
        /// Saves the surface to the asset. Called during <see cref="Update"/> when asset is loaded and surface is missing.
        /// </summary>
        /// <returns>True if failed, otherwise false.</returns>
        protected abstract bool SaveSurface();

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (_tmpAssetIsDirty)
            {
                _tmpAssetIsDirty = false;

                RefreshTempAsset();
            }

            if (_isWaitingForSurfaceLoad && _asset.IsLoaded)
            {
                _isWaitingForSurfaceLoad = false;

                if (LoadSurface())
                {
                    Close();
                    return;
                }

                // Setup
                _undo.Clear();
                _surface.Enabled = true;
                _propertiesEditor.BuildLayout();
                ClearEditedFlag();
                if (_showWholeGraphOnLoad)
                {
                    _showWholeGraphOnLoad = false;
                    _surface.ShowWholeGraph();
                }
            }
            else if (_refreshPropertiesOnLoad && _asset.IsLoaded)
            {
                _refreshPropertiesOnLoad = false;

                _propertiesEditor.BuildLayout();
            }
        }

        /// <inheritdoc />
        public override bool UseLayoutData => true;

        /// <inheritdoc />
        public override void OnLayoutSerialize(XmlWriter writer)
        {
            writer.WriteAttributeString("Split1", _split1.SplitterValue.ToString());
            writer.WriteAttributeString("Split2", _split2.SplitterValue.ToString());
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize(XmlElement node)
        {
            float value1;

            if (float.TryParse(node.GetAttribute("Split1"), out value1))
                _split1.SplitterValue = value1;
            if (float.TryParse(node.GetAttribute("Split2"), out value1))
                _split2.SplitterValue = value1;
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize()
        {
            _split1.SplitterValue = 0.7f;
            _split2.SplitterValue = 0.4f;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _undo.Clear();
            _propertiesEditor.Deselect();

            base.OnDestroy();
        }
    }
}
