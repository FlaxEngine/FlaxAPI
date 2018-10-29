// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEditor.Scripting
{
    /// <summary>
    /// Game scrips building service. Compiles user C# scripts into binary assemblies.
    /// Exposes many events used to track scripts compilation and reloading.
    /// </summary>
    public static partial class ScriptsBuilder
    {
        // TODO: expose api to inject custom defines to compilation and more customizations

        internal enum InBuildEditorTypes
        {
            Custom,
            Text,
            VS2008,
            VS2010,
            VS2012,
            VS2013,
            VS2015,
            VS2017,

            MAX
        };

        /// <summary>
        /// Compilation end event delegate.
        /// </summary>
        /// <param name="success">False if compilation has failed, otherwise true.</param>
        public delegate void CompilationEndDelegate(bool success);

        /// <summary>
        /// Compilation message events delegate.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="file">The target file.</param>
        /// <param name="line">The target line.</param>
        public delegate void CompilationMessageDelegate(string message, string file, int line);

        /// <summary>
        /// Occurs when compilation ends.
        /// </summary>
        public static event CompilationEndDelegate CompilationEnd;

        /// <summary>
        /// Occurs when compilation success.
        /// </summary>
        public static event Action CompilationSuccess;

        /// <summary>
        /// Occurs when compilation failed.
        /// </summary>
        public static event Action CompilationFailed;

        /// <summary>
        /// Occurs when compilation begins.
        /// </summary>
        public static event Action CompilationBegin;

        /// <summary>
        /// Occurs when compilation just started.
        /// </summary>
        public static event Action CompilationStarted;

        /// <summary>
        /// Occurs when user scripts reload action is called.
        /// </summary>
        public static event Action ScriptsReloadCalled;

        /// <summary>
        /// Occurs when user scripts reload starts.
        /// User objects should be removed at this point to reduce leaks and issues. Game scripts and game editor scripts assemblies will be reloaded.
        /// </summary>
        public static event Action ScriptsReloadBegin;

        /// <summary>
        /// Occurs when user scripts reload is performed (just before the actual reload, scenes are serialized and unloaded). All user objects should be cleanup.
        /// </summary>
        public static event Action ScriptsReload;

        /// <summary>
        /// Occurs when user scripts reload ends.
        /// </summary>
        public static event Action ScriptsReloadEnd;

        /// <summary>
        /// Occurs on compilation error.
        /// </summary>
        public static event CompilationMessageDelegate CompilationError;

        /// <summary>
        /// Occurs on compilation warning.
        /// </summary>
        public static event CompilationMessageDelegate CompilationWarning;

        /// <summary>
        /// Occurs when code editor starts asynchronous open a file or a solution.
        /// </summary>
        public static event Action CodeEditorAsyncOpenBegin;

        /// <summary>
        /// Occurs when code editor ends asynchronous open a file or a solution.
        /// </summary>
        public static event Action CodeEditorAsyncOpenEnd;

        /// <summary>
        /// Checks if need to compile source code. If so calls compilation.
        /// </summary>
        public static void CheckForCompile()
        {
            if (IsSourceDirty)
                Compile();
        }

        internal enum EventType
        {
            CompileBegin = 0,
            CompileStarted = 1,
            CompileEndGood = 2,
            CompileEndFailed = 3,
            ReloadCalled = 4,
            ReloadBegin = 5,
            Reload = 6,
            ReloadEnd = 7,
        }

        internal static void Internal_OnEvent(EventType type)
        {
            switch (type)
            {
            case EventType.CompileBegin:
                CompilationBegin?.Invoke();
                break;
            case EventType.CompileStarted:
                CompilationStarted?.Invoke();
                break;
            case EventType.CompileEndGood:
                CompilationEnd?.Invoke(true);
                CompilationSuccess?.Invoke();
                break;
            case EventType.CompileEndFailed:
                CompilationEnd?.Invoke(false);
                CompilationFailed?.Invoke();
                break;
            case EventType.ReloadCalled:
                ScriptsReloadCalled?.Invoke();
                break;
            case EventType.ReloadBegin:
                ScriptsReloadBegin?.Invoke();
                break;
            case EventType.Reload:
                ScriptsReload?.Invoke();
                break;
            case EventType.ReloadEnd:
                ScriptsReloadEnd?.Invoke();
                break;
            }
        }

        internal static void Internal_OnCompileEvent(string message, string file, int line, bool isError)
        {
            if (isError)
                CompilationError?.Invoke(message, file, line);
            else
                CompilationWarning?.Invoke(message, file, line);
        }

        internal static void Internal_OnCodeEditorEvent(bool isEnd)
        {
            if (isEnd)
                CodeEditorAsyncOpenEnd?.Invoke();
            else
                CodeEditorAsyncOpenBegin?.Invoke();
        }

        internal enum ApiEngineType
        {
            Engine = 0,
            Editor = 1,
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Internal_GetExistingEditors(byte* resultArray);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_OpenSolution(InBuildEditorTypes editorType);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_OpenFile(InBuildEditorTypes editorType, string path, int line);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GenerateApi(ApiEngineType type);
#endif

        #endregion
    }
}
