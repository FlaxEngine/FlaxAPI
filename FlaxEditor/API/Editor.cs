// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEditor.Content;
using FlaxEditor.Content.Import;
using FlaxEditor.Content.Settings;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Modules;
using FlaxEditor.Modules.SourceCodeEditing;
using FlaxEditor.Options;
using FlaxEditor.Scripting;
using FlaxEditor.States;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;
using FlaxEngine.Json;

namespace FlaxEditor
{
    public sealed partial class Editor
    {
        /// <summary>
        /// Gets the Editor instance.
        /// </summary>
        public static Editor Instance { get; private set; }

        /// <summary>
        /// The path to the local cache folder shared by all the installed editor instance for a given user (used also by the Flax Launcher).
        /// </summary>
        public static readonly string LocalCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Flax");

        static Editor()
        {
            JsonSerializer.Settings.Converters.Add(new SceneTreeNodeConverter());
        }

        private readonly List<EditorModule> _modules = new List<EditorModule>(16);
        private bool _isAfterInit, _areModulesInited, _areModulesAfterInitEnd, _isHeadlessMode;
        private ProjectInfo _projectInfo;
        private string _projectToOpen;

        /// <summary>
        /// Gets a value indicating whether Flax Engine is the best in the world.
        /// </summary>
        public bool IsFlaxEngineTheBest => true;

        /// <summary>
        /// Gets a value indicating whether this Editor is running a dev instance of the engine.
        /// </summary>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool IsDevInstance();
#else
        internal static bool IsDevInstance()
        {
            return false;
        }
#endif

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
        /// The prefabs module.
        /// </summary>
        public readonly PrefabsModule Prefabs;

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
        /// The editor options manager.
        /// </summary>
        public readonly OptionsModule Options;

        /// <summary>
        /// The editor per-project cache manager.
        /// </summary>
        public readonly ProjectCacheModule ProjectCache;

        /// <summary>
        /// The undo/redo
        /// </summary>
        public readonly EditorUndo Undo;

        /// <summary>
        /// The icons container.
        /// </summary>
        public readonly EditorIcons Icons;

        /// <summary>
        /// Gets the main transform gizmo used by the <see cref="SceneEditorWindow"/>.
        /// </summary>
        public Gizmo.TransformGizmo MainTransformGizmo => Windows.EditWin.Viewport.TransformGizmo;

        /// <summary>
        /// Gets a value indicating whether this instance is running in  `headless` mode. No windows or popups should be shown. Used in CL environment (without a graphical user interface).
        /// </summary>
        public bool IsHeadlessMode => _isHeadlessMode;

        /// <summary>
        /// Gets the project information structure. Loaded on editor startup.
        /// </summary>
        public ProjectInfo ProjectInfo => _projectInfo;

        /// <summary>
        /// Gets a value indicating whether Editor instance is initialized.
        /// </summary>
        public bool IsInitialized => _areModulesAfterInitEnd;

        /// <summary>
        /// Occurs when editor initialization starts. All editor modules already received OnInit callback and editor splash screen is visible.
        /// </summary>
        public event Action InitializationStart;

        /// <summary>
        /// Occurs when editor initialization ends. All editor modules already received OnEndInit callback and editor splash screen will be closed.
        /// </summary>
        public event Action InitializationEnd;

        internal Editor()
        {
            Instance = this;

            Log("Setting up C# Editor...");

            Internal_GetProjectInfo(out _projectInfo);

            Icons = new EditorIcons();
            Icons.GetIcons();

            // Create common editor modules
            RegisterModule(Options = new OptionsModule(this));
            RegisterModule(ProjectCache = new ProjectCacheModule(this));
            RegisterModule(Windows = new WindowsModule(this));
            RegisterModule(UI = new UIModule(this));
            RegisterModule(Thumbnails = new ThumbnailsModule(this));
            RegisterModule(Simulation = new SimulationModule(this));
            RegisterModule(Scene = new SceneModule(this));
            RegisterModule(Prefabs = new PrefabsModule(this));
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
            if (_areModulesInited)
                module.OnInit();
            if (_areModulesAfterInitEnd)
                module.OnEndInit();
        }

