// Celelej Game Engine scripting API

using System;

namespace CelelejEngine
{
    public interface ILogHandler
    {
        /// <summary>
        /// <para>A variant of ILogHandler.LogFormat that logs an exception message.</para>
        /// </summary>
        /// <param name="exception">Runtime Exception.</param>
        /// <param name="context">Object to which the message applies.</param>
        void LogException(Exception exception, Object context);

        /// <summary>
        /// <para>Logs a formatted message.</para>
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="message">Message to log.</param>
        void Log(LogType logType, Object context, string message);
    }
}
