// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// Track creation options.
    /// </summary>
    public struct TrackCreateOptions
    {
        /// <summary>
        /// The track archetype.
        /// </summary>
        public TrackArchetype Archetype;

        /// <summary>
        /// Create muted track.
        /// </summary>
        public bool Mute;

        /// <summary>
        /// Create looped track.
        /// </summary>
        public bool Loop;
    }
}
