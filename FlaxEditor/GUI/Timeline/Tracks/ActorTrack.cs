// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating <see cref="FlaxEngine.Actor"/> objects.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Tracks.SceneObjectTrack" />
    public sealed class ActorTrack : SceneObjectTrack
    {
        /// <summary>
        /// Gets the archetype.
        /// </summary>
        /// <returns>The archetype.</returns>
        public static TrackArchetype GetArchetype()
        {
            return new TrackArchetype
            {
                TypeId = 7,
                Name = "Actor",
                Create = options => new ActorTrack(ref options),
                Load = LoadTrack,
                Save = SaveTrack,
            };
        }

        private static void LoadTrack(int version, Track track, BinaryReader stream)
        {
            var e = (ActorTrack)track;
            e.ActorID = new Guid(stream.ReadBytes(16));
        }

        private static void SaveTrack(Track track, BinaryWriter stream)
        {
            var e = (ActorTrack)track;
            stream.Write(e.ActorID.ToByteArray());
        }

        /// <summary>
        /// The object ID.
        /// </summary>
        public Guid ActorID;

        /// <summary>
        /// Gets the object instance (it might be missing).
        /// </summary>
        public Actor Actor
        {
            get => Object.TryFind<Actor>(ref ActorID);
            set => ActorID = value?.ID ?? Guid.Empty;
        }

        /// <inheritdoc />
        public ActorTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            // Select Actor button
            const float buttonSize = 14;
            var icons = Editor.Instance.Icons;
            var selectActor = new Image(_addButton.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                TooltipText = "Selects the actor animated by this track",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.8f),
                Margin = new Margin(1),
                Brush = new SpriteBrush(icons.Search12),
                Parent = this
            };
            selectActor.Clicked += OnClickedSelectActor;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var actor = Actor;
            TitleTintColor = actor ? Color.White : Color.Red;
        }

        /// <inheritdoc />
        protected override void OnShowAddContextMenu(ContextMenu.ContextMenu menu)
        {
            base.OnShowAddContextMenu(menu);

            var actor = Actor;
            var selection = Editor.Instance.SceneEditing.Selection;

            if (actor == null)
            {
                if (selection.Count == 1 && selection[0] is ActorNode actorNode && actorNode.Actor)
                {
                    menu.AddButton("Select " + actorNode.Actor, OnClickedSelectActor);
                }
                else
                {
                    menu.AddButton("No actor selected");
                }
                return;
            }
            else if (selection.Count == 1)
            {
                // TODO: add option to change the actor to the selected one
            }

            menu.AddButton("....", OnAddPropertyTrack);
        }

        private void OnClickedSelectActor()
        {
            var selection = Editor.Instance.SceneEditing.Selection;
            if (selection.Count == 1 && selection[0] is ActorNode actorNode && actorNode.Actor)
            {
                Actor = actorNode.Actor;

                int counter = 0;
                string name = actorNode.Actor.Name;
                while (!Timeline.IsTrackNameValid(name))
                    name = string.Format("{0} {1}", actorNode.Actor.Name, counter++);
                Name = name;
            }
        }

        private void OnClickedSelectActor(Image image, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                var actor = Actor;
                if (actor)
                {
                    Editor.Instance.SceneEditing.Select(actor);
                }
            }
        }

        private void OnAddPropertyTrack()
        {
            var track = Timeline.AddTrack(AudioVolumeTrack.GetArchetype());
            track.ParentTrack = this;
            track.TrackIndex = TrackIndex + 1;
            track.Name = Guid.NewGuid().ToString();
            Timeline.OnTracksOrderChanged();
            Timeline.MarkAsEdited();
            Expand();
        }
    }
}
