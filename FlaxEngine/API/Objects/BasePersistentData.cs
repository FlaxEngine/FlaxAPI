// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public abstract partial class BasePersistentData
    {
        /// <summary>
        /// Gets persistant data from desired file.
        /// </summary>
        /// <param name="fileName">File to get persistant data from.</param>
        /// <param name="createNew">If file was not found, should method create new file.</param>
        /// <typeparam name="T">PersistentData expected type</typeparam>
        /// <returns>null if file was not found.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static T File<T>(string fileName, bool createNew = true) where T : BasePersistentData
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return (T)Internal_File(fileName, createNew);
#endif
        }

        /// <summary>
        /// Gets persistant data from desired file by its config mapped name.
        /// </summary>
        /// <param name="fileName">File to get persistant data from by name from internal dictionary. Has to be mapped in config file.</param>
        /// <param name="createNew">If file was not found, should method create new file.</param>
        /// <typeparam name="T">PersistentData expected type</typeparam>
        /// <returns>null if file was not found.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static T FileByName<T>(string fileName, bool createNew = true) where T : BasePersistentData
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return (T)Internal_FileByName(fileName, createNew);
#endif
        }

        /// <summary>
        /// Internal counter for amount of changes insinde persistent data collection. 
        /// </summary>
        /// <remarks>
        /// Increments number of changes made to current <see cref="BasePersistentData"/> and based on <see cref="PersistentDataConfig"/> 
        /// if number is exceeded <see cref="BasePersistentData.Flush"/> is called. Multiple methods of automatic flush can be combined.
        /// </remarks>
        /// <param name="amount"></param>
        /// <seealso cref="PersistentDataConfig"/>
        public void Increment(int amount = 1)
        {
        }
    }
}
