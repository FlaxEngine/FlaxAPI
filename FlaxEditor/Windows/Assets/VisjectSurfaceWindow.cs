// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
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
        protected class EditParamAction : IUndoAction
        {
            public VisjectSurfaceWindow<TAsset, TSurface, TPreview> Window;
            public int Index;
            public object Before;
            public object After;

            /// <inheritdoc />
            public string ActionString => "Edit parameter";

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

            private void Set(object value)
            {
                var param = Window.Surface.Parameters[Index];
                var valueToSet = value;

                // Visject surface parameters are only value type objects so convert value if need to (eg. instead of texture ref write texture id)
                switch (param.Type)
                {
                case ParameterType.Asset:
                case ParameterType.Actor:
                case ParameterType.CubeTexture:
                case ParameterType.Texture:
                case ParameterType.NormalMap:
                case ParameterType.RenderTarget:
                case ParameterType.RenderTargetArray:
                case ParameterType.RenderTargetCube:
                case ParameterType.RenderTargetVolume:
                    valueToSet = (value as FlaxEngine.Object)?.ID ?? Guid.Empty;
                    break;
                }

                param.Value = valueToSet;
                Window.OnParamEditUndo(this, value);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                Window = null;
                Before = null;
                After = null;
            }
        }

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
                    string typeName;
                    Guid id;
                    FlaxEngine.Content.GetAssetInfo(StringUtils.CombinePaths(Globals.EngineFolder, "Textures/NormalTexture.flax"), out typeName, out id);
                    param.Value = id;
                }
                Window.Surface.Parameters.Insert(Index, param);
                Window.Surface.OnParamCreated(param);
            }

            private void Remove()
            {
                var param = Window.Surface.Parameters[Index];
                Name = param.Name;
                Type = param.Type;
                Window.Surface.Parameters.RemoveAt(Index);
                Window.Surface.OnParamDeleted(param);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                Window = null;
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
        }

        private void OnUndo(IUndoAction action)
        {
            // Hack for emitter properties proxy object
            if (action is MultiUndoAction multiUndo && multiUndo.Actions.Length == 1 && multiUndo.Actions[0] is UndoActionObject undoActionObject && undoActionObject.Target == _propertiesEditor.Selection[0])
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
        /// Called when parameter edit undo action is performed.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="value">The new parameter value.</param>
        protected virtual void OnParamEditUndo(EditParamAction action, object value)
        {
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
        }

        /// <summary>
        /// Called when parameter remove undo action is performed.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnParamRemoveUndo(AddRemoveParamAction action)
        {
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
            }
            else if (_refreshPropertiesOnLoad && _asset.IsLoaded)
            {
                _refreshPropertiesOnLoad = false;

                _propertiesEditor.BuildLayout();
            }
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            // Base
            if (base.OnKeyDown(key))
                return true;

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

            return false;
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
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            _undo.Clear();
            _propertiesEditor.Deselect();

            base.Dispose();
        }
    }
}
