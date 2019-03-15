// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
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
        private bool _isModified;
        private float _framesPerSecond;
        private readonly List<Track> _tracks = new List<Track>();

        private readonly SplitPanel _splitter;
        private Button _addTrackButton;
        private Panel _tracksPanelArea;
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
            var addTrackButtonWidth = 50.0f;
            _addTrackButton = new Button(2, 2, addTrackButtonWidth, 18.0f)
            {
                TooltipText = "Add new tracks to the timeline",
                Text = "Add",
                Parent = headerTopArea
            };
            _addTrackButton.Clicked += OnAddTrackButtonClicked;

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
                    TooltipText = "Stop playback",
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
                    TooltipText = "Play/pause playback",
                    Brush = new SpriteBrush(icons.Play32),
                    Tag = false, // Set to true if image is set to Pause, false if Play
                    Parent = playbackButtonsPanel
                };
                _playbackPlay.Clicked += OnPlayClicked;
                playbackButtonsPanel.Width += playbackButtonsSize;
            }

            _tracksPanelArea = new Panel(ScrollBars.Vertical)
            {
                Size = new Vector2(_splitter.Panel1.Width, _splitter.Panel1.Height - playbackButtonsSize - headerTopAreaHeight),
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
        }

        private void OnAddTrackButtonClicked()
        {
            // TODO: maybe cache context menu object?
            var menu = new ContextMenu();
            for (int i = 0; i < TrackArchetypes.Count; i++)
            {
                var archetype = TrackArchetypes[i];

                var button = menu.AddButton(archetype.Name, OnAddTrackOptionClicked);
                button.Tag = archetype;
                button.Icon = archetype.Icon;
            }
            menu.Show(_addTrackButton.Parent, _addTrackButton.BottomLeft);
        }

        private void OnAddTrackOptionClicked(ContextMenuButton button)
        {
            var archetype = (TrackArchetype)button.Tag;
            var track = archetype.Create(archetype);
            if (track != null)
            {
                AddTrack(track);
            }
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
            track.Parent = _tracksPanel;

            OnTracksChanged();

            _tracksPanelArea.ScrollViewTo(track);
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
            _noTracksLabel.Visible = _tracks.Count == 0;
            TracksChanged?.Invoke();
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
