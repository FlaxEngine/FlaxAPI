// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    partial class Audio
    {
        /// <summary>
        /// The audio devices collection changed event.
        /// </summary>
        public static event Action DevicesChanged;

        internal static void Internal_DevicesChanged()
        {
            DevicesChanged?.Invoke();
        }
    }
}
