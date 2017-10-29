////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
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

            public Dictionary<PlatformType, Platform> PerPlatformOptions = new Dictionary<PlatformType, Platform>
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
                PerPlatformOptions[PlatformType.Windows].Output = StringUtils.CombinePaths(Globals.ProjectFolder, "Output/Windows");
                PerPlatformOptions[PlatformType.XboxOne].Output = StringUtils.CombinePaths(Globals.ProjectFolder, "Output/XboxOne");
                PerPlatformOptions[PlatformType.WindowsStore].Output = StringUtils.CombinePaths(Globals.ProjectFolder, "Output/WindowsStore");
            }

            public abstract class Platform
            {
                public enum Mode
                {
                    Release,
                    Debug,
                }

                [EditorOrder(10), Tooltip("Output folder path")]
                public string Output;

                [EditorOrder(20), Tooltip("Configuration build mode")]
                public Mode ConfigurationMode;

                protected abstract BuildPlatform BuildPlatform
                {
                    get;
                }

                protected virtual BuildOptions Options
                {
                    get { return ConfigurationMode == Mode.Debug ? BuildOptions.Debug : BuildOptions.None; }
                }

                public virtual void Build()
                {
                    GameCooker.Build(BuildPlatform, Options, Output);
                }
            }

            public class Windows : Platform
            {
                public enum Arch
                {
                    x64,
                    x86,
                };

                [EditorOrder(20), Tooltip("Target platform CPU type (32bit or 64bit)")]
                public Arch Architecture;

                protected override BuildPlatform BuildPlatform => Architecture == Arch.x86 ? BuildPlatform.Windows32 : BuildPlatform.Windows64;
            }

            public class UWP : Platform
            {
                public enum Arch
                {
                    x64,
                    x86,
                };

                [EditorOrder(30), Tooltip("Target platform CPU type")]
                public Arch Architecture;

                protected override BuildPlatform BuildPlatform => throw new NotImplementedException("Implement UWP platform building.");
            }

            public class Editor : CustomEditor
            {
                private PlatformType _platform;
                private Button _buildButton;

                public override void Initialize(LayoutElementsContainer layout)
                {
                    var proxy = (BuildTabProxy)Values[0];
                    _platform = proxy.Selector.Selected;
                    var platformObj = proxy.PerPlatformOptions[_platform];

                    var group = layout.Group(CustomEditorsUtil.GetPropertyNameUI(_platform.ToString()));

                    group.Object(new ReadOnlyValueContainer(platformObj));

                    _buildButton = layout.Button("Build").Button;
                    _buildButton.Clicked += OnBuildClicked;
                }

                private void OnBuildClicked()
                {
                    var proxy = (BuildTabProxy)Values[0];
                    var platformObj = proxy.PerPlatformOptions[_platform];
                    platformObj.Build();
                }

                public override void Refresh()
                {
                    if (_buildButton != null)
                    {
                        _buildButton.Enabled = !GameCooker.IsRunning;
                    }

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
