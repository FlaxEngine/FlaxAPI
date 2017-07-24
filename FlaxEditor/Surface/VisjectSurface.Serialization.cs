////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        // Note: surface serialization is port from c++ code base (also a legacy)
        // Refactor this in future togather with c++ backend

        private struct ConnectionHint
        {
            public SurfaceNode NodeA;
            public byte BoxA;
            public SurfaceNode NodeB;
            public byte BoxB;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        struct VisjectSurfaceMeta10// TypeID: 10, for surface
        {
            public Vector2 ViewPosition;
            public float Scale;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct VisjectSurfaceMeta11// TypeID: 11, for nodes
        {
            public Vector2 Position;
            public bool Selected;
        }

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            // #stuid c#
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }

        private static void ReadCommonValue(BinaryReader stream, ref object value)
        {
            byte type = stream.ReadByte();

            switch (type)
            {
                case 0:// CommonType::Bool:
                    value = stream.ReadByte() != 0;
                    break;
                case 1:// CommonType::Inteager:
                {
                    value = stream.ReadInt32();
                }
                    break;
                case 2:// CommonType::Float:
                {
                    value = stream.ReadSingle();
                }
                    break;
                case 3:// CommonType::Vector2:
                {
                    value = new Vector2(stream.ReadSingle(), stream.ReadSingle());
                }
                    break;
                case 4:// CommonType::Vector3:
                {
                    value = new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
                }
                    break;
                case 5:// CommonType::Vector4:
                {
                    value = new Vector4(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
                }
                    break;
                case 6:// CommonType::Color:
                {
                    value = new Color(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
                }
                    break;
                case 7:// CommonType::Guid:
                {
                    value = new Guid(stream.ReadBytes(16));
                }
                    break;
                /*case 8:// CommonType::String:
                {
                    String v;
                    ReadString(&v, 953);
                    data->Set(v);
                }
                    break;
                case 9:// CommonType::Box:
                {
                    BoundingBox v;
                    ReadBox(&v);
                    data->Set(v);
                }
                    break;
                case 10:// CommonType::Rotation:
                {
                    Quaternion v;
                    ReadQuaternion(&v);
                    data->Set(v);
                }
                    break;
                case 11:// CommonType::Transform:
                {
                    Transform v;
                    ReadTransform(&v);
                    data->Set(v);
                }
                    break;
                case 12:// CommonType::Sphere:
                {
                    BoundingSphere v;
                    ReadSphere(&v);
                    data->Set(v);
                }
                    break;
                case 13:// CommonType::Rect:
                {
                    Rect v;
                    ReadRect(&v);
                    data->Set(v);
                }
                    break;*/
                default: throw new SystemException();
            }
        }

        private unsafe bool loadGraph(BinaryReader stream)
        {
            // IMPORTANT! This must match C++ Graph format

            // Magic Code
            int tmp = stream.ReadInt32();
            if (tmp != 1963542358)
            {
                // Error
                Debug.LogWarning("Invalid Grpah format version");
                return true;
            }

            // Engine Build
            uint engineBuild = stream.ReadUInt32();

            // Load1
            {
                // Time saved
                DateTime tiemSaved = new DateTime(stream.ReadInt64());
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
                    var node = NodeFactory.CreateNode(id, this, groupId, typeId);
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
                    {
                        int length = stream.ReadInt32();
                        if (length > 0 && length < 2000)
                        {
                            var str = stream.ReadBytes(length * 2);
                            fixed (byte* strPtr = str)
                            {
                                var ptr = (char*)strPtr;
                                for (int j = 0; j < length; j++)
                                    ptr[j] = (char)(ptr[j] ^ 97);
                            }
                            param.Name = System.Text.Encoding.Unicode.GetString(str);
                        }
                    }
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
                            Debug.LogWarning($"Invalid node reference id (param: {param.Name}, node ref: {refID})");
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
                    if (valuesCnt == 0)
                    {
                        if (node.Values != null && node.Values.Length > 0)
                        {
                            // Error
                            Debug.LogWarning("Invalid node values.");
                            return true;
                        }
                    }
                    else
                    {
                        if (node.Values == null || node.Values.Length != valuesCnt)
                        {
                            // Error
                            Debug.LogWarning("Invalid node values.");
                            return true;
                        }

                        for (int j = 0; j < valuesCnt; j++)
                            ReadCommonValue(stream, ref node.Values[j]);
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
                                Debug.LogWarning("Invalid connected node id.");
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

                    OnNodeLoaded(node);
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
                    Debug.LogWarning("Invalid data.");
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
            Visible = false;

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
                MemoryStream stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    result = loadGraph(reader);
                }
            }
            catch (Exception ex)
            {
                // Error
                Debug.LogException(ex);
            }

            Visible = true;
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
                ViewPosition = meta10.ViewPosition;
            }
            else
            {
                // Reset view
                ViewScale = 1.0f;
                ViewPosition = Vector2.Zero;
            }

            // Post load
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].OnSurfaceLoaded();
            }

            // End
            _edited = false;
            Owner.OnSurfaceEditedChanged();

            return false;
        }
    }
}
