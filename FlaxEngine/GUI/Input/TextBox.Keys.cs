// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
            var window = Root;
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
            {
                if (IsReadOnly)
                    return true;

                int left = SelectionLeft;
                if (HasSelection)
                {
                    _text = _text.Remove(left, SelectionLength);
                    SetSelection(left);
                    OnTextChanged();
                }
                else if (CaretPosition > 0)
                {
                    left -= 1;
                    _text = _text.Remove(left, 1);
                    SetSelection(left);
                    OnTextChanged();
                }

                return true;
            }
            case Keys.Delete:
            {
                if (IsReadOnly)
                    return true;

                int left = SelectionLeft;
                if (HasSelection)
                {
                    _text = _text.Remove(left, SelectionLength);
                    SetSelection(left);
                    OnTextChanged();
                }
                else if (TextLength > 0 && left < TextLength)
                {
                    _text = _text.Remove(left, 1);
                    SetSelection(left);
                    OnTextChanged();
                }

                return true;
            }

            // Cancel
            case Keys.Escape:
            {
                // Restore text from start
                SetSelection(-1);
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
                SetSelection(0);
                return true;
            }

            // Scroll to end
            case Keys.End:
            {
                // Move caret after last character
                SetSelection(TextLength);
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
    }
}
