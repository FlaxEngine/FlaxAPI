// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Linq;
using FlaxEditor.Gizmo;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Dialogs;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
using FlaxEditor.Scripting;
using FlaxEditor.Utilities;
using FlaxEditor.Viewport.Cameras;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using DockHintWindow = FlaxEditor.GUI.Docking.DockHintWindow;
using MasterDockPanel = FlaxEditor.GUI.Docking.MasterDockPanel;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages Editor UI. Especially main window UI.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class UIModule : EditorModule
    {
        private Label _progressLabel;
        private ProgressBar _progressBar;

        private ContextMenuButton _menuFileSaveScenes;
        private ContextMenuButton _menuFileCloseScenes;
        private ContextMenuButton _menuEditUndo;
        private ContextMenuButton _menuEditRedo;
        private ContextMenuButton _menuEditCut;
        private ContextMenuButton _menuEditCopy;
        private ContextMenuButton _menuEditDelete;
        private ContextMenuButton _menuEditDuplicate;
        private ContextMenuButton _menuEditSelectAll;
        private ContextMenuButton _menuSceneMoveActorToViewport;
        private ContextMenuButton _menuSceneAlignActorWithViewport;
        private ContextMenuButton _menuSceneAlignViewportWtihActor;
        private ContextMenuButton _menuSceneCreateTerrain;
        private ContextMenuButton _menuGamePlay;
        private ContextMenuButton _menuGamePause;
        private ContextMenuButton _menuToolsBakeLightmaps;
        private ContextMenuButton _menuToolsClearLightmaps;
        private ContextMenuButton _menuToolsBakeAllEnvProbes;
        private ContextMenuButton _menuToolsBuildCSGMesh;
        private ContextMenuButton _menuToolsCancelBuilding;
        private ContextMenuButton _menuToolsSetTheCurrentSceneViewAsDefault;
        private ContextMenuChildMenu _menuWindowApplyWindowLayout;

        private ToolStripButton _toolStripUndo;
        private ToolStripButton _toolStripRedo;
        private ToolStripButton _toolStripTranslate;
        private ToolStripButton _toolStripRotate;
        private ToolStripButton _toolStripScale;
        private ToolStripButton _toolStripPlay;
        private ToolStripButton _toolStripPause;
        private ToolStripButton _toolStripStep;

        /// <summary>
        /// The main menu control.
        /// </summary>
        public MainMenu MainMenu;

        /// <summary>
        /// The tool strip control.
        /// </summary>
        public ToolStrip ToolStrip;

        /// <summary>
        /// The master dock panel for all Editor windows.
        /// </summary>
        public MasterDockPanel MasterPanel = new MasterDockPanel();

        /// <summary>
        /// The status strip control.
        /// </summary>
        public StatusBar StatusBar;

        /// <summary>
        /// The visject surface background texture. Cached to be used globally.
        /// </summary>
        public Texture VisjectSurfaceBackground;

        /// <summary>
        /// Gets the File menu.
        /// </summary>
        public MainMenuButton MenuFile { get; private set; }

        /// <summary>
        /// Gets the Edit menu.
        /// </summary>
        public MainMenuButton MenuEdit { get; private set; }

        /// <summary>
        /// Gets the Scene menu.
        /// </summary>
        public MainMenuButton MenuScene { get; private set; }

        /// <summary>
        /// Gets the Game menu.
        /// </summary>
        public MainMenuButton MenuGame { get; private set; }

        /// <summary>
        /// Gets the Tools menu.
        /// </summary>
        public MainMenuButton MenuTools { get; private set; }

        /// <summary>
        /// Gets the Window menu.
        /// </summary>
        public MainMenuButton MenuWindow { get; private set; }

        /// <summary>
        /// Gets the Help menu.
        /// </summary>
        public MainMenuButton MenuHelp { get; private set; }

        internal UIModule(Editor editor)
        : base(editor)
        {
            InitOrder = -90;

            CreateStyle();
        }

        /// <summary>
        /// Unchecks toolstrip pause button.
        /// </summary>
        public void UncheckPauseButton()
        {
            if (_toolStripPause != null)
                _toolStripPause.Checked = false;
        }

        /// <summary>
        /// Checks if toolstrip pause button is being checked.
        /// </summary>
        public bool IsPauseButtonChecked => _toolStripPause != null && _toolStripPause.Checked;

        /// <summary>
        /// Updates the toolstrip.
        /// </summary>
        public void UpdateToolstrip()
        {
            if (ToolStrip == null)
                return;

            var undoRedo = Editor.Undo;
            var gizmo = Editor.MainTransformGizmo;
            var state = Editor.StateMachine.CurrentState;

            // Update buttons
            bool canEditScene = state.CanEditScene;
            bool canEnterPlayMode = state.CanEnterPlayMode && SceneManager.IsAnySceneLoaded;
            //
            _toolStripUndo.Enabled = canEditScene && undoRedo.CanUndo;
            _toolStripRedo.Enabled = canEditScene && undoRedo.CanRedo;
            //
            var gizmoMode = gizmo.ActiveMode;
            _toolStripTranslate.Checked = gizmoMode == TransformGizmo.Mode.Translate;
            _toolStripRotate.Checked = gizmoMode == TransformGizmo.Mode.Rotate;
            _toolStripScale.Checked = gizmoMode == TransformGizmo.Mode.Scale;
            //
            var play = _toolStripPlay;
            var pause = _toolStripPause;
            var step = _toolStripStep;
            play.Enabled = canEnterPlayMode;
            if (Editor.StateMachine.IsPlayMode)
            {
                play.Checked = false;
                play.Icon = Editor.Icons.Stop32;
                pause.Enabled = true;
                pause.Checked = Editor.StateMachine.PlayingState.IsPaused;
                pause.AutoCheck = false;
                step.Enabled = true;
            }
            else
            {
                play.Checked = Editor.Simulation.IsPlayModeRequested;
                play.Icon = Editor.Icons.Play32;
                pause.Enabled = canEnterPlayMode;
                pause.AutoCheck = true;
                step.Enabled = false;
            }
        }

        /// <summary>
        /// Adds the menu button.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="text">The text.</param>
        /// <param name="clicked">The button clicked event.</param>
        /// <returns>The created menu item.</returns>
        public ContextMenuButton AddMenuButton(string group, string text, Action clicked)
        {
            var menuGroup = MainMenu.GetButton(group) ?? MainMenu.AddButton(group);
            return menuGroup.ContextMenu.AddButton(text, clicked);
        }

        /// <summary>
        /// Updates the status bar.
        /// </summary>
        public void UpdateStatusBar()
        {
            if (StatusBar == null)
                return;

            Color color;
            if (Editor.StateMachine.IsPlayMode)
            {
                color = Color.OrangeRed;
            }
            else
            {
                color = Style.Current.BackgroundSelected;
            }

            StatusBar.Text = Editor.StateMachine.CurrentState.Status ?? "Ready";
            StatusBar.StatusColor = color;
        }

        internal bool ProgressVisible
        {
            get => _progressLabel?.Parent.Visible ?? false;
            set
            {
                if (_progressLabel != null)
                    _progressLabel.Parent.Visible = value;
            }
        }

        internal void UpdateProgress(string text, float progress)
        {
            if (_progressLabel != null)
                _progressLabel.Text = text;
            if (_progressBar != null)
                _progressBar.Value = progress * 100.0f;
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Editor.Windows.MainWindowClosing += OnMainWindowClosing;
            var mainWindow = Editor.Windows.MainWindow.GUI;

            // Update window background
            mainWindow.BackgroundColor = Style.Current.Background;

            InitMainMenu(mainWindow);
            InitToolstrip(mainWindow);
            InitStatusBar(mainWindow);
            InitDockPanel(mainWindow);

            // Cache hint windows
            DockHintWindow.Proxy.InitHitProxy();
        }

        /// <inheritdoc />
        public override void OnEndInit()
        {
            Editor.MainTransformGizmo.ModeChanged += UpdateToolstrip;
            Editor.StateMachine.StateChanged += StateMachineOnStateChanged;
            Editor.Undo.UndoDone += OnUndoEvent;
            Editor.Undo.RedoDone += OnUndoEvent;
            Editor.Undo.ActionDone += OnUndoEvent;

            UpdateToolstrip();
        }

        private void OnUndoEvent(IUndoAction action)
        {
            UpdateToolstrip();
        }

        private void StateMachineOnStateChanged()
        {
            UpdateToolstrip();
            UpdateStatusBar();
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // Cleanup dock panel hint proxy windows (Flax will destroy them by var but it's better to clear them earlier)
            DockHintWindow.Proxy.Dispose();
        }

        private void CreateStyle()
        {
            var style = new Style();

            // Note: we pre-create editor style in constructor and load icons/fonts during editor init.

            // Metro Style colors
            style.Background = Color.FromBgra(0xFF1C1C1C);
            style.LightBackground = Color.FromBgra(0xFF2D2D30);
            style.Foreground = Color.FromBgra(0xFFFFFFFF);
            style.ForegroundDisabled = new Color(0.6f);
            style.BackgroundHighlighted = Color.FromBgra(0xFF54545C);
            style.BorderHighlighted = Color.FromBgra(0xFF6A6A75);
            style.BackgroundSelected = Color.FromBgra(0xFF007ACC);
            style.BorderSelected = Color.FromBgra(0xFF1C97EA);
            style.BackgroundNormal = Color.FromBgra(0xFF3F3F46);
            style.BorderNormal = Color.FromBgra(0xFF54545C);
            style.TextBoxBackground = Color.FromBgra(0xFF333337);
            style.TextBoxBackgroundSelected = Color.FromBgra(0xFF3F3F46);
            style.DragWindow = style.BackgroundSelected * 0.7f;
            style.ProgressNormal = Color.FromBgra(0xFF0ad328);

            // Color picking
            ColorValueBox.ShowPickColorDialog += ShowPickColorDialog;

            // Font
            var primaryFont = FlaxEngine.Content.LoadInternal<FontAsset>(EditorAssets.PrimaryFont);
            if (primaryFont)
            {
                primaryFont.WaitForLoaded();

                // Create fonts
                style.FontTitle = primaryFont.CreateFont(18);
                style.FontLarge = primaryFont.CreateFont(14);
                style.FontMedium = primaryFont.CreateFont(9);
                style.FontSmall = primaryFont.CreateFont(9);

                Assert.IsNotNull(style.FontTitle, "Missing Title font.");
                Assert.IsNotNull(style.FontLarge, "Missing Large font.");
                Assert.IsNotNull(style.FontMedium, "Missing Medium font.");
                Assert.IsNotNull(style.FontSmall, "Missing Small font.");
            }
            else
            {
                Editor.LogError("Cannot load primary GUI Style font " + EditorAssets.PrimaryFont);
            }

            // Icons
            style.ArrowDown = Editor.Icons.ArrowDown12;
            style.ArrowRight = Editor.Icons.ArrowRight12;
            style.Search = Editor.Icons.Search12;
            style.Settings = Editor.Icons.Settings12;
            style.Cross = Editor.Icons.Cross12;
            style.CheckBoxIntermediate = Editor.Icons.CheckBoxIntermediate12;
            style.CheckBoxTick = Editor.Icons.CheckBoxTick12;
            style.StatusBarSizeGrip = Editor.Icons.StatusBarSizeGrip12;
            style.Translate = Editor.Icons.Translate16;
            style.Rotate = Editor.Icons.Rotate16;
            style.Scale = Editor.Icons.Scale16;

            style.SharedTooltip = new Tooltip();

            VisjectSurfaceBackground = FlaxEngine.Content.LoadAsyncInternal<Texture>("Editor/VisjectSurface");

            // Set as default
            Style.Current = style;
        }

        private IColorPickerDialog ShowPickColorDialog(Color initialValue, ColorValueBox.ColorPickerEvent colorChanged, bool useDynamicEditing)
        {
            var dialog = new ColorPickerDialog(initialValue, colorChanged, useDynamicEditing);
            dialog.Show();
            return dialog;
        }

        private void InitMainMenu(FlaxEngine.GUI.RootControl mainWindow)
        {
            MainMenu = new MainMenu();
            MainMenu.Parent = mainWindow;

            // File
            MenuFile = MainMenu.AddButton("File");
            var cm = MenuFile.ContextMenu;
            cm.VisibleChanged += OnMenuFileShowHide;
            cm.AddButton("Save All", "Ctrl+S", Editor.SaveAll);
            _menuFileSaveScenes = cm.AddButton("Save scenes", Editor.Scene.SaveScenes);
            _menuFileCloseScenes = cm.AddButton("Close scenes", Editor.Scene.CloseAllScenes);
            cm.AddSeparator();
            cm.AddButton("Open Visual Studio project", Editor.Instance.CodeEditing.OpenSolution);
            cm.AddButton("Regenerate solution file", () => ScriptsBuilder.GenerateProject(true, true));
            cm.AddButton("Recompile scripts", ScriptsBuilder.Compile);
            cm.AddSeparator();
            cm.AddButton("Open project...", OpenProject);
            cm.AddSeparator();
            if (Editor.IsDevInstance())
            {
                cm.AddButton("Regenerate Engine API", () => ScriptsBuilder.Internal_GenerateApi(ScriptsBuilder.ApiEngineType.Engine));
                cm.AddButton("Regenerate Editor API", () => ScriptsBuilder.Internal_GenerateApi(ScriptsBuilder.ApiEngineType.Editor));
                cm.AddSeparator();
            }
            cm.AddButton("Exit", "Alt+F4", () => Editor.Windows.MainWindow.Close(ClosingReason.User));

            // Edit
            MenuEdit = MainMenu.AddButton("Edit");
            cm = MenuEdit.ContextMenu;
            cm.VisibleChanged += OnMenuEditShowHide;
            _menuEditUndo = cm.AddButton(string.Empty, "Ctrl+Z", Editor.PerformUndo);
            _menuEditRedo = cm.AddButton(string.Empty, "Ctrl+Y", Editor.PerformRedo);
            cm.AddSeparator();
            _menuEditCut = cm.AddButton("Cut", "Ctrl+X", Editor.SceneEditing.Cut);
            _menuEditCopy = cm.AddButton("Copy", "Ctrl+C", Editor.SceneEditing.Copy);
            cm.AddButton("Paste", "Ctrl+V", Editor.SceneEditing.Paste);
            cm.AddSeparator();
            _menuEditDelete = cm.AddButton("Delete", "Del", Editor.SceneEditing.Delete);
            _menuEditDuplicate = cm.AddButton("Duplicate", "Ctrl+D", Editor.SceneEditing.Duplicate);
            cm.AddSeparator();
            _menuEditSelectAll = cm.AddButton("Select all", "Ctrl+A", Editor.SceneEditing.SelectAllScenes);
            cm.AddButton("Find", "Ctrl+F", Editor.Windows.SceneWin.Search);

            // Scene
            MenuScene = MainMenu.AddButton("Scene");
            cm = MenuScene.ContextMenu;
            cm.VisibleChanged += OnMenuSceneShowHide;
            _menuSceneMoveActorToViewport = cm.AddButton("Move actor to viewport", MoveActorToViewport);
            _menuSceneAlignActorWithViewport = cm.AddButton("Align actor with viewport", AlignActorWithViewport);
            _menuSceneAlignViewportWtihActor = cm.AddButton("Align viewport with actor", AlignViewportWithActor);
            cm.AddSeparator();
            _menuSceneCreateTerrain = cm.AddButton("Create terrain", CreateTerrain);

            // Game
            MenuGame = MainMenu.AddButton("Game");
            cm = MenuGame.ContextMenu;
            cm.VisibleChanged += OnMenuGameShowHide;
            _menuGamePlay = cm.AddButton("Play", "F5", Editor.Simulation.RequestStartPlay);
            _menuGamePause = cm.AddButton("Pause", "F6", Editor.Simulation.RequestPausePlay);

            // Tools
            MenuTools = MainMenu.AddButton("Tools");
            cm = MenuTools.ContextMenu;
            cm.VisibleChanged += OnMenuToolsShowHide;
            _menuToolsBakeLightmaps = cm.AddButton("Bake lightmaps", "Ctrl+F10", Editor.BakeLightmapsOrCancel);
            _menuToolsClearLightmaps = cm.AddButton("Clear lightmaps data", Editor.ClearLightmaps);
            _menuToolsBakeAllEnvProbes = cm.AddButton("Bake all env probes", BakeAllEnvProbes);
            _menuToolsBuildCSGMesh = cm.AddButton("Build CSG mesh", BuildCSG);
            cm.AddSeparator();
            cm.AddButton("Game Cooker", Editor.Windows.GameCookerWin.FocusOrShow);
            _menuToolsCancelBuilding = cm.AddButton("Cancel building game", () => GameCooker.Cancel());
            cm.AddSeparator();
            cm.AddButton("Profiler", Editor.Windows.ProfilerWin.FocusOrShow);
            cm.AddSeparator();
            _menuToolsSetTheCurrentSceneViewAsDefault = cm.AddButton("Set current scene view as project default", SetTheCurrentSceneViewAsDefault);
            cm.AddButton("Take screenshot!", "F12", Editor.Windows.TakeScreenshot);
            cm.AddSeparator();
            cm.AddButton("Plugin Exporter", PluginExporterDialog.ShowIt);
            cm.AddButton("Plugins", () => Editor.Windows.PluginsWin.Show());
            cm.AddButton("Options", () => Editor.Windows.EditorOptionsWin.Show());

            // Window
            MenuWindow = MainMenu.AddButton("Window");
            cm = MenuWindow.ContextMenu;
            cm.VisibleChanged += OnMenuWindowVisibleChanged;
            cm.AddButton("Content", Editor.Windows.ContentWin.FocusOrShow);
            cm.AddButton("Scene Tree", Editor.Windows.SceneWin.FocusOrShow);
            cm.AddButton("Toolbox", Editor.Windows.ToolboxWin.FocusOrShow);
            cm.AddButton("Properties", Editor.Windows.PropertiesWin.FocusOrShow);
            cm.AddButton("Game", Editor.Windows.GameWin.FocusOrShow);
            cm.AddButton("Editor", Editor.Windows.EditWin.FocusOrShow);
            cm.AddButton("Debug Log", Editor.Windows.DebugWin.FocusOrShow);
            cm.AddButton("Graphics Quality", Editor.Windows.GraphicsQualityWin.FocusOrShow);
            cm.AddButton("Game Cooker", Editor.Windows.GameCookerWin.FocusOrShow);
            cm.AddButton("Profiler", Editor.Windows.ProfilerWin.FocusOrShow);
            cm.AddSeparator();
            cm.AddButton("Save window layout", Editor.Windows.SaveLayout);
            _menuWindowApplyWindowLayout = cm.AddChildMenu("Apply window layout");
            cm.AddButton("Restore default layout", Editor.Windows.LoadDefaultLayout);

            // Help
            MenuHelp = MainMenu.AddButton("Help");
            cm = MenuHelp.ContextMenu;
            cm.AddButton("Discord", () => Application.StartProcess(Constants.DiscordUrl));
            cm.AddButton("Documentation", () => Application.StartProcess(Constants.DocsUrl));
            cm.AddButton("Report an issue", () => Application.StartProcess(Constants.BugTrackerUrl));
            cm.AddSeparator();
            cm.AddButton("Official Website", () => Application.StartProcess(Constants.WebsiteUrl));
            cm.AddButton("Facebook Fanpage", () => Application.StartProcess(Constants.FacebookUrl));
            cm.AddButton("Youtube Channel", () => Application.StartProcess(Constants.YoutubeUrl));
            cm.AddButton("Twitter", () => Application.StartProcess(Constants.TwitterUrl));
            cm.AddSeparator();
            cm.AddButton("Information about Flax", () => new AboutDialog().Show());
        }

        private void InitToolstrip(FlaxEngine.GUI.RootControl mainWindow)
        {
            ToolStrip = new ToolStrip();
            ToolStrip.Parent = mainWindow;

            ToolStrip.AddButton(Editor.Icons.Save32, Editor.SaveAll).LinkTooltip("Save all (Ctrl+S)");
            ToolStrip.AddSeparator();
            _toolStripUndo = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Undo32, Editor.PerformUndo).LinkTooltip("Undo (Ctrl+Z)");
            _toolStripRedo = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Redo32, Editor.PerformRedo).LinkTooltip("Redo (Ctrl+Y)");
            ToolStrip.AddSeparator();
            _toolStripTranslate = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Translate32, () => Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Translate).LinkTooltip("Change Gizmo tool mode to Translate (1)");
            _toolStripRotate = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Rotate32, () => Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Rotate).LinkTooltip("Change Gizmo tool mode to Rotate (2)");
            _toolStripScale = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Scale32, () => Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Scale).LinkTooltip("Change Gizmo tool mode to Scale (3)");
            ToolStrip.AddSeparator();
            _toolStripPlay = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Play32, Editor.Simulation.RequestPlayOrStopPlay).LinkTooltip("Start/Stop simulation (F5)");
            _toolStripPause = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Pause32, Editor.Simulation.RequestResumeOrPause).LinkTooltip("Pause/Resume simulation(F6)");
            _toolStripStep = (ToolStripButton)ToolStrip.AddButton(Editor.Icons.Step32, Editor.Simulation.RequestPlayOneFrame).LinkTooltip("Step one frame in simulation");

            UpdateToolstrip();
        }

        private void InitStatusBar(FlaxEngine.GUI.RootControl mainWindow)
        {
            // Status Bar
            StatusBar = new StatusBar
            {
                Text = "Loading...",
                Parent = mainWindow
            };

            // Progress bar with label
            const float progressBarWidth = 120.0f;
            const float progressBarHeight = 18;
            const float progressBarRightMargin = 4;
            const float progressBarLeftMargin = 4;
            var progressPanel = new Panel(ScrollBars.None)
            {
                Visible = false,
                DockStyle = DockStyle.Fill,
                Parent = StatusBar
            };
            _progressBar = new ProgressBar(
                progressPanel.Width - progressBarWidth - progressBarRightMargin,
                (StatusBar.Height - progressBarHeight) * 0.5f,
                progressBarWidth,
                progressBarHeight)
            {
                AnchorStyle = AnchorStyle.CenterRight,
                Parent = progressPanel
            };
            _progressLabel = new Label(0, 0, _progressBar.Left - progressBarLeftMargin, progressPanel.Height)
            {
                HorizontalAlignment = TextAlignment.Far,
                AnchorStyle = AnchorStyle.CenterRight,
                Parent = progressPanel
            };

            UpdateStatusBar();
        }

        private void InitDockPanel(FlaxEngine.GUI.RootControl mainWindow)
        {
            // Dock Panel
            MasterPanel.Parent = mainWindow;
        }

        private void OpenProject()
        {
            // Ask user to select project file
            var files = MessageBox.OpenFileDialog(Editor.Windows.MainWindow, null, "Project files (Project.xml)\0Project.xml\0All files (*.*)\0*.*\0", false, "Select project file");
            if (files != null && files.Length > 0)
            {
                Editor.OpenProject(files[0]);
            }
        }

        private void OnMenuFileShowHide(Control control)
        {
            if (control.Visible == false)
                return;
            var c = (ContextMenu)control;

            bool hasOpenedScene = SceneManager.IsAnySceneLoaded;

            _menuFileSaveScenes.Enabled = hasOpenedScene;
            _menuFileCloseScenes.Enabled = hasOpenedScene;

            c.PerformLayout();
        }

        private void OnMenuEditShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var undoRedo = Editor.Undo;
            var hasSthSelected = Editor.SceneEditing.HasSthSelected;

            bool canUndo = undoRedo.CanUndo;
            _menuEditUndo.Enabled = canUndo;
            _menuEditUndo.Text = canUndo ? string.Format("Undo \'{0}\'", undoRedo.FirstUndoName) : "No undo";

            bool canRedo = undoRedo.CanRedo;
            _menuEditRedo.Enabled = canRedo;
            _menuEditRedo.Text = canRedo ? string.Format("Redo \'{0}\'", undoRedo.FirstRedoName) : "No redo";

            _menuEditCut.Enabled = hasSthSelected;
            _menuEditCopy.Enabled = hasSthSelected;
            _menuEditDelete.Enabled = hasSthSelected;
            _menuEditDuplicate.Enabled = hasSthSelected;
            _menuEditSelectAll.Enabled = SceneManager.IsAnySceneLoaded;

            control.PerformLayout();
        }

        private void OnMenuSceneShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var selection = Editor.SceneEditing;
            bool hasActorSelected = selection.HasSthSelected && selection.Selection[0] is ActorNode;

            _menuSceneMoveActorToViewport.Enabled = hasActorSelected;
            _menuSceneAlignActorWithViewport.Enabled = hasActorSelected;
            _menuSceneAlignViewportWtihActor.Enabled = hasActorSelected;
            _menuSceneCreateTerrain.Enabled = SceneManager.IsAnySceneLoaded && Editor.StateMachine.CurrentState.CanEditScene && !Editor.StateMachine.IsPlayMode;

            control.PerformLayout();
        }

        private void OnMenuGameShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var c = (ContextMenu)control;
            bool isInPlayMode = Editor.StateMachine.IsPlayMode;
            _menuGamePlay.Enabled = !isInPlayMode;
            _menuGamePause.Enabled = isInPlayMode;

            c.PerformLayout();
        }

        private void OnMenuToolsShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var c = (ContextMenu)control;
            bool canBakeLightmaps = GraphicsDevice.Limits.IsComputeSupported;
            bool canEdit = SceneManager.IsAnySceneLoaded && Editor.StateMachine.IsEditMode;
            bool isBakingLightmaps = Editor.ProgressReporting.BakeLightmaps.IsActive;
            _menuToolsBakeLightmaps.Enabled = (canEdit && canBakeLightmaps) || isBakingLightmaps;
            _menuToolsBakeLightmaps.Text = isBakingLightmaps ? "Cancel baking lightmaps" : "Bake lightmaps";
            _menuToolsClearLightmaps.Enabled = canEdit;
            _menuToolsBakeAllEnvProbes.Enabled = canEdit;
            _menuToolsBuildCSGMesh.Enabled = canEdit;
            _menuToolsCancelBuilding.Enabled = GameCooker.IsRunning;
            _menuToolsSetTheCurrentSceneViewAsDefault.Enabled = SceneManager.ScenesCount > 0;

            c.PerformLayout();
        }

        private void OnMenuWindowVisibleChanged(Control menu)
        {
            if (!menu.Visible)
                return;
            
            // Find layout to use
            var searchFolder = Globals.ProjectCacheFolder;
            var files = Directory.GetFiles(searchFolder, "Layout_*.xml", SearchOption.TopDirectoryOnly);
            var layouts = _menuWindowApplyWindowLayout.ContextMenu;
            layouts.DisposeAllItems();
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var name = file.Substring(searchFolder.Length + 8, file.Length - searchFolder.Length - 12);
                var button = layouts.AddButton(name, OnApplyLayoutButtonClicked);
                button.Tag = file;
            }
        }

        private void OnApplyLayoutButtonClicked(ContextMenuButton button)
        {
            Editor.Windows.LoadLayout((string)button.Tag);
        }

        private void AlignViewportWithActor()
        {
            var selection = Editor.SceneEditing;
            if (selection.HasSthSelected && selection.Selection[0] is ActorNode node)
            {
                var actor = node.Actor;
                var viewport = Editor.Windows.EditWin.Viewport;
                ((FPSCamera)viewport.ViewportCamera).MoveViewport(actor.Transform);
            }
        }

        private void MoveActorToViewport()
        {
            var selection = Editor.SceneEditing;
            if (selection.HasSthSelected && selection.Selection[0] is ActorNode node)
            {
                var actor = node.Actor;
                var viewport = Editor.Windows.EditWin.Viewport;
                using (new UndoBlock(Undo, actor, "Move to viewport"))
                {
                    actor.Position = viewport.ViewPosition;
                }
            }
        }

        private void AlignActorWithViewport()
        {
            var selection = Editor.SceneEditing;
            if (selection.HasSthSelected && selection.Selection[0] is ActorNode node)
            {
                var actor = node.Actor;
                var viewport = Editor.Windows.EditWin.Viewport;
                using (new UndoBlock(Undo, actor, "Align with viewport"))
                {
                    actor.Position = viewport.ViewPosition;
                    actor.Orientation = viewport.ViewOrientation;
                }
            }
        }

        private void CreateTerrain()
        {
            new Tools.Terrain.CreateTerrainDialog().Show(Editor.Windows.MainWindow);
        }

        private void BakeAllEnvProbes()
        {
            Editor.Scene.ExecuteOnGraph(node =>
            {
                if (node is EnvironmentProbeNode envProbeNode && envProbeNode.IsActive)
                {
                    ((EnvironmentProbe)envProbeNode.Actor).Bake();
                    node.ParentScene.IsEdited = true;
                }

                return node.IsActive;
            });
        }

        private void BuildCSG()
        {
            var scenes = SceneManager.Scenes;
            scenes.ToList().ForEach(x => x.BuildCSG(0));
            Editor.Scene.MarkSceneEdited(scenes);
        }

        private void SetTheCurrentSceneViewAsDefault()
        {
            var projectInfo = Editor.ProjectInfo;
            projectInfo.DefaultSceneId = SceneManager.Scenes[0].ID;
            projectInfo.DefaultSceneSpawn = Editor.Windows.EditWin.Viewport.ViewRay;
            projectInfo.Save();
        }

        private void OnMainWindowClosing()
        {
            // Clear UI references (GUI cannot be used after window closing)
            MainMenu = null;
            ToolStrip = null;
            MasterPanel = null;
            StatusBar = null;
            _progressLabel = null;
            _progressBar = null;

            MenuFile = null;
            MenuGame = null;
            MenuEdit = null;
            MenuWindow = null;
            MenuScene = null;
            MenuTools = null;
            MenuHelp = null;
        }
    }
}
