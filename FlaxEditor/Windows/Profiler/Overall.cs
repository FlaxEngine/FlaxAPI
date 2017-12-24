////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// The general profiling mode with major game performance charts and stats.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Profiler.ProfilerMode" />
    internal sealed class Overall : ProfilerMode
    {
        private SingleChart _fpsChart;
        private SingleChart _cpuChart;
        private SingleChart _cpuMemChart;
        private SingleChart _gpuMemChart;

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
            _cpuChart = new SingleChart
            {
                Title = "CPU",
                Parent = layout,
            };
            _cpuMemChart = new SingleChart
            {
                Title = "CPU Memory",
                Parent = layout,
            };
            _gpuMemChart = new SingleChart
            {
                Title = "GPU Memory",
                Parent = layout,
            };
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _fpsChart.Clear();
            _cpuChart.Clear();
            _cpuMemChart.Clear();
            _gpuMemChart.Clear();
        }

        /// <inheritdoc />
        public override void Update()
        {
            //_fpsChart.AddSample(Time.FramesPerSecond);
            //_cpuChart.AddSample((float)_process.TotalProcessorTime.TotalMilliseconds);
            //_cpuMemChart.AddSample(_process.WorkingSet64);
        }
    }
}
