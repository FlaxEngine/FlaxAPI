////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FlaxEditor.GUI;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Editor window used to show debug info, warning and error messages. Captures <see cref="Debug"/> messages.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public class DebugLogWindow : EditorWindow
    {
        /// <summary>
        /// Debug log entry description;
        /// </summary>
        public struct LogEntryDescription
        {
            /// <summary>
            /// The log level.
            /// </summary>
            public LogType Level;

            /// <summary>
            /// The message title.
            /// </summary>
            public string Title;

            /// <summary>
            /// The message description.
            /// </summary>
            public string Description;

            /// <summary>
            /// The target object hint id (don't store ref, object may be an actor that can be removed and restored later or sth).
            /// </summary>
            public Guid ContextObject;

            /// <summary>
            /// True if entry is scripts compilation result.
            /// </summary>
            public bool IsCompileResult;

            /// <summary>
            /// The location of the issue (file path).
            /// </summary>
            public string LocationFile;

            /// <summary>
            /// The location line number (zero or less to not use it);
            /// </summary>
            public int LocationLine;
        };

        private enum LogGroup
        {
            Error = 0,
            Warning,
            Info,
            Max
        };

        private class LogEntry : Control
        {
            /// <summary>
            /// The default height of the entries.
            /// </summary>
            public const float DefaultHeight = 48.0f;

            private DebugLogWindow _window;
            public LogGroup Group;
            public LogEntryDescription Desc;

            public LogEntry(DebugLogWindow window, ref LogEntryDescription desc)
                : base(true, 0, 0, 120, DefaultHeight)
            {
                DockStyle = DockStyle.Top;
                IsScrollable = true;

                _window = window;
                Desc = desc;
                switch (desc.Level)
                {
                    case LogType.Warning:
                        Group = LogGroup.Warning;
                        break;
                    case LogType.Log:
                        Group = LogGroup.Info;
                        break;
                    default:
                        Group = LogGroup.Error;
                        break;
                }
            }

            /// <summary>
            /// Gets the information text.
            /// </summary>
            public string Info => Desc.Title + '\n' + Desc.Description;

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                // Cache data
                var style = Style.Current;
                var index = IndexInParent;
                var clientRect = new Rectangle(Vector2.Zero, Size);

                // Background
                if (_window._selected == this)
                    Render2D.FillRectangle(clientRect, IsFocused ? style.BackgroundSelected : style.LightBackground);
                else if (IsMouseOver)
                    Render2D.FillRectangle(clientRect, style.BackgroundHighlighted);
                else if (index % 2 == 0)
                    Render2D.FillRectangle(clientRect, style.Background * 0.9f);

                // Title
                Render2D.DrawText(style.FontMedium, Desc.Title, new Rectangle(5, 5, clientRect.Width - 10, clientRect.Height - 10), style.Foreground);
            }

            /// <inheritdoc />
            public override void OnGotFocus()
            {
                base.OnGotFocus();

                _window.Selected = this;
            }

            /// <inheritdoc />
            protected override void OnVisibleChanged()
            {
                // Deselect on hide
                if (!Visible && _window.Selected == this)
                    _window.Selected = null;

                base.OnVisibleChanged();
            }

            /// <inheritdoc />
            public override bool OnKeyDown(KeyCode key)
            {
                // Up
                if (key == KeyCode.ArrowUp)
                {
                    int index = IndexInParent - 1;
                    if (index >= 1)// at 0 is scroll bar
                    {
                        var target = Parent.GetChild(index);
                        target.Focus();
                        ((Panel)Parent).ScrollViewTo(target);
                        return true;
                    }
                }
                // Down
                else if (key == KeyCode.ArrowDown)
                {
                    int index = IndexInParent + 1;
                    if (index < Parent.ChildrenCount)
                    {
                        var target = Parent.GetChild(index);
                        target.Focus();
                        ((Panel)Parent).ScrollViewTo(target);
                        return true;
                    }
                }
                // Ctrl+C
                else if (key == KeyCode.C && ParentWindow.GetKey(KeyCode.Control))
                {
                    Application.ClipboardText = Info;
                    return true;
                }

                return base.OnKeyDown(key);
            }

            public override bool OnMouseDoubleClick(Vector2 location, MouseButtons buttons)
            {
                // Show the location
                ScriptsBuilder.OpenFile(Desc.LocationFile, Desc.LocationLine);

                return true;
            }
        }

        private SplitPanel _splitPanel;
        private Label _logInfo;
        private Panel _entriesPanel;
        private ToolStrip _toolstrip;
        private LogEntry _selected;
        private int[] _logCountPerGroup = new int[(int)LogGroup.Max];
        private readonly Regex _logRegex = new Regex("at(.*) in (.*):(\\d*)");

        private readonly object _locker = new object();
        private bool _hasCompilationStarted;
        private readonly List<LogEntry> _pendingEntries = new List<LogEntry>(32);

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public DebugLogWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Debug";

            // Toolstrip
            _toolstrip = new ToolStrip(22);
            _toolstrip.ButtonClicked += OnTooltipButtonClicked;
            _toolstrip.AddButton(0, "Clear").LinkTooltip("Clears all log entries");
            _toolstrip.AddButton(1, "Clear on Play").SetAutoCheck(true).SetChecked(true).LinkTooltip("Clears all log entries on enter playmode");
            _toolstrip.AddButton(2, "Pause on Error").SetAutoCheck(true).LinkTooltip("Performs auto pause on error");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(10, Editor.UI.GetIcon("Error32")).SetAutoCheck(true).SetChecked(true).LinkTooltip("Shows/hides error messages");
            _toolstrip.AddButton(11, Editor.UI.GetIcon("Warning32")).SetAutoCheck(true).SetChecked(true).LinkTooltip("Shows/hides warning messages");
            _toolstrip.AddButton(12, Editor.UI.GetIcon("Info32")).SetAutoCheck(true).SetChecked(true).LinkTooltip("Shows/hides info messages");
            _toolstrip.Parent = this;
            updateCount();

            // Split panel
            _splitPanel = new SplitPanel(Orientation.Vertical, ScrollBars.Vertical, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.8f,
                Parent = this
            };

            // Log detail info
            _logInfo = new Label(0, 0, 120, 1)
            {
                Parent = _splitPanel.Panel2,
                AutoHeight = true,
                Margin = new Margin(4),
                HorizontalAlignment = TextAlignment.Near
            };

            // Entries panel
            _entriesPanel = _splitPanel.Panel1;

            // Bind events
            Debug.Logger.LogHandler.SendLog += LogHandlerOnSendLog;
            Debug.Logger.LogHandler.SendExceptionLog += LogHandlerOnSendExceptionLog;
            ScriptsBuilder.CompilationBegin += OnCompilationBegin;
            ScriptsBuilder.CompilationError += OnCompilationError;
            ScriptsBuilder.CompilationWarning += OnCompilationWarning;
            Editor.StateMachine.StateChanged += StateMachineOnStateChanged;
        }

        /// <summary>
        /// Clears the log.
        /// </summary>
        public void Clear()
        {
            if (_entriesPanel == null)
                return;

            // Remove normal entries, keep compilation results
            RemoveEntries(false);
        }

        /// <summary>
        /// Adds the specified log entry.
        /// </summary>
        /// <param name="desc">The log entry description.</param>
        public void Add(ref LogEntryDescription desc)
        {
            if (_entriesPanel == null)
                return;

            // Create new entry
            var newEntry = new LogEntry(this, ref desc);

            // Enqueue
            lock (_locker)
            {
                _pendingEntries.Add(newEntry);
            }

            // Pause on Error (we should do it as fast as possible)
            if (newEntry.Group == LogGroup.Error && _toolstrip.GetButton(2).Checked && Editor.StateMachine.CurrentState == Editor.StateMachine.PlayingState)
            {
                Editor.Simulation.RequestPausePlay();
            }
        }

        /// <summary>
        /// Gets or sets the selected entry.
        /// </summary>
        private LogEntry Selected
        {
            get => _selected;
            set
            {
                // Check if value will change
                if (_selected != value)
                {
                    // Select
                    _selected = value;
                    _logInfo.Text = _selected?.Info ?? string.Empty;
                }
            }
        }

        private void updateLogTypeVisibility(LogGroup group, bool isVisible)
        {
            _entriesPanel.IsLayoutLocked = true;

            var children = _entriesPanel.Children;
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is LogEntry logEntry && logEntry.Group == group)
                    logEntry.Visible = isVisible;
            }

            _entriesPanel.IsLayoutLocked = false;
            _entriesPanel.PerformLayout();
        }

        private void updateCount()
        {
            updateCount((int)LogGroup.Error, " Errors");
            updateCount((int)LogGroup.Warning, " Warnings");
            updateCount((int)LogGroup.Info, " Messages");
        }

        private void updateCount(int group, string msg)
        {
            _toolstrip.GetButton(10 + group).Text = _logCountPerGroup[@group] + msg;
        }

        private void OnTooltipButtonClicked(int id)
        {
            var button = _toolstrip.GetButton(id);
            switch (id)
            {
                case 0:
                    Clear();
                    break;

                case 10:
                    updateLogTypeVisibility(LogGroup.Error, button.Checked);
                    break;
                case 11:
                    updateLogTypeVisibility(LogGroup.Warning, button.Checked);
                    break;
                case 12:
                    updateLogTypeVisibility(LogGroup.Info, button.Checked);
                    break;
            }
        }

        private void LogHandlerOnSendLog(LogType level, string msg, Object o, string stackTrace)
        {
            LogEntryDescription desc = new LogEntryDescription
            {
                Level = level,
                Title = msg,
                ContextObject = o?.ID ?? Guid.Empty,
                IsCompileResult = false
            };

            if (!string.IsNullOrEmpty(stackTrace))
            {
                // Detect code location and remove leading internal stack trace part
                var matches = _logRegex.Matches(stackTrace);
                bool foundStart = false, noLocation = true;
                var fineStackTrace = new StringBuilder(stackTrace.Length);
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    if (foundStart)
                    {
                        if (noLocation)
                        {
                            desc.LocationFile = match.Groups[2].Value;
                            int.TryParse(match.Groups[3].Value, out desc.LocationLine);
                            noLocation = false;
                        }
                        fineStackTrace.AppendLine(match.Groups[0].Value);
                    }
                    else if (match.Groups[1].Value.Trim().StartsWith("FlaxEngine.Debug.Log", StringComparison.Ordinal))
                    {
                        foundStart = true;
                    }
                }
                desc.Description = fineStackTrace.ToString();
            }
            
            Add(ref desc);
        }

        private void LogHandlerOnSendExceptionLog(Exception exception, Object o)
        {
            LogEntryDescription desc = new LogEntryDescription
            {
                Level = LogType.Exception,
                Title = exception.Message,
                Description = exception.StackTrace,
                ContextObject = o?.ID ?? Guid.Empty,
                IsCompileResult = false
            };

            // Detect code location
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                var match = _logRegex.Match(exception.StackTrace);
                if (match.Success)
                {
                    desc.LocationFile = match.Groups[2].Value;
                    int.TryParse(match.Groups[3].Value, out desc.LocationLine);
                }
            }

            Add(ref desc);
        }

        private void OnCompilationBegin()
        {
            lock (_locker)
            {
                _hasCompilationStarted = true;
            }
        }

        private void OnCompilationError(string message, string file, int line)
        {
            LogEntryDescription desc = new LogEntryDescription
            {
                Level = LogType.Error,
                Title = message,
                Description = file + " at line " + line,
                IsCompileResult = true,
                LocationFile = file.Length > 0 ? StringUtils.CombinePaths(Globals.ProjectFolder, file) : string.Empty,
                LocationLine = line
            };

            Add(ref desc);

            // Focus window on errors (easier for user to get why scrips cannot compile)
            FocusOrShow();
        }

        private void OnCompilationWarning(string message, string file, int line)
        {
            LogEntryDescription desc = new LogEntryDescription
            {
                Level = LogType.Warning,
                Title = message,
                Description = file + " at line " + line,
                IsCompileResult = true,
                LocationFile = file.Length > 0 ? StringUtils.CombinePaths(Globals.ProjectFolder, file) : string.Empty,
                LocationLine = line
            };

            Add(ref desc);
        }

        private void StateMachineOnStateChanged()
        {
            // Clear on Play
            if (Editor.StateMachine.IsPlayMode && _toolstrip.GetButton(1).Checked)
            {
                Clear();
            }
        }

        private void RemoveEntries(bool isCompileResult)
        {
            _entriesPanel.IsLayoutLocked = true;

            Selected = null;
            for (int i = 0; i < _entriesPanel.ChildrenCount; i++)
            {
                if (_entriesPanel.GetChild(i) is LogEntry entry && entry.Desc.IsCompileResult == isCompileResult)
                {
                    _logCountPerGroup[(int)entry.Group]--;
                    entry.Dispose();
                    i--;
                }
            }
            updateCount();

            _entriesPanel.IsLayoutLocked = false;
            _entriesPanel.PerformLayout();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Check pending events (in a thread safe manner)
            lock (_locker)
            {
                if (_hasCompilationStarted)
                {
                    // Clear flag
                    _hasCompilationStarted = false;

                    // Remove old compilation results
                    RemoveEntries(true);
                }
                if (_pendingEntries.Count > 0)
                {
                    // TODO: we should provide max limit for entries count and remove if too many

                    // Check if user want's to scroll view by var (or is vewing earlier entry)
                    bool scrollView = (_entriesPanel.VScrollBar.Maximum - _entriesPanel.VScrollBar.TargetValue) < LogEntry.DefaultHeight * 1.5f;

                    // Add pending entries
                    LogEntry newEntry = null;
                    for (int i = 0; i < _pendingEntries.Count; i++)
                    {
                        newEntry = _pendingEntries[i];
                        newEntry.Visible = _toolstrip.GetButton(10 + (int)newEntry.Group).Checked;
                        newEntry.Parent = _entriesPanel;
                        _logCountPerGroup[(int)newEntry.Group]++;
                    }
                    _pendingEntries.Clear();
                    updateCount();
                    Assert.IsNotNull(newEntry);

                    // Scroll to the new entry
                    if (scrollView)
                        _entriesPanel.ScrollViewTo(newEntry);
                }
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            // TODO: restore cached buttons 'Checked' state from editor prefs
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Unbind events
            Debug.Logger.LogHandler.SendLog -= LogHandlerOnSendLog;
            Debug.Logger.LogHandler.SendExceptionLog -= LogHandlerOnSendExceptionLog;
            ScriptsBuilder.CompilationBegin -= OnCompilationBegin;
            ScriptsBuilder.CompilationError -= OnCompilationError;
            ScriptsBuilder.CompilationWarning -= OnCompilationWarning;
            Editor.StateMachine.StateChanged -= StateMachineOnStateChanged;

            base.OnDestroy();
        }
    }
}
