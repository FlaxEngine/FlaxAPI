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
    /// <summary>
    ///     #IMPORTANT SEE CONSTRUCTOR NOTES
    ///     <para>Input controller for handling complex input chords and simple character input</para>
    /// </summary>
    public class InputCommandsController : ICollection<InputCommand>
    {
        /// <summary>
        ///     System windows defitnion for key hold handling of second currentInput enter
        /// </summary>
        public const long SECOND_CHAR_INPUT_DELAY_TICKS = 5000000;

        /// <summary>
        ///     System windows defitnion for key hold handling of nth after second currentInput enter
        /// </summary>
        public const long N_TH_CHAR_INPUT_DELAY_TICKS = 300000;

        /// <summary>
        ///     Currently waiting list of previously entered <see cref="InputChord" />
        /// </summary>
        public List<InputChord> CurrentChordStack { get; } = new List<InputChord>(3);

        /// <summary>
        ///     Current list of all commands ready to be validated
        /// </summary>
        private HashSet<InputCommand> _commands = new HashSet<InputCommand>();

        /// <summary>
        ///     Last entered <see cref="InputChord" />
        /// </summary>
        private InputChord _lastInputChord;

        /// <summary>
        ///     <see cref="DateTime" /> of first input currentInput enter. Used for continous input
        /// </summary>
        private DateTime _firstChordInputTime;

        /// <summary>
        ///     Counter for until this point repeated execution of given currentInput
        /// </summary>
        private int _totalSingleChordInputs;

        /// <summary>
        ///     Is first <see cref="InputChord" /> validated and pressed
        /// </summary>
        /// <seealso cref="_isSecondExecuted" />
        private bool _isFirstExecuted;

        /// <summary>
        ///     Is second <see cref="InputChord" /> validated and pressed, used for in row break
        /// </summary>
        /// <seealso cref="_isFirstExecuted" />
        private bool _isSecondExecuted;

        /// <summary>
        ///     In order for this class to work correctly invoke <see cref="KeyPressed" /> while event
        ///     <see cref="Input.OnKeyPressed" /> fires with your custom input prevention logic
        ///     also invoke <see cref="KeyHold" /> while event <see cref="Input.OnKeyHold" /> fires with your custom input
        ///     prevention logic matching above prevention logic.
        ///     <para>Default constuctor.</para>
        /// </summary>
        public InputCommandsController()
        {
            Input.OnKeyReleased += KeyReleased;
        }

        /// <summary>
        ///     Is <see cref="InputCommandsController" /> waiting for more chords to be enter in order to validate
        /// </summary>
        public bool IsWaitingForNextChord { get; private set; }

        /// <summary>
        ///     InvokeUnconditionally manualy when your control will have key pressed using <see cref="Input.OnKeyPressed" />
        /// </summary>
        /// <param name="currentInput"></param>
        /// <returns>True if command was found and executed</returns>
        public bool KeyPressed(InputChord currentInput)
        {
            if (_lastInputChord == null || currentInput == null || !_lastInputChord.Equals(currentInput))
            {
                _isFirstExecuted = true;
                _lastInputChord = currentInput;
                _firstChordInputTime = DateTime.UtcNow;
                InternalExecute(currentInput);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     InvokeUnconditionally manualy when your control will have key hold using <see cref="Input.OnKeyHold" />
        /// </summary>
        /// <param name="currentInput"></param>
        /// <returns>True if command was found and executed</returns>
        public bool KeyHold(InputChord currentInput)
        {
            if (_lastInputChord != null && currentInput != null && _lastInputChord.Equals(currentInput) && !currentInput.All(code => code == KeyCode.Control || code == KeyCode.Shift || code == KeyCode.Alt))
            {
                //Do not remove now variable. There seems to be error in mono while comparing to DateTime
                var now = DateTime.UtcNow.Ticks;
                var inputDiff = now - _firstChordInputTime.Ticks;
                if (!_isSecondExecuted && inputDiff > SECOND_CHAR_INPUT_DELAY_TICKS)
                {
                    _isSecondExecuted = true;
                    InternalExecute(currentInput);
                    return true;
                }
                if (_isSecondExecuted)
                {
                    var nthCalculation = inputDiff - SECOND_CHAR_INPUT_DELAY_TICKS - _totalSingleChordInputs * N_TH_CHAR_INPUT_DELAY_TICKS;
                    if (nthCalculation >= 0)
                    {
                        InternalExecute(currentInput);
                        _totalSingleChordInputs++;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///     Invokes automaticly when <see cref="Input.OnKeyReleased" /> is pressed
        /// </summary>
        /// <param name="currentInput"></param>
        private void KeyReleased(InputChord currentInput)
        {
            if (_isFirstExecuted)
            {
                _isFirstExecuted = false;
                _isSecondExecuted = false;
                CurrentChordStack.Clear();
                IsWaitingForNextChord = true;
                _lastInputChord = null;
                _totalSingleChordInputs = 0;
            }
        }

        /// <summary>
        ///     Invokes found method.
        /// </summary>
        /// <param name="currentInput"></param>
        private void InternalExecute(InputChord currentInput)
        {
            FindAndInvoke(currentInput)?.Invoke();
        }

        /// <summary>
        ///     Validates if given input exists on stack and execute it
        /// </summary>
        /// <param name="currentInput"></param>
        /// <returns>Delegate to invoke or null if not found</returns>
        private InputCommand FindAndInvoke(InputChord currentInput)
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