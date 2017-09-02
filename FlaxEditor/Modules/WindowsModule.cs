// Flax Engine scripting API

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.Rendering;
using FlaxEngine.Utilities;
using Window = FlaxEngine.Window;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages Editor windows and popups.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class WindowsModule : EditorModule
    {
        /// <summary>
        /// The default workspace layout name.
        /// </summary>
        public static string DefaultLayoutName = "Default";

        /// <summary>
        /// The working workspace layout name.
        /// </summary>
        public static string WorkingLayoutName = "Current";

        private DateTime _lastLayoutSaveTime;

        /// <summary>
        /// The main editor window.
        /// </summary>
        public Window MainWindow { get; private set; }

        /// <summary>
        /// Occurs when main editor window is being closed.
        /// </summary>
        public event Action OnMainWindowClosing;

        /// <summary>
        /// The content window.
        /// </summary>
        public ContentWindow ContentWin;

        /// <summary>
        /// The edit game window.
        /// </summary>
        public EditGameWindow EditWin;

        /// <summary>
        /// The game window.
        /// </summary>
        public GameWindow GameWin;

        /// <summary>
        /// The properties window.
        /// </summary>
        public PropertiesWindow PropertiesWin;

        /// <summary>
        /// The scene tree window.
        /// </summary>
        public SceneTreeWindow SceneWin;

        /// <summary>
        /// The debug log window.
        /// </summary>
        public DebugLogWindow DebugWin;

        /// <summary>
        /// The toolbox window.
        /// </summary>
        public ToolboxWindow ToolboxWin;

        /// <summary>
        /// List with all created editor windows.
        /// </summary>
        public readonly List<EditorWindow> Windows = new List<EditorWindow>(32);

        internal WindowsModule(Editor editor)
            : base(editor)
        {
            // Init windows module first
            InitOrder = -100;
        }

        /// <summary>
        /// Takes the screenshot of the current viewport.
        /// </summary>
        public void TakeScreenshot()
        {
            // Select task
            SceneRenderTask target = null;
            if (Editor.Windows.EditWin.IsSelected)
            {
                // Use editor window
                target = EditWin.Viewport.Task;
            }
            else
            {
                // Use game window
                GameWin.FocusOrShow();
            }

            // Fire screenshot taking
            Screenshot.Capture(target);
        }

        /// <summary>
        /// Updates the main window title.
        /// </summary>
        public void UpdateWindowTitle()
        {
            var mainWindow = MainWindow;
            if (mainWindow)
            {
                var projectPath = Globals.ProjectFolder.Replace('/', '\\');
                var platformBit = Application.Is64bitApp ? "64" : "32";
                var title = string.Format("Flax Editor - \'{0}\' ({1}-bit)", projectPath, platformBit);
                mainWindow.Title = title;
            }
        }

        /// <summary>
        /// Flash main editor window to catch user attention
        /// </summary>
        public void FlashMainWindow()
        {
            MainWindow?.FlashWindow();
        }

        /// <summary>
        /// Finds the first window that is using given element to view/edit it.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Editor window or null if cannot find any window.</returns>
        public EditorWindow FindEditor(ContentItem item)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                var win = Windows[i];
                if (win.IsEditingItem(item))
                {
                    return win;
                }
            }

            return null;
        }

        /// <summary>
        /// Closes all windows that are using given element to view/edit it.
        /// </summary>
        /// <param name="item">The item.</param>
        public void CloseAllEditors(ContentItem item)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                var win = Windows[i];
                if (win.IsEditingItem(item))
                {
                    win.Close();
                    i--;
                }
            }
        }

        /// <summary>
        /// Saves the current workspace layout.
        /// </summary>
        public void SaveCurrentLayout()
        {
            _lastLayoutSaveTime = DateTime.UtcNow;
            SaveLayout(WorkingLayoutName);
        }

        /// <summary>
        /// Loads the default workspace layout.
        /// </summary>
        public void LoadDefaultLayout()
        {
            LoadLayout(DefaultLayoutName);
        }

        /// <summary>
        /// Loads the layout.
        /// </summary>
        /// <param name="name">The layout name.</param>
        public void LoadLayout(string name)
        {
            // TODO: finish this

            // for now just show default windows
            ContentWin.Show(FlaxEngine.GUI.Docking.DockState.DockBottom);
            DebugWin.Show(FlaxEngine.GUI.Docking.DockState.DockFill, ContentWin);
            ContentWin.SelectTab();
            ((FlaxEngine.GUI.SplitPanel)ContentWin.Parent.Parent.Parent.Parent).SplitterValue = 0.5f;
            SceneWin.Show(FlaxEngine.GUI.Docking.DockState.DockLeft);
            PropertiesWin.Show(FlaxEngine.GUI.Docking.DockState.DockRight);
            ToolboxWin.Show(FlaxEngine.GUI.Docking.DockState.DockTop, PropertiesWin);
            EditWin.Show(FlaxEngine.GUI.Docking.DockState.DockFill);
            GameWin.Show(FlaxEngine.GUI.Docking.DockState.DockFill);
            EditWin.SelectTab();
        }

        /// <summary>
        /// Saves the layout.
        /// </summary>
        /// <param name="name">The layout name.</param>
        public void SaveLayout(string name)
        {
            // TODO: finish this
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Assert.IsNull(MainWindow);
            
            // Create main window
            var settings = CreateWindowSettings.Default;
            settings.Title = "Flax Editor";
            settings.Size = new Vector2(1300, 900);
            settings.StartPosition = WindowStartPosition.CenterScreen;
            MainWindow = Window.Create(settings);
            if (MainWindow == null)
            {
                // Error
                // TODO: make it fatal error
                Debug.LogError("Failed to create editor main window!");
                return;
            }
            UpdateWindowTitle();

            // Link for main window events
            MainWindow.OnClosing += MainWindow_OnClosing;
            MainWindow.OnClosed += MainWindow_OnClosed;

            // Create default editor windows
            ContentWin = new ContentWindow(Editor);
            EditWin = new EditGameWindow(Editor);
            GameWin = new GameWindow(Editor);
            PropertiesWin = new PropertiesWindow(Editor);
            SceneWin = new SceneTreeWindow(Editor);
            DebugWin = new DebugLogWindow(Editor);
            ToolboxWin = new ToolboxWindow(Editor);

            // Bind events
            SceneManager.OnSceneSaveError += OnSceneSaveError;
            SceneManager.OnSceneLoaded += OnSceneLoaded;
            SceneManager.OnSceneLoadError += OnSceneLoadError;
            SceneManager.OnSceneLoading += OnSceneLoading;
            SceneManager.OnSceneSaved += OnSceneSaved;
            SceneManager.OnSceneSaving += OnSceneSaving;
            SceneManager.OnSceneUnloaded += OnSceneUnloaded;
            SceneManager.OnSceneUnloading += OnSceneUnloading;

            // TODO: link for OnScriptsReloadStart/OnScriptsReloadEnd events and don't fire scene events on scripts reload?
        }

        private void MainWindow_OnClosing(ClosingReason reason, ref bool cancel)
        {
            Editor.Log("Main window is closing, reason: " + reason);

            SaveCurrentLayout();
            
            // Block closing only on user events
            if (reason == ClosingReason.User)
            {
                // Check if cancel action or save scene before exit
                if (Editor.Scene.CheckSaveBeforeClose())
                {
                    // Cancel
                    cancel = true;
                    return;
                }

                // Close all asset editor windows
                for (int i = 0; i < Windows.Count; i++)
                {
                    var assetEditorWindow = Windows[i] as AssetEditorWindow;
                    if (assetEditorWindow != null)
                    {
                        if (assetEditorWindow.Close(ClosingReason.User))
                        {
                            // Cancel
                            cancel = true;
                            return;
                        }

                        // Remove it
                        Windows.Remove(assetEditorWindow);
                        i--;
                    }
                }
            }

            OnMainWindowClosing?.Invoke();
        }

        private void MainWindow_OnClosed()
        {
            Editor.Log("Main window is closed");
            MainWindow = null;

            // TODO: capture project icon screenshot before exit (like in c++ editor)
            
            Application.Exit();
        }

        /// <inheritdoc />
        public override void OnEndInit()
        {
            UpdateWindowTitle();

            // Initialize windows
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnInit();

            // Load current workspace layout
            LoadLayout(WorkingLayoutName);

            // Clear timer flag
            _lastLayoutSaveTime = DateTime.UtcNow;
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            // Auto save workspace layout every few seconds
            var now = DateTime.UtcNow;
            if (_lastLayoutSaveTime.Ticks > 10 && now - _lastLayoutSaveTime >= TimeSpan.FromSeconds(5))
            {
                SaveCurrentLayout();
            }
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // Shutdown windows
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnExit();

            // Unbind events
            SceneManager.OnSceneSaveError -= OnSceneSaveError;
            SceneManager.OnSceneLoaded -= OnSceneLoaded;
            SceneManager.OnSceneLoadError -= OnSceneLoadError;
            SceneManager.OnSceneLoading -= OnSceneLoading;
            SceneManager.OnSceneSaved -= OnSceneSaved;
            SceneManager.OnSceneSaving -= OnSceneSaving;
            SceneManager.OnSceneUnloaded -= OnSceneUnloaded;
            SceneManager.OnSceneUnloading -= OnSceneUnloading;

            // Close main window
            MainWindow?.Close(ClosingReason.EngineExit);
            MainWindow = null;
        }

        #region Window Events

        private void OnSceneSaveError(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneSaveError(scene, sceneId);
        }

        private void OnSceneLoaded(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneLoaded(scene, sceneId);
        }

        private void OnSceneLoadError(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneLoadError(scene, sceneId);
        }

        private void OnSceneLoading(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneLoading(scene, sceneId);
        }

        private void OnSceneSaved(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneSaved(scene, sceneId);
        }

        private void OnSceneSaving(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneSaving(scene, sceneId);
        }

        private void OnSceneUnloaded(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneUnloaded(scene, sceneId);
        }

        private void OnSceneUnloading(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnSceneUnloading(scene, sceneId);
        }

        #endregion
    }
}
