// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// Copies the selected items.
        /// </summary>
        public void Copy()
        {
            var selection = Selection;

            if (selection.Count == 0)
            {
                Application.ClipboardText = string.Empty;
                return;
            }

            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("Nodes");
                jsonWriter.WriteStartArray();

                for (int i = 0; i < selection.Count; i++)
                {
                    var node = selection[i];

                    jsonWriter.WriteStartObject();

                    jsonWriter.WritePropertyName("GroupID");
                    jsonWriter.WriteValue(node.GroupArchetype.GroupID);

                    jsonWriter.WritePropertyName("TypeID");
                    jsonWriter.WriteValue(node.Archetype.TypeID);

                    jsonWriter.WritePropertyName("ID");
                    jsonWriter.WriteValue(node.ID);

                    jsonWriter.WritePropertyName("X");
                    jsonWriter.WriteValue(node.Location.X);

                    jsonWriter.WritePropertyName("Y");
                    jsonWriter.WriteValue(node.Location.Y);

                    if (node.Values != null && node.Values.Length > 0)
                    {
                        jsonWriter.WritePropertyName("Values");
                        jsonWriter.WriteStartArray();

                        for (int j = 0; j < node.Values.Length; j++)
                        {
                            WriteCommonValue(jsonWriter, node.Values[j]);
                        }

                        jsonWriter.WriteEndArray();
                    }

                    if (node.Elements != null && node.Elements.Count > 0)
                    {
                        jsonWriter.WritePropertyName("Boxes");
                        jsonWriter.WriteStartArray();

                        for (int j = 0; j < node.Elements.Count; j++)
                        {
                            if (node.Elements[j] is Box box)
                            {
                                jsonWriter.WriteStartObject();

                                jsonWriter.WritePropertyName("ID");
                                jsonWriter.WriteValue(box.ID);

                                jsonWriter.WritePropertyName("NodeIDs");
                                jsonWriter.WriteStartArray();

                                for (int k = 0; k < box.Connections.Count; k++)
                                {
                                    var target = box.Connections[k];
                                    jsonWriter.WriteValue(target.ParentNode.ID);
                                }

                                jsonWriter.WriteEndArray();

                                jsonWriter.WritePropertyName("BoxIDs");
                                jsonWriter.WriteStartArray();

                                for (int k = 0; k < box.Connections.Count; k++)
                                {
                                    var target = box.Connections[k];
                                    jsonWriter.WriteValue(target.ID);
                                }

                                jsonWriter.WriteEndArray();

                                jsonWriter.WriteEndObject();
                            }
                        }

                        jsonWriter.WriteEndArray();
                    }

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndArray();

                jsonWriter.WriteEnd();
            }

            Application.ClipboardText = sw.ToString();
        }

        private class BoxDataModel
        {
            public int ID;
            public uint[] NodeIDs;
            public int[] BoxIDs;
        }

        private class NodeDataModel
        {
            public ushort GroupID;
            public ushort TypeID;
            public uint ID;
            public float X;
            public float Y;
            public object[] Values;
            public BoxDataModel[] Boxes;
        }

        private class NodesDataModel
        {
            public NodeDataModel[] Nodes;
        }

        /// <summary>
        /// Checks if can paste the nodes data from the clipboard.
        /// </summary>
        /// <returns>True if can paste data, otherwise false.</returns>
        public bool CanPaste()
        {
            var data = Application.ClipboardText;
            if (data == null || data.Length < 2)
                return false;

            try
            {
                var model = JsonConvert.DeserializeObject<NodesDataModel>(data);
                return model?.Nodes != null && model.Nodes.Length != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Pastes the copied items.
        /// </summary>
        public void Paste()
        {
            var data = Application.ClipboardText;
            if (data == null || data.Length < 2)
                return;

            try
            {
                // Load Mr Json
                var model = JsonConvert.DeserializeObject<NodesDataModel>(data);

                // Build the nodes IDs mapping (need to generate new IDs for the pasted nodes and preserve the internal connections)
                var idsMapping = new Dictionary<uint, uint>();
                for (int i = 0; i < model.Nodes.Length; i++)
                {
                    uint result = 1;
                    while (true)
                    {
                        bool valid = true;
                        if (idsMapping.ContainsValue(result))
                        {
                            result++;
                            valid = false;
                        }
                        else
                        {
                            for (int j = 0; j < _nodes.Count; j++)
                            {
                                if (_nodes[j].ID == result)
                                {
                                    result++;
                                    valid = false;
                                    break;
                                }
                            }
                        }

                        if (valid)
                            break;
                    }

                    idsMapping.Add(model.Nodes[i].ID, result);
                }

                // Find nodes upper left location
                Vector2 upperLeft = new Vector2(model.Nodes[0].X, model.Nodes[0].Y);
                for (int i = 1; i < model.Nodes.Length; i++)
                {
                    upperLeft.X = Mathf.Min(upperLeft.X, model.Nodes[i].X);
                    upperLeft.Y = Mathf.Min(upperLeft.Y, model.Nodes[i].Y);
                }

                // Create nodes
                var type = Type;
                var nodes = new Dictionary<uint, SurfaceNode>();
                var nodesData = new Dictionary<uint, NodeDataModel>();
                for (int i = 0; i < model.Nodes.Length; i++)
                {
                    var nodeData = model.Nodes[i];

                    // Peek type
                    GroupArchetype groupArchetype;
                    NodeArchetype nodeArchetype;
                    if (!NodeFactory.GetArchetype(nodeData.GroupID, nodeData.TypeID, out groupArchetype, out nodeArchetype))
                        throw new InvalidOperationException("Unknown node type.");

                    // Validate given node type
                    {
                        if ((nodeArchetype.Flags & NodeFlags.NoSpawnViaGUI) != 0)
                            continue;

                        if (type != SurfaceType.Material && (nodeArchetype.Flags & NodeFlags.MaterialOnly) != 0)
                            continue;

                        if (type != SurfaceType.AnimationGraph && (nodeArchetype.Flags & NodeFlags.AnimGraphOnly) != 0)
                            continue;

                        if (type != SurfaceType.Visject && (nodeArchetype.Flags & NodeFlags.VisjectOnly) != 0)
                            continue;
                    }

                    // Create
                    var node = NodeFactory.CreateNode(idsMapping[nodeData.ID], this, groupArchetype, nodeArchetype);
                    if (node == null)
                        throw new InvalidOperationException("Failed to create node.");
                    _nodes.Add(node);
                    nodes.Add(nodeData.ID, node);
                    nodesData.Add(nodeData.ID, nodeData);

                    // Initialize
                    if (nodeData.Values != null && node.Values.Length > 0)
                    {
                        if (node.Values != null && node.Values.Length == nodeData.Values.Length)
                        {
                            // Copy and fix values (Json deserializes may output them in a different format)
                            for (int l = 0; l < node.Values.Length; l++)
                            {
                                var src = nodeData.Values[l];
                                var dst = node.Values[l];

                                if (src is JToken token)
                                {
                                    if (dst is Vector2)
                                    {
                                        src = new Vector2(token["X"].Value<float>(),
                                                          token["Y"].Value<float>());
                                    }
                                    else if (dst is Vector3)
                                    {
                                        src = new Vector3(token["X"].Value<float>(),
                                                          token["Y"].Value<float>(),
                                                          token["Z"].Value<float>());
                                    }
                                    else if (dst is Vector4)
                                    {
                                        src = new Vector4(token["X"].Value<float>(),
                                                          token["Y"].Value<float>(),
                                                          token["Z"].Value<float>(),
                                                          token["W"].Value<float>());
                                    }
                                    else if (dst is Color)
                                    {
                                        src = new Color(token["R"].Value<float>(),
                                                        token["G"].Value<float>(),
                                                        token["B"].Value<float>(),
                                                        token["A"].Value<float>());
                                    }
                                    else
                                    {
                                        Editor.LogWarning("Unknown pasted node value token: " + token);
                                        src = dst;
                                    }
                                }
                                else if (src is double asDouble)
                                {
                                    src = (float)asDouble;
                                }
                                else if (dst is Guid)
                                {
                                    src = Guid.Parse((string)src);
                                }
                                else if (dst is int)
                                {
                                    src = Convert.ToInt32(src);
                                }
                                else if (dst is float)
                                {
                                    src = Convert.ToSingle(src);
                                }

                                node.Values[l] = src;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Invalid node custom values.");
                        }
                    }

                    OnNodeLoaded(node);
                }

                // Setup connections
                foreach (var e in nodes)
                {
                    var node = e.Value;
                    var nodeData = nodesData[e.Key];
                    foreach (var boxData in nodeData.Boxes)
                    {
                        var box = node.GetBox(boxData.ID);
                        if (box == null || boxData.BoxIDs == null || boxData.NodeIDs == null || boxData.BoxIDs.Length != boxData.NodeIDs.Length)
                            continue;

                        for (int i = 0; i < boxData.NodeIDs.Length; i++)
                        {
                            if (nodes.TryGetValue(boxData.NodeIDs[i], out var targetNode)
                                && targetNode.TryGetBox(boxData.BoxIDs[i], out var targetBox))
                            {
                                box.Connections.Add(targetBox);
                            }
                        }
                    }
                }

                // Arrange nodes
                foreach (var e in nodes)
                {
                    var node = e.Value;
                    var nodeData = nodesData[e.Key];
                    var pos = new Vector2(nodeData.X, nodeData.Y) - upperLeft;
                    node.Location = ViewPosition + pos + new Vector2(40);
                }

                // Post load
                foreach (var node in nodes)
                {
                    node.Value.OnSurfaceLoaded();
                }

                // Select those nodes
                Select(nodes.Values);

                MarkAsEdited();
            }
            catch (Exception ex)
            {
                Editor.LogWarning("Failed to paste Visject Surface nodes");
                Editor.LogWarning(ex);
                MessageBox.Show("Failed to paste Visject Surface nodes. " + ex.Message, "Paste failed", MessageBox.Buttons.OK, MessageBox.Icon.Error);
            }
        }

        /// <summary>
        /// Cuts the selected items.
        /// </summary>
        public void Cut()
        {
            Copy();
            Delete();
        }

        /// <summary>
        /// Duplicates the selected items.
        /// </summary>
        public void Duplicate()
        {
            Copy();
            Paste();
        }
    }
}
