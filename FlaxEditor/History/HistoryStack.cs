using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.Collections;

namespace FlaxEditor.History
{
    /// <summary>
    /// Controller for handling stack manipulations in history and reverse buffers.
    /// </summary>
    public sealed class HistoryStack
    {
        private int _historyActionsLimit;

        private CircularBuffer<IHistoryAction> _historyActions { get; }
        private CircularBuffer<IHistoryAction> _reverseActions { get; set; }

        public HistoryStack(int historyActionsLimit = 1000)
        {
            _historyActionsLimit = historyActionsLimit;
            _historyActions = new CircularBuffer<IHistoryAction>(_historyActionsLimit);
            _reverseActions = new CircularBuffer<IHistoryAction>(_historyActionsLimit);
        }

        public int HistoryCount => _historyActions.Count;
        public int ReverseCount => _reverseActions.Count;

        /// <summary>
        /// Adds new history element at top of history stack, and drops reverse stack
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Push(IHistoryAction item)
        {
            _historyActions.PushFront(item);
            if(_reverseActions.Count > 0)
            {
                _reverseActions.Clear();
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
            _historyActions.PopFront();
            _reverseActions.PushFront(item);
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
            _reverseActions.PopFront();
            _historyActions.PushFront(item);
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
                throw new ArgumentOutOfRangeException(nameof(skipElements), "argument cannot be smaller or equal to 0");
            }
            if (_historyActions.Count - skipElements <= 0)
            {
                foreach (var historyAction in _historyActions)
                {
                    _reverseActions.PushFront(historyAction);
                }
                var result = _historyActions.Back();
                _historyActions.Clear();
                return result;
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
                throw new ArgumentOutOfRangeException(nameof(skipElements), "skipElement cannot be smaller or equal to 0");
            }
            if (_reverseActions.Count - skipElements <= 0)
            {
                foreach (var reverseAction in _reverseActions.Reverse())
                {
                    _historyActions.PushFront(reverseAction);
                }
                _reverseActions.Clear();
                return PeekHistory();
            }
            // iterate all but one elements to skip. Last element is handled exclusivly
            for (int i = 0; i < skipElements - 1; i++)
            {
                PopReverse();
            }
            return PopReverse();
        }
    }
}