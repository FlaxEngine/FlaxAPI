// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Platform implementation of filesystem service.
    /// </summary>
    [Tooltip("Platform implementation of filesystem service.")]
    public static unsafe partial class FileSystem
    {
        /// <summary>
        /// Displays a standard dialog box that prompts the user to open a file(s).
        /// </summary>
        /// <param name="parentWindow">The parent window or null.</param>
        /// <param name="initialDirectory">The initial directory.</param>
        /// <param name="filter">The custom filter.</param>
        /// <param name="multiSelect">True if allow multiple files to be selected, otherwise use single-file mode.</param>
        /// <param name="title">The dialog title.</param>
        /// <param name="filenames">The output names of the files picked by the user.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ShowOpenFileDialog(Window parentWindow, string initialDirectory, string filter, bool multiSelect, string title, out string[] filenames)
        {
            return Internal_ShowOpenFileDialog(FlaxEngine.Object.GetUnmanagedPtr(parentWindow), initialDirectory, filter, multiSelect, title, out filenames);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ShowOpenFileDialog(IntPtr parentWindow, string initialDirectory, string filter, bool multiSelect, string title, out string[] filenames);

        /// <summary>
        /// Displays a standard dialog box that prompts the user to save a file(s).
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="initialDirectory">The initial directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="multiSelect">True if allow multiple files to be selected, otherwise use single-file mode.</param>
        /// <param name="title">The title.</param>
        /// <param name="filenames">The output names of the files picked by the user.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ShowSaveFileDialog(Window parentWindow, string initialDirectory, string filter, bool multiSelect, string title, out string[] filenames)
        {
            return Internal_ShowSaveFileDialog(FlaxEngine.Object.GetUnmanagedPtr(parentWindow), initialDirectory, filter, multiSelect, title, out filenames);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ShowSaveFileDialog(IntPtr parentWindow, string initialDirectory, string filter, bool multiSelect, string title, out string[] filenames);

        /// <summary>
        /// Displays a standard dialog box that prompts the user to select a folder.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="initialDirectory">The initial directory.</param>
        /// <param name="title">The dialog title.</param>
        /// <param name="path">The output path.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ShowBrowseFolderDialog(Window parentWindow, string initialDirectory, string title, out string path)
        {
            return Internal_ShowBrowseFolderDialog(FlaxEngine.Object.GetUnmanagedPtr(parentWindow), initialDirectory, title, out path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ShowBrowseFolderDialog(IntPtr parentWindow, string initialDirectory, string title, out string path);

        /// <summary>
        /// Opens a standard file explorer application and navigates to the given directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ShowFileExplorer(string path)
        {
            return Internal_ShowFileExplorer(path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ShowFileExplorer(string path);
    }
}
