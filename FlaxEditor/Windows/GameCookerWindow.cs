////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlaxEditor.Content.Settings;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Tabs;
using FlaxEngine.Utilities;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
#pragma warning disable 649

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
                    /// <summary>
                    /// The x64.
                    /// </summary>
                    x64,

                    /// <summary>
                    /// The x86.
                    /// </summary>
                    x86,
                };

                /// <summary>
                /// The architecture.
                /// </summary>
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
                /// <summary>
                /// The architecture.
                /// </summary>
                [EditorOrder(30), Tooltip("Target platform CPU type")]
                public Arch Architecture;

                protected override BuildPlatform BuildPlatform => Architecture == Arch.x86 ? BuildPlatform.WindowsStoreX86 : BuildPlatform.WindowsStoreX64;

                protected override Arch CPUArch => Architecture;
            }

            public class Xbox : UWP
            {
                protected override Arch CPUArch => Arch.x64;

                protected override BuildPlatform BuildPlatform => BuildPlatform.XboxOne;
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
                helpButton.Clicked += () => Application.StartProcess("https://docs.flaxengine.com/manual/editor/game-cooker/");
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
                var discardButton = new Button
                {
                    Text = "Discard",
                    Bounds = new Rectangle(6, buildButton.Top - height, Width - 12, 22),
                    AnchorStyle = AnchorStyle.BottomLeft,
                    Parent = this,
                };
                discardButton.Clicked += _cooker.GatherData;
                var saveButton = new Button
                {
                    Text = "Save",
                    Bounds = new Rectangle(6, discardButton.Top - height, Width - 12, 22),
                    AnchorStyle = AnchorStyle.BottomLeft,
                    Parent = this,
                };
                saveButton.Clicked += _cooker.SaveData;
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
        private readonly Queue<BuildTarget> _buildingQueue = new Queue<BuildTarget>();
        private string _preBuildAction;
        private string _postBuildAction;
        private BuildPreset[] _data;
        private bool _isDataDirty, _exitOnBuildEnd;

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
            
            GameCooker.Event += OnGameCookerEvent;

            sections.SelectedTabIndex = 1;
        }

        private void OnGameCookerEvent(GameCooker.EventType type, ref GameCooker.Options options1)
        {
            if (type == GameCooker.EventType.BuildStarted)
            {
                // Execute pre-build action
                if (!string.IsNullOrEmpty(_preBuildAction))
                    ExecueAction(_preBuildAction);
                _preBuildAction = null;
            }
            else if (type == GameCooker.EventType.BuildDone)
            {
                // Execute post-build action
                if (!string.IsNullOrEmpty(_postBuildAction))
                    ExecueAction(_postBuildAction);
                _postBuildAction = null;
            }
            else if (type == GameCooker.EventType.BuildFailed)
            {
                _postBuildAction = null;
            }
        }

        private void ExecueAction(string action)
        {
            string command = "echo off\ncd \"" + Globals.ProjectFolder.Replace('/', '\\') + "\"\necho on\n" + action;
            command = command.Replace("\n", "\r\n");

            // TODO: postprocess text using $(OutputPath) etc. macros
            // TODO: capture std out of the action (maybe call system() to execute it)

            try
            {
                var tmpBat = StringUtils.CombinePaths(Globals.TemporaryFolder, Guid.NewGuid().ToString("N") + ".bat");
                File.WriteAllText(tmpBat, command);
                Application.StartProcess(tmpBat, null, true, true);
                File.Delete(tmpBat);
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                Debug.LogError("Failed to execute build action.");
            }
        }

        internal void ExitOnBuildQueueEnd()
        {
            _exitOnBuildEnd = true;
        }

        /// <summary>
        /// Builds all the targets from the given preset.
        /// </summary>
        /// <param name="preset">The preset.</param>
        public void BuildAll(BuildPreset preset)
        {
            if (preset == null)
                throw new ArgumentNullException(nameof(preset));

            Editor.Log("Building all targets");
            foreach (var e in preset.Targets)
            {
                _buildingQueue.Enqueue(e.DeepClone());
            }
        }

        /// <summary>
        /// Builds the target.
        /// </summary>
        /// <param name="target">The target.</param>
        public void Build(BuildTarget target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Editor.Log("Building target");
            _buildingQueue.Enqueue(target.DeepClone());
        }

        private void BuildTarget()
        {
            if (_data == null || _data.Length <= _selectedPresetIndex || _selectedPresetIndex == -1)
                return;
            if (_data[_selectedPresetIndex].Targets == null || _data[_selectedPresetIndex].Targets.Length <= _selectedTargetIndex)
                return;

            Build(_data[_selectedPresetIndex].Targets[_selectedTargetIndex]);
        }

        private void BuildAllTargets()
        {
            if (_data == null || _data.Length <= _selectedPresetIndex || _selectedPresetIndex == -1)
                return;
            if (_data[_selectedPresetIndex].Targets == null || _data[_selectedPresetIndex].Targets.Length == 0)
                return;

            BuildAll(_data[_selectedPresetIndex]);
        }

        private void AddPreset()
        {
            var count = _data?.Length ?? 0;
            var presets = new BuildPreset[count + 1];
            if (count > 0)
                Array.Copy(_data, presets, count);
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
            _data = presets;

            MarkAsEdited();
            RefreshColumns();
        }

        private void AddTarget()
        {
            if (_data == null || _data.Length <= _selectedPresetIndex)
                return;

            var count = _data[_selectedPresetIndex].Targets?.Length ?? 0;
            var targets = new BuildTarget[count + 1];
            if (count > 0)
                Array.Copy(_data[_selectedPresetIndex].Targets, targets, count);
            targets[count] = new BuildTarget
            {
                Name = "Xbox One",
                Output = "Output\\XboxOne",
                Platform = BuildPlatform.XboxOne,
                Mode = BuildMode.Release,
            };
            _data[_selectedPresetIndex].Targets = targets;

            MarkAsEdited();
            RefreshColumns();
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
            if (_data == null || _data.Length <= index)
                return;
            var presets = _data.ToList();
            presets.RemoveAt(index);
            _data = presets.ToArray();
            MarkAsEdited();

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
                RefreshColumns();
            }
        }

        private void RemoveTarget(int index)
        {
            if (_selectedPresetIndex == -1 || _data == null || _data.Length <= _selectedPresetIndex)
                return;

            var preset = _data[_selectedPresetIndex];
            var targets = preset.Targets.ToList();
            targets.RemoveAt(index);
            preset.Targets = targets.ToArray();
            MarkAsEdited();

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
                RefreshColumns();
            }
        }

        private void RefreshColumns()
        {
            _presets.RefreshColumn(_data, _selectedPresetIndex);
            var presets = _data != null && _data.Length > _selectedPresetIndex && _selectedPresetIndex != -1 ? _data[_selectedPresetIndex].Targets : null;
            _targets.RefreshColumn(presets, _selectedTargetIndex);
        }

        private void SelectTarget(int presetIndex, int targetIndex)
        {
            object obj = null;
            if (presetIndex != -1 && targetIndex != -1 && _data != null && _data.Length > presetIndex)
            {
                var preset = _data[presetIndex];
                if (preset.Targets != null && preset.Targets.Length > targetIndex)
                    obj = preset.Targets[targetIndex];
            }

            _targetSettings.Select(obj);
            _selectedPresetIndex = presetIndex;
            _selectedTargetIndex = targetIndex;

            RefreshColumns();
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
            _targetSettings.Modified += MarkAsEdited;
        }

        private void MarkAsEdited()
        {
            if (!_isDataDirty)
            {
                _isDataDirty = true;
            }
        }

        private void ClearDirtyFlag()
        {
            if (_isDataDirty)
            {
                _isDataDirty = false;
            }
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

        /// <summary>
        /// Load the build presets from the settings.
        /// </summary>
        private void GatherData()
        {
            _data = null;

            var settings = GameSettings.Load<BuildSettings>();
            if (settings.Presets != null)
            {
                _data = new BuildPreset[settings.Presets.Length];
                for (int i = 0; i < _data.Length; i++)
                {
                    _data[i] = settings.Presets[i].DeepClone();
                }
            }

            ClearDirtyFlag();
            SelectTarget(0, 0);
        }

        /// <summary>
        /// Saves the build presets to the settings.
        /// </summary>
        private void SaveData()
        {
            if (_data == null)
                return;

            var settings = GameSettings.Load<BuildSettings>();
            settings.Presets = _data;
            GameSettings.Save(settings);

            ClearDirtyFlag();
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            GatherData();
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            // Building queue
            if (!GameCooker.IsRunning)
            {
                if (_buildingQueue.Count > 0)
                {
                    var target = _buildingQueue.Dequeue();

                    _preBuildAction = target.PreBuildAction;
                    _postBuildAction = target.PostBuildAction;

                    GameCooker.Build(target.Platform, target.Options, target.Output, target.Defines);
                }
                else if (_exitOnBuildEnd)
                {
                    _exitOnBuildEnd = false;
                    Application.Exit();
                }
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            GameCooker.Event -= OnGameCookerEvent;

            base.OnDestroy();
        }
    }
}
