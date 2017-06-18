////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    public static partial class Scripting
    {
        /// <summary>
        /// Occurs on scripting update.
        /// </summary>
        public static event Action OnUpdate;
        
        /// <summary>
        /// Occurs on scripting 'late' update.
        /// </summary>
        public static event Action OnLateUpdate;

        /// <summary>
        /// Occurs on scripting fxied update.
        /// </summary>
        public static event Action OnFixedUpdate;

        internal static void Internal_Update()
        {
            OnUpdate?.Invoke();
        }

        internal static void Internal_LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        internal static void Internal_FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
}
