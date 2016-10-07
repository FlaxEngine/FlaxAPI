using System;

namespace CelelejEngine
{
    /// <summary>
    /// Initializes a new instance of the Logger.
    /// </summary>
    public class Logger : ILogger
    {
        private const string kNoTagFormat = "{0}";
        private const string kTagFormat = "{0}: {1}";

        private Logger()
        {
        }

        /// <summary>
        /// Create a custom Logger.
        /// </summary>
        /// <param name="logHandler">Pass in default log handler or custom log handler.</param>
        public Logger(ILogHandler logHandler)
        {
            this.logHandler = logHandler;
            logEnabled = true;
            filterLogType = LogType.Log;
        }

        private static string GetString(object message)
        {
            return message == null ? "null" : message.ToString();
        }

        /// <summary>
        /// To selective enable debug log message.
        /// </summary>
        public LogType filterLogType { get; set; }

        /// <summary>
        /// To runtime toggle debug logging [ON/OFF].
        /// </summary>
        public bool logEnabled { get; set; }

        /// <summary>
        /// Set  Logger.ILogHandler.
        /// </summary>
        public ILogHandler logHandler { get; set; }

        /// <summary>
        /// Check logging is enabled based on the LogType.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <returns>
        /// Retrun true in case logs of LogType will be logged otherwise returns false.
        /// </returns>
        public bool IsLogTypeAllowed(LogType logType)
        {
            return logEnabled && (logType <= filterLogType || logType == LogType.Exception);
        }

        /// <summary>
        /// Logs message to the Celelej Console using default logger.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void Log(LogType logType, object message)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, null, GetString(message));
        }

        /// <summary>
        /// Logs message to the Celelej Console using default logger.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void Log(LogType logType, object message, Object context)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, context, GetString(message));
        }

        /// <summary>
        /// Logs message to the Celelej Console using default logger.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void Log(LogType logType, string tag, object message)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, null, string.Format(kTagFormat, tag, GetString(message)));
        }

        /// <summary>
        /// Logs message to the Celelej Console using default logger.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void Log(LogType logType, string tag, object message, Object context)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, context, string.Format(kTagFormat, tag, GetString(message)));
        }

        /// <summary>
        /// Logs message to the Celelej Console using default logger.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void Log(object message)
        {
            if (IsLogTypeAllowed(LogType.Log))
                logHandler.Log(LogType.Log, null, GetString(message));
        }

        /// <summary>
        /// Logs message to the Celelej Console using default logger.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void Log(string tag, object message)
        {
            if (IsLogTypeAllowed(LogType.Log))
                logHandler.Log(LogType.Log, null, string.Format(kTagFormat, tag, GetString(message)));
        }

        /// <summary>
        /// Logs message to the Celelej Console using default logger.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void Log(string tag, object message, Object context)
        {
            if (IsLogTypeAllowed(LogType.Log))
                logHandler.Log(LogType.Log, context, string.Format(kTagFormat, tag, GetString(message)));
        }

        /// <summary>
        /// A variant of Logger.Log that logs an error message.
        /// </summary>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void LogError(string tag, object message)
        {
            if (IsLogTypeAllowed(LogType.Error))
                logHandler.Log(LogType.Error, null, string.Format(kTagFormat, tag, GetString(message)));
        }

        /// <summary>
        /// A variant of Logger.Log that logs an error message.
        /// </summary>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogError(string tag, object message, Object context)
        {
            if (IsLogTypeAllowed(LogType.Error))
                logHandler.Log(LogType.Error, context, string.Format(kTagFormat, tag, GetString(message)));
        }

        /// <summary>
        /// A variant of Logger.Log that logs an exception message.
        /// </summary>
        /// <param name="exception">Runtime Exception.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogException(Exception exception)
        {
            if (logEnabled)
                logHandler.LogException(exception, null);
        }

        /// <summary>
        /// A variant of Logger.Log that logs an exception message.
        /// </summary>
        /// <param name="exception">Runtime Exception.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogException(Exception exception, Object context)
        {
            if (logEnabled)
                logHandler.LogException(exception, context);
        }

        public void Log(LogType logType, Object context, string message)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, context, message);
        }

        public void LogFormat(LogType logType, string format, params object[] args)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, null, string.Format(format, args));
        }

        /// <summary>
        /// Logs a formatted message.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public void Log(LogType logType, string format, params object[] args)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, null, string.Format(format, args));
        }

        /// <summary>
        /// Logs a formatted message.
        /// </summary>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public void Log(LogType logType, Object context, string format, params object[] args)
        {
            if (IsLogTypeAllowed(logType))
                logHandler.Log(logType, context, string.Format(format, args));
        }

        /// <summary>
        /// A variant of Logger.Log that logs an warning message.
        /// </summary>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void LogWarning(string tag, object message)
        {
            if (IsLogTypeAllowed(LogType.Warning))
                logHandler.Log(LogType.Warning, null, string.Format(kTagFormat, tag, GetString(message)));
        }

        /// <summary>
        /// A variant of Logger.Log that logs an warning message.
        /// </summary>
        /// <param name="tag">
        /// Used to identify the source of a log message. It usually identifies the class where the log call
        /// occurs.
        /// </param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogWarning(string tag, object message, Object context)
        {
            if (IsLogTypeAllowed(LogType.Warning))
                logHandler.Log(LogType.Warning, context, string.Format(kTagFormat, tag, GetString(message)));
        }
    }
}
