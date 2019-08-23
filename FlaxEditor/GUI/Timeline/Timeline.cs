// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.GUI.Input;
using FlaxEditor.GUI.Timeline.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline control that contains tracks section and headers. Can be used to create time-based media interface for camera tracks editing, audio mixing and events tracking.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public partial class Timeline : ContainerControl
    {
        private static readonly KeyValuePair<float, string>[] FPSValues =
        {
            new KeyValuePair<float, string>(12f, "12 fps"),
            new KeyValuePair<float, string>(15f, "15 fps"),
            new KeyValuePair<float, string>(23.976f, "23.97 (NTSC)"),
            new KeyValuePair<float, string>(24f, "24 fps"),
            new KeyValuePair<float, string>(25f, "25 (PAL)"),
            new KeyValuePair<float, string>(30f, "30 fps"),
            new KeyValuePair<float, string>(48f, "48 fps"),
            new KeyValuePair<float, string>(50f, "50 (PAL)"),
            new KeyValuePair<float, string>(60f, "60 fps"),
            new KeyValuePair<float, string>(100f, "100 fps"),
            new KeyValuePair<float, string>(120f, "120 fps"),
            new KeyValuePair<float, string>(240f, "240 fps"),
            new KeyValuePair<float, string>(0, "Custom"),
        };

        private sealed class TimeIntervalsHeader : ContainerControl
        {
            private Timeline _timeline;
            private bool _isLeftMouseButtonDown;

            public TimeIntervalsHeader(Timeline timeline)
            {
                _timeline = timeline;
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (base.OnMouseDown(location, buttons))
                    return true;

                if (buttons == MouseButton.Left)
                {
                    _isLeftMouseButtonDown = true;
                    _timeline._isMovingPositionHandle = true;
                    StartMouseCapture();
                    return true;
                }

                return false;
            }

            /// <inheritdoc />
            public override void OnMouseMove(Vector2 location)
            {
                base.OnMouseMove(location);

                if (_isLeftMouseButtonDown)
                {
                    Seek(ref location);
                }
            }

            private void Seek(ref Vector2 location)
            {
                if (_timeline.PlaybackState == PlaybackStates.Disabled)
                    return;

                var locationTimeline = PointToParent(_timeline, location);
                var locationTime = _timeline._backgroundArea.PointFromParent(_timeline, locationTimeline);
                var frame = (locationTime.X - StartOffset * 2.0f) / _timeline.Zoom / UnitsPerSecond * _timeline.FramesPerSecond;
                _timeline.OnSeek((int)frame);
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (base.OnMouseUp(location, buttons))
                    return true;

                if (buttons == MouseButton.Left && _isLeftMouseButtonDown)
                {
                    Seek(ref location);
                    EndMouseCapture();
                    return true;
                }

                return false;
            }

            /// <inheritdoc />
            public override void OnEndMouseCapture()
            {
                _isLeftMouseButtonDown = false;
                _timeline._isMovingPositionHandle = false;

                base.OnEndMouseCapture();
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                _timeline = null;

                base.Dispose();
            }
        }

        /// <summary>
        /// The base class for timeline properties proxy objects.
        /// </summary>
        /// <typeparam name="TTimeline">The type of the timeline.</typeparam>
        public abstract class ProxyBase<TTimeline>
        where TTimeline : Timeline
        {
            [HideInEditor, NoSerialize]
            public TTimeline Timeline;

            /// <summary>
            /// Gets or sets the total duration of the timeline in the frames amount.
            /// </summary>
            [EditorDisplay("General"), EditorOrder(10), Limit(1), Tooltip("Total duration of the timeline event the frames amount.")]
            public int DurationFrames
            {
                get => Timeline.DurationFrames;
                set => Timeline.DurationFrames = value;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ProxyBase{TTimeline}"/> class.
            /// </summary>
            /// <param name="track">The timeline.</param>
            protected ProxyBase(TTimeline timeline)
            {
                Timeline = timeline ?? throw new ArgumentNullException(nameof(timeline));
            }
        }

        /// <summary>
        /// The time axis value formatting modes.
        /// </summary>
        public enum TimeShowModes
        {
            /// <summary>
            /// The frame numbers.
            /// </summary>
            Frames,

            /// <summary>
            /// The seconds amount.
            /// </summary>
            Seconds,

            /// <summary>
            /// The time.
            /// </summary>
            Time,
        }

        /// <summary>
        /// The timeline animation playback states.
        /// </summary>
        public enum PlaybackStates
        {
            /// <summary>
            /// The timeline animation feature is disabled.
            /// </summary>
            Disabled,

            /// <summary>
            /// The timeline animation is stopped.
            /// </summary>
            Stopped,

            /// <summary>
            /// The timeline animation is playing.
            /// </summary>
            Playing,

            /// <summary>
            /// The timeline animation is paused.
            /// </summary>
            Paused,
        }

        /// <summary>
        /// The timeline playback buttons types.
        /// </summary>
        [Flags]
        public enum PlaybackButtons
        {
            /// <summary>
            /// No buttons.
            /// </summary>
            None = 0,

            /// <summary>
            /// The play/pause button.
            /// </summary>
            Play = 1,

            /// <summary>
            /// The stop button.
            /// </summary>
            Stop = 2,
        }

        /// <summary>
        /// The header top area height (in pixels).
        /// </summary>
        public static readonly float HeaderTopAreaHeight = 22.0f;

        /// <summary>
        /// The timeline units per second (on time axis).
        /// </summary>
        public static readonly float UnitsPerSecond = 100.0f;

        /// <summary>
        /// The start offset for the timeline (on time axis).
        /// </summary>
        public static readonly float StartOffset = 50.0f;

        private bool _isModified;
        private bool _isChangingFps;
        private float _framesPerSecond = 30.0f;
        private int _durationFrames = 30 * 5;
        private int _currentFrame;
        private TimeShowModes _timeShowMode = TimeShowModes.Frames;
        private PlaybackStates _state = PlaybackStates.Disabled;

        /// <summary>
        /// The tracks collection.
        /// </summary>
        protected readonly List<Track> _tracks = new List<Track>();

        private SplitPanel _splitter;
        private TimeIntervalsHeader _timeIntervalsHeader;
        private ContainerControl _backgroundScroll;
        private Background _background;
        private Panel _backgroundArea;
        private TimelineEdge _leftEdge;
        private TimelineEdge _rightEdge;
        private Button _addTrackButton;
        private ComboBox _fpsComboBox;
        private Button _viewButton;
        private FloatValueBox _fpsCustomValue;
        private Panel _tracksPanelArea;
        private VerticalPanel _tracksPanel;
        private Image _playbackStop;
        private Image _playbackPlay;
        private Label _noTracksLabel;
        private PositionHandle _positionHandle;
        private bool _isRightMouseButtonDown;
        private Vector2 _rightMouseButtonDownPos;
        private float _zoom = 1.0f;
        private bool _isMovingPositionHandle;

        /// <summary>
        /// Gets or sets the current time showing mode.
        /// </summary>
        public TimeShowModes TimeShowMode
        {
            get => _timeShowMode;
            set
            {
                if (_timeShowMode == value)
                    return;

                _timeShowMode = value;
                TimeShowModeChanged?.Invoke();
            }
        }

        /// <summary>
        /// Occurs when current time showing mode gets changed.
        /// </summary>
        public event Action TimeShowModeChanged;

        /// <summary>
        /// Gets or sets the current animation playback time position (frame number).
        /// </summary>
        public int CurrentFrame
        {
            get => _currentFrame;
            set
            {
                if (_currentFrame == value)
                    return;

                _currentFrame = value;
                UpdatePositionHandle();
                for (var i = 0; i < _tracks.Count; i++)
                {
                    _tracks[i].OnTimelineCurrentFrameChanged(_currentFrame);
                }
                CurrentFrameChanged?.Invoke();
            }
        }

        /// <summary>
        /// Gets the current animation time position (in seconds).
        /// </summary>
        public float CurrentTime => _currentFrame / _framesPerSecond;

        /// <summary>
        /// Occurs when current playback animation frame gets changed.
        /// </summary>
        public event Action CurrentFrameChanged;

        /// <summary>
        /// Gets or sets the amount of frames per second of the timeline animation.
        /// </summary>
        public float FramesPerSecond
        {
            get => _framesPerSecond;
            set
            {
                value = Mathf.Clamp(value, 0.1f, 1000.0f);
                if (Mathf.NearEqual(_framesPerSecond, value))
                    return;

                var oldDuration = Duration;
                var oldValue = _framesPerSecond;

                _isChangingFps = true;
                _framesPerSecond = value;
                if (_fpsComboBox != null)
                {
                    int index = FPSValues.Length - 1;
                    for (int i = 0; i < FPSValues.Length; i++)
                    {
                        if (Mathf.NearEqual(FPSValues[i].Key, value))
                        {
                            index = i;
                            break;
                        }
                    }
                    _fpsComboBox.SelectedIndex = index;
                }
                if (_fpsCustomValue != null)
                {
                    _fpsCustomValue.Value = value;
                }
                _isChangingFps = false;
                FramesPerSecondChanged?.Invoke();

                // Preserve media events and duration
                for (var i = 0; i < _tracks.Count; i++)
                {
                    _tracks[i].OnTimelineFpsChanged(oldValue, value);
                }
                Duration = oldDuration;

                MarkAsEdited();
                ArrangeTracks();
                UpdatePositionHandle();
            }
        }

        /// <summary>
        /// Occurs when frames per second gets changed.
        /// </summary>
        public event Action FramesPerSecondChanged;

        /// <summary>
        /// Gets or sets the timeline animation duration in frames.
        /// </summary>
        /// <seealso cref="FramesPerSecond"/>
        public int DurationFrames
        {
            get => _durationFrames;
            set
            {
                value = Mathf.Max(value, 1);
                if (_durationFrames == value)
                    return;

                _durationFrames = value;
                ArrangeTracks();
                DurationFramesChanged?.Invoke();
            }
        }

        /// <summary>
        /// Gets the timeline animation duration in seconds.
        /// </summary>
        /// <seealso cref="FramesPerSecond"/>
        public float Duration
        {
            get => _durationFrames / FramesPerSecond;
            set => DurationFrames = (int)(value * FramesPerSecond);
        }

        /// <summary>
        /// Occurs when timeline duration gets changed.
        /// </summary>
        public event Action DurationFramesChanged;

        /// <summary>
        /// Gets the collection of the tracks added to this timeline (read-only list).
        /// </summary>
        public IReadOnlyList<Track> Tracks => _tracks;

        /// <summary>
        /// Occurs when tracks collection gets changed.
        /// </summary>
        public event Action TracksChanged;

        /// <summary>
        /// Gets a value indicating whether this timeline was modified by the user (needs saving and flushing with data source).
        /// </summary>
        public bool IsModified => _isModified;

        /// <summary>
        /// Occurs when timeline gets modified (track edited, media moved, etc.).
        /// </summary>
        public event Action Modified;

        /// <summary>
        /// Occurs when timeline starts playing animation.
        /// </summary>
        public event Action Play;

        /// <summary>
        /// Occurs when timeline pauses playing animation.
        /// </summary>
        public event Action Pause;

        /// <summary>
        /// Occurs when timeline stops playing animation.
        /// </summary>
        public event Action Stop;

        /// <summary>
        /// Gets the splitter.
        /// </summary>
        public SplitPanel Splitter => _splitter;

        /// <summary>
        /// The track archetypes.
        /// </summary>
        public readonly List<TrackArchetype> TrackArchetypes = new List<TrackArchetype>(32);

        /// <summary>
        /// The selected tracks.
        /// </summary>
        public readonly List<Track> SelectedTracks = new List<Track>();

        /// <summary>
        /// The selected media events.
        /// </summary>
        public readonly List<Media> SelectedMedia = new List<Media>();

        /// <summary>
        /// Occurs when any collection of the selected objects in the timeline gets changed.
        /// </summary>
        public event Action SelectionChanged;

        /// <summary>
        /// Gets the media controls background panel (with scroll bars).
        /// </summary>
        public Panel MediaBackground => _backgroundArea;

        /// <summary>
        /// Gets the media controls parent panel.
        /// </summary>
        public Background MediaPanel => _background;

        /// <summary>
        /// Gets the state of the timeline animation playback.
        /// </summary>
        public PlaybackStates PlaybackState
        {
            get => _state;
            protected set
            {
                if (_state == value)
                    return;

                _state = value;

                // Update buttons UI
                var icons = Editor.Instance.Icons;
                switch (value)
                {
                case PlaybackStates.Disabled:
                    if (_playbackStop != null)
                    {
                        _playbackStop.Visible = false;
                    }
                    if (_playbackPlay != null)
                    {
                        _playbackPlay.Visible = false;
                    }
                    if (_positionHandle != null)
                    {
                        _positionHandle.Visible = false;
                    }
                    break;
                case PlaybackStates.Stopped:
                    if (_playbackStop != null)
                    {
                        _playbackStop.Visible = true;
                        _playbackStop.Enabled = false;
                    }
                    if (_playbackPlay != null)
                    {
                        _playbackPlay.Visible = true;
                        _playbackPlay.Enabled = true;
                        _playbackPlay.Brush = new SpriteBrush(icons.Play32);
                        _playbackPlay.Tag = false;
                    }
                    if (_positionHandle != null)
                    {
                        _positionHandle.Visible = true;
                    }
                    break;
                case PlaybackStates.Playing:
                    if (_playbackStop != null)
                    {
                        _playbackStop.Visible = true;
                        _playbackStop.Enabled = true;
                    }
                    if (_playbackPlay != null)
                    {
                        _playbackPlay.Visible = true;
                        _playbackPlay.Enabled = true;
                        _playbackPlay.Brush = new SpriteBrush(icons.Pause32);
                        _playbackPlay.Tag = true;
                    }
                    if (_positionHandle != null)
                    {
                        _positionHandle.Visible = true;
                    }
                    break;
                case PlaybackStates.Paused:
                    if (_playbackStop != null)
                    {
                        _playbackStop.Visible = true;
                        _playbackStop.Enabled = true;
                    }
                    if (_playbackPlay != null)
                    {
                        _playbackPlay.Visible = true;
                        _playbackPlay.Enabled = true;
                        _playbackPlay.Brush = new SpriteBrush(icons.Play32);
                        _playbackPlay.Tag = false;
                    }
                    if (_positionHandle != null)
                    {
                        _positionHandle.Visible = true;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        /// <summary>
        /// The timeline properties editing proxy object. Assign it to add timeline properties editing support.
        /// </summary>
        public object PropertiesEditObject;

        /// <summary>
        /// Gets or sets the timeline view zoom.
        /// </summary>
        public float Zoom
        {
            get => _zoom;
            set
            {
                value = Mathf.Clamp(value, 0.02f, 10.0f);
                if (Mathf.NearEqual(_zoom, value))
                    return;

                _zoom = value;

                foreach (var track in _tracks)
                {
                    track.OnTimelineZoomChanged();
                }

                ArrangeTracks();
                UpdatePositionHandle();
            }
        }

        /// <summary>
        /// Gets a value indicating whether user is moving position handle (seeking).
        /// </summary>
        public bool IsMovingPositionHandle => _isMovingPositionHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timeline"/> class.
        /// </summary>
        /// <param name="playbackButtons">The playback buttons to use.</param>
        /// <param name="canChangeFps">True if user can modify the timeline FPS, otherwise it will be fixed or controlled from the code.</param>
        public Timeline(PlaybackButtons playbackButtons, bool canChangeFps = true)
        {
            AutoFocus = false;
            _splitter = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None)
            {
                SplitterValue = 0.2f,
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            var headerTopArea = new ContainerControl(0, 0, _splitter.Panel1.Width, HeaderTopAreaHeight)
            {
                AutoFocus = false,
                BackgroundColor = Style.Current.LightBackground,
                DockStyle = DockStyle.Top,
                Parent = _splitter.Panel1
            };
            var addTrackButtonWidth = 40.0f;
            _addTrackButton = new Button(2, 2, addTrackButtonWidth, 18.0f)
            {
                TooltipText = "Add new tracks to the timeline",
                Text = "Add",
                Parent = headerTopArea
            };
            _addTrackButton.Clicked += OnAddTrackButtonClicked;
            var viewButtonWidth = 40.0f;
            _viewButton = new Button(_addTrackButton.Right + 2, 2, viewButtonWidth, 18.0f)
            {
                TooltipText = "Change timeline view options",
                Text = "View",
                Parent = headerTopArea
            };
            _viewButton.Clicked += OnViewButtonClicked;

            if (canChangeFps)
            {
                var changeFpsWidth = 70.0f;
                _fpsComboBox = new ComboBox(_viewButton.Right + 2, 2, changeFpsWidth)
                {
                    TooltipText = "Change timeline frames per second",
                    Parent = headerTopArea
                };
                for (int i = 0; i < FPSValues.Length; i++)
                {
                    _fpsComboBox.AddItem(FPSValues[i].Value);
                    if (Mathf.NearEqual(_framesPerSecond, FPSValues[i].Key))
                        _fpsComboBox.SelectedIndex = i;
                }
                if (_fpsComboBox.SelectedIndex == -1)
                    _fpsComboBox.SelectedIndex = FPSValues.Length - 1;
                _fpsComboBox.SelectedIndexChanged += OnFpsSelectedIndexChanged;
                _fpsComboBox.PopupShowing += OnFpsPopupShowing;
            }

            var playbackButtonsSize = 24.0f;
            var icons = Editor.Instance.Icons;
            var playbackButtonsArea = new ContainerControl(0, 0, 100, playbackButtonsSize)
            {
                BackgroundColor = Style.Current.LightBackground,
                DockStyle = DockStyle.Bottom,
                Parent = _splitter.Panel1
            };
            var playbackButtonsPanel = new ContainerControl(0, 0, 0, playbackButtonsSize)
            {
                AnchorStyle = AnchorStyle.Center,
                Parent = playbackButtonsArea
            };
            if ((playbackButtons & PlaybackButtons.Stop) == PlaybackButtons.Stop)
            {
                _playbackStop = new Image(playbackButtonsPanel.Width, 0, playbackButtonsSize, playbackButtonsSize)
                {
                    TooltipText = "Stop playback",
                    Brush = new SpriteBrush(icons.Stop32),
                    Visible = false,
                    Enabled = false,
                    Parent = playbackButtonsPanel
                };
                _playbackStop.Clicked += OnStopClicked;
                playbackButtonsPanel.Width += playbackButtonsSize;
            }
            if ((playbackButtons & PlaybackButtons.Play) == PlaybackButtons.Play)
            {
                _playbackPlay = new Image(playbackButtonsPanel.Width, 0, playbackButtonsSize, playbackButtonsSize)
                {
                    TooltipText = "Play/pause playback",
                    Brush = new SpriteBrush(icons.Play32),
                    Visible = false,
                    Tag = false, // Set to true if image is set to Pause, false if Play
                    Parent = playbackButtonsPanel
                };
                _playbackPlay.Clicked += OnPlayClicked;
                playbackButtonsPanel.Width += playbackButtonsSize;
            }

            _tracksPanelArea = new Panel(ScrollBars.Vertical)
            {
                Size = new Vector2(_splitter.Panel1.Width, _splitter.Panel1.Height - playbackButtonsSize - HeaderTopAreaHeight),
                DockStyle = DockStyle.Fill,
                Parent = _splitter.Panel1
            };
            _tracksPanel = new VerticalPanel
            {
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                Parent = _tracksPanelArea
            };
            _noTracksLabel = new Label
            {
                AnchorStyle = AnchorStyle.Center,
                TextColor = Color.Gray,
                TextColorHighlighted = Color.Gray * 1.1f,
                Text = "No tracks",
                Parent = _tracksPanelArea
            };

            _timeIntervalsHeader = new TimeIntervalsHeader(this)
            {
                AutoFocus = false,
                BackgroundColor = Style.Current.Background.RGBMultiplied(0.9f),
                Height = HeaderTopAreaHeight,
                DockStyle = DockStyle.Top,
                Parent = _splitter.Panel2
            };
            _backgroundArea = new Panel(ScrollBars.Both)
            {
                ClipChildren = false,
                BackgroundColor = Style.Current.Background.RGBMultiplied(0.7f),
                DockStyle = DockStyle.Fill,
                Parent = _splitter.Panel2
            };
            _backgroundScroll = new ContainerControl
            {
                AutoFocus = false,
                ClipChildren = false,
                Height = 0,
                Parent = _backgroundArea
            };
            _background = new Background(this)
            {
                ClipChildren = false,
                Height = 0,
                Parent = _backgroundArea
            };
            _leftEdge = new TimelineEdge(this, true, false)
            {
                Height = 0,
                Parent = _backgroundArea
            };
            _rightEdge = new TimelineEdge(this, false, true)
            {
                Height = 0,
                Parent = _backgroundArea
            };
            _positionHandle = new PositionHandle(this)
            {
                ClipChildren = false,
                Visible = false,
                Parent = _backgroundArea,
            };
            UpdatePositionHandle();
            PlaybackState = PlaybackStates.Disabled;
        }

        private void UpdatePositionHandle()
        {
            var handleWidth = 12.0f;
            _positionHandle.Bounds = new Rectangle(
                StartOffset * 2.0f - handleWidth * 0.5f + _currentFrame / _framesPerSecond * UnitsPerSecond * Zoom,
                HeaderTopAreaHeight * -0.5f,
                handleWidth,
                HeaderTopAreaHeight * 0.5f);
        }

        private void OnFpsPopupShowing(ComboBox comboBox)
        {
            if (_fpsCustomValue == null)
            {
                _fpsCustomValue = new FloatValueBox(_framesPerSecond, 63, 295, 45.0f, 0.1f, 1000.0f, 0.1f)
                {
                    Parent = comboBox.Popup
                };
                _fpsCustomValue.ValueChanged += OnFpsCustomValueChanged;
            }
            _fpsCustomValue.Value = FramesPerSecond;
        }

        private void OnFpsCustomValueChanged()
        {
            if (_isChangingFps || _fpsComboBox.SelectedIndex != FPSValues.Length - 1)
                return;

            // Custom value
            FramesPerSecond = _fpsCustomValue.Value;
        }

        private void OnFpsSelectedIndexChanged(ComboBox comboBox)
        {
            if (_isChangingFps)
                return;

            if (comboBox.SelectedIndex == FPSValues.Length - 1)
            {
                // Custom value
                FramesPerSecond = _fpsCustomValue.Value;
            }
            else
            {
                // Predefined value
                FramesPerSecond = FPSValues[comboBox.SelectedIndex].Key;
            }
        }

        private void OnAddTrackButtonClicked()
        {
            // TODO: maybe cache context menu object?
            var menu = new ContextMenu.ContextMenu();
            for (int i = 0; i < TrackArchetypes.Count; i++)
            {
                var archetype = TrackArchetypes[i];
                if (archetype.DisableSpawnViaGUI)
                    continue;

                var button = menu.AddButton(archetype.Name, OnAddTrackOptionClicked);
                button.Tag = archetype;
                button.Icon = archetype.Icon;
            }
            menu.Show(_addTrackButton.Parent, _addTrackButton.BottomLeft);
        }

        private void OnAddTrackOptionClicked(ContextMenuButton button)
        {
            var archetype = (TrackArchetype)button.Tag;
            AddTrack(archetype);
            MarkAsEdited();
        }

        private void OnViewButtonClicked()
        {
            // TODO: maybe cache context menu object?
            // TODO: maybe add some customization options/events to allow editor plugins for extending this part?
            var menu = new ContextMenu.ContextMenu();

            var showTimeAs = menu.AddChildMenu("Show time as");
            showTimeAs.ContextMenu.AddButton("Frames", () => TimeShowMode = TimeShowModes.Frames).Checked = TimeShowMode == TimeShowModes.Frames;
            showTimeAs.ContextMenu.AddButton("Seconds", () => TimeShowMode = TimeShowModes.Seconds).Checked = TimeShowMode == TimeShowModes.Seconds;
            showTimeAs.ContextMenu.AddButton("Time", () => TimeShowMode = TimeShowModes.Time).Checked = TimeShowMode == TimeShowModes.Time;

            menu.Show(_addTrackButton.Parent, _addTrackButton.BottomLeft);
        }

        private void OnStopClicked(Image stop, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                OnStop();
            }
        }

        private void OnPlayClicked(Image play, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                if ((bool)play.Tag)
                    OnPause();
                else
                    OnPlay();
            }
        }

        /// <summary>
        /// Called when animation should stop.
        /// </summary>
        public virtual void OnStop()
        {
            Stop?.Invoke();
            PlaybackState = PlaybackStates.Stopped;
        }

        /// <summary>
        /// Called when animation should play.
        /// </summary>
        public virtual void OnPlay()
        {
            Play?.Invoke();
            PlaybackState = PlaybackStates.Playing;
        }

        /// <summary>
        /// Called when animation should pause.
        /// </summary>
        public virtual void OnPause()
        {
            Pause?.Invoke();
            PlaybackState = PlaybackStates.Paused;
        }

        /// <summary>
        /// Called when animation playback position should be changed.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public virtual void OnSeek(int frame)
        {
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="archetype">The archetype.</param>
        /// <returns>The created track object.</returns>
        public Track AddTrack(TrackArchetype archetype)
        {
            var options = new TrackCreateOptions
            {
                Archetype = archetype,
                Mute = false,
            };
            var track = archetype.Create(options);
            if (track != null)
            {
                // Ensure name is unique
                int idx = 1;
                var name = track.Name;
                while (!IsTrackNameValid(track.Name))
                {
                    track.Name = string.Format("{0} {1}", name, idx++);
                }

                AddTrack(track);
            }
            return track;
        }

        /// <summary>
        /// Loads the timeline data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="stream">The input stream.</param>
        protected virtual void LoadTimelineData(int version, BinaryReader stream)
        {
        }

        /// <summary>
        /// Saves the timeline data.
        /// </summary>
        /// <param name="stream">The output stream.</param>
        protected virtual void SaveTimelineData(BinaryWriter stream)
        {
        }

        /// <summary>
        /// Loads the timeline from the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void Load(byte[] data)
        {
            Clear();

            using (var memory = new MemoryStream(data))
            using (var stream = new BinaryReader(memory))
            {
                int version = stream.ReadInt32();
                switch (version)
                {
                case 1:
                {
                    // [Deprecated on 23.07.2019, expires on 27.04.2020]

                    // Load properties
                    FramesPerSecond = stream.ReadSingle();
                    DurationFrames = stream.ReadInt32();
                    LoadTimelineData(version, stream);

                    // Load tracks
                    int tracksCount = stream.ReadInt32();
                    _tracks.Capacity = Math.Max(_tracks.Capacity, tracksCount);
                    for (int i = 0; i < tracksCount; i++)
                    {
                        var type = stream.ReadByte();
                        var flag = stream.ReadByte();
                        Track track = null;
                        var mute = (flag & 1) == 1;
                        for (int j = 0; j < TrackArchetypes.Count; j++)
                        {
                            if (TrackArchetypes[j].TypeId == type)
                            {
                                var options = new TrackCreateOptions
                                {
                                    Archetype = TrackArchetypes[j],
                                    Mute = mute,
                                };
                                track = TrackArchetypes[j].Create(options);
                                break;
                            }
                        }
                        if (track == null)
                            throw new Exception("Unknown timeline track type " + type);
                        int parentIndex = stream.ReadInt32();
                        int childrenCount = stream.ReadInt32();
                        track.Name = Utilities.Utils.ReadStr(stream, -13);
                        track.Tag = parentIndex;

                        track.Archetype.Load(version, track, stream);

                        AddLoadedTrack(track);
                    }
                    break;
                }
                case 2:
                {
                    // Load properties
                    FramesPerSecond = stream.ReadSingle();
                    DurationFrames = stream.ReadInt32();
                    LoadTimelineData(version, stream);

                    // Load tracks
                    int tracksCount = stream.ReadInt32();
                    _tracks.Capacity = Math.Max(_tracks.Capacity, tracksCount);
                    for (int i = 0; i < tracksCount; i++)
                    {
                        var type = stream.ReadByte();
                        var flag = stream.ReadByte();
                        Track track = null;
                        var mute = (flag & 1) == 1;
                        for (int j = 0; j < TrackArchetypes.Count; j++)
                        {
                            if (TrackArchetypes[j].TypeId == type)
                            {
                                var options = new TrackCreateOptions
                                {
                                    Archetype = TrackArchetypes[j],
                                    Mute = mute,
                                };
                                track = TrackArchetypes[j].Create(options);
                                break;
                            }
                        }
                        if (track == null)
                            throw new Exception("Unknown timeline track type " + type);
                        int parentIndex = stream.ReadInt32();
                        int childrenCount = stream.ReadInt32();
                        track.Name = Utilities.Utils.ReadStr(stream, -13);
                        track.Tag = parentIndex;
                        track.Color = stream.ReadColor32();

                        track.Archetype.Load(version, track, stream);

                        AddLoadedTrack(track);
                    }
                    break;
                }
                default: throw new Exception("Unknown timeline version " + version);
                }

                for (int i = 0; i < _tracks.Count; i++)
                {
                    var parentIndex = (int)_tracks[i].Tag;
                    _tracks[i].Tag = null;
                    if (parentIndex != -1)
                        _tracks[i].ParentTrack = _tracks[parentIndex];
                }
                for (int i = 0; i < _tracks.Count; i++)
                {
                    _tracks[i].OnLoaded();
                }
            }

            ArrangeTracks();
            ClearEditedFlag();
        }

        /// <summary>
        /// Saves the timeline data.
        /// </summary>
        /// <returns>The saved timeline data.</returns>
        public virtual byte[] Save()
        {
            // Serialize timeline to stream
            using (var memory = new MemoryStream(512))
            using (var stream = new BinaryWriter(memory))
            {
                // Save properties
                stream.Write(2);
                stream.Write(FramesPerSecond);
                stream.Write(DurationFrames);
                SaveTimelineData(stream);

                // Save tracks
                int tracksCount = Tracks.Count;
                stream.Write(tracksCount);
                for (int i = 0; i < tracksCount; i++)
                {
                    var track = Tracks[i];

                    stream.Write((byte)track.Archetype.TypeId);
                    byte flag = 0;
                    if (track.Mute)
                        flag |= 1;
                    stream.Write(flag);
                    stream.Write(_tracks.IndexOf(track.ParentTrack));
                    stream.Write(track.SubTracks.Count);
                    Utilities.Utils.WriteStr(stream, track.Name, -13);
                    stream.Write((Color32)track.Color);
                    track.Archetype.Save(track, stream);
                }

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public virtual void AddTrack(Track track)
        {
            _tracks.Add(track);
            track.OnTimelineChanged(this);
            track.Parent = _tracksPanel;

            OnTracksChanged();
            track.OnSpawned();

            _tracksPanelArea.ScrollViewTo(track);

            MarkAsEdited();
        }

        /// <summary>
        /// Adds the loaded track. Does not handle any UI updates.
        /// </summary>
        /// <param name="track">The track.</param>
        protected void AddLoadedTrack(Track track)
        {
            _tracks.Add(track);
            track.OnTimelineChanged(this);
            track.Parent = _tracksPanel;
        }

        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public virtual void RemoveTrack(Track track)
        {
            track.Parent = null;
            track.OnTimelineChanged(null);
            _tracks.Remove(track);

            OnTracksChanged();
        }

        /// <summary>
        /// Called when collection of the tracks gets changed.
        /// </summary>
        protected virtual void OnTracksChanged()
        {
            TracksChanged?.Invoke();
            ArrangeTracks();
        }

        /// <summary>
        /// Selects the specified track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="addToSelection">If set to <c>true</c> track will be added to selection, otherwise will clear selection before.</param>
        public void Select(Track track, bool addToSelection)
        {
            if (SelectedTracks.Contains(track) && (addToSelection || (SelectedTracks.Count == 1 && SelectedMedia.Count == 0)))
                return;

            if (!addToSelection)
            {
                SelectedTracks.Clear();
                SelectedMedia.Clear();
            }
            SelectedTracks.Add(track);
            OnSelectionChanged();
        }

        /// <summary>
        /// Deselects the specified track.
        /// </summary>
        /// <param name="track">The track.</param>
        public void Deselect(Track track)
        {
            if (!SelectedTracks.Contains(track))
                return;

            SelectedTracks.Remove(track);
            OnSelectionChanged();
        }

        /// <summary>
        /// Selects the specified media event.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="addToSelection">If set to <c>true</c> track will be added to selection, otherwise will clear selection before.</param>
        public void Select(Media media, bool addToSelection)
        {
            if (SelectedMedia.Contains(media) && (addToSelection || (SelectedTracks.Count == 0 && SelectedMedia.Count == 1)))
                return;

            if (!addToSelection)
            {
                SelectedTracks.Clear();
                SelectedMedia.Clear();
            }
            SelectedMedia.Add(media);
            OnSelectionChanged();
        }

        /// <summary>
        /// Deselects the specified media event.
        /// </summary>
        /// <param name="media">The media.</param>
        public void Deselect(Media media)
        {
            if (!SelectedMedia.Contains(media))
                return;

            SelectedMedia.Remove(media);
            OnSelectionChanged();
        }

        /// <summary>
        /// Deselects all media and tracks.
        /// </summary>
        public void Deselect()
        {
            if (SelectedMedia.Count == 0 && SelectedTracks.Count == 0)
                return;

            SelectedTracks.Clear();
            SelectedMedia.Clear();
            OnSelectionChanged();
        }

        /// <summary>
        /// Called when selection gets changed.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            SelectionChanged?.Invoke();
        }

        private void GetTracks(Track track, List<Track> tracks)
        {
            tracks.Add(track);
            tracks.AddRange(track.SubTracks);
        }

        /// <summary>
        /// Deletes the selected tracks/media events.
        /// </summary>
        public void DeleteSelection()
        {
            if (SelectedMedia.Count > 0)
            {
                throw new NotImplementedException("TODO: removing selected media events");
            }

            if (SelectedTracks.Count > 0)
            {
                // Delete selected tracks
                var tracks = new List<Track>(SelectedTracks.Count);
                for (int i = 0; i < SelectedTracks.Count; i++)
                {
                    var track = SelectedTracks[i];
                    track.ParentTrack = null;
                    GetTracks(track, tracks);
                }
                SelectedTracks.Clear();
                for (int i = 0; i < tracks.Count; i++)
                {
                    OnDeleteTrack(tracks[i]);
                }
                OnTracksChanged();
                MarkAsEdited();
            }
        }

        /// <summary>
        /// Deletes the tracks.
        /// </summary>
        /// <param name="track">The track to delete (and its sub tracks).</param>
        public void Delete(Track track)
        {
            if (track == null)
                throw new ArgumentNullException();

            // Delete tracks
            var tracks = new List<Track>(SelectedTracks.Count);
            track.ParentTrack = null;
            GetTracks(track, tracks);
            for (int i = 0; i < tracks.Count; i++)
            {
                OnDeleteTrack(tracks[i]);
            }
            OnTracksChanged();
            MarkAsEdited();
        }

        /// <summary>
        /// Called to delete track.
        /// </summary>
        /// <param name="track">The track.</param>
        protected virtual void OnDeleteTrack(Track track)
        {
            SelectedTracks.Remove(track);
            _tracks.Remove(track);
            track.OnDeleted();
        }

        /// <summary>
        /// Mark timeline as edited.
        /// </summary>
        public void MarkAsEdited()
        {
            _isModified = true;

            Modified?.Invoke();
        }

        /// <summary>
        /// Clears this instance. Removes all tracks, parameters and state.
        /// </summary>
        public void Clear()
        {
            Deselect();

            // Remove all tracks
            var tracks = new List<Track>(_tracks);
            for (int i = 0; i < tracks.Count; i++)
            {
                OnDeleteTrack(tracks[i]);
            }
            OnTracksChanged();

            ClearEditedFlag();
        }

        /// <summary>
        /// Clears the modification flag.
        /// </summary>
        public void ClearEditedFlag()
        {
            if (_isModified)
            {
                _isModified = false;
                Modified?.Invoke();
            }
        }

        internal void ChangeTrackIndex(Track track, int newIndex)
        {
            int oldIndex = _tracks.IndexOf(track);
            _tracks.RemoveAt(oldIndex);

            // Check if index is invalid
            if (newIndex < 0 || newIndex >= _tracks.Count)
            {
                // Append at the end
                _tracks.Add(track);
            }
            else
            {
                // Change order
                _tracks.Insert(newIndex, track);
            }
        }

        private void CollectTracks(Track track)
        {
            track.Parent = _tracksPanel;
            _tracks.Add(track);

            for (int i = 0; i < track.SubTracks.Count; i++)
            {
                CollectTracks(track.SubTracks[i]);
            }
        }

        /// <summary>
        /// Called when tracks order gets changed.
        /// </summary>
        public void OnTracksOrderChanged()
        {
            _tracksPanel.IsLayoutLocked = true;

            for (int i = 0; i < _tracks.Count; i++)
            {
                _tracks[i].Parent = null;
            }

            var rootTracks = new List<Track>();
            foreach (var track in _tracks)
            {
                if (track.ParentTrack == null)
                    rootTracks.Add(track);
            }
            _tracks.Clear();
            foreach (var track in rootTracks)
            {
                CollectTracks(track);
            }

            ArrangeTracks();

            _tracksPanel.IsLayoutLocked = false;
            _tracksPanel.PerformLayout();
        }

        /// <summary>
        /// Determines whether the specified track name is valid.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns> <c>true</c> if is track name is valid; otherwise, <c>false</c>.</returns>
        public bool IsTrackNameValid(string name)
        {
            name = name?.Trim();
            return !string.IsNullOrEmpty(name) && _tracks.All(x => x.Name != name);
        }

        /// <summary>
        /// Arranges the tracks.
        /// </summary>
        public void ArrangeTracks()
        {
            if (_noTracksLabel != null)
            {
                _noTracksLabel.Visible = _tracks.Count == 0;
            }

            for (int i = 0; i < _tracks.Count; i++)
            {
                _tracks[i].OnTimelineArrange();
            }

            if (_background != null)
            {
                float height = _tracksPanel.Height;

                _background.Visible = _tracks.Count > 0;
                _background.Bounds = new Rectangle(StartOffset, 0, Duration * UnitsPerSecond * Zoom, height);

                var edgeWidth = 6.0f;
                _leftEdge.Bounds = new Rectangle(_background.Left - edgeWidth * 0.5f + StartOffset, HeaderTopAreaHeight * -0.5f, edgeWidth, height + HeaderTopAreaHeight * 0.5f);
                _rightEdge.Bounds = new Rectangle(_background.Right - edgeWidth * 0.5f + StartOffset, HeaderTopAreaHeight * -0.5f, edgeWidth, height + HeaderTopAreaHeight * 0.5f);

                _backgroundScroll.Bounds = new Rectangle(0, 0, _background.Width + 5 * StartOffset, height);
            }
        }

        /// <inheritdoc />
        public override void PerformLayout(bool force = false)
        {
            ArrangeTracks();

            base.PerformLayout(force);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Synchronize scroll vertical bars for tracks and media panels to keep the view in sync
            var scroll1 = _tracksPanelArea.VScrollBar;
            var scroll2 = _backgroundArea.VScrollBar;
            if (scroll1.IsThumbClicked)
                scroll2.TargetValue = scroll1.Value;
            else
                scroll1.TargetValue = scroll2.Value;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            if (buttons == MouseButton.Right)
            {
                _isRightMouseButtonDown = true;
                _rightMouseButtonDownPos = location;

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Right && _isRightMouseButtonDown)
            {
                _isRightMouseButtonDown = false;

                if (Vector2.Distance(ref location, ref _rightMouseButtonDownPos) < 4.0f)
                {
                    if (!ContainsFocus)
                        Focus();

                    var controlUnderMouse = GetChildAtRecursive(location);
                    var mediaUnderMouse = controlUnderMouse;
                    while (mediaUnderMouse != null && !(mediaUnderMouse is Media))
                    {
                        mediaUnderMouse = mediaUnderMouse.Parent;
                    }

                    var menu = new ContextMenu.ContextMenu();
                    if (mediaUnderMouse is Media media)
                    {
                        media.OnTimelineShowContextMenu(menu, controlUnderMouse);
                        if (media.PropertiesEditObject != null)
                        {
                            menu.AddButton("Edit media", () => ShowEditPopup(media.PropertiesEditObject, ref location));
                        }
                    }
                    if (PropertiesEditObject != null)
                    {
                        menu.AddButton("Edit timeline", () => ShowEditPopup(PropertiesEditObject, ref location));
                    }
                    menu.AddSeparator();
                    menu.AddButton("Reset zoom", () => Zoom = 1.0f);
                    menu.AddButton("Show whole timeline", ShowWholeTimeline);
                    OnShowContextMenu(menu);
                    menu.Show(this, location);
                }
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <summary>
        /// Shows the whole timeline.
        /// </summary>
        public void ShowWholeTimeline()
        {
            var viewWidth = Width;
            var timelineWidth = Duration * UnitsPerSecond * Zoom + 8 * StartOffset;
            _backgroundArea.ViewOffset = Vector2.Zero;
            Zoom = viewWidth / timelineWidth;
        }

        class PropertiesEditPopup : ContextMenuBase
        {
            private Timeline _timeline;
            private bool _isDirty;

            public PropertiesEditPopup(Timeline timeline, object obj)
            {
                const float width = 280.0f;
                const float height = 160.0f;
                Size = new Vector2(width, height);

                var panel1 = new Panel(ScrollBars.Vertical)
                {
                    Bounds = new Rectangle(0, 0.0f, width, height),
                    Parent = this
                };
                var editor = new CustomEditorPresenter(null);
                editor.Panel.DockStyle = DockStyle.Top;
                editor.Panel.IsScrollable = true;
                editor.Panel.Parent = panel1;
                editor.Modified += OnModified;

                editor.Select(obj);

                _timeline = timeline;
            }

            private void OnModified()
            {
                _isDirty = true;
            }

            /// <inheritdoc />
            protected override void OnShow()
            {
                Focus();

                base.OnShow();
            }

            /// <inheritdoc />
            public override void Hide()
            {
                if (!Visible)
                    return;

                Focus(null);

                if (_isDirty)
                {
                    _timeline.MarkAsEdited();
                }

                base.Hide();
            }

            /// <inheritdoc />
            public override bool OnKeyDown(Keys key)
            {
                if (key == Keys.Escape)
                {
                    Hide();
                    return true;
                }

                return base.OnKeyDown(key);
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                _timeline = null;

                base.Dispose();
            }
        }

        /// <summary>
        /// Shows the timeline object editing popup.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="location">The show location (in timeline space).</param>
        protected virtual void ShowEditPopup(object obj, ref Vector2 location)
        {
            var popup = new PropertiesEditPopup(this, obj);
            popup.Show(this, location);
        }

        /// <summary>
        /// Called when showing context menu to the user. Can be used to add custom buttons.
        /// </summary>
        /// <param name="menu">The menu.</param>
        protected virtual void OnShowContextMenu(ContextMenu.ContextMenu menu)
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            // Clear references to the controls
            _splitter = null;
            _timeIntervalsHeader = null;
            _backgroundScroll = null;
            _background = null;
            _backgroundArea = null;
            _leftEdge = null;
            _rightEdge = null;
            _addTrackButton = null;
            _fpsComboBox = null;
            _viewButton = null;
            _fpsCustomValue = null;
            _tracksPanelArea = null;
            _tracksPanel = null;
            _playbackStop = null;
            _playbackPlay = null;
            _noTracksLabel = null;
            _positionHandle = null;

            base.Dispose();
        }
    }
}
