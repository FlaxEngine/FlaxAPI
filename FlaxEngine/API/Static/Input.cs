////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public static partial class Input
    {
        private static byte[] _previousPressedKeys;

        /// <summary>
        ///     Event that is fired when at least one key is hold down.
        /// </summary>
        public static event Action<byte[]> OnKeyHold;

        /// <summary>
        ///     Event that is fired when a new key is pressed. Contains all currently holding keys with addition to the new one.
        /// </summary>
        public static event Action<byte[]> OnKeyPressed;

        /// <summary>
        ///     Event that is fired when a key is released. Contains all currently holding keys without last one released.
        /// </summary>
        public static event Action<byte[]> OnKeyReleased;


        /// <summary>
        ///     All currently acitve keys, can be null
        /// </summary>
        public static byte[] PressedKeys { get; private set; }

        /// <summary>
        ///     Internal method used to get all currently active keys
        /// </summary>
        /// <param name="keyPressedArray"></param>
        internal static void Internal_KeyInputEvent(byte[] keyPressedArray)
        {
            _previousPressedKeys = PressedKeys;
            PressedKeys = keyPressedArray;
            if (keyPressedArray.Length > 0)
            {
                OnKeyHold?.Invoke(keyPressedArray);
            }
            if (!_previousPressedKeys.Equals(PressedKeys))
            {
                if (_previousPressedKeys.Length > PressedKeys.Length)
                {
                    OnKeyReleased?.Invoke(keyPressedArray);
                }
                else if (_previousPressedKeys.Length < PressedKeys.Length)
                {
                    OnKeyPressed?.Invoke(keyPressedArray);
                }
                else
                {
                    OnKeyPressed?.Invoke(keyPressedArray);
                    OnKeyReleased?.Invoke(keyPressedArray);
                }
            }
        }

        /// <summary>
        ///     Internal method used to get currently typed unicode characters
        /// </summary>
        /// <remarks>
        ///     Do not use for game input
        /// </remarks>
        /// <param name="unicode"></param>
        internal static void Internal_UnicodeInputEvent(int[] unicode)
        {
        }
    }
}