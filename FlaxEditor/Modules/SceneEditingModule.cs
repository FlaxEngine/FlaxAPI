// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Editing scenes module. Manages scene objects selection and editing modes.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class SceneEditingModule : EditorModule
    {
        private List<Actor> _selectedActors;

        /// <summary>
        /// The selected objects.
        /// </summary>
        public readonly List<object> Selection = new List<object>(64);

        /// <summary>
        /// Gets the selected actors collection.
        /// </summary>
        /// <value>
        /// The selected actors.
        /// </value>
        public List<Actor> SelectedActors => _selectedActors;

        /// <summary>
        /// Occurs when selected objects colelction gets changed.
        /// </summary>
        public event Action OnSelectionChanged;

        internal SceneEditingModule(Editor editor)
            : base(editor)
        {
            _selectedActors = new List<Actor>();
        }

        /// <summary>
        /// Selects all scenes.
        /// </summary>
        public void SelectAllScenes()
        {
            // Select them
            Select(SceneManager.Scenes);
        }

        /// <summary>
        /// Selects the specified collection of objects.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select<T>(List<T> selection, bool additive = false)
            where T : class
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
        public void Select<T>(T[] selection, bool additive = false)
            where T : class
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
        /// Selects the specified collection of objects.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select<T>(IEnumerable<T> selection, bool additive = false)
            where T : class
        {
            if (selection == null)
                throw new ArgumentNullException();

            // Check if won't change
            if (!additive && Selection.Count == selection.Count() && Selection.SequenceEqual(selection))
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
        public void Select(object selection, bool additive = false)
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
            // Cache data
            _selectedActors = Selection.OfType<Actor>().ToList();

            OnSelectionChanged?.Invoke();
        }
    }
}
