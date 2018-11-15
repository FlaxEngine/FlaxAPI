// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable ClassNeverInstantiated.Local
#pragma warning disable 649

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// Copies the selected items.
        /// </summary>
        public void Copy()
        {
            var selection = SelectedControls;

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
                    var node = selection[i] as SurfaceNode;
                    if (node == null)
                        continue;

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
                            Utilities.Utils.WriteCommonValue(jsonWriter, node.Values[j]);
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

                jsonWriter.WritePropertyName("Comments");
                jsonWriter.WriteStartArray();

                for (int i = 0; i < selection.Count; i++)
                {
                    var comment = selection[i] as SurfaceComment;
                    if (comment == null)
                        continue;

                    jsonWriter.WriteStartObject();

                    jsonWriter.WritePropertyName("Title");
                    jsonWriter.WriteValue(comment.Title);

                    jsonWriter.WritePropertyName("Color");
                    Utilities.Utils.WriteCommonValue(jsonWriter, comment.Color);

                    jsonWriter.WritePropertyName("Bounds");
                    Utilities.Utils.WriteCommonValue(jsonWriter, comment.Bounds);

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndArray();

                jsonWriter.WriteEnd();
            }

            Application.ClipboardText = sw.ToString();
        }

        /// <summary>
        /// The box data model.
        /// </summary>
        private class BoxDataModel
        {
            /// <summary>
            /// The identifier.
            /// </summary>
            public int ID;

            /// <summary>
            /// The connected nodes ids.
            /// </summary>
            public uint[] NodeIDs;

            /// <summary>
            /// The connected boxes ids.
            /// </summary>
            public int[] BoxIDs;
        }

        /// <summary>
        /// The node data model.
        /// </summary>
        private class NodeDataModel
        {
            /// <summary>
            /// The group identifier.
            /// </summary>
            public ushort GroupID;

            /// <summary>
            /// The type identifier.
            /// </summary>
            public ushort TypeID;

            /// <summary>
            /// The identifier.
            /// </summary>
            public uint ID;

            /// <summary>
            /// The x position.
            /// </summary>
            public float X;

            /// <summary>
            /// The y position.
            /// </summary>
            public float Y;

            /// <summary>
            /// The values.
            /// </summary>
            public object[] Values;

            /// <summary>
            /// The boxes.
            /// </summary>
            public BoxDataModel[] Boxes;
        }

        /// <summary>
        /// Comment data model.
        /// </summary>
        private class CommentDataModel
        {
            /// <summary>
            /// The title text.
            /// </summary>
            public string Title;

            /// <summary>
            /// The color.
            /// </summary>
            public Color Color;

            /// <summary>
            /// The bounds of the comment (in surface-space).
            /// </summary>
            public Rectangle Bounds;
        }

        /// <summary>
        /// Copied data model.
        /// </summary>
        private class DataModel
        {
            /// <summary>
            /// The nodes.
            /// </summary>
            public NodeDataModel[] Nodes;

            /// <summary>
            /// The comments.
            /// </summary>
            public CommentDataModel[] Comments;
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
                var model = JsonConvert.DeserializeObject<DataModel>(data);
                return model != null && ((model.Nodes != null && model.Nodes.Length != 0) || (model.Comments != null && model.Comments.Length != 0));
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
                var model = JsonConvert.DeserializeObject<DataModel>(data);
                if (model.Nodes == null)
                    model.Nodes = new NodeDataModel[0];
                if (model.Comments == null)
                    model.Comments = new CommentDataModel[0];

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
                            for (int j = 0; j < Nodes.Count; j++)
                            {
                                if (Nodes[j].ID == result)
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

                // Find controls upper left location
                Vector2 upperLeft;
                if (model.Nodes.Length > 0)
                {
                    upperLeft = new Vector2(model.Nodes[0].X, model.Nodes[0].Y);
                    for (int i = 1; i < model.Nodes.Length; i++)
                    {
                        upperLeft.X = Mathf.Min(upperLeft.X, model.Nodes[i].X);
                        upperLeft.Y = Mathf.Min(upperLeft.Y, model.Nodes[i].Y);
                    }
                    for (int i = 0; i < model.Comments.Length; i++)
                    {
                        upperLeft = Vector2.Min(upperLeft, model.Comments[i].Bounds.Location);
                    }
                }
                else
                {
                    upperLeft = model.Comments[0].Bounds.Location;
                    for (int i = 1; i < model.Comments.Length; i++)
                    {
                        upperLeft = Vector2.Min(upperLeft, model.Comments[i].Bounds.Location);
                    }
                }

                // Create nodes
                var nodes = new Dictionary<uint, SurfaceNode>();
                var nodesData = new Dictionary<uint, NodeDataModel>();
                for (int i = 0; i < model.Nodes.Length; i++)
                {
                    var nodeData = model.Nodes[i];

                    // Peek type
                    GroupArchetype groupArchetype;
                    NodeArchetype nodeArchetype;
                    if (!NodeFactory.GetArchetype(NodeArchetypes, nodeData.GroupID, nodeData.TypeID, out groupArchetype, out nodeArchetype))
                        throw new InvalidOperationException("Unknown node type.");

                    // Validate given node type
                    if ((nodeArchetype.Flags & NodeFlags.NoSpawnViaGUI) != 0 || !CanSpawnNodeType(nodeArchetype))
                        continue;

                    // Create
                    var node = NodeFactory.CreateNode(idsMapping[nodeData.ID], Context, groupArchetype, nodeArchetype);
                    if (node == null)
                        throw new InvalidOperationException("Failed to create node.");
                    Nodes.Add(node);
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
                                else if (dst is byte[] && src is string)
                                {
                                    src = Convert.FromBase64String((string)src);
                                }

                                node.Values[l] = src;
                            }
                        }
                        else
                        {
                            Editor.LogWarning("Invalid node custom values.");
                        }
                    }

                    Context.OnControlLoaded(node);
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

                // Create comments
                var comments = new List<SurfaceComment>();
                for (int i = 0; i < model.Comments.Length; i++)
                {
                    var commentData = model.Comments[i];

                    // Create
                    var comment = Context.SpawnComment(ref commentData.Bounds);
                    if (comment == null)
                        throw new InvalidOperationException("Failed to create comment.");
                    comments.Add(comment);

                    comment.Title = commentData.Title;
                    comment.Color = commentData.Color;

                    Context.OnControlLoaded(comment);
                }

                // Arrange controls
                foreach (var e in nodes)
                {
                    var node = e.Value;
                    var nodeData = nodesData[e.Key];
                    var pos = new Vector2(nodeData.X, nodeData.Y) - upperLeft;
                    node.Location = ViewPosition + pos + _mousePos / ViewScale;
                }
                foreach (var comment in comments)
                {
                    var pos = comment.Location - upperLeft;
                    comment.Location = ViewPosition + pos + _mousePos / ViewScale;
                }

                // Post load
                foreach (var node in nodes)
                {
                    node.Value.OnSurfaceLoaded();
                }
                foreach (var comment in comments)
                {
                    comment.OnSurfaceLoaded();
                }

                // Select those nodes and comments
                if (comments.Count == 0)
                {
                    Select(nodes.Values);
                }
                else if (nodes.Count == 0)
                {
                    Select(comments);
                }
                else
                {
                    Select(nodes.Values.Cast<SurfaceControl>().Union(comments));
                }

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
