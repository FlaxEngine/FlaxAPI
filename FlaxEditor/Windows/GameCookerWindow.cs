////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content.Settings;
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
                { PlatformType.XboxOne, new Xbox() },
                { PlatformType.WindowsStore, new WPA() },
            };

            public BuildTabProxy(GameCookerWindow win, PlatformSelector platformSelector)
            {
                GameCookerWin = win;
                Selector = platformSelector;

                // TODO: restore build settings from the Editor cache!
                PerPlatformOptions[PlatformType.Windows].Output = "Output/Windows";
                PerPlatformOptions[PlatformType.XboxOne].Output = "Output/XboxOne";
                PerPlatformOptions[PlatformType.WindowsStore].Output = "Output/WindowsStore";
            }

            public abstract class Platform
            {
                [EditorOrder(10), Tooltip("Output folder path")]
                public string Output;

                [EditorOrder(11), Tooltip("Show output folder in Explorer after build")]
                public bool ShowOutput = true;

                [EditorOrder(20), Tooltip("Configuration build mode")]
                public BuildMode ConfigurationMode;

                [EditorOrder(100), Tooltip("Custom macros")]
                public string[] Defines;

                protected abstract BuildPlatform BuildPlatform { get; }

                protected virtual BuildOptions Options
                {
                    get
                    {
                        BuildOptions options = BuildOptions.None;
                        if (ConfigurationMode == BuildMode.Debug)
                            options |= BuildOptions.Debug;
                        if (ShowOutput)
                            options |= BuildOptions.ShowOutput;
                        return options;
                    }
                }

                public virtual void Build()
                {
                    GameCooker.Build(BuildPlatform, Options, Output, Defines);
                }
            }

            public class Windows : Platform
            {
                public enum Arch
                {
                    x64,
                    x86,
                };

                [EditorOrder(30), Tooltip("Target platform CPU type (32bit or 64bit)")]
                public Arch Architecture;

                protected override BuildPlatform BuildPlatform => Architecture == Arch.x86 ? BuildPlatform.Windows32 : BuildPlatform.Windows64;
            }

            public abstract class UWP : Platform
            {
                public enum Arch
                {
                    x64,
                    x86,
                };

                protected abstract Arch CPUArch { get; }
            }

            public class WPA : UWP
            {
                [EditorOrder(30), Tooltip("Target platform CPU type")]
                public Arch Architecture;

                protected override BuildPlatform BuildPlatform => Architecture == Arch.x86 ? BuildPlatform.WindowsStoreX86 : BuildPlatform.WindowsStoreX64;

                protected override Arch CPUArch => Architecture;
            }

            public class Xbox : UWP
            {
                protected override Arch CPUArch => Arch.x64;

                protected override BuildPlatform BuildPlatform => throw new NotImplementedException("Implement Xbox One platform building.");
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

        private class PresetsTargetsColumnBase : ContainerControl
        {
            public PresetsTargetsColumnBase(bool isPresets, Action addClicked)
            {
                DockStyle = DockStyle.Left;
                Width = 140;

                var title = new Label
                {
                    Bounds = new Rectangle(0, 0, Width, 19),
                    Text = isPresets ? "Presets" : "Targets",
                    Parent = this,
                };
                var addButton = new Button
                {
                    Text = isPresets ? "New preset" : "Add target",
                    Bounds = new Rectangle(6, 22, Width - 12, title.Bottom),
                    Parent = this,
                };
                addButton.Clicked += addClicked;
            }

            protected void RemoveButtons()
            {
                for (int i = ChildrenCount - 1; i >= 0; i--)
                {
                    if (Children[i].Tag != null)
                    {
                        Children[i].Dispose();
                    }
                }
            }

            protected void AddButton(string name, int index, int selectedIndex, Action<Button> select, Action<Button> remove)
            {
                var height = 26;
                var y = 52 + index * height;
                var selectButton = new Button
                {
                    Text = name,
                    Bounds = new Rectangle(6, y + 2, Width - 12 - 20, 22),
                    Tag = index,
                    Parent = this,
                };
                if (selectedIndex == index)
                    selectButton.SetColors(Color.FromBgra(0xFFAB8400));
                selectButton.ButtonClicked += select;
                var removeButton = new Button
                {
                    Text = "x",
                    Bounds = new Rectangle(selectButton.Right + 4, y + 4, 18, 18),
                    Tag = index,
                    Parent = this,
                };
                removeButton.ButtonClicked += remove;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var color = Style.Current.Background * 0.8f;
                Render2D.DrawLine(new Vector2(Width, 0), new Vector2(Width, Height), color);
                Render2D.DrawLine(new Vector2(0, 48), new Vector2(Width, 48), color);
            }
        }

        private sealed class PresetsColumn : PresetsTargetsColumnBase
        {
            private GameCookerWindow _cooker;

            public PresetsColumn(GameCookerWindow cooker)
                : base(true, cooker.AddPreset)
            {
                _cooker = cooker;
            }

            public void RefreshColumn(BuildPreset[] presets, int selectedIndex)
            {
                RemoveButtons();

                if (presets != null)
                {
                    for (int i = 0; i < presets.Length; i++)
                    {
                        AddButton(presets[i].Name, i, selectedIndex,
                                  b => _cooker.SelectPreset((int)b.Tag),
                                  b => _cooker.RemovePreset((int)b.Tag));
                    }
                }
            }
        }

        private sealed class TargetsColumn : PresetsTargetsColumnBase
        {
            private GameCookerWindow _cooker;

            public TargetsColumn(GameCookerWindow cooker)
                : base(false, cooker.AddTarget)
            {
                _cooker = cooker;
            }

            public void RefreshColumn(BuildTarget[] targets, int selectedIndex)
            {
                RemoveButtons();

                if (targets != null)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        AddButton(targets[i].Name, i, selectedIndex,
                                  b => _cooker.SelectTarget((int)b.Tag),
                                  b => _cooker.RemoveTarget((int)b.Tag));
                    }
                }
            }
        }

        private PresetsColumn _presets;
        private TargetsColumn _targets;
        private int _selectedPresetIndex = -1;
        private int _selectedTargetIndex = -1;
        private CustomEditorPresenter _targetSettings;

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

            CreatePresetsTab(sections);
            CreateBuildTab(sections);

            sections.SelectedTabIndex = 1;
        }

        private void AddPreset()
        {
            var settings = GameSettings.Load<BuildSettings>();
            var count = settings.Presets?.Length ?? 0;
            var presets = new BuildPreset[count + 1];
            if (count > 0)
                Array.Copy(settings.Presets, presets, count);
            presets[count - 1] = new BuildPreset
            {
                Name = "Preset " + (count + 1),
                Targets = new[]
                {
                    new BuildTarget
                    {
                        Name = "Windows 64bit",
                        Output = "Output\\Win64",
                        Platform = BuildPlatform.Windows64,
                        Mode = BuildMode.Debug,
                    },
                    new BuildTarget
                    {
                        Name = "Windows 32bit",
                        Output = "Output\\Win32",
                        Platform = BuildPlatform.Windows32,
                        Mode = BuildMode.Debug,
                    },
                }
            };
            GameSettings.Save(settings);
            RefreshColumns(settings);
        }

        private void AddTarget()
        {

        }

        private void SelectPreset(int index)
        {
            SelectTarget(index, 0);
        }

        private void SelectTarget(int index)
        {
            SelectTarget(_selectedPresetIndex, index);
        }

        private void RemovePreset(int index)
        {

        }

        private void RemoveTarget(int index)
        {

        }

        private void RefreshColumns(BuildSettings settings)
        {
            _presets.RefreshColumn(settings.Presets, _selectedPresetIndex);
            var presets = settings.Presets != null && settings.Presets.Length > _selectedPresetIndex ? settings.Presets[_selectedPresetIndex].Targets : null;
            _targets.RefreshColumn(presets, _selectedTargetIndex);
        }

        private void SelectTarget(int presetIndex, int targetIndex)
        {
            object obj = null;
            var settings = GameSettings.Load<BuildSettings>();
            if (settings.Presets != null && settings.Presets.Length > presetIndex)
            {
                var preset = settings.Presets[presetIndex];
                if (preset.Targets != null && preset.Targets.Length > targetIndex)
                    obj = preset.Targets[targetIndex];
            }
            _targetSettings.Select(obj);
            _selectedPresetIndex = presetIndex;
            _selectedTargetIndex = targetIndex;
            RefreshColumns(settings);
        }

        private void CreatePresetsTab(Tabs sections)
        {
            var tab = sections.AddTab(new Tab("Presets"));

            _presets = new PresetsColumn(this)
            {
                Parent = tab,
            };
            _targets = new TargetsColumn(this)
            {
                Parent = tab,
            };
            var panel = new Panel(ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };

            _targetSettings = new CustomEditorPresenter(null);
            _targetSettings.Panel.Parent = panel;

            SelectTarget(0, 0);
        }

        private void CreateBuildTab(Tabs sections)
        {
            var tab = sections.AddTab(new Tab("Build"));

            var platformSelector = new PlatformSelector
            {
                DockStyle = DockStyle.Top,
                BackgroundColor = Style.Current.LightBackground,
                Parent = tab,
            };
            var panel = new Panel(ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };

            var settings = new CustomEditorPresenter(null);
            settings.Panel.Parent = panel;
            settings.Select(new BuildTabProxy(this, platformSelector));
        }
    }
}
