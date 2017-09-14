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
                new KeyCodeMap.KeyCommand(KeyCode.C, Copy),
                new KeyCodeMap.KeyCommand(KeyCode.V, Paste),
                new KeyCodeMap.KeyCommand(KeyCode.D, Duplicate),
                new KeyCodeMap.KeyCommand(KeyCode.X, Cut),
                new KeyCodeMap.KeyCommand(KeyCode.A, SelectAll)))
            {
                return true;
            }

            key.InvokeFirstCommand(
                new KeyCodeMap.KeyCommand(KeyCode.Escape, ResetText),
                    new KeyCodeMap.KeyCommand(KeyCode.Return, NextLineOrDeselect),
                    new KeyCodeMap.KeyCommand(KeyCode.Backspace, RemoveBackward),
                    new KeyCodeMap.KeyCommand(KeyCode.Delete, RemoveForward),
                    new KeyCodeMap.KeyCommand(KeyCode.Home, () => { setSelection(0); }),
                    new KeyCodeMap.KeyCommand(KeyCode.End, () => { setSelection(TextLength); }),
                    new KeyCodeMap.KeyCommand(KeyCode.ArrowRight, () => { MoveRight(shftDown, ctrDown); }),
                    new KeyCodeMap.KeyCommand(KeyCode.ArrowLeft, () => { MoveLeft(shftDown, ctrDown); }),
                    new KeyCodeMap.KeyCommand(KeyCode.ArrowUp, () => { MoveUp(shftDown, ctrDown); }),
                    new KeyCodeMap.KeyCommand(KeyCode.ArrowDown, () => { MoveDown(shftDown, ctrDown); })
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