////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FlaxEditor.Content.Import;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Modules;
using FlaxEditor.Scripting;
using FlaxEditor.States;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.Json;

namespace FlaxEditor
{
    public sealed partial class Editor
    {
        /// <summary>
        /// Gets the Editor instance.
        /// </summary>
        /// <value>
        /// The Editor instance.
        /// </value>
        public static Editor Instance { get; private set; }

        static Editor()
        {
            JsonSerializer.Settings.Converters.Add(new SceneTreeNodeConverter());
        }

        private readonly List<EditorModule> _modules = new List<EditorModule>(16);
        private bool _isAfterInit;

        /// <summary>
        /// Gets a value indicating whether Flax Engine is the best in the world.
        /// </summary>
        public bool IsFlaxEngineTheBest => true;

        /// <summary>
        /// Gets a value indicating whether this Editor is running a dev instance of the engine.
        /// </summary>
        internal bool IsDevInstance => true;

        /// <summary>
        /// The windows module.
        /// </summary>
        public readonly WindowsModule Windows;

        /// <summary>
        /// The UI module.
        /// </summary>
        public readonly UIModule UI;

        /// <summary>
        /// The thumbnails module.
        /// </summary>
        public readonly ThumbnailsModule Thumbnails;

        /// <summary>
        /// The simulation module.
        /// </summary>
        public readonly SimulationModule Simulation;

        /// <summary>
        /// The scene module.
        /// </summary>
        public readonly SceneModule Scene;

        /// <summary>
        /// The scene editing module.
        /// </summary>
        public readonly SceneEditingModule SceneEditing;

        /// <summary>
        /// The progress reporting module.
        /// </summary>
        public readonly ProgressReportingModule ProgressReporting;

        /// <summary>
        /// The content editing module.
        /// </summary>
        public readonly ContentEditingModule ContentEditing;

        /// <summary>
        /// The content database module.
        /// </summary>
        public readonly ContentDatabaseModule ContentDatabase;

        /// <summary>
        /// The content importing module.
        /// </summary>
        public readonly ContentImportingModule ContentImporting;

        /// <summary>
        /// The content editing
        /// </summary>
        public readonly CodeEditingModule CodeEditing;

        /// <summary>
        /// The editor state machine.
        /// </summary>
        public readonly EditorStateMachine StateMachine;

        /// <summary>
        /// The undo/redo
        /// </summary>
        public readonly EditorUndo Undo;

        /// <summary>
        /// Gets the main transform gizmo used by the <see cref="SceneEditorWindow"/>.
        /// </summary>
        /// <value>
        /// The main transform gizmo.
        /// </value>
        public Gizmo.TransformGizmo MainTransformGizmo => Windows.EditWin.Viewport.TransformGizmo;

        internal Editor()
        {
            Instance = this;

            Log("Setting up C# Editor...");

            // Create common editor modules
            RegisterModule(Windows = new WindowsModule(this));
            RegisterModule(UI = new UIModule(this));
            RegisterModule(Thumbnails = new ThumbnailsModule(this));
            RegisterModule(Simulation = new SimulationModule(this));
            RegisterModule(Scene = new SceneModule(this));
            RegisterModule(SceneEditing = new SceneEditingModule(this));
            RegisterModule(ContentEditing = new ContentEditingModule(this));
            RegisterModule(ContentDatabase = new ContentDatabaseModule(this));
            RegisterModule(ContentImporting = new ContentImportingModule(this));
            RegisterModule(CodeEditing = new CodeEditingModule(this));
            RegisterModule(ProgressReporting = new ProgressReportingModule(this));

            StateMachine = new EditorStateMachine(this);
            Undo = new EditorUndo(this);

            ScriptsBuilder.ScriptsReloadBegin += ScriptsBuilder_ScriptsReloadBegin;
            ScriptsBuilder.ScriptsReloadEnd += ScriptsBuilder_ScriptsReloadEnd;
        }

        private void ScriptsBuilder_ScriptsReloadBegin()
        {
            EnsureState<EditingSceneState>();
            StateMachine.GoToState<ReloadingScriptsState>();
        }

        private void ScriptsBuilder_ScriptsReloadEnd()
        {
            EnsureState<ReloadingScriptsState>();
            StateMachine.GoToState<EditingSceneState>();
        }

