////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
            Globals.Init();
        }
    }
}