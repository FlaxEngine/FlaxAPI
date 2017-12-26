////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// The general profiling mode with major game performance charts and stats.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Profiler.ProfilerMode" />
    internal sealed class CPU : ProfilerMode
    {
        private readonly SingleChart _mainChart;
        private ProfilingTools.EventCPU[] _eventsBuffer;
        private int _eventsCount;

        public CPU()
            : base("CPU")
        {
            // Layout
            var panel = new Panel(ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                Parent = this,
            };
            var layout = new VerticalPanel
            {
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                Parent = panel,
            };

            // Chart
            _mainChart = new SingleChart
            {
                Title = "Update",
                FormatSample = v => (Mathf.RoundToInt(v * 10.0f) / 10.0f) + " ms",
                Parent = layout,
            };
            _mainChart.SelectedSampleChanged += OnSelectedSampleChanged;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _mainChart.Clear();
        }

        /// <inheritdoc />
        public override void Update()
        {
            var stats = ProfilingTools.Stats;
            _mainChart.AddSample(stats.UpdateTimeMs);
            _eventsBuffer = ProfilingTools.GetEventsCPU(out _eventsCount, _eventsBuffer);
        }

        /// <inheritdoc />
        public override void UpdateView(int selectedFrame)
        {
            _mainChart.SelectedSampleIndex = selectedFrame;
        }
    }
}
