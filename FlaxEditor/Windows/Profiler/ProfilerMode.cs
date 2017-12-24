////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI.Tabs;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// Base class for all profiler modes. Implementation collects profiling events and presents it using dedicated UI.
    /// </summary>
    public class ProfilerMode : Tab
    {
        /// <inheritdoc />
        public ProfilerMode(string text)
            : base(text)
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
        }

        /// <summary>
        /// Updates this instance. Called every frame if live recording is enabled.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Updates the mode view. Called after init and on selected frame changed.
        /// </summary>
        /// <param name="selectedFrame">The selected frame index.</param>
        public virtual void UpdateView(int selectedFrame)
        {
        }
    }
}
