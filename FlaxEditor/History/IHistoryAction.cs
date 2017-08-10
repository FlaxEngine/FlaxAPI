////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor.History
{
    /// <summary>
    /// Interface for <see cref="HistoryStack"/> actions.
    /// </summary>
    public interface IHistoryAction
    {
        /// <summary>
        /// Id of made action
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name or key of performed action
        /// </summary>
        String ActionString { get; }
    }
}
