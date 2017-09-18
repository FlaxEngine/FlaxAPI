////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        private void AddCommandsToController(InputCommandsController controller)
        {
            controller.Add(new InputCommand(Copy, new InputChord(KeyCode.Control, KeyCode.C)));
            controller.Add(new InputCommand(Paste, new InputChord(KeyCode.Control, KeyCode.V)));
            controller.Add(new InputCommand(Duplicate, new InputChord(KeyCode.Control, KeyCode.D)));
            controller.Add(new InputCommand(Cut, new InputChord(KeyCode.Control, KeyCode.X)));
            controller.Add(new InputCommand(SelectAll, new InputChord(KeyCode.Control, KeyCode.A)));

            controller.Add(new InputCommand(ResetText, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(NextLineOrDeselect, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(RemoveBackward, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(RemoveForward, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(() => { setSelection(0); }, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(() => { setSelection(TextLength); }, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(() => { MoveRight(shftDown, ctrDown); }, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(() => { MoveLeft(shftDown, ctrDown); }, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(() => { MoveUp(shftDown, ctrDown); }, new InputChord(KeyCode.C)));
            controller.Add(new InputCommand(() => { MoveDown(shftDown, ctrDown); }, new InputChord(KeyCode.C)));

        }

        public InputCommandsController CommandsController { get; private set; } = new InputCommandsController();


        /// <inheritdoc />
        public override bool OnKeyPressed(InputChord key)
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
                new InputChord.KeyCommand(KeyCode.C, Copy),
                new InputChord.KeyCommand(KeyCode.V, Paste),
                new InputChord.KeyCommand(KeyCode.D, Duplicate),
                new InputChord.KeyCommand(KeyCode.X, Cut),
                new InputChord.KeyCommand(KeyCode.A, SelectAll)))
            {
                return true;
            }

            key.InvokeFirstCommand(
                new InputChord.KeyCommand(KeyCode.Escape, ResetText),
                    new InputChord.KeyCommand(KeyCode.Return, NextLineOrDeselect),
                    new InputChord.KeyCommand(KeyCode.Backspace, RemoveBackward),
                    new InputChord.KeyCommand(KeyCode.Delete, RemoveForward),
                    new InputChord.KeyCommand(KeyCode.Home, () => { setSelection(0); }),
                    new InputChord.KeyCommand(KeyCode.End, () => { setSelection(TextLength); }),
                    new InputChord.KeyCommand(KeyCode.ArrowRight, () => { MoveRight(shftDown, ctrDown); }),
                    new InputChord.KeyCommand(KeyCode.ArrowLeft, () => { MoveLeft(shftDown, ctrDown); }),
                    new InputChord.KeyCommand(KeyCode.ArrowUp, () => { MoveUp(shftDown, ctrDown); }),
                    new InputChord.KeyCommand(KeyCode.ArrowDown, () => { MoveDown(shftDown, ctrDown); })
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