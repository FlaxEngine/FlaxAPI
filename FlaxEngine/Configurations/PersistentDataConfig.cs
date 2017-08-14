using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public class PersistentDataConfig
    {
        /// <summary>
        /// Logs an error if tried to write to ReadOnly file
        /// </summary>
        public bool ReadOnly { get; set; }
        /// <summary>
        /// Preloades persistetnt data asynchronysly
        /// </summary>
        /// <remarks>
        /// Default loads on first use
        /// </remarks>
        public PreloadSettings PreloadFile { get; set; }
        /// <summary>
        /// Defines how automatic cache should behave between scenes changing
        /// </summary>
        public CacheSettings CacheOptions { get; set; }
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

        public class FlushSettings
        {
        }

        public class PreloadSettings
        {
        }


        public class CacheSettings
        {
        }
    }

    public class CompressionSettings
    {
    }

    public class CypherSettings
    {
    }
}
