////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Profiling;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// The GPU performance profiling mode.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.Profiler.ProfilerMode" />
    internal sealed class GPU : ProfilerMode
    {
        private readonly SingleChart _mainChart;
        private readonly Timeline _timeline;
        private readonly SamplesBuffer<EventGPU[]> _events = new SamplesBuffer<EventGPU[]>();

        public GPU()
            : base("GPU")
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
                Title = "Draw",
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
            // Gather GPU events
            var data = ProfilingTools.GetEventsGPU();
            _events.Add(data);

            // Peek draw time
            float drawTime = ProfilingTools.Stats.DrawTimeMs;
            if (data != null && data.Length > 0)
                drawTime = data[0].Time;
            _mainChart.AddSample(drawTime);
            
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

        private float AddEvent(float x, int maxDepth, int index, EventGPU[] events, ContainerControl parent)
        {
            EventGPU e = events[index];

            double scale = 100.0;
            float width = (float)(e.Time * scale);

            var control = new Timeline.Event(x, e.Depth, width)
            {
                Name = e.Name,
                TooltipText = string.Format("{0}, {1} ms", e.Name, ((int)(e.Time * 1000.0) / 1000.0f)),
                Parent = parent,
            };

            // Spawn sub events
            int childrenDepth = e.Depth + 1;
            if (childrenDepth <= maxDepth)
            {
                while (++index < events.Length)
                {
                    int subDepth = events[index].Depth;

                    if (subDepth <= e.Depth)
                        break;
                    if (subDepth == childrenDepth)
                    {
                        x += AddEvent(x, maxDepth, index, events, parent);
                    }
                }
            }

            return width;
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

            var container = _timeline.EventsContainer;
            var events = data;

            // Check maximum depth
            int maxDepth = 0;
            for (int j = 0; j < events.Length; j++)
            {
                maxDepth = Mathf.Max(maxDepth, events[j].Depth);
            }

            // Add events
            float x = 0;
            for (int j = 0; j < events.Length; j++)
            {
                if (events[j].Depth == 0)
                {
                    x += AddEvent(x, maxDepth, j, events, container);
                }
            }
        }
    }
}
