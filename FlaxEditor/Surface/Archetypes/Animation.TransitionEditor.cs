// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.CustomEditors;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    public static partial class Animation
    {
        /// <summary>
        /// TheAnim Graph state machine transition editor (as contextual popup).
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ContextMenuBase" />
        internal class TransitionEditor : ContextMenuBase
        {
            private StateMachineTransition _transition;

            /// <summary>
            /// Gets the transition being modified.
            /// </summary>
            public StateMachineTransition Transition => _transition;

            /// <summary>
            /// Initializes a new instance of the <see cref="TransitionEditor"/> class.
            /// </summary>
            /// <param name="transition">The transition.</param>
            public TransitionEditor(StateMachineTransition transition)
            {
                _transition = transition ?? throw new ArgumentNullException(nameof(transition));

                // Context menu dimensions
                const float width = 280.0f;
                const float height = 220.0f;
                Size = new Vector2(width, height);

                // Title
                var title = new Label(2, 2, width - 4, 23.0f)
                {
                    Font = new FontReference(Style.Current.FontLarge),
                    Text = transition.SurfaceName,
                    Parent = this
                };

                // Buttons
                float buttonsWidth = (width - 8.0f) * 0.5f;
                float buttonsHeight = 20.0f;
                var editRuleButton = new Button(2.0f, title.Bottom + 2.0f, buttonsWidth, buttonsHeight)
                {
                    Text = "Edit Rule",
                    Parent = this
                };
                editRuleButton.Clicked += OnEditRuleButtonClicked;
                var deleteButton = new Button(editRuleButton.Right + 2.0f, editRuleButton.Y, buttonsWidth, buttonsHeight)
                {
                    Text = "Delete",
                    Parent = this
                };
                deleteButton.Clicked += OnDeleteButtonClicked;

                // Actual panel
                var panel1 = new Panel(ScrollBars.Vertical)
                {
                    Bounds = new Rectangle(0, deleteButton.Bottom + 2.0f, width, height - deleteButton.Bottom - 2.0f),
                    Parent = this
                };
                var editor = new CustomEditorPresenter(null);
                editor.Panel.DockStyle = DockStyle.Top;
                editor.Panel.IsScrollable = true;
                editor.Panel.Parent = panel1;

                editor.Select(_transition);
            }

            private void OnEditRuleButtonClicked()
            {
                Hide();
                _transition.EditRule();
            }

            private void OnDeleteButtonClicked()
            {
                Hide();
                _transition.Delete();
            }

            /// <inheritdoc />
            protected override void OnShow()
            {
                // Prepare
                Focus();

                base.OnShow();
            }

            /// <inheritdoc />
            public override void Hide()
            {
                if (!Visible)
                    return;

                Focus(null);

                base.Hide();
            }

            /// <inheritdoc />
            public override bool OnKeyDown(Keys key)
            {
                if (key == Keys.Escape)
                {
                    Hide();
                    return true;
                }

                return base.OnKeyDown(key);
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                if (IsDisposing)
                    return;

                _transition = null;

                base.Dispose();
            }
        }
    }
}
