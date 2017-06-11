// Flax Engine scripting API

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    internal static class ClassLibraryInitializer
    {
        private static void Init()
        {
            UnhandledExceptionHandler.RegisterUECatcher();
            FlaxLogWriter.Init();
        }
    }
}