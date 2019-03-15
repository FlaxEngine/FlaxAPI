// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline track that represents a folder used to group and organize tracks.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class FolderTrack : Track
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderTrack"/> class.
        /// </summary>
        /// <param name="archetype">The archetype.</param>
        public FolderTrack(Timeline.TrackArchetype archetype)
        {
            Text = archetype.Name;
            Icon = archetype.Icon;
        }
    }
}
