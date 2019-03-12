// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
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
        private readonly List<Media> _media = new List<Media>();

        /// <summary>
        /// Gets the parent timeline.
        /// </summary>
        public Timeline Timeline => _timeline;

        /// <summary>
        /// Gets the collection of the media events added to this track (read-only list).
        /// </summary>
        public IReadOnlyList<Media> Media => _media;

        /// <summary>
        /// Occurs when collection of the media events gets changed.
        /// </summary>
        public event Action<Track> MediaChanged;

        /// <summary>
        /// Called when parent timeline gets changed.
        /// </summary>
        /// <param name="timeline">The timeline.</param>
        public virtual void OnTimelineChanged(Timeline timeline)
        {
            _timeline = timeline;
        }
    }
}
