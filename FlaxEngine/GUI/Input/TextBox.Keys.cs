////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            // Check if use lowercase or uppercase
            var window = ParentWindow;
            bool shftDown = window.GetKey(KeyCode.SHIFT);
            bool ctrDown = window.GetKey(KeyCode.CONTROL);
            bool uppercase = shftDown;
            if (window.GetKey(KeyCode.CAPITAL))
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
                case KeyCode.BACK:
                case KeyCode.DELETE:
                {
                    int left = SelectionLeft;
                    if (HasSelection)
                    {
                        _text = _text.Remove(left, SelectionLength);
                        setSelection(left);
                    }
                    else if (CaretPosition > 0)
                    {
                        left -= 1;
                        _text = _text.Remove(left, 1);
                        setSelection(left);
                    }

                    return true;
                }

                // Cancel
                case KeyCode.ESCAPE:
                {
                    // Restore text from start
                    setSelection(-1);
                    _text = _onStartEditValue;

                    Defocus();

                    return true;
                }

                // Enter
                case KeyCode.RETURN:
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
                case KeyCode.HOME:
                {
                    // Move caret to the first character
                    setSelection(0);
                    return true;
                }

                // Scroll to end
                case KeyCode.END:
                {
                    // Move caret after last character
                    setSelection(TextLength);
                    return true;
                }

                case KeyCode.RIGHT:
                    MoveRight(shftDown, ctrDown);
                    return true;

                case KeyCode.LEFT:
                    MoveLeft(shftDown, ctrDown);
                    return true;

                case KeyCode.UP:
                    MoveUp(shftDown, ctrDown);
                    return true;

                case KeyCode.DOWN:
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
                case KeyCode.OEM_PLUS:
                    ascii = uppercase ? '=' : '+';
                    break;
                case KeyCode.OEM_COMMA:
                    ascii = uppercase ? '<' : ',';
                    break;
                case KeyCode.OEM_MINUS:
                    ascii = uppercase ? '_' : '-';
                    break;
                case KeyCode.OEM_PERIOD:
                    ascii = uppercase ? '>' : '.';
                    break;
                case KeyCode.OEM_1:
                    ascii = uppercase ? ':' : ';';
                    break;
                case KeyCode.OEM_2:
                    ascii = uppercase ? '?' : '/';
                    break;
                case KeyCode.OEM_3:
                    ascii = uppercase ? '~' : '`';
                    break;
                case KeyCode.OEM_4:
                    ascii = uppercase ? '{' : '[';
                    break;
                case KeyCode.OEM_5:
                    ascii = uppercase ? '|' : '\\';
                    break;
                case KeyCode.OEM_6:
                    ascii = uppercase ? '}' : ']';
                    break;
                case KeyCode.OEM_7:
                    ascii = uppercase ? '\"' : '\'';
                    break;
                case KeyCode.OEM_8:
                    ascii = uppercase ? '@' : '#';
                    break;

                case KeyCode.TAB:
                    ascii = '\t';
                    break;
                case KeyCode.SPACE:
                    ascii = ' ';
                    break;
                case KeyCode.NUMPAD0:
                    ascii = '0';
                    break;
                case KeyCode.NUMPAD1:
                    ascii = '1';
                    break;
                case KeyCode.NUMPAD2:
                    ascii = '2';
                    break;
                case KeyCode.NUMPAD3:
                    ascii = '3';
                    break;
                case KeyCode.NUMPAD4:
                    ascii = '4';
                    break;
                case KeyCode.NUMPAD5:
                    ascii = '5';
                    break;
                case KeyCode.NUMPAD6:
                    ascii = '6';
                    break;
                case KeyCode.NUMPAD7:
                    ascii = '7';
                    break;
                case KeyCode.NUMPAD8:
                    ascii = '8';
                    break;
                case KeyCode.NUMPAD9:
                    ascii = '9';
                    break;
                case KeyCode.MULTIPLY:
                    ascii = '*';
                    break;
                case KeyCode.ADD:
                    ascii = '+';
                    break;
                case KeyCode.SEPARATOR:
                    ascii = ',';
                    break;
                case KeyCode.SUBTRACT:
                    ascii = '-';
                    break;
                case KeyCode.DECIMAL:
                    ascii = '.';
                    break;
                case KeyCode.DIVIDE:
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
    }
}
