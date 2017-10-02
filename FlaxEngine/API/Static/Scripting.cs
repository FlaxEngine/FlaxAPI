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
            Update?.Invoke();
        }

        internal static void Internal_LateUpdate()
        {
            LateUpdate?.Invoke();
        }

        internal static void Internal_FixedUpdate()
        {
            FixedUpdate?.Invoke();
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_FlushRemovedObjects();
#endif

        #endregion
    }
}
