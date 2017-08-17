////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Actions;
using FlaxEditor.SceneGraph;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Editing scenes module. Manages scene objects selection and editing modes.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class SceneEditingModule : EditorModule
    {
        /// <summary>
        /// The selected objects.
        /// </summary>
        public readonly List<SceneGraphNode> Selection = new List<SceneGraphNode>(64);

        /// <summary>
        /// Gets the amount of the selected objects.
        /// </summary>
        public int SelectionCount => Selection.Count;

        /// <summary>
        /// Gets a value indicating whether any object is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if any object is selected; otherwise, <c>false</c>.
        /// </value>
        public bool HasSthSelected => Selection.Count > 0;

        /// <summary>
        /// Occurs when selected objects colelction gets changed.
        /// </summary>
        public event Action OnSelectionChanged;

        internal SceneEditingModule(Editor editor)
            : base(editor)
        {
        }

        /// <summary>
        /// Selects all scenes.
        /// </summary>
        public void SelectAllScenes()
        {
            // Select all sccenes (linked to the root node)
            Select(Editor.Scene.Root.ChildNodes);
        }

        /// <summary>
        /// Selects the specified collection of objects.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select(List<SceneGraphNode> selection, bool additive = false)
        {
            if (selection == null)
                throw new ArgumentNullException();

            // Prevent from selecting null nodes
            selection.RemoveAll(x => x == null);

            // Check if won't change
            if (!additive && Selection.Count == selection.Count && Selection.SequenceEqual(selection))
                return;

            var before = Selection.ToArray();
            if (!additive)
                Selection.Clear();
            Selection.AddRange(selection);

            SelectionChange(before);
        }

        /// <summary>
        /// Selects the specified collection of objects.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select(SceneGraphNode[] selection, bool additive = false)
        {
            if (selection == null)
                throw new ArgumentNullException();

            Select(selection.ToList(), additive);
        }

        /// <summary>
        /// Selects the specified object.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select(SceneGraphNode selection, bool additive = false)
        {
            if (selection == null)
                throw new ArgumentNullException();

            // Check if won't change
            if (!additive && Selection.Count == 1 && Selection[0] == selection)
                return;

            var before = Selection.ToArray();
            if (!additive)
                Selection.Clear();
            Selection.Add(selection);

            SelectionChange(before);
        }

        /// <summary>
        /// Deselects given object.
        /// </summary>
        public void Deselect(SceneGraphNode node)
        {
            if (!Selection.Contains(node))
                return;

            var before = Selection.ToArray();
            Selection.Remove(node);

            SelectionChange(before);
        }

        /// <summary>
        /// Clears selected objects collection.
        /// </summary>
        public void Deselect()
        {
            // Check if won't change
            if (Selection.Count == 0)
                return;

            var before = Selection.ToArray();
            Selection.Clear();

            SelectionChange(before);
        }

        private void SelectionChange(SceneGraphNode[] before)
        {
            Undo.AddAction(new SelectionChangeAction(before, Selection.ToArray()));

            OnSelectionChanged?.Invoke();
        }

        internal void OnSelectionUndo(SceneGraphNode[] toSelect)
        {
            Selection.Clear();
            if (toSelect != null && toSelect.Length > 0)
                Selection.AddRange(toSelect);

            OnSelectionChanged?.Invoke();
        }

        /// <summary>
        /// Deletes the selected objects. Supports undo/redo.
        /// </summary>
        public void Delete()
        {
            // Peek things that can be removed
            var objects = Selection.BuildAllNodes().Where(x => x.CanDelete).ToList();
            if (objects.Count == 0)
                return;

            // Change selection
            var action1 = new SelectionChangeAction(Selection.ToArray(), new SceneGraphNode[0]);

            // Delete objects
            var action2 = new DeleteNodesAction(objects);

            // Merge two actions and perform them
            var action = new MultiUndoAction(new IUndoAction[] { action1, action2 }, action2.ActionString);
            action.Do();
            Undo.AddAction(action);
        }

        /// <summary>
        /// Copies the selected objects.
        /// </summary>
        public void Copy()
        {
            // Peek things that can be copied (copy all acctors)
            var objects = Selection.BuildAllNodes().Where(x => x.CanCopyPaste).ToList();
            if (objects.Count == 0)
                return;

            throw new NotImplementedException("TODO: implement Copy");
        }

        /// <summary>
        /// Pastes the copied objects. Supports undo/redo.
        /// </summary>
        public void Paste()
        {
            throw new NotImplementedException("TODO: implement Paste");
        }

        /// <summary>
        /// Cuts the selected objects. Supports undo/redo.
        /// </summary>
        public void Cut()
        {
            Copy();
            Delete();
        }

        /// <summary>
        /// Duplicates the selected objects. Supports undo/redo.
        /// </summary>
        public void Duplicate()
        {
            // Peek things that can be duplicated (duplicate only top objects)
            var objects = Selection.BuildNodesParents().Where(x => x.CanCopyPaste).ToList();
            if (objects.Count == 0)
                return;

            throw new NotImplementedException("TODO: implement Duplicate");
        }
    }
}
