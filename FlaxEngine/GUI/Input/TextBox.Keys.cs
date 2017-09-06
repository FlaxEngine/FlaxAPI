////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        /// <inheritdoc />
        public override bool OnKeyPressed(KeyCodeMap key)
        {
            // Do not input text if controll is not focused
            if (!IsFocused)
            {
                return true;
            }

            // Check if use lowercase or uppercase
            bool shftDown = ParentWindow.GetKey(KeyCode.Shift);
            bool ctrDown = ParentWindow.GetKey(KeyCode.Control);

            if (key.InvokeFirstCommand(KeyCode.Control,
                (KeyCode.C, Copy),
                (KeyCode.V, Paste),
                (KeyCode.D, Duplicate),
                (KeyCode.X, Cut),
                (KeyCode.A, SelectAll)))
            {
                return true;
            }

            key.InvokeFirstCommand(
                    (KeyCode.Escape, ResetText),
                    (KeyCode.Return, NextLineOrDeselect),
                    (KeyCode.Backspace, RemoveBackward),
                    (KeyCode.Delete, RemoveForward),
                    (KeyCode.Home, () => { setSelection(0); }),
                    (KeyCode.End, () => { setSelection(TextLength); }),
                    (KeyCode.ArrowRight, () => { MoveRight(shftDown, ctrDown); }),
                    (KeyCode.ArrowLeft, () => { MoveLeft(shftDown, ctrDown); }),
                    (KeyCode.ArrowUp, () => { MoveUp(shftDown, ctrDown); }),
                    (KeyCode.ArrowDown, () => { MoveDown(shftDown, ctrDown); })
                );
            return false;
        }

        private void OnTextEntered(string input)
        {
            // Do not input text if controll is not focused
            if (!IsFocused)
            {
                return;
            }
            // Do not input text if control key is pressed from modifiers key and alt is not pressed
            if (Input.GetKey(KeyCode.Control) && !Input.GetKey(KeyCode.Alt))
            {
                return;
            }
            Insert(input);
        }

        private void WriteText()
        {
        }
    }
}