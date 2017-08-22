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
        /// <summary>
        /// Event that is fired when at least one key is pressed.
        /// </summary>
        public static event Action<byte[]> OnKeyPressed;

        /// <summary>
        /// All currently acitve keys, can be null
        /// </summary>
        public static byte[] ActiveKeys { get; private set; }

        /// <summary>
        /// Converts virtual key code to unicode character
        /// </summary>
        /// <param name="virtualKeyCode"></param>
        /// <param name="scanCode"></param>
        /// <param name="keyboardState"></param>
        /// <param name="receivingBuffer"></param>
        /// <param name="bufferSize"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int ToUnicode(
            uint virtualKeyCode,
            uint scanCode,
            byte[] keyboardState,
            StringBuilder receivingBuffer,
            int bufferSize,
            uint flags
        );

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static string GetCharsFromKeys(KeyCode keyCode, bool shift)
        {
            var stringBuilder = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
            {
                keyboardState[(int)KeyCode.Shift] = 0xff;
            }
            ToUnicode((uint)keyCode, 0, keyboardState, stringBuilder, 256, 0);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Internal method used to get all currently active keys
        /// </summary>
        /// <param name="keyPressedArray"></param>
        internal static void Internal_KeyInputEvent(byte[] keyPressedArray)
        {
            //var a = "";
            //foreach (var b in keyPressedArray)
            //{
            //    a += (KeyCode)b + " ";
            //}
            //Debug.Log(a);
            ActiveKeys = keyPressedArray;
            OnKeyPressed?.Invoke(keyPressedArray);
        }
    }
}