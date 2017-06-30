////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Window used to present collection of selected object(s) properties in a grid. Supports Undo/Redo operations.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.SceneEditorWindow" />
    public class PropertiesWindow : SceneEditorWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public PropertiesWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Properties";
        }

        public override void Draw()
        {
            // TODO: remove this temp code, finish properties window
            string text;
            var selection = Editor.SceneEditing.Selection;
            if (selection.Count == 0)
            {
                text = "No actors selected";
            }
            else
            {
                text = "Selected:\n";
                foreach (var e in selection)
                {
                    text += e.Name + "\n";
                }
            }
            Render2D.DrawText(Style.Current.FontMedium, text, new Rectangle(Vector2.Zero, Size), Color.White, TextAlignment.Center, TextAlignment.Center);
            base.Draw();
        }
    }
}
