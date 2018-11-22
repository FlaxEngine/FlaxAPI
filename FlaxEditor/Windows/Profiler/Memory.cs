// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// The memory profiling mode focused on system memory allocations breakdown.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Profiler.ProfilerMode" />
    internal sealed class Memory : ProfilerMode
    {
        private readonly SingleChart _allocationsChart;

        public Memory()
        : base("Memory")
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
            _allocationsChart = new SingleChart
            {
                Title = "Memory Allocation",
                FormatSample = v => Utilities.Utils.FormatBytesCount((int)v),
                Parent = layout,
            };
            _allocationsChart.SelectedSampleChanged += OnSelectedSampleChanged;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _allocationsChart.Clear();
        }

        /// <inheritdoc />
        public override void Update(ref SharedUpdateData sharedData)
        {
            // Count memory allocated during last frame
            int memoryAlloc = 0;
            var events = sharedData.GetEventsCPU();
            for (int i = 0; i < events.Length; i++)
            {
                for (int j = 0; j < events[i].Events.Length; j++)
                {
                    var e = events[i].Events[j];
                    memoryAlloc += e.MemoryAllocation;
                }
            }

            _allocationsChart.AddSample(memoryAlloc);
        }

        /// <inheritdoc />
        public override void UpdateView(int selectedFrame, bool showOnlyLastUpdateEvents)
        {
            _allocationsChart.SelectedSampleIndex = selectedFrame;
        }
    }
}
