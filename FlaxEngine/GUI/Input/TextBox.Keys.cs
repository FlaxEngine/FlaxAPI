////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            // Check if use lowercase or uppercase
            var window = ParentWindow;
            bool shftDown = window.GetKey(KeyCode.Shift);
            bool ctrDown = window.GetKey(KeyCode.Control);
            bool uppercase = shftDown;
            if (window.GetKey(KeyCode.Capital))
            	uppercase = !uppercase;// TODO: capslock

            // Check if use combination with a control key
            if (ctrDown)
            {
                switch (key)
                {
                    case KeyCode.C:
                        Copy();
                        break;

                    case KeyCode.V:
                        Paste();
                        break;

                    case KeyCode.D:
                        Duplicate();
                        break;

                    case KeyCode.X:
                        Cut();
                        break;

                    case KeyCode.A:
                        SelectAll();
                        break;
                }

                return true;
            }

            // Check special keys
            switch (key)
            {
                // Delete
                case KeyCode.Backspace:
                case KeyCode.Delete:
                {
                    int left = SelectionLeft;
                    if (HasSelection)
                    {
                        _text = _text.Remove(left, SelectionLength);
                        setSelection(left);
                        OnTextChanged();
                        }
                    else if (CaretPosition > 0)
                    {
                        left -= 1;
                        _text = _text.Remove(left, 1);
                        setSelection(left);
                        OnTextChanged();
                    }

                    return true;
                }

                // Cancel
                case KeyCode.Escape:
                {
                    // Restore text from start
                    setSelection(-1);
                    _text = _onStartEditValue;

                    Defocus();
                    OnTextChanged();

                    return true;
                }

                // Enter
                case KeyCode.Return:
                {
                    if (IsMultiline)
                    {
                        // Insert new line
                        Insert('\n');
                    }
                    else
                    {
                        // End editing
                        Defocus();
                    }

                    return true;
                }

                // Scroll to begin
                case KeyCode.Home:
                {
                    // Move caret to the first character
                    setSelection(0);
                    return true;
                }

                // Scroll to end
                case KeyCode.End:
                {
                    // Move caret after last character
                    setSelection(TextLength);
                    return true;
                }

                case KeyCode.ArrowRight:
                    MoveRight(shftDown, ctrDown);
                    return true;

                case KeyCode.ArrowLeft:
                    MoveLeft(shftDown, ctrDown);
                    return true;

                case KeyCode.ArrowUp:
                    MoveUp(shftDown, ctrDown);
                    return true;

                case KeyCode.ArrowDown:
                    MoveDown(shftDown, ctrDown);
                    return true;
            }

            // Character insert
            char ascii = '\0';
            switch (key)
            {
                case KeyCode.Alpha0:
                    ascii = uppercase ? ')' : '0';
                    break;
                case KeyCode.Alpha1:
                    ascii = uppercase ? '!' : '1';
                    break;
                case KeyCode.Alpha2:
                    ascii = uppercase ? '@' : '2';
                    break;
                case KeyCode.Alpha3:
                    ascii = uppercase ? '#' : '3';
                    break;
                case KeyCode.Alpha4:
                    ascii = uppercase ? '$' : '4';
                    break;
                case KeyCode.Alpha5:
                    ascii = uppercase ? '%' : '5';
                    break;
                case KeyCode.Alpha6:
                    ascii = uppercase ? '^' : '6';
                    break;
                case KeyCode.Alpha7:
                    ascii = uppercase ? '&' : '7';
                    break;
                case KeyCode.Alpha8:
                    ascii = uppercase ? '*' : '8';
                    break;
                case KeyCode.Alpha9:
                    ascii = uppercase ? '(' : '9';
                    break;
                case KeyCode.Plus:
                    ascii = uppercase ? '=' : '+';
                    break;
                case KeyCode.Comma:
                    ascii = uppercase ? '<' : ',';
                    break;
                case KeyCode.Minus:
                    ascii = uppercase ? '_' : '-';
                    break;
                case KeyCode.Period:
                    ascii = uppercase ? '>' : '.';
                    break;
                case KeyCode.Colon:
                    ascii = uppercase ? ':' : ';';
                    break;
                case KeyCode.Slash:
                    ascii = uppercase ? '?' : '/';
                    break;
                case KeyCode.BackQuote:
                    ascii = uppercase ? '~' : '`';
                    break;
                case KeyCode.LeftBracket:
                    ascii = uppercase ? '{' : '[';
                    break;
                case KeyCode.Backslash:
                    ascii = uppercase ? '|' : '\\';
                    break;
                case KeyCode.RightBracket:
                    ascii = uppercase ? '}' : ']';
                    break;
                case KeyCode.Quote:
                    ascii = uppercase ? '\"' : '\'';
                    break;
                case KeyCode.Oem8:
                    ascii = uppercase ? '@' : '#';
                    break;

                case KeyCode.Tab:
                    ascii = '\t';
                    break;
                case KeyCode.Spacebar:
                    ascii = ' ';
                    break;
                case KeyCode.Numpad0:
                    ascii = '0';
                    break;
                case KeyCode.Numpad1:
                    ascii = '1';
                    break;
                case KeyCode.Numpad2:
                    ascii = '2';
                    break;
                case KeyCode.Numpad3:
                    ascii = '3';
                    break;
                case KeyCode.Numpad4:
                    ascii = '4';
                    break;
                case KeyCode.Numpad5:
                    ascii = '5';
                    break;
                case KeyCode.Numpad6:
                    ascii = '6';
                    break;
                case KeyCode.Numpad7:
                    ascii = '7';
                    break;
                case KeyCode.Numpad8:
                    ascii = '8';
                    break;
                case KeyCode.Numpad9:
                    ascii = '9';
                    break;
                case KeyCode.NumpadMultiply:
                    ascii = '*';
                    break;
                case KeyCode.NumpadAdd:
                    ascii = '+';
                    break;
                case KeyCode.NumpadSeparator:
                    ascii = ',';
                    break;
                case KeyCode.NumpadSubtract:
                    ascii = '-';
                    break;
                case KeyCode.NumpadDecimal:
                    ascii = '.';
                    break;
                case KeyCode.NumpadDivide:
                    ascii = '/';
                    break;

                case KeyCode.A:
                case KeyCode.B:
                case KeyCode.C:
                case KeyCode.D:
                case KeyCode.E:
                case KeyCode.F:
                case KeyCode.G:
                case KeyCode.H:
                case KeyCode.I:
                case KeyCode.J:
                case KeyCode.K:
                case KeyCode.L:
                case KeyCode.M:
                case KeyCode.N:
                case KeyCode.O:
                case KeyCode.P:
                case KeyCode.Q:
                case KeyCode.R:
                case KeyCode.S:
                case KeyCode.T:
                case KeyCode.U:
                case KeyCode.V:
                case KeyCode.W:
                case KeyCode.X:
                case KeyCode.Y:
                case KeyCode.Z:
                    ascii = (char)key;
                    if (!uppercase)
                        ascii += (char)0x20;
                    break;
            }
            if (ascii > 0)
            {
                Insert(ascii);
                return true;
            }

            return true;
        }

        private void WriteText()
        {
            
        }
    }
}
