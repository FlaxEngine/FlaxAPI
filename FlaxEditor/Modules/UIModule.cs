////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Linq;
using FlaxEditor.Gizmo;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Dialogs;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
using FlaxEditor.Scripting;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Docking;
using FlaxEngine.Rendering;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages Editor UI. Especially main window UI.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class UIModule : EditorModule
    {
        private SpriteAtlas _iconsAtlas;
        private Label _progressLabel;
        private ProgressBar _progressBar;

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

        // Cached internally to improve performance

        internal Sprite FolderClosed12;
        internal Sprite FolderOpened12;

        internal UIModule(Editor editor)
            : base(editor)
        {
            InitOrder = -90;

            CreateStyle();
        }

        /// <summary>
        /// Gets the editor icon sprite with given name.
        /// </summary>
        /// <param name="name">The icon name.</param>
        /// <returns>Sprite handle (may be invalid if icon has not been found or cannot load the sprite atlas).</returns>
        public Sprite GetIcon(string name)
        {
            // Load asset if needed
            if (_iconsAtlas == null)
            {
                _iconsAtlas = FlaxEngine.Content.LoadInternal<SpriteAtlas>("Editor/IconsAtlas");
                if (_iconsAtlas == null)
                {
                    // Error
                    Editor.LogError("Cannot load editor icons atlas.");
                    return Sprite.Invalid;
                }
                Assert.IsTrue(_iconsAtlas.IsLoaded);
            }

            // Find icon
            var result = _iconsAtlas.GetSprite(name);
            if (!result.IsValid)
            {
                // Warning
                Editor.LogWarning($"Failed to load sprite icon \'{name}\'.");
            }

            return result;
        }

        /// <summary>
        /// Unchecks toolstrip pause button.
        /// </summary>
        public void UncheckPauseButton()
        {
            if (ToolStrip != null)
                ToolStrip.GetButton(9).Checked = false;
        }

        /// <summary>
        /// Checks if toolstrip pause button is being checked.
        /// </summary>
        public bool IsPauseButtonChecked => ToolStrip != null && ToolStrip.GetButton(9).Checked;

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
            //ToolStrip.GetButton(2).Enabled = Editor.IsEdited;// Save All
            //
            ToolStrip.GetButton(3).Enabled = canEditScene && undoRedo.CanUndo;// Undo
            ToolStrip.GetButton(4).Enabled = canEditScene && undoRedo.CanRedo;// Redo
            //
            var gizmoMode = gizmo.ActiveMode;
            ToolStrip.GetButton(5).Checked = gizmoMode == TransformGizmo.Mode.Translate;// Translate mode
            ToolStrip.GetButton(6).Checked = gizmoMode == TransformGizmo.Mode.Rotate;// Rotate mode
            ToolStrip.GetButton(7).Checked = gizmoMode == TransformGizmo.Mode.Scale;// Scale mode
            //
            var play = ToolStrip.GetButton(8);// Play
            var pause = ToolStrip.GetButton(9);// Pause
            var step = ToolStrip.GetButton(10);// Step
            play.Enabled = canEnterPlayMode;
            if (Editor.StateMachine.IsPlayMode)
            {
                play.Checked = false;
                play.Icon = GetIcon("Stop32");
                pause.Enabled = true;
                pause.Checked = Editor.StateMachine.PlayingState.IsPaused;
                pause.AutoCheck = false;
                step.Enabled = true;
            }
            else
            {
                play.Checked = Editor.Simulation.IsPlayModeRequested;
                play.Icon = GetIcon("Play32");
                pause.Enabled = canEnterPlayMode;
                pause.AutoCheck = true;
                step.Enabled = false;
            }
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
                _progressBar.Value = progress;
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Editor.Windows.OnMainWindowClosing += OnOnMainWindowClosing;
            var mainWindow = Editor.Windows.MainWindow.GUI;
            
            // Update window background
            mainWindow.BackgroundColor = Style.Current.Background;
            
            InitMainMenu(mainWindow);
            InitToolstrip(mainWindow);
            InitStatusBar(mainWindow);
            InitDockPanel(mainWindow);

            // Precache hint windows
            DockHintWindow.Proxy.InitHitProxy();
        }

        /// <inheritdoc />
        public override void OnEndInit()
        {
            Editor.MainTransformGizmo.OnModeChanged += UpdateToolstrip;
            Editor.StateMachine.StateChanged += StateMachineOnStateChanged;
            Editor.Undo.UndoDone += UpdateToolstrip;
            Editor.Undo.RedoDone += UpdateToolstrip;
            Editor.Undo.ActionDone += UpdateToolstrip;

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
            _iconsAtlas = null;

            // Cleanup dock panel hint proxy windows (Flax will destroy them by var but it's better to clear them earlier)
            DockHintWindow.Proxy.Dispsoe();
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
            style.ShowPickColorDialog += (color, handler) => new ColorPickerDialog(color, handler).Show();
            
            // Font
            string primaryFontNameInternal = "Editor/Segoe Media Center Regular";
            var primaryFont = FlaxEngine.Content.LoadInternal<FontAsset>(primaryFontNameInternal);
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
                Debug.LogError("Cannot load primary GUI Style font " + primaryFontNameInternal);
            }

            // Icons
            style.ArrowDown = GetIcon("ArrowDown12");
            style.ArrowRight = GetIcon("ArrowRight12");
            style.Search = GetIcon("Search12");
            style.Settings = GetIcon("Settings12");
            style.Cross = GetIcon("Cross12");
            style.CheckBoxIntermediate = GetIcon("CheckBoxIntermediate12");
            style.CheckBoxTick = GetIcon("CheckBoxTick12");
            style.StatusBarSizeGrip = GetIcon("StatusBarSizeGrip12");
            style.Translate16 = GetIcon("Translate16");
            style.Rotate16 = GetIcon("Rotate16");
            style.Scale16 = GetIcon("Scale16");

            style.SharedTooltip = new Tooltip();

            // Cache icons
            FolderClosed12 = GetIcon("FolderClosed12");
            FolderOpened12 = GetIcon("FolderOpened12");
            VisjectSurfaceBackground = FlaxEngine.Content.LoadAsyncInternal<Texture>("Editor/VisjectSurface");

            // Set as default
            Style.Current = style;
        }

        private void InitMainMenu(FlaxEngine.GUI.Window mainWindow)
        {
            MainMenu = new MainMenu();
            MainMenu.Parent = mainWindow;

            // File
            var mm_File = MainMenu.AddButton("File");
            mm_File.ContextMenu.OnButtonClicked += mm_File_Click;
            mm_File.ContextMenu.VisibleChanged += mm_File_ShowHide;
            mm_File.ContextMenu.AddButton(2, "Save Scenes");
            mm_File.ContextMenu.AddButton(3, "Save All", "Ctrl+S");
            mm_File.ContextMenu.AddButton(4, "Close scenes");
            mm_File.ContextMenu.AddSeparator();
            mm_File.ContextMenu.AddButton(7, "Open Visual Studio project");
            mm_File.ContextMenu.AddButton(8, "Regenerate solution file");
            mm_File.ContextMenu.AddButton(9, "Recompile scripts");
            mm_File.ContextMenu.AddSeparator();
            if (Editor.IsDevInstance)
            {
                mm_File.ContextMenu.AddButton(98, "Regenerate Engine API");
                mm_File.ContextMenu.AddButton(99, "Regenerate Editor API");
                mm_File.ContextMenu.AddSeparator();
            }
            mm_File.ContextMenu.AddButton(6, "Exit", "Alt+F4");

            // Edit
            var mm_Edit = MainMenu.AddButton("Edit");
            mm_Edit.ContextMenu.OnButtonClicked += mm_Edit_Click;
            mm_Edit.ContextMenu.VisibleChanged += mm_Edit_ShowHide;
            mm_Edit.ContextMenu.AddButton(1, string.Empty, "Ctrl+Z");// Undo text is updated in mm_Edit_ShowHide
            mm_Edit.ContextMenu.AddButton(2, string.Empty, "Ctrl+Y");// Redo text is updated in mm_Edit_ShowHide
            mm_Edit.ContextMenu.AddSeparator();
            mm_Edit.ContextMenu.AddButton(3, "Cut", "Ctrl+X");
            mm_Edit.ContextMenu.AddButton(4, "Copy", "Ctrl+C");
            mm_Edit.ContextMenu.AddButton(5, "Paste", "Ctrl+V");
            mm_Edit.ContextMenu.AddSeparator();
            mm_Edit.ContextMenu.AddButton(6, "Delete", "Del");
            mm_Edit.ContextMenu.AddButton(7, "Duplicate", "Ctrl+D");
            mm_Edit.ContextMenu.AddSeparator();
            mm_Edit.ContextMenu.AddButton(8, "Select all", "Ctrl+A");
            mm_Edit.ContextMenu.AddButton(9, "Find", "Ctrl+F");

            // Scene
            var mm_Scene = MainMenu.AddButton("Scene");
            mm_Scene.ContextMenu.OnButtonClicked += mm_Scene_Click;
            mm_Scene.ContextMenu.VisibleChanged += mm_Scene_ShowHide;
            //mm_Scene.ContextMenu.AddButton(1, "Go to location...");
            //mm_scene.AddSeparator();
            mm_Scene.ContextMenu.AddButton(3, "Move actor to viewport");
            mm_Scene.ContextMenu.AddButton(4, "Align actor with viewport");
            mm_Scene.ContextMenu.AddButton(2, "Align viewport with actor");

            // Game
            var mm_Game = MainMenu.AddButton("Game");
            mm_Game.ContextMenu.OnButtonClicked += mm_Game_Click;
            mm_Game.ContextMenu.VisibleChanged += mm_Game_ShowHide;
            mm_Game.ContextMenu.AddButton(1, "Play", "F5");
            mm_Game.ContextMenu.AddButton(2, "Pause");

            // Tools
            var mm_Tools = MainMenu.AddButton("Tools");
            mm_Tools.ContextMenu.OnButtonClicked += mm_Tools_Click;
            mm_Tools.ContextMenu.VisibleChanged += mm_Tools_ShowHide;
            //mm_Tools.ContextMenu.AddButton(1, "Scene statistics");
            mm_Tools.ContextMenu.AddButton(2, "Bake lightmaps", "Ctrl+F10");
            mm_Tools.ContextMenu.AddButton(3, "Clear lightmaps data");
            mm_Tools.ContextMenu.AddButton(5, "Bake all env probes");
            mm_Tools.ContextMenu.AddButton(6, "Build CSG mesh");
            mm_Tools.ContextMenu.AddSeparator();
            mm_Tools.ContextMenu.AddButton(7, "Game Cooker");
            mm_Tools.ContextMenu.AddButton(8, "Cancel building game");
            mm_Tools.ContextMenu.AddSeparator();
            mm_Tools.ContextMenu.AddButton(9, "Profiler");
            mm_Tools.ContextMenu.AddSeparator();
            mm_Tools.ContextMenu.AddButton(4, "Take screenshot!", "F12");
            //mm_Tools.ContextMenu.AddSeparator();
            //mm_Tools.ContextMenu.AddButton(4, "Options");

            // Window
            var mm_Window = MainMenu.AddButton("Window");
            mm_Window.ContextMenu.OnButtonClicked += mm_Window_Click;
            mm_Window.ContextMenu.AddButton(10, "Content");
            mm_Window.ContextMenu.AddButton(11, "Scene Tree");
            mm_Window.ContextMenu.AddButton(12, "Toolbox");
            mm_Window.ContextMenu.AddButton(13, "Properties");
            mm_Window.ContextMenu.AddButton(15, "Game");
            mm_Window.ContextMenu.AddButton(16, "Editor");
            mm_Window.ContextMenu.AddButton(17, "Debug Log");
            mm_Window.ContextMenu.AddButton(18, "Graphics Quality");
            mm_Window.ContextMenu.AddSeparator();
            mm_Window.ContextMenu.AddButton(1, "Restore default layout");

            // Help
            var mm_Help = MainMenu.AddButton("Help");
            mm_Help.ContextMenu.OnButtonClicked += mm_Help_Click;
            mm_Help.ContextMenu.AddButton(1, "Forum");
            mm_Help.ContextMenu.AddButton(2, "Documentation");
            mm_Help.ContextMenu.AddButton(3, "Report an issue");
            mm_Help.ContextMenu.AddSeparator();
            mm_Help.ContextMenu.AddButton(7, "Official Website");
            mm_Help.ContextMenu.AddButton(4, "Facebook Fanpage");
            mm_Help.ContextMenu.AddButton(5, "Youtube Channel");
            mm_Help.ContextMenu.AddButton(8, "Twitter");
            mm_Help.ContextMenu.AddSeparator();
            mm_Help.ContextMenu.AddButton(6, "Information about Flax");
        }

        private void InitToolstrip(FlaxEngine.GUI.Window mainWindow)
        {
            ToolStrip = new ToolStrip();
            ToolStrip.ButtonClicked += onTootlstripButtonClicked;
            ToolStrip.Parent = mainWindow;

            //ToolStrip.AddButton(0, GetIcon("Logo32")).LinkTooltip("Flax Engine");// Welcome screen
            ToolStrip.AddButton(2, GetIcon("Save32")).LinkTooltip("Save all (Ctrl+S)");// Save all
            ToolStrip.AddSeparator();
            ToolStrip.AddButton(3, GetIcon("Undo32")).LinkTooltip("Undo (Ctrl+Z)");// Undo
            ToolStrip.AddButton(4, GetIcon("Redo32")).LinkTooltip("Redo (Ctrl+Y)");// Redo
            ToolStrip.AddSeparator();
            ToolStrip.AddButton(5, GetIcon("Translate32")).LinkTooltip("Change Gizmo tool mode to Translate (1)");// Translate mode
            ToolStrip.AddButton(6, GetIcon("Rotate32")).LinkTooltip("Change Gizmo tool mode to Rotate (2)");// Rotate mode
            ToolStrip.AddButton(7, GetIcon("Scale32")).LinkTooltip("Change Gizmo tool mode to Scale (3)");// Scale mode
            ToolStrip.AddSeparator();
            ToolStrip.AddButton(8, GetIcon("Play32")).LinkTooltip("Start/Stop simulation (F5)");// Play
            ToolStrip.AddButton(9, GetIcon("Pause32")).LinkTooltip("Pause simulation");// Pause
            ToolStrip.AddButton(10, GetIcon("Step32")).LinkTooltip("Step one frame in simulation");// Step

            UpdateToolstrip();
        }

        private void InitStatusBar(FlaxEngine.GUI.Window mainWindow)
        {
            // Status Bar
            StatusBar = new StatusBar
            {
                Text = "Ready",
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

        private void InitDockPanel(FlaxEngine.GUI.Window mainWindow)
        {
            // Dock Panel
            MasterPanel.Parent = mainWindow;
        }

        private void onTootlstripButtonClicked(int id)
        {
            switch (id)
            {
                // Welcome screen
                case 0:
                    new AboutDialog().Show();
                    break;

                // Save scene(s)
                case 1:
                    Editor.Scene.SaveScenes();
                    break;

                // Save all
                case 2:
                    Editor.SaveAll();
                    break;

                // Undo
                case 3:
                    Editor.PerformUndo();
                    break;

                // Redo
                case 4:
                    Editor.PerformRedo();
                    break;

                // Translate mode
                case 5:
                    Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Translate;
                    break;

                // Rotate mode
                case 6:
                    Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Rotate;
                    break;

                // Scale mode
                case 7:
                    Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Scale;
                    break;

                // Play
                case 8:
                    Editor.Simulation.RequestPlayOrStopPlay();
                    break;

                // Pause
                case 9:
                {
                    // Check if Editor is in pause state
                    if (Editor.StateMachine.PlayingState.IsPaused)
                    {
                        // Resume game
                        Editor.Simulation.RequestResumePlay();
                    }
                    else
                    {
                        // Pause game
                        Editor.Simulation.RequestPausePlay();
                    }
                    break;
                }

                // Step
                case 10:
                    Editor.Simulation.RequestPlayOneFrame();
                    break;
            }
        }

        private void mm_File_Click(int id, ContextMenu cm)
        {
            switch (id)
            {
                // Save Scenes
                case 2:
                    Editor.Scene.SaveScenes();
                    break;

                // Save All
                case 3:
                    Editor.SaveAll();
                    break;

                // Close scenes
                case 4:
                    Editor.Scene.CloseAllScenes();
                    break;
                    
                // Exit
                case 6:
                    Editor.Windows.MainWindow.Close(ClosingReason.User);
                    break;

                // Open Visual Studio project
                case 7:
                    ScriptsBuilder.OpenSolution();
                    break;

                // Regenerate solution file
                case 8:
                    ScriptsBuilder.GenerateProject(true, true);
                    break;

                // Recompile scripts
                case 9:
                    ScriptsBuilder.Compile();
                    break;

                // Regererate api
                case 98:
                    ScriptsBuilder.Internal_GenerateApi(ScriptsBuilder.ApiEngineType.Engine);
                    break;
                case 99:
                    ScriptsBuilder.Internal_GenerateApi(ScriptsBuilder.ApiEngineType.Editor);
                    break;
            }
        }

        private void mm_File_ShowHide(Control control)
        {
            if (control.Visible == false)
                return;
            var c = (ContextMenu)control;

            bool hasOpenedScene = SceneManager.IsAnySceneLoaded;

            c.GetButton(2).Enabled = hasOpenedScene;// Save Scenes
            c.GetButton(4).Enabled = hasOpenedScene;// Close scenes

            c.PerformLayout();
        }

        private void mm_Edit_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                case 1:
                    Editor.PerformUndo();
                    break;
                case 2:
                    Editor.PerformRedo();
                    break;
                case 3:
                    Editor.SceneEditing.Cut();
                    break;
                case 4:
                    Editor.SceneEditing.Copy();
                    break;
                case 5:
                    Editor.SceneEditing.Paste();
                    break;
                case 6:
                    Editor.SceneEditing.Delete();
                    break;
                case 7:
                    Editor.SceneEditing.Duplicate();
                    break;
                case 8:
                    Editor.SceneEditing.SelectAllScenes();
                    break;
                case 9:
                    Editor.Windows.SceneWin.Search();
                    break;
            }
        }

        private void mm_Edit_ShowHide(Control control)
        {
            if (control.Visible == false)
                return;
            var c = (ContextMenu)control;

            var undoRedo = Editor.Undo;
            var hasSthSelected = Editor.SceneEditing.HasSthSelected;

            var undoButton = c.GetButton(1);// Undo
            undoButton.Enabled = undoRedo.CanUndo;
            undoButton.Text = undoRedo.CanUndo ? string.Format("Undo \'{0}\'", undoRedo.FirstUndoName) : "No undo";

            var redoButton = c.GetButton(2);// Redo
            redoButton.Enabled = undoRedo.CanRedo;
            redoButton.Text = undoRedo.CanRedo ? string.Format("Redo \'{0}\'", undoRedo.FirstRedoName) : "No redo";

            c.GetButton(3).Enabled = hasSthSelected;// Cut
            c.GetButton(4).Enabled = hasSthSelected;// Copy
            c.GetButton(6).Enabled = hasSthSelected;// Delete
            c.GetButton(7).Enabled = hasSthSelected;// Duplicate
            c.GetButton(8).Enabled = SceneManager.IsAnySceneLoaded;// Select All

            c.PerformLayout();
        }

        private void mm_Scene_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                // Got to location...
                case 1: break;

                // Align viewport with actor
                case 2:
                {
                    var selection = Editor.SceneEditing;
                    if (selection.HasSthSelected && selection.Selection[0] is ActorNode node)
                    {
                        var actor = node.Actor;
                        var viewport = Editor.Windows.EditWin.Viewport;
                        viewport.MoveViewport(actor.Transform);
                    }
                    break;
                }

                // Move actor to viewport
                case 3:
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
                    break;
                }

                // Align actor with viewport
                case 4:
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
                    break;
                }
            }
        }

        private void mm_Scene_ShowHide(Control control)
        {
            if (control.Visible == false)
                return;
            var c = (ContextMenu)control;

            var selection = Editor.SceneEditing;
            bool hasActorSelected = selection.HasSthSelected && selection.Selection[0] is ActorNode;

            c.GetButton(2).Enabled = hasActorSelected;
            c.GetButton(3).Enabled = hasActorSelected;
            c.GetButton(4).Enabled = hasActorSelected;
        }

        private void mm_Game_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                // Play
                case 1: Editor.Simulation.RequestStartPlay(); break;
                    
                // Pause
                case 2: Editor.Simulation.RequestPausePlay(); break;
            }
        }

        private void mm_Game_ShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var c = (ContextMenu)control;
            bool isInPlayMode = Editor.StateMachine.IsPlayMode;
            c.GetButton(1).Enabled = !isInPlayMode; // Play
            c.GetButton(2).Enabled = isInPlayMode; // Pause

            c.PerformLayout();
        }

        private void mm_Tools_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                // Scene statistics
                case 1: break;

                // Bake lightmaps
                case 2:
                    Editor.BakeLightmapsOrCancel();
                    break;

                // Clear lightmaps data
                case 3:
                    Editor.ClearLightmaps();
                    break;

                // Take screenshot!
                case 4:
                    Editor.Windows.TakeScreenshot();
                    break;

                // Bake all env probes
                case 5:
                {
                    Editor.Scene.ExecuteOnGraph(node =>
                                                {
                                                    if (node is EnvironmentProbeNode envProbeNode)
                                                    {
                                                        ((EnvironmentProbe)envProbeNode.Actor).Bake();
                                                        node.ParentScene.IsEdited = true;
                                                    }
                                                    return node.IsActive;
                                                });
                    break;
                }

                // Build CSG mesh
                case 6:
                {
                    var scenes = SceneManager.Scenes;
                    scenes.ToList().ForEach(x => x.BuildCSG());
                    Editor.Scene.MarkSceneEdited(scenes);
                    break;
                }

                // Game Cooker
                case 7:
                {
                    Editor.Windows.GameCookerWin.ShowFloating();
                    break;
                }

                // Cancel building game
                case 8:
                {
                    GameCooker.Cancel();
                    break;
                }

                // Profiler
                case 9:
                {
                    Editor.Windows.ProfilerWin.FocusOrShow();
                    break;
                }
            }
        }

        private void mm_Tools_ShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var c = (ContextMenu)control;
            bool canBakeLightmaps = GraphicsDevice.Limits.IsComputeSupported;
            bool canEdit = SceneManager.IsAnySceneLoaded && Editor.StateMachine.IsEditMode;
            bool isBakingLightmaps = Editor.ProgressReporting.BakeLightmaps.IsActive;
            c.GetButton(2).Enabled = (canEdit && canBakeLightmaps) || isBakingLightmaps;// Bake lightmaps
            c.GetButton(2).Text = isBakingLightmaps ? "Cancel baking lightmaps" : "Bake lightmaps";
            c.GetButton(3).Enabled = canEdit;// Clear lightmaps data
            c.GetButton(5).Enabled = canEdit;// Bake all env probes
            c.GetButton(6).Enabled = canEdit;// Build CSG mesh
            c.GetButton(8).Enabled = GameCooker.IsRunning;//Cancel building game

            c.PerformLayout();
        }

        private void mm_Window_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                // Restore default layout
                case 1:
                    Editor.Windows.LoadDefaultLayout();
                    break;

                // Windows
                case 10:
                    Editor.Windows.ContentWin.FocusOrShow();
                    break;
                case 11:
                    Editor.Windows.SceneWin.FocusOrShow();
                    break;
                case 12:
                    Editor.Windows.ToolboxWin.FocusOrShow();
                    break;
                case 13:
                    Editor.Windows.PropertiesWin.FocusOrShow();
                    break;
                case 15:
                    Editor.Windows.GameWin.FocusOrShow();
                    break;
                case 16:
                    Editor.Windows.EditWin.FocusOrShow();
                    break;
                case 17:
                    Editor.Windows.DebugWin.FocusOrShow();
                    break;
                case 18:
                    Editor.Windows.GraphicsQualityWin.FocusOrShow();
                    break;
            }
        }

        private void mm_Help_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                // Forum
                case 1:
                    Application.StartProcess("http://answers.flaxengine.com/");
                    break;

                // Documentation
                case 2:
                    Application.StartProcess("http://docs.flaxengine.com/");
                    break;

                // Report an issue
                case 3:
                    Application.StartProcess("https://github.com/FlaxEngine/FlaxAPI/issues");
                    break;

                // Facebook Fanpage
                case 4:
                    Application.StartProcess("https://facebook.com/FlaxEngine");
                    break;

                // Youtube Channel
                case 5:
                    Application.StartProcess("https://www.youtube.com/channel/UChdER2A3n19rJWIMOZJClhw");
                    break;

                // Information about Flax
                case 6:
                    new AboutDialog().Show();
                    break;

                // Official Website
                case 7:
                    Application.StartProcess("http://flaxengine.com");
                    break;

                // Twitter
                case 8:
                    Application.StartProcess("http://twitter.com/FlaxEngine");
                    break;
            }
        }

        private void OnOnMainWindowClosing()
        {
            // Clear UI references (GUI cannot be used after window closing)
            MainMenu = null;
            ToolStrip = null;
            MasterPanel = null;
            StatusBar = null;
            _progressLabel = null;
            _progressBar = null;
        }
    }
}
