// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.Gizmo;
using FlaxEditor.GUI;
using FlaxEditor.SceneGraph;
using FlaxEditor.Viewport;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Prefab window allows to view and edit <see cref="Prefab"/> asset.
    /// </summary>
    /// <seealso cref="Prefab" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed partial class PrefabWindow : AssetEditorWindowBase<Prefab>
    {
        private readonly SplitPanel _split1;
        private readonly SplitPanel _split2;
        private readonly PrefabTree _tree;
        private readonly PrefabWindowViewport _viewport;
        private readonly CustomEditorPresenter _propertiesEditor;

        private readonly ToolStripButton _saveButton;
        private readonly ToolStripButton _toolStripUndo;
        private readonly ToolStripButton _toolStripRedo;
        private readonly ToolStripButton _toolStripTranslate;
        private readonly ToolStripButton _toolStripRotate;
        private readonly ToolStripButton _toolStripScale;

        private Undo _undo;
        private bool _focusCamera;
        private bool _isUpdatingSelection;

        /// <summary>
        /// Gets the prefab hierarchy tree control.
        /// </summary>
        public PrefabTree Tree => _tree;

        /// <summary>
        /// Gets the viewport.
        /// </summary>
        public PrefabWindowViewport Viewport => _viewport;

        /// <summary>
        /// Gets the undo system used by this window for changes tracking.
        /// </summary>
        public Undo Undo => _undo;

        /// <summary>
        /// The local scene nodes graph used by the prefab editor.
        /// </summary>
        public readonly LocalSceneGraph Graph;

        /// <inheritdoc />
        public PrefabWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Undo
            _undo = new Undo();
            _undo.UndoDone += OnUndoEvent;
            _undo.RedoDone += OnUndoEvent;
            _undo.ActionDone += OnUndoEvent;

            // Split Panel 1
            _split1 = new SplitPanel(Orientation.Horizontal, ScrollBars.Both, ScrollBars.None)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.2f,
                Parent = this
            };

            // Split Panel 2
            _split2 = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.6f,
                Parent = _split1.Panel2
            };

            // Prefab structure tree
            Graph = new LocalSceneGraph(new CustomRootNode(this));
            _tree = new PrefabTree();
            _tree.Margin = new Margin(0.0f, 0.0f, -14.0f, 0.0f); // Hide root node
            _tree.AddChild(Graph.Root.TreeNode);
            _tree.SelectedChanged += OnTreeSelectedChanged;
            _tree.RightClick += OnTreeRightClick;
            _tree.Parent = _split1.Panel1;

            // Prefab viewport
            _viewport = new PrefabWindowViewport(this)
            {
                Parent = _split2.Panel1
            };
            _viewport.TransformGizmo.ModeChanged += UpdateToolstrip;
            _viewport.ViewWidgetButtonMenu.AddSeparator();
            _viewport.ViewWidgetButtonMenu.AddButton("Show Default Scene", () => _viewport.ShowDefaultSceneActors = !_viewport.ShowDefaultSceneActors).SetChecked(true).SetAutoCheck(true).LinkTooltip("Shows/hides the default preview scene actors (sky light, directional light and sky)");
            
            // Prefab properties editor
            _propertiesEditor = new CustomEditorPresenter(_undo);
            _propertiesEditor.Panel.Parent = _split2.Panel2;
            _propertiesEditor.Modified += MarkAsEdited;

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.UI.GetIcon("Save32"), Save).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _toolStripUndo = (ToolStripButton)_toolstrip.AddButton(Editor.UI.GetIcon("Undo32"), _undo.PerformUndo).LinkTooltip("Undo (Ctrl+Z)");
            _toolStripRedo = (ToolStripButton)_toolstrip.AddButton(Editor.UI.GetIcon("Redo32"), _undo.PerformRedo).LinkTooltip("Redo (Ctrl+Y)");
            _toolstrip.AddSeparator();
            _toolStripTranslate = (ToolStripButton)_toolstrip.AddButton(Editor.UI.GetIcon("Translate32"), () => _viewport.TransformGizmo.ActiveMode = TransformGizmo.Mode.Translate).LinkTooltip("Change Gizmo tool mode to Translate (1)");
            _toolStripRotate = (ToolStripButton)_toolstrip.AddButton(Editor.UI.GetIcon("Rotate32"), () => _viewport.TransformGizmo.ActiveMode = TransformGizmo.Mode.Rotate).LinkTooltip("Change Gizmo tool mode to Rotate (2)");
            _toolStripScale = (ToolStripButton)_toolstrip.AddButton(Editor.UI.GetIcon("Scale32"), () => _viewport.TransformGizmo.ActiveMode = TransformGizmo.Mode.Scale).LinkTooltip("Change Gizmo tool mode to Scale (3)");

            Editor.Prefabs.PrefabApplied += OnPrefabApplied;
        }

        private void OnUndoEvent(IUndoAction action)
        {
            // All undo actions modify the asset except selection change
            if (!(action is SelectionChangeAction))
            {
                MarkAsEdited();
            }

            UpdateToolstrip();
        }

        private void OnPrefabApplied(Prefab prefab, Actor instance)
        {
            if (prefab == Asset)
            {
                ClearEditedFlag();

                _item.RefreshThumbnail();
            }
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the orginal asset
            if (!IsEdited)
                return;

            // Simply update changes
            Editor.Prefabs.ApplyAll(_viewport.Instance);
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            var undoRedo = _undo;
            var gizmo = _viewport.TransformGizmo;

            _saveButton.Enabled = IsEdited;
            _toolStripUndo.Enabled = undoRedo.CanUndo;
            _toolStripRedo.Enabled = undoRedo.CanRedo;
            //
            var gizmoMode = gizmo.ActiveMode;
            _toolStripTranslate.Checked = gizmoMode == TransformGizmo.Mode.Translate;
            _toolStripRotate.Checked = gizmoMode == TransformGizmo.Mode.Rotate;
            _toolStripScale.Checked = gizmoMode == TransformGizmo.Mode.Scale;

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _viewport.Prefab = _asset;
            Graph.MainActor = _viewport.Instance;
            _focusCamera = true;
            Selection.Clear();
            Select(Graph.Main);
            Graph.Root.TreeNode.ExpandAll(true);

            _undo.Clear();
            ClearEditedFlag();

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            Deselect();
            Graph.MainActor = null;
            _viewport.Prefab = null;
            _undo?.Clear();

            base.UnlinkItem();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (_focusCamera && _viewport.Task.FrameCount > 1)
            {
                _focusCamera = false;

                // Auto fit
                BoundingSphere bounds;
                Editor.GetActorEditorSphere(_viewport.Instance, out bounds);
                _viewport.ViewPosition = bounds.Center - _viewport.ViewDirection * (bounds.Radius * 1.2f);
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
                        Focus();
                        return true;
                    case Keys.Y:
                        _undo.PerformRedo();
                        Focus();
                        return true;
                    case Keys.X:
                        Cut();
                        break;
                    case Keys.C:
                        Copy();
                        break;
                    case Keys.V:
                        Paste();
                        break;
                    case Keys.D:
                        Duplicate();
                        break;
                    }
                }
                else
                {
                    switch (key)
                    {
                    case Keys.Delete:
                        Delete();
                        break;
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
            _split1.SplitterValue = 0.2f;
            _split2.SplitterValue = 0.6f;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            _undo.Dispose();
            Graph.Dispose();

            base.Dispose();
        }
    }
}
