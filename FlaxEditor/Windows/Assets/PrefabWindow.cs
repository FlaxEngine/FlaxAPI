// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FlaxEditor.Actions;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.Gizmo;
using FlaxEditor.GUI;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.GUI;
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
    public sealed class PrefabWindow : AssetEditorWindowBase<Prefab>
    {
        /// <summary>
        /// The prefab hierarchy tree control.
        /// </summary>
        /// <seealso cref="FlaxEditor.GUI.Tree" />
        public class PrefabTree : Tree
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PrefabTree"/> class.
            /// </summary>
            public PrefabTree()
            : base(true)
            {
            }
        }

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
        /// The current selection (readonly).
        /// </summary>
        public readonly List<SceneGraphNode> Selection = new List<SceneGraphNode>();

        /// <summary>
        /// Occurs when selection gets changed.
        /// </summary>
        public event Action SelectionChanged;

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
            _undo.UndoDone += UpdateToolstrip;
            _undo.RedoDone += UpdateToolstrip;
            _undo.ActionDone += UpdateToolstrip;

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
            Graph = new LocalSceneGraph();
            _tree = new PrefabTree();
            _tree.Margin = new Margin(0.0f, 0.0f, -14.0f, 0.0f); // Hide root node
            _tree.AddChild(Graph.Root.TreeNode);
            _tree.SelectedChanged += OnTreeSelectedChanged;
            //_tree.RightClick += Tree_OnRightClick;
            _tree.Parent = _split1.Panel1;

            // Prefab viewport
            _viewport = new PrefabWindowViewport(this)
            {
                Parent = _split2.Panel1
            };
            _viewport.TransformGizmo.ModeChanged += UpdateToolstrip;

            // Prefab properties editor
            _propertiesEditor = new CustomEditorPresenter(_undo, "Loading...");
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

        private void OnTreeSelectedChanged(List<TreeNode> before, List<TreeNode> after)
        {
            // Check if lock events
            if (_isUpdatingSelection)
                return;

            if (after.Count > 0)
            {
                // Get actors from nodes
                var actors = new List<SceneGraphNode>(after.Count);
                for (int i = 0; i < after.Count; i++)
                {
                    if (after[i] is ActorTreeNode node && node.Actor)
                        actors.Add(node.ActorNode);
                }

                // Select
                Select(actors);
            }
            else
            {
                // Deselect
                Deselect();
            }
        }

        private void OnPrefabApplied(Prefab prefab, Actor instance)
        {
            if (prefab == Asset)
            {
                ClearEditedFlag();

                _item.RefreshThumbnail();
            }
        }

        /// <summary>
        /// Called when selection gets changed.
        /// </summary>
        /// <param name="before">The selection before the change.</param>
        public void OnSelectionChanged(SceneGraphNode[] before)
        {
            Undo.AddAction(new SelectionChangeAction(before, Selection.ToArray(), OnSelectionUndo));

            OnSelectionChanges();
        }

        private void OnSelectionUndo(SceneGraphNode[] toSelect)
        {
            Selection.Clear();
            Selection.AddRange(toSelect);

            OnSelectionChanges();
        }

        private void OnSelectionChanges()
        {
            _isUpdatingSelection = true;

            // Update tree
            var selection = Selection;
            if (selection.Count == 0)
            {
                _tree.Deselect();
            }
            else
            {
                // Find nodes to select
                var nodes = new List<TreeNode>(selection.Count);
                for (int i = 0; i < selection.Count; i++)
                {
                    if (selection[i] is ActorNode node)
                    {
                        nodes.Add(node.TreeNode);
                    }
                }

                // Select nodes
                _tree.Select(nodes);

                // For single node selected scroll view so user can see it
                if (nodes.Count == 1)
                {
                    ScrollViewTo(nodes[0]);
                }
            }

            // Update properties editor
            var objects = Selection.ConvertAll(x => x.EditableObject).Distinct();
            _propertiesEditor.Select(objects);

            _isUpdatingSelection = false;

            // Send event
            SelectionChanged?.Invoke();
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
            _undo.Clear();
            Selection.Clear();
            Select(Graph.Main);
            Graph.Root.TreeNode.ExpandAll(true);

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            Deselect();
            Graph.Dispose();
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

        /// <summary>
        /// Selects the specified nodes collection.
        /// </summary>
        /// <param name="nodes">The nodec.</param>
        public void Select(List<SceneGraphNode> nodes)
        {
            if (nodes == null || nodes.Count == 0)
            {
                Deselect();
                return;
            }
            if (Utils.ArraysEqual(Selection, nodes))
                return;

            var before = Selection.ToArray();
            Selection.Clear();
            Selection.AddRange(nodes);
            OnSelectionChanged(before);
        }

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Select(SceneGraphNode node)
        {
            if (node == null)
            {
                Deselect();
                return;
            }
            if (Selection.Count == 1 && Selection[0] == node)
                return;

            var before = Selection.ToArray();
            Selection.Clear();
            Selection.Add(node);
            OnSelectionChanged(before);
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void Deselect()
        {
            if (Selection.Count == 0)
                return;

            var before = Selection.ToArray();
            Selection.Clear();
            OnSelectionChanged(before);
        }

        /// <summary>
        /// Cuts selected objects.
        /// </summary>
        public void Cut()
        {
            Copy();
            Delete();
        }

        /// <summary>
        /// Copies selected objects to system clipboard.
        /// </summary>
        public void Copy()
        {
            // Peek things that can be copied (copy all acctors)
            var objects = Selection.Where(x => x.CanCopyPaste).ToList().BuildAllNodes().Where(x => x.CanCopyPaste && x is ActorNode).ToList();
            if (objects.Count == 0)
                return;

            // Serialize actors
            var actors = objects.ConvertAll(x => ((ActorNode)x).Actor);
            var data = Actor.ToBytes(actors.ToArray());
            if (data == null)
            {
                Editor.LogError("Failed to copy actors data.");
                return;
            }

            // Copy data
            Application.ClipboardRawData = data;
        }

        /// <summary>
        /// Pastes objects from the system clipboard.
        /// </summary>
        public void Paste()
        {
            Paste(null);
        }

        /// <summary>
        /// Pastes the copied objects. Supports undo/redo.
        /// </summary>
        /// <param name="pasteTargetActor">The target actor to paste copied data.</param>
        public void Paste(Actor pasteTargetActor)
        {
            // Get clipboard data
            var data = Application.ClipboardRawData;

            // Ser aste target if only one actor is selected and no target provided
            if (pasteTargetActor == null && Selection.Count == 1 && Selection[0] is ActorNode actorNode)
            {
                pasteTargetActor = actorNode.Actor;
            }

            // Create paste action
            var pasteAction = CustomPasteActorsAction.CustomPaste(this, data, pasteTargetActor?.ID ?? Guid.Empty);
            if (pasteAction != null)
            {
                OnPasteAcction(pasteAction);
            }
        }

        /// <summary>
        /// Duplicates selected objects.
        /// </summary>
        public void Duplicate()
        {
            // Peek things that can be copied (copy all acctors)
            var objects = Selection.Where(x => x.CanCopyPaste && x != Graph.Main).ToList().BuildAllNodes().Where(x => x.CanCopyPaste && x is ActorNode).ToList();
            if (objects.Count == 0)
                return;

            // Serialize actors
            var actors = objects.ConvertAll(x => ((ActorNode)x).Actor);
            var data = Actor.ToBytes(actors.ToArray());
            if (data == null)
            {
                Editor.LogError("Failed to copy actors data.");
                return;
            }

            // Create paste action (with selecting spawned objects)
            var pasteAction = CustomPasteActorsAction.CustomDuplicate(this, data, Guid.Empty);
            if (pasteAction != null)
            {
                OnPasteAcction(pasteAction);
            }
        }

        private void OnPasteAcction(PasteActorsAction pasteAction)
        {
            pasteAction.Do(out _, out var nodeParents);

            // Select spawned objects
            var selectAction = new SelectionChangeAction(Selection.ToArray(), nodeParents.Cast<SceneGraphNode>().ToArray(), OnSelectionUndo);
            selectAction.Do();

            Undo.AddAction(new MultiUndoAction(pasteAction, selectAction));
            OnSelectionChanges();
        }

        private class CustomDeleteActorsAction : DeleteActorsAction
        {
            public CustomDeleteActorsAction(List<SceneGraphNode> objects)
            : base(objects)
            {
            }

            /// <inheritdoc />
            protected override void Delete()
            {
                var nodes = _nodeParents.ToArray();

                // Unlink nodes from parents (actors spawned for prefab editing are not in a gameplay and may not send some important events)
                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].Actor.Parent = null;
                }

                base.Delete();

                // Remove nodes (actors in prefab are not in a gameplay and some events from the engine may not be send eg. ActorDeleted)
                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].Dispose();
                }
            }
        }

        private class CustomPasteActorsAction : PasteActorsAction
        {
            private PrefabWindow _window;

            private CustomPasteActorsAction(PrefabWindow window, byte[] data, Guid[] objectIds, ref Guid pasteParent, string name)
            : base(data, objectIds, ref pasteParent, name)
            {
                _window = window;
            }

            internal static CustomPasteActorsAction CustomPaste(PrefabWindow window, byte[] data, Guid pasteParent)
            {
                var objectIds = Actor.TryGetSerializedObjectsIds(data);
                if (objectIds == null)
                    return null;

                return new CustomPasteActorsAction(window, data, objectIds, ref pasteParent, "Paste actors");
            }

            internal static CustomPasteActorsAction CustomDuplicate(PrefabWindow window, byte[] data, Guid pasteParent)
            {
                var objectIds = Actor.TryGetSerializedObjectsIds(data);
                if (objectIds == null)
                    return null;

                return new CustomPasteActorsAction(window, data, objectIds, ref pasteParent, "Duplicate actors");
            }

            /// <inheritdoc />
            protected override void LinkBrokenParentReference(Actor actor)
            {
                // Link to prefab root
                actor.SetParent(_window.Graph.MainActor, false);
            }

            /// <inheritdoc />
            public override void Undo()
            {
                var nodes = _nodeParents.ToArray();

                for (int i = 0; i < nodes.Length; i++)
                {
                    var node = SceneGraphFactory.FindNode(_nodeParents[i]);
                    if (node != null)
                    {
                        // Unlink nodes from parents (actors spawned for prefab editing are not in a gameplay and may not send some important events)
                        if (node is ActorNode actorNode)
                            actorNode.Actor.Parent = null;

                        // Remove objects
                        node.Delete();

                        // Remove nodes (actors in prefab are not in a gameplay and some events from the engine may not be send eg. ActorDeleted)
                        node.Dispose();
                    }
                }

                _nodeParents.Clear();
            }
        }

        /// <summary>
        /// Deletes selected objects.
        /// </summary>
        public void Delete()
        {
            // Peek things that can be removed
            var objects = Selection.Where(x => x.CanDelete && x != Graph.Main).ToList().BuildAllNodes().Where(x => x.CanDelete).ToList();
            if (objects.Count == 0)
                return;

            // Change selection
            var action1 = new SelectionChangeAction(Selection.ToArray(), new SceneGraphNode[0], OnSelectionUndo);

            // Delete objects
            var action2 = new CustomDeleteActorsAction(objects);

            // Merge actions and perform them
            var action = new MultiUndoAction(new IUndoAction[]
            {
                action1,
                action2
            }, action2.ActionString);
            action.Do();
            Undo.AddAction(action);
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
            _undo = null;

            base.Dispose();
        }
    }
}
