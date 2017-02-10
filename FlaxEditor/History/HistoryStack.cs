using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.History
{
    /// <summary>
    /// Stack handling operations on custom stack with revert option
    /// </summary>
    public sealed class HistoryStack
    {
        public const int BUFFER_AMOUNT_PERCENT = 10;

        private int _historyActionsLimit;
        private int _orphansAmountLimit;

        private List<IHistoryAction> _historyActions { get; } = new List<IHistoryAction>();
        private List<IHistoryAction> _reverseActions { get; set; } = new List<IHistoryAction>();
        private List<List<IHistoryAction>> _orphanReverseActions { get; } = new List<List<IHistoryAction>>();

        public HistoryStack(int historyActionsLimit = 1000, int orhpansAmountLimit = 20)
        {
            _historyActionsLimit = historyActionsLimit;
            _orphansAmountLimit = orhpansAmountLimit;
        }

        /// <summary>
        /// Adds new history element at top of history stack, and drops reverse stack
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Push(IHistoryAction item)
        {
            _historyActions.Add(item);
            if(_reverseActions.Count > 0)
            {
                _orphanReverseActions.Add(_reverseActions);
                _reverseActions = new List<IHistoryAction>();
            }
        }

        /// <summary>
        /// Gets top-most item in history stack
        /// </summary>
        /// <returns>Found element or null</returns>
        public IHistoryAction PeekHistory()
        {
            return _historyActions.Count == 0 ? null : _historyActions[_historyActions.Count - 1];
        }

        /// <summary>
        /// Gets top-most item in reverse stack
        /// </summary>
        /// <returns>Found element or null</returns>
        public IHistoryAction PeekReverse()
        {
            return _reverseActions.Count == 0 ? null : _reverseActions[_reverseActions.Count - 1];
        }

        /// <summary>
        /// Gets top-most item in history stack, and removes it from history stack. Adds forgot element in reverse stack.
        /// </summary>
        /// <returns>Found element or null</returns>
        public IHistoryAction PopHistory()
        {
            var item = PeekHistory();
            if (item == null) return null;
            _historyActions.Remove(item);
            _reverseActions.Add(item);
            return item;
        }

        /// <summary>
        /// Gets top-most item in reverse stack, and removes it from reverse stack. Adds forgot element in history stack.
        /// </summary>
        /// <returns>Found element or null</returns>
        public IHistoryAction PopReverse()
        {
            var item = PeekReverse();
            if (item == null) return null;
            _reverseActions.Remove(item);
            _historyActions.Add(item);
            return item;
        }

        /// <summary>
        /// Gets element at given index from top of history stack, and adds all skipped elements to reverse stack
        /// </summary>
        /// <remarks>If skipElements is bigger, then amount of elements in history, returns null, clears history and pushes all to reverse stack</remarks>
        /// <param name="skipElements">Amount of elements to skip from history stack</param>
        /// <returns>>Found element or null</returns>
        public IHistoryAction TravelBack(int skipElements)
        {
            if (skipElements <= 0)
            {
                throw new IndexOutOfRangeException("skipElement cannot be smaller or equal to 0");
            }
            if (_historyActions.Count - skipElements <= 0)
            {
                _reverseActions.AddRange(_historyActions);
                return null;
            }
            // iterate all but one elements to skip. Last element is handled exclusivly
            for (int i = 0; i < skipElements - 1; i++)
            {
                PopHistory();
            }
            return PopHistory();
        }

        /// <summary>
        /// Gets element at given index from top of reverse stack, and adds all skipped elements to history stack
        /// </summary>
        /// <remarks>If skipElements is bigger, then amount of elements in reverse, returns null, clears reverse and pushes all to history stack</remarks>
        /// <param name="skipElements">Amount of elements to skip from reverse stack</param>
        /// <returns>>Found element or null</returns>
        public IHistoryAction TravelReverse(int skipElements)
        {
            if (skipElements <= 0)
            {
                throw new IndexOutOfRangeException("skipElement cannot be smaller or equal to 0");
            }
            if (_reverseActions.Count - skipElements <= 0)
            {
                _historyActions.AddRange(_reverseActions);
                return PeekHistory();
            }
            // iterate all but one elements to skip. Last element is handled exclusivly
            for (int i = 0; i < skipElements - 1; i++)
            {
                PopReverse();
            }
            return PopReverse();
        }

        private void ValidateActionLimits()
        {
            
        }
    }
}