        internal void RegisterModule(EditorModule module)
        {
            Log("Register Editor module " + module);

            _modules.Add(module);
            if (_isAfterInit)
                _modules.Sort((a, b) => a.InitOrder - b.InitOrder);
        }

        internal void Init()
        {
            EnsureState<LoadingState>();
            Log("Editor init");

            // Note: we don't sort modules before Init (optimized)
            _modules.Sort((a, b) => a.InitOrder - b.InitOrder);
            _isAfterInit = true;

            // Initialize modules (from front to back)
            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].OnInit();
            }

            // Start Editor initalization ending phrase (will wait for scripts compilation result)
            StateMachine.LoadingState.StartInitEnding();
        }

        internal void EndInit()
        {
            EnsureState<LoadingState>();
            Log("Editor end init");

            // Change state
            StateMachine.GoToState<EditingSceneState>();

            // Initialize modules (from front to back)
            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].OnEndInit();
            }

            // Close splash and show main window
            CloseSplashScreen();
            Assert.IsNotNull(Windows.MainWindow);
            Windows.MainWindow.Show();
            Windows.MainWindow.Focus();
        }

        internal void Update()
        {
            try
            {
                // Update state machine
                StateMachine.Update();

                // Update modules
                for (int i = 0; i < _modules.Count; i++)
                {
                    _modules[i].OnUpdate();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        internal void OnPlayBegin()
        {
            for (int i = 0; i < _modules.Count; i++)
                _modules[i].OnPlayBegin();
        }

        internal void OnPlayEnd()
        {
            for (int i = 0; i < _modules.Count; i++)
                _modules[i].OnPlayEnd();
        }

        internal void Exit()
        {
            Log("Editor exit");

            // Start exit
            StateMachine.GoToState<ClosingState>();

            Scene.ClearRefsToSceneObjects();

            // Release modules (from back to front)
            for (int i = _modules.Count - 1; i >= 0; i--)
            {
                _modules[i].OnExit();
            }

            // Cleanup
            Undo.Dispose();
            Instance = null;

            ScriptsBuilder.ScriptsReloadBegin -= ScriptsBuilder_ScriptsReloadBegin;
            ScriptsBuilder.ScriptsReloadEnd -= ScriptsBuilder_ScriptsReloadEnd;
        }

        /// <summary>
        /// Undo last action.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PerformUndo()
        {
            Undo.PerformUndo();
        }

        /// <summary>
        /// Redo last action.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PerformRedo()
        {
            Undo.PerformRedo();
        }

        /// <summary>
        /// Saves all changes (scenes, assets, etc.).
        /// </summary>
        public void SaveAll()
        {
            // Layout
            Windows.SaveCurrentLayout();

            // Scenes
            Scene.SaveScenes();

            // Assets
            for (int i = 0; i < Windows.Windows.Count; i++)
            {
                if (Windows.Windows[i] is AssetEditorWindow win)
                {
                    win.Save();
                }
            }
        }

        /// <summary>
        /// Ensure that editor is in a given state, otherwise throws <see cref="InvalidStateException"/>.
        /// </summary>
        /// <param name="state">Valid state to check.</param>
        /// <exception cref="InvalidStateException"></exception>
        public void EnsureState(EditorState state)
        {
            if (StateMachine.CurrentState != state)
                throw new InvalidStateException($"Operation cannot be performed in the current editor state. Current: {StateMachine.CurrentState}, Expected: {state}");
        }

        /// <summary>
        /// Ensure that editor is in a state of given type, otherwise throws <see cref="InvalidStateException"/>.
        /// </summary>
        /// <typeparam name="TStateType">The type of the state type.</typeparam>
        public void EnsureState<TStateType>()
        {
            var state = StateMachine.GetState<TStateType>() as EditorState;
            EnsureState(state);
        }

        /// <summary>
        /// Logs the specified message to the log file.
        /// </summary>
        /// <param name="msg">The message.</param>
        public static void Log(string msg)
        {
            Internal_LogWrite(LogType.Log, msg);
        }

        /// <summary>
        /// Logs the specified warning message to the log file.
        /// </summary>
        /// <param name="msg">The message.</param>
        public static void LogWarning(string msg)
        {
            Internal_LogWrite(LogType.Warning, msg);
        }

        /// <summary>
        /// Logs the specified warning exception to the log file.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void LogWarning(Exception ex)
        {
            LogWarning("Exception: " + ex.Message);
            LogWarning(ex.StackTrace);
        }

        /// <summary>
        /// Logs the specified error message to the log file.
        /// </summary>
        /// <param name="msg">The message.</param>
        public static void LogError(string msg)
        {
            Internal_LogWrite(LogType.Error, msg);
        }

        /// <summary>
        /// New asset types allowed to create.
        /// </summary>
        public enum NewAssetType
        {
            /// <summary>
            /// The material. See <see cref="FlaxEngine.Material"/>.
            /// </summary>
            Material = 0,

            /// <summary>
            /// The material instance. See <see cref="FlaxEngine.MaterialInstance"/>.
            /// </summary>
            MaterialInstance = 1,
        };

        /// <summary>
        /// Imports the asset file to the target location.
        /// </summary>
        /// <param name="inputPath">The source file path.</param>
        /// <param name="outputPath">The result asset file path.</param>
        /// <returns>True if importing failed, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool Import(string inputPath, string outputPath)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_Import(inputPath, outputPath, IntPtr.Zero);
#endif
        }

        /// <summary>
        /// Imports the texture asset file to the target location.
        /// </summary>
        /// <param name="inputPath">The source file path.</param>
        /// <param name="outputPath">The result asset file path.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>True if importing failed, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool Import(string inputPath, string outputPath, TextureImportSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException();
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            TextureImportSettings.InternalOptions internalOptions;
            settings.ToInternal(out internalOptions);
            return Internal_ImportTexture(inputPath, outputPath, ref internalOptions);
#endif
        }

        /// <summary>
        /// Imports the model asset file to the target location.
        /// </summary>
        /// <param name="inputPath">The source file path.</param>
        /// <param name="outputPath">The result asset file path.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>True if importing failed, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool Import(string inputPath, string outputPath, ModelImportSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException();
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            ModelImportSettings.InternalOptions internalOptions;
            settings.ToInternal(out internalOptions);
            return Internal_ImportModel(inputPath, outputPath, ref internalOptions);
#endif
        }

        /// <summary>
        /// Serializes the given object to json asset.
        /// </summary>
        /// <param name="outputPath">The result asset file path.</param>
        /// <param name="obj">The obj to serialize.</param>
        /// <returns>True if saving failed, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool SaveJsonAsset(string outputPath, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            string str = JsonSerializer.Serialize(obj);
            return Internal_SaveJsonAsset(outputPath, str, obj.GetType().FullName);
