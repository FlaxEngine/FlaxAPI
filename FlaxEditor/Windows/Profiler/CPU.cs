////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Profiling;
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
        private readonly Timeline _timeline;
        private readonly SamplesBuffer<ThreadStats[]> _events = new SamplesBuffer<ThreadStats[]>();

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

            // Timeline
            _timeline = new Timeline
            {
                Height = 340,
                Parent = layout,
            };
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _mainChart.Clear();
            _events.Clear();
        }

        /// <inheritdoc />
        public override void Update()
        {
            var stats = ProfilingTools.Stats;
            _mainChart.AddSample(stats.UpdateTimeMs);

            // Gather CPU events
            var data = ProfilingTools.GetEventsCPU();
            _events.Add(data);

            // Update timeline if using the last frame
            if (_mainChart.SelectedSampleIndex == -1)
            {
                UpdateTimeline();
            }
        }

        /// <inheritdoc />
        public override void UpdateView(int selectedFrame)
        {
            _mainChart.SelectedSampleIndex = selectedFrame;
            UpdateTimeline();
        }

        private void AddEvent(double startTime, int index, EventCPU[] events, ContainerControl parent)
        {
            EventCPU e = events[index];

            double scale = 100.0;
            float x = (float)((e.Start - startTime) * scale);
            double length = e.End - e.Start;
            float width = (float)(length * scale);

            var control = new Timeline.Event(x, width)
            {
                Name = e.Name,
                TooltipText = string.Format("{0}, {1} ms", e.Name, (int)((length * 100) / 100)),
                Parent = parent,
            };

            // Spawn sub events
            /*while (index++ < events.Length)
            {
                int subDepth = events[index].Depth;

                if (subDepth <= e.Depth)
                    break;
                if (subDepth == e.Depth + 1)
                {
                    AddEvent(startTime, index, events, control);
                }
            }*/
        }

        private void UpdateTimeline()
        {
            // Clear previous events
            _timeline.EventsContainer.DisposeChildren();

            if (_events.Count == 0)
                return;
            var data = _events.Get(_mainChart.SelectedSampleIndex);
            if (data == null || data.Length == 0)
                return;

            /*double startTime = data[0].Start;
            for (int i = 0; i < data.Length; i++)
            {
                // Always should start from the root event
                if (data[i].Depth == 0)
                {
                    AddEvent(startTime, i, data, _timeline.EventsContainer);
                }
            }*/
        }
    }
}
