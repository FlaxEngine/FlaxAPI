////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Surface.Elements;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface node control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class SurfaceNode : ContainerControl
    {
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
            if (_arch->DependentBoxes[0] == -1 || _arch->IndependentBoxes[0] == -1)
            {
                // Back
                return;
            }

            // Get type to assign to all dependent boxes
            GraphConnectionType type = _arch->DefaultType;
            for (int i = 0; i < SURFACE_NODE_MAX_BOX_DEPEDENCIES; i++)
            {
                if (_arch->IndependentBoxes[i] == -1)
                    break;
                SurfaceGraphBox* b = _node->GetBox(_arch->IndependentBoxes[i]);
                if (b && b->HasConnection())
                {
                    // Check if that type if part of default type
                    if (_arch->DefaultType & b->Connections[0]->Type)
                    {
                        type = b->Connections[0]->Data->GetCurrentType();
                        break;
                    }
                }
            }

            // Assign connection type
            for (int i = 0; i < SURFACE_NODE_MAX_BOX_DEPEDENCIES; i++)
            {
                if (_arch->DependentBoxes[i] == -1)
                    break;
                SurfaceGraphBox* b = _node->GetBox(_arch->DependentBoxes[i]);
                if (b)
                {
                    // Set new type
                    b->Data->SetCurrentType(type);
                }
            }

            // Validate minor independent boxes to fit main one
            for (int i = 0; i < SURFACE_NODE_MAX_BOX_DEPEDENCIES; i++)
            {
                if (_arch->IndependentBoxes[i] == -1)
                    break;
                SurfaceGraphBox* b = _node->GetBox(_arch->IndependentBoxes[i]);
                if (b)
                {
                    // Set new type
                    b->Data->SetCurrentType(type);
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
