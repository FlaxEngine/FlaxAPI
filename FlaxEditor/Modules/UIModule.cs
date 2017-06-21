// Flax Engine scripting API

using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Docking;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages Editor UI. Especially main window UI.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class UIModule : EditorModule
    {
        private SpriteAtlas _iconsAtlas;

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

        // Cached internally to improve performance
        internal Sprite FolderClosed12;
        internal Sprite FolderOpened12;
        
        internal UIModule(Editor editor)
            : base(editor)
        {
            // Init content database after Widows module
            InitOrder = -90;
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
                    return Sprite.Invalid;
                }
                Assert.IsTrue(_iconsAtlas.IsLoaded);
            }

            // Find icon
            var result = _iconsAtlas.GetSprite(name);
            if (!result.IsValid)
            {
                // Warning
                Debug.LogWarning($"Failed to load sprite icon \'{name}\'.");
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
        /// Checks if toolstrip pause button is checked/
        /// </summary>
        /// <returns>True if toolstrip pause button is checked, otherwise false.</returns>
        public bool IsPauseButtonChecked()
        {
            if (ToolStrip != null)
                return ToolStrip.GetButton(9).Checked;
            return false;
        }

        /// <summary>
        /// Updates the toolstrip.
        /// </summary>
        public void UpdateToolstrip()
        {
            if (ToolStrip == null)
                return;

            // TODO: update all toolstrip buttons
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

        /// <inheritdoc />
        public override void OnInit()
        {
            Editor.Windows.OnMainWindowClosing += OnOnMainWindowClosing;
            var mainWindow = Editor.Windows.MainWindow.GUI;

            InitStyle(mainWindow);
            InitMainMenu(mainWindow);
            InitToolstrip(mainWindow);
            InitStatusBar(mainWindow);
            InitDockPanel(mainWindow);

            // Cache icons
            FolderClosed12 = GetIcon("FolderClosed12");
            FolderOpened12 = GetIcon("FolderOpened12");
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            _iconsAtlas = null;

            // Cleanup dock panel hint proxy windows (Flax will destroy them by auto but it's better to clear them earlier)
            DockHintWindow.Proxy.Dispsoe();
        }

        private void InitStyle(FlaxEngine.GUI.Window mainWindow)
        {
            Style style = new Style();

            // Font
            string primaryFontNameInternal = "Editor/Segoe Media Center Regular";
            var primaryFont = FlaxEngine.Content.LoadInternal<FontAsset>(primaryFontNameInternal);
            if (primaryFont)
            {
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

            // Set as default
            Style.Current = style;
            mainWindow.BackgroundColor = style.Background;
        }

        private void InitMainMenu(FlaxEngine.GUI.Window mainWindow)
        {
            MainMenu = new MainMenu();
            MainMenu.Parent = mainWindow;

            // File
            var mm_File = MainMenu.AddButton("File");
            mm_File.ContextMenu.OnButtonClicked += mm_File_Click;
            mm_File.ContextMenu.AddButton(2, "Save Scenes");
            mm_File.ContextMenu.AddButton(3, "Save All", "Ctrl+S");
            mm_File.ContextMenu.AddSeparator();
            mm_File.ContextMenu.AddButton(7, "Open Visual Studio project");
            mm_File.ContextMenu.AddButton(8, "Regenerate solution file");
            mm_File.ContextMenu.AddButton(9, "Recompile scripts");
            mm_File.ContextMenu.AddSeparator();
#if GENERATE_API
            // TODO: add API generating UI for C# editor
	        mm_File.ContextMenu.AddButton(98, "Regenerate Engine API");
	        mm_File.ContextMenu.AddButton(99, "Regenerate Editor API");
	        mm_File.ContextMenu.AddSeparator();
#endif
            mm_File.ContextMenu.AddButton(3, "Save Scene", "Ctrl+S");
            mm_File.ContextMenu.AddButton(4, "Save Scene as...");
            mm_File.ContextMenu.AddSeparator();
            mm_File.ContextMenu.AddButton(6, "Exit", "Alt+F4");

            // Edit
            var mm_Edit = MainMenu.AddButton("Edit");
            mm_Edit.ContextMenu.OnButtonClicked += mm_Edit_Click;
            mm_Edit.ContextMenu.OnVisibleChanged += mm_Edit_ShowHide;
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
            //mm_Scene.ContextMenu.AddButton(1, "Go to location...");
            //mm_scene->AddSeparator();
            mm_Scene.ContextMenu.AddButton(3, "Move actor to viewport");
            mm_Scene.ContextMenu.AddButton(4, "Align actor with viewport");
            mm_Scene.ContextMenu.AddButton(2, "Align viewport with actor");
            
            // Game
            var mm_Game = MainMenu.AddButton("Game");
            mm_Game.ContextMenu.OnButtonClicked += mm_Game_Click;
            //mm_Game.ContextMenu.AddButton(1, "Play", "F5 ???"); // TODO: finish Game menu
            //mm_Game.ContextMenu.AddButton(2, "Play in Editor");
            //mm_Game.ContextMenu.AddButton(3, "Pause");
            //mm_Game.ContextMenu.AddSeparator();
            //mm_Game.ContextMenu.AddButton(4, "Game Cooker");

            // Tools
            var mm_Tools = MainMenu.AddButton("Tools");
            mm_Tools.ContextMenu.OnButtonClicked += mm_Tools_Click;
            mm_Tools.ContextMenu.OnVisibleChanged += mm_Tools_ShowHide;
            //mm_Tools.ContextMenu.AddButton(1, "Scene statistics");
            mm_Tools.ContextMenu.AddButton(2, "Bake lightmaps", "Ctrl+F10");
            mm_Tools.ContextMenu.AddButton(3, "Clear lightmaps data");
            mm_Tools.ContextMenu.AddButton(5, "Bake all env probes");
            mm_Tools.ContextMenu.AddSeparator();
            mm_Tools.ContextMenu.AddButton(4, "Take screenshot!", "F12");
            //mm_Tools.ContextMenu.AddSeparator();
            //mm_Tools.ContextMenu.AddButton(3, "Configuration");
            //mm_Tools.ContextMenu.AddButton(4, "Options");

            // Window
            var mm_Window = MainMenu.AddButton("Window");
            mm_Window.ContextMenu.OnButtonClicked += mm_Window_Click;
            mm_Window.ContextMenu.AddButton(10, "Content");
            mm_Window.ContextMenu.AddButton(11, "Scene Tree");
            mm_Window.ContextMenu.AddButton(12, "Toolbox");
            mm_Window.ContextMenu.AddButton(13, "Properties");
            mm_Window.ContextMenu.AddButton(14, "Quality");
            mm_Window.ContextMenu.AddButton(15, "Game");
            mm_Window.ContextMenu.AddButton(16, "Editor");
            mm_Window.ContextMenu.AddButton(17, "Debug Log");
            mm_Window.ContextMenu.AddSeparator();
            mm_Window.ContextMenu.AddButton(1, "Restore default layout");

            // Help
            var mm_Help = MainMenu.AddButton("Help");
            mm_Help.ContextMenu.OnButtonClicked += mm_Help_Click;
            mm_Help.ContextMenu.AddButton(1, "Forum");
            mm_Help.ContextMenu.AddButton(2, "Documentation");
            mm_Help.ContextMenu.AddButton(3, "Report a bug");
            mm_Help.ContextMenu.AddSeparator();
            mm_Help.ContextMenu.AddButton(7, "Official Website");
            mm_Help.ContextMenu.AddButton(4, "Facebook Fanpage");
            mm_Help.ContextMenu.AddButton(5, "Youtube Channel");
            //mm_Help.ContextMenu.AddSeparator();
            //mm_Help.ContextMenu.AddButton(6, "Information about Flax");
        }

        private void InitToolstrip(FlaxEngine.GUI.Window mainWindow)
        {
            ToolStrip = new ToolStrip();
            ToolStrip.OnButtonClicked += onTootlstripButtonClicked;
            ToolStrip.Parent = mainWindow;

            // TODO: add tooltips support like in c++
            ToolStrip.AddButton(0, GetIcon("Logo32"));//.LinkTooltip(SharedToolTip, "Flax Engine");// Welcome screen
            ToolStrip.AddButton(2, GetIcon("Save32"));//.LinkTooltip(SharedToolTip, "Save all (Ctrl+S)");// Save all
            ToolStrip.AddSeparator();
            ToolStrip.AddButton(3, GetIcon("Undo32"));//.LinkTooltip(SharedToolTip, "Undo (Ctrl+Z)");// Undo
            ToolStrip.AddButton(4, GetIcon("Redo32"));//.LinkTooltip(SharedToolTip, "Redo (Ctrl+Y)");// Redo
            ToolStrip.AddSeparator();
            ToolStrip.AddButton(5, GetIcon("Translate32"));//.LinkTooltip(SharedToolTip, "Change Gizmo tool mode to Translate (1)");// Translate mode
            ToolStrip.AddButton(6, GetIcon("Rotate32"));//.LinkTooltip(SharedToolTip, "Change Gizmo tool mode to Rotate (2)");// Rotate mode
            ToolStrip.AddButton(7, GetIcon("Scale32"));//.LinkTooltip(SharedToolTip, "Change Gizmo tool mode to Scale (3)");// Scale mode
            ToolStrip.AddSeparator();
            ToolStrip.AddButton(8, GetIcon("Play32"));//.LinkTooltip(SharedToolTip, "Start/Stop simulation (F5)");// Play
            ToolStrip.AddButton(9, GetIcon("Pause32"));//.LinkTooltip(SharedToolTip, "Pause simulation");// Pause
            ToolStrip.AddButton(10, GetIcon("Step32"));//.LinkTooltip(SharedToolTip, "Step one frame in simulation");// Step

            UpdateToolstrip();
        }

        private void InitStatusBar(FlaxEngine.GUI.Window mainWindow)
        {
            // Status Bar
            StatusBar = new StatusBar();
            StatusBar.Parent = mainWindow;

            StatusBar.Text = "Ready";
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
                /*case 0:
                    // TODO: Welcome screen
                    break;

                // Save scene(s)
                case 1: CSceneModule->SaveScenes(); break;

                // Save all
                case 2: CEditor->SaveAll(); break;

                // Undo
                case 3: CEditor->UndoRedo.Undo(); break;

                // Redo
                case 4: CEditor->UndoRedo.Redo(); break;

                // Translate mode
                case 5: CEditor->GetMainGizmo()->SetMode(GizmoMode::Translate); break;

                // Rotate mode
                case 6: CEditor->GetMainGizmo()->SetMode(GizmoMode::Rotate); break;

                // Scale mode
                case 7: CEditor->GetMainGizmo()->SetMode(GizmoMode::Scale); break;

                // Play
                case 8:
                {
                    // Check if Editor is in play mode
                    if (CEditor->StateMachine->IsPlayMode())
                    {
                        // Stop game
                        CSimulationModule->RequestStopPlay();
                    }
                    else
                    {
                        // Start playing (will validate state)
                        CSimulationModule->RequestStartPlay();
                    }
                }
                    break;

                // Pause
                case 9:
                {
                    // Check if Editor is in pause state
                    if (CEditor->StateMachine->PlayingState.IsPaused())
                    {
                        // Resume game
                        CSimulationModule->RequestResumePlay();
                    }
                    else
                    {
                        // Pause game
                        CSimulationModule->RequestPausePlay();
                    }
                }
                    break;

                // Step
                case 10: CSimulationModule->RequestPlayOneFrame(); break;*/
            }
        }

        private void mm_File_Click(int id, ContextMenu cm)
        {
            switch (id)
            {
                // Save Scenes
                case 2: Editor.Scene.SaveScenes(); break;

                // Save All
                case 3: Editor.SaveAll(); break;

                // Exit
                case 6: Editor.Windows.MainWindow.Close(ClosingReason.User); break;

                // Open Visual Studio project
                //case 7: CodeEditingManager::Instance()->OpenSolution(); break;

                // Regenerate solution file
                //case 8: ScriptsBuilder::Instance()->GenerateProject(true, true); break;

                // Recompile scripts
                //case 9: ScriptsBuilder::Instance()->Compile(); break;
            }
        }

        private void mm_Edit_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                //case 1: CEditor->UndoRedo.Undo(); break;
                //case 2: CEditor->UndoRedo.Redo(); break;
                //case 3: CEditor->GetMainGizmo()->Cut(); break;
                //case 4: CEditor->GetMainGizmo()->CopySelection(); break;
                //case 5: CEditor->GetMainGizmo()->Paste(); break;
                //case 6: CEditor->GetMainGizmo()->DeleteSelection(); break;
                //case 7: CEditor->GetMainGizmo()->Duplicate(); break;
                //case 8: CEditor->GetMainGizmo()->SelectAll(); break;
                //case 9: CWindowsModule->SceneGraph->Search(); break;
            }
        }

        private void mm_Edit_ShowHide(Control c)
        {
            if (c.Visible == false)
                return;

            /*auto & undoRedo = CEditor->UndoRedo;
            auto gizmo = CEditor->GetMainGizmo();
            auto c = (CContextMenu*)cm;

            auto undoButton = c->GetButton(1);// Undo
            undoButton->SetEnabled(undoRedo.HasUndo());
            undoButton->Text = undoRedo.HasUndo() ? LocalizationData::FormatEditorMessage(93, undoRedo.GetFirstUndo()->Get)) : LocalizationData::GetEditorMessage(95);

            auto redoButton = c->GetButton(2);// Redo
            redoButton->SetEnabled(undoRedo.HasRedo());
            redoButton->Text = undoRedo.HasRedo() ? LocalizationData::FormatEditorMessage(94, undoRedo.GetFirstRedo()->Get)) : LocalizationData::GetEditorMessage(96);

            c->GetButton(3)->SetEnabled(gizmo->HasSthSelected());// Cut
            c->GetButton(4)->SetEnabled(gizmo->HasSthSelected());// Copy
            c->GetButton(6)->SetEnabled(gizmo->HasSthSelected());// Delete
            c->GetButton(7)->SetEnabled(gizmo->HasSthSelected());// Duplicate
            c->GetButton(8)->SetEnabled(SceneManager::Instance()->IsAnySceneLoaded());// Select All

            c->PerformLayout();*/
        }

        private void mm_Scene_Click(int id, ContextMenuBase cm)
        {
            /*auto& undoRedo = CEditor->UndoRedo;
            auto gizmo = CEditor->GetMainGizmo();

            switch (id)
            {
                // Got to location...
                case 1: break;

                // Align viewport with actor
                case 2:
                {
                    if (gizmo->HasSthSelected())
                    {
                        auto actor = gizmo->GetSelection()->At(0);
                        auto viewport = CWindowsModule->Frame->GetViewport();
                        viewport->MoveViewport(actor->GetTransform());
                    }
                    
                    break;
                }

                // Move actor to viewport
                case 3:
                {
                    if (gizmo->HasSthSelected())
                    {
                        auto actor = gizmo->GetSelection()->At(0);
                        auto viewport = CWindowsModule->Frame->GetViewport();
                        Array<Transform> before(1);
                        before.Add(actor->GetTransform());
                        actor->SetPosition(viewport->GetViewPosition());

                        if (!undoRedo.IsDisabled())
                            undoRedo.AddAction(new TransformActors(gizmo, before));
                    }
                    
                    break;
                } 

                // Align actor with viewport
                case 4:
                {
                    if (gizmo->HasSthSelected())
                    {
                        auto actor = gizmo->GetSelection()->At(0);
                        auto viewport = CWindowsModule->Frame->GetViewport();
                        Array<Transform> before(1);
                        before.Add(actor->GetTransform());
                        Transform transform;
                        transform.Translation = viewport->GetViewPosition();
                        transform.Orientation = viewport->GetViewOrientation();
                        transform.Scale = actor->GetScale();
                        actor->SetTransform(transform);

                        if (!undoRedo.IsDisabled())
                            undoRedo.AddAction(new TransformActors(gizmo, before));
                    }

                    break;
                }
            }*/
        }

        private void mm_Game_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                // 
                case 1: break;
            }
        }

        private void mm_Tools_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                /*// Scene statistics
                case 1: break;

                // Bake lightmaps
                case 2: bakeOrCancelLightmaps(); break;

                // Clear lightmaps data
                case 3: CEditor->clearStaticLighting(); break;

                // Take screenshot!
                case 4: TakeScreenshot(); break;

                // Bake all env probes
                case 5:
                {
                    Function<bool, Actor*> f([](Actor* actor) -> bool
            
                    {
                        auto envProbe = dynamic_cast<EnvironmentProbe*>(actor);
                        if (envProbe)
                        {
                            envProbe->Bake();
                            CEditor->MarkAsEdited();
                        }
                        return actor->IsActiveInTree();
                    });
                    SceneQuery::TreeExecute(f);
                    
                    break;
                }*/
            }
        }

        private void mm_Tools_ShowHide(Control c)
        {
            if (c.Visible == false)
                return;

            /*auto c = (CContextMenu*)cm;

            bool canBakeLightmaps = CEditor->Device->Limits->IsComputeSupported();
            bool canEdit = SceneManager::Instance()->IsAnySceneLoaded() && CEditor->GetCurrentStateType() == EditorStates::EditingScene;
            bool isBakingLightmaps = ProgressManager::Instance()->BakeLightmaps.IsActive();
            c->GetButton(2)->SetEnabled((canEdit && canBakeLightmaps) || isBakingLightmaps);// Bake lightmaps
            c->GetButton(2)->Text = isBakingLightmaps ? "Cancel baking lightmaps") : "Bake lightmaps");
            c->GetButton(3)->SetEnabled(canEdit);// Clear lightmaps data
            c->GetButton(5)->SetEnabled(canEdit);// Bake all env probes

            c->PerformLayout();*/
        }

        private void mm_Window_Click(int id, ContextMenuBase cm)
        {
            switch (id)
            {
                // Restore default layout
                case 1: Editor.Windows.LoadDefaultLayout(); break;
                
                // Windows
                case 10: Editor.Windows.ContentWin.FocusOrShow(); break;
                //case 11: Editor.Windows.SceneTreeWin.FocusOrShow(); break;
                case 12: Editor.Windows.ToolboxWin.FocusOrShow(); break;
                //case 13: Editor.Windows.PropertiesWin.FocusOrShow(); break;
                //case 14: Editor.Windows.QualityWin.FocusOrShow(); break;
                //case 15: Editor.Windows.GameWin.FocusOrShow(); break;
                //case 16: Editor.Windows.EditorWin.FocusOrShow(); break;
                //case 17: Editor.Windows.DebugWin.FocusOrShow(); break;
            }
        }

        void mm_Help_Click(int id, ContextMenuBase cm)
        {
            /*switch (id)
            {
                // Forum
                case 1: Application::StartProcess("http://answers.flaxengine.com/")); break;

                // Documentation
                case 2: Application::StartProcess("http://docs.flaxengine.com/")); break;

                // Report a bug
                case 3:
                    MISSING_CODE("report a bug feature");
                    //Application::StartProcess("http://celelej.com/documentation/report-a-bug/"));
                    break;

                // Facebook Fanpage
                case 4: Application::StartProcess("https://facebook.com/FlaxEngine")); break;

                // Youtube Channel
                case 5: Application::StartProcess("https://youtube.com/Celelej")); break;

                // Informations about Flax
                case 6: MISSING_EDITOR_FEATURE("Show info window"); break;

                // Official Website
                case 7: Application::StartProcess("http://flaxengine.com")); break;
            }*/
        }

        private void OnOnMainWindowClosing()
        {
            // Clear UI references (GUI cannot be used after window closing)
            MainMenu = null;
            ToolStrip = null;
            MasterPanel = null;
            StatusBar = null;
        }
    }
}
