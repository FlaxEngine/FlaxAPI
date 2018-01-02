////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine.GUI.Tabs;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// Base class for all profiler modes. Implementation collects profiling events and presents it using dedicated UI.
    /// </summary>
    public class ProfilerMode : Tab
    {
        /// <summary>
        /// The maximum amount of samples to collect.
        /// </summary>
        public const int MaxSamples = 60 * 5;

        /// <summary>
        /// The minimum event time in ms.
        /// </summary>
        public const double MinEventTimeMs = 0.000000001;

        /// <summary>
        /// Occurs when selected sample gets changed. Profiling window should propagate this cahnge to all charts and view modes.
        /// </summary>
        public event Action<int> SelectedSampleChanged;

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
        /// <param name="showOnlyLastUpdateEvents">True if show only events that happened during the last engine update (excluding events from fixed update or draw event), otherwise show all collected events.</param>
        public virtual void UpdateView(int selectedFrame, bool showOnlyLastUpdateEvents)
        {
        }

        /// <summary>
        /// Called when selected sample gets changed.
        /// </summary>
        /// <param name="frameIndex">Index of the view frame.</param>
        protected virtual void OnSelectedSampleChanged(int frameIndex)
        {
            SelectedSampleChanged?.Invoke(frameIndex);
        }
    }
}
