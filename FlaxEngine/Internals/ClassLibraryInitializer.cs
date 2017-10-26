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
        /// <param name="flags">The packed flags with small meta for the API.</param>
        /// <param name="platform">The runtime platform.</param>
        internal static void Init(int flags, PlatformType platform)
        {
            Application._is64Bit = (flags & 0x01) != 0;
            Application._isEditor = (flags & 0x02) != 0;
            Application._platform = platform;

            UnhandledExceptionHandler.RegisterCatcher();
            FlaxLogWriter.Init();
            Globals.Init();

            MainRenderTask.Instance = RenderTask.Create<MainRenderTask>();
        }
    }
}
