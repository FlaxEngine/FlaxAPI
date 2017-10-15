////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
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

        /// <summary>
        /// Flushes the removed objects (disposed objects using Object.Destroy).
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void FlushRemovedObjects();
    }
}
