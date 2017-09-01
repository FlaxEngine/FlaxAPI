////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Scripting;

namespace FlaxEditor.Progress.Handlers
{
    /// <summary>
    /// Scripts compilation progress reporting handler.
    /// </summary>
    /// <seealso cref="FlaxEditor.Progress.ProgressHandler" />
    public sealed class CompileScriptsProgress : ProgressHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompileScriptsProgress"/> class.
        /// </summary>
        public CompileScriptsProgress()
        {
            // Link for events
            ScriptsBuilder.CompilationBegin += OnStart;
            ScriptsBuilder.CompilationSuccess += OnEnd;
            ScriptsBuilder.CompilationFailed += OnEnd;
            ScriptsBuilder.CompilationStarted += () => OnUpdate(0.2f, "Compiling scripts...");
            ScriptsBuilder.ScriptsReloadCalled += () => OnUpdate(0.8f, "Reloading scripts...");
        }

        /// <inheritdoc />
        protected override void OnStart()
        {
            base.OnStart();

            OnUpdate(0, "Starting scripts compilation...");
        }
    }
}
