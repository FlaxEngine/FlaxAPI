// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public static partial class MessageBox
    {
        /// <summary>
        /// Specifies constants defining which information to display.
        /// </summary>
        public enum Icon
        {
            /// <summary>
            /// The asterisk icon.
            /// </summary>
            Asterisk = 0,

            /// <summary>
            /// The error icon.
            /// </summary>
            Error,

            /// <summary>
            /// The exclamation icon.
            /// </summary>
            Exclamation,

            /// <summary>
            /// The hand icon.
            /// </summary>
            Hand,

            /// <summary>
            /// The information icon.
            /// </summary>
            Information,

            /// <summary>
            /// No icon.
            /// </summary>
            None,

            /// <summary>
            /// The question icon.
            /// </summary>
            Question,

            /// <summary>
            /// The stop sign icon.
            /// </summary>
            Stop,

            /// <summary>
            /// The warning icon.
            /// </summary>
            Warning
        };

        /// <summary>
        /// Specifies constants defining which buttons to display on a Message Box.
        /// </summary>
        public enum Buttons
        {
            /// <summary>
            /// Abort, Retry and Ignore buttons
            /// </summary>
            AbortRetryIgnore = 0,

            /// <summary>
            /// OK button
            /// </summary>
            OK,

            /// <summary>
            /// OK and Cancel buttons
            /// </summary>
            OKCancel,

            /// <summary>
            /// Retry and Cancel buttons
            /// </summary>
            RetryCancel,

            /// <summary>
            /// Yes and No buttons
            /// </summary>
            YesNo,

            /// <summary>
            /// Yes, No and Cancel buttons
            /// </summary>
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
        /// <param name="parent">Parent window or null if not used</param>
        /// <param name="text">The text to display in the message box</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(Window parent, string text)
        {
            return Show(parent, text, "Info", Buttons.OK, Icon.None);
        }

        /// <summary>
        /// Displays a message box with specified text and caption
        /// </summary>
        /// <param name="parent">Parent window or null if not used</param>
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
        /// <param name="parent">Parent window or null if not used</param>
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
