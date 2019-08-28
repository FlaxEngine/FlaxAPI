// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Linq;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating <see cref="FlaxEngine.Actor"/> objects.
    /// </summary>
    /// <seealso cref="ObjectTrack" />
    sealed class ActorTrack : ObjectTrack
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
        public override Object Object => Actor;

        /// <inheritdoc />
        protected override void OnShowAddContextMenu(ContextMenu.ContextMenu menu)
        {
            base.OnShowAddContextMenu(menu);

            var actor = Actor;
            var selection = Editor.Instance.SceneEditing.Selection;

            // Missing actor case
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

            var type = actor.GetType();
            if (AddObjectProperties(menu, type) != 0)
                menu.AddSeparator();

            // Child scripts
            var scripts = actor.Scripts;
            for (int i = 0; i < scripts.Length; i++)
            {
                var script = scripts[i];

                // Prevent from adding the same track twice
                if (SubTracks.Any(x => x is ObjectTrack y && y.Object == script))
                    continue;

                var name = CustomEditorsUtil.GetPropertyNameUI(script.GetType().Name);
                menu.AddButton(name, OnAddScriptTrack).Tag = script;
            }
        }

        private void OnAddScriptTrack(ContextMenuButton button)
        {
            var script = (Script)button.Tag;

            var track = (ScriptTrack)Timeline.AddTrack(ScriptTrack.GetArchetype());
            track.ParentTrack = this;
            track.TrackIndex = TrackIndex + 1;
            track.Script = script;

            int counter = 0;
            string name = script.GetType().Name;
            while (!Timeline.IsTrackNameValid(name))
                name = string.Format("{0} {1}", script.GetType().Name, counter++);
            track.Name = name;

            Timeline.OnTracksOrderChanged();
            Timeline.MarkAsEdited();
            Expand();
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
    }
}