////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Metadata container.
    /// </summary>
    public class SurfaceMeta
    {
        /// <summary>
        /// Metadata entry
        /// </summary>
        public struct Entry
        {
            public int TypeID;
            public DateTime CreationTime;
            public byte[] Data;
        }

        /// <summary>
        /// All meta entries
        /// </summary>
        public readonly List<Entry> Entries = new List<Entry>();

        /// <summary>
        /// Load from the stream
        /// </summary>
        /// <param name="engineBuild">Saved Visject Surface engine build number code</param>
        /// <param name="stream">Stream</param>
        public void Load(uint engineBuild, BinaryReader stream)
        {
            Release();

            // Version 1
            {
                int entries = stream.ReadInt32();

                Entries.Capacity = entries;

                for (int i = 0; i < entries; i++)
                {
                    Entry e = new Entry();

                    e.TypeID = stream.ReadInt32();
                    e.CreationTime = new DateTime(stream.ReadInt64());

                    uint dataSize = stream.ReadUInt32();
                    e.Data = new byte[dataSize];
                    stream.Read(e.Data, 0, (int)dataSize);

                    Entries.Add(e);
                }
            }
        }

        /// <summary>
        /// Save to the stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="saveData">True if load meta data</param>
        /// <returns>True if cannot save data</returns>
        /*bool Save(WriteStream* stream, bool saveData)
        {
            
        }*/

        /// <summary>
        /// Releases meta data.
        /// </summary>
        public void Release()
        {
            Entries.Clear();
            Entries.TrimExcess();
        }

        /// <summary>
        /// Gets the entry.
        /// </summary>
        /// <param name="typeID">Entry type ID</param>
        /// <returns>Entry</returns>
        public Entry GetEntry(int typeID)
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].TypeID == typeID)
                {
                    return Entries[i];
                }
            }

            return new Entry();
        }

        /// <summary>
        /// Adds new entry.
        /// </summary>
        /// <param name="typeID">Type ID</param>
        /// <param name="data">Bytes to set</param>
        public void AddEntry(int typeID, byte[] data)
        {
            Entry e = new Entry();
            e.CreationTime = DateTime.UtcNow;
            e.TypeID = typeID;
            e.Data = data;
            Entries.Add(e);
        }
    }
}
