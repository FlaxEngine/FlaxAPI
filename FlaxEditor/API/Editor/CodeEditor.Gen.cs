// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Types of in-build code editors.
    /// </summary>
    [Tooltip("Types of in-build code editors.")]
    public enum CodeEditorTypes
    {
        Custom,

        SystemDefault,

        VS2008,

        VS2010,

        VS2012,

        VS2013,

        VS2015,

        VS2017,

        VS2019,

        /// <summary>
        /// The count of items in the CodeEditorTypes enum.
        /// </summary>
        [Tooltip("The count of items in the CodeEditorTypes enum.")]
        MAX,
    }
}

namespace FlaxEditor
{
    /// <summary>
    /// Editor utility to managed and use different code editors. Allows to open solution and source code files.
    /// </summary>
    [Tooltip("Editor utility to managed and use different code editors. Allows to open solution and source code files.")]
    public static unsafe partial class CodeEditingManager
    {
        /// <summary>
        /// Determines whether asynchronous open action is running in a background.
        /// </summary>
        [Tooltip("Determines whether asynchronous open action is running in a background.")]
        public static bool IsAsyncOpenRunning
        {
            get { return Internal_IsAsyncOpenRunning(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAsyncOpenRunning();

        /// <summary>
        /// Opens the file. Handles async opening.
        /// </summary>
        /// <param name="editorType">The code editor type.</param>
        /// <param name="path">The file path.</param>
        /// <param name="line">The target line (use 0 to not use it).</param>
        public static void OpenFile(CodeEditorTypes editorType, string path, int line)
        {
            Internal_OpenFile(editorType, path, line);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_OpenFile(CodeEditorTypes editorType, string path, int line);

        /// <summary>
        /// Opens the solution project. Handles async opening.
        /// </summary>
        /// <param name="editorType">The code editor type.</param>
        public static void OpenSolution(CodeEditorTypes editorType)
        {
            Internal_OpenSolution(editorType);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_OpenSolution(CodeEditorTypes editorType);
    }
}
