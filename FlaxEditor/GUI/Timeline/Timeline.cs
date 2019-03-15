// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline control that contains tracks section and headers. Can be used to create time-based media interface for camera tracks editing, audio mixing and events tracking.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Timeline : ContainerControl
    {
        /// <summary>
        /// The timeline playback buttons types.
        /// </summary>
        [Flags]
        public enum PlaybackButtons
        {
            /// <summary>
            /// The play/pause button.
            /// </summary>
            Play = 1,

            /// <summary>
            /// The stop button.
            /// </summary>
            Stop = 2,
        }

        private bool _isModified;
        private float _framesPerSecond;
        private readonly List<Track> _tracks = new List<Track>();

        private readonly SplitPanel _splitter;
        private Button _addTrackButton;
        private VerticalPanel _tracksPanel;
        private readonly Image _playbackStop;
        private readonly Image _playbackPlay;
        private readonly Label _noTracksLabel;

        /// <summary>
        /// Gets or sets the frames amount per second of the timeline animation.
        /// </summary>
        public float FramesPerSecond
        {
            get => _framesPerSecond;
            set
            {
                if (Mathf.NearEqual(_framesPerSecond, value))
                    return;

                _framesPerSecond = value;
                FramesPerSecondChanged?.Invoke();
            }
        }

        /// <summary>
        /// Occurs when frames per second gets changed changed.
        /// </summary>
        public event Action FramesPerSecondChanged;

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
        /// Initializes a new instance of the <see cref="Timeline"/> class.
        /// </summary>
        /// <param name="playbackButtons">The playback buttons to use.</param>
        public Timeline(PlaybackButtons playbackButtons)
        {
            _splitter = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None)
            {
                SplitterValue = 0.2f,
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            var headerTopAreaHeight = 22.0f;
            var headerTopArea = new ContainerControl(0, 0, _splitter.Panel1.Width, headerTopAreaHeight)
            {
                BackgroundColor = Style.Current.LightBackground,
                DockStyle = DockStyle.Top,
                Parent = _splitter.Panel1
            };
            _addTrackButton = new Button(2, 2, 50.0f, 18.0f)
            {
                Text = "Add",
                Parent = headerTopArea
            };

            var playbackButtonsSize = 20.0f;
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
                    Brush = new SpriteBrush(icons.Stop32),
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
                    Brush = new SpriteBrush(icons.Play32),
                    Tag = false, // Set to true if image is set to Pause, false if Play
                    Parent = playbackButtonsPanel
                };
                _playbackPlay.Clicked += OnPlayClicked;
                playbackButtonsPanel.Width += playbackButtonsSize;
            }

            var trackPanelArea = new Panel(ScrollBars.Vertical)
            {
                Size = new Vector2(_splitter.Panel1.Width, _splitter.Panel1.Height - playbackButtonsSize - headerTopAreaHeight),
                DockStyle = DockStyle.Fill,
                Parent = _splitter.Panel1
            };
            _tracksPanel = new VerticalPanel
            {
                Parent = trackPanelArea
            };
            _noTracksLabel = new Label
            {
                AnchorStyle = AnchorStyle.Center,
                TextColor = Color.Gray,
                TextColorHighlighted = Color.Gray * 1.1f,
                Text = "No tracks",
                Parent = trackPanelArea
            };
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

            // Update buttons UI
            var icons = Editor.Instance.Icons;
            _playbackStop.Enabled = false;
            _playbackPlay.Enabled = true;
            _playbackPlay.Brush = new SpriteBrush(icons.Play32);
            _playbackPlay.Tag = false;
        }

        /// <summary>
        /// Called when animation should play.
        /// </summary>
        public virtual void OnPlay()
        {
            Play?.Invoke();

            // Update buttons UI
            var icons = Editor.Instance.Icons;
            _playbackStop.Enabled = true;
            _playbackPlay.Enabled = true;
            _playbackPlay.Brush = new SpriteBrush(icons.Pause32);
            _playbackPlay.Tag = true;
        }

        /// <summary>
        /// Called when animation should pause.
        /// </summary>
        public virtual void OnPause()
        {
            Pause?.Invoke();

            // Update buttons UI
            var icons = Editor.Instance.Icons;
            _playbackStop.Enabled = true;
            _playbackPlay.Enabled = true;
            _playbackPlay.Brush = new SpriteBrush(icons.Play32);
            _playbackPlay.Tag = false;
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public virtual void AddTrack(Track track)
        {
            _tracks.Add(track);
            track.OnTimelineChanged(this);

            OnTracksChanged();
        }

        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public virtual void RemoveTrack(Track track)
        {
            track.OnTimelineChanged(null);
            _tracks.Remove(track);

            OnTracksChanged();
        }

        /// <summary>
        /// Called when collection of the tracks gets changed.
        /// </summary>
        protected virtual void OnTracksChanged()
        {
            _noTracksLabel.Visible = _tracks.Count == 0;
            TracksChanged?.Invoke();
        }

        /// <summary>
        /// Mark timeline as edited.
        /// </summary>
        public void MarkAsEdited()
        {
            _isModified = true;

            Modified?.Invoke();
        }
    }
}
