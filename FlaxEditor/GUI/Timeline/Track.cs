// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The Timeline track that contains a header and custom timeline events/media.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Track : ContainerControl
    {
        private Timeline _timeline;
        private Track _parentTrack;
        private float _xOffset;
        private float ChildrenIndent = 12.0f;
        private float DefaultNodeOffsetY = 1.0f;
        private float DefaultDragInsertPositionMargin = 2.0f;
        private Margin _margin = new Margin(2.0f);
        private readonly List<Media> _media = new List<Media>();
        private readonly List<Track> _subTracks = new List<Track>();
        private bool _opened;
        private bool _isMouseDown;
        private float _mouseDownTime;
        private bool _mouseOverArrow;
        private Vector2 _mouseDownPos;

        private DragItemPositioning _dragOverMode;
        private bool _isDragOverHeader;

        /// <summary>
        /// Gets the parent timeline.
        /// </summary>
        public Timeline Timeline => _timeline;

        /// <summary>
        /// Gets the parent track (null if this track is one of the root tracks in timeline).
        /// </summary>
        public Track ParentTrack => _parentTrack;

        /// <summary>
        /// Gets the collection of the media events added to this track (read-only list).
        /// </summary>
        public IReadOnlyList<Media> Media => _media;

        /// <summary>
        /// Occurs when collection of the media events gets changed.
        /// </summary>
        public event Action<Track> MediaChanged;

        /// <summary>
        /// Gets the collection of the child tracks added to this track (read-only list).
        /// </summary>
        public IReadOnlyList<Track> SubTracks => _subTracks;

        /// <summary>
        /// Occurs when collection of the sub tracks gets changed.
        /// </summary>
        public event Action<Track> SubTracksChanged;

        /// <summary>
        /// The track text.
        /// </summary>
        public string Text;

        /// <summary>
        /// The track icon.
        /// </summary>
        public Sprite Icon;

        /// <summary>
        /// Gets or sets a value indicating whether this node is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get => _opened;
            set
            {
                if (value)
                    Expand();
                else
                    Collapse();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node is collapsed.
        /// </summary>
        public bool IsCollapsed
        {
            get => !_opened;
            set
            {
                if (value)
                    Collapse();
                else
                    Expand();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        public Track()
        : base(0, 0, 100, 22.0f)
        {
        }

        /// <summary>
        /// Gets the arrow rectangle.
        /// </summary>
        protected Rectangle ArrowRect => new Rectangle(_xOffset + 2 + _margin.Left, 2, 12, 12);

        /// <summary>
        /// Called when parent timeline gets changed.
        /// </summary>
        /// <param name="timeline">The timeline.</param>
        public virtual void OnTimelineChanged(Timeline timeline)
        {
            _timeline = timeline;
        }

        /// <summary>
        /// Called when parent track gets changed.
        /// </summary>
        /// <param name="parent">The parent track.</param>
        public virtual void OnParentTrackChanged(Track parent)
        {
            _parentTrack = parent;
        }

        /// <summary>
        /// Adds the media.
        /// </summary>
        /// <param name="media">The media.</param>
        public virtual void AddMedia(Media media)
        {
            _media.Add(media);
            media.OnTimelineChanged(this);

            OnMediaChanged();
        }

        /// <summary>
        /// Removes the media.
        /// </summary>
        /// <param name="media">The media.</param>
        public virtual void RemoveMedia(Media media)
        {
            media.OnTimelineChanged(null);
            _media.Remove(media);

            OnMediaChanged();
        }

        /// <summary>
        /// Adds the sub track.
        /// </summary>
        /// <param name="track">The track.</param>
        public virtual void AdSubTrack(Track track)
        {
            _subTracks.Add(track);
            track.OnParentTrackChanged(this);

            OnSubTracksChanged();
        }

        /// <summary>
        /// Removes the sub track.
        /// </summary>
        /// <param name="track">The track.</param>
        public virtual void RemoveSubTrack(Track track)
        {
            track.OnParentTrackChanged(null);
            _subTracks.Remove(track);

            OnSubTracksChanged();
        }

        /// <summary>
        /// Called when collection of the media items gets changed.
        /// </summary>
        protected virtual void OnMediaChanged()
        {
            MediaChanged?.Invoke(this);
        }

        /// <summary>
        /// Called when collection of the sub tracks gets changed.
        /// </summary>
        protected virtual void OnSubTracksChanged()
        {
            SubTracksChanged?.Invoke(this);
        }

        /// <summary>
        /// Begins the drag drop operation.
        /// </summary>
        protected virtual void DoDragDrop()
        {
        }

        /// <summary>
        /// Called when expanded state gets changed.
        /// </summary>
        protected virtual void OnExpandedChanged()
        {
        }

        /// <summary>
        /// Expand track.
        /// </summary>
        public void Expand()
        {
            // Parents first
            ExpandAllParents();

            // Change state
            bool prevState = _opened;
            _opened = true;

            // Update
            OnExpandedChanged();
            if (HasParent)
                Parent.PerformLayout();
            else
                PerformLayout();
        }

        /// <summary>
        /// Collapse track.
        /// </summary>
        public void Collapse()
        {
            // Change state
            _opened = false;

            // Update
            OnExpandedChanged();
            if (HasParent)
                Parent.PerformLayout();
            else
                PerformLayout();
        }

        /// <summary>
        /// Expand track and all the children.
        /// </summary>
        public void ExpandAll()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            Expand();

            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is TreeNode node)
                {
                    node.ExpandAll();
                }
            }

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Collapse track and all the children.
        /// </summary>
        public void CollapseAll()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            Collapse();

            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is TreeNode node)
                {
                    node.CollapseAll();
                }
            }

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Ensure that all node parents are expanded.
        /// </summary>
        public void ExpandAllParents()
        {
            _parentTrack?.Expand();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            // Arrange sub tracks
            float xOffset = _xOffset + ChildrenIndent;
            for (int i = 0; i < SubTracks.Count; i++)
            {
                var track = SubTracks[i];
                track._xOffset = xOffset;
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            //BackgroundColor = Style.Current.BackgroundNormal;
            var style = Style.Current;
            bool isSelected = _timeline.SelectedTracks.Contains(this);
            bool isFocused = _timeline.ContainsFocus;
            var left = _xOffset + 16; // offset + arrow
            var _headerHeight = Height;
            var _headerRect = new Rectangle(0, 0, Width, _headerHeight);
            var textRect = new Rectangle(left, 0, Width - left, _headerHeight);
            _margin.ShrinkRectangle(ref textRect);
            var TextColor = style.Foreground;
            var BackgroundColorSelected = style.BackgroundSelected;
            var BackgroundColorHighlighted = style.BackgroundHighlighted;
            var BackgroundColorSelectedUnfocused = style.LightBackground;
            var TextFont = new FontReference(style.FontSmall);
            var _mouseOverHeader = IsMouseOver;

            // Draw background
            if (isSelected || _mouseOverHeader)
            {
                Render2D.FillRectangle(_headerRect, (isSelected && isFocused) ? BackgroundColorSelected : (_mouseOverHeader ? BackgroundColorHighlighted : BackgroundColorSelectedUnfocused));
            }

            // Draw arrow
            if (SubTracks.Count > 0)
            {
                Render2D.DrawSprite(_opened ? style.ArrowDown : style.ArrowRight, ArrowRect, _mouseOverHeader ? Color.White : new Color(0.8f, 0.8f, 0.8f, 0.8f));
            }

            // Draw icon
            if (Icon.IsValid)
            {
                Render2D.DrawSprite(Icon, new Rectangle(textRect.Left, (_headerHeight - 16) * 0.5f, 16, 16));
                textRect.X += 18.0f;
                textRect.Width -= 18.0f;
            }

            // Draw text
            Render2D.DrawText(TextFont.GetFont(), Text, textRect, TextColor, TextAlignment.Near, TextAlignment.Center);

            // Draw drag and drop effect
            if (IsDragOver)
            {
                Color dragOverColor = style.BackgroundSelected * 0.6f;
                Rectangle rect;
                switch (_dragOverMode)
                {
                case DragItemPositioning.At:
                    rect = textRect;
                    break;
                case DragItemPositioning.Above:
                    rect = new Rectangle(textRect.X, textRect.Y - DefaultDragInsertPositionMargin - DefaultNodeOffsetY, textRect.Width, DefaultDragInsertPositionMargin * 2.0f);
                    break;
                case DragItemPositioning.Below:
                    rect = new Rectangle(textRect.X, textRect.Bottom - DefaultDragInsertPositionMargin, textRect.Width, DefaultDragInsertPositionMargin * 2.0f);
                    break;
                default:
                    rect = Rectangle.Empty;
                    break;
                }
                Render2D.FillRectangle(rect, dragOverColor);
            }

            base.Draw();
        }


        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            // Check if mouse hits bar and node isn't a root
            if (IsMouseOver)
            {
                // Check if left button goes down
                if (buttons == MouseButton.Left)
                {
                    _isMouseDown = true;
                    _mouseDownPos = location;
                    _mouseDownTime = Time.UnscaledGameTime;
                }

                // Handled
                Focus();
                return true;
            }

            // Handled
            Focus();
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Check if mouse hits bar
            if (buttons == MouseButton.Right)
            {
                // TODO: show context menu for this track
            }
            else if (buttons == MouseButton.Left)
            {
                // Clear flag
                _isMouseDown = false;
                _mouseDownTime = -1;
            }

            // Prevent from selecting node when user is just clicking at an arrow
            if (!_mouseOverArrow)
            {
                var window = Root;
                if (window.GetKey(Keys.Control))
                {
                    // Add/Remove
                    if (_timeline.SelectedTracks.Contains(this))
                        _timeline.Deselect(this);
                    else
                        _timeline.Select(this, true);
                }
                else
                {
                    // Select
                    _timeline.Select(this, false);
                }
            }

            // Check if mouse hits arrow
            if (SubTracks.Count > 0 && _mouseOverArrow)
            {
                // Toggle open state
                /*if (_opened)
                    Collapse();
                else
                    Expand();*/
            }

            // Handled
            Focus();
            return true;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Cache flag
            _mouseOverArrow = SubTracks.Count > 0 && ArrowRect.Contains(location);

            // Check if start drag and drop
            if (_isMouseDown && Vector2.Distance(_mouseDownPos, location) > 10.0f)
            {
                // Clear flag
                _isMouseDown = false;
                _mouseDownTime = -1;

                // Start
                DoDragDrop();
                return;
            }

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flags
            _mouseOverArrow = false;

            // Check if start drag and drop
            if (_isMouseDown)
            {
                // Clear flag
                _isMouseDown = false;
                _mouseDownTime = -1;

                // Start
                DoDragDrop();
            }

            // Base
            base.OnMouseLeave();
        }
    }
}
