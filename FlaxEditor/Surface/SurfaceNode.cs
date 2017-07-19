////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface node control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class SurfaceNode : ContainerControl
    {
        private Rectangle _bounds;
        private Rectangle _headerRect;
        private Rectangle _closeButtonRect;
        private Rectangle _footerRect;
        private Vector2 _mousePosition;
        private bool _isSelected;

        /// <summary>
        /// The surface.
        /// </summary>
        public readonly VisjectSurface Surface;

        /// <summary>
        /// The node archetype.
        /// </summary>
        public readonly NodeArchetype Archetype;

        /// <summary>
        /// The group archetype.
        /// </summary>
        public readonly GroupArchetype GroupArchetype;

        /// <summary>
        /// The elements collection.
        /// </summary>
        public readonly List<ISurfaceNodeElement> Elements = new List<ISurfaceNodeElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceNode"/> class.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="nodeArch">The node archetype.</param>
        /// <param name="groupArch">The group archetype.</param>
        public SurfaceNode(VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(true, 0, 0, 100, 100)
        {
            Surface = surface;
            Archetype = nodeArch;
            GroupArchetype = groupArch;
        }

        /// <summary>
        /// Remeove all connections from and to that node.
        /// </summary>
        public void RemoveConnections()
        {
            for (int i = 0; i < _elements.Count(); i++)
            {
                auto box = dynamic_cast<Box*>(_elements[i]);
                if (box)
                    box->RemoveConnections();
            }

            UpdateBoxesTypes();
        }

        /// <summary>
        /// Updates dependant/independent boxes types.
        /// </summary>
        public void UpdateBoxesTypes()
        {
            // Check there is no need to use box types depedency feature
            if (Archetype.DependentBoxes == null || Archetype.IndependentBoxes == null)
            {
                // Back
                return;
            }

            // Get type to assign to all dependent boxes
            ConnectionType type = Archetype.DefaultType;
            for (int i = 0; i < Archetype.IndependentBoxes.Length; i++)
            {
                if (Archetype.IndependentBoxes[i] == -1)
                    break;
                var b = GetBox(Archetype.IndependentBoxes[i]);
                if (b != null && b.HasAnyConnection)
                {
                    // Check if that type if part of default type
                    if ((Archetype.DefaultType & b.Connections[0].DefaultType) != 0)
                    {
                        type = b.Connections[0].CurrentType;
                        break;
                    }
                }
            }

            // Assign connection type
            for (int i = 0; i < Archetype.DependentBoxes.Length; i++)
            {
                if (Archetype.DependentBoxes[i] == -1)
                    break;
                var b = GetBox(Archetype.DependentBoxes[i]);
                if (b != null)
                {
                    // Set new type
                    b.CurrentType = type;
                }
            }

            // Validate minor independent boxes to fit main one
            for (int i = 0; i < Archetype.IndependentBoxes.Length; i++)
            {
                if (Archetype.IndependentBoxes[i] == -1)
                    break;
                var b = GetBox(Archetype.IndependentBoxes[i]);
                if (b != null)
                {
                    // Set new type
                    b.CurrentType = type;
                }
            }
        }

        /// <summary>
        /// Tries to get box with given ID.
        /// </summary>
        /// <param name="id">The box id.</param>
        /// <returns>Box or null if cannot find.</returns>
        public Box GetBox(int id)
        {
            // TODO: maybe create local cache for boxes? but not a dictionary, use lookup table because ids are usally small (less than 20)

            Box result = null;
            for (int i = 0; i < _elements.Count(); i++)
            {
                result = dynamic_cast <::Box *> (_elements[i]);
                if (result && result->GetArchetype()->BoxID == id)
                    break;
                result = nullptr;
            }
            return result;
        }

        /// <summary>
        /// Called when surface gets loaded and boxes are connected.
        /// </summary>
        public virtual void OnSurfaceLoaded()
        {
            UpdateBoxesTypes();
        }

        /// <summary>
        /// Updates the given box connection.
        /// </summary>
        /// <param name="box">The box.</param>
        public virtual void ConnectionTick(Box box)
        {
            UpdateBoxesTypes();
        }
    }
}
