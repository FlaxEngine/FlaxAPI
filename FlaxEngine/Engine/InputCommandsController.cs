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

        public bool IsWaitingForNextChord { get; private set; }
        public bool IsFirstChord => CurrentChordStack.Count == 0;

        private HashSet<InputCommand> _commands = new HashSet<InputCommand>();

        private InputChord _lastInputChord;
        private DateTime _totalSingleChordInput;
        private int _totalSingleChordInputs;

        public void Execute(InputChord currentInput)
        {
            if(_lastInputChord != currentInput)
            {
                _lastInputChord = currentInput;
                _totalSingleChordInput = DateTime.UtcNow;
                InternalExecute(currentInput);
            }
            else
            {
                var inputDiff = (_totalSingleChordInput - DateTime.UtcNow).Ticks;
                if (inputDiff > SECOND_CHAR_INPUT_DELAY_TICKS)
                {
                    InternalExecute(currentInput);
                    return;
                }
                var nthCalculation = (inputDiff - SECOND_CHAR_INPUT_DELAY_TICKS) - (_totalSingleChordInputs * N_TH_CHAR_INPUT_DELAY_TICKS);
                if(nthCalculation >= 0 && nthCalculation <= N_TH_CHAR_INPUT_DELAY_TICKS)
                {
                    InternalExecute(currentInput);
                    _totalSingleChordInputs++;
                }
                else if(nthCalculation > N_TH_CHAR_INPUT_DELAY_TICKS)
                {
                    CurrentChordStack.Clear();
                    IsWaitingForNextChord = true;
                    CurrentChordStack.Add(currentInput);

                }
            }
        }

        private void InternalExecute(InputChord currentInput)
        {
            if (IsCommandChord(currentInput))
            {
                AddCurrentInputToStack(currentInput)?.Execute();
            }
        }

        private bool IsCommandChord(InputChord currentInput)
        {
            if (IsFirstChord)
            {
                if (currentInput.IsControl() || currentInput.IsControlShift() || currentInput.IsControlAltShift() || !currentInput.IsAlphaNumeric())
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
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