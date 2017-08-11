////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.History
{
    /// <summary>
    /// Interface for <see cref="HistoryStack"/> actions.
    /// </summary>
    public interface IHistoryAction
    {
        /// <summary>
        /// Name or key of performed action
        /// </summary>
        string ActionString { get; }
    }
}
