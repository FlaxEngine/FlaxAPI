////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public static partial class Input
    {
        internal static int gamepadsVersion;
        internal static Gamepad[] gamepads;

        /// <summary>
        /// The gamepads changed event. Called when new gamepad device gets disconnected or added. Called always on main thread before the scripts update or during <see cref="ScanGamepads"/> call.
        /// </summary>
        public static Action GamepadsChanged;

        /// <summary>
        /// Gets the gamepad devices detected by the engine.
        /// </summary>
        public static Gamepad[] Gamepads
        {
            get
            {
                if (gamepads == null)
                {
                    int count = Internal_GetGamepadsCount();
                    gamepads = new Gamepad[count];
                    for (int i = 0; i < count; i++)
                        gamepads[i] = new Gamepad(i, gamepadsVersion);
                }
                return gamepads;
            }
        }

        /// <summary>
        /// Scans the connected gamepad devices to find the new ones.
        /// </summary>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void ScanGamepads();
#else
        public static void ScanGamepads()
        {
        }
#endif

        internal static void Internal_GamepadsChanged()
        {
            gamepadsVersion++;
            gamepads = null;
            GamepadsChanged?.Invoke();
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetGamepadsCount();
#endif

        #endregion
    }
}
