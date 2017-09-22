using System;

namespace FlaxEngine
{
    /// <summary>
    ///     Wrapper for <see cref="InputChord" /> with matching <see cref="Action" />
    /// </summary>
    public class InputCommand
    {
        /// <summary>
        ///     Key combination that should invoke an action on <see cref="ValidateAndInvoke" />
        /// </summary>
        public InputChord[] KeyCombinations { get; }

        /// <summary>
        ///     Action to be invoked with given InputChords
        /// </summary>
        public Action Action { get; }

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="action">Action to be invoked with given InputChords</param>
        /// <param name="keyCombinations">Key combination that should invoke an action on <see cref="ValidateAndInvoke" /></param>
        public InputCommand(Action action, params InputChord[] keyCombinations)
        {
            Action = action;
            KeyCombinations = keyCombinations;
        }

        /// <summary>
        ///     Validates if action should be executed based on input and combinationIndex
        /// </summary>
        /// <param name="combinationIndex">Current combination to be validated. KeyCombinations[combinationIndex]</param>
        /// <param name="currentInput">Currently inputed <see cref="InputChord" /></param>
        /// <returns></returns>
        public virtual bool Validate(int combinationIndex, InputChord currentInput)
        {
            if (combinationIndex != KeyCombinations.Length - 1)
            {
                return false;
            }
            return KeyCombinations[combinationIndex].Equals(currentInput);
        }

        /// <summary>
        ///     Invokes action unconditioanlly
        /// </summary>
        public virtual void Invoke()
        {
            Action?.Invoke();
        }

        /// <summary>
        ///     Validates input for given array of chords
        /// </summary>
        public virtual void ValidateAndInvoke(InputChord[] currentInput)
        {
            for (int i = 0; i < currentInput.Length; i++)
            {
                if (!Validate(i, currentInput[i]))
                {
                    return;
                }
            }
            Invoke();
        }
    }
}