// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
            /// <summary>
            /// The type identifier.
            /// </summary>
            public int TypeID;

            /// <summary>
            /// The data.
            /// </summary>
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
                    stream.ReadInt64(); // don't use CreationTime

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
        /// <returns>True if cannot save data</returns>
        public void Save(BinaryWriter stream)
        {
            stream.Write(Entries.Count);

            for (int i = 0; i < Entries.Count; i++)
            {
                Entry e = Entries[i];

                stream.Write(e.TypeID);
                stream.Write((long)0);

                uint dataSize = e.Data != null ? (uint)e.Data.Length : 0;
                stream.Write(dataSize);
                if (dataSize > 0)
                {
                    stream.Write(e.Data);
                }
            }
        }

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
            Entry e = new Entry
            {
                TypeID = typeID,
                Data = data
            };
            Entries.Add(e);
        }
    }
}
