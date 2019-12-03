// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
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

        private HScrollBar _hScroll;
        private VScrollBar _vScroll;
        private RichTextBox _output;

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
                Location = new Vector2(2, 2),
                Parent = this,
            };
            _output.TargetViewOffsetChanged += OnOutputTargetViewOffsetChanged;
            _output.TextChanged += OnOutputTextChanged;

            // Bind events
            Editor.Options.OptionsChanged += OnEditorOptionsChanged;
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
            _timestampsFormats = options.Interface.OutputLogTimestampsFormat;
            _showLogType = options.Interface.OutputLogShowLogType;
        }

        /// <summary>
        /// Clears the log.
        /// </summary>
        public void Clear()
        {
            _entries?.Clear();
            _isDirty = true;
            _textBufferCount = 0;
            _textBuffer.Clear();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            if (_output != null)
            {
                _output.Size = new Vector2(_vScroll.X - 2, _hScroll.Y - 2);
            }
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
                for (int i = _textBufferCount; i < _entries.Count; i++)
                {
                    ref var entry = ref entries[i];
                    if (((int)entry.Level & _logTypeShowMask) == 0)
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
            _hScroll = null;
            _vScroll = null;
            _output = null;

            base.OnDestroy();
        }
    }
}
