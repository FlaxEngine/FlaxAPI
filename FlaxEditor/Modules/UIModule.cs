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
		private ContextMenuButton _menuGamePlay;
		private ContextMenuButton _menuGamePause;
		private ContextMenuButton _menuToolsBakeLightmaps;
		private ContextMenuButton _menuToolsClearLightmaps;
		private ContextMenuButton _menuToolsBakeAllEnvProbes;
		private ContextMenuButton _menuToolsBuildCSGMesh;
		private ContextMenuButton _menuToolsCancelBuilding;

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

		// Cached internally to improve performance

		internal Sprite FolderClosed12;
		internal Sprite FolderOpened12;
		internal Sprite DragBar12;

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
				_iconsAtlas = FlaxEngine.Content.LoadInternal<SpriteAtlas>(EditorAssets.IconsAtlas);
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
				Debug.LogError("Cannot load primary GUI Style font " + EditorAssets.PrimaryFont);
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
			DragBar12 = GetIcon("DragBar12");
			VisjectSurfaceBackground = FlaxEngine.Content.LoadAsyncInternal<Texture>("Editor/VisjectSurface");

			// Set as default
			Style.Current = style;
		}

		private void InitMainMenu(FlaxEngine.GUI.Window mainWindow)
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
			cm.AddButton("Open Visual Studio project", ScriptsBuilder.OpenSolution);
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
			_menuSceneAlignViewportWtihActor = cm.AddButton("Align viewport with actor", AlignViewportWtihActor);

			// Game
			MenuGame = MainMenu.AddButton("Game");
			cm = MenuGame.ContextMenu;
			cm.VisibleChanged += OnMenuGameShowHide;
			_menuGamePlay = cm.AddButton("Play", "F5", Editor.Simulation.RequestStartPlay);
			_menuGamePause = cm.AddButton("Pause", Editor.Simulation.RequestPausePlay);

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
			cm.AddButton("Take screenshot!", "F12", Editor.Windows.TakeScreenshot);

			// Window
			MenuWindow = MainMenu.AddButton("Window");
			cm = MenuWindow.ContextMenu;
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
			cm.AddButton("Restore default layout", Editor.Windows.LoadDefaultLayout);

			// Help
			MenuHelp = MainMenu.AddButton("Help");
			cm = MenuHelp.ContextMenu;
			cm.AddButton("Forum", () => Application.StartProcess("http://answers.flaxengine.com/"));
			cm.AddButton("Documentation", () => Application.StartProcess("http://docs.flaxengine.com/"));
			cm.AddButton("Report an issue", () => Application.StartProcess("https://github.com/FlaxEngine/FlaxAPI/issues"));
			cm.AddSeparator();
			cm.AddButton("Official Website", () => Application.StartProcess("http://flaxengine.com"));
			cm.AddButton("Facebook Fanpage", () => Application.StartProcess("https://facebook.com/FlaxEngine"));
			cm.AddButton("Youtube Channel", () => Application.StartProcess("https://www.youtube.com/channel/UChdER2A3n19rJWIMOZJClhw"));
			cm.AddButton("Twitter", () => Application.StartProcess("http://twitter.com/FlaxEngine"));
			cm.AddSeparator();
			cm.AddButton("Information about Flax", () => new AboutDialog().Show());
		}

		private void InitToolstrip(FlaxEngine.GUI.Window mainWindow)
		{
			ToolStrip = new ToolStrip();
			ToolStrip.Parent = mainWindow;

			ToolStrip.AddButton(GetIcon("Save32"), Editor.SaveAll).LinkTooltip("Save all (Ctrl+S)");
			ToolStrip.AddSeparator();
			_toolStripUndo = (ToolStripButton)ToolStrip.AddButton(GetIcon("Undo32"), Editor.PerformUndo).LinkTooltip("Undo (Ctrl+Z)");
			_toolStripRedo = (ToolStripButton)ToolStrip.AddButton(GetIcon("Redo32"), Editor.PerformRedo).LinkTooltip("Redo (Ctrl+Y)");
			ToolStrip.AddSeparator();
			_toolStripTranslate = (ToolStripButton)ToolStrip.AddButton(GetIcon("Translate32"), () => Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Translate).LinkTooltip("Change Gizmo tool mode to Translate (1)");
			_toolStripRotate = (ToolStripButton)ToolStrip.AddButton(GetIcon("Rotate32"), () => Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Rotate).LinkTooltip("Change Gizmo tool mode to Rotate (2)");
			_toolStripScale = (ToolStripButton)ToolStrip.AddButton(GetIcon("Scale32"), () => Editor.MainTransformGizmo.ActiveMode = TransformGizmo.Mode.Scale).LinkTooltip("Change Gizmo tool mode to Scale (3)");
			ToolStrip.AddSeparator();
			_toolStripPlay = (ToolStripButton)ToolStrip.AddButton(GetIcon("Play32"), Editor.Simulation.RequestPlayOrStopPlay).LinkTooltip("Start/Stop simulation (F5)");
			_toolStripPause = (ToolStripButton)ToolStrip.AddButton(GetIcon("Pause32"), Editor.Simulation.RequestResumeOrPause).LinkTooltip("Pause simulation");
			_toolStripStep = (ToolStripButton)ToolStrip.AddButton(GetIcon("Step32"), Editor.Simulation.RequestPlayOneFrame).LinkTooltip("Step one frame in simulation");

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

			c.PerformLayout();
		}

		private void AlignViewportWtihActor()
		{
			var selection = Editor.SceneEditing;
			if (selection.HasSthSelected && selection.Selection[0] is ActorNode node)
			{
				var actor = node.Actor;
				var viewport = Editor.Windows.EditWin.Viewport;
				viewport.MoveViewport(actor.Transform);
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
			scenes.ToList().ForEach(x => x.BuildCSG());
			Editor.Scene.MarkSceneEdited(scenes);
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
