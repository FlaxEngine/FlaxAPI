////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    public static partial class MessageBox
    {
        /// <summary>
        /// Specifies constants defining which information to display.
        /// </summary>
        public enum Icon
        {
            Asterisk = 0,
            Error,
            Exclamation,
            Hand,
            Information,
            None,
            Question,
            Stop,
            Warning
        };

        /// <summary>
        /// Specifies constants defining which buttons to display on a Message Box.
        /// </summary>
        public enum Buttons
        {
            AbortRetryIgnore = 0,
            OK,
            OKCancel,
            RetryCancel,
            YesNo,
            YesNoCancel
        };

        /// <summary>
        /// Displays a message box with specified text
        /// </summary>
        /// <param name="text">The text to display in the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(string text)
        {
            return Show(null, text, "Info", Buttons.OK, Icon.None);
        }

        /// <summary>
        /// Displays a message box with specified text and caption
        /// </summary>
        /// <param name="text">The text to display in the message box</param>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(string text, string caption)
        {
            return Show(null, text, caption, Buttons.OK, Icon.None);
        }

        /// <summary>
        /// Displays a message box with specified text, caption, buttons, and icon
        /// </summary>
        /// <param name="text">The text to display in the message box</param>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="buttons">One of the Buttons values that specifies which buttons to display in the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(string text, string caption, Buttons buttons)
        {
            return Show(null, text, caption, buttons, Icon.None);
        }

        /// <summary>
        /// Displays a message box with specified text, caption, buttons, and icon
        /// </summary>
        /// <param name="text">The text to display in the message box</param>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="buttons">One of the Buttons values that specifies which buttons to display in the message box</param>
        /// <param name="icon">One of the Icon values that specifies which icon to display in the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(string text, string caption, Buttons buttons, Icon icon)
        {
            return Show(null, text, caption, buttons, icon);
        }

        /// <summary>
        /// Displays a message box with specified text
        /// </summary>
        /// <param name="parent">Parant window or null if not used</param>
        /// <param name="text">The text to display in the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(Window parent, string text)
        {
            return Show(parent, text, "Info", Buttons.OK, Icon.None);
        }

        /// <summary>
        /// Displays a message box with specified text and caption
        /// </summary>
        /// <param name="parent">Parant window or null if not used</param>
        /// <param name="text">The text to display in the message box</param>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(Window parent, string text, string caption)
        {
            return Show(parent, text, caption, Buttons.OK, Icon.None);
        }

        /// <summary>
        /// Displays a message box with specified text, caption and buttons
        /// </summary>
        /// <param name="parent">Parant window or null if not used</param>
        /// <param name="text">The text to display in the message box</param>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="buttons">One of the Buttons values that specifies which buttons to display in the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(Window parent, string text, string caption, Buttons buttons)
        {
            return Show(parent, text, caption, buttons, Icon.None);
        }
    }
}
