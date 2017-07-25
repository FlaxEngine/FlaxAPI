////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Main class for Custom Editors used to present selected objects properties and allow to modify them.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class CustomEditor : ContainerControl
    {
        /// <summary>
        /// The selected objects list.
        /// </summary>
        protected readonly List<object> Selection = new List<object>();

        /// <summary>
        /// Occurs when selection gets changed.
        /// </summary>
        public event Action SelectionChanged;

        public CustomEditor(CustomEditor parentEditor = null)
            : base(false, 0, 0, 64, 64)
        {
        }

        /// <summary>
        /// Selects the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Select(object obj)
        {
            if(obj == null)
                throw new ArgumentNullException();
            if (Selection.Count == 1 && Selection[0] == obj)
                return;

            Selection.Clear();
            Selection.Add(obj);

            OnSelectionChanged();
        }

        /// <summary>
        /// Selects the specified objects.
        /// </summary>
        /// <param name="objects">The objects.</param>
        public void Select(IEnumerable<object> objects)
        {
            if (objects == null)
                throw new ArgumentNullException();

            Selection.Clear();
            Selection.AddRange(objects);

            OnSelectionChanged();
        }

        /// <summary>
        /// Clears the selected objects.
        /// </summary>
        public void Deselect()
        {
            if (Selection.Count == 0)
                return;

            Selection.Clear();

            OnSelectionChanged();
        }

        /// <summary>
        /// Called when selection gets changed.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            SelectionChanged?.Invoke();
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            Deselect();

            base.OnDestroy();
        }
    }
}
