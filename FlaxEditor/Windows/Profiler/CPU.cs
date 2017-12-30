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
    /// The CPU performance profiling mode.
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

        private void AddEvent(double startTime, int maxDepth, int index, EventCPU[] events, ContainerControl parent)
        {
            EventCPU e = events[index];

            double length = e.End - e.Start;
            double scale = 100.0;
            float x = (float)((e.Start - startTime) * scale);
            float width = (float)(length * scale);
            
            var control = new Timeline.Event(x, e.Depth, width)
            {
                Name = e.Name,
                TooltipText = string.Format("{0}, {1} ms", e.Name, ((int)(length * 1000.0) / 1000.0f)),
                Parent = parent,
            };

            // Spawn sub events
            int childrenDepth = e.Depth + 1;
            if (childrenDepth <= maxDepth)
            {
                while (index < events.Length)
                {
                    int subDepth = events[index].Depth;

                    if (subDepth <= e.Depth)
                        break;
                    if (subDepth == childrenDepth)
                    {
                        AddEvent(startTime, maxDepth, index, events, control);
                    }

                    index++;
                }
            }
        }

        private void UpdateTimeline()
        {
            var container = _timeline.EventsContainer;

            // Clear previous events
            container.DisposeChildren();

            container.LockChildrenRecursive();

            UpdateTimelineInner();

            container.UnlockChildrenRecursive();
            container.PerformLayout();
        }

        private void UpdateTimelineInner()
        {
            if (_events.Count == 0)
                return;
            var data = _events.Get(_mainChart.SelectedSampleIndex);
            if (data == null || data.Length == 0)
                return;
            
            // Find the first event start time (for the timeline start time)
            double startTime = data[0].Events[0].Start;
            for (int i = 1; i < data.Length; i++)
            {
                startTime = Math.Min(startTime, data[i].Events[0].Start);
            }

            var container = _timeline.EventsContainer;

            // Create timeline track per thread
            for (int i = 0; i < data.Length; i++)
            {
                var events = data[i].Events;

                // Check maximum depth
                int maxDepth = 0;
                for (int j = 0; j < events.Length; j++)
                {
                    maxDepth = Mathf.Max(maxDepth, events[j].Depth);
                }

                // Add events
                for (int j = 0; j < events.Length; j++)
                {
                    if (events[j].Depth == 0)
                    {
                        AddEvent(startTime, maxDepth, j, events, container);
                    }
                }
            }
        }
    }
}