#endif
        }


        #region Env Probes Baking

        /// <summary>
        /// Occurs when environment probe baking starts.
        /// </summary>
        public static event Action<EnvironmentProbe> EnvProbeBakeStart;

        /// <summary>
        /// Occurs when environment probe baking ends.
        /// </summary>
        public static event Action<EnvironmentProbe> EnvProbeBakeEnd;

        internal static void Internal_EnvProbeBake(bool started, EnvironmentProbe probe)
        {
            if (started)
                EnvProbeBakeStart?.Invoke(probe);
            else
                EnvProbeBakeEnd?.Invoke(probe);
        }

        #endregion

        #region Lightmaps Baking

        /// <summary>
        /// Lightmaps baking steps.
        /// </summary>
        public enum LightmapsBakeSteps
        {
            /// <summary>
            /// The cache entries stage.
            /// </summary>
            CacheEntries,

            /// <summary>
            /// The generate lightmap charts stage.
            /// </summary>
            GenerateLightmapCharts,

            /// <summary>
            /// The pack lightmap charts stage.
            /// </summary>
            PackLightmapCharts,

            /// <summary>
            /// The update lightmaps collection stage.
            /// </summary>
            UpdateLightmapsCollection,

            /// <summary>
            /// The update entries stage.
            /// </summary>
            UpdateEntries,

            /// <summary>
            /// The generate hemispheres cache stage.
            /// </summary>
            GenerateHemispheresCache,

            /// <summary>
            /// The render hemispheres stage.
            /// </summary>
            RenderHemispheres,

            /// <summary>
            /// The cleanup stage.
            /// </summary>
            Cleanup
        }

        /// <summary>
        /// Lightmaps baking progress event delegate.
        /// </summary>
        /// <param name="step">The current step.</param>
        /// <param name="stepProgress">The current step progress (normalized to [0;1]).</param>
        /// <param name="totalProgress">The total baking progress (normalized to [0;1]).</param>
        public delegate void LightmapsBakeProgressDelegate(LightmapsBakeSteps step, float stepProgress, float totalProgress);

        /// <summary>
        /// Lighmaps baking nd event delegate.
        /// </summary>
        /// <param name="failed">True if baking failed or has been canceled, otherwise false.</param>
        public delegate void LightmapsBakeEndDelegate(bool failed);

        /// <summary>
        /// Occurs when lightmaps baking starts.
        /// </summary>
        public static event Action LightmapsBakeStart;

        /// <summary>
        /// Occurs when lightmaps baking ends.
        /// </summary>
        public static event LightmapsBakeEndDelegate LightmapsBakeEnd;

        /// <summary>
        /// Occurs when lightmaps baking progress changes.
        /// </summary>
        public static event LightmapsBakeProgressDelegate LightmapsBakeProgress;

        internal static void Internal_LightmapsBake(LightmapsBakeSteps step, float stepProgress, float totalProgress, bool isProgressEvent)
        {
            if (isProgressEvent)
                LightmapsBakeProgress?.Invoke(step, stepProgress, totalProgress);
            else if (step == LightmapsBakeSteps.CacheEntries)
                LightmapsBakeStart?.Invoke();
            else if (step == LightmapsBakeSteps.GenerateLightmapCharts)
                LightmapsBakeEnd?.Invoke(false);
            else
                LightmapsBakeEnd?.Invoke(true);
        }

        /// <summary>
        /// Starts lightmaps baking for the open scenes or cancel it if already running.
        /// </summary>
        public void BakeLightmapsOrCancel()
        {
            bool isBakingLightmaps = ProgressReporting.BakeLightmaps.IsActive;

            if (isBakingLightmaps)
                Internal_BakeLightmaps(true);
            else
                StateMachine.GoToState<BuildingLightingState>();
        }

        /// <summary>
        /// Clears the lightmaps linkage for all open scenes.
        /// </summary>
        public void ClearLightmaps()
        {
            var scenes = SceneManager.Scenes;
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i].ClearLightmaps();
            }
            Scene.MarkSceneEdited(scenes);
        }

        #endregion

        #region Internal Calls

        internal IntPtr GetMainWindowPtr()
        {
            return Windows.MainWindow.unmanagedPtr;
        }

        internal bool Internal_CanReloadScripts()
        {
            return StateMachine.CurrentState.CanReloadScripts;
        }

        internal bool Internal_CanAutoBuildCSG()
        {
            return StateMachine.CurrentState.CanEditScene;
        }

        internal void Internal_GetMousePosition(out Vector2 resultAsRef)
        {
            resultAsRef = Vector2.Minimum;
            if (Windows.GameWin != null && Windows.GameWin.ContainsFocus)
            {
                var win = Windows.GameWin.ParentWindow;
                if (win != null)
                    resultAsRef = Windows.GameWin.Viewport.PointFromWindow(win.MousePosition);
            }
        }

        internal void Internal_SetMousePosition(ref Vector2 val)
        {
            if (Windows.GameWin != null && Windows.GameWin.ContainsFocus)
            {
                var win = Windows.GameWin.ParentWindow;
                if (win != null)
                    win.MousePosition = Windows.GameWin.Viewport.PointToWindow(val);
            }
        }

        internal IntPtr Internal_GetGameWinPtr()
        {
            IntPtr result = IntPtr.Zero;
            if (Windows.GameWin != null && Windows.GameWin.ContainsFocus)
            {
                var win = Windows.GameWin.ParentWindow;
                if (win != null)
                    result = win.NativeWindow.unmanagedPtr;
            }
            return result;
        }

        internal bool Internal_OnAppExit()
        {
            // In editor play mode (when main window is not closed) just skip engine exit and leave the play mode
            if (StateMachine.IsPlayMode && Windows.MainWindow != null)
            {
                Simulation.RequestStopPlay();
                return false;
            }
            return true;
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CloneAssetFile(string dstPath, string srcPath, ref Guid dstId);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Import(string inputPath, string outputPath, IntPtr arg);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ImportTexture(string inputPath, string outputPath, ref TextureImportSettings.InternalOptions options);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ImportModel(string inputPath, string outputPath, ref ModelImportSettings.InternalOptions options);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveJsonAsset(string outputPath, string data, string typename);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CopyCache(ref Guid dstId, ref Guid srcId);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_BakeLightmaps(bool cancel);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LogWrite(LogType type, string msg);
#endif

        #endregion
    }
}
