using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    internal static class ClassLibraryInitializer
    {
        private static void Init()
        {
            UnhandledExceptionHandler.RegisterUECatcher();
            CelelejLogWriter.Init();
        }
    }
}
