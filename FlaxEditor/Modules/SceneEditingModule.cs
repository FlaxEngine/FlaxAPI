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
        /// Selects the specified collection of objects.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="additive">if set to <c>true</c> will use additive mode, otherwise will clear previous selection.</param>
        public void Select(IEnumerable<object> selection, bool additive = false)
        {
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
