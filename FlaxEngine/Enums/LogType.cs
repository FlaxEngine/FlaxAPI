// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// The type of the log message in <see cref="Debug"/>.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// LogType used for Errors.
        /// </summary>
        Error = 0,

        /// <summary>
        /// LogType used for Asserts. (These could also indicate an error inside Flax itself.)
        /// </summary>
        Assert = 1,

        /// <summary>
        /// LogType used for Warnings.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// LogType used for regular log messages.
        /// </summary>
        Log = 3,

        /// <summary>
        /// LogType used for Exceptions.
        /// </summary>
        Exception = 4
    }
}
