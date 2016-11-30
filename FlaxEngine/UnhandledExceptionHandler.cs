// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    internal sealed class UnhandledExceptionHandler
    {
        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exceptionObject = args.ExceptionObject as Exception;
            if (exceptionObject != null)
                PrintException("Unhandled Exception: ", exceptionObject);
            NativeUnhandledExceptionHandler();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void NativeUnhandledExceptionHandler();

        private static void PrintException(string title, Exception e)
        {
            Debug.LogError(string.Concat(title, e.ToString()));
            if (e.InnerException != null)
                PrintException("Inner Exception: ", e.InnerException);
        }

        internal static void RegisterUECatcher()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
        }
    }
}
