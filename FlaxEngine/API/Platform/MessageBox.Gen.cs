// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a dialog box.
    /// </summary>
    [Tooltip("Specifies identifiers to indicate the return value of a dialog box.")]
    public enum DialogResult
    {
        /// <summary>
        /// The abort.
        /// </summary>
        [Tooltip("The abort.")]
        Abort = 0,

        /// <summary>
        /// The cancel.
        /// </summary>
        [Tooltip("The cancel.")]
        Cancel,

        /// <summary>
        /// The ignore.
        /// </summary>
        [Tooltip("The ignore.")]
        Ignore,

        /// <summary>
        /// The no.
        /// </summary>
        [Tooltip("The no.")]
        No,

        /// <summary>
        /// The none.
        /// </summary>
        [Tooltip("The none.")]
        None,

        /// <summary>
        /// The ok.
        /// </summary>
        [Tooltip("The ok.")]
        OK,

        /// <summary>
        /// The retry.
        /// </summary>
        [Tooltip("The retry.")]
        Retry,

        /// <summary>
        /// The yes.
        /// </summary>
        [Tooltip("The yes.")]
        Yes,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Specifies constants defining which information to display.
    /// </summary>
    [Tooltip("Specifies constants defining which information to display.")]
    public enum MessageBoxIcon
    {
        /// <summary>
        /// Asterisk
        /// </summary>
        [Tooltip("Asterisk")]
        Asterisk,

        /// <summary>
        /// Error
        /// </summary>
        [Tooltip("Error")]
        Error,

        /// <summary>
        /// Exclamation
        /// </summary>
        [Tooltip("Exclamation")]
        Exclamation,

        /// <summary>
        /// Hand
        /// </summary>
        [Tooltip("Hand")]
        Hand,

        /// <summary>
        /// Information
        /// </summary>
        [Tooltip("Information")]
        Information,

        /// <summary>
        /// None
        /// </summary>
        [Tooltip("None")]
        None,

        /// <summary>
        /// Question
        /// </summary>
        [Tooltip("Question")]
        Question,

        /// <summary>
        /// Stop
        /// </summary>
        [Tooltip("Stop")]
        Stop,

        /// <summary>
        /// Warning
        /// </summary>
        [Tooltip("Warning")]
        Warning,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Specifies constants defining which buttons to display on a Message Box.
    /// </summary>
    [Tooltip("Specifies constants defining which buttons to display on a Message Box.")]
    public enum MessageBoxButtons
    {
        /// <summary>
        /// Abort, Retry, Ignore
        /// </summary>
        [Tooltip("Abort, Retry, Ignore")]
        AbortRetryIgnore,

        /// <summary>
        /// OK
        /// </summary>
        [Tooltip("OK")]
        OK,

        /// <summary>
        /// OK, Cancel
        /// </summary>
        [Tooltip("OK, Cancel")]
        OKCancel,

        /// <summary>
        /// Retry, Cancel
        /// </summary>
        [Tooltip("Retry, Cancel")]
        RetryCancel,

        /// <summary>
        /// Yes, No
        /// </summary>
        [Tooltip("Yes, No")]
        YesNo,

        /// <summary>
        /// Yes, No, Cancel
        /// </summary>
        [Tooltip("Yes, No, Cancel")]
        YesNoCancel,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Message dialogs utility (native platform).
    /// </summary>
    [Tooltip("Message dialogs utility (native platform).")]
    public static unsafe partial class MessageBox
    {
        /// <summary>
        /// Displays a message box with specified text.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(string text)
        {
            return Internal_Show(text);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show(string text);

        /// <summary>
        /// Displays a message box with specified text and caption.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(string text, string caption)
        {
            return Internal_Show1(text, caption);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show1(string text, string caption);

        /// <summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Internal_Show2(text, caption, buttons);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show2(string text, string caption, MessageBoxButtons buttons);

        /// <summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Internal_Show3(text, caption, buttons, icon);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show3(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

        /// <summary>
        /// Displays a message box with specified text.
        /// </summary>
        /// <param name="parent">The parent window or null if not used.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(Window parent, string text)
        {
            return Internal_Show4(FlaxEngine.Object.GetUnmanagedPtr(parent), text);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show4(IntPtr parent, string text);

        /// <summary>
        /// Displays a message box with specified text and caption.
        /// </summary>
        /// <param name="parent">The parent window or null if not used.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(Window parent, string text, string caption)
        {
            return Internal_Show5(FlaxEngine.Object.GetUnmanagedPtr(parent), text, caption);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show5(IntPtr parent, string text, string caption);

        /// <summary>
        /// Displays a message box with specified text, caption and buttons.
        /// </summary>
        /// <param name="parent">The parent window or null if not used.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(Window parent, string text, string caption, MessageBoxButtons buttons)
        {
            return Internal_Show6(FlaxEngine.Object.GetUnmanagedPtr(parent), text, caption, buttons);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show6(IntPtr parent, string text, string caption, MessageBoxButtons buttons);

        /// <summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="parent">The parent window or null if not used.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        /// <returns>The message box dialog result.</returns>
        public static DialogResult Show(Window parent, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Internal_Show7(FlaxEngine.Object.GetUnmanagedPtr(parent), text, caption, buttons, icon);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DialogResult Internal_Show7(IntPtr parent, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
    }
}
