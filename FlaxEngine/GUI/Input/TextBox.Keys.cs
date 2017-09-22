////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    public partial class TextBox
    {
        /// <inheritdoc />
        protected void AddCommandsToController()
        {
            CommandsController.Add(new InputCommand(Copy, new InputChord(KeyCode.Control, KeyCode.C)));
            CommandsController.Add(new InputCommand(Paste, new InputChord(KeyCode.Control, KeyCode.V)));
            CommandsController.Add(new InputCommand(Duplicate, new InputChord(KeyCode.Control, KeyCode.D)));
            CommandsController.Add(new InputCommand(Cut, new InputChord(KeyCode.Control, KeyCode.X)));
            CommandsController.Add(new InputCommand(SelectAll, new InputChord(KeyCode.Control, KeyCode.A)));

            CommandsController.Add(new InputCommand(ResetText, new InputChord(KeyCode.Escape)));
            CommandsController.Add(new InputCommand(NextLineOrDeselect, new InputChord(KeyCode.Return)));
            CommandsController.Add(new InputCommand(RemoveBackward, new InputChord(KeyCode.Backspace)));
            CommandsController.Add(new InputCommand(RemoveForward, new InputChord(KeyCode.Delete)));

            CommandsController.Add(new InputCommand(MoveSelectorToLineStart, new InputChord(KeyCode.Home)));
            CommandsController.Add(new InputCommand(MoveSelectorToLineEnd, new InputChord(KeyCode.End)));

            CommandsController.Add(new InputCommand(MoveSelectorRight, new InputChord(KeyCode.ArrowRight)));
            CommandsController.Add(new InputCommand(MoveSelectorLeft, new InputChord(KeyCode.ArrowLeft)));
            CommandsController.Add(new InputCommand(MoveSelectorUp, new InputChord(KeyCode.ArrowUp)));
            CommandsController.Add(new InputCommand(MoveSelectorDown, new InputChord(KeyCode.ArrowDown)));

            CommandsController.Add(new InputCommand(ExtendSelectionRight, new InputChord(KeyCode.Shift, KeyCode.ArrowRight)));
            CommandsController.Add(new InputCommand(ExtendSelectionLeft, new InputChord(KeyCode.Shift, KeyCode.ArrowLeft)));
            CommandsController.Add(new InputCommand(ExtendSelectionUp, new InputChord(KeyCode.Shift, KeyCode.ArrowUp)));
            CommandsController.Add(new InputCommand(ExtendSelectionDown, new InputChord(KeyCode.Shift, KeyCode.ArrowDown)));

            CommandsController.Add(new InputCommand(JumpToNextWord, new InputChord(KeyCode.Control, KeyCode.ArrowRight)));
            CommandsController.Add(new InputCommand(JumpToPreviousWord, new InputChord(KeyCode.Control, KeyCode.ArrowLeft)));
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