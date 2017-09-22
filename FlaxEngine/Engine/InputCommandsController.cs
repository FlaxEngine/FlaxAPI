// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine.Assertions;

namespace FlaxEngine
{
    public class InputCommandsController : ICollection<InputCommand>
    {
        public const long SECOND_CHAR_INPUT_DELAY_TICKS = 5000000;
        public const long N_TH_CHAR_INPUT_DELAY_TICKS = 300000;

        public List<InputChord> CurrentChordStack { get; } = new List<InputChord>(3);

        private HashSet<InputCommand> _commands = new HashSet<InputCommand>();

        private InputChord _lastInputChord;
        private DateTime _totalSingleChordInput;
        private int _totalSingleChordInputs;
        private bool _isSecondExecuted;
        private bool _isFirstExecuted;

        public InputCommandsController()
        {
            Input.OnKeyReleased += KeyReleased;
        }

        public bool IsWaitingForNextChord { get; private set; }
        public bool IsFirstChord => CurrentChordStack.Count == 0;
        public bool AcceptsAlphaNumeric { get; set; } = true;
        public bool SuccessIfNotFound { get; set; }

        public bool KeyPressed(InputChord chord)
        {
            if (_lastInputChord == null || chord == null || !_lastInputChord.Equals(chord))
            {
                _isFirstExecuted = true;
                _lastInputChord = chord;
                _totalSingleChordInput = DateTime.UtcNow;
                InternalExecute(chord);
                Debug.Log("First " + IsFirstChord);
                return true;
            }
            return false;
        }

        public bool KeyHold(InputChord chord)
        {
            return Execute(chord);
        }

        private void KeyReleased(InputChord chord)
        {
            if (_isFirstExecuted)
            {
                Debug.Log("Clear");
                _isFirstExecuted = false;
                _isSecondExecuted = false;
                CurrentChordStack.Clear();
                IsWaitingForNextChord = true;
                _lastInputChord = null;
                _totalSingleChordInputs = 0;
            }
        }

        private bool Execute(InputChord currentInput)
        {
            if (_lastInputChord != null && currentInput != null && _lastInputChord.Equals(currentInput) && !currentInput.All(code => code == KeyCode.Control || code == KeyCode.Shift || code == KeyCode.Alt))
            {
                //Do not remove now variable. There seems to be error in mono while comparing to DateTime
                var now = DateTime.UtcNow.Ticks;
                var inputDiff = now - _totalSingleChordInput.Ticks;
                if (!_isSecondExecuted && inputDiff > SECOND_CHAR_INPUT_DELAY_TICKS)
                {
                    _isSecondExecuted = true;
                    InternalExecute(currentInput);
                    Debug.Log("Second " + IsFirstChord);
                    return true;
                }
                if (_isSecondExecuted)
                {
                    var nthCalculation = inputDiff - SECOND_CHAR_INPUT_DELAY_TICKS - _totalSingleChordInputs * N_TH_CHAR_INPUT_DELAY_TICKS;
                    if (nthCalculation >= 0)
                    {
                        Debug.Log("nthCalculation " + nthCalculation + " " + IsFirstChord);
                        InternalExecute(currentInput);
                        _totalSingleChordInputs++;
                    }
                }
            }
            return SuccessIfNotFound;
        }

        private void InternalExecute(InputChord currentInput)
        {
            AddCurrentInputToStack(currentInput)?.Execute();
        }


        private InputCommand AddCurrentInputToStack(InputChord currentInput)
        {
            var match = _commands.Where(command => command.Validate(CurrentChordStack.Count, currentInput)).ToList();
            if (match.Count > 1)
            {
                IsWaitingForNextChord = true;
                CurrentChordStack.Add(currentInput);
                return null;
            }
            else
            {
                IsWaitingForNextChord = false;
                CurrentChordStack.Clear();
                if (match.Count == 1)
                {
                    return match.First();
                }
            }
            return null;
        }

        /// <inheritdoc />
        public IEnumerator<InputCommand> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(InputCommand item)
        {
            if (item == null || item.KeyCombinations.Length == 0)
            {
                throw new NullReferenceException("Given item cannot be null or contains 0 keys");
            }
            _commands.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _commands.Clear();
        }

        /// <inheritdoc />
        public bool Contains(InputCommand item)
        {
            return _commands.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(InputCommand[] array, int arrayIndex)
        {
            _commands.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(InputCommand item)
        {
            return _commands.Remove(item);
        }

        /// <inheritdoc />
        public int Count => _commands.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;
    }
}