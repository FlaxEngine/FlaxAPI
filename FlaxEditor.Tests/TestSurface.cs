// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEditor.Surface;
using NUnit.Framework;

namespace FlaxEditor.Tests
{
    [TestFixture]
    public class TestSurface
    {
        [Test]
        public void TestArchetypes()
        {
            // Validate all archetypes (reduce mistakes)
            for (int groupIndex = 0; groupIndex < NodeFactory.DefaultGroups.Count; groupIndex++)
            {
                var group = NodeFactory.DefaultGroups[groupIndex];

                // Unique group id
                for (int i = groupIndex + 1; i < NodeFactory.DefaultGroups.Count; i++)
                {
                    Assert.IsFalse(group.GroupID == NodeFactory.DefaultGroups[i].GroupID, string.Format("Invalid group ID: {0} and {1} have the same ID", group.GroupID, NodeFactory.DefaultGroups[i].GroupID));
                }

                for (int nodeIndex = 0; nodeIndex < group.Archetypes.Length; nodeIndex++)
                {
                    var node = group.Archetypes[nodeIndex];

                    // Unique node ids
                    for (int i = nodeIndex + 1; i < group.Archetypes.Length; i++)
                    {
                        Assert.IsFalse(node.TypeID == group.Archetypes[i].TypeID, string.Format("Invalid node TypeID: {0} and {1} have the same ID", node.Title, group.Archetypes[i].Title));
                    }

                    // Unique box ids
                    if (node.Elements != null)
                    {
                        for (int i = 0; i < node.Elements.Length; i++)
                        {
                            if (node.Elements[i].Type == NodeElementType.Input || node.Elements[i].Type == NodeElementType.Output)
                            {
                                for (int j = i + 1; j < node.Elements.Length; j++)
                                {
                                    if (node.Elements[j].Type == NodeElementType.Input || node.Elements[j].Type == NodeElementType.Output)
                                    {
                                        Assert.IsFalse(node.Elements[i].BoxID == node.Elements[j].BoxID, string.Format("Invalid BoxID for node {0} in group {3}, elements: {1} and {2} have the same ID", node.Title, i, j, group.Name));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
