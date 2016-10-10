// Celelej Game Engine scripting API

using System;
using System.Diagnostics;

namespace CelelejEngine
{
    /// <summary>
    /// Class containing methods to ease debugging while developing a game.
    /// </summary>
    public sealed class Debug
    {
        internal static Logger _logger;

        /// <summary>
        /// Get default debug logger.
        /// </summary>
        public static ILogger logger
        {
            get { return _logger; }
        }

        static Debug()
        {
            _logger = new Logger(new DebugLogHandler());
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void Assert(bool condition)
        {
            if (!condition)
                logger.Log(LogType.Assert, "Assertion failed");
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="context">Object to which the message applies.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void Assert(bool condition, Object context)
        {
            if (!condition)
                logger.Log(LogType.Assert, (object)"Assertion failed", context);
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void Assert(bool condition, object message)
        {
            if (!condition)
                logger.Log(LogType.Assert, message);
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="message">String to be converted to string representation for display.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void Assert(bool condition, string message)
        {
            if (!condition)
                logger.Log(LogType.Assert, message);
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void Assert(bool condition, object message, Object context)
        {
            if (!condition)
                logger.Log(LogType.Assert, message, context);
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="message">String to be converted to string representation for display.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void Assert(bool condition, string message, Object context)
        {
            if (!condition)
                logger.Log(LogType.Assert, (object)message, context);
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            if (!condition)
                logger.LogFormat(LogType.Assert, format, args);
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the Celelej console on failure.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AssertFormat(bool condition, Object context, string format, params object[] args)
        {
            if (!condition)
                logger.Log(LogType.Assert, context, string.Format(format, args));
        }
        
        /// <summary>
        /// Logs message to the Celelej Console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void Log(object message)
        {
            logger.Log(LogType.Log, message);
        }

        /// <summary>
        /// Logs message to the Celelej Console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(object message, Object context)
        {
            logger.Log(LogType.Log, message, context);
        }

        /// <summary>
        /// A variant of Debug.Log that logs an assertion message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void LogAssertion(object message)
        {
            logger.Log(LogType.Assert, message);
        }

        /// <summary>
        /// A variant of Debug.Log that logs an assertion message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void LogAssertion(object message, Object context)
        {
            logger.Log(LogType.Assert, message, context);
        }

        /// <summary>
        /// Logs a formatted assertion message to the Celelej console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            logger.LogFormat(LogType.Assert, format, args);
        }

        /// <summary>
        /// Logs a formatted assertion message to the Celelej console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void LogAssertionFormat(Object context, string format, params object[] args)
        {
            logger.Log(LogType.Assert, context, string.Format(format, args));
        }

        /// <summary>
        /// A variant of Debug.Log that logs an error message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void LogError(object message)
        {
            logger.Log(LogType.Error, message);
        }

        /// <summary>
        /// A variant of Debug.Log that logs an error message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogError(object message, Object context)
        {
            logger.Log(LogType.Error, message, context);
        }

        /// <summary>
        /// Logs a formatted error message to the Celelej console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogErrorFormat(string format, params object[] args)
        {
            logger.LogFormat(LogType.Error, format, args);
        }

        /// <summary>
        /// Logs a formatted error message to the Celelej console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            logger.Log(LogType.Error, context, string.Format(format, args));
        }

        /// <summary>
        /// A variant of Debug.Log that logs an error message to the console.
        /// </summary>
        /// <param name="exception">Runtime Exception.</param>
        public static void LogException(Exception exception)
        {
            logger.LogException(exception, null);
        }

        /// <summary>
        /// A variant of Debug.Log that logs an error message to the console.
        /// </summary>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="exception">Runtime Exception.</param>
        public static void LogException(Exception exception, Object context)
        {
            logger.LogException(exception, context);
        }

        /// <summary>
        /// Logs a formatted message to the Celelej Console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogFormat(string format, params object[] args)
        {
            logger.LogFormat(LogType.Log, format, args);
        }

        /// <summary>
        /// Logs a formatted message to the Celelej Console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogFormat(Object context, string format, params object[] args)
        {
            logger.Log(LogType.Log, context, string.Format(format, args));
        }
        
        /// <summary>
        /// A variant of Debug.Log that logs a warning message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void LogWarning(object message)
        {
            logger.Log(LogType.Warning, message);
        }

        /// <summary>
        /// A variant of Debug.Log that logs a warning message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogWarning(object message, Object context)
        {
            logger.Log(LogType.Warning, message, context);
        }

        /// <summary>
        /// Logs a formatted warning message to the Celelej Console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogWarningFormat(string format, params object[] args)
        {
            logger.Log(LogType.Warning, format, args);
        }

        /// <summary>
        /// Logs a formatted warning message to the Celelej Console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            logger.Log(LogType.Warning, context, string.Format(format, args));
        }
    }
}
