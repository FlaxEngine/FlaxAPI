////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        protected virtual void AddCommandsToController(InputCommandsController controller)
        {
            controller.Add(new InputCommand(Copy, new InputChord(KeyCode.Control, KeyCode.C)));
            controller.Add(new InputCommand(Paste, new InputChord(KeyCode.Control, KeyCode.V)));
            controller.Add(new InputCommand(Duplicate, new InputChord(KeyCode.Control, KeyCode.D)));
            controller.Add(new InputCommand(Cut, new InputChord(KeyCode.Control, KeyCode.X)));
            controller.Add(new InputCommand(SelectAll, new InputChord(KeyCode.Control, KeyCode.A)));

            controller.Add(new InputCommand(ResetText, new InputChord(KeyCode.Escape)));
            controller.Add(new InputCommand(NextLineOrDeselect, new InputChord(KeyCode.Return)));
            controller.Add(new InputCommand(RemoveBackward, new InputChord(KeyCode.Backspace)));
            controller.Add(new InputCommand(RemoveForward, new InputChord(KeyCode.Delete)));

            controller.Add(new InputCommand(MoveSelectorToLineStart, new InputChord(KeyCode.Home)));
            controller.Add(new InputCommand(MoveSelectorToLineEnd, new InputChord(KeyCode.End)));

            controller.Add(new InputCommand(MoveSelectorRight, new InputChord(KeyCode.ArrowRight)));
            controller.Add(new InputCommand(MoveSelectorLeft, new InputChord(KeyCode.ArrowLeft)));
            controller.Add(new InputCommand(MoveSelectorUp, new InputChord(KeyCode.ArrowUp)));
            controller.Add(new InputCommand(MoveSelectorDown, new InputChord(KeyCode.ArrowDown)));

            controller.Add(new InputCommand(ExtendSelectionRight, new InputChord(KeyCode.Shift, KeyCode.ArrowRight)));
            controller.Add(new InputCommand(ExtendSelectionLeft, new InputChord(KeyCode.Shift, KeyCode.ArrowLeft)));
            controller.Add(new InputCommand(ExtendSelectionUp, new InputChord(KeyCode.Shift, KeyCode.ArrowUp)));
            controller.Add(new InputCommand(ExtendSelectionDown, new InputChord(KeyCode.Shift, KeyCode.ArrowDown)));

            controller.Add(new InputCommand(JumpToNextWord, new InputChord(KeyCode.Control, KeyCode.ArrowRight)));
            controller.Add(new InputCommand(JumpToPreviousWord, new InputChord(KeyCode.Control, KeyCode.ArrowLeft)));
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

            return CommandsController.Execute(key);
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