// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.Assertions;

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
        /// Flash main editor window to catch user attention
        /// </summary>
        public void FlashMainWindow()
        {
            MainWindow?.FlashWindow();
        }

        /// <summary>
        /// Closes all windows that are using given element to view/edit
        /// </summary>
        /// TODO: finsih this
        /*public void CloseAllEditors(ContentElement el);
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                auto win = Windows[i];
                if (win.IsEditingElement(el))
                {
                    win.Close();
                }
            }
        }
        */

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
            MainWindow = Window.Create(settings);

            // TODO: create default editor windows here

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

        /// <inheritdoc />
        public override void OnEndInit()
        {
            // Load current workspace layout
            LoadLayout(WorkingLayoutName);

            // Clear timer flag
            _lastLayoutSaveTime = DateTime.UtcNow;
        }

        /// <inheritdoc />
        public override void OnExit()
        {
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
