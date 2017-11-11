////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Content.Settings;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Tabs;
using FlaxEngine.Utilities;

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
                        if (presets[i] == null)
                            return;
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

                var height = 26;
                var helpButton = new Button
                {
                    Text = "Help",
                    Bounds = new Rectangle(6, Height - height, Width - 12, 22),
                    AnchorStyle = AnchorStyle.BottomLeft,
                    Parent = this,
                };
                // TODO: update link to game cooker docs
                helpButton.Clicked += () => Application.StartProcess("http://docs.flaxengine.com/manual/index.html");
                var buildAllButton = new Button
                {
                    Text = "Build All",
                    Bounds = new Rectangle(6, helpButton.Top - height, Width - 12, 22),
                    AnchorStyle = AnchorStyle.BottomLeft,
                    Parent = this,
                };
                buildAllButton.Clicked += _cooker.BuildAllTargets;
                var buildButton = new Button
                {
                    Text = "Build",
                    Bounds = new Rectangle(6, buildAllButton.Top - height, Width - 12, 22),
                    AnchorStyle = AnchorStyle.BottomLeft,
                    Parent = this,
                };
                buildButton.Clicked += _cooker.BuildTarget;
            }

            public void RefreshColumn(BuildTarget[] targets, int selectedIndex)
            {
                RemoveButtons();

                if (targets != null)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (targets[i] == null)
                            return;
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
        private Queue<BuildTarget> _buildingQueue = new Queue<BuildTarget>();

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

        private void BuildTarget()
        {
            var settings = GameSettings.Load<BuildSettings>();
            if (settings.Presets == null || settings.Presets.Length <= _selectedPresetIndex || _selectedPresetIndex == -1)
                return;
            if (settings.Presets[_selectedPresetIndex].Targets == null || settings.Presets[_selectedPresetIndex].Targets.Length <= _selectedTargetIndex)
                return;

            Editor.Log("Building target");
            _buildingQueue.Enqueue(settings.Presets[_selectedPresetIndex].Targets[_selectedTargetIndex].DeepClone());
        }

        private void BuildAllTargets()
        {
            var settings = GameSettings.Load<BuildSettings>();
            if (settings.Presets == null || settings.Presets.Length <= _selectedPresetIndex || _selectedPresetIndex == -1)
                return;
            if (settings.Presets[_selectedPresetIndex].Targets == null || settings.Presets[_selectedPresetIndex].Targets.Length == 0)
                return;

            Editor.Log("Building all targets");
            foreach (var e in settings.Presets[_selectedPresetIndex].Targets)
            {
                _buildingQueue.Enqueue(e.DeepClone());
            }
        }

        private void AddPreset()
        {
            var settings = GameSettings.Load<BuildSettings>();
            var count = settings.Presets?.Length ?? 0;
            var presets = new BuildPreset[count + 1];
            if (count > 0)
                Array.Copy(settings.Presets, presets, count);
            presets[count] = new BuildPreset
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
            settings.Presets = presets;
            GameSettings.Save(settings);
            RefreshColumns(settings);
        }

        private void AddTarget()
        {
            var settings = GameSettings.Load<BuildSettings>();
            if (settings.Presets == null || settings.Presets.Length <= _selectedPresetIndex)
                return;
            var count = settings.Presets[_selectedPresetIndex].Targets?.Length ?? 0;
            var targets = new BuildTarget[count + 1];
            if (count > 0)
                Array.Copy(settings.Presets[_selectedPresetIndex].Targets, targets, count);
            targets[count] = new BuildTarget
            {
                Name = "Xbox One",
                Output = "Output\\XboxOne",
                Platform = BuildPlatform.XboxOne,
                Mode = BuildMode.Release,
            };
            settings.Presets[_selectedPresetIndex].Targets = targets;
            GameSettings.Save(settings);
            RefreshColumns(settings);
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
            var settings = GameSettings.Load<BuildSettings>();
            if (settings.Presets == null || settings.Presets.Length <= index)
                return;
            var presets = settings.Presets.ToList();
            presets.RemoveAt(index);
            settings.Presets = presets.ToArray();
            GameSettings.Save(settings);
            if (presets.Count == 0)
            {
                SelectTarget(-1, -1);
            }
            else if (_selectedPresetIndex == index)
            {
                SelectTarget(0, 0);
            }
            else
            {
                RefreshColumns(settings);
            }
        }

        private void RemoveTarget(int index)
        {
            if (_selectedPresetIndex == -1)
                return;
            var settings = GameSettings.Load<BuildSettings>();
            if (settings.Presets == null || settings.Presets.Length <= _selectedPresetIndex)
                return;
            var preset = settings.Presets[_selectedPresetIndex];
            var targets = preset.Targets.ToList();
            targets.RemoveAt(index);
            preset.Targets = targets.ToArray();
            GameSettings.Save(settings);
            if (targets.Count == 0)
            {
                SelectTarget(_selectedPresetIndex, -1);
            }
            else if (_selectedPresetIndex == index)
            {
                SelectTarget(_selectedPresetIndex, 0);
            }
            else
            {
                RefreshColumns(settings);
            }
        }

        private void RefreshColumns(BuildSettings settings)
        {
            _presets.RefreshColumn(settings.Presets, _selectedPresetIndex);
            var presets = settings.Presets != null && settings.Presets.Length > _selectedPresetIndex && _selectedPresetIndex != -1 ? settings.Presets[_selectedPresetIndex].Targets : null;
            _targets.RefreshColumn(presets, _selectedTargetIndex);
        }

        private void SelectTarget(int presetIndex, int targetIndex)
        {
            object obj = null;
            var settings = GameSettings.Load<BuildSettings>();
            if (presetIndex != -1 && targetIndex != -1 && settings.Presets != null && settings.Presets.Length > presetIndex)
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

        /// <inheritdoc />
        public override void OnInit()
        {
            SelectTarget(0, 0);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Building queue
            if (_buildingQueue.Count > 0 && !GameCooker.IsRunning)
            {
                var target = _buildingQueue.Dequeue();

                GameCooker.Build(target.Platform, target.Options, target.Output, target.Defines);
            }
        }
    }
}
