////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.CustomEditors;
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
        /// Proxy object for the Build tab.
        /// </summary>
        [CustomEditor(typeof(BuildTabProxy.Editor))]
        private class BuildTabProxy
        {
            public readonly GameCookerWindow GameCookerWin;
            public readonly PlatformSelector Selector;

            public Dictionary<PlatformType, object> PerPlatformOptions = new Dictionary<PlatformType, object>
            {
                { PlatformType.Windows, new Windows() },
                { PlatformType.XboxOne, new UWP() },
                { PlatformType.WindowsStore, new UWP() },
            };

            public BuildTabProxy(GameCookerWindow win, PlatformSelector platformSelector)
            {
                GameCookerWin = win;
                Selector = platformSelector;

                // TODO: restore build settings from the Editor cache!
                ((Windows)PerPlatformOptions[PlatformType.Windows]).Output = StringUtils.CombinePaths(Globals.ProjectFolder, "Output/Windows");
                ((UWP)PerPlatformOptions[PlatformType.XboxOne]).Output = StringUtils.CombinePaths(Globals.ProjectFolder, "Output/XboxOne");
                ((UWP)PerPlatformOptions[PlatformType.WindowsStore]).Output = StringUtils.CombinePaths(Globals.ProjectFolder, "Output/WindowsStore");
            }

            private class Windows
            {
                public enum Arch
                {
                    x86,
                    x64,
                };

                public enum Mode
                {
                    Release,
                    Debug,
                }

                [EditorOrder(10), Tooltip("Output folder path")]
                public string Output;

                [EditorOrder(20), Tooltip("Target platform CPU type (32bit or 64bit)")]
                public Arch Architecture;

                [EditorOrder(30), Tooltip("Configuration build mode")]
                public Mode ConfigurationMode;
            }

            private class UWP
            {
                public enum Arch
                {
                    x86,
                    x64,
                };

                public enum Mode
                {
                    Release,
                    Debug,
                }

                [EditorOrder(10), Tooltip("Output folder path")]
                public string Output;

                [EditorOrder(20), Tooltip("Target platform CPU type")]
                public Arch Architecture;

                [EditorOrder(30), Tooltip("Configuration build mode")]
                public Mode ConfigurationMode;
            }

            public class Editor : CustomEditor
            {
                private PlatformType _platform;

                public override void Initialize(LayoutElementsContainer layout)
                {
                    var proxy = (BuildTabProxy)Values[0];
                    _platform = proxy.Selector.Selected;

                    var group = layout.Group(CustomEditorsUtil.GetPropertyNameUI(_platform.ToString()));

                    group.Object(new ReadOnlyValueContainer(proxy.PerPlatformOptions[_platform]));
                    
                    var buildButton = layout.Button("Build");
                    buildButton.Button.Clicked += OnBuildClicked;
                }

                private void OnBuildClicked()
                {
                    MessageBox.Show("call building!");
                }

                public override void Refresh()
                {
                    if (Values.Count > 0 && Values[0] is BuildTabProxy proxy && proxy.Selector.Selected != _platform)
                    {
                        RebuildLayout();
                    }
                }
            }
        }

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
            var gameSettings = sections.AddTab(new Tab("Game Settings"));

            // TODO: insert game settings asset editor
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (ParentWindow.GetKeyDown(KeyCode.A))
            {
                UnlockChildrenRecursive();
                PerformLayout();
            }
        }

        private void CreateBuildTab(Tabs sections)
        {
            var build = sections.AddTab(new Tab("Build"));

            var platformSelector = new PlatformSelector
            {
                DockStyle = DockStyle.Top,
                BackgroundColor = Style.Current.LightBackground,
                Parent = build,
            };
            var panel = new Panel(ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                Parent = build
            };
            
            var settings = new CustomEditorPresenter(null);
            settings.Panel.Parent = panel;
            settings.Select(new BuildTabProxy(this, platformSelector));
        }
    }
}
