// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;
using Newtonsoft.Json;
using Object = FlaxEngine.Object;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating <see cref="FlaxEngine.Actor"/> objects.
    /// </summary>
    /// <seealso cref="ObjectTrack" />
    public sealed class ActorTrack : ObjectTrack
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

            // Object properties
            // TODO: implement editor-wide cache for animated properties per object type (add this in CodeEditingModule)
            var type = actor.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                var p = properties[i];

                // Properties with read/write
                if (!(p.CanRead && p.CanWrite && p.GetIndexParameters().GetLength(0) == 0))
                    continue;

                var attributes = p.GetCustomAttributes();

                // Check if has attribute to skip animating
                if (attributes.Any(x => x is NoAnimateAttribute))
                    continue;

                // Validate value type
                var valueType = p.PropertyType;
                if (BasicTypesNames.TryGetValue(valueType, out var name))
                {
                    // Basic type
                }
                else if (valueType.IsValueType)
                {
                    // Enum or Structure
                    name = valueType.Name;
                }
                else
                {
                    // Animating subobjects properties is not supported
                    continue;
                }

                // Prevent from adding the same property twice
                if (SubTracks.Any(x => x is ObjectPropertyTrack y && y.PropertyName == p.Name))
                    continue;

                menu.AddButton(name + " " + p.Name, OnAddObjectPropertyTrack).Tag = p;
            }
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

        private void OnAddObjectPropertyTrack(ContextMenuButton button)
        {
            var p = (PropertyInfo)button.Tag;

            // Detect the type of the track to use
            var valueType = p.PropertyType;
            if (BasicTypesTrackArchetypes.TryGetValue(valueType, out var archetype))
            {
                // Basic type
            }
            else if (valueType.IsEnum)
            {
                // Enum
                archetype = KeyframesObjectPropertyTrack.GetArchetype();
            }
            else
            {
                throw new Exception("Invalid property type to create animation track for it. Value type: " + valueType);
            }

            var track = (ObjectPropertyTrack)Timeline.AddTrack(archetype);
            track.ParentTrack = this;
            track.TrackIndex = TrackIndex + 1;
            track.Name = Guid.NewGuid().ToString();
            track.Property = p;

            Timeline.OnTracksOrderChanged();
            Timeline.MarkAsEdited();
            Expand();
        }

        private static readonly Dictionary<Type, string> BasicTypesNames = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
        };

        private static readonly Dictionary<Type, TrackArchetype> BasicTypesTrackArchetypes = new Dictionary<Type, TrackArchetype>
        {
            { typeof(bool), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(byte), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(char), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(short), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(ushort), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(int), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(uint), KeyframesObjectPropertyTrack.GetArchetype() },
            { typeof(long), KeyframesObjectPropertyTrack.GetArchetype() },
            //{ typeof(float), CurveObjectPropertyTrack.GetArchetype() },
            //{ typeof(double), CurveObjectPropertyTrack.GetArchetype() },
        };
    }
}
