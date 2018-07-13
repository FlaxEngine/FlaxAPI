// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Threading;
using FlaxEngine.GUI;
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
            Application._mainThreadId = Thread.CurrentThread.ManagedThreadId;
            Application._platform = platform;

            UnhandledExceptionHandler.RegisterCatcher();
            Globals.Init();
            Input.Init();

            if (!Application.IsEditor)
            {
                CreateGuiStyle();
            }

            MainRenderTask.Instance = RenderTask.Create<MainRenderTask>();
        }

        /// <summary>
        /// Sets the managed window as a main game window. Called after creating game window by the native code.
        /// </summary>
        /// <param name="window">The window.</param>
        internal static void SetWindow(Window window)
        {
            // Link it as a GUI root control
            window.GUI.BackgroundColor = Color.Transparent;
            RootControl.GameRoot = window.GUI;
        }

        private static void CreateGuiStyle()
        {
            var style = new Style
            {
                Background = Color.FromBgra(0xFF1C1C1C),
                LightBackground = Color.FromBgra(0xFF2D2D30),
                Foreground = Color.FromBgra(0xFFFFFFFF),
                ForegroundDisabled = new Color(0.6f),
                BackgroundHighlighted = Color.FromBgra(0xFF54545C),
                BorderHighlighted = Color.FromBgra(0xFF6A6A75),
                BackgroundSelected = Color.FromBgra(0xFF007ACC),
                BorderSelected = Color.FromBgra(0xFF1C97EA),
                BackgroundNormal = Color.FromBgra(0xFF3F3F46),
                BorderNormal = Color.FromBgra(0xFF54545C),
                TextBoxBackground = Color.FromBgra(0xFF333337),
                ProgressNormal = Color.FromBgra(0xFF0ad328),
                TextBoxBackgroundSelected = Color.FromBgra(0xFF3F3F46),
                SharedTooltip = new Tooltip(),
            };
            style.DragWindow = style.BackgroundSelected * 0.7f;

            Style.Current = style;
        }
    }
}
