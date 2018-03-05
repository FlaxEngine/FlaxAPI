////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
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
            public Sprite Icon;

            public LogEntry(DebugLogWindow window, ref LogEntryDescription desc)
                : base(0, 0, 120, DefaultHeight)
            {
                DockStyle = DockStyle.Top;
                IsScrollable = true;

                _window = window;
                Desc = desc;
                switch (desc.Level)
                {
                    case LogType.Warning:
                        Group = LogGroup.Warning;
                        Icon = _window.IconWarning;
                        break;
                    case LogType.Log:
                        Group = LogGroup.Info;
                        Icon = _window.IconInfo;
                        break;
                    default:
                        Group = LogGroup.Error;
                        Icon = _window.IconError;
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

                // Icon
                Render2D.DrawSprite(Icon, new Rectangle(5, 8, 32, 32), Color.White);

                // Title
                Render2D.DrawText(style.FontMedium, Desc.Title, new Rectangle(38, 8, clientRect.Width - 40, clientRect.Height - 10), style.Foreground);
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
            public override bool OnKeyDown(Keys key)
            {
                // Up
                if (key == Keys.ArrowUp)
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
                else if (key == Keys.ArrowDown)
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
                else if (key == Keys.C && ParentWindow.GetKey(Keys.Control))
                {
                    Application.ClipboardText = Info;
                    return true;
                }

                return base.OnKeyDown(key);
            }

            public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
            {
                // Show the location
                ScriptsBuilder.OpenFile(Desc.LocationFile, Desc.LocationLine);

                return true;
            }
        }

        private readonly SplitPanel _split;
        private readonly Label _logInfo;
        private readonly Panel _entriesPanel;
	    private LogEntry _selected;
        private readonly int[] _logCountPerGroup = new int[(int)LogGroup.Max];
        private readonly Regex _logRegex = new Regex("at(.*) in (.*):(\\d*)");

        private readonly object _locker = new object();
        private bool _hasCompilationStarted;
        private readonly List<LogEntry> _pendingEntries = new List<LogEntry>(32);

	    private readonly ToolStripButton _clearOnPlayButton;
	    private readonly ToolStripButton _pauseOnErrorButton;
		private readonly ToolStripButton[] _groupButtons = new ToolStripButton[3];

        internal Sprite IconInfo;
        internal Sprite IconWarning;
        internal Sprite IconError;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public DebugLogWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
	        Title = "Debug";

            // Toolstrip
            var toolstrip = new ToolStrip(22);
            toolstrip.AddButton("Clear", Clear).LinkTooltip("Clears all log entries");
	        _clearOnPlayButton = (ToolStripButton)toolstrip.AddButton("Clear on Play").SetAutoCheck(true).SetChecked(true).LinkTooltip("Clears all log entries on enter playmode");
	        _pauseOnErrorButton = (ToolStripButton)toolstrip.AddButton("Pause on Error").SetAutoCheck(true).LinkTooltip("Performs auto pause on error");
            toolstrip.AddSeparator();
	        _groupButtons[0] = (ToolStripButton)toolstrip.AddButton(editor.UI.GetIcon("Error32"), () => UpdateLogTypeVisibility(LogGroup.Error, _groupButtons[0].Checked)).SetAutoCheck(true).SetChecked(true).LinkTooltip("Shows/hides error messages");
	        _groupButtons[1] = (ToolStripButton)toolstrip.AddButton(editor.UI.GetIcon("Warning32"), () => UpdateLogTypeVisibility(LogGroup.Warning, _groupButtons[1].Checked)).SetAutoCheck(true).SetChecked(true).LinkTooltip("Shows/hides warning messages");
	        _groupButtons[2] = (ToolStripButton)toolstrip.AddButton(editor.UI.GetIcon("Info32"), () => UpdateLogTypeVisibility(LogGroup.Info, _groupButtons[2].Checked)).SetAutoCheck(true).SetChecked(true).LinkTooltip("Shows/hides info messages");
            toolstrip.Parent = this;
            UpdateCount();

            // Split panel
            _split = new SplitPanel(Orientation.Vertical, ScrollBars.Vertical, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.8f,
                Parent = this
            };

            // Log detail info
            _logInfo = new Label(0, 0, 120, 1)
            {
                Parent = _split.Panel2,
                AutoHeight = true,
                Margin = new Margin(4),
                HorizontalAlignment = TextAlignment.Near
            };

            // Entries panel
            _entriesPanel = _split.Panel1;

            // Bind events
            Debug.Logger.LogHandler.SendLog += LogHandlerOnSendLog;
            Debug.Logger.LogHandler.SendExceptionLog += LogHandlerOnSendExceptionLog;
            ScriptsBuilder.CompilationBegin += OnCompilationBegin;
            ScriptsBuilder.CompilationError += OnCompilationError;
            ScriptsBuilder.CompilationWarning += OnCompilationWarning;
            GameCooker.Event += OnGameCookerEvent;
        }

        private void OnGameCookerEvent(GameCooker.EventType eventType, ref GameCooker.Options options)
        {
            if (eventType == GameCooker.EventType.BuildFailed)
                FocusOrShow();
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
            if (newEntry.Group == LogGroup.Error && _pauseOnErrorButton.Checked && Editor.StateMachine.CurrentState == Editor.StateMachine.PlayingState)
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

        private void UpdateLogTypeVisibility(LogGroup group, bool isVisible)
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

        private void UpdateCount()
        {
            UpdateCount((int)LogGroup.Error, " Errors");
            UpdateCount((int)LogGroup.Warning, " Warnings");
            UpdateCount((int)LogGroup.Info, " Messages");
        }

        private void UpdateCount(int group, string msg)
        {
            _groupButtons[group].Name = _logCountPerGroup[@group] + msg;
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

        private void RemoveEntries(bool isCompileResult)
        {
            _entriesPanel.IsLayoutLocked = true;

            Selected = null;
            for (int i = 0; i < _entriesPanel.ChildrenCount; i++)
            {
                // Clear all entries and non-error compile results
                if (_entriesPanel.GetChild(i) is LogEntry entry && (entry.Desc.IsCompileResult == isCompileResult || entry.Group != LogGroup.Error))
                {
                    _logCountPerGroup[(int)entry.Group]--;
                    entry.Dispose();
                    i--;
                }
            }
            UpdateCount();

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
                        newEntry.Visible = _groupButtons[(int)newEntry.Group].Checked;
                        newEntry.Parent = _entriesPanel;
                        _logCountPerGroup[(int)newEntry.Group]++;
                    }
                    _pendingEntries.Clear();
                    UpdateCount();
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
            // Cache entries icons
            IconInfo = Editor.UI.GetIcon("Info32");
            IconWarning = Editor.UI.GetIcon("Warning32");
            IconError = Editor.UI.GetIcon("Error32");

            // TODO: restore cached buttons 'Checked' state from editor prefs
        }

        /// <inheritdoc />
        public override void OnPlayBegin()
        {
            // Clear on Play
            if (_clearOnPlayButton.Checked)
            {
                Clear();
            }
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
            GameCooker.Event -= OnGameCookerEvent;

            base.OnDestroy();
        }

	    /// <inheritdoc />
	    public override bool UseLayoutData => true;

	    /// <inheritdoc />
	    public override void OnLayoutSerialize(XmlWriter writer)
	    {
		    writer.WriteAttributeString("Split", _split.SplitterValue.ToString());
	    }

	    /// <inheritdoc />
	    public override void OnLayoutDeserialize(XmlElement node)
	    {
		    float value1;

		    if (float.TryParse(node.GetAttribute("Split"), out value1))
			    _split.SplitterValue = value1;
	    }

	    /// <inheritdoc />
	    public override void OnLayoutDeserialize()
	    {
		    _split.SplitterValue = 0.8f;
	    }
	}
}
