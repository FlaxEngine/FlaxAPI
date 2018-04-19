// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// The platform the game is running. Can be accessed via <see cref="Application.Platform"/>.
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// Running on Windows (standalone or editor).
        /// </summary>
        Windows = 1,

        /// <summary>
        /// Running on Xbox One.
        /// </summary>
        XboxOne = 2,

        /// <summary>
        /// Running Windows Store App (Universal Windows Platform).
        /// </summary>
        WindowsStore = 3,
    }
}
