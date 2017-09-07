////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Scripting;

namespace FlaxEditor.Progress.Handlers
{
    /// <summary>
    /// Async code editor opening progress reporting handler.
    /// </summary>
    /// <seealso cref="FlaxEditor.Progress.ProgressHandler" />
    public sealed class CodeEditorOpenProgress : ProgressHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeEditorOpenProgress"/> class.
        /// </summary>
        public CodeEditorOpenProgress()
        {
            // Link for events
            ScriptsBuilder.CodeEditorAsyncOpenBegin += OnStart;
            ScriptsBuilder.CodeEditorAsyncOpenEnd += OnEnd;
        }

        /// <inheritdoc />
        protected override void OnStart()
        {
            base.OnStart();

            OnUpdate(10, "Starting editor...");
        }
    }
}
