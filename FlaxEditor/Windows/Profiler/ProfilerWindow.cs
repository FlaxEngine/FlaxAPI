////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Tabs;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// Editor tool window for profiling games.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed class ProfilerWindow : EditorWindow
    {
        private readonly ToolStripButton _liveRecordingButton;
        private readonly ToolStripButton _prevFrameButton;
        private readonly ToolStripButton _nextFrameButton;
        private readonly Tabs _tabs;
        private int _frameIndex;
        private int _framesCount;

        /// <summary>
        /// Gets or sets a value indicating whether live events recording is enabled.
        /// </summary>
        public bool LiveRecording
        {
            get => _liveRecordingButton.Checked;
            set
            {
                if (value != LiveRecording)
                {
                    _liveRecordingButton.Checked = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected frame to view (note: some view modes may not use it).
        /// </summary>
        public int ViewFrameIndex
        {
            get => _frameIndex;
            set
            {
                value = Mathf.Clamp(value, 0, _framesCount);
                if (_frameIndex != value)
                {
                    _frameIndex = value;

                    if (_tabs.SelectedTab is ProfilerMode mode)
                        mode.UpdateView(_frameIndex);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilerWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public ProfilerWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Profiler";

            var toolstrip = new ToolStrip
            {
                Parent = this,
            };
            toolstrip.ButtonClicked += OnToolstripButtonClicked;
            _liveRecordingButton = toolstrip.AddButton(1, editor.UI.GetIcon("Play32"));
            _liveRecordingButton.LinkTooltip("Live profiling events recording");
            _liveRecordingButton.AutoCheck = true;
            toolstrip.AddButton(2, editor.UI.GetIcon("Rotate32")).LinkTooltip("Clear data");
            toolstrip.AddSeparator();
            _prevFrameButton = toolstrip.AddButton(3, editor.UI.GetIcon("ArrowLeft32"));
            _prevFrameButton.LinkTooltip("Previous frame");
            _nextFrameButton = toolstrip.AddButton(4, editor.UI.GetIcon("ArrowRight32"));
            _nextFrameButton.LinkTooltip("Next frame");

            _tabs = new Tabs
            {
                Orientation = Orientation.Vertical,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(120, 32),
                Parent = this
            };
            _tabs.SelectedTabChanged += OnSelectedTabChanged;
        }

        /// <summary>
        /// Adds the mode.
        /// </summary>
        /// <remarks>
        /// To remove the mode simply call <see cref="Control.Dispose"/> on mode.
        /// </remarks>
        /// <param name="mode">The mode.</param>
        public void AddMode(ProfilerMode mode)
        {
            mode.Init();
            _tabs.AddTab(mode);
        }

        /// <summary>
        /// Clears data.
        /// </summary>
        public void Clear()
        {
            _frameIndex = 0;
            _framesCount = 0;
            for (int i = 0; i < _tabs.ChildrenCount; i++)
            {
                if (_tabs.Children[i] is ProfilerMode mode)
                {
                    mode.Clear();
                    mode.UpdateView(0);
                }
            }

            UpdateButtons();
        }

        private void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                case 2:
                {
                    Clear();
                    break;
                }

                case 3:
                {
                    ViewFrameIndex--;
                    break;
                }

                case 4:
                {
                    ViewFrameIndex++;
                    break;
                }
            }
        }

        private void OnSelectedTabChanged(Tabs tabs)
        {
            if (tabs.SelectedTab is ProfilerMode mode)
                mode.UpdateView(_frameIndex);
        }

        private void UpdateButtons()
        {
            _prevFrameButton.Enabled = _frameIndex > 0;
            _nextFrameButton.Enabled = (_framesCount - _frameIndex - 1) > 0;
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            // Create default modes
            AddMode(new Overall());

            // Init view
            _frameIndex = 0;
            for (int i = 0; i < _tabs.ChildrenCount; i++)
            {
                if (_tabs.Children[i] is ProfilerMode mode)
                    mode.UpdateView(0);
            }

            UpdateButtons();
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            if (LiveRecording)
            {
                for (int i = 0; i < _tabs.ChildrenCount; i++)
                {
                    if (_tabs.Children[i] is ProfilerMode mode)
                        mode.Update();
                }

                _framesCount++;
                UpdateButtons();
            }
        }
    }
}
