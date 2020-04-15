// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using FlaxEditor.Content;
using FlaxEditor.GUI;
using FlaxEditor.Surface;
using FlaxEngine;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// The base class for editor windows that use <see cref="FlaxEditor.Surface.VisjectSurface"/> for content editing by graph functions (eg. material functions, particle emitter functions).
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    /// <seealso cref="FlaxEditor.Surface.IVisjectSurfaceOwner" />
    public abstract class VisjectFunctionSurfaceWindow<TAsset, TSurface> : AssetEditorWindowBase<TAsset>, IVisjectSurfaceOwner
    where TAsset : Asset
    where TSurface : VisjectSurface
    {
        /// <summary>
        /// The surface.
        /// </summary>
        protected TSurface _surface;

        private readonly ToolStripButton _saveButton;
        private readonly ToolStripButton _undoButton;
        private readonly ToolStripButton _redoButton;
        private bool _showWholeGraphOnLoad = true;

        /// <summary>
        /// True if asset is dirty, otherwise false.
        /// </summary>
        protected bool _assetIsDirty;

        /// <summary>
        /// True if window is waiting for asset load to load surface.
        /// </summary>
        protected bool _isWaitingForSurfaceLoad;

        /// <summary>
        /// The undo.
        /// </summary>
        protected Undo _undo;

        /// <summary>
        /// Gets the Visject Surface.
        /// </summary>
        public TSurface Surface => _surface;

        /// <summary>
        /// Gets the undo history context for this window.
        /// </summary>
        public Undo Undo => _undo;

        /// <inheritdoc />
        protected VisjectFunctionSurfaceWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Undo
            _undo = new Undo();
            _undo.UndoDone += OnUndoRedo;
            _undo.RedoDone += OnUndoRedo;
            _undo.ActionDone += OnUndoRedo;

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

        private void OnUndoRedo(IUndoAction action)
        {
            MarkAsEdited();
            UpdateToolstrip();
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
            if (!IsEdited)
                return;

            if (RefreshTempAsset())
            {
                return;
            }

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
            _assetIsDirty = true;
        }

        /// <inheritdoc />
        public void OnSurfaceClose()
        {
            Close();
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

            if (_assetIsDirty)
            {
                _assetIsDirty = false;

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
                ClearEditedFlag();
                if (_showWholeGraphOnLoad)
                {
                    _showWholeGraphOnLoad = false;
                    _surface.ShowWholeGraph();
                }
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _undo.Clear();

            base.OnDestroy();
        }
    }
}
