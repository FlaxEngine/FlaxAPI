////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// Editor tool window for profiling games.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed class ProfilerWindow : EditorWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilerWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public ProfilerWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Profiler";
        }
    }
}
