using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlaxEngine
{
    /// <summary>
    ///     Simple mapping for array of bytes to quick access structure with helper methods to access keycodes
    /// </summary>
    public class KeyCodeMap : IEnumerable<KeyCode>
    {
        /// <summary>
        /// TODO comment
        /// </summary>
        public class KeyCommand
        {
            public KeyCode Code { get; set; }
            public Action ActionToInvoke { get; set; }
            public KeyCommand(KeyCode code, Action actionToInvoke)
            {
                Code = code;
                ActionToInvoke = actionToInvoke;
            }
        }

        private readonly HashSet<KeyCode> _keyCodes;

        /// <summary>
        ///     Amount of keys currently pressed
        /// </summary>
        public int Count => KeyCodes.Count;

        /// <summary>
        ///     Converter from keyPressed to <see cref="HashSet{T}" />
        /// </summary>
        /// <param name="keyPressed">Currently pressed keys prepared to be mapped</param>
        /// <param name="previousSet">Previously pressed keys prepared to be mapped</param>
        public KeyCodeMap(byte[] keyPressed)
        {
            _keyCodes = new HashSet<KeyCode>();
            if(keyPressed == null || keyPressed.Length == 0)
            {
                return;
            }
            for (int i = 0; i < keyPressed.Length; i++)
            {
                KeyCodes.Add((KeyCode)keyPressed[i]);
            }
        }

        /// <summary>
        ///     Currently pressed keycodes
        /// </summary>
        public HashSet<KeyCode> KeyCodes
        {
            get { return _keyCodes; }
        }

        /// <summary>
        ///     Currently pressed keycodes
        /// </summary>
        /// <param name="code">Expected pressed key</param>
        /// <returns></returns>
        public bool this[KeyCode code]
        {
            get { return KeyCodes.Contains(code); }
        }

        /// <summary>
        ///     Shortcut to invoke method if key is pressed
        /// </summary>
        /// <param name="code"></param>
        /// <param name="actionToInvoke"></param>
        public bool InvokeCommand(KeyCode code, Action actionToInvoke)
        {
            if (KeyCodes.Contains(code))
            {
                actionToInvoke?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Shortcut to invoke first matching of provided method that matches privided <see cref="KeyCode"/>
        /// </summary>
        /// <param name="actionsToInvoke">All actions that should be invoked if key is pressed</param>
        /// <returns></returns>
        public bool InvokeFirstCommand(params KeyCommand[] actionsToInvoke)
        {
            foreach (var tuple in actionsToInvoke)
            {
                if (InvokeCommand(tuple.Code, tuple.ActionToInvoke))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Shortcut to invoke first matching of provided method that matches privided <see cref="KeyCode"/>
        /// </summary>
        /// <param name="actionsToInvoke">All actions that should be invoked if key is pressed</param>
        /// <returns></returns>
        public bool InvokeFirstCommand(KeyCode modifier, params KeyCommand[] actionsToInvoke)
        {
            if (!KeyCodes.Contains(modifier))
                return false;
            foreach (var tuple in actionsToInvoke)
            {
                if (InvokeCommand(tuple.Code, tuple.ActionToInvoke))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Shortcut to invoke first matching of provided method that matches privided <see cref="KeyCode"/>
        /// </summary>
        /// <param name="actionsToInvoke">All actions that should be invoked if key is pressed</param>
        /// <returns></returns>
        public bool InvokeFirstCommand(KeyCode[] modifiers, params KeyCommand[] actionsToInvoke)
        {
            foreach (var modifier in modifiers)
            {
                if (!KeyCodes.Contains(modifier))
                    return false;
            }
            foreach (var tuple in actionsToInvoke)
            {
                if (InvokeCommand(tuple.Code, tuple.ActionToInvoke))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Is <see cref="KeyCode.Shift" /> pressed
        /// </summary>
        /// <returns></returns>
        public bool IsShift()
        {
            return _keyCodes.Contains(KeyCode.Shift);
        }

        /// <summary>
        ///     Is <see cref="KeyCode.Control" /> pressed
        /// </summary>
        /// <returns></returns>
        public bool IsControl()
        {
            return _keyCodes.Contains(KeyCode.Control);
        }

        /// <summary>
        ///     Is <see cref="KeyCode.Alt" /> Pressed
        /// </summary>
        /// <returns></returns>
        public bool IsAlt()
        {
            return _keyCodes.Contains(KeyCode.Alt);
        }

        /// <summary>
        ///     Is Both <see cref="KeyCode.Control" /> and <see cref="KeyCode.Alt" /> pressed known as AltGr
        /// </summary>
        /// <returns></returns>
        public bool IsAltGr()
        {
            return IsControl() && IsAlt();
        }

        /// <summary>
        ///     Is both <see cref="KeyCode.Control" /> and <see cref="KeyCode.Alt" /> pressed known as AltGr
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use IsAltGr instead")]
        public bool IsControlAlt()
        {
            return IsControl() && IsAlt();
        }

        /// <summary>
        ///     Is both <see cref="KeyCode.Control" /> and  <see cref="KeyCode.Shift" /> pressed
        /// </summary>
        /// <returns></returns>
        public bool IsControlShift()
        {
            return IsControl() && IsShift();
        }

        /// <summary>
        ///     Is both <see cref="KeyCode.Control" /> and <see cref="KeyCode.Alt" /> and  <see cref="KeyCode.Shift" /> pressed
        /// </summary>
        /// <returns></returns>
        public bool IsAltShift()
        {
            return IsShift() && IsAlt();
        }

        /// <summary>
        ///     Is all <see cref="KeyCode.Alt" /> and  <see cref="KeyCode.Shift" /> pressed
        /// </summary>
        /// <returns></returns>
        public bool IsControlAltShift()
        {
            return IsControl() && IsAlt() && IsShift();
        }

        /// <inheritdoc />
        public IEnumerator<KeyCode> GetEnumerator()
        {
            return KeyCodes.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var keyCodeMap = (KeyCodeMap)obj;
            if(keyCodeMap?.Count != Count)
            {
                return false;
            }
            foreach (var code in KeyCodes)
            {
                if (!keyCodeMap.KeyCodes.Contains(code))
                {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var keyCode in _keyCodes)
            {
                stringBuilder.Append(keyCode + " ");
            }
            return stringBuilder.ToString();
        }
    }
}