// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    partial class Input
    {
        /// <summary>
        /// Event fired when virtual input action is triggered. Called before scripts update. See <see cref="ActionMappings"/> to edit configuration.
        /// </summary>
        /// <seealso cref="InputEvent"/>
        public static event Action<string> ActionTriggered;

        internal static void Internal_ActionTriggered(string name)
        {
            ActionTriggered?.Invoke(name);
        }

        /// <summary>
        /// The gamepads changed event. Called when new gamepad device gets disconnected or added. Can be called always on main thread before the scripts update.
        /// </summary>
        public static event Action GamepadsChanged;

        internal static void Internal_GamepadsChanged()
        {
            GamepadsChanged?.Invoke();
        }
    }
}
