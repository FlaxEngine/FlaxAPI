// Flax Engine scripting API

using System.Collections.Generic;
using FlaxEditor.Modules;
using FlaxEditor.States;
using FlaxEngine;

namespace FlaxEditor
{
    public sealed partial class Editor
    {
        private readonly List<EditorModule> _modules = new List<EditorModule>(16);
        private bool _isAfterInit;

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
        /// The editor state machine.
        /// </summary>
        public readonly EditorStateMachine StateMachine;

        internal Editor()
        {
            Debug.Log("Setting up C# Editor...");

            // Create common editor modules
            Windows = new WindowsModule(this);
            UI = new UIModule(this);
            Thumbnails = new ThumbnailsModule(this);
            Simulation = new SimulationModule(this);
            Scripting = new ScriptingModule(this);
            Scene = new SceneModule(this);
            ProgressReporting = new ProgressReportingModule(this);
            ContentEditing = new ContentEditingModule(this);
            ContentDatabase = new ContentDatabaseModule(this);

            // Create state machine
            StateMachine = new EditorStateMachine(this);
        }

        internal void RegisterModule(EditorModule module)
        {
            Debug.Log("Register Editor module " + module);

            _modules.Add(module);
            if (_isAfterInit)
                _modules.Sort((a, b) => a.InitOrder - b.InitOrder);
        }

        internal void Init()
        {
            Debug.Log("Editor init");

            // Note: we don't sort modules before Init (optimized)
            _modules.Sort((a, b) => a.InitOrder - b.InitOrder);
            _isAfterInit = true;

            // Initialize modules (from front to back)
            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].OnInit();
            }
        }

        internal void EndInit()
        {
            Debug.Log("Editor end init");

            // Initialize modules (from front to back)
            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].OnEndInit();
            }
        }

        internal void Update()
        {
            // Update modules
            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].OnUpdate();
            }
        }

        internal void Exit()
        {
            Debug.Log("Editor exit");

            // Release modules (from back to front)
            for (int i = _modules.Count - 1; i >= 0; i--)
            {
                _modules[i].OnExit();
            }
        }
    }
}
