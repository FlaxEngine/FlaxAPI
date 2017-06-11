// Flax Engine scripting API

using FlaxEditor.GUI;
using FlaxEngine;
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
        /// <summary>
        /// The main menu control.
        /// </summary>
        public MainMenu MainMenu;

        /// <summary>
        /// The master dock panel for all Editor windows.
        /// </summary>
        public MasterDockPanel MasterPanel = new MasterDockPanel();

        internal UIModule(Editor editor)
            : base(editor)
        {
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Editor.Windows.OnMainWindowClosing += OnOnMainWindowClosing;
            var mainWindow = Editor.Windows.MainWindow.GUI;

            InitMainMenu(mainWindow);
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
            mm_Edit.ContextMenu.OnVisibilityChanged += mm_Edit_ShowHide;
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
#if BUILD == BUILD_DEBUG
            mm_Scene.ContextMenu.AddButton(10000, "[DEBUG] Select view camera");
#endif

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
            mm_Tools.ContextMenu.OnVisibilityChanged += mm_Tools_ShowHide;
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
            mm_Window.ContextMenu.AddButton(13, "Scene Properties");
            mm_Window.ContextMenu.AddButton(10, "Quality");
            mm_Window.ContextMenu.AddButton(11, "Game");
            mm_Window.ContextMenu.AddButton(12, "Editor");
            mm_Window.ContextMenu.AddButton(14, "Debug Log");
            mm_Window.ContextMenu.AddSeparator();
#if BUILD == BUILD_DEBUG
            mm_Window.ContextMenu.AddButton(10000, "Show test C# window");
            mm_Window.ContextMenu.AddSeparator();
#endif
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

        private void mm_Edit_ShowHide(ContextMenuBase cm)
        {
            if (cm.Visible == false)
                return;

            /*auto & undoRedo = CEditor->UndoRedo;
            auto gizmo = CEditor->GetMainGizmo();
            auto c = (CContextMenu*)cm;

            auto undoButton = c->GetButton(1);// Undo
            undoButton->SetEnabled(undoRedo.HasUndo());
            undoButton->Text = undoRedo.HasUndo() ? LocalizationData::FormatEditorMessage(93, undoRedo.GetFirstUndo()->GetText()) : LocalizationData::GetEditorMessage(95);

            auto redoButton = c->GetButton(2);// Redo
            redoButton->SetEnabled(undoRedo.HasRedo());
            redoButton->Text = undoRedo.HasRedo() ? LocalizationData::FormatEditorMessage(94, undoRedo.GetFirstRedo()->GetText()) : LocalizationData::GetEditorMessage(96);

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

        private void mm_Tools_ShowHide(ContextMenuBase cm)
        {
            if (cm.Visible == false)
                return;

            /*auto c = (CContextMenu*)cm;

            bool canBakeLightmaps = CEditor->Device->Limits->IsComputeSupported();
            bool canEdit = SceneManager::Instance()->IsAnySceneLoaded() && CEditor->GetCurrentStateType() == EditorStates::EditingScene;
            bool isBakingLightmaps = ProgressManager::Instance()->BakeLightmaps.IsActive();
            c->GetButton(2)->SetEnabled((canEdit && canBakeLightmaps) || isBakingLightmaps);// Bake lightmaps
            c->GetButton(2)->Text = isBakingLightmaps ? TEXT("Cancel baking lightmaps") : TEXT("Bake lightmaps");
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

                /*case 10: Editor.Windows.QualityWin->FocusOrShow(); break;
                case 11: Editor.Windows.GameWin->FocusOrShow(); break;
                case 12: Editor.Windows.Frame->FocusOrShow(); break;
                case 13: CWindowsModule.ScenePropertiesWin->FocusOrShow(); break;
                case 14: Editor.Windows.DebugLogWin->FocusOrShow(); break;*/
            }
        }

        void mm_Help_Click(int id, ContextMenuBase cm)
        {
            /*switch (id)
            {
                // Forum
                case 1: Application::StartProcess(TEXT("http://answers.flaxengine.com/")); break;

                // Documentation
                case 2: Application::StartProcess(TEXT("http://docs.flaxengine.com/")); break;

                // Report a bug
                case 3:
                    MISSING_CODE("report a bug feature");
                    //Application::StartProcess(TEXT("http://celelej.com/documentation/report-a-bug/"));
                    break;

                // Facebook Fanpage
                case 4: Application::StartProcess(TEXT("https://facebook.com/FlaxEngine")); break;

                // Youtube Channel
                case 5: Application::StartProcess(TEXT("https://youtube.com/Celelej")); break;

                // Informations about Flax
                case 6: MISSING_EDITOR_FEATURE("Show info window"); break;

                // Official Website
                case 7: Application::StartProcess(TEXT("http://flaxengine.com")); break;
            }*/
        }

        private void OnOnMainWindowClosing()
        {
            // Clear UI references (GUI cannot be used after window closing)
            MainMenu = null;
            MasterPanel = null;
        }
    }
}
