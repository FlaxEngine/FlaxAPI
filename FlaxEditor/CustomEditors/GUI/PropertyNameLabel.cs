// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.GUI
{
    /// <summary>
    /// Displays custom editor property name.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Label" />
    public class PropertyNameLabel : Label
    {
        // TODO: if name is too long to show -> use tooltip to show it

        private bool _mouseDown;

        /// <summary>
        /// Helper value used by the <see cref="PropertiesList"/> to draw property names in a proper area.
        /// </summary>
        internal int FirstChildControlIndex;

        /// <summary>
        /// The linked custom editor (shows the label property).
        /// </summary>
        internal CustomEditor LinkedEditor;

        /// <summary>
        /// The highlight strip color drawn on a side (transparent if skip rendering).
        /// </summary>
        public Color HighlightStripColor;

        /// <summary>
        /// Occurs when label creates the context menu popup for th property. Can be used to add some custom logic per property editor.
        /// </summary>
        public event Action<PropertyNameLabel, ContextMenu> SetupContextMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNameLabel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public PropertyNameLabel(string name)
        {
            Text = name;
            HorizontalAlignment = TextAlignment.Near;
            VerticalAlignment = TextAlignment.Center;
            Margin = new Margin(4, 0, 0, 0);
            ClipText = true;

            HighlightStripColor = Color.Transparent;
        }

        internal void LinkEditor(CustomEditor editor)
        {
            if (LinkedEditor == null)
            {
                LinkedEditor = editor;
                editor.LinkLabel(this);
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            if (HighlightStripColor.A > 0.0f)
            {
                Render2D.FillRectangle(new Rectangle(0, 0, 2, Height), HighlightStripColor);
            }
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            _mouseDown = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Right)
            {
                _mouseDown = true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;

            if (_mouseDown && buttons == MouseButton.Right)
            {
                _mouseDown = false;

                if (LinkedEditor == null && SetupContextMenu == null)
                    return false;

                var menu = new ContextMenu();
                if (LinkedEditor != null)
                {
                    var revertToPrefab = menu.AddButton("Revert to Prefab", LinkedEditor.RevertToReferenceValue);
                    revertToPrefab.Enabled = LinkedEditor.CanRevertReferenceValue;
                }
                SetupContextMenu?.Invoke(this, menu);
                menu.Show(this, location);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            _mouseDown = false;

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            SetupContextMenu = null;

            base.Dispose();
        }
    }
}
