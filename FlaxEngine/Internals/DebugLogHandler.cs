////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    internal sealed class DebugLogHandler : ILogHandler
    {
        /// <summary>
        /// Occurs on sending a log message.
        /// </summary>
        public event LogDelegate SendLog;

        /// <summary>
        /// Occurs on sending a log message.
        /// </summary>
        public event LogExceptionDegetae SendExceptionLog;

        public void LogException(Exception exception, Object context)
        {
            Internal_LogException(exception, context?.unmanagedPtr ?? IntPtr.Zero);
            
            SendExceptionLog?.Invoke(exception, context);
        }

        public void Log(LogType logType, Object context, string message)
        {
#if DEBUG
            string stackTrace = Environment.StackTrace;
#else
            string stackTrace = string.Empty;
#endif
            Internal_Log(logType, message, context?.unmanagedPtr ?? IntPtr.Zero, stackTrace);

            SendLog?.Invoke(logType, message, context, stackTrace);
        }

        internal static void Internal_SendLog(LogType type, string message)
        {
            Debug.Logger.Log(type, message);
        }

        internal static void Internal_SendLogException(Exception exception)
        {
            Debug.Logger.LogException(exception);
        }
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Log(LogType level, string msg, IntPtr obj, string stackTrace);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LogException(Exception exception, IntPtr obj);
    }
}
