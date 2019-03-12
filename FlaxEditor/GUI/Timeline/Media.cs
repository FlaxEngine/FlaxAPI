// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// Timeline track media event (range-based). Can be added to the timeline track.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public abstract class Media : ContainerControl
    {
        private Timeline _timeline;
        private Track _tack;

        /// <summary>
        /// Gets or sets the start frame of the media event.
        /// </summary>
        public int StartFrame { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the media event in the timeline sequence frames amount.
        /// </summary>
        public int DurationFrames { get; set; }

        /// <summary>
        /// Gets the parent timeline.
        /// </summary>
        public Timeline Timeline => _timeline;

        /// <summary>
        /// Gets the track.
        /// </summary>
        public Track Track => _tack;
    }
}
