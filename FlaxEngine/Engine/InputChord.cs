using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlaxEngine
{
    /// <summary>
    ///     Simple mapping for chord of keys to quick access structure with helper methods to access keycodes
    /// </summary>
    public class InputChord : IEnumerable<KeyCode>
    {
        private readonly HashSet<KeyCode> _keyCodes = new HashSet<KeyCode>();

        /// <summary>
        ///     Amount of keys currently pressed
        /// </summary>
        public int Count => (int)KeyCodes?.Count;

        /// <summary>
        ///     Converter from array of bytes to internally <see cref="HashSet{T}" />
        /// </summary>
        /// <param name="keyPressed">Currently pressed keys prepared to be mapped</param>
        public InputChord(byte[] keyPressed)
        {
            if (keyPressed == null || keyPressed.Length == 0)
            {
                return;
            }
            foreach (byte @byte in keyPressed)
            {
                KeyCodes.Add((KeyCode)@byte);
            }
        }

        /// <summary>
        ///     Converter from chord of keys to internally <see cref="HashSet{T}" />
        /// </summary>
        /// <param name="keyCodes">Chord of keys required to be mapped</param>
        public InputChord(params KeyCode[] keyCodes)
        {
            foreach (KeyCode key in keyCodes)
            {
                KeyCodes.Add(key);
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

        /// <summary>
        ///     Does given input consists of only alphabetic characters
        /// </summary>
        /// <returns>Returns true if there are no other KeysPressed then alphabetic characters</returns>
        public bool IsAlphabetic()
        {
            foreach (var keyCode in KeyCodes)
            {
                if (keyCode < KeyCode.A || keyCode > KeyCode.Z)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Does given input consists of only alphabetic or numeric characters
        /// </summary>
        /// <returns>Returns true if there are no other KeysPressed then alphabetic or numeric characters</returns>
        public bool IsAlphaNumeric()
        {
            foreach (var keyCode in KeyCodes)
            {
                if ((keyCode < KeyCode.A || keyCode > KeyCode.Z) && (keyCode < KeyCode.Alpha0 || keyCode > KeyCode.Alpha9) && (keyCode < KeyCode.Numpad0 || keyCode > KeyCode.Numpad9))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Does given input consists of only numeric characters
        /// </summary>
        /// <returns>Returns true if there are no other KeysPressed then numeric characters</returns>
        public bool IsNumeric()
        {
            foreach (var keyCode in KeyCodes)
            {
                if ((keyCode < KeyCode.Alpha0 || keyCode > KeyCode.Alpha9) && (keyCode < KeyCode.Numpad0 || keyCode > KeyCode.Numpad9))
                {
                    return false;
                }
            }
            return true;
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
            var keyCodeMap = (InputChord)obj;
            if (keyCodeMap?.Count != Count)
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
        public override int GetHashCode()
        {
            return KeyCodes.GetHashCode();
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

        /// <summary>
        ///     Implicit conversion operator
        /// </summary>
        /// <param name="map">Current key code map</param>
        /// <returns>KeyCodes array</returns>
        public static implicit operator KeyCode[](InputChord map)
        {
            return map.ToArray();
        }
    }
}