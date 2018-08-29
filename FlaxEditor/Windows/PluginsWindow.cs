// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Editor tool window for plugins management using <see cref="PluginManager"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed class PluginsWindow : EditorWindow
    {
        private Tabs _categories;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public PluginsWindow(Editor editor)
        : base(editor, true, ScrollBars.None)
        {
            Title = "Plugins";

            _categories = new Tabs
            {
                Orientation = Orientation.Vertical,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(120, 32),
                Parent = this
            };
        }
    }
}
