// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using Newtonsoft.Json;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
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

        [StructLayout(LayoutKind.Sequential)]
        struct VisjectSurfaceMeta10 // TypeID: 10, for surface
        {
            public Vector2 ViewCenterPosition;
            public float Scale;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct VisjectSurfaceMeta11 // TypeID: 11, for nodes
        {
            public Vector2 Position;
            public bool Selected;
        }

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            // #stupid c#
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }

        private static byte[] StructureToByteArray<T>(ref T stuff) where T : struct
        {
            // #stupid c#
            int size = Marshal.SizeOf(typeof(T));
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(stuff, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        private static unsafe string ReadStr(BinaryReader stream, int check)
        {
            int length = stream.ReadInt32();
            if (length > 0 && length < 2000)
            {
                var str = stream.ReadBytes(length * 2);
                fixed (byte* strPtr = str)
                {
                    var ptr = (char*)strPtr;
                    for (int j = 0; j < length; j++)
                        ptr[j] = (char)(ptr[j] ^ check);
                }
                return System.Text.Encoding.Unicode.GetString(str);
            }

            return string.Empty;
        }

        private static unsafe void WriteStr(BinaryWriter stream, string str, int check)
        {
            int length = str.Length;
            stream.Write(length);
            var bytes = System.Text.Encoding.Unicode.GetBytes(str);
            if (bytes.Length != length * 2)
                throw new ArgumentException();
            fixed (byte* bytesPtr = bytes)
            {
                var ptr = (char*)bytesPtr;
                for (int j = 0; j < length; j++)
                    ptr[j] = (char)(ptr[j] ^ check);
            }
            stream.Write(bytes);
        }

        private static void ReadCommonValue(BinaryReader stream, ref object value)
        {
            byte type = stream.ReadByte();

            switch (type)
            {
            case 0: // CommonType::Bool:
                value = stream.ReadByte() != 0;
                break;
            case 1: // CommonType::Integer:
            {
                value = stream.ReadInt32();
            }
                break;
            case 2: // CommonType::Float:
            {
                value = stream.ReadSingle();
            }
                break;
            case 3: // CommonType::Vector2:
            {
                value = new Vector2(stream.ReadSingle(), stream.ReadSingle());
            }
                break;
            case 4: // CommonType::Vector3:
            {
                value = new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
            }
                break;
            case 5: // CommonType::Vector4:
            {
                value = new Vector4(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
            }
                break;
            case 6: // CommonType::Color:
            {
                value = new Color(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
            }
                break;
            case 7: // CommonType::Guid:
            {
                value = new Guid(stream.ReadBytes(16));
            }
                break;
            case 8: // CommonType::String:
            {
                int length = stream.ReadInt32();
                if (length <= 0)
                {
                    value = string.Empty;
                }
                else
                {
                    var data = new char[length];
                    for (int i = 0; i < length; i++)
                    {
                        var c = stream.ReadUInt16();
                        data[i] = (char)(c ^ 953);
                    }
                    value = new string(data);
                }
                break;
            }
            /*case 9:// CommonType::Box:
            {
                BoundingBox v;
                ReadBox(&v);
                data.Set(v);
            }
                break;
            case 10:// CommonType::Rotation:
            {
                Quaternion v;
                ReadQuaternion(&v);
                data.Set(v);
            }
                break;
            case 11:// CommonType::Transform:
            {
                Transform v;
                ReadTransform(&v);
                data.Set(v);
            }
                break;
            case 12:// CommonType::Sphere:
            {
                BoundingSphere v;
                ReadSphere(&v);
                data.Set(v);
            }
                break;
            case 13:// CommonType::Rect:
            {
                Rect v;
                ReadRect(&v);
                data.Set(v);
            }
                break;*/
            case 15: // CommonType::Matrix
            {
                value = new Matrix(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(),
                                   stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(),
                                   stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(),
                                   stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
                break;
            }
            case 16: // CommonType::Blob
            {
                int length = stream.ReadInt32();
                value = stream.ReadBytes(length);
                break;
            }
            default: throw new SystemException();
            }
        }

        private static void WriteCommonValue(BinaryWriter stream, object value)
        {
            if (value is bool asBool)
            {
                stream.Write((byte)0);
                stream.Write((byte)(asBool ? 1 : 0));
            }
            else if (value is int asInt)
            {
                stream.Write((byte)1);
                stream.Write(asInt);
            }
            else if (value is float asFloat)
            {
                stream.Write((byte)2);
                stream.Write(asFloat);
            }
            else if (value is double asDouble)
            {
                stream.Write((byte)2);
                stream.Write((float)asDouble);
            }
            else if (value is Vector2 asVector2)
            {
                stream.Write((byte)3);
                stream.Write(asVector2.X);
                stream.Write(asVector2.Y);
            }
            else if (value is Vector3 asVector3)
            {
                stream.Write((byte)4);
                stream.Write(asVector3.X);
                stream.Write(asVector3.Y);
                stream.Write(asVector3.Z);
            }
            else if (value is Vector4 asVector4)
            {
                stream.Write((byte)5);
                stream.Write(asVector4.X);
                stream.Write(asVector4.Y);
                stream.Write(asVector4.Z);
                stream.Write(asVector4.W);
            }
            else if (value is Color asColor)
            {
                stream.Write((byte)6);
                stream.Write(asColor.R);
                stream.Write(asColor.G);
                stream.Write(asColor.B);
                stream.Write(asColor.A);
            }
            else if (value is Guid asGuid)
            {
                stream.Write((byte)7);
                stream.Write(asGuid.ToByteArray());
            }
            else if (value is string asString)
            {
                stream.Write((byte)8);
                stream.Write(asString.Length);
                for (int i = 0; i < asString.Length; i++)
                    stream.Write((ushort)(asString[i] ^ 953));
            }
            else if (value is Matrix asMatrix)
            {
                stream.Write((byte)15);
                stream.Write(asMatrix.M11);
                stream.Write(asMatrix.M12);
                stream.Write(asMatrix.M13);
                stream.Write(asMatrix.M14);
                stream.Write(asMatrix.M21);
                stream.Write(asMatrix.M22);
                stream.Write(asMatrix.M23);
                stream.Write(asMatrix.M24);
                stream.Write(asMatrix.M31);
                stream.Write(asMatrix.M32);
                stream.Write(asMatrix.M33);
                stream.Write(asMatrix.M34);
                stream.Write(asMatrix.M41);
                stream.Write(asMatrix.M42);
                stream.Write(asMatrix.M43);
                stream.Write(asMatrix.M44);
            }
            else if (value is byte[] asBlob)
            {
                stream.Write((byte)16);
                stream.Write(asBlob.Length);
                stream.Write(asBlob);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static void WriteCommonValue(JsonWriter stream, object value)
        {
            if (value is bool asBool)
            {
                stream.WriteValue(asBool);
            }
            else if (value is int asInt)
            {
                stream.WriteValue(asInt);
            }
            else if (value is float asFloat)
            {
                stream.WriteValue(asFloat);
            }
            else if (value is Vector2 asVector2)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("X");
                stream.WriteValue(asVector2.X);
                stream.WritePropertyName("Y");
                stream.WriteValue(asVector2.Y);
                stream.WriteEndObject();
            }
            else if (value is Vector3 asVector3)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("X");
                stream.WriteValue(asVector3.X);
                stream.WritePropertyName("Y");
                stream.WriteValue(asVector3.Y);
                stream.WritePropertyName("Z");
                stream.WriteValue(asVector3.Z);
                stream.WriteEndObject();
            }
            else if (value is Vector4 asVector4)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("X");
                stream.WriteValue(asVector4.X);
                stream.WritePropertyName("Y");
                stream.WriteValue(asVector4.Y);
                stream.WritePropertyName("Z");
                stream.WriteValue(asVector4.Z);
                stream.WritePropertyName("W");
                stream.WriteValue(asVector4.W);
                stream.WriteEndObject();
            }
            else if (value is Color asColor)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("R");
                stream.WriteValue(asColor.R);
                stream.WritePropertyName("G");
                stream.WriteValue(asColor.G);
                stream.WritePropertyName("B");
                stream.WriteValue(asColor.B);
                stream.WritePropertyName("A");
                stream.WriteValue(asColor.A);
                stream.WriteEndObject();
            }
            else if (value is Rectangle asRectangle)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("Location");
                WriteCommonValue(stream, asRectangle.Location);
                stream.WritePropertyName("Size");
                WriteCommonValue(stream, asRectangle.Size);
                stream.WriteEndObject();
            }
            else if (value is Guid asGuid)
            {
                stream.WriteValue(asGuid);
            }
            else if (value is string asString)
            {
                stream.WriteValue(asString);
            }
            else if (value is Matrix asMatrix)
            {
                stream.WriteStartObject();

                stream.WritePropertyName("M11");
                stream.WriteValue(asMatrix.M11);
                stream.WritePropertyName("M12");
                stream.WriteValue(asMatrix.M12);
                stream.WritePropertyName("M13");
                stream.WriteValue(asMatrix.M13);
                stream.WritePropertyName("M14");
                stream.WriteValue(asMatrix.M14);

                stream.WritePropertyName("M21");
                stream.WriteValue(asMatrix.M21);
                stream.WritePropertyName("M22");
                stream.WriteValue(asMatrix.M22);
                stream.WritePropertyName("M23");
                stream.WriteValue(asMatrix.M23);
                stream.WritePropertyName("M24");
                stream.WriteValue(asMatrix.M24);

                stream.WritePropertyName("M31");
                stream.WriteValue(asMatrix.M31);
                stream.WritePropertyName("M32");
                stream.WriteValue(asMatrix.M32);
                stream.WritePropertyName("M33");
                stream.WriteValue(asMatrix.M33);
                stream.WritePropertyName("M34");
                stream.WriteValue(asMatrix.M34);

                stream.WritePropertyName("M41");
                stream.WriteValue(asMatrix.M41);
                stream.WritePropertyName("M42");
                stream.WriteValue(asMatrix.M42);
                stream.WritePropertyName("M43");
                stream.WriteValue(asMatrix.M43);
                stream.WritePropertyName("M44");
                stream.WriteValue(asMatrix.M44);

                stream.WriteEndObject();
            }
            else if (value is byte[] asBlob)
            {
                stream.WriteValue(Convert.ToBase64String(asBlob));
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private bool LoadGraph(BinaryReader stream)
        {
            // IMPORTANT! This must match C++ Graph format

            // Magic Code
            int tmp = stream.ReadInt32();
            if (tmp != 1963542358)
            {
                // Error
                Editor.LogWarning("Invalid Graph format version");
                return true;
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
                if (_nodes.Capacity < nodesCount)
                    _nodes.Capacity = nodesCount;
                List<ConnectionHint> tmpHints = new List<ConnectionHint>(nodesCount * 4);

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
                    var node = NodeFactory.CreateNode(NodeArchetypes, id, this, groupId, typeId);
                    if (node == null)
                    {
                        // Error
                        return true;
                    }
                    _nodes.Add(node);
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
                    param.Name = ReadStr(stream, 97);
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
                    ReadCommonValue(stream, ref param.Value);

                    // Meta
                    param.Meta.Load(engineBuild, stream);
                }

                // For each node
                for (int i = 0; i < nodesCount; i++)
                {
                    var node = _nodes[i];

                    // Values
                    int valuesCnt = stream.ReadInt32();
                    int nodeValuesCnt = node.Values?.Length ?? 0;
                    if (valuesCnt == nodeValuesCnt)
                    {
                        for (int j = 0; j < valuesCnt; j++)
                            ReadCommonValue(stream, ref node.Values[j]);
                    }
                    else
                    {
                        Editor.LogWarning(string.Format("Invalid node values. Loaded: {0}, expected: {1}. Type: {2}, {3}", valuesCnt, nodeValuesCnt, node.Archetype.Title, node.Archetype.TypeID));

                        object dummy = null;
                        for (int j = 0; j < valuesCnt; j++)
                            ReadCommonValue(stream, ref dummy);
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
                Meta.Load(engineBuild, stream);

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
                    Editor.LogWarning("Invalid data.");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Loads surface from the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool Load(byte[] bytes)
        {
            Enabled = false;

            // Clean data
            Parameters.Clear();
            _nodes.Clear();
            _startBox = null;
            _lastBoxUnderMouse = null;
            _surface.DisposeChildren();

            bool result = true;
            try
            {
                // Load graph
                using (var stream = new MemoryStream(bytes))
                using (var reader = new BinaryReader(stream))
                {
                    result = LoadGraph(reader);
                }
            }
            catch (Exception ex)
            {
                // Error
                Editor.LogWarning(ex);
            }

            Enabled = true;

            if (result)
            {
                // Error
                return true;
            }

            // Load surface meta
            var meta = Meta.GetEntry(10);
            if (meta.Data != null)
            {
                var meta10 = ByteArrayToStructure<VisjectSurfaceMeta10>(meta.Data);
                ViewScale = meta10.Scale;
                ViewCenterPosition = meta10.ViewCenterPosition;
            }
            else
            {
                // Reset view
                ViewScale = 1.0f;
                ViewCenterPosition = Vector2.Zero;
            }

            // Load surface comments
            var commentsData = Meta.GetEntry(666);
            if (commentsData.Data != null)
            {
                using (var stream = new MemoryStream(commentsData.Data))
                using (var reader = new BinaryReader(stream))
                {
                    var commentsCount = reader.ReadInt32();

                    for (int i = 0; i < commentsCount; i++)
                    {
                        var title = ReadStr(reader, 71);
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
            for (int i = 0; i < _surface.Children.Count; i++)
            {
                if (_surface.Children[i] is SurfaceControl control)
                    control.OnSurfaceLoaded();
            }

            // Update boxes types for nodes that dependant box types based on incoming connections
            {
                bool keepUpdating = false;
                int updateLimit = 100;
                do
                {
                    for (int i = 0; i < _surface.Children.Count; i++)
                    {
                        if (_surface.Children[i] is SurfaceNode node && !node.HasDependentBoxesSetup)
                        {
                            node.UpdateBoxesTypes();
                            keepUpdating = true;
                        }
                    }
                } while (keepUpdating && updateLimit-- > 0);
            }

            // End
            _edited = false;
            Owner.OnSurfaceEditedChanged();

            return false;
        }

        private bool SaveGraph(BinaryWriter stream)
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
            stream.Write(_nodes.Count);

            // Parameters count
            stream.Write(Parameters.Count);

            // For each node
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];

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
                WriteStr(stream, param.Name, 97);
                stream.Write((byte)(param.IsPublic ? 1 : 0));
                stream.Write((byte)(param.IsStatic ? 1 : 0));
                stream.Write((byte)(param.IsUIVisible ? 1 : 0));
                stream.Write((byte)(param.IsUIEditable ? 1 : 0));

                // References
                stream.Write(param.ReferencedBy.Count);
                for (int j = 0; j < param.ReferencedBy.Count; j++)
                    stream.Write(param.ReferencedBy[j].ID);

                // Value
                WriteCommonValue(stream, param.Value);

                // Meta
                param.Meta.Save(stream);
            }

            // For each node
            var boxes = new List<Box>();
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];

                // Values
                if (node.Values != null)
                {
                    stream.Write(node.Values.Length);
                    for (int j = 0; j < node.Values.Length; j++)
                        WriteCommonValue(stream, node.Values[j]);
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
                            // Error
                            return true;
                        }

                        stream.Write(targetBox.ParentNode.ID);
                        stream.Write((byte)targetBox.ID);
                    }
                }

                // Meta
                node.Meta.Save(stream);
            }

            // Visject Meta
            Meta.Save(stream);

            // Ending char
            stream.Write((byte)'\t');

            return false;
        }

        /// <summary>
        /// Saves the surface graph to bytes.
        /// </summary>
        /// <returns>The bytes with surface data or null if failed.</returns>
        public byte[] Save()
        {
            var hasFocus = IsFocused;
            Enabled = false;

            // Save surface meta
            VisjectSurfaceMeta10 meta10;
            meta10.ViewCenterPosition = ViewCenterPosition;
            meta10.Scale = ViewScale;
            Meta.Release();
            Meta.AddEntry(10, StructureToByteArray(ref meta10));

            // Save surface comments in surface meta container
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

                        WriteStr(writer, comment.Title, 71);
                        writer.Write(comment.Color.R);
                        writer.Write(comment.Color.G);
                        writer.Write(comment.Color.B);
                        writer.Write(comment.Color.A);
                        writer.Write(comment.X);
                        writer.Write(comment.Y);
                        writer.Write(comment.Width);
                        writer.Write(comment.Height);
                    }

                    Meta.AddEntry(666, stream.ToArray());
                }
            }

            // Save all nodes meta
            VisjectSurfaceMeta11 meta11;
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                meta11.Position = node.Location;
                meta11.Selected = node.IsSelected;
                node.Meta.Release();
                node.Meta.AddEntry(11, StructureToByteArray(ref meta11));
            }

            // Save graph
            bool result = true;
            byte[] bytes = null;
            try
            {
                // Save graph
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    result = SaveGraph(writer);
                    if (result == false)
                        bytes = stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Error
                Editor.LogWarning(ex);
            }

            Enabled = true;
            if (hasFocus) Focus();

            if (result)
            {
                // Error
                return null;
            }

            // Clear flag
            if (_edited)
            {
                _edited = false;
                Owner.OnSurfaceEditedChanged();
            }

            return bytes;
        }
    }
}
