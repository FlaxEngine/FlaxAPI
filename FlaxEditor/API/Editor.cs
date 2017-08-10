////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Modules;
using FlaxEditor.States;
using FlaxEngine;
using FlaxEngine.Assertions;

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

        private readonly List<EditorModule> _modules = new List<EditorModule>(16);
        private bool _isAfterInit;

        /// <summary>
        /// Gets a value indicating whether Flax Engine is the best in the world.
        /// </summary>
        public bool IsFlaxEngineTheBest => true;

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
        /// The scripting module.
        /// </summary>
        public readonly ScriptingModule Scripting;

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
        public readonly Undo Undo;

        internal Editor()
        {
            Instance = this;

            Editor.Log("Setting up C# Editor...");

            // Create common editor modules
            RegisterModule(Windows = new WindowsModule(this));
            RegisterModule(UI = new UIModule(this));
            RegisterModule(Thumbnails = new ThumbnailsModule(this));
            RegisterModule(Simulation = new SimulationModule(this));
            RegisterModule(Scripting = new ScriptingModule(this));
            RegisterModule(Scene = new SceneModule(this));
            RegisterModule(SceneEditing = new SceneEditingModule(this));
            RegisterModule(ProgressReporting = new ProgressReportingModule(this));
            RegisterModule(ContentEditing = new ContentEditingModule(this));
            RegisterModule(ContentDatabase = new ContentDatabaseModule(this));
            RegisterModule(ContentImporting = new ContentImportingModule(this));
            RegisterModule(CodeEditing = new CodeEditingModule(this));
            
            StateMachine = new EditorStateMachine(this);
            Undo = new Undo();
        }

        internal void RegisterModule(EditorModule module)
        {
            Editor.Log("Register Editor module " + module);

            _modules.Add(module);
            if (_isAfterInit)
                _modules.Sort((a, b) => a.InitOrder - b.InitOrder);
        }

        internal void Init()
        {
            EnsureState<LoadingState>();
            Editor.Log("Editor init");

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
            Editor.Log("Editor end init");

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

        internal void Exit()
        {
            Editor.Log("Editor exit");

            // Start exit
            StateMachine.GoToState<ClosingState>();

            // Release modules (from back to front)
            for (int i = _modules.Count - 1; i >= 0; i--)
            {
                _modules[i].OnExit();
            }

            Instance = null;
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
            throw new NotImplementedException();

            // TODO: save assets

            Scene.SaveScenes();
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
            // TODO: redirect this msg to log file not a console
            Debug.Log(msg);
        }

        /// <summary>
        /// Logs the specified warning message to the log file.
        /// </summary>
        /// <param name="msg">The message.</param>
        public static void LogWarning(string msg)
        {
            // TODO: redirect this msg to log file not a console
            Debug.LogWarning(msg);
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
            // TODO: redirect this msg to log file not a console
            Debug.LogError(msg);
        }
        
        public enum NewAssetType
        {
            Material = 0,
            MaterialInstance = 1,
        };

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CloneAssetFile(string dstPath, string srcPath, ref Guid dstId);
#endif

        #endregion
    }
}
