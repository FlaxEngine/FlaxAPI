////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            // Check if use lowercase or uppercase
            var window = ParentWindow;
            bool shftDown = window.GetKey(Keys.Shift);
            bool ctrDown = window.GetKey(Keys.Control);
            bool uppercase = shftDown;
            if (window.GetKey(Keys.Capital))
            	uppercase = !uppercase;// TODO: capslock

            // Check if use combination with a control key
            if (ctrDown)
            {
                switch (key)
                {
                    case Keys.C:
                        Copy();
                        break;

                    case Keys.V:
                        Paste();
                        break;

                    case Keys.D:
                        Duplicate();
                        break;

                    case Keys.X:
                        Cut();
                        break;

                    case Keys.A:
                        SelectAll();
                        break;
                }

                return true;
            }

            // Check special keys
            switch (key)
            {
                // Delete
                case Keys.Backspace:
                case Keys.Delete:
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
                case Keys.Escape:
                {
                    // Restore text from start
                    setSelection(-1);
                    _text = _onStartEditValue;

                    Defocus();
                    OnTextChanged();

                    return true;
                }

                // Enter
                case Keys.Return:
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
                case Keys.Home:
                {
                    // Move caret to the first character
                    setSelection(0);
                    return true;
                }

                // Scroll to end
                case Keys.End:
                {
                    // Move caret after last character
                    setSelection(TextLength);
                    return true;
                }

                case Keys.ArrowRight:
                    MoveRight(shftDown, ctrDown);
                    return true;

                case Keys.ArrowLeft:
                    MoveLeft(shftDown, ctrDown);
                    return true;

                case Keys.ArrowUp:
                    MoveUp(shftDown, ctrDown);
                    return true;

                case Keys.ArrowDown:
                    MoveDown(shftDown, ctrDown);
                    return true;
            }

            // Character insert
            char ascii = '\0';
            switch (key)
            {
                case Keys.Alpha0:
                    ascii = uppercase ? ')' : '0';
                    break;
                case Keys.Alpha1:
                    ascii = uppercase ? '!' : '1';
                    break;
                case Keys.Alpha2:
                    ascii = uppercase ? '@' : '2';
                    break;
                case Keys.Alpha3:
                    ascii = uppercase ? '#' : '3';
                    break;
                case Keys.Alpha4:
                    ascii = uppercase ? '$' : '4';
                    break;
                case Keys.Alpha5:
                    ascii = uppercase ? '%' : '5';
                    break;
                case Keys.Alpha6:
                    ascii = uppercase ? '^' : '6';
                    break;
                case Keys.Alpha7:
                    ascii = uppercase ? '&' : '7';
                    break;
                case Keys.Alpha8:
                    ascii = uppercase ? '*' : '8';
                    break;
                case Keys.Alpha9:
                    ascii = uppercase ? '(' : '9';
                    break;
                case Keys.Plus:
                    ascii = uppercase ? '=' : '+';
                    break;
                case Keys.Comma:
                    ascii = uppercase ? '<' : ',';
                    break;
                case Keys.Minus:
                    ascii = uppercase ? '_' : '-';
                    break;
                case Keys.Period:
                    ascii = uppercase ? '>' : '.';
                    break;
                case Keys.Colon:
                    ascii = uppercase ? ':' : ';';
                    break;
                case Keys.Slash:
                    ascii = uppercase ? '?' : '/';
                    break;
                case Keys.BackQuote:
                    ascii = uppercase ? '~' : '`';
                    break;
                case Keys.LeftBracket:
                    ascii = uppercase ? '{' : '[';
                    break;
                case Keys.Backslash:
                    ascii = uppercase ? '|' : '\\';
                    break;
                case Keys.RightBracket:
                    ascii = uppercase ? '}' : ']';
                    break;
                case Keys.Quote:
                    ascii = uppercase ? '\"' : '\'';
                    break;
                case Keys.Oem8:
                    ascii = uppercase ? '@' : '#';
                    break;

                case Keys.Tab:
                    ascii = '\t';
                    break;
                case Keys.Spacebar:
                    ascii = ' ';
                    break;
                case Keys.Numpad0:
                    ascii = '0';
                    break;
                case Keys.Numpad1:
                    ascii = '1';
                    break;
                case Keys.Numpad2:
                    ascii = '2';
                    break;
                case Keys.Numpad3:
                    ascii = '3';
                    break;
                case Keys.Numpad4:
                    ascii = '4';
                    break;
                case Keys.Numpad5:
                    ascii = '5';
                    break;
                case Keys.Numpad6:
                    ascii = '6';
                    break;
                case Keys.Numpad7:
                    ascii = '7';
                    break;
                case Keys.Numpad8:
                    ascii = '8';
                    break;
                case Keys.Numpad9:
                    ascii = '9';
                    break;
                case Keys.NumpadMultiply:
                    ascii = '*';
                    break;
                case Keys.NumpadAdd:
                    ascii = '+';
                    break;
                case Keys.NumpadSeparator:
                    ascii = ',';
                    break;
                case Keys.NumpadSubtract:
                    ascii = '-';
                    break;
                case Keys.NumpadDecimal:
                    ascii = '.';
                    break;
                case Keys.NumpadDivide:
                    ascii = '/';
                    break;

                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
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
