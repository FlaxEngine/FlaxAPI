// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline media that represents an camera cut media event.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Media" />
    public class CameraCutMedia : Media
    {
        private sealed class Proxy : ProxyBase<CameraCutTrack, CameraCutMedia>
        {
            public Proxy(CameraCutTrack track, CameraCutMedia media)
            : base(track, media)
            {
            }
        }

        /// <inheritdoc />
        public override void OnTimelineChanged(Track track)
        {
            base.OnTimelineChanged(track);

            PropertiesEditObject = new Proxy(Track as CameraCutTrack, this);
        }
    }

    /// <summary>
    /// The timeline track for animating <see cref="FlaxEngine.Camera"/> objects.
    /// </summary>
    /// <seealso cref="ActorTrack" />
    public class CameraCutTrack : ActorTrack
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public new static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 16,
                Name = "Camera Cut",
                Create = options => new CameraCutTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (CameraCutTrack)track;
            e.ActorID = new Guid(stream.ReadBytes(16));
            var m = e.TrackMedia;
            m.StartFrame = stream.ReadInt32();
            m.DurationFrames = stream.ReadInt32();
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (CameraCutTrack)track;
            stream.Write(e.ActorID.ToByteArray());
            if (e.Media.Count != 0)
            {
                var m = e.TrackMedia;
                stream.Write(m.StartFrame);
                stream.Write(m.DurationFrames);
            }
            else
            {
                stream.Write(0);
                stream.Write(track.Timeline.DurationFrames);
            }
        }

        /// <summary>
        /// Gets the camera object instance (it might be missing).
        /// </summary>
        public Camera Camera => Actor as Camera;

        /// <summary>
        /// Gets the camera track media.
        /// </summary>
        public CameraCutMedia TrackMedia
        {
            get
            {
                CameraCutMedia media;
                if (Media.Count == 0)
                {
                    media = new CameraCutMedia
                    {
                        StartFrame = 0,
                        DurationFrames = Timeline != null ? (int)(Timeline.FramesPerSecond * 2) : 60,
                    };
                    AddMedia(media);
                }
                else
                {
                    media = (CameraCutMedia)Media[0];
                }
                return media;
            }
        }

        /// <inheritdoc />
        public CameraCutTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            Height = 68;
        }

        /// <inheritdoc />
        protected override bool IsActorValid(Actor actor)
        {
            return base.IsActorValid(actor) && actor is Camera;
        }

        /// <inheritdoc />
        public override void OnSpawned()
        {
            // Ensure to have valid media added
            // ReSharper disable once UnusedVariable
            var media = TrackMedia;

            base.OnSpawned();
        }
    }
}