        internal void Init(bool isHeadless)
        {
            EnsureState<LoadingState>();
            _isHeadlessMode = isHeadless;
            Log("Editor init");
            if (isHeadless)
                Log("Running in headless mode");

            // Note: we don't sort modules before Init (optimized)
            _modules.Sort((a, b) => a.InitOrder - b.InitOrder);
            _isAfterInit = true;

            // Initialize modules (from front to back)
            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].OnInit();
            }
            _areModulesInited = true;

            // Start Editor initialization ending phrase (will wait for scripts compilation result)
            StateMachine.LoadingState.StartInitEnding();

            InitializationStart?.Invoke();
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
                try
                {
                    _modules[i].OnEndInit();
                }
                catch (Exception ex)
                {
                    LogWarning(ex);
                    LogError("Failed to initialize editor module " + _modules[i]);
                }
            }
            _areModulesAfterInitEnd = true;

            InitializationEnd?.Invoke();

            // Close splash and show main window
            CloseSplashScreen();
            Assert.IsNotNull(Windows.MainWindow);
            if (!IsHeadlessMode)
            {
                Windows.MainWindow.Show();
                Windows.MainWindow.Focus();
            }

            // Load scene
            // TODO: loading last open scenes from Editor Cache
            {
                var defaultSceneAsset = ContentDatabase.Find(_projectInfo.DefaultSceneId);
                if (defaultSceneAsset is SceneItem)
                {
                    Editor.Log("Loading default project scene");
                    Scene.OpenScene(_projectInfo.DefaultSceneId);

                    // Use spawn point
                    Windows.EditWin.Viewport.ViewRay = _projectInfo.DefaultSceneSpawn;
                }
            }
        }

        internal void Update()
        {
            try
            {
                // Update state machine
                StateMachine.Update();

                // Drop performance if app has no focus (only when game is not running)
                if (!StateMachine.IsPlayMode)
                {
                    bool isFocused = Application.HasFocus;
                    Time.DrawFPS = isFocused ? 60 : 15;
                    Time.UpdateFPS = isFocused ? 60 : 15;
                    Time.PhysicsFPS = isFocused ? 30 : 20;
                }

                // Update modules
                for (int i = 0; i < _modules.Count; i++)
                {
                    _modules[i].OnUpdate();
                }

                if (Input.GetKeyDown(Keys.F6))
                {
                    Simulation.RequestResumeOrPause();
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

            // Cleanup
            Scene.ClearRefsToSceneObjects(true);

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

            // Invoke new instance if need to open a project
            if (!string.IsNullOrEmpty(_projectToOpen))
            {
                string editorExePath = Globals.StartupPath + "/Win64/FlaxEditor.exe";
                string args = string.Format("-project \"{0}\"", _projectToOpen);
                _projectToOpen = null;
                Application.StartProcess(editorExePath, args);
            }
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
        /// Closes this project with running editor and opens the given project.
        /// </summary>
        /// <param name="projectFilePath">The project file path.</param>
        public void OpenProject(string projectFilePath)
        {
            if (projectFilePath == null || !System.IO.File.Exists(projectFilePath))
            {
                // Error
                MessageBox.Show("Missing project");
                return;
            }

            // Cache project path and start editor exit (it will open new instance on valid closing)
            _projectToOpen = StringUtils.NormalizePath(System.IO.Path.GetDirectoryName(projectFilePath));
            Windows.MainWindow.Close(ClosingReason.User);
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
            Debug.Logger.LogHandler.LogWrite(LogType.Log, msg);
        }

        /// <summary>
        /// Logs the specified warning message to the log file.
        /// </summary>
        /// <param name="msg">The message.</param>
        public static void LogWarning(string msg)
        {
            Debug.Logger.LogHandler.LogWrite(LogType.Warning, msg);
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
            Debug.Logger.LogHandler.LogWrite(LogType.Error, msg);
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

            /// <summary>
            /// The collision data. See <see cref="FlaxEngine.CollisionData"/>.
            /// </summary>
            CollisionData = 2,

            /// <summary>
            /// The animation graph. See <see cref="FlaxEngine.AnimationGraph"/>.
            /// </summary>
            AnimationGraph = 3,

            /// <summary>
            /// The skeleton mask. See <see cref="FlaxEngine.SkeletonMask"/>.
            /// </summary>
            SkeletonMask = 4,
        }

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
        /// Imports the audio asset file to the target location.
        /// </summary>
        /// <param name="inputPath">The source file path.</param>
        /// <param name="outputPath">The result asset file path.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>True if importing failed, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static bool Import(string inputPath, string outputPath, AudioImportSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException();
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            AudioImportSettings.InternalOptions internalOptions;
            settings.ToInternal(out internalOptions);
            return Internal_ImportAudio(inputPath, outputPath, ref internalOptions);
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

        /// <summary>
        /// Cooks the mesh collision data and saves it to the asset using <see cref="CollisionData"/> format. action cannot be performed on a main thread.
        /// </summary>
        /// <param name="path">The output asset path.</param>
        /// <param name="type">The collision data type.</param>
        /// <param name="model">The source model.</param>
        /// <param name="modelLodIndex">The source model LOD index.</param>
        /// <param name="convexFlags">The convex mesh generation flags.</param>
        /// <param name="convexVertexLimit">The convex mesh vertex limit. Use values in range [8;255]</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool CookMeshCollision(string path, CollisionDataType type, Model model, int modelLodIndex = 0, ConvexMeshGenerationFlags convexFlags = ConvexMeshGenerationFlags.None, int convexVertexLimit = 255)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (type == CollisionDataType.None)
                throw new ArgumentException(nameof(type));

            return Internal_CookMeshCollision(path, type, model.unmanagedPtr, modelLodIndex, convexFlags, convexVertexLimit);
        }

        /// <summary>
        /// Gets the material shader source code (HLSL shader code).
        /// </summary>
        /// <param name="asset">The material asset.</param>
        /// <returns>The generated source code.</returns>
        public static string GetMaterialShaderSourceCode(Material asset)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));
            if (asset.WaitForLoaded())
                throw new FlaxException("Failed to load asset.");

            var source = Internal_GetMaterialShaderSourceCode(asset.unmanagedPtr);
            if (source == null)
                throw new FlaxException("Failed to get material source code.");

            return source;
        }

        /// <summary>
        /// Creates the prefab asset from the given root actor. Saves it to the output file path.
        /// </summary>
        /// <param name="path">The output asset path.</param>
        /// <param name="actor">The target actor (prefab root). It cannot be a scene but it can contain a scripts and/or full hierarchy of objects to save.</param>
        /// <param name="autoLink">True if auto connect the target actor and related objects to the created prefab.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool CreatePrefab(string path, Actor actor, bool autoLink)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            return Internal_CreatePrefab(path, actor.unmanagedPtr, autoLink);
        }

        /// <summary>
        /// Gets the actor bounding sphere (including child actors).
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="sphere">The bounding sphere.</param>
        public static void GetActorEditorSphere(Actor actor, out BoundingSphere sphere)
        {
            BoundingBox box;
            Internal_GetEditorBoxWithChildren(actor.unmanagedPtr, out box);
            BoundingSphere.FromBox(ref box, out sphere);
            sphere.Radius = Math.Max(sphere.Radius, 15.0f);
        }

        /// <summary>
        /// Gets the actor bounding box (including child actors).
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="box">The bounding box.</param>
        public static void GetActorEditorBox(Actor actor, out BoundingBox box)
        {
            Internal_GetEditorBoxWithChildren(actor.unmanagedPtr, out box);
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
        /// Lightmaps baking nd event delegate.
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

        [StructLayout(LayoutKind.Sequential)]
        internal struct InternalOptions
        {
            public byte AutoReloadScriptsOnMainWindowFocus;
            public byte AutoRebuildCSG;
            public float AutoRebuildCSGTimeoutMs;
        }

        internal void BuildCommand(string arg)
        {
            if (TryBuildCommand(arg))
                Application.Exit();
        }

        private bool TryBuildCommand(string arg)
        {
            if (GameCooker.IsRunning)
                return true;
            if (arg == null)
                return true;

            Editor.Log("Using CL build for \"" + arg + "\"");

            int dotPos = arg.IndexOf('.');
            string presetName, targetName;
            if (dotPos == -1)
            {
                presetName = arg;
                targetName = string.Empty;
            }
            else
            {
                presetName = arg.Substring(0, dotPos);
                targetName = arg.Substring(dotPos + 1);
            }

            var settings = GameSettings.Load<BuildSettings>();
            var preset = settings.GetPreset(presetName);
            if (preset == null)
            {
                Editor.LogWarning("Missing preset.");
                return true;
            }

            if (string.IsNullOrEmpty(targetName))
            {
                Windows.GameCookerWin.BuildAll(preset);
            }
            else
            {
                var target = preset.GetTarget(targetName);
                if (target == null)
                {
                    Editor.LogWarning("Missing target.");
                    return true;
                }

                Windows.GameCookerWin.Build(target);
            }

            Windows.GameCookerWin.ExitOnBuildQueueEnd();
            return false;
        }

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
            resultAsRef = Vector2.Zero;
            if (Windows.GameWin != null && Windows.GameWin.ContainsFocus)
            {
                var win = Windows.GameWin.Root;
                if (win != null)
                    resultAsRef = Windows.GameWin.Viewport.PointFromWindow(win.MousePosition);
            }
        }

        internal void Internal_SetMousePosition(ref Vector2 val)
        {
            if (Windows.GameWin != null && Windows.GameWin.ContainsFocus)
            {
                Windows.GameWin.SetGameMousePosition(ref val);
            }
        }

        internal void Internal_GetGameWinPtr(bool forceGet, out IntPtr result)
        {
            result = IntPtr.Zero;
            if (Windows.GameWin != null && (forceGet || Windows.GameWin.ContainsFocus))
            {
                var win = Windows.GameWin.Root as WindowRootControl;
                if (win != null)
                    result = win.Window.unmanagedPtr;
            }
        }

        internal void Internal_GetGameWindowSize(out Vector2 resultAsRef)
        {
            resultAsRef = Vector2.Zero;
            var gameWin = Windows.GameWin;
            if (gameWin != null)
            {
                // Handle case when Game window is not selected in tab view
                var dockedTo = gameWin.ParentDockPanel;
                if (dockedTo != null && dockedTo.SelectedTab != gameWin && dockedTo.SelectedTab != null)
                    resultAsRef = dockedTo.SelectedTab.Size;
                else
                    resultAsRef = gameWin.Size;
            }
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
        internal static extern void Internal_SetPlayMode(bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetProjectInfo(out ProjectInfo info);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CloneAssetFile(string dstPath, string srcPath, ref Guid dstId);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Import(string inputPath, string outputPath, IntPtr arg);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ImportTexture(string inputPath, string outputPath, ref TextureImportSettings.InternalOptions options);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ImportModel(string inputPath, string outputPath, ref ModelImportSettings.InternalOptions options);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ImportAudio(string inputPath, string outputPath, ref AudioImportSettings.InternalOptions options);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetAudioClipMetadata(IntPtr obj, out int originalSize, out int importedSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveJsonAsset(string outputPath, string data, string typename);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CopyCache(ref Guid dstId, ref Guid srcId);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_BakeLightmaps(bool cancel);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetMaterialShaderSourceCode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CookMeshCollision(string path, CollisionDataType type, IntPtr model, int modelLodIndex, ConvexMeshGenerationFlags convexFlags, int convexVertexLimit);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CreatePrefab(string path, IntPtr actor, bool autoLink);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCollisionWires(IntPtr collisionData, out Vector3[] triangles, out int[] indices);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetEditorBoxWithChildren(IntPtr obj, out BoundingBox resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOptions(ref InternalOptions options);
#endif

        #endregion
    }
}
