////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Tabs;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Editor tool window for building games using <see cref="GameCooker"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed class GameCookerWindow : EditorWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameCookerWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public GameCookerWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Game Cooker";

            var sections = new Tabs
            {
                Orientation = Orientation.Vertical,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(120, 32),
                Parent = this
            };
            
            CreateGameSettingsTab(sections);
            CreateBuildTab(sections);

            sections.SelectedTabIndex = 0;
        }

        private void CreateGameSettingsTab(Tabs sections)
        {
            var gameSettings = AddSection(sections, "Game Settings");

            // TODO: insert game settings asset editor
        }

        private void CreateBuildTab(Tabs sections)
        {
            var build = AddSection(sections, "Build");

            var platformSelector = new PlatformSelector
            {
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                Parent = build,
            };
        }

        private ContainerControl AddSection(Tabs parentTabs, string title)
        {
            var tab = parentTabs.AddTab(new Tab(title));
            var panel = new Panel(ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };
            return panel;
        }
    }
}
