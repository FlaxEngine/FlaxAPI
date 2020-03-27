// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using FlaxEngine.GUI;

namespace FlaxEngine
{
    internal static class ClassLibraryInitializer
    {
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                Debug.LogError("Unhandled Exception: " + exception.Message);
                Debug.LogException(exception);
            }
        }

        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                foreach (var ex in e.Exception.InnerExceptions)
                {
                    Debug.LogError("Unhandled Task Exception: " + ex.Message);
                    Debug.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Initializes Flax API. Called before everything else from native code.
        /// </summary>
        /// <param name="flags">The packed flags with small meta for the API.</param>
        /// <param name="platform">The runtime platform.</param>
        internal static void Init(int flags, PlatformType platform)
        {
#if DEBUG
            Debug.Logger.LogHandler.LogWrite(LogType.Info, "Using FlaxAPI in Debug");
#else
            Debug.Logger.LogHandler.LogWrite(LogType.Info, "Using FlaxAPI in Release");
#endif

            Platform._is64Bit = (flags & 0x01) != 0;
            Platform._isEditor = (flags & 0x02) != 0;
            Platform._mainThreadId = Thread.CurrentThread.ManagedThreadId;
            Platform._platform = platform;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            if (!Platform.IsEditor)
            {
                CreateGuiStyle();
            }
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

        internal static Type MakeGenericType(Type genericType, Type[] genericArgs)
        {
            return genericType.MakeGenericType(genericArgs);
        }

        internal static object[] GetDictionaryKeys(IDictionary dictionary)
        {
            var keys = dictionary.Keys;
            var result = new object[keys.Count];
            keys.CopyTo(result, 0);
            return result;
        }

        private static void CreateGuiStyle()
        {
            var style = new Style
            {
                Background = Color.FromBgra(0xFF1C1C1C),
                LightBackground = Color.FromBgra(0xFF2D2D30),
                Foreground = Color.FromBgra(0xFFFFFFFF),
                ForegroundGrey = Color.FromBgra(0xFFA9A9B3),
                ForegroundDisabled = Color.FromBgra(0xFF787883),
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
