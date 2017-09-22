////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Main class for Custom Editors used to present selected objects properties and allow to modify them.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElementsContainer" />
    public class CustomEditorPresenter : LayoutElementsContainer
    {
        /// <summary>
        /// The panel control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.VerticalPanel" />
        public class PresenterPanel : VerticalPanel
        {
            private CustomEditorPresenter _presenter;

            internal PresenterPanel(CustomEditorPresenter presenter)
            {
                _presenter = presenter;
                DockStyle = DockStyle.Top;
                IsScrollable = true;
            }

            /// <inheritdoc />
            public override void Update(float deltaTime)
            {
                // Update editors
                _presenter.Update();

                base.Update(deltaTime);
            }
        }
        
        /// <summary>
        /// The root editor. Mocks some custom editors events. Created a child editor for the selected objects.
        /// </summary>
        /// <seealso cref="FlaxEditor.CustomEditors.SyncPointEditor" />
        protected class RootEditor : SyncPointEditor
        {
            /// <summary>
            /// The selected objects editor.
            /// </summary>
            public CustomEditor Editor;
            
            /// <summary>
            /// Setups editor for selected objects.
            /// </summary>
            /// <param name="presenter">The presenter.</param>
            public void Setup(CustomEditorPresenter presenter)
            {
                Cleanup();
                Initialize(presenter, presenter, null);
            }

            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                var selection = Presenter.Selection;
                if (selection.Count > 0)
                {
                    Type type = typeof(object);
                    if (selection.HasDiffrentTypes == false)
                        type = selection[0].GetType();
                    Editor = CustomEditorsUtil.CreateEditor(type, false);
                    Editor.Initialize(Presenter, Presenter, selection);
                    OnChildCreated(Editor);
                }
                else
                {
                    var label = layout.Label("No selection", TextAlignment.Center);
                    label.Label.Height = 20.0f;
                }
                
                base.Initialize(layout);
            }

            /// <inheritdoc />
            protected override void OnModified()
            {
                Presenter.Modified?.Invoke();

                base.OnModified();
            }

            /// <inheritdoc />
            protected override void SyncChildValues(ValueContainer values)
            {
                // Skip
            }
        }
        
        /// <summary>
        /// The panel.
        /// </summary>
        public readonly PresenterPanel Panel;

        /// <summary>
        /// The selected objects editor (root, it generatec actual editor for selection).
        /// </summary>
        protected readonly RootEditor Editor;

        /// <summary>
        /// The selected objects list.
        /// </summary>
        protected readonly ValueContainer Selection = new ValueContainer(null);

        /// <summary>
        /// The undo object used by this editor.
        /// </summary>
        public readonly Undo Undo;

        /// <summary>
        /// Occurs when selection gets changed.
        /// </summary>
        public event Action SelectionChanged;

        /// <summary>
        /// Occurs when any property gets changed.
        /// </summary>
        public event Action Modified;

        /// <summary>
        /// Occurs when presenter wants to gather undo objects to record changes. Can be overriden to provide custom objects collection.
        /// </summary>
        public Func<CustomEditorPresenter, IEnumerable<object>> GetUndoObjects = presenter => presenter.Selection;

        /// <summary>
        /// Gets the amount of objects being selected.
        /// </summary>
        public int SelectionCount => Selection.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomEditorPresenter"/> class.
        /// </summary>
        /// <param name="undo">The undo. It's optional.</param>
        public CustomEditorPresenter(Undo undo)
        {
            Undo = undo;
            Panel = new PresenterPanel(this);
            Editor = new RootEditor();
            Editor.Initialize(this, this, null);
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
        /// Builds the editors layout.
        /// </summary>
        public virtual void BuildLayout()
        {
            // TODO: implement layout elements reusing to reduce memory hit

            // Clear layout
            var parentScrollV = (Panel.Parent as Panel)?.VScrollBar?.Value ?? -1;
            Panel.IsLayoutLocked = true;
            Panel.DisposeChildren();
            
            ClearLayout();
            Editor.Setup(this);

            Panel.UnlockChildrenRecursive();
            Panel.PerformLayout();

            // Restore scroll value
            if (parentScrollV != -1)
                ((Panel)Panel.Parent).VScrollBar.Value = parentScrollV;
        }

        /// <summary>
        /// Called when selection gets changed.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            BuildLayout();
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Updates custom editors. Refreshes UI values and applies changes to the selected objects.
        /// </summary>
        internal void Update()
        {
            Editor?.RefreshInternal();
        }

        /// <inheritdoc />
        public override ContainerControl ContainerControl => Panel;
    }
}
