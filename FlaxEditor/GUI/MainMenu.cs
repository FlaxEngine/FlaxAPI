// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Menu strip with child buttons.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public sealed class MainMenu : ContainerControl
    {
        private MainMenuButton _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        : base(0, 0, 120, 20)
        {
            CanFocus = false;
            DockStyle = DockStyle.Top;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <returns>Created button control.</returns>
        public MainMenuButton AddButton(string text)
        {
            return AddChild(new MainMenuButton(text));
        }

        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <returns>The button or null if missing.</returns>
        public MainMenuButton GetButton(string text)
        {
            MainMenuButton result = null;
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is MainMenuButton button && string.Equals(button.Text, text, StringComparison.OrdinalIgnoreCase))
                {
                    result = button;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Selects the specified button. Used by <see cref="MainMenuButton"/>.
        /// </summary>
        /// <param name="button">The button.</param>
        internal void Select(MainMenuButton button)
        {
            // Check if popup menu has been already hidden
            if (_selected != null && !_selected.ContextMenu.Visible)
                _selected = null;

            if (_selected != button)
            {
                _selected = button;

                if (_selected != null && _selected.ContextMenu.HasChildren)
                {
                    _selected.ContextMenu.Show(_selected, new Vector2(0, _selected.Height));
                }
            }
            else if (_selected != null)
            {
                _selected.ContextMenu.Hide();
                _selected = null;
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            BackgroundColor = Style.Current.LightBackground;
            base.Draw();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Arrange controls
            float y = 2;
            float x = 1;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Visible)
                {
                    c.Location = new Vector2(x, y);
                    x += c.Width + 2;
                }
            }
        }
    }
}
