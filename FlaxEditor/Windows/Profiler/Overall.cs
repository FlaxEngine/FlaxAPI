// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Profiling;
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
        private readonly SingleChart _drawTimeChart;
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
            _fpsChart.SelectedSampleChanged += OnSelectedSampleChanged;
            _updateTimeChart = new SingleChart
            {
                Title = "Update Time",
                FormatSample = v => (Mathf.RoundToInt(v * 10.0f) / 10.0f) + " ms",
                Parent = layout,
            };
            _updateTimeChart.SelectedSampleChanged += OnSelectedSampleChanged;
            _drawTimeChart = new SingleChart
            {
                Title = "Draw Time",
                FormatSample = v => (Mathf.RoundToInt(v * 10.0f) / 10.0f) + " ms",
                Parent = layout,
            };
            _drawTimeChart.SelectedSampleChanged += OnSelectedSampleChanged;
            _cpuMemChart = new SingleChart
            {
                Title = "CPU Memory",
                FormatSample = v => ((int)v) + " MB",
                Parent = layout,
            };
            _cpuMemChart.SelectedSampleChanged += OnSelectedSampleChanged;
            _gpuMemChart = new SingleChart
            {
                Title = "GPU Memory",
                FormatSample = v => ((int)v) + " MB",
                Parent = layout,
            };
            _gpuMemChart.SelectedSampleChanged += OnSelectedSampleChanged;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _fpsChart.Clear();
            _updateTimeChart.Clear();
            _drawTimeChart.Clear();
            _cpuMemChart.Clear();
            _gpuMemChart.Clear();
        }

        /// <inheritdoc />
        public override void Update(ref SharedUpdateData sharedData)
        {
            _fpsChart.AddSample(sharedData.Stats.FPS);
            _updateTimeChart.AddSample(sharedData.Stats.UpdateTimeMs);
            _drawTimeChart.AddSample(sharedData.Stats.DrawTimeMs);
            _cpuMemChart.AddSample(sharedData.Stats.ProcessMemory_UsedPhysicalMemory / 1024 / 1024);
            _gpuMemChart.AddSample(sharedData.Stats.MemoryGPU_Used / 1024 / 1024);
        }

        /// <inheritdoc />
        public override void UpdateView(int selectedFrame, bool showOnlyLastUpdateEvents)
        {
            _fpsChart.SelectedSampleIndex = selectedFrame;
            _updateTimeChart.SelectedSampleIndex = selectedFrame;
            _drawTimeChart.SelectedSampleIndex = selectedFrame;
            _cpuMemChart.SelectedSampleIndex = selectedFrame;
            _gpuMemChart.SelectedSampleIndex = selectedFrame;
        }
    }
}
