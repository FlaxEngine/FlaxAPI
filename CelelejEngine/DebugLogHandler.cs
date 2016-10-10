// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    internal sealed class DebugLogHandler : ILogHandler
    {
        public void LogException(Exception exception, Object context)
        {
            Internal_LogException(exception, context != null ? context.unmanagedPtr : IntPtr.Zero);
        }

        public void Log(LogType logType, Object context, string message)
        {
            Internal_Log(logType, message, context != null ? context.unmanagedPtr : IntPtr.Zero);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Log(LogType level, string msg, IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LogException(Exception exception, IntPtr obj);
    }
}
