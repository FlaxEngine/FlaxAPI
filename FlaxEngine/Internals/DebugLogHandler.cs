// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    internal sealed class DebugLogHandler : ILogHandler
    {
        public void LogException(Exception exception, Object context)
        {
            Internal_LogException(exception, context?.unmanagedPtr ?? IntPtr.Zero);
        }

        public void Log(LogType logType, Object context, string message)
        {
#if DEBUG
            string stackTrace = string.Empty;// Environment.StackTrace;
#else
            string stackTrace = string.Empty;
#endif
            Internal_Log(logType, message, context?.unmanagedPtr ?? IntPtr.Zero, stackTrace);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Log(LogType level, string msg, IntPtr obj, string stackTrace);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LogException(Exception exception, IntPtr obj);
    }
}
