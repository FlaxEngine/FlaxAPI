////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public static partial class Input
    {
        private static KeyCodeMap _previousPressedKeys;

        /// <summary>
        ///     Event that is fired when at least one key is hold down.
        /// </summary>
        public static event Action<KeyCodeMap> OnKeyHold;

        /// <summary>
        ///     Event that is fired when a new key is pressed. Contains all currently holding keys with addition to the new one.
        /// </summary>
        public static event Action<KeyCodeMap> OnKeyPressed;

        /// <summary>
        ///     Event that is fired when a key is released. Contains all currently holding keys without last one released.
        /// </summary>
        public static event Action<KeyCodeMap> OnKeyReleased;

        /// <summary>
        ///     Event taht is fired when a key with modifier or special character or IME input returns a character.
        /// </summary>
        public static event Action<string> OnTextEntered;


        /// <summary>
        ///     All currently acitve keys, can be null
        /// </summary>
        public static KeyCodeMap PressedKeys { get; private set; }

        /// <summary>
        ///     Internal method used to get all currently active keys
        /// </summary>
        /// <param name="keyPressedArray"></param>
        internal static void Internal_KeyInputEvent(byte[] keyPressedArray)
        {
            if (keyPressedArray.Length > 0)
            {
                var keysMapped = new KeyCodeMap(keyPressedArray);
                _previousPressedKeys = PressedKeys;
                PressedKeys = keysMapped;
                OnKeyHold?.Invoke(keysMapped);
                if (!_previousPressedKeys.Equals(PressedKeys))
                {
                    if (_previousPressedKeys.Count > PressedKeys.Count)
                    {
                        OnKeyReleased?.Invoke(keysMapped);
                    }
                    else if (_previousPressedKeys.Count < PressedKeys.Count)
                    {
                        OnKeyPressed?.Invoke(keysMapped);
                    }
                    else
                    {
                        OnKeyPressed?.Invoke(keysMapped);
                        OnKeyReleased?.Invoke(keysMapped);
                    }
                }
            }
            else
            {
                _previousPressedKeys = PressedKeys;
                PressedKeys = new KeyCodeMap(null);
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
            StringBuilder builder = new StringBuilder(unicode.Length);
            foreach (var i in unicode)
            {
                builder.Append((char)i);
            }
            OnTextEntered?.Invoke(builder.ToString());
        }
    }
}