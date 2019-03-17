// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEditor.Viewport.Previews;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline editor for particle system asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Timeline" />
    public sealed class ParticleSystemTimeline : Timeline
    {
        private ParticleSystemPreview _preview;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystemTimeline"/> class.
        /// </summary>
        /// <param name="preview">The particle system preview.</param>
        public ParticleSystemTimeline(ParticleSystemPreview preview)
        : base(PlaybackButtons.Play | PlaybackButtons.Stop)
        {
            _preview = preview;

            // Setup track types
            var icons = Editor.Instance.Icons;
            TrackArchetypes.Add(new TrackArchetype
            {
                Name = "Folder",
                Icon = icons.Folder64,
                Create = (archetype) => new FolderTrack(archetype),
            });
            TrackArchetypes.Add(new TrackArchetype
            {
                Name = "Emitter",
                Create = (archetype) => new ParticleEmitterTrack(archetype),
            });
        }
    }
}
