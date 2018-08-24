// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Configuration for persistant data
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PersistentDataConfig
    {
        /// <summary>
        /// Logs an error if tried to write to ReadOnly file
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Preloades persistetnt data settings
        /// </summary>
        //public PreloadSettingsEnum PreloadFile { get; set; }
        /// <summary>
        /// Defines encrypting and decrypting options
        /// </summary>
        public CypherSettings CypherOptions { get; set; }

        /// <summary>
        /// Defines if multiple settings should be stored inside one global file
        /// </summary>
        /// <remarks>
        /// Requires <see cref="PersistentDataConfig"/> with below provided file name, or first used <see cref="PersistentDataConfig"/> will be used
        /// </remarks>
        public string StoreAsOneFile { get; set; }

        /// <summary>
        /// Defines if when loading data should be downloaded from provided address
        /// </summary>
        public string DownloadFromNetworkResource { get; set; }

        /// <summary>
        /// Defines automatic flush options
        /// </summary>
        public FlushSettings FlushOptions { get; set; }

        /// <summary>
        /// Defines commpressions options
        /// </summary>
        public CompressionSettings CompressionOptions { get; set; }

        /// <summary>
        /// Defines if persistatnt data file should have checksum
        /// </summary>
        public bool ValidateWithChecksum { get; set; }

        /// <summary>
        /// Configuration options for automatic or manual flush
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct FlushSettings
        {
            /// <summary>
            /// Overrides all other flush options if true. Do not perform any automatic flushing
            /// </summary>
            public bool IsManualFlush { get; set; }
        }

        /// <summary>
        /// Enum defining preload data mode
        /// </summary>
        public enum PreloadSettingsEnum
        {
            /// <summary>
            /// PersistentData will not be loaded automaticaly
            /// <para>Load has to be performed manually, or data will be set to default until overwritten</para>
            /// </summary>
            Never = 0,

            /// <summary>
            /// PersistentData will be loaded automaticaly before scene loading is performed
            /// </summary>
            /// <remarks>
            /// Most useful when scene requires some data from file. Same effect will be achieved when used <see cref="OnFirstUse"/>
            /// </remarks>
            BeforeSceneLoad = 10,

            /// <summary>
            /// PersistentData will be loaded automaticaly before scene loading is performed. 
            /// </summary>
            /// <remarks>Most of the scene initialization is performed, but object creation is waiting for data to be loaded</remarks>
            /// <seealso cref="BeforeSceneLoad"/>
            BeforeSceneLoadAsynchronus = 11,

            /// <summary>
            /// PersistentData will be loaded after scene is loaded and all objects on the scene are created and before Awake is executed
            /// <para>
            /// Most useful when object are initialized with persistant data but we want to include settings on first loading screen.
            /// </para>
            /// <para>
            /// Same effect will be achieved when used <see cref="OnFirstUse"/>
            /// </para>
            /// </summary>
            BeforeObjectInitialization = 20,

            /// <summary>
            /// PersistentData will be loaded after scene is loaded and all objects on the scene are created and before Awake is executed
            /// </summary>
            /// <remarks>Most useful when object are initialized with persistant data but we want to include settings on first loading screen.</remarks>
            /// <seealso cref="BeforeObjectInitialization"/>
            BeforeObjectInitializationAsynchronus = 21,

            /// <summary>
            /// Persistent data is loaded before first Get or Save is used
            /// </summary>
            OnFirstUse = 50,
        }
    }

    /// <summary>
    /// Configuration for compression
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CompressionSettings
    {
        /// <summary>
        /// Use plain text, do not compress files
        /// </summary>
        public bool UsePlainText { get; set; }
    }

    /// <summary>
    /// Configuration for cypher
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CypherSettings
    {
        /// <summary>
        /// Do not use cypher, and leave plain text
        /// </summary>
        public bool UsePlainText { get; set; }
    }
}
