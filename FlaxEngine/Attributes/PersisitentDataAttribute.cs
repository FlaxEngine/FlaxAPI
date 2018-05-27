using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    ///     This attributes provides simple way of mapping data from fields and properties to persistant data structure.
    /// </summary>
    /// <remarks>
    ///     Multiple ways of saving values are possible using this method.
    ///     - Manual flush <see cref="BasePersistentData.Flush()" />
    ///     - Time based flush <see cref="PersistentDataConfig" />
    ///     - Incrementation manual counter <see cref="PersistentDataConfig" /> <see cref="BasePersistentData.Call()" />
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PersisitentDataAttribute : Attribute
    {
        /// <summary>
        ///     Override value for mapped "key" value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Custom file name used to store this key-value pair
        /// </summary>
        public string File { get; set; }

        /// <summary>
        ///     Custom config name mapped to the File name for this key-value pair
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        ///     Maps marked field to the global persistant data file using default Field/Property name format
        /// </summary>
        public PersisitentDataAttribute()
        {
        }

        /// <summary>
        ///     Maps marked field to the global persistant data file with custom key format
        /// </summary>
        /// <param name="key">Override value for mapped "key" value</param>
        public PersisitentDataAttribute(string key)
        {
            Key = key;
        }

        /// <summary>
        ///     Maps marked field to the specified persistant data file with custom key format
        /// </summary>
        /// <param name="key">Override value for mapped "key" value</param>
        /// <param name="file">Custom file name used to store this key-value pair</param>
        /// <param name="isConfigName">If true, file name is instead configName</param>
        public PersisitentDataAttribute(string key, string file, bool isConfigName = false)
        {
            Key = key;
            if (isConfigName)
            {
                File = file;
            }
            else
            {
                ConfigName = file;
            }
        }
    }
}
