// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.Options;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Editor window used to show engine output logs.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed class OutputLogWindow : EditorWindow
    {
        /// <summary>
        /// The single log message entry.
        /// </summary>
        private struct Entry
        {
            /// <summary>
            /// The log level.
            /// </summary>
            public LogType Level;

            /// <summary>
            /// The log time (in UTC local format).
            /// </summary>
            public DateTime Time;

            /// <summary>
            /// The message contents.
            /// </summary>
            public string Message;
        };

        private InterfaceOptions.TimestampsFormats _timestampsFormats;
        private bool _showLogType;

        private List<Entry> _entries = new List<Entry>(1024);
        private bool _isDirty;
        private int _logTypeShowMask = (int)LogType.Info | (int)LogType.Warning | (int)LogType.Error | (int)LogType.Fatal;
        private const int OutCapacity = 64;
        private string[] _outMessages = new string[OutCapacity];
        private byte[] _outLogTypes = new byte[OutCapacity];
        private long[] _outLogTimes = new long[OutCapacity];
        private int _textBufferCount;
        private StringBuilder _textBuffer = new StringBuilder();
        private DateTime _startupTime;

        private Button _viewDropdown;
        private TextBox _searchBox;
        private HScrollBar _hScroll;
        private VScrollBar _vScroll;
        private RichTextBox _output;
        private ContextMenu _contextMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public OutputLogWindow(Editor editor)
        : base(editor, true, ScrollBars.None)
        {
            Title = "Output Log";
            ClipChildren = false;
            OnEditorOptionsChanged(Editor.Options.Options);

            // Setup UI
            _viewDropdown = new Button(2, 2, 40.0f, TextBoxBase.DefaultHeight)
            {
                TooltipText = "Change output log view options",
                Text = "View",
                Parent = this,
            };
            _viewDropdown.Clicked += OnViewButtonClicked;
            _searchBox = new TextBox(false, _viewDropdown.Right + 2, 2, Width - _viewDropdown.Right - 2 - ScrollBar.DefaultSize)
            {
                WatermarkText = "Search...",
                Parent = this,
            };
            _searchBox.TextChanged += OnSearchBoxTextChanged;
            _hScroll = new HScrollBar(Height - ScrollBar.DefaultSize, Width)
            {
                AnchorStyle = AnchorStyle.Bottom,
                Parent = this,
            };
            _hScroll.ValueChanged += OnHScrollValueChanged;
            _vScroll = new VScrollBar(Width - ScrollBar.DefaultSize, Height)
            {
                AnchorStyle = AnchorStyle.Right,
                Parent = this,
            };
            _vScroll.ValueChanged += OnVScrollValueChanged;
            _output = new RichTextBox
            {
                IsReadOnly = true,
                IsMultiline = true,
                BackgroundSelectedFlashSpeed = 0.0f,
                Location = new Vector2(2, _viewDropdown.Bottom + 2),
                Parent = this,
            };
            _output.TargetViewOffsetChanged += OnOutputTargetViewOffsetChanged;
            _output.TextChanged += OnOutputTextChanged;

            // Setup context menu
            _contextMenu = new ContextMenu();
            _contextMenu.AddButton("Clear log", Clear);
            _contextMenu.AddButton("Copy selection", _output.Copy);
            _contextMenu.AddButton("Select All", _output.SelectAll);

            // Bind events
            Editor.Options.OptionsChanged += OnEditorOptionsChanged;
        }

        private void OnViewButtonClicked()
        {
            var menu = new ContextMenu();

            var infoLogButton = menu.AddButton("Info");
            infoLogButton.AutoCheck = true;
            infoLogButton.Checked = (_logTypeShowMask & (int)LogType.Info) != 0;
            infoLogButton.Clicked += () => ToggleLogTypeShow(LogType.Info);

            var warningLogButton = menu.AddButton("Warning");
            warningLogButton.AutoCheck = true;
            warningLogButton.Checked = (_logTypeShowMask & (int)LogType.Warning) != 0;
            warningLogButton.Clicked += () => ToggleLogTypeShow(LogType.Warning);

            var errorLogButton = menu.AddButton("Error");
            errorLogButton.AutoCheck = true;
            errorLogButton.Checked = (_logTypeShowMask & (int)LogType.Error) != 0;
            errorLogButton.Clicked += () => ToggleLogTypeShow(LogType.Error);

            menu.Show(_viewDropdown.Parent, _viewDropdown.BottomLeft);
        }

        private void OnSearchBoxTextChanged()
        {
            Refresh();
        }

        private void ToggleLogTypeShow(LogType type)
        {
            _logTypeShowMask ^= (int)type;
            Refresh();
        }

        private void OnHScrollValueChanged()
        {
            var viewOffset = _output.ViewOffset;
            viewOffset.X = _hScroll.Value;
            _output.TargetViewOffset = viewOffset;
        }

        private void OnVScrollValueChanged()
        {
            var viewOffset = _output.ViewOffset;
            viewOffset.Y = _vScroll.Value;
            _output.TargetViewOffset = viewOffset;
        }

        private void OnOutputTargetViewOffsetChanged()
        {
            _hScroll.TargetValue = _output.TargetViewOffset.X;
            _vScroll.TargetValue = _output.TargetViewOffset.Y;
        }

        private void OnOutputTextChanged()
        {
            _hScroll.Maximum = _output.TextSize.X;
            _vScroll.Maximum = _output.TextSize.Y;
        }

        private void OnEditorOptionsChanged(EditorOptions options)
        {
            if (options.Interface.OutputLogTimestampsFormat == _timestampsFormats &&
                options.Interface.OutputLogShowLogType == _showLogType)
                return;

            _timestampsFormats = options.Interface.OutputLogTimestampsFormat;
            _showLogType = options.Interface.OutputLogShowLogType;
            Refresh();
        }

        /// <summary>
        /// Refreshes the log output.
        /// </summary>
        private void Refresh()
        {
            _textBufferCount = 0;
            _textBuffer.Clear();
            _isDirty = true;
        }

        /// <summary>
        /// Clears the log.
        /// </summary>
        public void Clear()
        {
            _entries?.Clear();
            Refresh();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            if (_output != null)
            {
                _searchBox.Width = Width - _viewDropdown.Right - 2 - ScrollBar.DefaultSize;
                _output.Size = new Vector2(_vScroll.X - 2, _hScroll.Y - 4 - _viewDropdown.Bottom);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;

            if (buttons == MouseButton.Right)
            {
                _contextMenu.Show(this, location);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            FlaxEngine.Profiler.BeginEvent("OutputLogWindow.Update");

            // Read the incoming log messages
            int logCount;
            do
            {
                logCount = Editor.Internal_ReadOutputLogs(_outMessages, _outLogTypes, _outLogTimes);

                for (int i = 0; i < logCount; i++)
                {
                    var entry = new Entry
                    {
                        Level = (LogType)_outLogTypes[i],
                        Time = new DateTime(_outLogTimes[i], DateTimeKind.Utc),
                        Message = _outMessages[i],
                    };
                    _entries.Add(entry);
                    _outMessages[i] = null;
                    _isDirty = true;
                }
            } while (logCount != 0);

            if (_isDirty)
            {
                _isDirty = false;

                // Generate the output log
                var entries = Utils.ExtractArrayFromList(_entries);
                var searchQuery = _searchBox.Text;
                for (int i = _textBufferCount; i < _entries.Count; i++)
                {
                    ref var entry = ref entries[i];
                    if (((int)entry.Level & _logTypeShowMask) == 0)
                        continue;
                    if (searchQuery.Length != 0 && entry.Message.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) == -1)
                        continue;

                    switch (_timestampsFormats)
                    {
                    case InterfaceOptions.TimestampsFormats.Utc:
                        _textBuffer.AppendFormat("[ {0} ]: ", entry.Time.ToUniversalTime());
                        break;
                    case InterfaceOptions.TimestampsFormats.LocalTime:
                        _textBuffer.AppendFormat("[ {0} ]: ", entry.Time);
                        break;
                    case InterfaceOptions.TimestampsFormats.TimeSinceStartup:
                        var diff = entry.Time - _startupTime;
                        _textBuffer.AppendFormat("[ {0:000}:{1:00}:{2:00}.{3:000} ]: ", diff.Hours, diff.Minutes, diff.Seconds, diff.Milliseconds);
                        break;
                    }

                    if (_showLogType)
                    {
                        _textBuffer.AppendFormat("[{0}] ", entry.Level);
                    }

                    _textBuffer.AppendLine(entry.Message);
                }

                // Update the output
                _output.Text = _textBuffer.ToString();
                _textBufferCount = _entries.Count;
            }

            base.Update(deltaTime);

            FlaxEngine.Profiler.EndEvent();
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            _startupTime = Time.StartupTime;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            if (IsDisposing)
                return;

            // Unbind events
            Editor.Options.OptionsChanged -= OnEditorOptionsChanged;

            // Cleanup
            _textBuffer.Clear();
            _textBuffer = null;
            _entries.Clear();
            _entries = null;
            _outMessages = null;
            _outLogTypes = null;
            _outLogTimes = null;

            // Unlink controls
            _viewDropdown = null;
            _searchBox = null;
            _hScroll = null;
            _vScroll = null;
            _output = null;
            _contextMenu = null;

            base.OnDestroy();
        }
    }
}
