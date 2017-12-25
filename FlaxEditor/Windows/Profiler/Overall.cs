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
    internal sealed class Overall : ProfilerMode
    {
        private readonly SingleChart _fpsChart;
        private readonly SingleChart _updateTimeChart;
        private readonly SingleChart _cpuMemChart;
        private readonly SingleChart _gpuMemChart;

        public Overall()
            : base("Overall")
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

            // Charts
            _fpsChart = new SingleChart
            {
                Title = "FPS",
                Parent = layout,
            };
            _updateTimeChart = new SingleChart
            {
                Title = "Update",
                FormatSample = v => (Mathf.RoundToInt(v * 10.0f) / 10.0f) + " ms",
                Parent = layout,
            };
            _cpuMemChart = new SingleChart
            {
                Title = "CPU Memory",
                FormatSample = v => ((int)v) + " MB",
                Parent = layout,
            };
            _gpuMemChart = new SingleChart
            {
                Title = "GPU Memory",
                FormatSample = v => ((int)v) + " MB",
                Parent = layout,
            };
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _fpsChart.Clear();
            _updateTimeChart.Clear();
            _cpuMemChart.Clear();
            _gpuMemChart.Clear();
        }

        /// <inheritdoc />
        public override void Update()
        {
            var stats = ProfilingTools.Stats;
            _fpsChart.AddSample(stats.FPS);
            _updateTimeChart.AddSample(stats.UpdateTimeMs);
            _cpuMemChart.AddSample(stats.MemoryCPU_UsedPhysicalMemory / 1024 / 1024);
            _gpuMemChart.AddSample(stats.MemoryGPU_Used / 1024 / 1024);
        }
    }
}
