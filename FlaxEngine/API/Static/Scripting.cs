// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// C# scripting service.
    /// </summary>
    public static class Scripting
    {
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

        internal static void Internal_Update()
        {
            Time.SyncData();
            Update?.Invoke();
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
        }

        /// <summary>
        /// Flushes the removed objects (disposed objects using Object.Destroy).
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void FlushRemovedObjects();
    }
}
