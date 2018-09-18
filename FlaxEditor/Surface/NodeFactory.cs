// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        /// The default Visject Node archetype groups collection.
        /// </summary>
        public static readonly List<GroupArchetype> DefaultGroups = new List<GroupArchetype>(16)
        {
            new GroupArchetype
            {
                GroupID = 1,
                Name = "Material",
                Color = new Color(231, 76, 60),
                Archetypes = Archetypes.Material.Nodes
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
                Archetypes = Archetypes.Math.Nodes
            },
            new GroupArchetype
            {
                GroupID = 4,
                Name = "Packing",
                Color = new Color(155, 89, 182),
                Archetypes = Archetypes.Packing.Nodes
            },
            new GroupArchetype
            {
                GroupID = 5,
                Name = "Textures",
                Color = new Color(46, 204, 113),
                Archetypes = Archetypes.Textures.Nodes
            },
            new GroupArchetype
            {
                GroupID = 6,
                Name = "Parameters",
                Color = new Color(52, 73, 94),
                Archetypes = Archetypes.Parameters.Nodes
            },
            new GroupArchetype
            {
                GroupID = 7,
                Name = "Tools",
                Color = new Color(149, 165, 166),
                Archetypes = Archetypes.Tools.Nodes
            },
            new GroupArchetype
            {
                GroupID = 8,
                Name = "Layers",
                Color = new Color(249, 105, 116),
                Archetypes = Archetypes.Layers.Nodes
            },
            new GroupArchetype
            {
                GroupID = 9,
                Name = "Animations",
                Color = new Color(105, 179, 160),
                Archetypes = Archetypes.Animation.Nodes
            },
        };

#if DEBUG
        static NodeFactory()
        {
            // Validate all archetypes (reduce mistakes)
            for (int groupIndex = 0; groupIndex < Groups.Count; groupIndex++)
            {
                var group = Groups[groupIndex];

                // Unique group id
                for (int i = groupIndex + 1; i < Groups.Count; i++)
                {
                    if(group.GroupID == Groups[i].GroupID)
                        throw new AccessViolationException("Invalid group ID.");
                }

                for (int nodeIndex = 0; nodeIndex < group.Archetypes.Length; nodeIndex++)
                {
                    var node = group.Archetypes[nodeIndex];

                    // Unique node ids
                    for (int i = nodeIndex + 1; i < group.Archetypes.Length; i++)
                    {
                        if (node.TypeID == group.Archetypes[i].TypeID)
                            throw new AccessViolationException("Invalid node ID.");
                    }

                    // Unique box ids
                    for (int i = 0; i < node.Elements.Length; i++)
                    {
                        if (node.Elements[i].Type == NodeElementType.Input || node.Elements[i].Type == NodeElementType.Output)
                        {
                            for (int j = i + 1; j < node.Elements.Length; j++)
                            {
                                if (node.Elements[j].Type == NodeElementType.Input || node.Elements[j].Type == NodeElementType.Output)
                                {
                                    if (node.Elements[i].BoxID == node.Elements[j].BoxID)
                                        throw new AccessViolationException("Invalid box ID.");
                                }
                            }
                        }
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Gets the archetypes for the node.
        /// </summary>
        /// <param name="groups">The group archetypes.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <param name="typeID">The type identifier.</param>
        /// <param name="gArch">The output group archetype.</param>
        /// <param name="arch">The output node archetype.</param>
        /// <returns>True if found it, otherwise false.</returns>
        public static bool GetArchetype(List<GroupArchetype> groups, ushort groupID, ushort typeID, out GroupArchetype gArch, out NodeArchetype arch)
        {
            gArch = null;
            arch = null;

            // Find archetype for that node
            foreach (var groupArchetype in groups)
            {
                if (groupArchetype.GroupID == groupID && groupArchetype.Archetypes != null)
                {
                    foreach (var nodeArchetype in groupArchetype.Archetypes)
                    {
                        if (nodeArchetype.TypeID == typeID)
                        {
                            // Found
                            gArch = groupArchetype;
                            arch = nodeArchetype;
                            return true;
                        }
                    }
                }
            }

            // Error
            Editor.LogError($"Failed to create Visject Surface node with id: {groupID}:{typeID}");
            return false;
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="groups">The group archetypes.</param>
        /// <param name="id">The node id.</param>
        /// <param name="surface">The surface.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <param name="typeID">The type identifier.</param>
        /// <returns>Created node or null if failed.</returns>
        public static SurfaceNode CreateNode(List<GroupArchetype> groups, uint id, VisjectSurface surface, ushort groupID, ushort typeID)
        {
            // Find archetype for that node
            foreach (var groupArchetype in groups)
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
                                node = nodeArchetype.Create(id, surface, nodeArchetype, groupArchetype);
                            else
                                node = new SurfaceNode(id, surface, nodeArchetype, groupArchetype);
                            return node;
                        }
                    }
                }
            }

            // Error
            Editor.LogError($"Failed to create Visject Surface node with id: {groupID}:{typeID}");
            return null;
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="id">The node id.</param>
        /// <param name="surface">The surface.</param>
        /// <param name="groupArchetype">The group archetype.</param>
        /// <param name="nodeArchetype">The node archetype.</param>
        /// <returns>Created node or null if failed.</returns>
        public static SurfaceNode CreateNode(uint id, VisjectSurface surface, GroupArchetype groupArchetype, NodeArchetype nodeArchetype)
        {
            Assert.IsTrue(groupArchetype.Archetypes.Contains(nodeArchetype));

            // Create
            SurfaceNode node;
            if (nodeArchetype.Create != null)
                node = nodeArchetype.Create(id, surface, nodeArchetype, groupArchetype);
            else
                node = new SurfaceNode(id, surface, nodeArchetype, groupArchetype);
            return node;
        }
    }
}
