////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.Assertions;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// It's responsible for creating new <see cref="SurfaceNode"/> objects.
    /// It contains collection of <see cref="GroupArchetype"/> and allows to plug custom node types as well.
    /// </summary>
    public static class NodeFactory
    {
        /// <summary>
        /// The groups collection.
        /// </summary>
        public static readonly List<GroupArchetype> Groups = new List<GroupArchetype>(16)
        {
            new GroupArchetype
            {
                GroupID = 1,
                Name = "Material",
                Color = new Color(231, 76, 60),
                Archetypes = new NodeArchetype[0] // TODO: finish this
            },
            new GroupArchetype
            {
                GroupID = 2,
                Name = "Constants",
                Color = new Color(243, 156, 18),
                Archetypes = Archetypes.Constants.Nodes
            },
            new GroupArchetype
            {
                GroupID = 3,
                Name = "Math",
                Color = new Color(52, 152, 219),
                Archetypes = new NodeArchetype[0] // TODO: finish this
            },
            new GroupArchetype
            {
                GroupID = 4,
                Name = "Packing",
                Color = new Color(155, 89, 182),
                Archetypes = new NodeArchetype[0] // TODO: finish this
            },
            new GroupArchetype
            {
                GroupID = 5,
                Name = "Textures",
                Color = new Color(46, 204, 113),
                Archetypes = new NodeArchetype[0] // TODO: finish this
            },
            new GroupArchetype
            {
                GroupID = 6,
                Name = "Parameters",
                Color = new Color(52, 73, 94),
                Archetypes = new NodeArchetype[0] // TODO: finish this
            },
            new GroupArchetype
            {
                GroupID = 7,
                Name = "Tools",
                Color = new Color(149, 165, 166),
                Archetypes = new NodeArchetype[0] // TODO: finish this
            },
            new GroupArchetype
            {
                GroupID = 8,
                Name = "Layers",
                Color = new Color(249, 105, 116),
                Archetypes = new NodeArchetype[0] // TODO: finish this
            },
        };

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <param name="typeID">The type identifier.</param>
        /// <returns>Created node or null if failed.</returns>
        public static SurfaceNode CreateNode(VisjectSurface surface, ushort groupID, ushort typeID)
        {
            // Find archetype for that node
            foreach (var groupArchetype in Groups)
            {
                if (groupArchetype.GroupID == groupID && groupArchetype.Archetypes != null)
                {
                    foreach (var nodeArchetype in groupArchetype.Archetypes)
                    {
                        if (nodeArchetype.TypeID == typeID)
                        {
                            // Create
                            SurfaceNode node;
                            if (nodeArchetype.Create != null)
                                node = nodeArchetype.Create(surface, nodeArchetype, groupArchetype);
                            else
                                node = new SurfaceNode(surface, nodeArchetype, groupArchetype);
                            return node;
                        }
                    }
                }
            }

            // Error
            Debug.LogError($"Failed to find Visject Surface node with id: {groupID}:{typeID}");
            return null;
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="groupArchetype">The group archetype.</param>
        /// <param name="nodeArchetype">The node archetype.</param>
        /// <returns>Created node or null if failed.</returns>
        public static SurfaceNode CreateNode(VisjectSurface surface, GroupArchetype groupArchetype, NodeArchetype nodeArchetype)
        {
            Assert.IsTrue(groupArchetype.Archetypes.Contains(nodeArchetype));

            // Create
            SurfaceNode node;
            if (nodeArchetype.Create != null)
                node = nodeArchetype.Create(surface, nodeArchetype, groupArchetype);
            else
                node = new SurfaceNode(surface, nodeArchetype, groupArchetype);
            return node;
        }
    }
}
