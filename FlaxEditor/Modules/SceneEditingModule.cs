// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.SceneGraph;
using FlaxEngine;

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
        public readonly List<ISceneTreeNode> Selection = new List<ISceneTreeNode>(64);

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
            Select(Editor.Windows.SceneWin.Root.ChildNodes);
        }

        /// <summary>
        /// Selects the specified collection of objects.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select(List<ISceneTreeNode> selection, bool additive = false)
        {
            if (selection == null)
                throw new ArgumentNullException();

            // Check if won't change
            if (!additive && Selection.Count == selection.Count && Selection.SequenceEqual(selection))
                return;

            if (!additive)
                Selection.Clear();
            Selection.AddRange(selection);

            SelectionChanged();
        }

        /// <summary>
        /// Selects the specified collection of objects.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select(ISceneTreeNode[] selection, bool additive = false)
        {
            if (selection == null)
                throw new ArgumentNullException();

            // Check if won't change
            if (!additive && Selection.Count == selection.Length && Selection.SequenceEqual(selection))
                return;

            if (!additive)
                Selection.Clear();
            Selection.AddRange(selection);

            SelectionChanged();
        }
        
        /// <summary>
        /// Selects the specified object.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select(ISceneTreeNode selection, bool additive = false)
        {
            if (selection == null)
                throw new ArgumentNullException();

            // Check if won't change
            if (!additive && Selection.Count == 1 && Selection[0] == selection)
                return;

            if (!additive)
                Selection.Clear();
            Selection.Add(selection);

            SelectionChanged();
        }

        /// <summary>
        /// Deselects given object.
        /// </summary>
        public void Deselect(ISceneTreeNode node)
        {
            if (!Selection.Remove(node))
                return;
            
            SelectionChanged();
        }

        /// <summary>
        /// Clears selected objects collection.
        /// </summary>
        public void Deselect()
        {
            // Check if won't change
            if (Selection.Count == 0)
                return;

            Selection.Clear();

            SelectionChanged();
        }

        private void SelectionChanged()
        {
            OnSelectionChanged?.Invoke();
        }

        /// <summary>
        /// Deletes the selected objects. Supports undo/redo.
        /// </summary>
        public void Delete()
        {
            throw new NotImplementedException("TODO: implement Delete");
        }

        /// <summary>
        /// Copies the selected objects.
        /// </summary>
        public void Copy()
        {
            throw new NotImplementedException("TODO: implement Copy");
        }

        /// <summary>
        /// Pastes the copied objects.
        /// </summary>
        public void Paste()
        {
            throw new NotImplementedException("TODO: implement Paste");
        }

        /// <summary>
        /// Cuts the selected objects.
        /// </summary>
        public void Cut()
        {
            throw new NotImplementedException("TODO: implement Cut");
        }

        /// <summary>
        /// Duplicates the selected objects.
        /// </summary>
        public void Duplicate()
        {
            throw new NotImplementedException("TODO: implement duplicate");
        }
    }
}
