// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// The memory profiling mode focused on system memory allocations breakdown.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Profiler.ProfilerMode" />
    internal sealed class Memory : ProfilerMode
    {
        private readonly SingleChart _nativeAllocationsChart;
        private readonly SingleChart _managedAllocationsChart;

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
            _nativeAllocationsChart = new SingleChart
            {
                Title = "Native Memory Allocation",
                FormatSample = v => Utilities.Utils.FormatBytesCount((int)v),
                Parent = layout,
            };
            _nativeAllocationsChart.SelectedSampleChanged += OnSelectedSampleChanged;
            _managedAllocationsChart = new SingleChart
            {
                Title = "Managed Memory Allocation",
                FormatSample = v => Utilities.Utils.FormatBytesCount((int)v),
                Parent = layout,
            };
            _managedAllocationsChart.SelectedSampleChanged += OnSelectedSampleChanged;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _nativeAllocationsChart.Clear();
            _managedAllocationsChart.Clear();
        }

        /// <inheritdoc />
        public override void Update(ref SharedUpdateData sharedData)
        {
            // Count memory allocated during last frame
            int nativeMemoryAllocation = 0;
            int managedMemoryAllocation = 0;
            var events = sharedData.GetEventsCPU();
            var length = events?.Length ?? 0;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < events[i].Events.Length; j++)
                {
                    var e = events[i].Events[j];
                    nativeMemoryAllocation += e.NativeMemoryAllocation;
                    managedMemoryAllocation += e.ManagedMemoryAllocation;
                }
            }

            _nativeAllocationsChart.AddSample(nativeMemoryAllocation);
            _managedAllocationsChart.AddSample(managedMemoryAllocation);
        }

        /// <inheritdoc />
        public override void UpdateView(int selectedFrame, bool showOnlyLastUpdateEvents)
        {
            _nativeAllocationsChart.SelectedSampleIndex = selectedFrame;
            _managedAllocationsChart.SelectedSampleIndex = selectedFrame;
        }
    }
}
