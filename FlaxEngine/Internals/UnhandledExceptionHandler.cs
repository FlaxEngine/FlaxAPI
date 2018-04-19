// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    internal sealed class UnhandledExceptionHandler
    {
        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            if (args.ExceptionObject is Exception exception)
            {
                Debug.LogError("Unhandled Exception: " + exception.Message);
                Debug.LogException(exception);
            }

            Handler();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Handler();

        internal static void RegisterCatcher()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
        }
    }
}
