////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        /// <inheritdoc />
        public override bool OnCharInput(char c)
        {
            Insert(c);
            return true;
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            // Check if use lowercase or uppercase
            var window = ParentWindow;
            bool shftDown = window.GetKey(Keys.Shift);
            bool ctrDown = window.GetKey(Keys.Control);
            
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

            return true;
        }

        private void WriteText()
        {
            
        }
    }
}
