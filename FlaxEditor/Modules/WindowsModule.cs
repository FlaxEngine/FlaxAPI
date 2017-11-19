// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI.Docking;
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
        private DateTime _lastLayoutSaveTime;
        private float _projectIconScreenshotTimeout = -1;
        private string _windowsLayoutPath;

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
        /// The game cooker window.
        /// </summary>
        public GameCookerWindow GameCookerWin;

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
            SaveLayout(_windowsLayoutPath);
        }

        /// <summary>
        /// Loads the default workspace layout for the current editor version.
        /// </summary>
        public void LoadDefaultLayout()
        {
            LoadLayout(StringUtils.CombinePaths(Globals.EditorFolder, "DefaultLayout.xml"));
        }

        /// <summary>
        /// Loads the layout from the file.
        /// </summary>
        /// <param name="path">The layout file path.</param>
        public void LoadLayout(string path)
        {
            if (Editor.IsHeadlessMode)
                return;

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

            GameCookerWin.Show(FlaxEngine.GUI.Docking.DockState.DockRight, ContentWin);
            GameCookerWin.SelectTab();
        }

        private void SavePanel(XmlWriter writer, DockPanel panel)
        {
            writer.WriteAttributeString("SelectedTab", panel.SelectedTabIndex.ToString());
            writer.WriteAttributeString("Tabs", panel.TabsCount.ToString());
            writer.WriteAttributeString("ChildPanels", panel.ChildPanelsCount.ToString());

            for (int i = 0; i < panel.TabsCount; i++)
            {
                var win = panel.Tabs[i];
                writer.WriteStartElement("Window");

                writer.WriteAttributeString("Typename", win.SerializationTypename);

                writer.WriteEndElement();
            }

            for (int i = 0; i < panel.ChildPanelsCount; i++)
            {
                var p = panel.ChildPanels[i];

                // Skip empty panels
                if (p.TabsCount == 0)
                    continue;

                writer.WriteStartElement("Panel");

                float splitterValue;
                DockState state = p.TryGetDockState(out splitterValue);

                writer.WriteAttributeString("DockState", ((int)state).ToString());
                writer.WriteAttributeString("SplitterValue", splitterValue.ToString(CultureInfo.InvariantCulture));

                SavePanel(writer, p);

                writer.WriteEndElement();
            }
        }

        private static void SaveBounds(XmlWriter writer, Window win)
        {
            var bounds = win.ClientBounds;

            writer.WriteElementString("X", bounds.X.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Y", bounds.Y.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Width", bounds.Width.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Height", bounds.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Saves the layout to the file.
        /// </summary>
        /// <param name="path">The layout file path.</param>
        public void SaveLayout(string path)
        {
            if (Editor.IsHeadlessMode)
                return;

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
            };

            var masterPanel = Editor.UI.MasterPanel;

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DockPanelLayout");

                // Metadata
                writer.WriteAttributeString("Version", "4");
                writer.WriteAttributeString("WindowsCount", masterPanel.Windows.Count.ToString());
                writer.WriteAttributeString("FloatingPanelsCount", masterPanel.FloatingPanels.Count.ToString());

                // Main window info
                if (MainWindow)
                {
                    writer.WriteStartElement("MainWindow");

                    SaveBounds(writer, MainWindow);

                    writer.WriteEndElement();
                }

                // Master panel structure
                writer.WriteStartElement("MasterPanel");
                SavePanel(writer, masterPanel);
                writer.WriteEndElement();

                // Save all floating windows structure
                for (int i = 0; i < masterPanel.FloatingPanels.Count; i++)
                {
                    var panel = masterPanel.FloatingPanels[i];
                    var window = panel.Window;

                    if (window == null)
                    {
                        Editor.LogWarning("Floating panel has missing window");
                        continue;
                    }

                    writer.WriteStartElement("Float");

                    SaveBounds(writer, window.NativeWindow);
                    SavePanel(writer, panel);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Assert.IsNull(MainWindow);

            _windowsLayoutPath = StringUtils.CombinePaths(Globals.ProjectCacheFolder, "WindowsLayout.xml");

            // Create main window
            var settings = CreateWindowSettings.Default;
            settings.Title = "Flax Editor";
            settings.Size = new Vector2(1300, 900);
            settings.StartPosition = WindowStartPosition.CenterScreen;
            MainWindow = Window.Create(settings);
            if (MainWindow == null)
            {
                // Error
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
            GameCookerWin = new GameCookerWindow(Editor);

            // Bind events
            SceneManager.OnSceneSaveError += OnSceneSaveError;
            SceneManager.OnSceneLoaded += OnSceneLoaded;
            SceneManager.OnSceneLoadError += OnSceneLoadError;
            SceneManager.OnSceneLoading += OnSceneLoading;
            SceneManager.OnSceneSaved += OnSceneSaved;
            SceneManager.OnSceneSaving += OnSceneSaving;
            SceneManager.OnSceneUnloaded += OnSceneUnloaded;
            SceneManager.OnSceneUnloading += OnSceneUnloading;
        }

        private void MainWindow_OnClosing(ClosingReason reason, ref bool cancel)
        {
            Editor.Log("Main window is closing, reason: " + reason);

            if (Editor.StateMachine.IsPlayMode)
            {
                // Cancel closing buut leave the play mode
                cancel = true;
                Editor.Log("Skip closing ediotr and leave the play mode");
                Editor.Simulation.RequestStopPlay();
                return;
            }

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
                    if (Windows[i] is AssetEditorWindow assetEditorWindow)
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

            // Capture project icon screenshot (not in play mode and if editor was used for some time)
            if (!Editor.StateMachine.IsPlayMode && Time.TimeSinceStartup >= 5.0f)
            {
                Editor.Log("Capture project icon screenshot");
                _projectIconScreenshotTimeout = Time.TimeSinceStartup + 0.8f; // wait 800ms for a screenshot task
                EditWin.Viewport.SaveProjectIcon();
            }
            else
            {
                // Close editor
                Application.Exit();
            }
        }

        /// <inheritdoc />
        public override void OnEndInit()
        {
            UpdateWindowTitle();

            // Initialize windows
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnInit();

            // Load current workspace layout
            LoadLayout(_windowsLayoutPath);

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

            // Auto close on project icon saving end
            if (_projectIconScreenshotTimeout > 0 && Time.TimeSinceStartup > _projectIconScreenshotTimeout)
            {
                Editor.Log("Closing Editor after project icon screenshot");
                EditWin.Viewport.SaveProjectIconEnd();
                Application.Exit();
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

            // Close all windows
            var windows = Window.Windows.ToArray();
            for (int i = 0; i < windows.Length; i++)
            {
                if (windows[i].IsVisible)
                    windows[i].Close(ClosingReason.EngineExit);
            }
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

        /// <inheritdoc />
        public override void OnPlayBegin()
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnPlayBegin();
        }

        /// <inheritdoc />
        public override void OnPlayEnd()
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].OnPlayEnd();
        }

        #endregion
    }
}
