using System;

namespace FlaxEngine
{
    public class InputCommand
    {
        public InputChord[] KeyCombinations { get; }
        public Action Action { get; }

        public InputCommand(Action action, params InputChord[] keyCombinations)
        {
            Action = action;
            KeyCombinations = keyCombinations;
        }

        public virtual bool Validate(int combinationIndex, InputChord map)
        {
            return KeyCombinations[combinationIndex].Equals(map);
        }

        public virtual void Execute()
        {
            Action?.Invoke();
        }
    }
}