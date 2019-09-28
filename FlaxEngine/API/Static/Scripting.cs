// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        internal static void Internal_Update()
        {
            Time.SyncData();

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
            Time.SyncData();

            LateUpdate?.Invoke();
        }

        internal static void Internal_FixedUpdate()
        {
            Time.SyncData();

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
