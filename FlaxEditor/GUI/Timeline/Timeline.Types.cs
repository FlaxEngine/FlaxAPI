// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline
{
    public partial class Timeline
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

        /// <summary>
        /// Create a new track object.
        /// </summary>
        /// <param name="archetype">The archetype.</param>
        /// <returns>The created track object.</returns>
        public delegate Track CreateTrackDelegate(TrackArchetype archetype);

        /// <summary>
        /// Defines the track type.
        /// </summary>
        public class TrackArchetype
        {
            /// <summary>
            /// The name of the track type (for UI).
            /// </summary>
            public string Name;

            /// <summary>
            /// The icon of the track type (for UI).
            /// </summary>
            public Sprite Icon;

            /// <summary>
            /// The track create factory callback.
            /// </summary>
            public CreateTrackDelegate Create;
        }
    }
}
