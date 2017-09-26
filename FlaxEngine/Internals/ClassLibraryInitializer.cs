////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.Rendering;

namespace FlaxEngine
{
    internal static class ClassLibraryInitializer
    {
        /// <summary>
        /// Initializes Flax API. Called before everything else from native code.
        /// </summary>
        internal static void Init()
        {
            UnhandledExceptionHandler.RegisterCatcher();
            FlaxLogWriter.Init();
            Globals.Init();

            MainRenderTask.Instance = RenderTask.Create<MainRenderTask>();
        }
    }
}
