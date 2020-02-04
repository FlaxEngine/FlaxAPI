// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// The platform the game is running. Can be accessed via <see cref="Platform.PlatformType"/>.
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// Running on Windows.
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

        /// <summary>
        /// Running on Linux system.
        /// </summary>
        Linux = 4,

        /// <summary>
        /// Running on PlayStation 4.
        /// </summary>
        PS4 = 5,
    }
}
