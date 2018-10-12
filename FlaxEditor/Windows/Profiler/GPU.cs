// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI;
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
        private readonly Table _table;
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

            // Table
            var headerColor = Style.Current.LightBackground;
            _table = new Table
            {
                Columns = new[]
                {
                    new ColumnDefinition
                    {
                        UseExpandCollapseMode = true,
                        CellAlignment = TextAlignment.Near,
                        Title = "Event",
                        TitleBackgroundColor = headerColor,
                    },
                    new ColumnDefinition
                    {
                        Title = "Total",
                        TitleBackgroundColor = headerColor,
                        FormatValue = (x) => ((float)x).ToString("0.0") + '%',
                    },
                    new ColumnDefinition
                    {
                        Title = "GPU ms",
                        TitleBackgroundColor = headerColor,
                        FormatValue = (x) => ((float)x).ToString("0.000"),
                    },
                    new ColumnDefinition
                    {
                        Title = "Draw Calls",
                        TitleBackgroundColor = headerColor,
                    },
                    new ColumnDefinition
                    {
                        Title = "Triangles",
                        TitleBackgroundColor = headerColor,
                    },
                    new ColumnDefinition
                    {
                        Title = "Vertices",
                        TitleBackgroundColor = headerColor,
                    },
                },
                Parent = layout,
            };
            _table.Splits = new[]
            {
                0.5f,
                0.1f,
                0.1f,
                0.1f,
                0.1f,
                0.1f,
            };
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _mainChart.Clear();
            _events.Clear();
        }

        /// <inheritdoc />
        public override void Update(ref SharedUpdateData sharedData)
        {
            // Gather GPU events
            var data = sharedData.GetEventsGPU();
            _events.Add(data);

            // Peek draw time
            float drawTime = sharedData.Stats.DrawTimeMs;
            if (data != null && data.Length > 0)
                drawTime = data[0].Time;
            _mainChart.AddSample(drawTime);

            // Update timeline if using the last frame
            if (_mainChart.SelectedSampleIndex == -1)
            {
                UpdateTimeline();
                UpdateTable();
            }
        }

        /// <inheritdoc />
        public override void UpdateView(int selectedFrame, bool showOnlyLastUpdateEvents)
        {
            _mainChart.SelectedSampleIndex = selectedFrame;
            UpdateTimeline();
            UpdateTable();
        }

        private float AddEvent(float x, int maxDepth, int index, EventGPU[] events, ContainerControl parent)
        {
            EventGPU e = events[index];

            double scale = 100.0;
            float width = (float)(e.Time * scale);

            var control = new Timeline.Event(x, e.Depth, width)
            {
                Name = e.Name,
                TooltipText = string.Format("{0}, {1} ms", e.Name, ((int)(e.Time * 10000.0) / 10000.0f)),
                Parent = parent,
            };

            // Spawn sub events
            int childrenDepth = e.Depth + 1;
            if (childrenDepth <= maxDepth)
            {
                // Count sub events total duration
                double subEventsDuration = 0;
                int tmpIndex = index;
                while (++tmpIndex < events.Length)
                {
                    int subDepth = events[tmpIndex].Depth;

                    if (subDepth <= e.Depth)
                        break;
                    if (subDepth == childrenDepth)
                        subEventsDuration += events[tmpIndex].Time;
                }

                // Skip if has no sub events
                if (subEventsDuration > 0)
                {
                    // Apply some offset to sub-events (center them within this event)
                    x += (float)((e.Time - subEventsDuration) * scale) * 0.5f;

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
            }

            return width;
        }

        private void UpdateTimeline()
        {
            var container = _timeline.EventsContainer;

            // Clear previous events
            container.DisposeChildren();

            container.LockChildrenRecursive();

            _timeline.Height = UpdateTimelineInner();

            container.UnlockChildrenRecursive();
            container.PerformLayout();
        }

        private float UpdateTimelineInner()
        {
            if (_events.Count == 0)
                return 0;
            var data = _events.Get(_mainChart.SelectedSampleIndex);
            if (data == null || data.Length == 0)
                return 0;

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

            return Timeline.Event.DefaultHeight * (maxDepth + 2);
        }

        private void UpdateTable()
        {
            _table.DisposeChildren();

            _table.LockChildrenRecursive();

            UpdateTableInner();

            _table.UnlockChildrenRecursive();
            _table.PerformLayout();
        }

        private void UpdateTableInner()
        {
            if (_events.Count == 0)
                return;
            var data = _events.Get(_mainChart.SelectedSampleIndex);
            if (data == null || data.Length == 0)
                return;

            float totalTimeMs = _mainChart.SelectedSample;

            // Add rows
            var rowColor2 = Style.Current.Background * 1.4f;
            for (int i = 0; i < data.Length; i++)
            {
                var e = data[i];

                var row = new Row
                {
                    Values = new object[]
                    {
                        // Event
                        e.Name,

                        // Total (%)
                        (int)(e.Time / totalTimeMs * 1000.0f) / 10.0f,

                        // GPU ms
                        (e.Time * 10000.0f) / 10000.0f,

                        // Draw Calls
                        e.Stats.DrawCalls,

                        // Triangles
                        e.Stats.Triangles,

                        // Vertices
                        e.Stats.Vertices,
                    },
                    Depth = e.Depth,
                    Width = _table.Width,
                    Parent = _table,
                };

                if (i % 2 == 0)
                    row.BackgroundColor = rowColor2;
            }
        }
    }
}
