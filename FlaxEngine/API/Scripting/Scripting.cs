// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlaxEngine.GUI;

namespace FlaxEngine
{
    /// <summary>
    /// C# scripting service.
    /// </summary>
    public static class Scripting
    {
        private static readonly List<Action> UpdateActions = new List<Action>();

        /// <summary>
        /// Occurs on scripting update.
        /// </summary>
        public static event Action Update;

        /// <summary>
        /// Occurs on scripting 'late' update.
        /// </summary>
        public static event Action LateUpdate;

        /// <summary>
        /// Occurs on scripting `fixed` update.
        /// </summary>
        public static event Action FixedUpdate;

        /// <summary>
        /// Occurs when scripting engine is disposing. Engine is during closing and some services may be unavailable (eg. loading scenes). This may be called after the engine fatal error event.
        /// </summary>
        public static event Action Exit;

        /// <summary>
        /// Calls the given action on the next scripting update.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        public static void InvokeOnUpdate(Action action)
        {
            lock (UpdateActions)
            {
                UpdateActions.Add(action);
            }
        }

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
        internal static void Init()
        {
#if DEBUG
            Debug.Logger.LogHandler.LogWrite(LogType.Info, "Using FlaxAPI in Debug");
#else
            Debug.Logger.LogHandler.LogWrite(LogType.Info, "Using FlaxAPI in Release");
#endif

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

        internal static void Internal_Update()
        {
            Update?.Invoke();

            lock (UpdateActions)
            {
                for (int i = 0; i < UpdateActions.Count; i++)
                {
                    try
                    {
                        UpdateActions[i]();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
                UpdateActions.Clear();
            }
        }

        internal static void Internal_LateUpdate()
        {
            LateUpdate?.Invoke();
        }

        internal static void Internal_FixedUpdate()
        {
            FixedUpdate?.Invoke();
        }

        internal static void Internal_Exit()
        {
            Exit?.Invoke();

            Json.JsonSerializer.Dispose();
        }

        /// <summary>
        /// Returns true if game scripts assembly has been loaded.
        /// </summary>
        /// <returns>True if game scripts assembly is loaded, otherwise false.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool IsGameAssemblyLoaded();

        /// <summary>
        /// Flushes the removed objects (disposed objects using Object.Destroy).
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void FlushRemovedObjects();
    }
}
