// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using Utils = FlaxEditor.Utilities.Utils;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurfaceContext
    {
        // Note: surface serialization is port from c++ code base (also a legacy)
        // Refactor this in future together with c++ backend

        private struct ConnectionHint
        {
            public SurfaceNode NodeA;
            public byte BoxA;
            public SurfaceNode NodeB;
            public byte BoxB;
        }

        private readonly ThreadLocal<List<Box>> _cachedBoxes = new ThreadLocal<List<Box>>(() => new List<Box>());
        private readonly ThreadLocal<List<ConnectionHint>> _cachedConnections = new ThreadLocal<List<ConnectionHint>>(() => new List<ConnectionHint>());

        /// <summary>
        /// Loads the surface from bytes. Clears the surface before and uses context source data as a surface bytes source.
        /// </summary>
        /// <remarks>
        /// Assume this method does not throw exceptions but uses return value as a error code.
        /// </remarks>
        /// <returns>True if failed, otherwise false.</returns>
        public bool Load()
        {
            try
            {
                // Prepare
                Clear();

                Loading?.Invoke(this);

                // Load bytes
                var bytes = Context.SurfaceData;
                if (bytes == null)
                    throw new Exception("Failed to load surface data.");

                // Load graph (empty bytes data means empty graph for simplicity when using subgraphs)
                if (bytes.Length > 0)
                {
                    using (var stream = new MemoryStream(bytes))
                    using (var reader = new BinaryReader(stream))
                    {
                        LoadGraph(reader);
                    }
                }

                // Load surface meta
                var meta = _meta.GetEntry(10);
                if (meta.Data != null)
                {
                    Utils.ByteArrayToStructure(meta.Data, out CachedSurfaceMeta);
                }
                else
                {
                    // Reset view
                    CachedSurfaceMeta.ViewCenterPosition = Vector2.Zero;
                    CachedSurfaceMeta.Scale = 1.0f;
                }

                // Load surface comments
                var commentsData = _meta.GetEntry(666);
                if (commentsData.Data != null)
                {
                    using (var stream = new MemoryStream(commentsData.Data))
                    using (var reader = new BinaryReader(stream))
                    {
                        var commentsCount = reader.ReadInt32();

                        for (int i = 0; i < commentsCount; i++)
                        {
                            var title = Utils.ReadStr(reader, 71);
                            var color = new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                            var bounds = new Rectangle(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                            var comment = SpawnComment(ref bounds);
                            if (comment == null)
                                throw new InvalidOperationException("Failed to create comment.");

                            comment.Title = title;
                            comment.Color = color;

                            OnControlLoaded(comment);
                        }
                    }
                }

                // Post load
                for (int i = 0; i < RootControl.Children.Count; i++)
                {
                    if (RootControl.Children[i] is SurfaceControl control)
                        control.OnSurfaceLoaded();
                }

                RootControl.UnlockChildrenRecursive();

                // Update boxes types for nodes that dependant box types based on incoming connections
                {
                    bool keepUpdating = false;
                    int updateLimit = 100;
                    do
                    {
                        for (int i = 0; i < RootControl.Children.Count; i++)
                        {
                            if (RootControl.Children[i] is SurfaceNode node && !node.HasDependentBoxesSetup)
                            {
                                node.UpdateBoxesTypes();
                                keepUpdating = true;
                            }
                        }
                    } while (keepUpdating && updateLimit-- > 0);
                }

                Loaded?.Invoke(this);

                // Clear modification flag
                _isModified = false;
            }
            catch (Exception ex)
            {
                // Error
                Editor.LogWarning("Loading Visject Surface data failed.");
                Editor.LogWarning(ex);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves the surface to bytes. Performs also modified child surfaces saving before.
        /// </summary>
        /// <remarks>
        /// Assume this method does not throw exceptions but uses return value as a error code.
        /// </remarks>
        /// <returns>True if failed, otherwise false.</returns>
        public bool Save()
        {
            // Save all children modified before saving the current surface
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsModified && Children[i].Save())
                    return true;
            }

            _meta.Release();

            Saving?.Invoke(this);

            // Save surface meta
            _meta.AddEntry(10, Utils.StructureToByteArray(ref CachedSurfaceMeta));

            // Save surface comments (in surface meta container)
            var comments = Comments;
            if (comments.Count > 0)
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(comments.Count);

                    for (int i = 0; i < comments.Count; i++)
                    {
                        var comment = comments[i];

                        Utils.WriteStr(writer, comment.Title, 71);
                        writer.Write(comment.Color.R);
                        writer.Write(comment.Color.G);
                        writer.Write(comment.Color.B);
                        writer.Write(comment.Color.A);
                        writer.Write(comment.X);
                        writer.Write(comment.Y);
                        writer.Write(comment.Width);
                        writer.Write(comment.Height);
                    }

                    _meta.AddEntry(666, stream.ToArray());
                }
            }

            // Save all nodes meta
            VisjectSurface.Meta11 meta11;
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                meta11.Position = node.Location;
                meta11.Selected = false; // don't save selection to prevent stupid binary diffs on asset
                node.Meta.Release();
                // TODO: reuse byte[] array for all nodes to reduce dynamic memory allocations
                node.Meta.AddEntry(11, Utils.StructureToByteArray(ref meta11));
            }

            // Save graph
            try
            {
                // Save graph
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    // Save graph to bytes
                    SaveGraph(writer);
                    var bytes = stream.ToArray();

                    // Send data to the container
                    Context.SurfaceData = bytes;

                    Saved?.Invoke(this);

                    // Clear modification flag
                    _isModified = false;
                }
            }
            catch (Exception ex)
            {
                // Error
                Editor.LogWarning("Saving Visject Surface data failed.");
                Editor.LogWarning(ex);
                return true;
            }

            return false;
        }

        private void SaveGraph(BinaryWriter stream)
        {
            // IMPORTANT! This must match C++ Graph format
            // Changes: don't write save time and keep engine build constant

            // Magic Code
            stream.Write(1963542358);

            // Engine Build
            stream.Write(6118);

            // Time saved
            stream.Write((long)0);

            // Nodes count
            stream.Write(Nodes.Count);

            // Parameters count
            stream.Write(Parameters.Count);

            // For each node
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];

                // ID
                stream.Write(node.ID);

                // Type
                stream.Write(node.Type);
            }

            // For each param
            for (int i = 0; i < Parameters.Count; i++)
            {
                var param = Parameters[i];

                // Properties
                stream.Write((byte)param.Type);
                stream.Write(param.ID.ToByteArray());
                Utils.WriteStr(stream, param.Name, 97);
                stream.Write((byte)(param.IsPublic ? 1 : 0));
                stream.Write((byte)(param.IsStatic ? 1 : 0));
                stream.Write((byte)(param.IsUIVisible ? 1 : 0));
                stream.Write((byte)(param.IsUIEditable ? 1 : 0));

                // References
                stream.Write(param.ReferencedBy.Count);
                for (int j = 0; j < param.ReferencedBy.Count; j++)
                    stream.Write(param.ReferencedBy[j].ID);

                // Value
                Utils.WriteCommonValue(stream, param.Value);

                // Meta
                param.Meta.Save(stream);
            }

            // For each node
            var boxes = _cachedBoxes.Value;
            boxes.Clear();
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];

                // Values
                if (node.Values != null)
                {
                    stream.Write(node.Values.Length);
                    for (int j = 0; j < node.Values.Length; j++)
                        Utils.WriteCommonValue(stream, node.Values[j]);
                }
                else
                {
                    stream.Write(0);
                }

                // Boxes
                node.GetBoxes(boxes);
                stream.Write((ushort)boxes.Count);
                for (int j = 0; j < boxes.Count; j++)
                {
                    var box = boxes[j];

                    stream.Write((byte)box.ID);
                    stream.Write((uint)box.DefaultType);
                    stream.Write((ushort)box.Connections.Count);
                    for (int k = 0; k < box.Connections.Count; k++)
                    {
                        var targetBox = box.Connections[k];

                        if (targetBox == null)
                        {
                            throw new Exception("Missing target box.");
                        }

                        stream.Write(targetBox.ParentNode.ID);
                        stream.Write((byte)targetBox.ID);
                    }
                }

                // Meta
                node.Meta.Save(stream);
            }
            boxes.Clear();

            // Visject Meta
            _meta.Save(stream);

            // Ending char
            stream.Write((byte)'\t');
        }

        private void LoadGraph(BinaryReader stream)
        {
            // IMPORTANT! This must match C++ Graph format

            // Magic Code
            int tmp = stream.ReadInt32();
            if (tmp != 1963542358)
            {
                // Error
                throw new Exception("Invalid Graph format version");
            }

            // Engine Build
            uint engineBuild = stream.ReadUInt32();

            // Load1
            {
                // Time saved (not used anymore to prevent binary diffs after saving unmodified surface)
                stream.ReadInt64();
                byte[] guidBytes = new byte[16];

                // Nodes count
                int nodesCount = stream.ReadInt32();
                if (Nodes.Capacity < nodesCount)
                    Nodes.Capacity = nodesCount;
                List<ConnectionHint> tmpHints = _cachedConnections.Value;
                tmpHints.Clear();
                tmpHints.Capacity = Mathf.Max(tmpHints.Capacity, nodesCount * 4);

                // Parameters count
                int parametersCount = stream.ReadInt32();
                if (Parameters.Capacity < parametersCount)
                    Parameters.Capacity = parametersCount;

                // For each node
                for (int i = 0; i < nodesCount; i++)
                {
                    // ID
                    uint id = stream.ReadUInt32();

                    // Type
                    ushort typeId = stream.ReadUInt16();
                    ushort groupId = stream.ReadUInt16();

                    // Create node
                    var node = NodeFactory.CreateNode(_surface.NodeArchetypes, id, this, groupId, typeId);
                    if (node == null)
                    {
                        // Error
                        throw new Exception("Cannot create graph node.");
                    }
                    Nodes.Add(node);
                }

                // For each param
                for (int i = 0; i < parametersCount; i++)
                {
                    // Create param
                    var param = new SurfaceParameter();
                    Parameters.Add(param);

                    // Properties
                    param.Type = (ParameterType)stream.ReadByte();
                    stream.Read(guidBytes, 0, 16);
                    param.ID = new Guid(guidBytes);
                    param.Name = Utils.ReadStr(stream, 97);
                    param.IsPublic = stream.ReadByte() != 0;
                    param.IsStatic = stream.ReadByte() != 0;
                    param.IsUIVisible = stream.ReadByte() != 0;
                    param.IsUIEditable = stream.ReadByte() != 0;

                    // References
                    int refsCount = stream.ReadInt32();
                    param.ReferencedBy.Capacity = refsCount;
                    for (int j = 0; j < refsCount; j++)
                    {
                        uint refID = stream.ReadUInt32();
                        var node = FindNode(refID);
                        if (node == null)
                        {
                            // Error
                            Editor.LogWarning($"Invalid node reference id (param: {param.Name}, node ref: {refID})");
                        }
                        else
                        {
                            param.ReferencedBy.Add(node);
                        }
                    }

                    // Value
                    Utils.ReadCommonValue(stream, ref param.Value);

                    // Meta
                    param.Meta.Load(engineBuild, stream);
                }

                // For each node
                for (int i = 0; i < nodesCount; i++)
                {
                    var node = Nodes[i];

                    // Values
                    int valuesCnt = stream.ReadInt32();
                    int nodeValuesCnt = node.Values?.Length ?? 0;
                    if (valuesCnt == nodeValuesCnt)
                    {
                        for (int j = 0; j < valuesCnt; j++)
                        {
                            // ReSharper disable once PossibleNullReferenceException
                            Utils.ReadCommonValue(stream, ref node.Values[j]);
                        }
                    }
                    else
                    {
                        Editor.LogWarning(String.Format("Invalid node values. Loaded: {0}, expected: {1}. Type: {2}, {3}", valuesCnt, nodeValuesCnt, node.Archetype.Title, node.Archetype.TypeID));

                        object dummy = null;
                        for (int j = 0; j < valuesCnt; j++)
                            Utils.ReadCommonValue(stream, ref dummy);
                    }

                    // Boxes
                    ushort boxesCount = stream.ReadUInt16();
                    for (int j = 0; j < boxesCount; j++)
                    {
                        var id = stream.ReadByte();
                        uint type = stream.ReadUInt32();
                        ushort connectionsCnt = stream.ReadUInt16();

                        ConnectionHint hint;
                        hint.NodeB = node;
                        hint.BoxB = id;

                        for (int k = 0; k < connectionsCnt; k++)
                        {
                            uint targetNodeID = stream.ReadUInt32();
                            byte targetBoxID = stream.ReadByte();

                            hint.NodeA = FindNode(targetNodeID);
                            if (hint.NodeA == null)
                            {
                                // Error
                                Editor.LogWarning("Invalid connected node id.");
                            }
                            else
                            {
                                hint.BoxA = targetBoxID;

                                tmpHints.Add(hint);
                            }
                        }
                    }

                    // Meta
                    node.Meta.Load(engineBuild, stream);

                    OnControlLoaded(node);
                }

                // Visject Meta
                _meta.Load(engineBuild, stream);

                // Setup connections
                for (int i = 0; i < tmpHints.Count; i++)
                {
                    var c = tmpHints[i];
                    var boxA = c.NodeA.GetBox(c.BoxA);
                    var boxB = c.NodeB.GetBox(c.BoxB);
                    if (boxA != null && boxB != null)
                    {
                        boxA.Connections.Add(boxB);
                    }
                }

                // Ending char
                byte end = stream.ReadByte();
                if (end != '\t')
                {
                    // Error
                    throw new Exception("Invalid data.");
                }
            }
        }

        /// <summary>
        /// Called when control gets added to the surface as spawn operation (eg. add new comment or add new node).
        /// </summary>
        /// <param name="control">The control.</param>
        public virtual void OnControlSpawned(SurfaceControl control)
        {
            control.OnSpawned();
        }

        /// <summary>
        /// Called when control gets loaded and should be added to the surface. Handles surface nodes initialization.
        /// </summary>
        /// <param name="control">The control.</param>
        public virtual void OnControlLoaded(SurfaceControl control)
        {
            if (control is SurfaceNode node)
            {
                // Initialize node
                OnNodeLoaded(node);
            }

            // Link control
            control.OnLoaded();
            control.Parent = RootControl;

            if (control is SurfaceComment)
            {
                // Move comments to the background
                control.IndexInParent = 0;
            }
        }

        /// <summary>
        /// Called when node gets loaded and should be added to the surface. Creates node elements from the archetype.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnNodeLoaded(SurfaceNode node)
        {
            // Create child elements of the node based on it's archetype
            int elementsCount = node.Archetype.Elements?.Length ?? 0;
            for (int i = 0; i < elementsCount; i++)
            {
                // ReSharper disable once PossibleNullReferenceException
                var arch = node.Archetype.Elements[i];
                ISurfaceNodeElement element = null;
                switch (arch.Type)
                {
                case NodeElementType.Input:
                    element = new InputBox(node, arch);
                    break;
                case NodeElementType.Output:
                    element = new OutputBox(node, arch);
                    break;
                case NodeElementType.BoolValue:
                    element = new BoolValue(node, arch);
                    break;
                case NodeElementType.FloatValue:
                    element = new FloatValue(node, arch);
                    break;
                case NodeElementType.IntegerValue:
                    element = new IntegerValue(node, arch);
                    break;
                case NodeElementType.ColorValue:
                    element = new ColorValue(node, arch);
                    break;
                case NodeElementType.ComboBox:
                    element = new ComboBoxElement(node, arch);
                    break;
                case NodeElementType.Asset:
                    element = new AssetSelect(node, arch);
                    break;
                case NodeElementType.Text:
                    element = new TextView(node, arch);
                    break;
                case NodeElementType.TextBox:
                    element = new TextBoxView(node, arch);
                    break;
                case NodeElementType.SkeletonNodeSelect:
                    element = new SkeletonNodeSelectElement(node, arch);
                    break;
                }
                if (element != null)
                {
                    // Link element
                    node.AddElement(element);
                }
            }

            // Load metadata
            var meta = node.Meta.GetEntry(11);
            if (meta.Data != null)
            {
                var meta11 = Utils.ByteArrayToStructure<VisjectSurface.Meta11>(meta.Data);
                node.Location = meta11.Position;
                //node.IsSelected = meta11.Selected;
            }
        }
    }
}
