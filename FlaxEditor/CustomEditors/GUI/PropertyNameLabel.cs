// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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

        /// <summary>
        /// Helper value used by the <see cref="PropertiesList"/> to draw property names in a proper area.
        /// </summary>
        internal int FirstChildControlIndex;

        /// <summary>
        /// The linked custom editor (shows the label property).
        /// </summary>
        internal CustomEditor LinkedEditor;

        /// <summary>
        /// The highlight strip color drawn on a side (transparen if skip rendering).
        /// </summary>
        public Color HighlightStripColor;

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
                Render2D.FillRectangle(new Rectangle(0, 0, 2, Height), HighlightStripColor, HighlightStripColor.A < 1.0f);
            }
        }
    }
}
