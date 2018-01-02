////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.GUI;
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
        private readonly Table _table;
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
                        FormatValue = FormatCellPercentage,
                    },
                    new ColumnDefinition
                    {
                        Title = "Self",
                        TitleBackgroundColor = headerColor,
                        FormatValue = FormatCellPercentage,
                    },
                    new ColumnDefinition
                    {
                        Title = "Time ms",
                        TitleBackgroundColor = headerColor,
                        FormatValue = FormatCellMs,
                    },
                    new ColumnDefinition
                    {
                        Title = "Self ms",
                        TitleBackgroundColor = headerColor,
                        FormatValue = FormatCellMs,
                    },
                },
                Parent = layout,
            };
            _table.Splits = new[]
            {
                0.6f,
                0.1f,
                0.1f,
                0.1f,
                0.1f,
            };
        }

        private string FormatCellPercentage(object x)
        {
            return ((float)x).ToString("0.0") + '%';
        }

        private string FormatCellMs(object x)
        {
            return ((float)x).ToString("0.00");
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
                UpdateTable();
            }
        }

        /// <inheritdoc />
        public override void UpdateView(int selectedFrame)
        {
            _mainChart.SelectedSampleIndex = selectedFrame;
            UpdateTimeline();
            UpdateTable();
        }

        private void AddEvent(double startTime, int maxDepth, float xOffset, int depthOffset, int index, EventCPU[] events, ContainerControl parent)
        {
            EventCPU e = events[index];

            double length = e.End - e.Start;
            double scale = 100.0;
            float x = (float)((e.Start - startTime) * scale);
            float width = (float)(length * scale);
            
            var control = new Timeline.Event(x + xOffset, e.Depth + depthOffset, width)
            {
                Name = e.Name,
                TooltipText = string.Format("{0}, {1} ms", e.Name, ((int)(length * 1000.0) / 1000.0f)),
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
                        AddEvent(startTime, maxDepth, xOffset, depthOffset, index, events, parent);
                    }
                }
            }
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

            // Find the first event start time (for the timeline start time)
            double startTime = data[0].Events[0].Start;
            for (int i = 1; i < data.Length; i++)
            {
                startTime = Math.Min(startTime, data[i].Events[0].Start);
            }

            var container = _timeline.EventsContainer;

            // Create timeline track per thread
            int depthOffset = 0;
            for (int i = 0; i < data.Length; i++)
            {
                var events = data[i].Events;

                // Check maximum depth
                int maxDepth = 0;
                for (int j = 0; j < events.Length; j++)
                {
                    maxDepth = Mathf.Max(maxDepth, events[j].Depth);
                }

                // Add thread label
                float xOffset = 90;
                var label = new Timeline.TrackLabel
                {
                    Bounds = new Rectangle(0, depthOffset * Timeline.Event.DefaultHeight, xOffset, (maxDepth + 2) * Timeline.Event.DefaultHeight),
                    Name = data[i].Name,
                    BackgroundColor = Style.Current.Background * 1.1f,
                    Parent = container,
                };
                
                // Add events
                for (int j = 0; j < events.Length; j++)
                {
                    if (events[j].Depth == 0)
                    {
                        AddEvent(startTime, maxDepth, xOffset, depthOffset, j, events, container);
                    }
                }

                depthOffset += maxDepth + 2;
            }

            return Timeline.Event.DefaultHeight * depthOffset;
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
            var rowColor2 = Style.Current.Background * 1.02f;
            for (int j = 0; j < data.Length; j++)
            {
                var events = data[j].Events;

                for (int i = 0; i < events.Length; i++)
                {
                    var e = events[i];
                    var time = Math.Max(e.End - e.Start, MinEventTimeMs);

                    // Count sub-events time
                    double subEventsTimeTotal = 0;
                    for (int k = i + 1; k < events.Length; k++)
                    {
                        var sub = events[k];
                        if (sub.Depth == e.Depth + 1)
                            subEventsTimeTotal += Math.Max(sub.End - sub.Start, MinEventTimeMs);
                        else if (sub.Depth <= e.Depth)
                            break;
                    }
                    
                    var row = new Row
                    {
                        Values = new object[]
                        {
                            // Event
                            e.Name,

                            // Total (%)
                            (int)(time / totalTimeMs * 1000.0f) / 10.0f,

                            // Self (%)
                            (int)((time - subEventsTimeTotal) / time * 1000.0f) / 10.0f,

                            // Time ms
                            (float)((time * 10000.0f) / 10000.0f),

                            // Self ms
                            (float)(((time - subEventsTimeTotal) * 10000.0f) / 10000.0f),
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
}
