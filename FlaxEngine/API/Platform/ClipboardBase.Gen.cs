// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Native platform clipboard service.
    /// </summary>
    [Tooltip("Native platform clipboard service.")]
    public static unsafe partial class Clipboard
    {
        /// <summary>
        /// Sets text to the clipboard.
        /// </summary>
        [Tooltip("Sets text to the clipboard.")]
        public static string Text
        {
            get { return Internal_GetText(); }
            set { Internal_SetText(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetText();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetText(string text);

        /// <summary>
        /// Sets the raw bytes data to the clipboard.
        /// </summary>
        [Tooltip("Sets the raw bytes data to the clipboard.")]
        public static byte[] RawData
        {
            get { return Internal_GetRawData(); }
            set { Internal_SetRawData(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_GetRawData();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRawData(byte[] data);

        /// <summary>
        /// Sets the files to the clipboard.
        /// </summary>
        [Tooltip("Sets the files to the clipboard.")]
        public static string[] Files
        {
            get { return Internal_GetFiles(); }
            set { Internal_SetFiles(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string[] Internal_GetFiles();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFiles(string[] files);

        /// <summary>
        /// Clear the clipboard contents.
        /// </summary>
        public static void Clear()
        {
            Internal_Clear();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Clear();
    }
}
